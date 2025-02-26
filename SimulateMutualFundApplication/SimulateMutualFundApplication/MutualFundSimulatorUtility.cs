using Microsoft.Data.SqlClient;
using MutualFundSimulatorApplication.Business;
using MutualFundSimulatorApplication.Model;
using System;
using System.Collections.Generic;

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

        /// <summary>
        /// Displays the main menu, allowing users to login, register, or exit the application.
        /// </summary>
        public void MainMenu()
        {
            //Console.WriteLine("hii");
            try
            {
                if (!_fundBussines.IsNavAlreadyUpdated())
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
                            case 2: _userLogin.RegisterUser(); break;
                            case 3: Console.WriteLine("exiting..."); return;
                            default: Console.WriteLine("Invalid Input. Give Valid Input"); break;
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

        /// <summary>
        /// Displays the sub-menu for logged-in users, showing portfolio, investment options, and wallet management.
        /// </summary>
        public void SubMenu()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("\n--- Sub Menu ---");
                    Console.WriteLine("1: Add Money to Wallet");
                    Console.WriteLine("2: Portfolio");
                    Console.WriteLine("3: SipOrLumpsum");
                    Console.WriteLine("4: GetUpcomingSIPInstallments");
                    Console.WriteLine("5: Exit");
                    Console.Write("Enter your Choice: ");
                    if (int.TryParse(Console.ReadLine(), out int input))
                    {
                        switch (input)
                        {
                            case 1:
                                AddMoneyToWallet();
                                break;
                            case 2:
                                UserPortfolio(); 
                                break;
                            case 3:
                                SipOrLumpsum();
                                break;
                            case 4:
                                _fundBussines.GetUpcomingSIPInstallments();
                                break;
                            case 5:
                                return;
                            default:
                                Console.WriteLine("Invalid Input. Give Valid Input");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid Input. Give Valid Input");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Allows the user to add money to their wallet, ensuring a minimum of ₹1000.
        /// </summary>
        private void AddMoneyToWallet()
        {
            try
            {
                Console.Write("Enter amount to add to wallet (minimum ₹1000): ");
                if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount >= 1000)
                {
                    _fundBussines.AddMoneyToWallet(amount);
                }
                else
                {
                    Console.WriteLine("Invalid amount. Minimum amount is ₹1000.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Displays the user's investment portfolio, including lump sum and SIP details with profit/loss.
        /// </summary>
        private void UserPortfolio()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_fundBussines.GetConnectionString()))
                {
                    connection.Open();
                    string query = @"
                        SELECT fundname, investedamount, lumpsumstartdate 
                        FROM UserLumpsumPortfolio 
                        WHERE useremail = @useremail AND lumpsumstartdate <= @today AND deducted = 0";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", _user.userEmail);
                        command.Parameters.AddWithValue("@today", User.CurrentDate);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string fundName = reader.GetString(0);
                                decimal investedAmount = reader.GetDecimal(1);
                                DateTime startDate = reader.GetDateTime(2);
                                decimal originalAmount = investedAmount / (1 - new MutualFundSimulatorUtility(null, null, null, null, null).GetFundDetails()[fundName].expenseRatio / 100m);

                                if (_user.walletBalance >= originalAmount)
                                {
                                    _fundBussines.AddMoneyToWallet(-originalAmount);
                                    _fundBussines.SaveExpense(fundName, originalAmount - investedAmount, startDate);

                                    string updateQuery = "UPDATE UserLumpsumPortfolio SET deducted = 1 WHERE useremail = @useremail AND fundname = @fundname AND lumpsumstartdate = @startdate";
                                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                                    {
                                        updateCommand.Parameters.AddWithValue("@useremail", _user.userEmail);
                                        updateCommand.Parameters.AddWithValue("@fundname", fundName);
                                        updateCommand.Parameters.AddWithValue("@startdate", startDate);
                                        updateCommand.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Insufficient funds for lump sum in {fundName}. Required: ₹{originalAmount}, Available: ₹{_user.walletBalance}");
                                }
                            }
                        }
                    }
                }

                _fundBussines.UpdateCurrentAmountsForAllInvestments();
                decimal lumpSumProfitLoss = _fundBussines.DisplayLumpSumPortfolio();
                decimal sipProfitLoss = _fundBussines.DisplaySIPPortfolio();
                decimal totalProfitLoss = lumpSumProfitLoss + sipProfitLoss;

                Console.WriteLine($"\nWallet Balance: ₹ {_user.walletBalance}");

                Console.Write("\nOverall Profit/Loss: ");
                if (totalProfitLoss >= 0)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write($"+{totalProfitLoss:F4}");
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"-{Math.Abs(totalProfitLoss):F4}");
                }
                Console.ResetColor();
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Navigates to the fund menu for SIP or lump sum investment options.
        /// </summary>
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

        /// <summary>
        /// Displays the fund category menu, allowing users to select a fund type for investment.
        /// </summary>
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
                            case 6: return;
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

        /// <summary>
        /// Displays the commodity funds menu, allowing selection of specific commodity funds.
        /// </summary>
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

        /// <summary>
        /// Displays the index funds menu, allowing selection of specific index funds.
        /// </summary>
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

        /// <summary>
        /// Displays the balanced funds menu, allowing selection of specific balanced funds.
        /// </summary>
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

        /// <summary>
        /// Displays the debt funds menu, allowing selection of specific debt funds.
        /// </summary>
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

        /// <summary>
        /// Displays the equity funds menu, allowing selection of specific equity funds.
        /// </summary>
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

        /// <summary>
        /// Provides options to view details or invest in a selected fund.
        /// </summary>
        /// <param name="fundName"></param>
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

        /// <summary>
        /// Returns a dictionary of fund details including description, risk, and expense ratio.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, (string description, string risk, decimal expenseRatio)> GetFundDetails()
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

        /// <summary>
        /// Displays detailed information about a selected fund and offers investment options.
        /// </summary>
        /// <param name="fundName"></param>
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
                    Console.WriteLine($"Expense Ratio: {details.expenseRatio}% per month");
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
    }
}