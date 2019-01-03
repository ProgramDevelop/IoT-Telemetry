namespace Telemetry.Web.Services.Auth
{
    public interface IAuthService
    {
        bool IsUserExist(string login);
        bool CheckCredentials(string login, string password);
        bool Register(string login, string password);
    }
}
