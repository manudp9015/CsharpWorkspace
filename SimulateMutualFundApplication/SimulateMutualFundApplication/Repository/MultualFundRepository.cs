using Microsoft.Data.SqlClient;
using MutualFundSimulatorApplication.Model;
using System;
using System.Collections.Generic;

namespace MutualFundSimulatorApplication.Repository
{
    internal class MutualFundRepository
    {
        private readonly string _connectionString = @"Server=LAPTOP-HS9AFKH4;Database=MutualFundSimulator;Trusted_Connection=True;TrustServerCertificate=True;";
        public string ConnectionString
        {
            get { return _connectionString; }
        }
        private User _user;
        private UserLumpsumInvest _userLumpsum;
        private UserSipInvest _userSip;

        public MutualFundRepository(User user, UserLumpsumInvest userLumpsum, UserSipInvest userSip)
        {
            _user = user;
            _userLumpsum = userLumpsum;
            _userSip = userSip;
        }

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

        public void SaveUserDetails()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Users (useremail, username, userage, userpassword, userphone, walletbalance) VALUES (@useremail, @username, @userage, @userpassword, @userphone, @walletbalance)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", _user.userEmail);
                        command.Parameters.AddWithValue("@username", _user.name);
                        command.Parameters.AddWithValue("@userage", _user.age);
                        command.Parameters.AddWithValue("@userpassword", _user.password);
                        command.Parameters.AddWithValue("@userphone", _user.phoneNumber);
                        command.Parameters.AddWithValue("@walletbalance", 0m); // Initialize walletbalance

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                            Console.WriteLine("User data inserted successfully!");
                        else
                            Console.WriteLine("Failed to insert user data.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        public void SaveLumpsumInvest()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = @"
                INSERT INTO UserLumpsumPortfolio (useremail, fundname, quantity, investedamount, currentamount, durationinmonths, lumpsumstartdate, lumpsumenddate) 
                VALUES (@useremail, @fundname, @quantity, @investedamount, @currentamount, @durationinmonths, @lumpsumstartdate, @lumpsumenddate)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", _user.userEmail);
                        command.Parameters.AddWithValue("@fundname", _userLumpsum.fundName);
                        command.Parameters.AddWithValue("@quantity", _userLumpsum.quantity);
                        command.Parameters.AddWithValue("@investedamount", _userLumpsum.investedAmount);
                        command.Parameters.AddWithValue("@currentamount", _userLumpsum.currentAmount);
                        command.Parameters.AddWithValue("@durationinmonths", _userLumpsum.durationInMonths);
                        command.Parameters.AddWithValue("@lumpsumstartdate", _userLumpsum.lumpsumStartDate);
                        command.Parameters.AddWithValue("@lumpsumenddate", _userLumpsum.lumpsumEndDate);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                            Console.WriteLine("Lump sum investment saved successfully!");
                        else
                            Console.WriteLine("Failed to save lump sum investment.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        public void UpdateCurrentAmountsForAllInvestments()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string investmentsQuery = "SELECT lumpsumid, useremail, fundname, quantity FROM UserLumpsumPortfolio WHERE useremail = @useremail";
                    SqlCommand investmentsCommand = new SqlCommand(investmentsQuery, connection);
                    investmentsCommand.Parameters.AddWithValue("@useremail", _user.userEmail);

                    List<(int lumpsumid, decimal updatedCurrentAmount)> updates = new List<(int, decimal)>();

                    using (SqlDataReader reader = investmentsCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int lumpsumid = (int)reader["lumpsumid"];
                            string fundName = (string)reader["fundname"];
                            decimal quantity = (decimal)reader["quantity"];

                            decimal latestNAV = GetFundPrice(fundName);
                            decimal updatedCurrentAmount = quantity * latestNAV;
                            updates.Add((lumpsumid, updatedCurrentAmount));
                        }
                    }

                    foreach (var update in updates)
                    {
                        UpdateCurrentAmount(update.lumpsumid, update.updatedCurrentAmount, connection);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

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
                    if (rowsAffected > 0) { }
                    else
                        Console.WriteLine($"Update failed for lumpsumid {lumpsumid}. No rows affected.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

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
                        if (rowsAffected > 0)
                            Console.WriteLine("\nSIP Investment Saved Successfully.");
                        else
                            Console.WriteLine("\nError: SIP Investment Not Saved.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
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
                Console.WriteLine($"An unexpected error occurred while saving investment expense: {ex.Message}");
            }
        }
        public void IncrementInstallments()
        {
            try
            {
                DateTime currentDate = DateTime.Today;

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = @"
                SELECT sipid, fundname, sipamount, nextinstallmentdate, totalinstallments, totalinvestedamount, currentamount, totalunits, durationinmonths, sipenddate 
                FROM UserSIPPortfolio 
                WHERE useremail = @useremail";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", _user.userEmail);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            List<(int sipid, decimal sipAmount, string fundName, DateTime nextInstallmentDate, int totalInstallments, decimal totalInvestedAmount, decimal totalUnits, DateTime sipEndDate)> sips = new List<(int, decimal, string, DateTime, int, decimal, decimal, DateTime)>();

                            while (reader.Read())
                            {
                                sips.Add((
                                    (int)reader["sipid"],
                                    (decimal)reader["sipamount"],
                                    (string)reader["fundname"],
                                    (DateTime)reader["nextinstallmentdate"],
                                    (int)reader["totalinstallments"],
                                    (decimal)reader["totalinvestedamount"],
                                    (decimal)reader["totalunits"],
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

                                while (currentDate >= nextDate && nextDate <= sip.sipEndDate)
                                {
                                    decimal latestNAV = GetFundPrice(sip.fundName);
                                    decimal newUnits = Math.Round(sip.sipAmount / latestNAV, 2);
                                    units += newUnits;
                                    invested += sip.sipAmount;
                                    installments++;
                                    nextDate = nextDate.AddMonths(1);

                                    Console.WriteLine($"Processed installment {installments} for {sip.fundName} on {nextDate.AddMonths(-1):yyyy-MM-dd}");
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

                                    Console.WriteLine($"SIP for {sip.fundName} updated to {installments} installments.");
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
                       SUM(currentamount) AS TotalCurrentAmount
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
                                Console.WriteLine("Fund Name\t\tTotal Invested Amount\tTotal Current Amount");
                                while (reader.Read())
                                {
                                    decimal totalInvestedAmount = reader.GetDecimal(reader.GetOrdinal("TotalInvestedAmount"));
                                    decimal totalCurrentAmount = reader.GetDecimal(reader.GetOrdinal("TotalCurrentAmount"));
                                    decimal profitLoss = totalCurrentAmount - totalInvestedAmount;
                                    totalLumpSumProfitLoss += profitLoss;

                                    Console.WriteLine($"{reader["fundname"]}\t\t{totalInvestedAmount}\t\t\t{totalCurrentAmount}");
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
            return totalLumpSumProfitLoss; // Return profit/loss for lump sum
        }

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
                       totalinstallments, totalinvestedamount, currentamount
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
                                Console.WriteLine("Fund Name\t\tSIP Amount\tSIP Start Date\tNext Installment Date\tTotal Installments\tTotal Invested Amount\tCurrent Amount");
                                while (reader.Read())
                                {
                                    decimal totalInvestedAmount = reader.GetDecimal(reader.GetOrdinal("totalinvestedamount"));
                                    decimal currentAmount = reader.GetDecimal(reader.GetOrdinal("currentamount"));
                                    decimal profitLoss = currentAmount - totalInvestedAmount;
                                    totalSipProfitLoss += profitLoss;

                                    Console.WriteLine($"{reader["fundname"]}\t{reader["sipamount"]}\t\t{reader["sipstartdate"]:yyyy-MM-dd}\t{reader["nextinstallmentdate"]:yyyy-MM-dd}\t\t{reader["totalinstallments"]}\t\t\t{totalInvestedAmount}\t\t\t{currentAmount}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("No SIP investments found in your portfolio.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            return totalSipProfitLoss; // Return profit/loss for SIP
        }

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
                        WHERE useremail = @useremail AND nextinstallmentdate > @today";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", _user.userEmail);
                        command.Parameters.AddWithValue("@today", DateTime.Today);
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
                                    percentageChange = 0; // No change for unknown types
                                    break;
                            }
                            decimal newNav = Math.Round(currentNav * (1 + percentageChange / 100), 2);

                            string query = @"
                        IF NOT EXISTS (SELECT 1 FROM FundNAV WHERE fundname = @fundname AND navdate = CAST(GETDATE() AS DATE))
                        BEGIN
                            INSERT INTO FundNAV (fundtype, fundname, navvalue, navdate)
                            VALUES (@fundtype, @fundname, @navvalue, CAST(GETDATE() AS DATE));
                        END";

                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@fundtype", fundType);
                                command.Parameters.AddWithValue("@fundname", fundName);
                                command.Parameters.AddWithValue("@navvalue", newNav);
                                command.ExecuteNonQuery();
                                Console.WriteLine($"{fundName} ({fundType}): New NAV = ₹ {newNav} (Change: {percentageChange}%)");
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

        public bool IsNavAlreadyUpdated()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM FundNAV WHERE navdate = CAST(GETDATE() AS DATE)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
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
                            _user.walletBalance += amount; // Update in-memory user object
                            Console.WriteLine($"Successfully added ₹{amount} to wallet. New balance: ₹{_user.walletBalance}");
                        }
                        else
                            Console.WriteLine("Failed to add money to wallet.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}