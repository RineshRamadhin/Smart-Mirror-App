using Smart_Mirror_App_WPF.Data.Models;
using System;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Gmail.v1.Data;

namespace Smart_Mirror_App_WPF.Data.API
{
    public class GoogleGmailService : DefaultGoogleService<GoogleGmailModel, List<GoogleGmailModel>, Message>
    {
        private List<GoogleGmailModel> _gmails = new List<GoogleGmailModel>();
        private UserCredential _credential;
        private string _applicationName = "Smart Mirror Gmail Service";

        public GoogleGmailService(UserCredential credential)
        {
            this._credential = credential;
        }

        public override List<GoogleGmailModel> GetData()
        {
            return _gmails;
        }

        protected override void SetData(GoogleGmailModel dataModel)
        {
            _gmails.Add(dataModel);
        }

        public override void CreateService()
        {
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credential,
                ApplicationName = _applicationName,
            });

            UsersResource.MessagesResource.ListRequest allMailRequest = service.Users.Messages.List("me");

            IList<Message> messages = allMailRequest.Execute().Messages;

            if (messages != null && messages.Count > 0)
            {
                foreach (var message in messages)
                {
                    UsersResource.MessagesResource.GetRequest mailRequest = service.Users.Messages.Get("me", message.Id);
                    Message mail = mailRequest.Execute();
                    this.ResponseParser(mail);
                }
            }
        }

        protected override void ResponseParser(Message response)
        {
            GoogleGmailModel email = new GoogleGmailModel();
            email.snippet = response.Snippet;
            email.id = response.Id;
            email.labels = response.LabelIds;
            foreach (var header in response.Payload.Headers)
            {
                if (header.Name == "From")
                {
                    email.from = header.Value;
                }
                if (header.Name == "Subject")
                {
                    email.subject = header.Value;
                }
            }
            this.SetData(email);
        }

        public override void InsertToDb(List<GoogleGmailModel> data)
        {
            throw new NotImplementedException();
        }
    }
}
