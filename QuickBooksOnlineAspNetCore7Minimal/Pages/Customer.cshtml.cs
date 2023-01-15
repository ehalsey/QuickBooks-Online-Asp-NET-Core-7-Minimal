using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;

namespace QuickBooksOnlineAspNetCore7Minimal.Pages
{
    [Authorize]
    public class CustomerModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly string _qboApiBaseUrl = string.Empty;
        private readonly string _qboMinorVersion = string.Empty;
        public string CreateCustomerResponse { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;


        public CustomerModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _qboApiBaseUrl = _configuration["QuickBooksOnline:AccountingEndpoint"] ?? "";
            _qboMinorVersion = _configuration["QuickBooksOnline:MinorVersion"] ?? "65";
        }
        
        public void OnGet()
        {
            //var endpoint = _configuration["QuickBooksOnline:AccountingEndpoint"];
            var companyIdClaim = this.User.Claims.FirstOrDefault(c => c.Type == "realmid");
            if (companyIdClaim == null)
            {
                CompanyId = "Missing realmid claim";
            }
            else
            {
                CompanyId = companyIdClaim.Value;
            }

        }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = await this.HttpContext.GetTokenAsync(OpenIdConnectDefaults.AuthenticationScheme, "access_token");

            using (var httpClient = new HttpClient())
            {
                var jsonData = """{ "FullyQualifiedName": "Erics Groceries", "PrimaryEmailAddr": { "Address": "jdrew@myemail.com" }, "DisplayName": "Erics Groceries", "Suffix": "Jr", "Title": "Mr", "MiddleName": "B", "Notes": "Here are other details.", "FamilyName": "Watson", "PrimaryPhone": { "FreeFormNumber": "(555) 555-5555" }, "CompanyName": "Erics Groceries", "BillAddr": { "CountrySubDivisionCode": "CA", "City": "Mountain View", "PostalCode": "94042", "Line1": "123 Main Street", "Country": "USA" }, "GivenName": "Eric"}"""
                 .Replace("Erics Groceries", $"Test Customer {DateTime.Now.ToString("yyyyMMddHHmmss")}");
                var companyId = this.User.Claims.FirstOrDefault(c => c.Type == "realmid");
                if (companyId == null)
                {
                    CreateCustomerResponse = "Missing realmid claim. Please sign out and sign in again.";
                    return Page();
                }

                var content = new StringContent(jsonData, Encoding.UTF8, new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (var response = await httpClient.PostAsync($"{_qboApiBaseUrl}/{companyId.Value}/customer?minorversion={_qboMinorVersion}", content))
                {
                    CreateCustomerResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return Page();
        }

    }
}
