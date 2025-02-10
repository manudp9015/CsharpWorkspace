using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using static HospitalManagement.Appointment;


namespace HospitalManagement
{
    
    public  class HospitalDataStore
    {
        public  Dictionary<string, List<Appointment>> _doctorAppointmentsByDate = new Dictionary<string, List<Appointment>>();
        public  List<Doctor> _doctors = new List<Doctor>();
        public  List<string> _timeSlots = new List<string>();
        public  Dictionary<string, Patient> _patientdictionary = new Dictionary<string, Patient>();

    }

    internal class HospitalManagementUtility
    {

        private HospitalDataStore _hospitalDataStore;
        private DoctorAvailabilityChecker _doctorAvailabilityChecker;
        private PatientRegistration _patientRegistration;
        private BookPatientAppointment _bookPatientAppointment;
        private CancelPatientAppointment _cancelPatientAppointment;

        public HospitalManagementUtility(
            HospitalDataStore hospitalDataStore,
            DoctorAvailabilityChecker doctorAvailabilityChecker,
            PatientRegistration patientRegistration,
            BookPatientAppointment bookPatientAppointment,
            CancelPatientAppointment cancelPatientAppointment)
        {
            _hospitalDataStore = hospitalDataStore;
            _doctorAvailabilityChecker = doctorAvailabilityChecker;
            _patientRegistration = patientRegistration;
            _bookPatientAppointment = bookPatientAppointment;
            _cancelPatientAppointment = cancelPatientAppointment;
        }

        private void InitializeData()
        { 
            try
            {
                _hospitalDataStore._timeSlots.Add("10:00 AM");
                _hospitalDataStore._timeSlots.Add("10:30 AM");
                _hospitalDataStore._timeSlots.Add("11:00 AM");
                _hospitalDataStore._timeSlots.Add("11:30 AM");
                _hospitalDataStore._timeSlots.Add("12:00 PM");
                _hospitalDataStore._timeSlots.Add("12:30 PM");
                _hospitalDataStore._timeSlots.Add("02:00 PM");
                _hospitalDataStore._timeSlots.Add("02:30 PM");
                _hospitalDataStore._timeSlots.Add("03:00 PM");
                _hospitalDataStore._timeSlots.Add("03:30 PM");

                _hospitalDataStore._doctors.Add(new Doctor("D1", "Krishna", "Orthopedic"));
                _hospitalDataStore._doctors.Add(new Doctor("D2", "Sagar", "Neurologist"));
                _hospitalDataStore._doctors.Add(new Doctor("D3", "Bharath", "Orthopedic"));
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void MainMenu()
        {
            try
            {
                InitializeData();
                AdminLogin();
                while (true)
                {
                    Console.WriteLine();
                    Console.WriteLine("Main Menu");
                    Console.WriteLine("Enter 1: Doctor Availability");
                    Console.WriteLine("Enter 2: Registration of New patient");
                    Console.WriteLine("Enter 3: Give Appointment to Patient");
                    Console.WriteLine("Enter 4: Cancel Appointment for a Patient");
                    Console.WriteLine("Enter 5: Exit");
                    Console.WriteLine();

                    Console.Write("Enter your Choice: ");
                    if (int.TryParse(Console.ReadLine(), out int input))
                    {
                        switch (input)
                        {
                            case 1: _doctorAvailabilityChecker.CheckDoctorAvailability(); break;
                            case 2: _patientRegistration.RegisterNewPatient(); break;
                            case 3: _bookPatientAppointment.BookAppointment(); break;
                            case 4: _cancelPatientAppointment.CancelAppointment(); break;
                            case 5: Console.WriteLine("exiting..."); return;
                            default: Console.WriteLine("Invalid Input. Give Valid Input"); break;
                        }
                    }
                    else Console.WriteLine("Invalid Input. Give Valid Input");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void AdminLogin()
        {
            try
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
                            break;
                        else
                            Console.WriteLine("You Enter Incorrect Password.");
                    }
                    else
                        Console.WriteLine("You Enter Incorrect UserID.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
