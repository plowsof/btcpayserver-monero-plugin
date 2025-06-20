using BTCPayServer.Tests;
using BTCPayServer.Plugins.Monero.Services;
using BTCPayServer.Plugins.Monero;
using Xunit.Abstractions;
using System.Threading.Tasks;
using Xunit;
using System;

namespace BTCPayServer.Plugins.IntegrationTests.Monero
{
    public class MoneroAndBitcoinIntegrationTestBase : UnitTestBase
    {
        public MoneroAndBitcoinIntegrationTestBase(ITestOutputHelper helper) : base(helper)
        {
            SetDefaultEnv("BTCPAY_XMR_DAEMON_URI", "http://127.0.0.1:18081");
            SetDefaultEnv("BTCPAY_XMR_WALLET_DAEMON_URI", "http://127.0.0.1:18082");
        }

        private static void SetDefaultEnv(string key, string defaultValue)
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(key)))
            {
                Environment.SetEnvironmentVariable(key, defaultValue);
            }
        }
        protected async Task OpenWalletMaually(string cryptoCode = "XMR")
        {
            using var tester = CreateServerTester();
            await tester.StartAsync();
            var moneroRpcProvider = tester.PayTester.GetService<MoneroRPCProvider>();
            if (moneroRpcProvider.WalletRpcClients.TryGetValue(cryptoCode.ToUpperInvariant(), out var walletClient))
            {
                await MoneroRPCProvider.CreateTestWallet(walletClient);
            }
        }
    }
}