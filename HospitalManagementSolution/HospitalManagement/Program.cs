using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;


namespace HospitalManagement
{

    class Program
    {
        public static void Main()
        {
            HospitalManagementUtility hospitalManagementUtility = new HospitalManagementUtility();
            hospitalManagementUtility.MainMenu();
        }
    }
}