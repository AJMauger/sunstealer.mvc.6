# Sunstealer.MVC aspcore 6.0

Sandpit aspcore 6.0 API server with Identity and Swagger.

An Identity Server 6 is required with:

using Duende.IdentityServer.Models;

namespace Sunstealer.IdentityServer.Models;

public class Configuration {

    public static IEnumerable<Duende.IdentityServer.Models.ApiScope> ApiScopes =>
        new List<Duende.IdentityServer.Models.ApiScope>
        { 
            new Duende.IdentityServer.Models.ApiScope("swagger", "Swagger") 
        };

    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<Duende.IdentityServer.Models.Client> Clients =>
        new List<Duende.IdentityServer.Models.Client>
        {
            // ajm: authorization code => users
            new Duende.IdentityServer.Models.Client()
            {
                AllowedGrantTypes = Duende.IdentityServer.Models.GrantTypes.Code,
                AllowedScopes = {
                    Duende.IdentityServer.IdentityServerConstants.StandardScopes.OpenId,
                    Duende.IdentityServer.IdentityServerConstants.StandardScopes.Profile,
                    "sunstealer"
                },
                ClientId = "clientIdSunstealer",
                ClientSecrets = {
                    new Duende.IdentityServer.Models.Secret("secret".Sha256())
                },
                PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },
                RedirectUris = { "https://localhost:5001/signin-oidc" }
            },
            // ajm: client credentials => server
            new Duende.IdentityServer.Models.Client
            {
                AllowedCorsOrigins = {"https://localhost:5001"},
                // !interactive user => use the ClientId/ClientSecrets for authentication
                AllowedGrantTypes = Duende.IdentityServer.Models.GrantTypes.ClientCredentials,
                // scopes that client has access to
                AllowedScopes = { "swagger" },
                Claims = { new Duende.IdentityServer.Models.ClientClaim("role", "Admin") },   // ajm: policy.RequireClaim("role", "Admin")
                ClientClaimsPrefix = "",
                ClientId = "clientIdSwagger",
                ClientSecrets =
                {
                    new Duende.IdentityServer.Models.Secret("secret".Sha256())
                },
                RedirectUris = { "https://localhost:5001/Swagger/oauth2-redirect.html" },
            }
        };


    public static System.Collections.Generic.List<Duende.IdentityServer.Test.TestUser> Users =
        new System.Collections.Generic.List<Duende.IdentityServer.Test.TestUser>()
        {
            new Duende.IdentityServer.Test.TestUser()
            {
                SubjectId = "AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE",
                Username = "adam",
                Password = "password",
                Claims = new[]
                {
                    new System.Security.Claims.Claim(IdentityModel.JwtClaimTypes.Email, "ajm@mail.com"),
                    new System.Security.Claims.Claim(IdentityModel.JwtClaimTypes.Name, "Adam"),
                    new System.Security.Claims.Claim(IdentityModel.JwtClaimTypes.Role, "Admin"),    
                }
            }
        };
}
