using Smart_Mirror_App_WPF.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Smart_Mirror_App_WPF.Data.API
{
    public class GooglePlusData
    {
        private string accesToken;
        private GoogleProfileModel userProfile;

        public GooglePlusData(string accesToken)
        {
            this.accesToken = accesToken;
            userProfile = new GoogleProfileModel();
        }

        public GoogleProfileModel GetUserProfile()
        {
            return userProfile;
        }

        public async Task RequestUserProfile()
        {
            GoogleProfileModel requestResponse = new GoogleProfileModel();
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
            GoogleProfileModel parsedUserProfile = new GoogleProfileModel();
            string jsonContent = await response.Content.ReadAsStringAsync();
            GoogleProfileModel profile = JsonConvert.DeserializeObject<GoogleProfileModel>(jsonContent);
            if (jsonContent != null)
            {

            }
        }

        private void SetUserProfile(string name, string imageUrl)
        {
            userProfile.displayName = name;
            userProfile.imageUrl = imageUrl;
        }
    }
}
