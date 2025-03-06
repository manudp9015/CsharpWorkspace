using Microsoft.Data.SqlClient;
using MutualFundSimulatorService.Model;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using MutualFundSimulatorService.Business.ConcreteClass;
using static MutualFundSimulatorService.Repository.Interface.IRepository;
using MutualFundSimulatorService.Repository.Interface;
namespace MutualFundSimulatorService.Repository.ConcreteClass

{
    public class MutualFundRepository : IRepository
    {
        private readonly string _connectionString = @"Server=LAPTOP-HS9AFKH4;Database=MutualFundSimulatorApi;Trusted_Connection=True;TrustServerCertificate=True;";
        public string ConnectionString => _connectionString;
        private readonly User _user;
        private readonly UserLumpsumInvest _userLumpsum;
        private readonly UserSipInvest _userSip;

        public MutualFundRepository(User user, UserLumpsumInvest userLumpsum, UserSipInvest userSip)
        {
            _user = user;
            _userLumpsum = userLumpsum;
            _userSip = userSip;
        }

        /// <summary>
        /// Verifies user details against the database by checking email and password match.
        /// </summary>
        /// <returns></returns>
        public bool AuthenticateUser()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT id FROM Users WHERE useremail = @useremail AND userpassword = @password";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", _user.userEmail);
                        command.Parameters.AddWithValue("@password", _user.password);
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            _user.id = (int)result; // Set the ID after successful authentication
                            return true;
                        }
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error authenticating user: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Saves a new user's details into the Users table with an initial wallet balance provided by the user.
        /// </summary>
        /// <returns></returns>
        public bool SaveUserDetails(User userDetails)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE useremail = @useremail";
                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@useremail", userDetails.userEmail);
                        int existingCount = (int)checkCommand.ExecuteScalar();
                        if (existingCount > 0)
                            return false;
                    }

                    if (userDetails.walletBalance < 1000)
                        return false;

                    string insertQuery = @"INSERT INTO Users (useremail, username, userage, userpassword, userphone, walletbalance) 
                                  OUTPUT INSERTED.id 
                                  VALUES (@useremail, @username, @userage, @userpassword, @userphone, @walletbalance)";
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", userDetails.userEmail);
                        command.Parameters.AddWithValue("@username", userDetails.name);
                        command.Parameters.AddWithValue("@userage", userDetails.age);
                        command.Parameters.AddWithValue("@userpassword", userDetails.password);
                        command.Parameters.AddWithValue("@userphone", userDetails.phoneNumber);
                        command.Parameters.AddWithValue("@walletbalance", userDetails.walletBalance);

                        int newId = (int)command.ExecuteScalar();
                        if (newId > 0)
                        {
                            _user.id = newId; // Set the new ID
                            _user.userEmail = userDetails.userEmail;
                            _user.name = userDetails.name;
                            _user.age = userDetails.age;
                            _user.password = userDetails.password;
                            _user.phoneNumber = userDetails.phoneNumber;
                            _user.walletBalance = userDetails.walletBalance;
                            return true;
                        }
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return false;
            }
        }

        public void SaveLumpsumInvest(bool deducted)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = @"
                    INSERT INTO UserLumpsumPortfolio (userid, fundname, quantity, investedamount, currentamount, lumpsumstartdate, deducted) 
                    VALUES (@userid, @fundname, @quantity, @investedamount, @currentamount, @lumpsumstartdate, @deducted)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userid", _user.id);
                        command.Parameters.AddWithValue("@fundname", _userLumpsum.fundName);
                        command.Parameters.AddWithValue("@quantity", _userLumpsum.quantity);
                        command.Parameters.AddWithValue("@investedamount", _userLumpsum.investedAmount);
                        command.Parameters.AddWithValue("@currentamount", _userLumpsum.currentAmount);
                        command.Parameters.AddWithValue("@lumpsumstartdate", _userLumpsum.lumpsumStartDate);
                        command.Parameters.AddWithValue("@deducted", deducted ? 1 : 0);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the current amount for all lump sum investments based on the latest NAV and monthly expenses.
        /// </summary> 
        public void UpdateCurrentAmountsForAllInvestments()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string investmentsQuery = @"
                SELECT lumpsumid, useremail, fundname, quantity, investedamount, currentamount, lumpsumstartdate 
                FROM UserLumpsumPortfolio 
                WHERE useremail = @useremail";
                    SqlCommand investmentsCommand = new SqlCommand(investmentsQuery, connection);
                    investmentsCommand.Parameters.AddWithValue("@useremail", _user.userEmail);

                    List<(int lumpsumid, string fundName, decimal quantity, decimal investedAmount, decimal currentAmount, DateTime startDate)> updates = new List<(int, string, decimal, decimal, decimal, DateTime)>();

                    using (SqlDataReader reader = investmentsCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            updates.Add((
                                (int)reader["lumpsumid"],
                                (string)reader["fundname"],
                                (decimal)reader["quantity"],
                                (decimal)reader["investedamount"],
                                (decimal)reader["currentamount"],
                                (DateTime)reader["lumpsumstartdate"]
                            ));
                        }
                    }

                    foreach (var update in updates)
                    {
                        decimal latestNAV = GetFundPrice(update.fundName);
                        decimal adjustedCurrentAmount = update.quantity * latestNAV;

                        UpdateCurrentAmount(update.lumpsumid, adjustedCurrentAmount, connection);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the current amount for a specific lump sum investment in the database.
        /// </summary>
        /// <param name="lumpsumid"></param>
        /// <param name="updatedCurrentAmount"></param>
        /// <param name="connection"></param>
        public void UpdateCurrentAmount(int lumpsumid, decimal updatedCurrentAmount, SqlConnection connection)
        {
            try
            {
                string updateQuery = "UPDATE UserLumpsumPortfolio SET currentamount = @updatedcurrentamount WHERE lumpsumid = @lumpsumid";
                using (SqlCommand updateCmd = new SqlCommand(updateQuery, connection))
                {
                    updateCmd.Parameters.AddWithValue("@updatedcurrentamount", updatedCurrentAmount);
                    updateCmd.Parameters.AddWithValue("@lumpsumid", lumpsumid);

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                        Console.WriteLine($"Update failed for lumpsumid {lumpsumid}. No rows affected.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }


        /// <summary>
        /// Save SIP investment details to the UserSIPPortfolio table.
        /// </summary>
        public void SaveSIPInvest()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = @"
                    INSERT INTO UserSIPPortfolio (userid, fundname, sipamount, sipstartdate, nextinstallmentdate, totalinstallments, totalinvestedamount, currentamount, totalunits, durationinmonths, sipenddate) 
                    VALUES (@userid, @fundname, @sipamount, @sipstartdate, @nextinstallmentdate, @totalinstallments, @totalinvestedamount, @currentamount, @totalunits, @durationinmonths, @sipenddate)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userid", _user.id);
                        command.Parameters.AddWithValue("@fundname", _userSip.fundName);
                        command.Parameters.AddWithValue("@sipamount", _userSip.sipAmount);
                        command.Parameters.AddWithValue("@sipstartdate", _userSip.sipStartDate);
                        command.Parameters.AddWithValue("@nextinstallmentdate", _userSip.nextInstallmentDate);
                        command.Parameters.AddWithValue("@totalinstallments", _userSip.totalInstallments);
                        command.Parameters.AddWithValue("@totalinvestedamount", _userSip.totalInvestedAmount);
                        command.Parameters.AddWithValue("@currentamount", _userSip.currentAmount);
                        command.Parameters.AddWithValue("@totalunits", _userSip.totalUnits);
                        command.Parameters.AddWithValue("@durationinmonths", _userSip.durationInMonths);
                        command.Parameters.AddWithValue("@sipenddate", _userSip.sipEndDate);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Save a expense entry in the Expenses table for a given fund and date.
        /// </summary>
        /// <param name="fundName"></param>
        /// <param name="expenseAmount"></param>
        /// <param name="expenseDate"></param>
        public void SaveExpense(string fundName, decimal expenseAmount, DateTime expenseDate)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = @"
                    INSERT INTO Expenses (userid, fundname, expenseamount, expensedate)
                    VALUES (@userid, @fundname, @expenseamount, @expensedate)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userid", _user.id);
                        command.Parameters.AddWithValue("@fundname", fundName);
                        command.Parameters.AddWithValue("@expenseamount", expenseAmount);
                        command.Parameters.AddWithValue("@expensedate", expenseDate);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates SIP installments based on the current date, deducting funds and applying expenses.
        /// </summary>
        public void IncrementInstallments()
        {
            try
            {
                Console.WriteLine($"IncrementInstallments: UserID = {_user.id}, Today = {User.CurrentDate}");
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string selectQuery = @"
                SELECT sipamount, fundname, totalinstallments, durationinmonths, 
                       totalinvestedamount, totalunits, nextinstallmentdate, sipenddate
                FROM UserSIPPortfolio
                WHERE userid = @userid AND nextinstallmentdate <= @today AND sipenddate >= @today";

                    // Step 1: Collect all SIPs to process
                    var sipsToUpdate = new List<(decimal sipAmount, string fundName, int totalInstallments, int durationInMonths,
                                                 decimal totalInvestedAmount, decimal totalUnits, DateTime nextInstallmentDate, DateTime sipEndDate)>();

                    using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@userid", _user.id);
                        selectCommand.Parameters.AddWithValue("@today", User.CurrentDate);

                        Console.WriteLine($"Query: {selectQuery.Replace("@userid", _user.id.ToString()).Replace("@today", User.CurrentDate.ToString())}");

                        using (SqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                Console.WriteLine("No SIPs found to increment.");
                                return;
                            }

                            while (reader.Read())
                            {
                                sipsToUpdate.Add((
                                    reader.GetDecimal(reader.GetOrdinal("sipamount")),
                                    reader.GetString(reader.GetOrdinal("fundname")),
                                    reader.GetInt32(reader.GetOrdinal("totalinstallments")),
                                    reader.GetInt32(reader.GetOrdinal("durationinmonths")),
                                    reader.GetDecimal(reader.GetOrdinal("totalinvestedamount")),
                                    reader.GetDecimal(reader.GetOrdinal("totalunits")),
                                    reader.GetDateTime(reader.GetOrdinal("nextinstallmentdate")),
                                    reader.GetDateTime(reader.GetOrdinal("sipenddate"))
                                ));
                            }
                        } 
                    }

                    
                    foreach (var sip in sipsToUpdate)
                    {
                        decimal sipAmount = sip.sipAmount;
                        string fundName = sip.fundName;
                        int totalInstallments = sip.totalInstallments;
                        int durationInMonths = sip.durationInMonths;
                        decimal totalInvestedAmount = sip.totalInvestedAmount;
                        decimal totalUnits = sip.totalUnits;
                        DateTime nextInstallmentDate = sip.nextInstallmentDate;
                        DateTime sipEndDate = sip.sipEndDate;

                        Console.WriteLine($"Processing SIP: Fund = {fundName}, NextInstallment = {nextInstallmentDate}, TotalInstallments = {totalInstallments}");

                        while (User.CurrentDate >= nextInstallmentDate && totalInstallments < durationInMonths && nextInstallmentDate <= sipEndDate)
                        {
                            decimal pricePerUnit = GetFundPrice(fundName);
                            decimal expenseRatio = FundDetailsUtility.GetFundDetails().ContainsKey(fundName)
                                ? FundDetailsUtility.GetFundDetails()[fundName].expenseRatio
                                : 0.070m;
                            decimal expense = sipAmount * expenseRatio / 100m;
                            decimal netAmount = sipAmount - expense;

                            decimal walletBalance = GetWalletBalance();
                            if (walletBalance < sipAmount)
                            {
                                Console.WriteLine($"Insufficient wallet balance for SIP: {fundName}. Required: {sipAmount}, Available: {walletBalance}");
                                return;
                            }

                            totalUnits += netAmount / pricePerUnit;
                            totalInvestedAmount += netAmount;
                            totalInstallments++;
                            nextInstallmentDate = nextInstallmentDate.AddMonths(1);

                            Console.WriteLine($"Updated: NextInstallment = {nextInstallmentDate}, TotalInstallments = {totalInstallments}");

                            AddMoneyToWallet(-sipAmount);
                            SaveExpense(fundName, expense, User.CurrentDate);

                            string updateQuery = @"
                        UPDATE UserSIPPortfolio 
                        SET totalunits = @totalUnits, 
                            totalinstallments = @totalInstallments, 
                            totalinvestedamount = @totalInvestedAmount, 
                            nextinstallmentdate = @nextInstallmentDate, 
                            currentamount = @currentAmount 
                        WHERE userid = @userid AND fundname = @fundName";
                            using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                            {
                                updateCommand.Parameters.AddWithValue("@totalUnits", totalUnits);
                                updateCommand.Parameters.AddWithValue("@totalInstallments", totalInstallments);
                                updateCommand.Parameters.AddWithValue("@totalInvestedAmount", totalInvestedAmount);
                                updateCommand.Parameters.AddWithValue("@nextInstallmentDate", nextInstallmentDate);
                                updateCommand.Parameters.AddWithValue("@currentAmount", totalUnits * pricePerUnit);
                                updateCommand.Parameters.AddWithValue("@userid", _user.id);
                                updateCommand.Parameters.AddWithValue("@fundName", fundName);
                                int rowsAffected = updateCommand.ExecuteNonQuery();
                                Console.WriteLine($"Rows affected: {rowsAffected}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in IncrementInstallments: {ex.Message}");
            }
        }

        private decimal GetMonthlyExpenseRatio(string fundName, SqlConnection connection)
        {
            var fundDetails = FundDetailsUtility.GetFundDetails();
            return fundDetails.ContainsKey(fundName) ? fundDetails[fundName].expenseRatio : 0.070m;
        }

        /// <summary>
        /// Displays the user's lump sum investment portfolio and calculates total profit/loss.
        /// </summary>
        /// <returns></returns>
        public List<LumpSumPortfolioItem> DisplayLumpSumPortfolio()
        {
            var portfolioItems = new List<LumpSumPortfolioItem>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = @"
                    SELECT fundname, 
                           SUM(quantity) AS TotalQuantity,
                           SUM(investedamount) AS TotalInvestedAmount
                    FROM UserLumpsumPortfolio 
                    WHERE userid = @userid
                    GROUP BY fundname
                    ORDER BY fundname";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userid", _user.id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string fundName = reader.GetString(reader.GetOrdinal("fundname"));
                                decimal totalQuantity = reader.GetDecimal(reader.GetOrdinal("TotalQuantity"));
                                decimal totalInvestedAmount = reader.GetDecimal(reader.GetOrdinal("TotalInvestedAmount"));
                                decimal latestNAV = GetFundPrice(fundName);
                                decimal totalCurrentAmount = totalQuantity * latestNAV;

                                portfolioItems.Add(new LumpSumPortfolioItem
                                {
                                    FundName = fundName,
                                    Quantity = totalQuantity,
                                    TotalInvestedAmount = totalInvestedAmount,
                                    TotalCurrentAmount = totalCurrentAmount
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            return portfolioItems;
        }

        /// <summary>
        /// Displays the user's SIP investment portfolio and calculates total profit/loss.
        /// </summary>
        /// <returns></returns>
        public List<SipPortfolioItem> DisplaySIPPortfolio()
        {
            var portfolioItems = new List<SipPortfolioItem>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = @"
                    SELECT fundname, sipamount, sipstartdate, nextinstallmentdate, 
                           totalinstallments, totalinvestedamount, totalunits
                    FROM UserSIPPortfolio
                    WHERE userid = @userid";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userid", _user.id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string fundName = reader.GetString(reader.GetOrdinal("fundname"));
                                decimal sipAmount = reader.GetDecimal(reader.GetOrdinal("sipamount"));
                                decimal totalUnits = reader.GetDecimal(reader.GetOrdinal("totalunits"));
                                decimal totalInvestedAmount = reader.GetDecimal(reader.GetOrdinal("totalinvestedamount"));
                                DateTime sipStartDate = reader.GetDateTime(reader.GetOrdinal("sipstartdate"));
                                DateTime nextInstallment = reader.GetDateTime(reader.GetOrdinal("nextinstallmentdate"));
                                int totalInstallments = reader.GetInt32(reader.GetOrdinal("totalinstallments"));
                                decimal latestNAV = GetFundPrice(fundName);
                                decimal currentAmount = totalUnits * latestNAV;

                                portfolioItems.Add(new SipPortfolioItem
                                {
                                    FundName = fundName,
                                    SipAmount = sipAmount,
                                    Quantity = totalUnits,
                                    SipStartDate = sipStartDate,
                                    NextInstallment = nextInstallment,
                                    TotalInstallments = totalInstallments,
                                    TotalInvestedAmount = totalInvestedAmount,
                                    CurrentAmount = currentAmount
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            return portfolioItems;
        }

        /// <summary>
        /// Retrieves and displays upcoming SIP installment dates for the user.
        /// </summary>
        /// <summary>
        public List<(string FundName, DateTime NextInstallmentDate)> GetUpcomingSIPInstallments()
        {
            var upcomingInstallments = new List<(string FundName, DateTime NextInstallmentDate)>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = @"
                SELECT fundname, nextinstallmentdate 
                FROM UserSIPPortfolio 
                WHERE userid = @userid 
                AND nextinstallmentdate > @today 
                AND sipenddate >= @today 
                ORDER BY nextinstallmentdate";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userid", _user.id);
                        command.Parameters.AddWithValue("@today", User.CurrentDate);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string fundName = reader["fundname"].ToString();
                                DateTime nextInstallmentDate = (DateTime)reader["nextinstallmentdate"];
                                upcomingInstallments.Add((fundName, nextInstallmentDate));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            return upcomingInstallments;
        }

        /// <summary>
        /// Updates NAV values for all funds in the FundNAV table with random values based on fund type and date.
        /// </summary>
        public void UpdateFundNav()
        {
            try
            {
                var funds = new Dictionary<string, List<string>>
                {
                    { "Equity", new List<string> { "Large-Cap Equity Fund", "Mid-Cap Equity Fund", "Small-Cap Equity Fund", "Sectoral/Thematic Fund", "Multi-Cap Fund" } },
                    { "Debt", new List<string> { "Overnight Fund", "Liquid Fund", "Ultra-Short Term Fund", "Short-Term Debt Fund", "Low Duration Fund" } },
                    { "Index", new List<string> { "Nifty 50 Index Fund", "Sensex Index Fund", "Nifty Next 50 Index Fund", "Nifty Bank Index Fund", "Nifty IT Index Fund" } },
                    { "Balanced", new List<string> { "Aggressive Hybrid Fund", "Conservative Hybrid Fund", "Dynamic Asset Allocation Fund", "Balanced Advantage Fund", "Multi-Asset Allocation Fund" } },
                    { "Commodity", new List<string> { "Gold ETF Fund", "Silver ETF Fund", "Multi-Commodity Fund", "Energy Commodity Fund", "Agricultural Commodity Fund" } }
                };

                Random random = new Random();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    foreach (var category in funds)
                    {
                        string fundType = category.Key;
                        foreach (string fundName in category.Value)
                        {
                            decimal currentNav = GetLatestNAV(connection, fundName);

                            decimal percentageChange;
                            switch (fundType)
                            {
                                case "Equity":
                                    percentageChange = (decimal)(random.NextDouble() * 4 - 2); // -2% to +2%
                                    break;
                                case "Debt":
                                    percentageChange = (decimal)(random.NextDouble() * 1 - 0.5); // -0.5% to +0.5%
                                    break;
                                case "Index":
                                    percentageChange = (decimal)(random.NextDouble() * 3 - 1.5); // -1.5% to +1.5%
                                    break;
                                case "Balanced":
                                    percentageChange = (decimal)(random.NextDouble() * 2 - 1); // -1% to +1%
                                    break;
                                case "Commodity":
                                    percentageChange = (decimal)(random.NextDouble() * 0.5 - 0.25); // -0.25% to +0.25%
                                    break;
                                default:
                                    percentageChange = 0;
                                    break;
                            }
                            decimal newNav = Math.Round(currentNav * (1 + percentageChange / 100), 2);

                            string query = @"
                                IF NOT EXISTS (SELECT 1 FROM FundNAV WHERE fundname = @fundname AND navdate = @currentDate)
                                BEGIN
                                    INSERT INTO FundNAV (fundtype, fundname, navvalue, navdate)
                                    VALUES (@fundtype, @fundname, @navvalue, @currentDate);
                                END";

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@fundtype", fundType);
                                command.Parameters.AddWithValue("@fundname", fundName);
                                command.Parameters.AddWithValue("@navvalue", newNav);
                                command.Parameters.AddWithValue("@currentDate", User.CurrentDate);
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Fetches the latest NAV value for a fund from the database.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="fundName"></param>
        /// <returns></returns>
        public decimal GetLatestNAV(SqlConnection connection, string fundName)
        {
            try
            {
                string query = "SELECT TOP 1 navvalue FROM FundNAV WHERE fundname = @fundname ORDER BY navdate DESC";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@fundname", fundName);
                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToDecimal(result) : 100.00m;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Checks if NAV values have been updated for the current date.
        /// </summary>
        /// <returns></returns>
        public bool IsNavAlreadyUpdated()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM FundNAV WHERE navdate = @currentDate";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@currentDate", User.CurrentDate);
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Adds or subtracts money from the user's wallet balance in the database.
        /// </summary>
        /// <param name="amount"></param>
        public void AddMoneyToWallet(decimal amount)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = "UPDATE Users SET walletbalance = walletbalance + @amount WHERE id = @id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@amount", amount);
                        command.Parameters.AddWithValue("@id", _user.id);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                            _user.walletBalance += amount;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
        /// <summary>
        /// Retrieves the most recent NAV value for a specified fund from the FundNAV table.
        /// </summary>
        /// <param name="fundName"></param>
        /// <returns></returns>
        public decimal GetFundPrice(string fundName)
        {
            try
            {
                decimal navValue = 0m;
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT TOP 1 navvalue FROM FundNAV WHERE fundname = @fundname ORDER BY navdate DESC";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@fundname", fundName);
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            navValue = Convert.ToDecimal(result);
                        }
                    }
                }
                return navValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching NAV: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Checks if a user exists in the database based on their ID.
        /// </summary>
        /// <param name="id">The user ID to validate</param>
        /// <returns>True if the user exists, false otherwise</returns>
        public bool IsValidUserId(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE id = @id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error validating user ID: {ex.Message}");
                return false;
            }
        }

        public decimal GetWalletBalance()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT walletbalance FROM Users WHERE id = @id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", _user.id);
                        object result = command.ExecuteScalar();
                        return result != null && result != DBNull.Value ? Convert.ToDecimal(result) : 0m;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching wallet balance: {ex.Message}");
                return 0m;
            }
        }

    }
}