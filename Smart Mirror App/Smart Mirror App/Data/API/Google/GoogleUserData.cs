using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Smart_Mirror_App.Data.Models;
using Windows.Data.Json;

namespace Smart_Mirror_App.Data.API.Google
{
    class GoogleUserData
    {
        private string accesToken;
        private GoogleUserModel userProfile;

        public GoogleUserData(string accesToken)
        {
            this.accesToken = accesToken;
            userProfile = new GoogleUserModel();
        }

        public GoogleUserModel GetUserProfile()
        {
            return userProfile;
        }

        public async Task RequestUserProfile()
        {
            GoogleUserModel requestResponse = new GoogleUserModel();
            HttpClient httpClient = new HttpClient();

            var searchUrl = "https://www.googleapis.com/plus/v1/people/me";
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accesToken);
        

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(searchUrl);
                ParseUserResponse(response);
            }
            catch (HttpRequestException httpError)
            {
                Debug.WriteLine(httpError);
            }

        }

        private async void ParseUserResponse(HttpResponseMessage response)
        {
            GoogleUserModel parsedUserProfile = new GoogleUserModel();
            string jsonContent = await response.Content.ReadAsStringAsync();
            if (jsonContent != null)
            {
                JsonObject jsonObject = JsonObject.Parse(jsonContent);
                string name = jsonObject.GetNamedString("displayName");
                SetUserProfile(name);
            }
        }

        private void SetUserProfile(string name)
        {
            userProfile.name = name;
        }
    }
}
