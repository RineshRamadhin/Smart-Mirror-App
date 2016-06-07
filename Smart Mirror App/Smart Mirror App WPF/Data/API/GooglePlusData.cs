using Smart_Mirror_App_WPF.Data.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Smart_Mirror_App_WPF.Data.Database;
using Smart_Mirror_App_WPF.Data.API.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Plus.v1;
using Google.Apis.Plus.v1.Data;

namespace Smart_Mirror_App_WPF.Data.API
{
    public class GooglePlusData : DefaultGoogleData<GoogleProfileModel>
    {
        private GoogleProfileModel _userProfile;
        private UserCredential _credentials;

        public GooglePlusData(UserCredential credential)
        {
            _userProfile = new GoogleProfileModel();
            this._credentials = credential;
            this._userProfile.smartMirrorUsername = credential.UserId;
        }

        private GoogleProfileModel ParseUserProfile(GoogleProfileResponseModel profileResponse, GoogleProfileModel profile)
        {
            profile.displayName = profileResponse.displayName;
            string imageUrl;
            profileResponse.image.TryGetValue("url", out imageUrl);
            profile.imageUrl = imageUrl;
            profile.gender = profileResponse.gender;

            return profile;
        }

        private void InsertProfileToDb(GoogleProfileModel profile)
        {
            GoogleProfileTable profileDb = new GoogleProfileTable();
            profileDb.InsertRow(profile);
        }

        public override async Task HttpRequestData()
        {
            GoogleProfileModel requestResponse = new GoogleProfileModel();
            HttpClient httpClient = new HttpClient();

            var requestUrl = "https://www.googleapis.com/plus/v1/people/me";
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _credentials.Token.AccessToken);

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
                ResponseParser(response);
            }
            catch (HttpRequestException httpError)
            {
                Debug.WriteLine(httpError);
            }
        }

        protected override async void ResponseParser(HttpResponseMessage response)
        {
            GoogleProfileModel parsedUserProfile = new GoogleProfileModel();
            string jsonContent = await response.Content.ReadAsStringAsync();
            GoogleProfileResponseModel profileResponse = JsonConvert.DeserializeObject<GoogleProfileResponseModel>(jsonContent);
            if (profileResponse != null)
            {
                parsedUserProfile = this.ParseUserProfile(profileResponse, parsedUserProfile);
                this.SetData(parsedUserProfile);
            }
        }

        public override GoogleProfileModel GetData()
        {
            return _userProfile;
        }

        protected override void SetData(GoogleProfileModel profile)
        {
            _userProfile.displayName = profile.displayName;
            _userProfile.imageUrl = profile.imageUrl;
            _userProfile.gender = profile.gender;

            this.InsertProfileToDb(this._userProfile);
        }

        public override void InsertToDb(GoogleProfileModel profile)
        {
            GoogleProfileTable profileDb = new GoogleProfileTable();
            profileDb.InsertRow(profile);
        }
    }
}
