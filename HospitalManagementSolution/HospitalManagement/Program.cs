using System;

// under this namespace only every class members are present which required in this project(Hospital Management System);
namespace HospitalManagement
{
    class Program
    {
        //this is only main method in my project , execution starts from here 
        public static void Main()
        {
       
            HospitalDataStore hospitalDataStore = new HospitalDataStore();
            DoctorAvailabilityChecker doctorAvailabilityChecker = new DoctorAvailabilityChecker(hospitalDataStore);
            PatientRegistration patientRegistration = new PatientRegistration(hospitalDataStore);
            BookPatientAppointment bookPatientAppointment = new BookPatientAppointment(hospitalDataStore);
            CancelPatientAppointment cancelPatientAppointment = new CancelPatientAppointment(hospitalDataStore);

            
            HospitalManagementUtility hospitalManagementUtility = new HospitalManagementUtility(
                hospitalDataStore,
                doctorAvailabilityChecker,
                patientRegistration,
                bookPatientAppointment,
                cancelPatientAppointment
            );

            
            hospitalManagementUtility.MainMenu();
        }
    }
}
