﻿using System;
using System.Collections.Generic;

namespace HospitalManagement
{
    //This class represents a patient with basic details such as name, email, and city.
    public class Patient
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
         
    }

    // This class represents a doctor with a unique ID, name, and medical specialty.
    public class Doctor
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

    // This class represents an appointment linking a patient to a doctor at a specific date and time.
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
