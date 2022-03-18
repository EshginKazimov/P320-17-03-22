using P320.AuthenticationService.Models;

namespace P320.AuthenticationService.Contracts
{
    public interface IAuthService
    {
        string GetToken(CredentialModel credentialModel);
    }
}
