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
            Lumpsum _lumpsumInvest = new Lumpsum(_fundBussines, _userLumpsum,_user);
            Sip _sipInvest = new Sip(_fundBussines, _userSip,_user);
            UserLogin _userLogin = new UserLogin(_repository, _user);

            MutualFundSimulatorUtility mutualFundSimulatorUtility = new MutualFundSimulatorUtility(_userLogin, _user, _fundBussines, _lumpsumInvest, _sipInvest);
            mutualFundSimulatorUtility.MainMenu();
        }
    }

}