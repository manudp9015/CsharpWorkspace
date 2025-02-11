using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HospitalManagement
{
    //The PatientRegistration class provide a method for patient registration
    internal class PatientRegistration
    {
        private HospitalDataStore _hospitalDataStore;
        public PatientRegistration(HospitalDataStore hospitalDataStore)
        {
            _hospitalDataStore= hospitalDataStore;
        }

        /// <summary>
        /// By using this method RegisterNewPatient we can register the new patient to hospital and store in _patientdictionary
        /// </summary>
        public void RegisterNewPatient()
        {
            try
            {
                Patient patient = new Patient();
                Console.Write("Enter Patient Name: ");
                patient.Name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(patient.Name))
                {
                    Console.WriteLine("Patient Name cannot be empty.");
                    return;
                }

                Console.Write("Enter Patient CITY: ");
                patient.City = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(patient.City))
                {
                    Console.WriteLine("Patient City cannot be empty.");
                    return;
                }
                Console.Write("Enter Patient Email: ");
                patient.Email = Console.ReadLine();

                string email = patient.Email;
                string pattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
                //ManuD@gmail.com
                if (Regex.IsMatch(email, pattern))
                {
                    bool isAdded = _hospitalDataStore._patientdictionary.TryAdd(patient.Email, patient);

                    if (isAdded == false)
                        Console.WriteLine("Given Email Already Exist.");
                    else
                    {

                        Console.WriteLine("Patient Registration Completed.");
                    }
                }
                else Console.WriteLine("Invalid Gmail address.");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
