using BTCPayServer.Tests;

using Xunit.Abstractions;

//https://github.com/btcpay-monero/btcpayserver-monero-plugin/blob/master/Plugins/Monero/Services/MoneroRPCProvider.cs
//moneroLikeStoreController
using BTCPayServer.Plugins.Monero.Configuration;
using BTCPayServer.Plugins.Monero.RPC;
using BTCPayServer.Plugins.Monero.RPC.Models;
using BTCPayServer.Services;
using BTCPayServer.Plugins.Monero.Services;


// private static async Task CreateTestWallet(JsonRpcClient walletRpcClient)


namespace BTCPayServer.Plugins.IntegrationTests.Monero
{
    public class MoneroAndBitcoinIntegrationTestBase : UnitTestBase
    {
        private readonly MoneroRPCProvider _moneroRpcProvider;
        public MoneroAndBitcoinIntegrationTestBase(ITestOutputHelper helper) : base(helper)
        {
            SetDefaultEnv("BTCPAY_XMR_DAEMON_URI", "http://127.0.0.1:18081");
            SetDefaultEnv("BTCPAY_XMR_WALLET_DAEMON_URI", "http://127.0.0.1:18082");
            //do we want to be using cheatmode or nah
            _moneroRpcProvider = GetService<MoneroRPCProvider>(); 
            MoneroRPCProvider.CreateTestWallet(_moneroRpcProvider.WalletRpcClients["XMR"]).GetAwaiter().GetResult();
        }

        private static void SetDefaultEnv(string key, string defaultValue)
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(key)))
            {
                Environment.SetEnvironmentVariable(key, defaultValue);
            }
        }
    }
}