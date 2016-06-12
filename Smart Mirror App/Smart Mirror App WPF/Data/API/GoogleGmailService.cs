using Smart_Mirror_App_WPF.Data.Models;
using System;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Gmail.v1.Data;
using System.Diagnostics;
using System.Linq;
using Smart_Mirror_App_WPF.Data.Database;

namespace Smart_Mirror_App_WPF.Data.API
{
    public class GoogleGmailService : DefaultGoogleService<GoogleGmailModel, List<GoogleGmailModel>, Message>
    {
        private List<GoogleGmailModel> _gmails = new List<GoogleGmailModel>();
        private readonly UserCredential _credential;
        private const string ApplicationName = "Smart Mirror Gmail Service";

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
            var service = new GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = _credential,
                ApplicationName = ApplicationName,
            });

            var allMailRequest = service.Users.Messages.List("me");
            allMailRequest.MaxResults = 20;
            allMailRequest.LabelIds = "CATEGORY_PERSONAL";
            allMailRequest.IncludeSpamTrash = false;

            try
            {
                var messages = allMailRequest.Execute().Messages;
                // messages does not contain all the details of the mail, so we need to get more details
                if (messages == null || messages.Count <= 0) return;
                SetData(GetDetailsMail(messages, service));
                InsertToDb(_gmails);
            } catch (Exception error)
            {
                // TODO; Catch 400 error meaning user has no gmail 
                Debug.WriteLine(error);
            }
        }

        /// <summary>
        /// Gets more details of all the mails
        /// </summary>
        /// <param name="emails">all the emails from the ListResponse</param>
        /// <param name="service">the created GmailService</param>
        /// <returns></returns>
        private List<GoogleGmailModel> GetDetailsMail(IEnumerable<Message> emails, GmailService service)
        {
            var allMails = new List<GoogleGmailModel>();
            foreach (var message in emails)
            {
                allMails.Add(this.ResponseParser(service.Users.Messages.Get("me", message.Id).Execute()));
            }
            return allMails;
        }

        protected override GoogleGmailModel ResponseParser(Message response)
        {
            var email = new GoogleGmailModel
            {
                userId = _credential.UserId,
                snippet = response.Snippet,
                id = response.Id,
                labels = FilterLabels(response.LabelIds)
            };
            email = FilterHeadersMail(response, email);
            return email;
        }

        /// <summary>
        /// Filters only the neccessary data we need from the mail
        /// </summary>
        /// <param name="response">the mail detail response</param>
        /// <param name="email">the mail we want to save/use</param>
        /// <returns>the mail you want to save/use</returns>
        private static GoogleGmailModel FilterHeadersMail(Message response, GoogleGmailModel email)
        {
            foreach (var header in response.Payload.Headers)
            {
                if (header.Name == "From")
                    email.from = header.Value;
                if (header.Name == "Subject")
                    email.subject = header.Value;
                if (header.Name == "Date")
                {
                    DateTime date;
                    DateTime.TryParse(header.Value, out date);
                    email.date = date;
                }
            }
            return email;
        }

        /// <summary>
        /// Sets all Labels of the mail in one string
        /// </summary>
        /// <param name="labels">The list of labels</param>
        /// <returns>One string with all labels seperated by a "-"</returns>
        private static string FilterLabels(IEnumerable<string> labels)
        {
            return labels.Aggregate("", (current, label) => current + (label + "-"));
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
