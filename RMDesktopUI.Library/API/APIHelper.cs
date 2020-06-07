using RMDesktopUI.Helpers;
using RMDesktopUI.Library.Models;
using RMDesktopUI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RMDesktopUI.Library.API
{
    public class APIHelper : IAPIHelper
    {
        private HttpClient httpClient;
        private ILoggedInUserModel _loggedInUser;

        public APIHelper(ILoggedInUserModel loggedInUser)
        {
            InitilizeClient();
            _loggedInUser = loggedInUser; 
        }

        public HttpClient client
        {
            get
            {
                return httpClient;
            }
        }

        private void InitilizeClient()
        {
            string api = ConfigurationManager.AppSettings["api"];

            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(api);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void LoggOffUser()
        {
            httpClient.DefaultRequestHeaders.Clear();
        }

        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("grant_type","password"),
                new KeyValuePair<string,string>("username",username),
                new KeyValuePair<string,string>("password",password)
            });

            using (HttpResponseMessage response = await httpClient.PostAsync("/Token", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    Console.WriteLine($"{result.Access_Token} , {result.UserName}");

                    return result;
                }
                else
                {
                    //var resourceManager = new ResourceManager("RetailManager.RMDesktopUI.Resources.ExeptionMassages", Assembly.GetExecutingAssembly());
                    throw new LoginExeption("Password or username is incorrect.");
                }
            }
        }

        public async Task GetLoggedInUserInfo(string token)
        {
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            using (HttpResponseMessage response = await httpClient.GetAsync("/api/User"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<LoggedInUserModel>();
                    _loggedInUser.Id = result.Id;
                    _loggedInUser.FirstName = result.FirstName;
                    _loggedInUser.LastName = result.LastName;
                    _loggedInUser.EmailAddress = result.EmailAddress;
                    _loggedInUser.CreatedDate = result.CreatedDate;
                    _loggedInUser.Token = token;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }

            }
        }
    }
}
