using System;

namespace HospitalManagement
{
    class Program
    {
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
