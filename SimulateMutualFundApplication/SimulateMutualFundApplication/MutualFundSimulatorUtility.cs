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
            string name;
            int age;
            string phoneNumber;
            string email, pattern;
            string password;

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
                        return;
                    }
                }
            } while (string.IsNullOrWhiteSpace(name) || !name.All(char.IsLetter));

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
                    return;
                }
                if (ageAttempts == 3)
                {
                    Console.WriteLine("Maximum attempts reached for age.");
                    return;
                }
            } while (age <= 0);

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
                    return;
                }
            } while (phoneNumber.Length != 10 || !long.TryParse(phoneNumber,out _));

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
                    return;
                }
            } while (!Regex.IsMatch(email, pattern));

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
                    return;
                }
            } while (string.IsNullOrWhiteSpace(password));

            SaveUserDetails(name, age, password, phoneNumber);
            Console.WriteLine("User registration completed successfully!");
        }

        private void LoginUser()
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

        public bool AuthenticateUser(string password)
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

        public void SaveUserDetails(string userName, int userage, string userPassword, string userPhone)
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
                            case 4: GetUpcomingSIPInstallments(); break;
                            case 5: return;
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
            FundMenu();
        }

        private void UserPortfolio()
        {
            UpdateCurrentAmountsForAllInvestments();
            DisplayLumpSumPortfolio();

            IncrementInstallments();
            DisplaySIPPortfolio();
        }

        private void DisplayLumpSumPortfolio()
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
                        if (reader.HasRows)
                        {
                            Console.WriteLine("\n--- User LumpSum Portfolio ---");
                            Console.WriteLine("Fund Name\t\tInvestment Type\tTotal Invested Amount\tTotal Current Amount");
                            while (reader.Read())
                            {
                                Console.WriteLine($"{reader["fundname"]}\t{reader["investmenttype"]}\t\t{reader["TotalInvestedAmount"]}\t\t\t{reader["TotalCurrentAmount"]}");
                            }
                        }
                        else
                            Console.WriteLine("No LumpSum investments found in your portfolio.");
                    }
                }
            }
        }

        private void DisplaySIPPortfolio()
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
                        if (reader.HasRows)
                        {
                            Console.WriteLine("\n--- User SIP Portfolio ---");
                            Console.WriteLine("Fund Name\t\tSIP Amount\tSIP Start Date\tNext Installment Date\tTotal Installments\tTotal Invested Amount\tCurrent Amount");
                            while (reader.Read())
                            {
                                Console.WriteLine($"{reader["fundname"]}\t{reader["sipamount"]}\t\t{reader["sipstartdate"]:yyyy-MM-dd}\t{reader["nextinstallmentdate"]:yyyy-MM-dd}\t\t{reader["totalinstallments"]}\t\t\t{reader["totalinvestedamount"]}\t\t\t{reader["currentamount"]}");
                            }
                        }
                        else
                            Console.WriteLine("No SIP investments found in your portfolio.");
                    }
                }
            }
        }



        private void UpdateCurrentAmountsForAllInvestments()
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
        private decimal RetrieveFundNAV(string fundName)
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

        private void UpdateCurrentAmount(int investmentId, decimal updatedCurrentAmount, SqlConnection connection)
        {
            string updateQuery = "UPDATE UserPortfolio SET currentamount = @updatedCurrentAmount WHERE investmentid = @investmentId";

            using (SqlCommand updateCmd = new SqlCommand(updateQuery, connection))
            {
                updateCmd.Parameters.AddWithValue("@updatedCurrentAmount", updatedCurrentAmount);
                updateCmd.Parameters.AddWithValue("@investmentId", investmentId);
                updateCmd.ExecuteNonQuery();
            }
        }



        private void GetUpcomingSIPInstallments()
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
                        {
                            Console.WriteLine("\nNo upcoming SIP investments found for this user.");
                        }
                    }
                }
            }
        }


        public void FundMenu()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("\nFund Menu");
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
                            case 2: DebtFunds(); break;
                            case 3: BalancedFunds(); break;
                            case 4: IndexFunds(); break;
                            case 5: LiquidFunds(); break;
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

        private void LiquidFunds()
        {
            throw new NotImplementedException();
        }

        private void IndexFunds()
        {
            throw new NotImplementedException();
        }

        private void BalancedFunds()
        {
            throw new NotImplementedException();
        }

        private void DebtFunds()
        {
            throw new NotImplementedException();
        }

        public void EquityFundsMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- Equity Funds Menu ---");
                Console.WriteLine("1. Large-Cap Equity Fund");
                Console.WriteLine("2. Mid-Cap Equity Fund");
                Console.WriteLine("3. Small-Cap Equity Fund");
                Console.WriteLine("4. Sectoral/Thematic Fund");
                Console.WriteLine("5. Multi-Cap Fund");
                Console.WriteLine("6. Back to Main Menu\n");
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
        public void InvestInFund(string fundName)
        {
            Console.WriteLine($"You selected: {fundName}");
            Console.WriteLine("1. View Fund Details");
            Console.WriteLine("2. Invest in this Fund");
            Console.WriteLine("3. Back to Fund Menu");
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

        public void LumpSumInvest(string fundName)
        {
            Console.WriteLine($"\n--- Investing in {fundName} ---");

            decimal pricePerUnit = GetFundPrice(fundName);
            Console.WriteLine($"Current price per unit: ₹ {pricePerUnit}");

            Console.Write("Enter the lump sum amount you want to invest: ₹ ");
            if (decimal.TryParse(Console.ReadLine(), out decimal lumpSumAmount) && lumpSumAmount > 0)
            {
               
                decimal unitsPurchased = lumpSumAmount / pricePerUnit;

                Console.WriteLine($"Total units purchased: {unitsPurchased} units at ₹ {pricePerUnit} per unit.");
                Console.WriteLine($"Total investment amount: ₹ {lumpSumAmount}");

                Console.Write("Confirm lump sum investment? (yes/no): ");
                string confirmation = Console.ReadLine()?.ToLower();

                if (confirmation == "yes")
                { 
                    SaveInvestment(fundName, unitsPurchased, "LumpSum", lumpSumAmount);
                    
                    Console.WriteLine("Lump sum investment successful!");
                }
                else
                {
                    Console.WriteLine("Investment cancelled.");
                }
            }
            else
            {
                Console.WriteLine("Invalid lump sum amount. Please try again.");
            }

            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }

        private void SIPInvest(string fundName)
        {
            Console.WriteLine("\n--- SIP Investment ---");

            Console.Write("Enter SIP Amount: ");
            decimal sipAmount;
            while (!decimal.TryParse(Console.ReadLine(), out sipAmount) || sipAmount <= 0)
            {
                Console.Write("Invalid amount. Enter a valid SIP amount: ");
            }

            Console.Write("Enter SIP Start Date (yyyy-MM-dd): ");
            DateTime sipStartDate;
            while (!DateTime.TryParse(Console.ReadLine(), out sipStartDate) || sipStartDate < DateTime.Today)
            {
                Console.Write("Invalid date. Enter a valid date (yyyy-MM-dd): ");
            }

            DateTime nextInstallmentDate = sipStartDate.AddMonths(1);

            SaveSIPInvest(fundName, sipAmount, sipStartDate, nextInstallmentDate);
        }

        private void SaveSIPInvest(string fundName, decimal sipAmount, DateTime sipStartDate, DateTime nextInstallmentDate)
        {
            decimal nav = GetFundPrice(fundName);
            decimal unitsPurchased = sipAmount / nav;

            decimal currentAmount = unitsPurchased * nav;
            int totalInstallments = 1; 
            decimal totalInvestedAmount = sipAmount;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
            INSERT INTO UserSIPPortfolio (useremail, fundname, sipamount, sipstartdate,nextinstallmentdate,totalinstallments,totalinvestedamount,currentamount) 
            VALUES (@useremail, @fundname, @sipamount, @sipstartdate, @nextinstallmentdate, @totalinstallments, @totalinvestedamount, @currentamount)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@useremail", _userEmail);
                    command.Parameters.AddWithValue("@fundname", fundName);
                    command.Parameters.AddWithValue("@sipamount", sipAmount);
                    command.Parameters.AddWithValue("@sipstartdate", sipStartDate);
                    command.Parameters.AddWithValue("@nextinstallmentdate", nextInstallmentDate);
                    command.Parameters.AddWithValue("@totalinstallments", totalInstallments);
                    command.Parameters.AddWithValue("@totalinvestedamount", totalInvestedAmount);
                    command.Parameters.AddWithValue("@currentamount", currentAmount);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        Console.WriteLine("\nSIP Investment Saved Successfully.");
                    else
                        Console.WriteLine("\nError: SIP Investment Not Saved.");
                }
            }
        }
        private void IncrementInstallments()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                SELECT fundname, sipamount, nextinstallmentdate, totalinstallments, totalinvestedamount, currentamount 
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

                            
                            if (DateTime.Today >= nextInstallmentDate)
                            {
                                
                                totalInstallments += 1;
                                totalInvestedAmount += sipAmount;
                                currentAmount += sipAmount;
                                string updateQuery = @"
                                                        UPDATE UserSIPPortfolio 
                                                        SET totalinstallments = @totalinstallments, 
                                                            totalinvestedamount = @totalinvestedamount, 
                                                            currentamount = @currentamount, 
                                                            nextinstallmentdate = @nextinstallmentdate
                                                        WHERE useremail = @useremail AND fundname = @fundname";
                                using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                                {
                                    updateCommand.Parameters.AddWithValue("@totalinstallments", totalInstallments);
                                    updateCommand.Parameters.AddWithValue("@totalinvestedamount", totalInvestedAmount);
                                    updateCommand.Parameters.AddWithValue("@currentamount", currentAmount);
                                    updateCommand.Parameters.AddWithValue("@nextinstallmentdate", nextInstallmentDate.AddMonths(1));
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

        private decimal GetFundPrice(string fundName)
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

        private void SaveInvestment(string fundName, decimal quantity, string investmentType, decimal investedAmount)
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
                        Console.WriteLine(ex);
                    }
                }
            }
            
        }
        public void ViewFundDetails(string fundName)
        {
            Console.WriteLine($"\n--- {fundName} Details ---");
            switch (fundName)
            {
                case "Large-Cap Equity Fund":
                    Console.WriteLine("This fund focuses on investing in large-cap companies with strong market presence.");
                    Console.WriteLine("Risk: Moderate | Recommended for long-term investors.");
                    break;

                case "Mid-Cap Equity Fund":
                    Console.WriteLine("Invests in mid-cap companies with potential for growth.");
                    Console.WriteLine("Risk: High | Suitable for aggressive investors.");
                    break;

                case "Small-Cap Equity Fund":
                    Console.WriteLine("Targets small-cap companies with high growth potential.");
                    Console.WriteLine("Risk: Very High | Ideal for experienced investors.");
                    break;

                case "Sectoral/Thematic Fund":
                    Console.WriteLine("Focused on specific sectors like IT, Pharma, or Energy.");
                    Console.WriteLine("Risk: High | Suitable for those with sector-specific knowledge.");
                    break;

                case "Multi-Cap Fund":
                    Console.WriteLine("Invests across large-cap, mid-cap, and small-cap stocks.");
                    Console.WriteLine("Risk: Moderate | Offers a balanced portfolio.");
                    break;

                default:
                    Console.WriteLine("Fund details not available.");
                    break;
            }
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }
        static void UpdateFundNav()
        {
            string[] funds =
            {
                "Large-Cap Equity Fund",
                "Mid-Cap Equity Fund",
                "Small-Cap Fund",
                "Sectoral/Thematic Fund",
                "Multi-Cap Fund"
            };

            Random random = new Random();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                foreach (string fund in funds)
                {
                    decimal currentNav = GetLatestNAV(connection, fund);
                    decimal percentageChange = (decimal)(random.NextDouble() * 4 - 2); // Random change between -2% to +2%
                    decimal newNav = Math.Round(currentNav * (1 + (percentageChange / 100)), 2);

                    string query = @"
                                    IF NOT EXISTS (SELECT 1 FROM FundNAV WHERE fundname = @fundname AND navdate = CAST(GETDATE() AS DATE))
                                    BEGIN
                                        INSERT INTO FundNAV (fundtype, fundname, navvalue, navdate)
                                        VALUES ('Equity', @fundname, @navvalue, CAST(GETDATE() AS DATE));
                                    END";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@fundname", fund);
                        command.Parameters.AddWithValue("@navvalue", newNav);
                        command.ExecuteNonQuery();
                        Console.WriteLine($"{fund}: New NAV = ₹ {newNav} (Change: {percentageChange}%)");
                    }
                }
            }
        }

        static decimal GetLatestNAV(SqlConnection connection, string fundName)
        {
            string query = "SELECT TOP 1 navvalue FROM FundNAV WHERE fundname = @fundname ORDER BY navdate DESC";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@fundname", fundName);
                object result = command.ExecuteScalar();
                return result != null ? Convert.ToDecimal(result) : 100.00m; 
            }
        }
        private bool IsNavAlreadyUpdated()
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
    }
}
