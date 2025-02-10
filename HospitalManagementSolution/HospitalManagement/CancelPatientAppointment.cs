using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement
{
    internal class CancelPatientAppointment
    {
        private HospitalDataStore _hospitalDataStore;
        public CancelPatientAppointment(HospitalDataStore hospitalDataStore)
        {
            _hospitalDataStore= hospitalDataStore;
        }
        public  void CancelAppointment()
        {
            try
            {
                Console.Write("Enter your email: ");
                string patientEmail = Console.ReadLine();
                if (!_hospitalDataStore._patientdictionary.ContainsKey(patientEmail))
                {
                    Console.WriteLine($"Your Patient Email: {patientEmail} is invalid or not registered yet.");
                    return;
                }
                Console.WriteLine("Available doctors:");
                for (int i = 0; i < _hospitalDataStore._doctors.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Name: {_hospitalDataStore._doctors[i].Name}, Specialty: {_hospitalDataStore._doctors[i].Specialty}");
                }
                Console.Write("Select a doctor (1-" + _hospitalDataStore._doctors.Count + "): ");
                int doctorChoice;
                if (!int.TryParse(Console.ReadLine(), out doctorChoice) || doctorChoice < 1 || doctorChoice > _hospitalDataStore._doctors.Count)
                {
                    Console.WriteLine("Invalid doctor choice. Try again.");
                    return;
                }
                Doctor selectedDoctor = _hospitalDataStore._doctors[doctorChoice - 1];
                bool appointmentFound = false;
                foreach (var dateEntry in _hospitalDataStore._doctorAppointmentsByDate)
                {
                    List<Appointment> appointments = dateEntry.Value;
                    List<Appointment> patientAppointments = new List<Appointment>();
                    for (int i = 0; i < appointments.Count; i++)
                    {
                        if (appointments[i].PatientEmail == patientEmail && appointments[i].DoctorName == selectedDoctor.Name)                      
                            patientAppointments.Add(appointments[i]);    
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

