using MutualFundSimulatorApplication.Business;
using MutualFundSimulatorApplication.Model;
using MutualFundSimulatorApplication.Repository;
using System;

namespace MutualFundSimulatorApplication
{
    class Program
    {
        static void Main()
        {
            User _user = new User();
            UserSipInvest _userSip = new UserSipInvest();
            UserLumpsumInvest _userLumpsum = new UserLumpsumInvest();
            MutualFundRepository _repository = new MutualFundRepository(_user, _userLumpsum, _userSip);
            MutualFundBusiness _fundBussines = new MutualFundBusiness(_repository);
            Lumpsum _lumpsumInvest = new Lumpsum(_fundBussines, _userLumpsum, _user);
            Sip _sipInvest = new Sip(_fundBussines, _userSip, _user);
            UserLogin _userLogin = new UserLogin(_repository, _user);

            // Set the current date  for testing purpose, To check sip works correctly or not.
            User.CurrentDate = new DateTime(2025, 02, 27);

            // Create database tables 
            DBPatch dBPatch = new DBPatch(_repository.ConnectionString);
            dBPatch.CreateTablesForMutualFunds();

            MutualFundSimulatorUtility mutualFundSimulatorUtility = new MutualFundSimulatorUtility(_userLogin, _user, _fundBussines, _lumpsumInvest, _sipInvest);
            mutualFundSimulatorUtility.MainMenu();
        }
    }
}