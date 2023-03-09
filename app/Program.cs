var builder = WebApplication.CreateBuilder(args);

// ajm: sandpit
var AppName = "sunstealer.mvc";

builder.Services.AddHttpClient();

// ajm: identity
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie("Cookies")
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = "https://localhost:9001";
    options.ClientId = "clientIdSunstealer";
    options.ClientSecret = "secret";
    options.UsePkce = true;
    options.ResponseType = "code";
    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.SaveTokens = true;
    options.SignedOutRedirectUri = "https://localhost:5001/signout-callback-oidc";
})

.AddJwtBearer("Bearer", options =>
{
    options.Authority = "https://localhost:9001";
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateAudience = false
    };
});

// ajm: identity
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("role", "Admin"));
});

// ajm: swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { 
        Title = "Protected API",
        Version = "v1"
    });

    // ajm: identity
    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.OAuth2,
        Flows = new Microsoft.OpenApi.Models.OpenApiOAuthFlows
        {
            ClientCredentials = new Microsoft.OpenApi.Models.OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("https://localhost:9001/connect/authorize"),
                TokenUrl = new Uri("https://localhost:9001/connect/token"),
                Scopes = new Dictionary<string, string> {
                  {"swagger", AppName}
                }
            }
        }
    });

    options.OperationFilter<sunstealer.mvc.AuthorizationOptionsFilter>();
});

builder.Services.AddSingleton<sunstealer.mvc.Services.IApplication, sunstealer.mvc.Services.Application>();
builder.Services.AddHostedService<sunstealer.mvc.Services.Background>();
builder.Services.AddScoped<sunstealer.mvc.Services.IGeneric, sunstealer.mvc.Services.Generic>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// ajm: swagger
app.UseSwagger(options => { });
app.UseSwaggerUI(options => {
    options.RoutePrefix = "Swagger";
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");

    options.OAuthClientId("clientIdSwagger");
    options.OAuthAppName(AppName);
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ajm: identity
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default", pattern: "{controller=Home}/{action=Index}/{id?}"
).RequireAuthorization();

app.Run();

