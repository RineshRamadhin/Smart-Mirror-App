﻿using Smart_Mirror_App_WPF.Data.Models;
using System;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Gmail.v1.Data;
using System.Diagnostics;

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
            try
            {
                IList<Message> messages = allMailRequest.Execute().Messages;
                this.GetDetailsMail(messages, service);
            } catch (Exception error)
            {
                // TODO; Catch 400 error meaning user has no gmail 
                Debug.WriteLine(error);
            }
        }
 
        private void GetDetailsMail(IList<Message> emails, GmailService service)
        {
            if (emails != null && emails.Count > 0)
            {
                List<GoogleGmailModel> allMails = new List<GoogleGmailModel>();
                foreach (var message in emails)
                {
                    UsersResource.MessagesResource.GetRequest mailRequest = service.Users.Messages.Get("me", message.Id);
                    Message mailDetails = mailRequest.Execute();
                    var email = this.ResponseParser(mailDetails);
                    allMails.Add(email);

                }
                this.SetData(allMails);
            }
        }

        protected override GoogleGmailModel ResponseParser(Message response)
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
            return email;
        }

        

        public override void InsertToDb(List<GoogleGmailModel> data)
        {
            foreach (var mail in data)
            {

            }
        }
    }
}
