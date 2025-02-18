using Azure;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MutualFundSimulatorApplication
{
    internal class MutualFundSimulatorUtility
    {
        private string _userEmail;
        const string _connectionString = @"Server=LAPTOP-HS9AFKH4;Database=MutualFundSimulator;Trusted_Connection=True;TrustServerCertificate=True;";
        private MutualFundRepository _repository;

        public MutualFundSimulatorUtility()
        {
            _repository = new MutualFundRepository();
        }
        public void MainMenu()
        {
            try
            { 
                if (IsNavAlreadyUpdated() == false)
                {
                    UpdateFundNav();
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
                            case 1: LoginUser(); break;
                            case 2: RegisterUser(); break;
                            case 3: Console.WriteLine("exiting..."); return;
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

        private void RegisterUser()
        {
            try
            {
                string name;
                int age;
                string phoneNumber;
                string pattern;
                string password;

                bool validName = ValidateUserName(out name);
                if (!validName)
                {
                    return;
                }
                bool validAge = ValidateUserAge(out age);
                if (!validAge)
                {
                    return;
                }
                bool validPhoneNumber = ValidateUserMobilNumber(out phoneNumber);
                if (!validPhoneNumber)
                {
                    return;
                }
                bool validMail = ValidateUserMail(out _userEmail, out pattern);
                if (!validMail)
                {
                    return;
                }
                bool validPassword = ValidateUserPassword(out password);
                if (!validPassword)
                {
                    return;
                }
                SaveUserDetails(name, age, password, phoneNumber);
                Console.WriteLine("User registration completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        private static bool ValidateUserPassword(out string password)
        {
            try
            {
                int passwordAttempts = 0;
                do
                {
                    Console.Write("Enter your password: ");
                    password = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(password))
                    {
                        passwordAttempts++;
                        Console.WriteLine("Password cannot be empty.");
                    }
                    if (passwordAttempts == 3)
                    {
                        Console.WriteLine("Maximum attempts reached for password.");
                        return false;
                    }
                } while (string.IsNullOrWhiteSpace(password)); return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static bool ValidateUserMail(out string email, out string pattern)
        {
            try
            {
                int emailAttempts = 0;
                do
                {
                    Console.Write("Enter your Gmail: ");
                    email = Console.ReadLine();
                    pattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
                    if (!Regex.IsMatch(email, pattern))
                    {
                        emailAttempts++;
                        Console.WriteLine("Invalid Gmail address. Please enter a valid Gmail.");
                    }
                    if (emailAttempts == 3)
                    {
                        Console.WriteLine("Maximum attempts reached for email.");
                        return false;
                    }
                } while (!Regex.IsMatch(email, pattern)); return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static bool ValidateUserMobilNumber(out string phoneNumber)
        {
            try
            {
                int phoneAttempts = 0;
                do
                {
                    Console.Write("Enter your phone number: ");
                    phoneNumber = Console.ReadLine();
                    if (phoneNumber.Length != 10 || !long.TryParse(phoneNumber, out _))
                    {
                        phoneAttempts++;
                        Console.WriteLine("Invalid phone number. Please enter a 10-digit number.");

                    }
                    if (phoneAttempts == 3)
                    {
                        Console.WriteLine("Maximum attempts reached for phone number.");
                        return false;
                    }
                } while (phoneNumber.Length != 10 || !long.TryParse(phoneNumber, out _)); return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static bool ValidateUserAge(out int age)
        {
            try
            {
                int ageAttempts = 0;
                do
                {
                    Console.Write("Enter your age: ");
                    string input = Console.ReadLine();
                    if (!int.TryParse(input, out age))
                    {
                        ageAttempts++;
                        Console.WriteLine("Invalid input. Please enter a valid numeric age.");
                    }
                    else if (age < 18)
                    {
                        Console.WriteLine("Access denied. Your age is less than 18.");
                        return false;
                    }
                    if (ageAttempts == 3)
                    {
                        Console.WriteLine("Maximum attempts reached for age.");
                        return false;
                    }
                } while (age <= 0); return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static bool ValidateUserName(out string name)
        {
            try
            {
                int nameAttempts = 0;
                do
                {
                    Console.Write("Enter your name: ");
                    name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(name) || !name.All(char.IsLetter))
                    {
                        nameAttempts++;
                        Console.WriteLine("User name must be alphabetic and cannot be empty.");
                        if (nameAttempts == 3)
                        {
                            Console.WriteLine("Maximum attempts reached for name.");
                            return false;
                        }
                    }
                } while (string.IsNullOrWhiteSpace(name) || !name.All(char.IsLetter)); return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void LoginUser()
        {
            try
            {
                string userPassword;
                Console.Write("Enter your email: ");
                _userEmail = Console.ReadLine();
                Console.Write("Enter your password: ");
                userPassword = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(_userEmail) || string.IsNullOrWhiteSpace(userPassword))
                {
                    Console.WriteLine("Both email and password are required. Please try again.");
                    return;
                }
                if (AuthenticateUser(userPassword))
                {
                    Console.WriteLine("Login successful.");
                    SubMenu();
                }
                else
                    Console.WriteLine("Invalid email or password.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool AuthenticateUser(string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE useremail = @usermail AND userpassword = @password";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@usermail", _userEmail);
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

        public void SaveUserDetails(string userName, int userage, string userPassword, string userPhone)
        {
            try
            {
                string connectionString = @"Server=LAPTOP-HS9AFKH4;Database=MutualFundSimulator;Trusted_Connection=True;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Users (useremail, username, userage, userpassword, userphone)" +
                                   " VALUES (@useremail, @username, @userage, @userpassword, @userphone)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", _userEmail);
                        command.Parameters.AddWithValue("@username", userName);
                        command.Parameters.AddWithValue("@userage", userage);
                        command.Parameters.AddWithValue("@userpassword", userPassword);
                        command.Parameters.AddWithValue("@userphone", userPhone);
                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                                Console.WriteLine("User data inserted successfully!");
                            else
                                Console.WriteLine("Failed to insert user data.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
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
                            case 3: GetUpcomingSIPInstallments(); break;
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
                UpdateCurrentAmountsForAllInvestments();
                DisplayLumpSumPortfolio();

                IncrementInstallments();
                DisplaySIPPortfolio();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        private void DisplayLumpSumPortfolio()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = @"
                                    SELECT fundname, investmenttype, 
                                           SUM(investedamount) AS TotalInvestedAmount, 
                                           SUM(currentamount) AS TotalCurrentAmount
                                    FROM UserPortfolio 
                                    WHERE useremail = @useremail
                                    GROUP BY fundname, investmenttype
                                    ORDER BY fundname, investmenttype";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", _userEmail);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            decimal totalProfitLoss = 0;
                            if (reader.HasRows)
                            {
                                Console.WriteLine("\n--- User LumpSum Portfolio ---");
                                Console.WriteLine("Fund Name\t\tInvestment Type\tTotal Invested Amount\tTotal Current Amount");
                                while (reader.Read())
                                {
                                    decimal totalInvestedAmount = reader.GetDecimal(reader.GetOrdinal("TotalInvestedAmount"));
                                    decimal totalCurrentAmount = reader.GetDecimal(reader.GetOrdinal("TotalCurrentAmount"));
                                    decimal profitLoss = totalCurrentAmount - totalInvestedAmount;
                                    totalProfitLoss += profitLoss;

                                    Console.WriteLine($"{reader["fundname"]}\t{reader["investmenttype"]}\t\t{totalInvestedAmount}\t\t\t{totalCurrentAmount}");
                                }

                               
                                if (totalProfitLoss >= 0)
                                    Console.WriteLine($"\nOverall, you made a Profit of {totalProfitLoss} in your LumpSum invest.");
                                else
                                    Console.WriteLine($"\nOverall, you made a Loss of {-totalProfitLoss} in your LumpSum invest.");
                            }
                            else
                                Console.WriteLine("No LumpSum investments found in your portfolio.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        private void DisplaySIPPortfolio()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = @"
                                    SELECT fundname, sipamount, sipstartdate, nextinstallmentdate, 
                                           totalinstallments, totalinvestedamount, currentamount
                                    FROM UserSIPPortfolio
                                    WHERE useremail = @useremail";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", _userEmail);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            decimal totalProfitLoss = 0;
                            if (reader.HasRows)
                            {
                                Console.WriteLine("\n--- User SIP Portfolio ---");
                                Console.WriteLine("Fund Name\t\tSIP Amount\tSIP Start Date\tNext Installment Date\tTotal Installments\tTotal Invested Amount\tCurrent Amount");
                                while (reader.Read())
                                {
                                    decimal totalInvestedAmount = reader.GetDecimal(reader.GetOrdinal("totalinvestedamount"));
                                    decimal currentAmount = reader.GetDecimal(reader.GetOrdinal("currentamount"));
                                    decimal profitLoss = currentAmount - totalInvestedAmount;
                                    totalProfitLoss += profitLoss;

                                    Console.WriteLine($"{reader["fundname"]}\t{reader["sipamount"]}\t\t{reader["sipstartdate"]:yyyy-MM-dd}\t{reader["nextinstallmentdate"]:yyyy-MM-dd}\t\t{reader["totalinstallments"]}\t\t\t{totalInvestedAmount}\t\t\t{currentAmount}");
                                }

                               
                                if (totalProfitLoss >= 0)
                                    Console.WriteLine($"\nOverall, you made a Profit of {totalProfitLoss} in your SIP invest.");
                                else
                                    Console.WriteLine($"\nOverall, you made a Loss of {-totalProfitLoss} in your SIP invest.");
                            }
                            else
                                Console.WriteLine("No SIP investments found in your portfolio.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        private void UpdateCurrentAmountsForAllInvestments()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string investmentsQuery = "SELECT investmentid, useremail, fundname, quantity FROM UserPortfolio";
                    SqlCommand investmentsCommand = new SqlCommand(investmentsQuery, connection);

                    List<(int investmentId, decimal updatedCurrentAmount)> updates = new List<(int, decimal)>();

                    using (SqlDataReader reader = investmentsCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int investmentId = (int)reader["investmentid"];
                            string fundName = (string)reader["fundname"];
                            decimal quantity = (decimal)reader["quantity"];

                            decimal latestNAV = RetrieveFundNAV(fundName);
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

        private decimal RetrieveFundNAV(string fundName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT TOP 1 navvalue FROM FundNAV WHERE fundname = @fundname ORDER BY navdate DESC";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@fundname", fundName);
                        object result = command.ExecuteScalar();
                        return result != null ? Convert.ToDecimal(result) : 100.00m;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
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

        private void GetUpcomingSIPInstallments()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = @"
                                    SELECT fundname, nextinstallmentdate 
                                    FROM UserSIPPortfolio 
                                    WHERE useremail = @useremail AND nextinstallmentdate > @today";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", _userEmail);
                        command.Parameters.AddWithValue("@today", DateTime.Today);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                Console.WriteLine("Upcoming SIP Installment Dates:");
                                while (reader.Read())
                                {
                                    string fundName = reader["fundname"].ToString();
                                    DateTime nextInstallmentDate = (DateTime)reader["nextinstallmentdate"];
                                    Console.WriteLine($"{fundName}: {nextInstallmentDate.ToShortDateString()}");
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
                    Console.WriteLine("5: Liquid Funds");
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
                            case 5: LiquidFundsMenu(); break;
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

        public void LiquidFundsMenu()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("\n--- Liquid Funds Menu ---");
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
                Console.WriteLine("1. View Fund Details");
                Console.WriteLine("2. Lumpsum Invest in this Fund");
                Console.WriteLine("3. Sip Invest in this Fund");
                Console.WriteLine("4. Back to Fund Menu");
                Console.Write("Enter your choice: ");

                switch (Console.ReadLine())
                {
                    case "1": ViewFundDetails(fundName); break;
                    case "2": LumpSumInvest(fundName); break;
                    case "3": SIPInvest(fundName); break;
                    case "4": return;
                    default: Console.WriteLine("Invalid choice. Please try again."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        private void LumpSumInvest(string fundName)
        {
            try
            {
                Console.WriteLine($"\n--- Investing in {fundName} ---");

                decimal pricePerUnit = _repository.GetFundPrice(fundName);
                Console.WriteLine($"Current price per unit: ₹ {pricePerUnit}");

                Console.Write("Enter the lump sum amount you want to invest (minimum ₹5000): ");
                if (decimal.TryParse(Console.ReadLine(), out decimal lumpSumAmount) && lumpSumAmount >= 5000)
                {
                    decimal unitsPurchased = Math.Round(lumpSumAmount / pricePerUnit, 2);

                    Console.WriteLine($"Total units purchased: {unitsPurchased} units at ₹ {pricePerUnit} per unit.");
                    Console.WriteLine($"Total investment amount: ₹ {lumpSumAmount}");

                    Console.Write("Confirm lump sum investment? (yes/no): ");
                    string confirmation = Console.ReadLine()?.ToLower();

                    if (confirmation == "yes")
                    {
                        int durationInMonths = 12; // Minimum 1 year
                        DateTime startDate = DateTime.Today;
                        DateTime endDate = startDate.AddMonths(durationInMonths);

                        _repository.SaveLumpsumInvest(_userEmail, fundName, unitsPurchased, "LumpSum", lumpSumAmount, durationInMonths, startDate, endDate);
                        Console.WriteLine("Lump sum investment successful!");
                    }
                    else
                    {
                        Console.WriteLine("Investment cancelled.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid lump sum amount. Minimum amount is ₹5000.");
                }
                Console.WriteLine("\nPress any key to return to the menu...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        private void SIPInvest(string fundName)
        {
            try
            {
                int amountAttempts = 0;
                Console.WriteLine("\n--- SIP Investment ---");

                decimal sipAmount;
                Console.Write("Enter SIP Amount (Min ₹500): ");
                while (!decimal.TryParse(Console.ReadLine(), out sipAmount) || sipAmount < 500)
                {
                    amountAttempts++;
                    if (amountAttempts == 3)
                    {
                        Console.WriteLine("Maximum attempts reached for amount."); return;
                    }
                    Console.Write("Invalid amount. Enter a valid SIP amount (Min ₹500): ");
                }

                int dateAttempts = 0;
                DateTime sipStartDate;
                Console.Write("Enter SIP Start Date (yyyy-MM-dd): ");
                while (!DateTime.TryParse(Console.ReadLine(), out sipStartDate) || sipStartDate < DateTime.Today)
                {
                    dateAttempts++;
                    if (dateAttempts == 3)
                    {
                        Console.WriteLine("Maximum attempts reached for date."); return;
                    }
                    Console.Write("Invalid date. Enter a valid date (yyyy-MM-dd): ");
                }

                int durationAttempts = 0;
                int sipYears;
                Console.Write("Enter SIP Duration (Min 1 Year): ");
                while (!int.TryParse(Console.ReadLine(), out sipYears) || sipYears < 1)
                {
                    durationAttempts++;
                    if (durationAttempts == 3)
                    {
                        Console.WriteLine("Maximum attempts reached for duration."); return;
                    }
                    Console.Write("Invalid duration. Enter a valid number of years (Min 1 Year): ");
                }

                DateTime sipEndDate = sipStartDate.AddYears(sipYears);
                DateTime nextInstallmentDate = sipStartDate.AddMonths(1);

                _repository.SaveSIPInvest(fundName, sipAmount, sipStartDate, nextInstallmentDate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }



        



        private void IncrementInstallments()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = @"
                SELECT fundname, sipamount, nextinstallmentdate, totalinstallments, totalinvestedamount, currentamount, totalUnits 
                FROM UserSIPPortfolio 
                WHERE useremail = @useremail";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", _userEmail);
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

                                if (DateTime.Today >= nextInstallmentDate)
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
                                        updateCommand.Parameters.AddWithValue("@useremail", _userEmail);
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


        private decimal GetFundPrice(string fundName)
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

        private void SaveLumpsumInvest(string fundName, decimal quantity, string investmentType, decimal investedAmount)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO UserPortfolio (useremail, fundname, quantity, investmenttype, investedamount )" +
                                   " VALUES (@useremail, @fundname, @quantity,@investmenttype, @investedamount)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@useremail", _userEmail);
                        command.Parameters.AddWithValue("@fundname", fundName);
                        command.Parameters.AddWithValue("@quantity", quantity);
                        command.Parameters.AddWithValue("@investmenttype", investmentType);
                        command.Parameters.AddWithValue("@investedamount", investedAmount);
                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                                Console.WriteLine($"{_userEmail} investment in {fundName} saved with {quantity} units and a total of ₹ {investedAmount}.");
                            else
                                Console.WriteLine("Failed to insert user data.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                        }
                    }
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
                }
                else
                {
                    Console.WriteLine("Fund details not available.");
                }
                Console.WriteLine("\nPress any key to return to the menu...");
                Console.ReadKey();
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
                { "Long Duration Fund", ("Invests in long-term debt instruments, typically with higher interest rate risk.", "High | Suitable for long-term conservative investors.") },
                { "Short Duration Fund", ("Invests in debt instruments with a short maturity period, reducing interest rate risk.", "Low to Moderate | Suitable for conservative investors.") },
                { "Corporate Bond Fund", ("Invests in corporate bonds, offering higher yields with moderate risk.", "Moderate | Suitable for income-seeking investors.") },
                { "Government Bond Fund", ("Invests in government securities, providing stability and low risk.", "Low | Suitable for risk-averse investors.") },
                { "Gilt Fund", ("Invests in government securities with longer duration for potentially higher returns.", "Moderate | Suitable for conservative long-term investors.") }
            };
            }
            catch (Exception)
            {
                throw;
            }
        }

        static void UpdateFundNav()
        {
            try
            {
                var funds = new Dictionary<string, List<string>>
                {
                    { "Equity", new List<string> { "Large-Cap Equity Fund", "Mid-Cap Equity Fund", "Small-Cap Equity Fund", "Sectoral/Thematic Fund", "Multi-Cap Fund" } },
                    { "Debt", new List<string> { "Overnight Fund", "Liquid Fund", "Ultra-Short Term Fund", "Short-Term Debt Fund", "Low Duration Fund" } },
                    { "Index", new List<string> { "Nifty 50 Index Fund", "Sensex Index Fund", "Nifty Next 50 Index Fund", "Nifty Bank Index Fund", "Nifty IT Index Fund" } },
                    { "Balanced", new List<string> { "Aggressive Hybrid Fund", "Conservative Hybrid Fund", "Dynamic Asset Allocation Fund", "Balanced Advantage Fund", "Multi-Asset Allocation Fund" } },
                    { "Liquid", new List<string> { "Liquid Fund", "Overnight Fund", "Ultra-Short Term Fund", "Short-Term Debt Fund", "Low Duration Fund" } }
                };

                Random random = new Random();
                using (SqlConnection connection = new SqlConnection(_connectionString))
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
                                case "Liquid":
                                    percentageChange = (decimal)(random.NextDouble() * 0.5 - 0.25); // -0.25% to +0.25%
                                    break;
                                default:
                                    percentageChange = 0; // No change for unknown types
                                    break;
                            }
                            decimal newNav = Math.Round(currentNav * (1 + (percentageChange / 100)), 2);

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

        static decimal GetLatestNAV(SqlConnection connection, string fundName)
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

        private bool IsNavAlreadyUpdated()
        {
            try
            {
                string connectionString = @"Server=LAPTOP-HS9AFKH4;Database=MutualFundSimulator;Trusted_Connection=True;TrustServerCertificate=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
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
    }
}
