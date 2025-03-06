using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MutualFundSimulatorService.Business.ConcreteClass
{
    internal class FundDetailsUtility
    {
        /// <summary>
        /// Returns a dictionary of fund details including description, risk, and expense ratio.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, (string description, string risk, decimal expenseRatio)> GetFundDetails()
        {
            try
            {
                return new Dictionary<string, (string description, string risk, decimal expenseRatio)>
                {
                    { "Large-Cap Equity Fund", ("Invests in large-cap companies with strong market presence.", "Moderate", 0.070m) },
                    { "Mid-Cap Equity Fund", ("Invests in mid-cap companies with growth potential.", "High", 0.075m) },
                    { "Small-Cap Equity Fund", ("Targets small-cap companies with high growth.", "Very High", 0.080m) },
                    { "Sectoral/Thematic Fund", ("Focused on specific sectors like IT or Pharma.", "High", 0.072m) },
                    { "Multi-Cap Fund", ("Invests across large, mid, and small-cap stocks.", "Moderate", 0.068m) },
                    { "Overnight Fund", ("Invests in overnight securities for safety.", "Low", 0.010m) },
                    { "Liquid Fund", ("Short-term debt instruments for liquidity.", "Low", 0.015m) },
                    { "Ultra-Short Term Fund", ("Debt with short-term horizon.", "Low to Moderate", 0.020m) },
                    { "Short-Term Debt Fund", ("Debt with short maturity.", "Moderate", 0.025m) },
                    { "Low Duration Fund", ("Low duration debt instruments.", "Moderate", 0.030m) },
                    { "Nifty 50 Index Fund", ("Tracks Nifty 50 Index.", "Moderate", 0.035m) },
                    { "Sensex Index Fund", ("Tracks BSE Sensex.", "Moderate", 0.038m) },
                    { "Nifty Next 50 Index Fund", ("Tracks Nifty Next 50.", "Moderate", 0.040m) },
                    { "Nifty Bank Index Fund", ("Tracks Nifty Bank Index.", "Moderate", 0.045m) },
                    { "Nifty IT Index Fund", ("Tracks Nifty IT Index.", "Moderate", 0.050m) },
                    { "Aggressive Hybrid Fund", ("Higher equity allocation.", "High", 0.060m) },
                    { "Conservative Hybrid Fund", ("Higher debt allocation.", "Low to Moderate", 0.055m) },
                    { "Dynamic Asset Allocation Fund", ("Flexible equity-debt allocation.", "Moderate", 0.062m) },
                    { "Balanced Advantage Fund", ("Dynamic allocation based on valuations.", "Moderate", 0.065m) },
                    { "Multi-Asset Allocation Fund", ("Multiple asset classes.", "Moderate", 0.070m) },
                    { "Gold ETF Fund", ("Gold-backed assets.", "Moderate", 0.075m) },
                    { "Silver ETF Fund", ("Silver-backed securities.", "Moderate to High", 0.080m) },
                    { "Multi-Commodity Fund", ("Diversified commodities.", "Moderate", 0.082m) },
                    { "Energy Commodity Fund", ("Energy commodities like oil.", "High", 0.070m) },
                    { "Agricultural Commodity Fund", ("Agricultural commodities.", "Moderate", 0.078m) }
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
