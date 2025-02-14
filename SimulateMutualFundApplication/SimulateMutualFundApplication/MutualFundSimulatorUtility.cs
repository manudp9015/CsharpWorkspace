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
        private string userEmail;
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
            string email,pattern;
            string password;

            do
            {
                Console.Write("Enter your name: ");
                name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                    Console.WriteLine("User name cannot be empty.");
            } while (string.IsNullOrWhiteSpace(name));

            do
            {
                Console.Write("Enter your age: ");
                string input = Console.ReadLine();
                if (!int.TryParse(input, out age))
                    Console.WriteLine("Invalid input. Please enter a valid numeric age.");
                else if (age < 18)
                {
                    Console.WriteLine("Access denied. You age less than 18.");
                    return;
                }
            } while (age <= 0);

            do
            {
                Console.Write("Enter your phone number: ");
                phoneNumber = Console.ReadLine();
                if (phoneNumber.Length != 10 || !long.TryParse(phoneNumber, out _))
                    Console.WriteLine("Invalid phone number. Please enter a 10-digit number.");
            } while (phoneNumber.Length != 10 || !long.TryParse(phoneNumber, out _));


            do
            {
                Console.Write("Enter your Gmail: ");
                email = Console.ReadLine();
                pattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
                if (!Regex.IsMatch(email, pattern))
                    Console.WriteLine("Invalid Gmail address. Please enter a valid Gmail ");
            } while (!Regex.IsMatch(email,pattern));

            do
            {
                Console.Write("Enter your password: ");
                password = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(password))
                    Console.WriteLine("Password cannot be empty.");
            } while (string.IsNullOrWhiteSpace(password));

            SaveUserDetails(name, age, password, phoneNumber);
            Console.WriteLine("User registration completed successfully!");
        }


        private void LoginUser()
        {
            string userPassword;
            Console.Write("Enter your email₹: ");
            userEmail = Console.ReadLine();
            Console.Write("Enter your password: ");
            userPassword = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userEmail) || string.IsNullOrWhiteSpace(userPassword))
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
            string connectionString = @"Server=LAPTOP-HS9AFKH4;Database=MutualFundSimulator;Trusted_Connection=True;TrustServerCertificate=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
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
                    command.Parameters.AddWithValue("@useremail", userEmail);
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
                    Console.WriteLine("2: SIP/Lumpsum");
                    Console.WriteLine("3: NestSipDate");
                    Console.WriteLine("4: Exit\n");
                  
                    Console.Write("Enter your Choice: ");
                    if (int.TryParse(Console.ReadLine(), out int input))
                    {
                        switch (input)
                        {
                            case 1: UserPortfolio(); break;
                            case 2: SipOrLumpsum(); break;
                            case 3: NestSipDate(); break;
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

        private void NestSipDate()
        {

        }

        private void SipOrLumpsum()
        {
            FundMenu();
        }

        private void UserPortfolio()
        {
            string connectionString = @"Server=LAPTOP-HS9AFKH4;Database=MutualFundSimulator;Trusted_Connection=True;TrustServerCertificate=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT fundname, quantity, investedamount, currentamount FROM UserPortfolio WHERE useremail = @useremail";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@useremail", userEmail);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("No investments found in your portfolio.");
                            return;
                        }
                        Console.WriteLine("\n--- User Portfolio ---");
                        Console.WriteLine("Fund Name\tQuantity\tInvested Amount\tCurrent Amount");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["fundName"]}\t{reader["quantity"]}\t{reader["investedAmount"]}\t{reader["currentAmount"]}");
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
                case "2": Invest(fundName); break;
                case "3": return;
                default: Console.WriteLine("Invalid choice. Please try again."); break;
            }
        }


        public void Invest(string fundName)
        {
            Console.WriteLine($"\n--- Investing in {fundName} ---");
           
            decimal pricePerUnit = GetFundPrice(fundName);
            Console.WriteLine($"Current price per unit: ₹ {pricePerUnit}");

            Console.Write("Enter the quantity you want to purchase: ");
            if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
            {
                decimal currentAmount = quantity * pricePerUnit;

                Console.WriteLine($"Total investment amount: ₹ {currentAmount}");
                Console.Write("Confirm investment? (yes/no): ");
                string confirmation = Console.ReadLine()?.ToLower();

                if (confirmation == "yes")
                {
                    SaveInvestment(fundName, quantity, currentAmount);
                    Console.WriteLine("Investment successful!");
                }
                else
                {
                    Console.WriteLine("Investment cancelled.");
                }
            }
            else
            {
                Console.WriteLine("Invalid quantity. Please try again.");
            }
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
        }


        private decimal GetFundPrice(string fundName)
        {
            string connectionString = @"Server=LAPTOP-HS9AFKH4;Database=MutualFundSimulator;Trusted_Connection=True;TrustServerCertificate=True;";
            decimal navValue = 0m;

            using (SqlConnection connection = new SqlConnection(connectionString))
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

        private void SaveInvestment(string fundName, int quantity, decimal currentAmount)
        {
            string connectionString = @"Server=LAPTOP-HS9AFKH4;Database=MutualFundSimulator;Trusted_Connection=True;TrustServerCertificate=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO UserPortfolio (useremail, fundname, quantity, investedamount, currentamount)" +
                               " VALUES (@useremail, @fundname, @quantity, @investedamount, @currentamount)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@useremail", userEmail);
                    command.Parameters.AddWithValue("@fundname", fundName);
                    command.Parameters.AddWithValue("@quantity", quantity);
                    command.Parameters.AddWithValue("@investedamount", currentAmount);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                            Console.WriteLine($"{userEmail} investment in {fundName} saved with {quantity} units and a total of ₹ {currentAmount}.");
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
            string connectionString = @"Server=LAPTOP-HS9AFKH4;Database=MutualFundSimulator;Trusted_Connection=True;TrustServerCertificate=True;";

            string[] funds =
            {
                "Large-Cap Equity Fund",
                "Mid-Cap Equity Fund",
                "Small-Cap Fund",
                "Sectoral/Thematic Fund",
                "Multi-Cap Fund"
            };

            Random random = new Random();

            using (SqlConnection connection = new SqlConnection(connectionString))
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
