using Smart_Mirror_App_WPF.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Smart_Mirror_App_WPF.Data.Database;

namespace Smart_Mirror_App_WPF.Data.API
{
    public class GooglePlusData
    {
        private string _accessToken;
        private GoogleProfileModel userProfile;

        public GooglePlusData(string accessToken, string username)
        {
            userProfile = new GoogleProfileModel();
            this.userProfile.smartMirrorUsername = username;
            this._accessToken = accessToken;
        }

        public GoogleProfileModel GetUserProfile()
        {
            return userProfile;
        }

        public async Task RequestUserProfile()
        {
            GoogleProfileModel requestResponse = new GoogleProfileModel();
            HttpClient httpClient = new HttpClient();

            var requestUrl = "https://www.googleapis.com/plus/v1/people/me";
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
                await ParseUserResponse(response);
            }
            catch (HttpRequestException httpError)
            {
                Debug.WriteLine(httpError);
            }
        }

        private async Task ParseUserResponse(HttpResponseMessage response)
        {
            GoogleProfileModel parsedUserProfile = new GoogleProfileModel();
            string jsonContent = await response.Content.ReadAsStringAsync();
            GoogleProfileResponseModel profileResponse = JsonConvert.DeserializeObject<GoogleProfileResponseModel>(jsonContent);
            if (profileResponse != null)
            {
                SetUserProfile(profileResponse);
            }
        }

        private void SetUserProfile(GoogleProfileResponseModel profile)
        {
            userProfile.displayName = profile.displayName;
            string imageUrl; 
            profile.image.TryGetValue("url", out imageUrl);
            userProfile.imageUrl = imageUrl;
            userProfile.gender = profile.gender;

            this.InsertProfileToDb(this.userProfile);
        }

        private void InsertProfileToDb(GoogleProfileModel profile)
        {
            GoogleProfileTable profileDb = new GoogleProfileTable();
            profileDb.InsertRow(profile);
        }
    }
}
