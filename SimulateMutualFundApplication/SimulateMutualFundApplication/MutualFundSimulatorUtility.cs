using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MutualFundSimulatorApplication
{
    internal class MutualFundSimulatorUtility
    {
        private UserLogin _userLogin;
        private User _user;
        private MutualFundBusiness _fundBussines;
        private Lumpsum _lumpsumInvest;
        private Sip _sipInvest;
        public MutualFundSimulatorUtility(UserLogin userLogin, User user, MutualFundBusiness fundBussines, Lumpsum lumpsumInvest, Sip sipInvest)
        {
            _userLogin = userLogin;
            _user = user;
            _fundBussines = fundBussines;
            _lumpsumInvest = lumpsumInvest;
            _sipInvest = sipInvest;
        }

        public void MainMenu()
        {
            Console.WriteLine("hi");
            try
            {
                if (_fundBussines.IsNavAlreadyUpdated() == false)
                {
                    _fundBussines.UpdateFundNav();
                }

                while (true)
                {
                    Console.WriteLine("\n--- Main Menu ---");
                    Console.WriteLine("1: Login User");
                    Console.WriteLine("2: Register User");
                    Console.WriteLine("3: Exit\n");

                    Console.Write("Enter your Choice: ");
                    if (int.TryParse(Console.ReadLine(), out int input))
                    {
                        switch (input)
                        {
                            case 1:
                                if (_userLogin.LoginUser())
                                {
                                    SubMenu();  
                                }
                                break;
                            case 2:
                                _userLogin.RegisterUser();
                                break;
                            case 3:
                                Console.WriteLine("exiting...");
                                return;
                            default:
                                Console.WriteLine("Invalid Input. Give Valid Input");
                                break;
                        }
                    }
                    else
                        Console.WriteLine("Invalid Input. Give Valid Input");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        public void SubMenu()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("\n--- Sub Menu ---");
                    Console.WriteLine("1: Portfolio");
                    Console.WriteLine("2: SipOrLumpsum");
                    Console.WriteLine("3: GetUpcomingSIPInstallments");
                    Console.WriteLine("4: Exit\n");
                    Console.Write("Enter your Choice: ");
                    if (int.TryParse(Console.ReadLine(), out int input))
                    {
                        switch (input)
                        {
                            case 1: UserPortfolio(); break;
                            case 2: SipOrLumpsum(); break;
                            case 3: _fundBussines.GetUpcomingSIPInstallments(); break;
                            case 4: return;
                            default: Console.WriteLine("Invalid Input. Give Valid Input"); break;
                        }
                    }
                    else Console.WriteLine("Invalid Input. Give Valid Input");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        private void SipOrLumpsum()
        {
            try
            {
                FundMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        private void UserPortfolio()
        {
            try
            {
                _fundBussines.UpdateCurrentAmountsForAllInvestments();
                _fundBussines.DisplayLumpSumPortfolio();
                _fundBussines.IncrementInstallments();
                _fundBussines.DisplaySIPPortfolio();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        public void FundMenu()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("\n--- Fund Menu ---");
                    Console.WriteLine("1: Equity Funds");
                    Console.WriteLine("2: Debt Funds");
                    Console.WriteLine("3: Balanced Funds");
                    Console.WriteLine("4: Index Funds");
                    Console.WriteLine("5: CommodityFundsMenu");
                    Console.WriteLine("6: Exit\n");

                    Console.Write("Enter your Choice: ");
                    if (int.TryParse(Console.ReadLine(), out int input))
                    {
                        switch (input)
                        {
                            case 1: EquityFundsMenu(); break;
                            case 2: DebtFundsMenu(); break;
                            case 3: BalancedFundsMenu(); break;
                            case 4: IndexFundsMenu(); break;
                            case 5: CommodityFundsMenu(); break;
                            case 6: Console.WriteLine("exiting..."); return;
                            default: Console.WriteLine("Invalid Input. Give Valid Input"); break;
                        }
                    }
                    else Console.WriteLine("Invalid Input. Give Valid Input");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        public void CommodityFundsMenu()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("\n--- Commodity Funds Menu ---");
                    Console.WriteLine("1. Gold ETF Fund");
                    Console.WriteLine("2. Silver ETF Fund");
                    Console.WriteLine("3. Multi-Commodity Fund");
                    Console.WriteLine("4. Energy Commodity Fund");
                    Console.WriteLine("5. Agricultural Commodity Fund");
                    Console.WriteLine("6. Back to Fund Menu\n");
                    Console.Write("Enter your choice: ");

                    switch (Console.ReadLine())
                    {
                        case "1": InvestInFund("Gold ETF Fund"); break;
                        case "2": InvestInFund("Silver ETF Fund"); break;
                        case "3": InvestInFund("Multi-Commodity Fund"); break;
                        case "4": InvestInFund("Energy Commodity Fund"); break;
                        case "5": InvestInFund("Agricultural Commodity Fund"); break;
                        case "6": return;
                        default: Console.WriteLine("Invalid choice. Please try again."); break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }


        public void IndexFundsMenu()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("\n--- Index Funds Menu ---");
                    Console.WriteLine("1. Nifty 50 Index Fund");
                    Console.WriteLine("2. Sensex Index Fund");
                    Console.WriteLine("3. Nifty Next 50 Index Fund");
                    Console.WriteLine("4. Nifty Bank Index Fund");
                    Console.WriteLine("5. Nifty IT Index Fund");
                    Console.WriteLine("6. Back to Fund Menu\n");
                    Console.Write("Enter your choice: ");

                    switch (Console.ReadLine())
                    {
                        case "1": InvestInFund("Nifty 50 Index Fund"); break;
                        case "2": InvestInFund("Sensex Index Fund"); break;
                        case "3": InvestInFund("Nifty Next 50 Index Fund"); break;
                        case "4": InvestInFund("Nifty Bank Index Fund"); break;
                        case "5": InvestInFund("Nifty IT Index Fund"); break;
                        case "6": return;
                        default: Console.WriteLine("Invalid choice. Please try again."); break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        public void BalancedFundsMenu()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("\n--- Balanced Funds Menu ---");
                    Console.WriteLine("1. Aggressive Hybrid Fund");
                    Console.WriteLine("2. Conservative Hybrid Fund");
                    Console.WriteLine("3. Dynamic Asset Allocation Fund");
                    Console.WriteLine("4. Balanced Advantage Fund");
                    Console.WriteLine("5. Multi-Asset Allocation Fund");
                    Console.WriteLine("6. Back to Fund Menu\n");
                    Console.Write("Enter your choice: ");

                    switch (Console.ReadLine())
                    {
                        case "1": InvestInFund("Aggressive Hybrid Fund"); break;
                        case "2": InvestInFund("Conservative Hybrid Fund"); break;
                        case "3": InvestInFund("Dynamic Asset Allocation Fund"); break;
                        case "4": InvestInFund("Balanced Advantage Fund"); break;
                        case "5": InvestInFund("Multi-Asset Allocation Fund"); break;
                        case "6": return;
                        default: Console.WriteLine("Invalid choice. Please try again."); break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        public void DebtFundsMenu()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("\n--- Debt Funds Menu ---");
                    Console.WriteLine("1. Overnight Fund");
                    Console.WriteLine("2. Liquid Fund");
                    Console.WriteLine("3. Ultra-Short Term Fund");
                    Console.WriteLine("4. Short-Term Debt Fund");
                    Console.WriteLine("5. Low Duration Fund");
                    Console.WriteLine("6. Back to Fund Menu\n");
                    Console.Write("Enter your choice: ");

                    switch (Console.ReadLine())
                    {
                        case "1": InvestInFund("Overnight Fund"); break;
                        case "2": InvestInFund("Liquid Fund"); break;
                        case "3": InvestInFund("Ultra-Short Term Fund"); break;
                        case "4": InvestInFund("Short-Term Debt Fund"); break;
                        case "5": InvestInFund("Low Duration Fund"); break;
                        case "6": return;
                        default: Console.WriteLine("Invalid choice. Please try again."); break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        public void EquityFundsMenu()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("\n--- Equity Funds Menu ---");
                    Console.WriteLine("1. Large-Cap Equity Fund");
                    Console.WriteLine("2. Mid-Cap Equity Fund");
                    Console.WriteLine("3. Small-Cap Equity Fund");
                    Console.WriteLine("4. Sectoral/Thematic Fund");
                    Console.WriteLine("5. Multi-Cap Fund");
                    Console.WriteLine("6. Back to Fund Menu\n");
                    Console.Write("Enter your choice: ");

                    switch (Console.ReadLine())
                    {
                        case "1": InvestInFund("Large-Cap Equity Fund"); break;
                        case "2": InvestInFund("Mid-Cap Equity Fund"); break;
                        case "3": InvestInFund("Small-Cap Equity Fund"); break;
                        case "4": InvestInFund("Sectoral/Thematic Fund"); break;
                        case "5": InvestInFund("Multi-Cap Fund"); break;
                        case "6": return;
                        default: Console.WriteLine("Invalid choice. Please try again."); break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        public void InvestInFund(string fundName)
        {
            try
            {
                Console.WriteLine($"You selected: {fundName}");
                Console.WriteLine("\n1. View Fund Details");
                Console.WriteLine("2. Lumpsum Invest in this Fund");
                Console.WriteLine("3. Sip Invest in this Fund");
                Console.WriteLine("4. Back to Fund Menu\n");
                Console.Write("Enter your choice: ");

                switch (Console.ReadLine())
                {
                    case "1": ViewFundDetails(fundName); break;
                    case "2": _lumpsumInvest.LumpSumInvest(fundName); break;
                    case "3": _sipInvest.SIPInvest(fundName); break;
                    case "4": return;
                    default: Console.WriteLine("Invalid choice. Please try again."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        public void ViewFundDetails(string fundName)
        {
            try
            {
                var fundDetails = GetFundDetails();

                if (fundDetails.ContainsKey(fundName))
                {
                    var details = fundDetails[fundName];
                    Console.WriteLine($"\n--- {fundName} Details ---");
                    Console.WriteLine($"Description: {details.description}");
                    Console.WriteLine($"Risk: {details.risk}");
                    InvestInFund(fundName);
                }
                else
                {
                    Console.WriteLine("Fund details not available.");
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        private Dictionary<string, (string description, string risk)> GetFundDetails()
        {
            try
            {
                return new Dictionary<string, (string description, string risk)>
        {
            { "Large-Cap Equity Fund", ("This fund focuses on investing in large-cap companies with strong market presence.", "Moderate | Recommended for long-term investors.") },
            { "Mid-Cap Equity Fund", ("Invests in mid-cap companies with potential for growth.", "High | Suitable for aggressive investors.") },
            { "Small-Cap Equity Fund", ("Targets small-cap companies with high growth potential.", "Very High | Ideal for experienced investors.") },
            { "Sectoral/Thematic Fund", ("Focused on specific sectors like IT, Pharma, or Energy.", "High | Suitable for those with sector-specific knowledge.") },
            { "Multi-Cap Fund", ("Invests across large-cap, mid-cap, and small-cap stocks.", "Moderate | Offers a balanced portfolio.") },
            { "Overnight Fund", ("Invests in overnight securities, providing liquidity and safety.", "Low | Suitable for short-term investors.") },
            { "Liquid Fund", ("Invests in short-term debt instruments to provide liquidity with low risk.", "Low | Good for conservative investors.") },
            { "Ultra-Short Term Fund", ("Invests in debt and money market instruments with a short-term horizon.", "Low to Moderate | Suitable for conservative investors.") },
            { "Short-Term Debt Fund", ("Focuses on debt instruments with a short-term maturity.", "Moderate | Suitable for short-term investors.") },
            { "Low Duration Fund", ("Invests in debt instruments with a lower duration, reducing interest rate risk.", "Moderate | Suitable for risk-averse investors.") },
            { "Nifty 50 Index Fund", ("Tracks the performance of the Nifty 50 Index, representing the top 50 Indian stocks.", "Moderate | Suitable for passive investors.") },
            { "Sensex Index Fund", ("Tracks the performance of the BSE Sensex, representing the top 30 Indian stocks.", "Moderate | Suitable for long-term passive investors.") },
            { "Nifty Next 50 Index Fund", ("Tracks the Nifty Next 50 Index, representing 50 companies after the Nifty 50.", "Moderate | Suitable for passive investors.") },
            { "Nifty Bank Index Fund", ("Tracks the Nifty Bank Index, representing major Indian banks.", "Moderate | Suitable for those focused on the banking sector.") },
            { "Nifty IT Index Fund", ("Tracks the Nifty IT Index, focusing on IT sector companies.", "Moderate | Suitable for those focused on the IT sector.") },
            { "Aggressive Hybrid Fund", ("Invests in both equity and debt, with a higher proportion in equity for growth.", "High | Suitable for aggressive investors.") },
            { "Conservative Hybrid Fund", ("Invests in both equity and debt, with a higher proportion in debt for stability.", "Low to Moderate | Suitable for conservative investors.") },
            { "Dynamic Asset Allocation Fund", ("Adjusts the allocation between equity and debt based on market conditions.", "Moderate | Suitable for investors looking for flexibility.") },
            { "Balanced Advantage Fund", ("A flexible fund that dynamically allocates between equity and debt based on valuations.", "Moderate | Suitable for long-term investors.") },
            { "Multi-Asset Allocation Fund", ("Invests in multiple asset classes like equity, debt, and commodities.", "Moderate | Suitable for diversification.") },
            { "Gold ETF Fund", ("Invests in gold-backed assets such as ETFs and gold mining companies.", "Moderate | Suitable for investors seeking an inflation hedge.") },
            { "Silver ETF Fund", ("Focuses on silver as a commodity, including silver-backed securities.", "Moderate to High | Suitable for commodity-focused investors.") },
            { "Multi-Commodity Fund", ("Diversified investment in multiple commodities like gold, silver, and metals.", "Moderate | Suitable for diversified commodity exposure.") },
            { "Energy Commodity Fund", ("Invests in energy-related commodities such as oil, natural gas, and renewables.", "High | Suitable for those interested in energy markets.") },
            { "Agricultural Commodity Fund", ("Focuses on agricultural commodities like wheat, corn, and soybeans.", "Moderate | Suitable for investors with an interest in agricultural markets.") }
        };
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}