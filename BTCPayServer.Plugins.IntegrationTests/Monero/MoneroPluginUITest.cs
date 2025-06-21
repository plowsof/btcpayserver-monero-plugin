using BTCPayServer.Tests;

using Xunit;
using Xunit.Abstractions;

using BTCPayServer.Plugins.Monero.Services;
using BTCPayServer.Plugins.Monero;
using System;



namespace BTCPayServer.Plugins.IntegrationTests.Monero;

public class MoneroPluginIntegrationTest(ITestOutputHelper helper) : MoneroAndBitcoinIntegrationTestBase(helper)
{
    [Fact]
    public async Task EnableMoneroPluginSuccessfully()
    {
        helper.WriteLine("**DEBUG*** UI TEST 1");
        await using var s = CreatePlaywrightTester();
        await s.StartAsync();
        await s.RegisterNewUser(true);
        await s.CreateNewStore();
        ////////
        using var tester = CreateServerTester();
        await tester.StartAsync();
        var moneroRpcProvider = tester.PayTester.GetService<MoneroRPCProvider>();
        var maxAttempts = 25;
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
           try
           {
               helper.WriteLine("**DEBUG*** loooooops");
               if (moneroRpcProvider.IsAvailable("XMR"))
               {
                   helper.WriteLine("**DEBUG*** REAADY");
                   return;
               }
               else
               {
                    helper.WriteLine("**DEBUG** not reADY");
               }
            }
            catch
            {
                //a
            }
            Task.Delay(1000);
        }
        //////////////////////////////////
        helper.WriteLine("**DEBUG*** UI TEST 2");
        await s.Page.Locator("a.nav-link[href*='monerolike/XMR']").ClickAsync();
        await s.Page.CheckAsync("#Enabled");
        helper.WriteLine("**DEBUG** UI TEST pre label");
        await s.Page.Locator("#NewAccountLabel").FillAsync("Wallet Label");
        helper.WriteLine("**DEBUG** UI TEST post label");
        await s.Page.SelectOptionAsync("#SettlementConfirmationThresholdChoice", "2");
        await s.Page.ClickAsync("#SaveButton");
        var classList = await s.Page.Locator("svg.icon-checkmark").GetAttributeAsync("class");
        Assert.Contains("text-success", classList);
    }
}
