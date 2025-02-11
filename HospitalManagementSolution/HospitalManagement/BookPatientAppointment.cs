using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HospitalManagement
{
    /*The BookPatientAppointment class have Bookappointment,Isholiday and IsvalidDate methods which
    help to Book an Appointment to patient*/

    internal class BookPatientAppointment
    {
        private HospitalDataStore _hospitalDataStore;

        public BookPatientAppointment(HospitalDataStore hospitalDataStore)
        {
            _hospitalDataStore= hospitalDataStore;
        }

        /// <summary>
        /// The IsHoliday method apply constraints to patient to avoid  booking an holidays slots
        /// </summary>
        /// <param name="inputDate"></param>
        /// <param name="hospitalDataStore"></param>
        /// <returns></returns>
        public static bool IsHoliday(string inputDate, HospitalDataStore hospitalDataStore)
        {
            DateTime date = DateTime.ParseExact(inputDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            //List<DateTime> holidays = new List<DateTime>
            //{
            //    new DateTime(2025, 01, 26), // Republic Day
            //    new DateTime(2025, 08, 15), // Independence Day
            //    new DateTime(2025, 10, 02), // Gandhi Jayanti
            //    new DateTime(2025, 12, 25), // Christmas
            //    new DateTime(2025, 05, 01)  // May Day (Labour Day)
            //};

            //This if condition make sure if day is saturday then it provide different time slots  
            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                hospitalDataStore._timeSlots = new List<string>
                {
                    "09:00 AM", "09:30 AM", "10:00 AM", "10:30 PM"
                };
            }
            //This if condition avoid booking slots in sunday
            if (date.DayOfWeek == DayOfWeek.Sunday /*|| holidays.Contains(date)*/)
            {
                Console.WriteLine("Appointments cannot be booked on Sundays.");
                return true;
            }

            return false;
        }

        /// <summary>
        /// The IsValidDate method make sure input dates are  valid 
        /// </summary>
        /// <param name="dateInput"></param>
        /// <returns></returns>
        public static bool IsValidDate(string dateInput)
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

        /// <summary>
        ///  The BookAppointment Book an appointment for a patient if patient email exist, selected doctor available,
        ///  have a slots for given date and patient dont have appointment with multiple docotrs at same time
        ///  then only it book an slot for patient
        /// </summary>
        public void BookAppointment()
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

                Console.Write("Enter appointment date (dd-MM-yyyy): ");
                string inputDate = Console.ReadLine();
                if(IsHoliday(inputDate,_hospitalDataStore))
                {
                    return;
                }
                
                if (!IsValidDate(inputDate))
                {
                    Console.WriteLine("Invalid date. Please enter a valid date in the format dd-MM-yyyy.");
                    return;
                }

                Console.WriteLine("Available doctors:");
                for (int i = 0; i < _hospitalDataStore._doctors.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. ID: {_hospitalDataStore._doctors[i].ID}, Name: {_hospitalDataStore._doctors[i].Name}, Specialty: {_hospitalDataStore._doctors[i].Specialty}");
                }

                Console.Write("Select a doctor (1-" + _hospitalDataStore._doctors.Count + "): ");
                int doctorChoice;
                if (!int.TryParse(Console.ReadLine(), out doctorChoice) || doctorChoice < 1 || doctorChoice > _hospitalDataStore._doctors.Count)
                {
                    Console.WriteLine("Invalid doctor choice. Try again.");
                    return;
                }
                Doctor selectedDoctor = _hospitalDataStore._doctors[doctorChoice - 1];
                DateTime currentTime = DateTime.Now;

                Console.WriteLine($"Available slots on {inputDate}:");
                List<string> availableSlots = new List<string>();

                for (int i = 0; i < _hospitalDataStore._timeSlots.Count; i++)
                {
                    DateTime slotTime = DateTime.ParseExact(_hospitalDataStore._timeSlots[i], "hh:mm tt", CultureInfo.InvariantCulture);

                    if (inputDate == DateTime.Today.ToString("dd-MM-yyyy") && slotTime.TimeOfDay <= currentTime.TimeOfDay)
                    {
                        continue;
                    }

                    bool isSlotTaken = false;
                    bool isSlotTakenByPatient = false;

                    if (_hospitalDataStore._doctorAppointmentsByDate.ContainsKey(inputDate))
                    {
                        List<Appointment> appointments = _hospitalDataStore._doctorAppointmentsByDate[inputDate];

                        foreach (Appointment appointment in appointments)
                        {
                            if (appointment.DoctorID == selectedDoctor.ID && appointment.Time == _hospitalDataStore._timeSlots[i])
                            {
                                isSlotTaken = true;
                            }
                            if (appointment.PatientEmail == patientEmail && appointment.Time == _hospitalDataStore._timeSlots[i])
                            {
                                isSlotTakenByPatient = true;
                            }
                        }
                    }

                    if (!isSlotTaken && !isSlotTakenByPatient)
                    {
                        availableSlots.Add(_hospitalDataStore._timeSlots[i]);
                        Console.WriteLine($"{availableSlots.Count}. {_hospitalDataStore._timeSlots[i]}");
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
                if (!_hospitalDataStore._doctorAppointmentsByDate.ContainsKey(inputDate))
                {
                    _hospitalDataStore._doctorAppointmentsByDate[inputDate] = new List<Appointment>();
                }

                _hospitalDataStore._doctorAppointmentsByDate[inputDate].Add(new Appointment(inputDate, selectedTime, patientEmail, selectedDoctor.ID, selectedDoctor.Name, selectedDoctor.Specialty));
                Console.WriteLine($"Appointment booked for {patientEmail} with {selectedDoctor.Name} (Specialty: {selectedDoctor.Specialty}) on {inputDate} at {selectedTime}.");
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
