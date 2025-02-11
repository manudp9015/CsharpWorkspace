using System;
using System.Collections.Generic;

namespace HospitalManagement
{
    public class Patient
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get
            {
                return City;
            }
             set
            {
                if(value=="bangalore")
                {

                }
            } }
              
    }

    public  class Doctor
    {
        public string ID { get; }
        public string Name { get; }
        public string Specialty { get; }
       
        public Doctor(string id, string name, string specialty)
        {
            ID = id;
            Name = name;
            Specialty = specialty;
        }
    }

    public class Appointment
    {
        public string Date { get; }
        public string Time { get; }
        public string PatientEmail { get; set; }
        public string DoctorID { get; }
        public string DoctorName { get; }
        public string DoctorSpecialty { get; }

        public Appointment(string date, string time, string patientEmail, string doctorID, string doctorName, string doctorSpecialty)
        {
            Date = date;
            Time = time;
            PatientEmail = patientEmail;
            DoctorID = doctorID;
            DoctorName = doctorName;
            DoctorSpecialty = doctorSpecialty;
        }
        

    }
}
