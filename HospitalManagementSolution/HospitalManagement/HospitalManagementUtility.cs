using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using static HospitalManagement.Appointment;


namespace HospitalManagement
{
    internal class HospitalManagementUtility
    {

        Dictionary<string, Patient> _patientdictionary = new Dictionary<string, Patient>();
        List<string> _timeSlots = new List<string>();
        List<Doctor> _doctors = new List<Doctor>();
        Dictionary<string, List<Appointment>> _doctorAppointmentsByDate = new Dictionary<string, List<Appointment>>();
        private void InitializeData()
        {
            try
            {
                _timeSlots.Add("10:00 AM");
                _timeSlots.Add("10:30 AM");
                _timeSlots.Add("11:00 AM");
                _timeSlots.Add("11:30 AM");
                _timeSlots.Add("12:00 PM");
                _timeSlots.Add("12:30 PM");
                _timeSlots.Add("02:00 PM");
                _timeSlots.Add("02:30 PM");
                _timeSlots.Add("03:00 PM");
                _timeSlots.Add("03:30 PM");


                _doctors.Add(new Doctor("D1", "Krishna", "Orthopedic"));
                _doctors.Add(new Doctor("D2", "Sagar", "Neurologist"));
                _doctors.Add(new Doctor("D3", "Bharath", "Orthopedic"));
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
                            case 1: CheckDoctorAvailability(); break;
                            case 2: RegisterNewPatient(); break;
                            case 3: BookAppointment(); break;
                            case 4: CancelAppointment(); break;
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
        private void CheckDoctorAvailability()
        {
            try
            {
                foreach (var doctor in _doctors)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Doctor ID: {doctor.ID}, Name: {doctor.Name}, Specialty: {doctor.Specialty}");

                    bool slotFound = false;

                    foreach (var dateEntry in _doctorAppointmentsByDate)
                    {
                        List<Appointment> appointments = dateEntry.Value;

                        for (int i = 0; i < appointments.Count; i++)
                        {
                            if (appointments[i].DoctorName == doctor.Name)
                            {
                                if (!slotFound)
                                {
                                    Console.WriteLine("Booked Slots:");
                                    slotFound = true;
                                }
                                Console.WriteLine($"{i + 1}. {dateEntry.Key} at {appointments[i].Time} - Patient: {appointments[i].PatientEmail}");
                            }
                        }
                    }
                    if (!slotFound)
                    {
                        Console.WriteLine("No Slots are Booked Yet.");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void RegisterNewPatient()
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

                if (Regex.IsMatch(email, pattern))
                {
                    bool isAdded = _patientdictionary.TryAdd(patient.Email, patient);

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
        static bool IsValidDate(string dateInput)
        {
            try
            {
                DateTime tempDate;
                if (DateTime.TryParseExact(dateInput, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tempDate))
                {
                    if (tempDate < DateTime.Today)
                    {
                        Console.WriteLine("Cannot book an appointment for a past date.");
                        return false;
                    }
                    return true;
                }
                else
                {
                    Console.WriteLine("Invalid date format. Please enter the date in dd-MM-yyyy format.");
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        void BookAppointment()
        {
            try
            {

                Console.Write("Enter your email: ");
                string patientEmail = Console.ReadLine();
                if (!_patientdictionary.ContainsKey(patientEmail))
                {
                    Console.WriteLine($"Your Pateint Email:{patientEmail} not Registered Yet.");
                    return;
                }

                Console.Write("Enter appointment date (dd-MM-yyyy): ");
                string inputDate = Console.ReadLine();

                if (!IsValidDate(inputDate))
                {
                    Console.WriteLine("Invalid date. Please enter a valid date in the format dd-MM-yyyy.");
                    return;
                }

                Console.WriteLine("Available doctors:");
                for (int i = 0; i < _doctors.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. ID: {_doctors[i].ID}, Name: {_doctors[i].Name}, Specialty: {_doctors[i].Specialty}");
                }

                Console.Write("Select a doctor (1-" + _doctors.Count + "): ");
                int doctorChoice;
                if (!int.TryParse(Console.ReadLine(), out doctorChoice) || doctorChoice < 1 || doctorChoice > _doctors.Count)
                {
                    Console.WriteLine("Invalid doctor choice. Try again.");
                    return;
                }

                Doctor selectedDoctor = _doctors[doctorChoice - 1];

                Console.WriteLine($"Available slots on {inputDate}:");
                List<string> availableSlots = new List<string>();

                for (int i = 0; i < _timeSlots.Count; i++)
                {
                    bool isSlotTaken = false;

                    if (_doctorAppointmentsByDate.TryGetValue(inputDate, out List<Appointment> appointments))
                    {
                        foreach (Appointment appointment in appointments)
                        {
                            if (appointment.DoctorID == selectedDoctor.ID && appointment.Time == _timeSlots[i])
                            {
                                isSlotTaken = true;
                                break;
                            }
                        }
                    }
                    if (!isSlotTaken)
                    {
                        availableSlots.Add(_timeSlots[i]);
                        Console.WriteLine($"{availableSlots.Count}. {_timeSlots[i]}");
                    }
                }
                if (availableSlots.Count == 0)
                {
                    Console.WriteLine("No available slots for the selected date.");
                    return;
                }

                Console.Write("Select a slot (1-" + availableSlots.Count + "): ");
                int slotChoice;
                if (!int.TryParse(Console.ReadLine(), out slotChoice) || slotChoice < 1 || slotChoice > availableSlots.Count)
                {
                    Console.WriteLine("Invalid slot choice. Try again.");
                    return;
                }
                string selectedTime = availableSlots[slotChoice - 1];
                if (!_doctorAppointmentsByDate.ContainsKey(inputDate))
                {
                    _doctorAppointmentsByDate[inputDate] = new List<Appointment>();
                }
                _doctorAppointmentsByDate[inputDate].Add(new Appointment(inputDate, selectedTime, patientEmail, selectedDoctor.ID, selectedDoctor.Name, selectedDoctor.Specialty));
                Console.WriteLine($"Appointment booked for {patientEmail} with {selectedDoctor.Name} (Specialty: {selectedDoctor.Specialty}) on {inputDate} at {selectedTime}.");
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void CancelAppointment()
        {
            try
            {
                Console.Write("Enter your email: ");
                string patientEmail = Console.ReadLine();
                if (!_patientdictionary.ContainsKey(patientEmail))
                {
                    Console.WriteLine($"Your Patient Email: {patientEmail} is not registered yet.");
                    return;
                }
                Console.WriteLine("Available doctors:");
                for (int i = 0; i < _doctors.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Name: {_doctors[i].Name}, Specialty: {_doctors[i].Specialty}");
                }
                Console.Write("Select a doctor (1-" + _doctors.Count + "): ");
                int doctorChoice;
                if (!int.TryParse(Console.ReadLine(), out doctorChoice) || doctorChoice < 1 || doctorChoice > _doctors.Count)
                {
                    Console.WriteLine("Invalid doctor choice. Try again.");
                    return;
                }
                Doctor selectedDoctor = _doctors[doctorChoice - 1];
                bool appointmentFound = false;

                foreach (var dateEntry in _doctorAppointmentsByDate)
                {
                    List<Appointment> appointments = dateEntry.Value;
                    List<Appointment> patientAppointments = new List<Appointment>();

                    for (int i = 0; i < appointments.Count; i++)
                    {
                        if (appointments[i].PatientEmail == patientEmail && appointments[i].DoctorName == selectedDoctor.Name)
                        {
                            patientAppointments.Add(appointments[i]);
                        }
                    }
                    if (patientAppointments.Count > 0)
                    {
                        appointmentFound = true;
                        Console.WriteLine("Your booked slots:");
                        for (int i = 0; i < patientAppointments.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {dateEntry.Key} at {patientAppointments[i].Time}");
                        }
                        Console.Write("Enter the number of the slot you want to cancel: ");
                        int slotChoice;
                        if (int.TryParse(Console.ReadLine(), out slotChoice) && slotChoice >= 1 && slotChoice <= patientAppointments.Count)
                        {
                            Appointment appointmentToCancel = patientAppointments[slotChoice - 1];
                            appointments.Remove(appointmentToCancel);
                            Console.WriteLine($"Appointment on {dateEntry.Key} at {appointmentToCancel.Time} with {selectedDoctor.Name} has been canceled.");
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Invalid slot choice.");
                            return;
                        }
                    }
                }
                if (!appointmentFound)
                {
                    Console.WriteLine("No appointments found for the given email and selected doctor.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
