using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Sales.WEB.Auth
{
    public class AuthenticationProviderTest : AuthenticationStateProvider
    {
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var anonimous = new ClaimsIdentity();
            var rolaxUser = new ClaimsIdentity(new List<Claim>
                {
                    new Claim("FirstName", "David"),
                    new Claim("LastName", "Rolax"),
                    new Claim(ClaimTypes.Name, "rolax@yopmail.com"),
                    new Claim(ClaimTypes.Role, "Admin")
                },
                authenticationType: "test");
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(rolaxUser)));

        }
    }
}
