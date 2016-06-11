using Google.Apis.Auth.OAuth2;
using Google.Apis.Plus.v1;
using Google.Apis.Plus.v1.Data;
using Google.Apis.Services;
using Smart_Mirror_App_WPF.Data.Database;
using Smart_Mirror_App_WPF.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace Smart_Mirror_App_WPF.Data.API
{
    public class GooglePlusService : DefaultGoogleService<GoogleProfileModel, List<GoogleProfileModel>, Person>
    {
        private List<GoogleProfileModel> _profiles = new List<GoogleProfileModel>();
        private GoogleProfileModel _currentUserProfile = new GoogleProfileModel();
        private readonly UserCredential _credential;
        private readonly string _applicationName = "Smart Mirror Google Plus Service";
        
        public GooglePlusService(UserCredential credential)
        {
            _credential = credential;
        }

        /// <summary>
        /// Creates a service and starts requesting google+ data
        /// </summary>
        public override void CreateService()
        {
            var service = new PlusService(new BaseClientService.Initializer
            {
                HttpClientInitializer = _credential,
                ApplicationName = _applicationName,
            });
            RequestGooglePlusData(service);
        }

        private void RequestGooglePlusData(PlusService service)
        {
            var profiles = new List<GoogleProfileModel> { ResponseParser(service.People.Get("me").Execute()) };

            SetData(profiles);
            InsertToDb(profiles);
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
            new GoogleProfileTable().InsertRow(data.FirstOrDefault());
        }

        protected override GoogleProfileModel ResponseParser(Person response)
        {
            var userProfile = new GoogleProfileModel
            {
                smartMirrorUsername = _credential.UserId,
                displayName = response.DisplayName,
                gender = response.Gender,
                imageUrl = response.Image.Url,
                location = FilterLocationResponse(response),
                birthday = response.Birthday
            };

            return userProfile;
        }

        /// <summary>
        /// Filters all the location of the user and get his/her primary location
        /// </summary>
        /// <param name="response">the API response</param>
        /// <returns>A string of the location of the user</returns>
        private static string FilterLocationResponse(Person response)
        {
            string location = "";
            if (response.PlacesLived == null) return location;
            foreach (var placesLived in response.PlacesLived)
            {
                if (placesLived.Primary != true) continue;
                location = placesLived.Value;
                break;
            }
            return location;
        }

        protected override void SetData(List<GoogleProfileModel> itemList)
        {
            _profiles = itemList;
            _currentUserProfile = _profiles.FirstOrDefault();
        }
    }
}
