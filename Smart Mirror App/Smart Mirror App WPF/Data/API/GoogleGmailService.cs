using Smart_Mirror_App_WPF.Data.Models;
using System;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Gmail.v1.Data;
using System.Diagnostics;
using Smart_Mirror_App_WPF.Data.Database;

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

        protected override void SetData(List<GoogleGmailModel> itemList)
        {
            this._gmails = itemList;
        }

        public override void CreateService()
        {
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credential,
                ApplicationName = _applicationName,
            });

            UsersResource.MessagesResource.ListRequest allMailRequest = service.Users.Messages.List("me");
            allMailRequest.LabelIds = "CATEGORY_PERSONAL";
            allMailRequest.IncludeSpamTrash = false;

            try
            {
                IList<Message> messages = allMailRequest.Execute().Messages;
                if (messages != null && messages.Count > 0)
                {
                    var allMails = this.GetDetailsMail(messages, service);
                    this.SetData(allMails);
                    this.InsertToDb(_gmails);
                }
            } catch (Exception error)
            {
                // TODO; Catch 400 error meaning user has no gmail 
                Debug.WriteLine(error);
            }
        }
 
        private List<GoogleGmailModel> GetDetailsMail(IList<Message> emails, GmailService service)
        {
            var allMails = new List<GoogleGmailModel>();
            foreach (var message in emails)
            {
                UsersResource.MessagesResource.GetRequest mailRequest = service.Users.Messages.Get("me", message.Id);
                var email = this.ResponseParser(mailRequest.Execute());
                allMails.Add(email);
            }
            return allMails;
        }

        protected override GoogleGmailModel ResponseParser(Message response)
        {
            var email = new GoogleGmailModel();
            email.userId = _credential.UserId;
            email.snippet = response.Snippet;
            email.id = response.Id;
            email.labels = FilterLabels(response.LabelIds);
            email = FilterHeadersMail(response, email);
            return email;
        }

        private GoogleGmailModel FilterHeadersMail(Message response, GoogleGmailModel email)
        {
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
                if (header.Name == "Date")
                {
                    DateTime date;
                    DateTime.TryParse(header.Value, out date);
                    email.date = date;
                }
            }
            return email;
        }

        private string FilterLabels(IList<string> labels)
        {
            string returnLabel = "";
            foreach (var label in labels)
            {
                returnLabel += label + "-";
            }
            return returnLabel;
        }

        public override void InsertToDb(List<GoogleGmailModel> data)
        {
            var gmailTable = new GoogleGmailTable();
            foreach (var mail in data)
            {
                gmailTable.InsertRow(mail);
            }
        }
    }
}
