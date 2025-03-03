using Microsoft.Data.SqlClient;
using MutualFundSimulatorService.Model;
using System;
using System.Collections.Generic;
using MutualFundSimulatorService.Business;
using Microsoft.Extensions.DependencyInjection;
namespace MutualFundSimulatorService.Repository
    
{
    public class MutualFundRepository
    {
        private readonly string _connectionString = @"Server=LAPTOP-HS9AFKH4;Database=MutualFundSimulator;Trusted_Connection=True;TrustServerCertificate=True;";
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
                    string query = "SELECT COUNT(*) FROM Users WHERE useremail = @usermail AND userpassword = @password";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@usermail", _user.userEmail);
                        command.Parameters.AddWithValue("@password", _user.password);

                        int count = (int)command.ExecuteScalar();
                        return count == 1;
                    }
                }
            }
            catch (Exception)
            {
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
                                   VALUES (@useremail, @username, @userage, @userpassword, @userphone, @walletbalance)";
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", userDetails.userEmail);
                        command.Parameters.AddWithValue("@username", userDetails.name);
                        command.Parameters.AddWithValue("@userage", userDetails.age);
                        command.Parameters.AddWithValue("@userpassword", userDetails.password);
                        command.Parameters.AddWithValue("@userphone", userDetails.phoneNumber);
                        command.Parameters.AddWithValue("@walletbalance", userDetails.walletBalance);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
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
        /// <summary>
        /// Save lump sum investment details to the UserLumpsumPortfolio table.
        /// </summary>
        /// <param name="deducted"></param>
        public void SaveLumpsumInvest(bool deducted)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = @"
                INSERT INTO UserLumpsumPortfolio (useremail, fundname, quantity, investedamount, currentamount, lumpsumstartdate, deducted) 
                VALUES (@useremail, @fundname, @quantity, @investedamount, @currentamount, @lumpsumstartdate, @deducted)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", _user.userEmail);
                        command.Parameters.AddWithValue("@fundname", _userLumpsum.fundName);
                        command.Parameters.AddWithValue("@quantity", _userLumpsum.quantity);
                        command.Parameters.AddWithValue("@investedamount", _userLumpsum.investedAmount);
                        command.Parameters.AddWithValue("@currentamount", _userLumpsum.currentAmount);
                        command.Parameters.AddWithValue("@lumpsumstartdate", _userLumpsum.lumpsumStartDate);
                        command.Parameters.AddWithValue("@deducted", deducted ? 1 : 0);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected <= 0)
                            Console.WriteLine("Failed to save lump sum investment.");
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
                        INSERT INTO UserSIPPortfolio (useremail, fundname, sipamount, sipstartdate, nextinstallmentdate, totalinstallments, totalinvestedamount, currentamount, totalunits, durationinmonths, sipenddate) 
                        VALUES (@useremail, @fundname, @sipamount, @sipstartdate, @nextinstallmentdate, @totalinstallments, @totalinvestedamount, @currentamount, @totalunits, @durationinmonths, @sipenddate)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", _user.userEmail);
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

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected <= 0)
                            Console.WriteLine("\nError: SIP Investment Not Saved.");
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
                        INSERT INTO Expenses (useremail, fundname, expenseamount, expensedate)
                        VALUES (@useremail, @fundname, @expenseamount, @expensedate)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", _user.userEmail);
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
                DateTime currentDate = User.CurrentDate;

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT sipid, fundname, sipamount, sipstartdate, nextinstallmentdate, totalinstallments, totalinvestedamount, currentamount, totalunits, durationinmonths, sipenddate 
                        FROM UserSIPPortfolio 
                        WHERE useremail = @useremail";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", _user.userEmail);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            List<(int sipid, decimal sipAmount, string fundName, DateTime sipStartDate, DateTime nextInstallmentDate, int totalInstallments, decimal totalInvestedAmount, decimal totalUnits, int durationInMonths, DateTime sipEndDate)> sips = new List<(int, decimal, string, DateTime, DateTime, int, decimal, decimal, int, DateTime)>();

                            while (reader.Read())
                            {
                                sips.Add((
                                    (int)reader["sipid"],
                                    (decimal)reader["sipamount"],
                                    (string)reader["fundname"],
                                    (DateTime)reader["sipstartdate"],
                                    (DateTime)reader["nextinstallmentdate"],
                                    (int)reader["totalinstallments"],
                                    (decimal)reader["totalinvestedamount"],
                                    (decimal)reader["totalunits"],
                                    (int)reader["durationinmonths"],
                                    (DateTime)reader["sipenddate"]
                                ));
                            }
                            reader.Close();

                            foreach (var sip in sips)
                            {
                                DateTime nextDate = sip.nextInstallmentDate;
                                int installments = sip.totalInstallments;
                                decimal invested = sip.totalInvestedAmount;
                                decimal units = sip.totalUnits;

                                while (currentDate >= nextDate && nextDate <= sip.sipEndDate && installments < sip.durationInMonths)
                                {
                                    decimal latestNAV = GetFundPrice(sip.fundName);
                                    decimal monthlyExpenseRatio = GetMonthlyExpenseRatio(sip.fundName, connection);
                                    decimal expense = sip.sipAmount * monthlyExpenseRatio / 100m;
                                    decimal netAmount = sip.sipAmount - expense;
                                    decimal newUnits = Math.Round(netAmount / latestNAV, 2);

                                    if (_user.walletBalance >= sip.sipAmount)
                                    {
                                        AddMoneyToWallet(-sip.sipAmount);
                                        SaveExpense(sip.fundName, expense, nextDate);

                                        units += newUnits;
                                        invested += netAmount;
                                        installments++;
                                        nextDate = nextDate.AddMonths(1);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Insufficient funds for installment {installments + 1} of {sip.fundName} on {nextDate:yyyy-MM-dd}. Required: ₹{sip.sipAmount}, Available: ₹{_user.walletBalance}");
                                        break;
                                    }
                                }

                                if (installments > sip.totalInstallments)
                                {
                                    decimal newCurrentAmount = units * GetFundPrice(sip.fundName);

                                    string updateQuery = @"
                                        UPDATE UserSIPPortfolio 
                                        SET totalinstallments = @totalinstallments, 
                                            totalinvestedamount = @totalinvestedamount, 
                                            currentamount = @currentamount, 
                                            nextinstallmentdate = @nextinstallmentdate,
                                            totalunits = @totalunits
                                        WHERE sipid = @sipid";

                                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                                    {
                                        updateCommand.Parameters.AddWithValue("@totalinstallments", installments);
                                        updateCommand.Parameters.AddWithValue("@totalinvestedamount", invested);
                                        updateCommand.Parameters.AddWithValue("@currentamount", newCurrentAmount);
                                        updateCommand.Parameters.AddWithValue("@nextinstallmentdate", nextDate);
                                        updateCommand.Parameters.AddWithValue("@totalunits", units);
                                        updateCommand.Parameters.AddWithValue("@sipid", sip.sipid);
                                        updateCommand.ExecuteNonQuery();
                                    }
                                }
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
        /// Retrieve the monthly expense ratio for a specific fund.
        /// </summary>
        /// <param name="fundName"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        private decimal GetMonthlyExpenseRatio(string fundName, SqlConnection connection)
        {
            var fundDetails = MutualFundSimulatorUtility.GetFundDetails();
            return fundDetails.ContainsKey(fundName) ? fundDetails[fundName].expenseRatio : 0.070m;
        }

        /// <summary>
        /// Displays the user's lump sum investment portfolio and calculates total profit/loss.
        /// </summary>
        /// <returns></returns>
        public decimal DisplayLumpSumPortfolio()
        {
            decimal totalLumpSumProfitLoss = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = @"
                    SELECT fundname, 
                           SUM(investedamount) AS TotalInvestedAmount, 
                           SUM(quantity) AS TotalQuantity
                    FROM UserLumpsumPortfolio 
                    WHERE useremail = @useremail
                    GROUP BY fundname
                    ORDER BY fundname";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", _user.userEmail);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                Console.WriteLine("\n--- User LumpSum Portfolio ---");
                                Console.WriteLine("Fund Name\t\tQuantity\tTotal Invested Amount\tTotal Current Amount");
                                while (reader.Read())
                                {
                                    string fundName = reader.GetString(reader.GetOrdinal("fundname"));
                                    decimal totalInvestedAmount = reader.GetDecimal(reader.GetOrdinal("TotalInvestedAmount"));
                                    decimal totalQuantity = reader.GetDecimal(reader.GetOrdinal("TotalQuantity"));
                                    decimal latestNAV = GetFundPrice(fundName);
                                    decimal totalCurrentAmount = totalQuantity * latestNAV;

                                    decimal profitLoss = totalCurrentAmount - totalInvestedAmount;
                                    totalLumpSumProfitLoss += profitLoss;

                                    Console.WriteLine($"{fundName}\t\t{totalQuantity:F2}\t\t{totalInvestedAmount:F2}\t\t\t{totalCurrentAmount:F4}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("No LumpSum investments found in your portfolio.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            return totalLumpSumProfitLoss;
        }

        /// <summary>
        /// Displays the user's SIP investment portfolio and calculates total profit/loss.
        /// </summary>
        /// <returns></returns>
        public decimal DisplaySIPPortfolio()
        {
            decimal totalSipProfitLoss = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = @"
                    SELECT fundname, sipamount, sipstartdate, nextinstallmentdate, 
                           totalinstallments, totalinvestedamount, totalunits
                    FROM UserSIPPortfolio
                    WHERE useremail = @useremail";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", _user.userEmail);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                Console.WriteLine("\n--- User SIP Portfolio ---");
                                Console.WriteLine("Fund Name\t\tSIP Amount\tQuantity\tSIP Start Date\tNext Installment\tTotal Installments\tTotal Invested Amount\tCurrent Amount");
                                while (reader.Read())
                                {
                                    string fundName = reader.GetString(reader.GetOrdinal("fundname"));
                                    decimal totalInvestedAmount = reader.GetDecimal(reader.GetOrdinal("totalinvestedamount"));
                                    decimal totalUnits = reader.GetDecimal(reader.GetOrdinal("totalunits"));
                                    decimal latestNAV = GetFundPrice(fundName);
                                    decimal currentAmount = totalUnits * latestNAV;

                                    decimal profitLoss = currentAmount - totalInvestedAmount;
                                    totalSipProfitLoss += profitLoss;

                                    Console.WriteLine($"{fundName}\t{reader["sipamount"]}\t\t{totalUnits:F2}\t\t{reader["sipstartdate"]:yyyy-MM-dd}\t{reader["nextinstallmentdate"]:yyyy-MM-dd}\t\t{reader["totalinstallments"]}\t\t\t{totalInvestedAmount:F2}\t\t\t{currentAmount:F4}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nNo SIP investments found in your portfolio.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            return totalSipProfitLoss;
        }

        /// <summary>
        /// Retrieves and displays upcoming SIP installment dates for the user.
        /// </summary>
        public void GetUpcomingSIPInstallments()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT fundname, nextinstallmentdate 
                        FROM UserSIPPortfolio 
                        WHERE useremail = @useremail AND nextinstallmentdate > @today AND sipenddate >= @today";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", _user.userEmail);
                        command.Parameters.AddWithValue("@today", User.CurrentDate);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                Console.WriteLine("Upcoming SIP Installment Dates:");
                                while (reader.Read())
                                {
                                    _userSip.fundName = reader["fundname"].ToString();
                                    _userSip.nextInstallmentDate = (DateTime)reader["nextinstallmentdate"];
                                    Console.WriteLine($"{_userSip.fundName}: {_userSip.nextInstallmentDate.ToShortDateString()}");
                                }
                            }
                            else
                                Console.WriteLine("\nNo upcoming SIP investments found for this user.");
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
                    string query = "UPDATE Users SET walletbalance = walletbalance + @amount WHERE useremail = @useremail";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@amount", amount);
                        command.Parameters.AddWithValue("@useremail", _user.userEmail);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            _user.walletBalance += amount;
                        }
                        else
                            Console.WriteLine("Failed to update wallet balance.");
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
    }
}