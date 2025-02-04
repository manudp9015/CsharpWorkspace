using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement
{
    //this class contain all nessesary maethods for hospital management system
    internal class HospitalManagementUtility
    {
        
        Dictionary<string, Patient> patientdictionary = new Dictionary<string, Patient>();
        Dictionary<string, string> doctorappointment = new Dictionary<string, string>();
        Dictionary<string, string> doctortomorrowappointment = new Dictionary<string, string>();


        public void MainMenu()
        {
            
            Adminmethod();
            while (true)
                {
                    Console.WriteLine("Main Menu");
                    Console.WriteLine("Enter 1: Doctor Availability");
                    Console.WriteLine("Enter 2: New patient Registeration");
                    Console.WriteLine("Enter 3: Give Appointment to patient");
                    Console.WriteLine("Enter 4: Cancel Appointment for a patient");
                    Console.WriteLine("Enter 5: Exit");
                    Console.WriteLine();

                    Console.Write("Enter your Choice: ");
                    if (int.TryParse(Console.ReadLine(), out int input))
                    {
                        switch (input)
                        {
                            case 1: DoctorAvailability(); break;
                            case 2: NewPatientRegistration(); break;
                            case 3: GiveAppointment(); break;
                            case 4: CancelAppointment(); break;
                            case 5: Console.WriteLine("exiting..."); return;
                            default: Console.WriteLine("Invalid Input. Give Valid Input"); break;
                        }
                    }
                    else Console.WriteLine("Invalid Input. Give Valid Input");
                }
            
        }

        // By using this method we can cancel the appointment of patient
        private void CancelAppointment()
        {
            Console.Write("Enter the Email of patient to Cancel Appointment");
            string s=Console.ReadLine();
            if(doctorappointment.ContainsKey(s))
            {  
                doctorappointment.Remove(s);
                Console.WriteLine($"patient {s} Appointment is Cancelled Successfully.");
            }
            else
            {
                Console.WriteLine($"patient {s} not Exist.");
            }
        }

        //By using this method we can assign oppointment to Patient
        private void GiveAppointment()
        {
            Console.Write(" Enter a Patient Email: ");
            string s = Console.ReadLine();
            if (patientdictionary.ContainsKey(s))
            {
                
                if (doctorappointment.Count<10)
                {
                  
                   bool answer= doctorappointment.TryAdd(s, "Doctor1");

                    if(answer==false) Console.WriteLine("Given Email Already Exist.");
             
                    else Console.WriteLine("Patient have Appointment with Doctor1");
                }
                else
                {
                    if (doctortomorrowappointment.Count < 10)
                    {

                        bool answer = doctortomorrowappointment.TryAdd(s, "Doctor1");

                        if (answer == false) Console.WriteLine("Given Email Already Exist.");

                        else Console.WriteLine("Patient have Appointment with Doctor1 Tomorrow");
                    }
                    else
                    {
                        Console.WriteLine("Doctor1 has Busy Schedule Today and Tomorrow, Please Revisit Day After Tomorrow.");
                    }
                    
                }
               
            }
            else
            {
                Console.WriteLine("Given Email is Not Registered, Please Register as New patient");
            }
        }

        //By using this method we can make registration of new user
        private void NewPatientRegistration()
        {
     
            Patient patient= new Patient();
            Console.Write("Enter Patient Name: ");
            patient.Name = Console.ReadLine();

            Console.Write("Enter Patient Email: ");
            patient.email = Console.ReadLine();

            Console.Write("Enter Patient CITY: ");
            patient.city = Console.ReadLine();

            

            bool answer = this.patientdictionary.TryAdd(patient.email, patient);

            if (answer == false) Console.WriteLine("Given Email Already Exist.");

            else Console.WriteLine("Patient have Appointment with Doctor1");

            foreach (KeyValuePair<string, Patient> entry in this.patientdictionary)
            {
                Console.WriteLine($"patient Email:{entry.Key} Name:{entry.Value.Name} City:{entry.Value.city}");
                Console.WriteLine();
            }
           
        }

        //By using this method we can check how many Patient sheduled 
        private void DoctorAvailability()
        {

            int count = doctorappointment.Count;
            Console.WriteLine("Doctor1 Have Schedule for : " + count +"Patient.");

        }

        //this provide login for admin using id and password
        private void Adminmethod()
        {

            while (true)
            {

                Console.Write("Enter Admin UserID: ");
                var id = Console.ReadLine();
                if (id == "123")
                {
                    Console.Write("Enter Admin User Password: ");
                    var pass = Console.ReadLine();

                    if (pass == "123")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("You Enter Wrong Password.");
                    }
                }
                else
                {
                    Console.WriteLine("You Enter Wrong UserID.");

                }

            }
        }

    }

    //patient class for creating user defined type
    class Patient
    {
        public string Name { get; set; }
        public string email { get; set; }
        public string city { get; set; }
    }

}
