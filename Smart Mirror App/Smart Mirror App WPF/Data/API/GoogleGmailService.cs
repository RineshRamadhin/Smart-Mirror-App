using Smart_Mirror_App_WPF.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Gmail.v1.Data;

namespace Smart_Mirror_App_WPF.Data.API
{
    public class GoogleGmailService : DefaultGoogleData<GoogleGmailModel>
    {
        private GoogleGmailModel _gmails;
        private UserCredential _credential;
        private string _applicationName = "Smart Mirror Gmail Service";

        public GoogleGmailService(UserCredential credential)
        {
            this._credential = credential;
        }

        public override GoogleGmailModel GetData()
        {
            throw new NotImplementedException();
        }

        public void CreateGoogleService()
        {
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credential,
                ApplicationName = _applicationName,
            });

            // Define parameters of request.
            UsersResource.LabelsResource.ListRequest request = service.Users.Labels.List("me");
            UsersResource.MessagesResource.ListRequest request2 = service.Users.Messages.List("me");

            IList<Message> messages = request2.Execute().Messages;

            if (messages != null && messages.Count > 0)
            {
                foreach (var message in messages)
                {
                    UsersResource.MessagesResource.GetRequest mailRequest = service.Users.Messages.Get("me", message.Id);
                    Message mail = mailRequest.Execute();
                }
            }


            // List labels.
            IList<Label> labels = request.Execute().Labels;
            Console.WriteLine("Labels:");
            if (labels != null && labels.Count > 0)
            {
                foreach (var labelItem in labels)
                {
                    Console.WriteLine("{0}", labelItem.Name);
                }
            }
            else
            {
                Console.WriteLine("No labels found.");
            }
            Console.Read();
        }

        public override async Task HttpRequestData()
        {
            HttpClient httpClient = new HttpClient();

            var requestUrl = "https://www.googleapis.com/gmail/v1/users/me/messages?maxResults=20&fields=messages";
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " );

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

        public override void InsertToDb(GoogleGmailModel data)
        {
            throw new NotImplementedException();
        }

        protected override async void ResponseParser(HttpResponseMessage response)
        {
            string jsonContent = await response.Content.ReadAsStringAsync();
        }

        protected override void SetData(GoogleGmailModel dataModel)
        {
            throw new NotImplementedException();
        }
    }
}
