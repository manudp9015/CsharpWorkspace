using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace MutualFundSimulatorApplication
{
    public class MutualFundRepository
    {
        private const string _connectionString = @"Server=LAPTOP-HS9AFKH4;Database=MutualFundSimulator;Trusted_Connection=True;TrustServerCertificate=True;";

        public bool AuthenticateUser(string userEmail, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE useremail = @usermail AND userpassword = @password";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@usermail", userEmail);
                        command.Parameters.AddWithValue("@password", password);

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

        public void SaveUserDetails(string userEmail, string userName, int userAge, string userPassword, string userPhone)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Users (useremail, username, userage, userpassword, userphone) VALUES (@useremail, @username, @userage, @userpassword, @userphone)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", userEmail);
                        command.Parameters.AddWithValue("@username", userName);
                        command.Parameters.AddWithValue("@userage", userAge);
                        command.Parameters.AddWithValue("@userpassword", userPassword);
                        command.Parameters.AddWithValue("@userphone", userPhone);

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

        public void SaveLumpsumInvest(string userEmail, string fundName, decimal quantity, string investmentType, decimal investedAmount, int durationInMonths, DateTime startDate, DateTime endDate)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO UserPortfolio (useremail, fundname, quantity, investmenttype, investedamount, durationInMonths, startDate, endDate) 
                        VALUES (@useremail, @fundname, @quantity, @investmenttype, @investedamount, @durationInMonths, @startDate, @endDate)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", userEmail);
                        command.Parameters.AddWithValue("@fundname", fundName);
                        command.Parameters.AddWithValue("@quantity", quantity);
                        command.Parameters.AddWithValue("@investmenttype", investmentType);
                        command.Parameters.AddWithValue("@investedamount", investedAmount);
                        command.Parameters.AddWithValue("@durationInMonths", durationInMonths);
                        command.Parameters.AddWithValue("@startDate", startDate);
                        command.Parameters.AddWithValue("@endDate", endDate);

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

        public void SaveSIPInvest(string userEmail, string fundName, decimal sipAmount, DateTime startDate, DateTime nextInstallmentDate, int durationInMonths, DateTime endDate)
        {
            try
            {
                decimal nav = GetFundPrice(fundName);
                decimal unitsPurchased = Math.Round(sipAmount / nav, 2);
                decimal currentAmount = unitsPurchased * nav;
                int totalInstallments = 1;
                decimal totalInvestedAmount = sipAmount;

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO UserSIPPortfolio (useremail, fundname, sipamount, sipstartdate, nextinstallmentdate, totalinstallments, totalinvestedamount, currentamount, totalUnits, durationInMonths, startDate, endDate) 
                        VALUES (@useremail, @fundname, @sipamount, @sipstartdate, @nextinstallmentdate, @totalinstallments, @totalinvestedamount, @currentamount, @totalUnits, @durationInMonths, @startDate, @endDate)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", userEmail);
                        command.Parameters.AddWithValue("@fundname", fundName);
                        command.Parameters.AddWithValue("@sipamount", sipAmount);
                        command.Parameters.AddWithValue("@sipstartdate", startDate);
                        command.Parameters.AddWithValue("@nextinstallmentdate", nextInstallmentDate);
                        command.Parameters.AddWithValue("@totalinstallments", totalInstallments);
                        command.Parameters.AddWithValue("@totalinvestedamount", totalInvestedAmount);
                        command.Parameters.AddWithValue("@currentamount", currentAmount);
                        command.Parameters.AddWithValue("@totalUnits", unitsPurchased);
                        command.Parameters.AddWithValue("@durationInMonths", durationInMonths);
                        command.Parameters.AddWithValue("@startDate", startDate);
                        command.Parameters.AddWithValue("@endDate", endDate);

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

        public decimal GetFundPrice(string fundName)
        {
            try
            {
                decimal navValue = 0m;
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT TOP 1 navvalue FROM FundNAV WHERE fundname = @fundname ORDER BY navdate DESC";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@fundname", fundName);
                        object result = command.ExecuteScalar();
                        if (result != null)
                            navValue = Convert.ToDecimal(result);
                    }
                }
                return navValue;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateCurrentAmountsForAllInvestments(string userEmail)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string investmentsQuery = "SELECT investmentid, useremail, fundname, quantity FROM UserPortfolio WHERE useremail = @useremail";
                    SqlCommand investmentsCommand = new SqlCommand(investmentsQuery, connection);
                    investmentsCommand.Parameters.AddWithValue("@useremail", userEmail);

                    List<(int investmentId, decimal updatedCurrentAmount)> updates = new List<(int, decimal)>();

                    using (SqlDataReader reader = investmentsCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int investmentId = (int)reader["investmentid"];
                            string fundName = (string)reader["fundname"];
                            decimal quantity = (decimal)reader["quantity"];

                            decimal latestNAV = GetFundPrice(fundName);
                            decimal updatedCurrentAmount = quantity * latestNAV;
                            updates.Add((investmentId, updatedCurrentAmount));
                        }
                    }
                    foreach (var update in updates)
                    {
                        UpdateCurrentAmount(update.investmentId, update.updatedCurrentAmount, connection);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        private void UpdateCurrentAmount(int investmentId, decimal updatedCurrentAmount, SqlConnection connection)
        {
            try
            {
                string updateQuery = "UPDATE UserPortfolio SET currentamount = @updatedCurrentAmount WHERE investmentid = @investmentId";

                using (SqlCommand updateCmd = new SqlCommand(updateQuery, connection))
                {
                    updateCmd.Parameters.AddWithValue("@updatedCurrentAmount", updatedCurrentAmount);
                    updateCmd.Parameters.AddWithValue("@investmentId", investmentId);
                    updateCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        public void IncrementInstallments(string userEmail)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT fundname, sipamount, nextinstallmentdate, totalinstallments, totalinvestedamount, currentamount, totalUnits, durationInMonths, endDate 
                        FROM UserSIPPortfolio 
                        WHERE useremail = @useremail";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", userEmail);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string fundName = reader["fundname"].ToString();
                                decimal sipAmount = (decimal)reader["sipamount"];
                                DateTime nextInstallmentDate = (DateTime)reader["nextinstallmentdate"];
                                int totalInstallments = (int)reader["totalinstallments"];
                                decimal totalInvestedAmount = (decimal)reader["totalinvestedamount"];
                                decimal currentAmount = (decimal)reader["currentamount"];
                                decimal totalUnits = (decimal)reader["totalUnits"];
                                int durationInMonths = (int)reader["durationInMonths"];
                                DateTime endDate = (DateTime)reader["endDate"];

                                if (DateTime.Today >= nextInstallmentDate && DateTime.Today <= endDate)
                                {
                                    // Fetch the latest NAV for the fund
                                    decimal latestNAV = GetFundPrice(fundName);

                                    // Calculate new units purchased in this installment
                                    decimal newUnits = Math.Round(sipAmount / latestNAV, 2);

                                    // Update total units and total invested amount
                                    totalUnits += newUnits;
                                    totalInvestedAmount += sipAmount;

                                    // Recalculate current amount based on total units and latest NAV
                                    currentAmount = totalUnits * latestNAV;

                                    // Update the next installment date
                                    nextInstallmentDate = nextInstallmentDate.AddMonths(1);

                                    // Update the database
                                    string updateQuery = @"
                                        UPDATE UserSIPPortfolio 
                                        SET totalinstallments = @totalinstallments, 
                                            totalinvestedamount = @totalinvestedamount, 
                                            currentamount = @currentamount, 
                                            nextinstallmentdate = @nextinstallmentdate,
                                            totalUnits = @totalUnits
                                        WHERE useremail = @useremail AND fundname = @fundname";

                                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                                    {
                                        updateCommand.Parameters.AddWithValue("@totalinstallments", totalInstallments + 1);
                                        updateCommand.Parameters.AddWithValue("@totalinvestedamount", totalInvestedAmount);
                                        updateCommand.Parameters.AddWithValue("@currentamount", currentAmount);
                                        updateCommand.Parameters.AddWithValue("@nextinstallmentdate", nextInstallmentDate);
                                        updateCommand.Parameters.AddWithValue("@totalUnits", totalUnits);
                                        updateCommand.Parameters.AddWithValue("@useremail", userEmail);
                                        updateCommand.Parameters.AddWithValue("@fundname", fundName);
                                        updateCommand.ExecuteNonQuery();
                                    }

                                    Console.WriteLine($"SIP installment for {fundName} has been successfully updated.");
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
    }
}