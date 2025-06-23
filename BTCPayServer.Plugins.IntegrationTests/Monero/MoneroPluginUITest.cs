using BTCPayServer.Tests;

using Xunit;
using Xunit.Abstractions;

using BTCPayServer.Plugins.Monero.Services;
using BTCPayServer.Plugins.Monero;
using System;

using System.Threading.Tasks;


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
        helper.WriteLine("**DEBUG*** sanity 1");
        using var tester = CreateServerTester();
        helper.WriteLine("**DEBUG*** sanity 2");
        await tester.StartAsync();
        helper.WriteLine("**DEBUG*** sanity 3");
        helper.WriteLine("**DEBUG*** UI TEST BEFORE");
        var moneroRpcProvider = tester.PayTester.GetService<MoneroRPCProvider>();
        helper.WriteLine("**DEBUG*** UI TEST AFTER");

        var maxAttempts = 25;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
           try
           {
               helper.WriteLine("**DEBUG*** loooooops");
               if (moneroRpcProvider.IsAvailable("XMR"))
               {
                   helper.WriteLine("**DEBUG*** REAADY");
                   break;
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
            await Task.Delay(1000);
        }
        //////////////////////////////////
        helper.WriteLine("**DEBUG*** UI TEST 2");
        await s.Page.Locator("a.nav-link[href*='monerolike/XMR']").ClickAsync();
        await s.Page.CheckAsync("#Enabled");
        helper.WriteLine("**DEBUG** UI TEST pre label");
        //dont await it
        s.Page.Locator("#NewAccountLabel").FillAsync("Wallet Label");
        //loop refresh wait for it to appear?
        int tried = 0;
        retry:
        if (await s.Page.Locator("#NewAccountLabel").IsVisibleAsync())
            return;
        
        if (tried > 5)
        {
            await s.Page.Locator("#NewAccountLabel").WaitForAsync();
            return;
        }
        
        tried++;
        await s.Page.ReloadAsync();
        await Task.Delay(10000);
        goto retry;

        helper.WriteLine("**DEBUG** UI TEST post label");
        await s.Page.SelectOptionAsync("#SettlementConfirmationThresholdChoice", "2");
        await s.Page.ClickAsync("#SaveButton");
        var classList = await s.Page.Locator("svg.icon-checkmark").GetAttributeAsync("class");
        Assert.Contains("text-success", classList);
    }
}
