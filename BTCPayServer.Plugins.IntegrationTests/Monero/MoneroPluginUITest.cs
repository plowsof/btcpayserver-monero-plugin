using BTCPayServer.Tests;

using Xunit;
using Xunit.Abstractions;

using BTCPayServer.Plugins.Monero.Services;


namespace BTCPayServer.Plugins.IntegrationTests.Monero;

public class MoneroPluginIntegrationTest(ITestOutputHelper helper) : MoneroAndBitcoinIntegrationTestBase(helper)
{
    [Fact]
    public async Task EnableMoneroPluginSuccessfully()
    {
        //surely not
        //renamed cus 'wallet exists already error :/'
        await OpenWalletManually();
        await using var s = CreatePlaywrightTester();
        await s.StartAsync();
        await s.RegisterNewUser(true);
        await s.CreateNewStore();
        await s.Page.Locator("a.nav-link[href*='monerolike/XMR']").ClickAsync();
        await s.Page.CheckAsync("#Enabled");
        await s.Page.SelectOptionAsync("#SettlementConfirmationThresholdChoice", "2");
        await s.Page.ClickAsync("#SaveButton");
        var classList = await s.Page.Locator("svg.icon-checkmark").GetAttributeAsync("class");
        Assert.Contains("text-success", classList);
    }
}