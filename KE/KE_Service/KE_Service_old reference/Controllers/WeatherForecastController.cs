using Microsoft.AspNetCore.Mvc;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Identity.Client;

using Newtonsoft.Json;
using System.ComponentModel;
using System.Net;
using System.Security;
using System.Security.Cryptography.Xml;
using System.Text;
using static KE_Service.Controllers.FolderCreator;

namespace KE_Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Post([FromBody] TokenRequest tokenRequest)
        {
            FolderCreator folderCreator = new FolderCreator();

            folderCreator.CreateFolderAsync(tokenRequest.AccessToken);

            return null;

            //{
            //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //    {
            //        Date = DateTime.Now.AddDays(index),
            //        TemperatureC = Random.Shared.Next(-20, 55),
            //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //    })
            //    .ToArray();
            //}
        }
    }

    public class FolderCreator
    {
        static  int _i = 0;
        public async Task<AuthenticationResult> Connect()

        {
            ExchangeService ewsClient;
            string EmailId = "emaildev@oakpointpartners.com";
            string Password = "Fah58393";

           

            AuthenticationResult result = null;

            //Client Id and TensntID 
            var pcaOptions = new PublicClientApplicationOptions
            {
                ClientId = "9c7b9698-141c-46bf-8eac-ba9ecf8794c3",// ConfigurationManager.AppSettings["appId"],
                TenantId = "284c928c-7c8e-4cb0-8220-375a7c54a1a2" // ConfigurationManager.AppSettings["tenantId"]
            };

            string[] scopes = new string[]
{
    "user.read",                // Read basic user profile
    "mail.read",                // Read user mail
    "calendars.read",           // Read user calendars
    "files.read",               // Read user files
    "offline_access"            // Request refresh tokens for long-lived access
};
            var app = PublicClientApplicationBuilder.Create(pcaOptions.ClientId)
                                .WithRedirectUri("http://localhost:44302")
                                .WithAuthority($"https://login.microsoftonline.com/{pcaOptions.TenantId}") // Use tenant-specific authority
                                .Build();

            var result1 = await app.AcquireTokenInteractive(scopes)
                                    .WithUseEmbeddedWebView(false) // For desktop apps
                                    .ExecuteAsync();

            //  return result1.AccessToken;


            return null;

            var pca = PublicClientApplicationBuilder
                .CreateWithApplicationOptions(pcaOptions).Build();

            // The permission scope required for EWS access
            var ewsScopes = new string[] { "https://outlook.office365.com/EWS.AccessAsUser.All" };

            try
            {

                var securePassword = new SecureString();
                foreach (char c in Password)        // you should fetch the password
                    securePassword.AppendChar(c);

                string token = "";
                result = await pca.AcquireTokenByUsernamePassword(ewsScopes,
                                                    EmailId,
                                                     securePassword)
                      .ExecuteAsync();


                token = result.AccessToken;


                ewsClient = new ExchangeService();
                ewsClient.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");
                ewsClient.Credentials = new OAuthCredentials(token);// authResult.AccessToken);

               // ServiceLog.Logger.LogWrite("Token received successfully", ServiceLog.LogType.Info);
            }
            catch (MsalException ex)
            {
                //ServiceLog.Logger.LogWrite("Exchange Connect Method :" + ex.ToString(), ServiceLog.LogType.Error);
            }
            catch (Exception ex)
            {
               // ServiceLog.Logger.LogWrite("Exchange Connect Method :" + ex.ToString(), ServiceLog.LogType.Error);
            }
            return result;

        }
        public async System.Threading.Tasks.Task CreateFolder(string TokenID)
        {
            try
            {
                // Connect and obtain access token
                AuthenticationResult authResult =  await Connect();
                if (authResult == null)
                {
                    // Log or throw an exception to indicate authentication failure
                    Console.WriteLine("Authentication failed.");
                    //return null;
                }

                // Create ExchangeService instance and set URL
                ExchangeService ewsClient = new ExchangeService();
                ewsClient.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");

                // Authenticate with OAuth access token
                ewsClient.Credentials = new OAuthCredentials(TokenID);// authResult.AccessToken);

                // Create a new folder
                Folder rootFolder = Folder.Bind(ewsClient, WellKnownFolderName.MsgFolderRoot);
                Folder newFolder = new Folder(ewsClient);
                newFolder.DisplayName = "NewFolderName1234"; // Replace with desired folder name

                // Save the new folder to the root folder
                newFolder.Save(rootFolder.Id);

                // Folder created successfully
                Console.WriteLine("New folder created successfully.");
            }
            catch (Exception ex)
            {
                // Log or handle any exceptions
                Console.WriteLine("Error creating folder: " + ex.Message);
            }
          //  return null;
        }
        public async System.Threading.Tasks.Task CreateFolderAsync(string token)
        {

            await CreateFolder(token);
            return;
            _i++;
            string folderName = "AshishSinha" + _i;

            var accessToken = token;// "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IkZjVE51enRycDFZQW16TXY3Um9SOXg4RGhhTT0iLCJ4NXQiOiJGY1ROdXp0cnAxWUFtek12N1JvUjl4OERoYU09Iiwibm9uY2UiOiJhdWNVbWFXREw0WTh6VVF6N2FocHNBcGt2WkkydGw3X1FKOS1BcDNBVW5zY1RKZlE5aUZPVWYtUFJ0aDRwbFd6ZHFjcUV3N0NrV2NZTG1jNS1lZ2VXSEdHZWxnSW9rZldYUl9zZ3dQd0xlQnpsTjZWbkNCQmVFVkpURkpxTTB4SDFhRDR3QkFsZ2FFV3ItaVduLUpTZ0hFdXhhRzdiMjBsZVcxel8zMUlPT2siLCJpc3Nsb2MiOiJTSjBQUjIyTUIzMTkxIiwic3JzbiI6NjM4NDkwMTg5OTEyNTY4MjU4fQ.eyJzYXAtdmVyc2lvbiI6IjEzIiwiYXBwaWQiOiJmZGJiMmNiNy05…TcxMzUwNzgwNywiaXNzIjoiaHR0cHM6Ly9zdWJzdHJhdGUub2ZmaWNlLmNvbS9zdHMvIiwiYXVkIjoiaHR0cHM6Ly9vdXRsb29rLm9mZmljZTM2NS5jb20iLCJzc2VjIjoiQzV3TVVWN1hOUzlWYjEwNSJ9.C40qjnlpeRvDpWn41tZhZQHmvkOePGXP_zdPZIa8XfODEaZHQ_L5rojD_xdcsIvOOU0C6aT3Hq8B579vPR0kJbviBbgpBLWJU1M0ZG_ytHbdiEoEnnnLSfpnMn53BNqLarJHCipyK75kG_M8sHz5pCnacvVfcIOfhLXx7qZWlxXMJOu3uwBM8IEkhRUbalOnVZIoPH3S25Disxn5ADQlgqCJg7_Yp6gVvFMQjXkfo8b_30Z3k5e6GYflh5r3GLd1WlpIOEy1T8xUfKz1ztO7sO5VENzXh3ug9URQ8EvyRZlNpYMI038-LgBXQif5Pb8UVIqmod4HHf9Wib7jOPNqrQ";// await GetAccessTokenAsync()


            #region EWS 

            // Set up ExchangeService with appropriate credentials
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2013_SP1);
            service.Credentials = new WebCredentials("emaildev@oakpointpartners.com", "Fah58393");
            service.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");
            try
            {
                // Create a new folder under the root folder
                Folder rootFolder = Folder.Bind(service, WellKnownFolderName.MsgFolderRoot);
                Folder newFolder = new Folder(service);
                newFolder.DisplayName = "NewFolderName";

                // Save the new folder to the root folder
                newFolder.Save(rootFolder.Id);

                Console.WriteLine("New folder created successfully.");

            }
            catch(Exception ex)
            {
                int a = 0;
            }
            #endregion


            #region SoapMethod
            try
            {
                // Exchange server URL
                string exchangeServerUrl = "https://outlook.office365.com/ews/Exchange.asmx";


                // Construct SOAP request
                string soapRequest = $@"
                <s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'>
                    <s:Header>
                        <t:ExchangeImpersonation xmlns:t='http://schemas.microsoft.com/exchange/services/2006/types'>
                            <t:ConnectingSID>
                                <t:PrimarySmtpAddress>your_email_address</t:PrimarySmtpAddress>
                            </t:ConnectingSID>
                        </t:ExchangeImpersonation>
                        <t:RequestServerVersion Version='Exchange2013' xmlns:t='http://schemas.microsoft.com/exchange/services/2006/types' />
                        <t:MailboxCulture xmlns:t='http://schemas.microsoft.com/exchange/services/2006/types'>en-US</t:MailboxCulture>
                        <t:TimeZoneContext xmlns:t='http://schemas.microsoft.com/exchange/services/2006/types'>
                            <t:TimeZoneDefinition Id='Eastern Standard Time' />
                        </t:TimeZoneContext>
                    </s:Header>
                    <s:Body>
                        <CreateFolder xmlns='http://schemas.microsoft.com/exchange/services/2006/messages' xmlns:t='http://schemas.microsoft.com/exchange/services/2006/types'>
                            <ParentFolderId>
                                <t:DistinguishedFolderId Id='inbox' />
                            </ParentFolderId>
                            <Folders>
                                <t:Folder>
                                    <t:DisplayName>{folderName}</t:DisplayName>
                                </t:Folder>
                            </Folders>
                        </CreateFolder>
                    </s:Body>
                </s:Envelope>";

                // Make SOAP request
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(exchangeServerUrl);
                request.Method = "POST";
                request.ContentType = "text/xml; charset=utf-8";
                request.Headers.Add("Authorization", "Bearer " + accessToken);
                request.Headers.Add("SOAPAction", "http://schemas.microsoft.com/exchange/services/2006/messages/CreateFolder");

                // Write SOAP request to request stream
                using (Stream stream = request.GetRequestStream())
                {
                    byte[] soapBytes = Encoding.UTF8.GetBytes(soapRequest);
                    stream.Write(soapBytes, 0, soapBytes.Length);
                }

                // Get SOAP response
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string soapResponse = reader.ReadToEnd();

                // Process SOAP response
                // Add your logic here to handle the SOAP response
                Console.WriteLine("Folder created successfully.");
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine("Error: " + ex.Message);
            }
            #endregion

            return;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                // Encode folderName to ensure it contains only ASCII characters
            var encodedFolderName =   Uri.EscapeDataString(folderName);

                var requestBody = new
                {
                    DisplayName = encodedFolderName
                };

                var requestBodyJson = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
                try
                {
                    var response = await httpClient.PostAsync("https://outlook.office.com/api/v2.0/me/MailFolders", new StringContent(requestBodyJson, Encoding.UTF8, "application/json"));
                    var responseData = await response.Content.ReadAsStringAsync(); //getting folderid of created folder
                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseData);
                    Rootobject rootObject = JsonConvert.DeserializeObject<Rootobject>(responseData);

                    

                    var requestUrl = $"https://outlook.office.com/api/v2.0/me/MailFolders/{rootObject.Id}";
                     response = await httpClient.GetAsync(requestUrl);
                    responseData = await response.Content.ReadAsStringAsync();

                    //Rename Folder 

                    // Folder ID of the folder to rename
                    string folderId = rootObject.Id;

                    // New display name for the folder
                    string newDisplayName = "Baba1234";

                    // Construct request body with the new display name
                     requestBody = new
                    {
                        DisplayName = newDisplayName
                    };

                    // Convert request body to JSON string
                     requestBodyJson = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);

                     response = await httpClient.PatchAsync($"https://outlook.office.com/api/v2.0/me/mailFolders/{folderId}", new StringContent(requestBodyJson, Encoding.UTF8, "application/json"));
                    responseData = await response.Content.ReadAsStringAsync();
                    //


                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Folder created successfully");
                        // You can replace Console.WriteLine with your preferred logging mechanism
                    }
                    else
                    {
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Error creating folder: " + errorMessage);
                        // You can replace Console.WriteLine with your preferred logging mechanism
                    }
                }
                catch(Exception ex)
                {
                    int a = 0;
                }
            }

        }
        public class TokenRequest
        {
            public string AccessToken { get; set; }
        }
        //private async Task<string> GetAccessTokenAsync()
        //{
        //    var confidentialClientApplication = ConfidentialClientApplicationBuilder
        //        .Create("YourClientId")
        //        .WithClientSecret("YourClientSecret")
        //        .WithAuthority(new Uri("https://login.microsoftonline.com/YourTenantId"))
        //        .Build();

        //    string[] scopes = { "https://outlook.office365.com/.default" };

        //    var result = await confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();
        //    return result.AccessToken;
        //}
    }
    public class Rootobject
    {
        public string odatacontext { get; set; }
        public string odataid { get; set; }
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string ParentFolderId { get; set; }
        public int ChildFolderCount { get; set; }
        public int UnreadItemCount { get; set; }
        public int TotalItemCount { get; set; }
        public int SizeInBytes { get; set; }
        public bool IsHidden { get; set; }
    }


}