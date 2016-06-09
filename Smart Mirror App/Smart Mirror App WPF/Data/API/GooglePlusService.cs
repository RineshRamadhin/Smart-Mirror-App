using Google.Apis.Auth.OAuth2;
using Google.Apis.Plus.v1;
using Google.Apis.Plus.v1.Data;
using Google.Apis.Services;
using Smart_Mirror_App_WPF.Data.Database;
using Smart_Mirror_App_WPF.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Mirror_App_WPF.Data.API
{
    public class GooglePlusService : DefaultGoogleService<GoogleProfileModel, List<GoogleProfileModel>, Person>
    {
        private List<GoogleProfileModel> _profiles = new List<GoogleProfileModel>();
        private GoogleProfileModel _currentUserProfile = new GoogleProfileModel();
        private UserCredential _credential;
        private string _applicationName = "Smart Mirror Google Plus Service";
        
        public GooglePlusService(UserCredential credential)
        {
            this._credential = credential;
        }

        public override void CreateService()
        {
            var service = new PlusService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credential,
                ApplicationName = _applicationName,
            });

            PeopleResource.GetRequest personRequest = service.People.Get("me");
            var profile = this.ResponseParser(personRequest.Execute());
            var profiles = new List<GoogleProfileModel>();

            profiles.Add(profile);
            this.SetData(profiles);
            this.InsertToDb(profiles);
        }

        public override List<GoogleProfileModel> GetData()
        {
            return _profiles;
        }

        public GoogleProfileModel GetUserProfile()
        {
            return _currentUserProfile;
        }

        public override void InsertToDb(List<GoogleProfileModel> data)
        {
            var googleProfileTable = new GoogleProfileTable();
            googleProfileTable.InsertRow(data.FirstOrDefault());
        }

        protected override GoogleProfileModel ResponseParser(Person response)
        {
            var userProfile = new GoogleProfileModel();
            userProfile.smartMirrorUsername = _credential.UserId;
            userProfile.displayName = response.DisplayName;
            userProfile.gender = response.Gender;
            userProfile.imageUrl = response.Image.Url;
            userProfile.location = FilterLocationResponse(response);
 
            return userProfile;
        }

        private string FilterLocationResponse(Person response)
        {
            string location = "";
            foreach (var placesLived in response.PlacesLived)
            {
                if (placesLived.Primary == true)
                {
                    location = placesLived.Value;
                    break;
                }
            }
            return location;
        }

        protected override void SetData(List<GoogleProfileModel> itemList)
        {
            this._profiles = itemList;
            this._currentUserProfile = _profiles.FirstOrDefault();
        }
    }
}
