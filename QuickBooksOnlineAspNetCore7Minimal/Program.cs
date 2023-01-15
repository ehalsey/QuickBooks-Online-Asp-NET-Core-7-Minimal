using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services
    .AddAuthentication("OpenIdConnect")
    .AddCookie()
    .AddOpenIdConnect(options => {
        options.SignInScheme = "Cookies";
        options.ClientId = builder.Configuration["QuickBooksOnline:ClientId"];
        options.ClientSecret = builder.Configuration["QuickBooksOnline:ClientSecret"];
        options.MetadataAddress = builder.Configuration["QuickBooksOnline:DiscoveryEndpoint"];
        options.ResponseType = OpenIdConnectResponseType.Code;
        options.SignedOutRedirectUri = "/";
        options.ProtocolValidator.RequireNonce = false;
        options.Scope.Add("com.intuit.quickbooks.accounting");
        options.Scope.Add("openid");
        options.Scope.Add("email");
        options.Scope.Add("profile");
        options.Scope.Add("address");
        options.Scope.Add("phone");
        options.SaveTokens = true;
        options.UseTokenLifetime = true;
        options.MapInboundClaims = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.ClaimActions.MapUniqueJsonKey("realmid", "realmid");
        options.ClaimActions.MapUniqueJsonKey(ClaimTypes.Name, "email");
    });
builder.Services.AddAuthorization();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
