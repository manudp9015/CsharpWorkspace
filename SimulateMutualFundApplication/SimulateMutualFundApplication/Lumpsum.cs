using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MutualFundSimulatorApplication
{
    internal class Lumpsum
    {
        private MutualFundBusiness _fundBussines;
        private UserLumpsumInvest _userLumpsum;
        public Lumpsum(MutualFundBusiness fundBussines, UserLumpsumInvest userLumpsum)
        {
            _fundBussines = fundBussines;
            _userLumpsum = userLumpsum;
        }
        public void LumpSumInvest(string fundname)
        {
            try
            {
                _userLumpsum.fundName=fundname;
                Console.WriteLine($"\n--- Investing in {_userLumpsum.fundName} ---");

                decimal pricePerUnit = _fundBussines.GetFundPrice(_userLumpsum.fundName);
                Console.WriteLine($"Current price per unit: ₹ {pricePerUnit}");

                Console.Write("Enter the lump sum amount you want to invest (minimum ₹5000): ");
                if (decimal.TryParse(Console.ReadLine(), out decimal lumpSumAmount) && lumpSumAmount >= 5000)
                {
                    _userLumpsum.quantity = Math.Round(lumpSumAmount / pricePerUnit, 2);

                    Console.WriteLine($"Total units purchased: {_userLumpsum.quantity} units at ₹ {pricePerUnit} per unit.");
                    Console.WriteLine($"Total investment amount: ₹ {lumpSumAmount}");
                    _userLumpsum.investedAmount = lumpSumAmount;
                    Console.Write("Confirm lump sum investment? (yes/no): ");
                    string confirmation = Console.ReadLine()?.ToLower();

                    if (confirmation == "yes")
                    {
                        _userLumpsum.durationInMonths = 12;
                        _userLumpsum.lumpsumStartDate = DateTime.Today;
                        _userLumpsum.lumpsumEndDate = _userLumpsum.lumpsumStartDate.AddMonths(_userLumpsum.durationInMonths);
                        
                        _fundBussines.SaveLumpsumInvest();
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
    }
}
