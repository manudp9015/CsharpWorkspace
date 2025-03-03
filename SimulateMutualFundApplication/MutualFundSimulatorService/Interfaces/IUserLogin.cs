namespace MutualFundSimulatorService.Interfaces
{
    public interface IUserLogin
    {
        bool LoginUser(string email, string password);
    }
}