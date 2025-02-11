using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement
{
    //The DoctorAvailabilityChecker class check available doctor and there booked slot and display 
    internal class DoctorAvailabilityChecker
    {

        private HospitalDataStore _hospitalDataStore;
        public DoctorAvailabilityChecker(HospitalDataStore hospitalDataStore)
        {
            _hospitalDataStore = hospitalDataStore;
        }

        /// <summary>
        /// The CheckDoctorAvailability check docotr and booked slot for a patient with respect to doctor and display
        /// </summary>
        public void CheckDoctorAvailability()
        {
            try
            { 
                foreach (var doctor in _hospitalDataStore._doctors)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Doctor ID: {doctor.ID}, Name: {doctor.Name}, Specialty: {doctor.Specialty}");

                    bool slotFound = false;

                    foreach (var dateEntry in _hospitalDataStore._doctorAppointmentsByDate)
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
    }
}
