using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace Comilio
{
    public class SmsMessage
    {
        public const string SMS_TYPE_CLASSIC = "Classic";
        public const string SMS_TYPE_SMART = "Smart";
        public const string SMS_TYPE_SMARTPRO = "SmartPro";

        private string _username;
        private string _password;
        private string _type;
        private string _sender;
        private string[] _recipients;
        private string _smsId;
        private string _status;

        public SmsMessage(string sms_id)
            : this()
        {
            _smsId = sms_id;
        }

        public SmsMessage()
        {
            _type = SMS_TYPE_SMART;
        }

        public SmsMessage Authenticate(string api_username, string api_password)
        {
            _username = api_username.Trim();
            _password = api_password.Trim();

            if (string.IsNullOrEmpty(_username))
            {
                throw new SmsException("API Username cannot be empty");
            }

            if (string.IsNullOrEmpty(_password))
            {
                throw new SmsException("API Password cannot be empty");
            }

            return this;
        }

        public SmsMessage SetType(string type)
        {
            if (new string[] { SMS_TYPE_CLASSIC, SMS_TYPE_SMART, SMS_TYPE_SMARTPRO }.Contains(type))
            {
                _type = type;
            }
            else
            {
                throw new SmsException("Specified type is not valid");
            }

            return this;
        }

        public SmsMessage SetSender(string sender)
        {
            if (!IsValidNumberFormat(sender))
            {
                int senderLenght = sender.Length;

                if (senderLenght > 11)
                {
                    _sender = null;
                    throw new SmsException("Specified sender '$sender' is not valid");
                }
            }

            _sender = sender;

            return this;
        }

        public SmsMessage SetRecipients(string[] recipients)
        {
            foreach (var recipient in recipients)
            {
                if (!IsValidNumberFormat(recipient))
                {
                    _recipients = null;
                    throw new Exception($"Recipient '{ recipient }' is not valid");
                }
            }

            _recipients = recipients;

            return this;
        }

        public bool Send(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new Exception("SMS message text cannot be empty");
            }

            if (_recipients != null || _recipients.Count() == 0)
            {
                throw new Exception("At least one recipient is required");
            }

            if (string.IsNullOrEmpty(_username) || string.IsNullOrEmpty(_password))
            {
                throw new Exception("Auth required");
            }

            var payload = new SendSmsRequest
            {
                MessageType = _type,
                Recipients = _recipients,
                Sender = _sender,
                Text = message,
            };

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{ _username }:{ _password }")));

                var response = httpClient.PostAsync(BuildUrl("/message"), new StringContent(JsonConvert.SerializeObject(payload))).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                dynamic responseObject = JsonConvert.DeserializeObject<dynamic>(responseBody);

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        _smsId = responseObject.message_id;
                        return true;
                    case System.Net.HttpStatusCode.Unauthorized:
                        throw new SmsException("Authentication failed");
                    default:
                        throw new Exception($"Unable to send SMS. Gateway response: { responseBody }");
                }
            }
        }

        public SmsMessageState[] GetStatus(string sms_Id = null)
        {
            string smsId = string.IsNullOrEmpty(sms_Id) ? _smsId : sms_Id;

            if (string.IsNullOrEmpty(smsId))
            {
                throw new SmsException("SMS message ID not set");
            }

            if (string.IsNullOrEmpty(_username) || string.IsNullOrEmpty(_password))
            {
                throw new Exception("Auth required");
            }

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{ _username }:{ _password }")));

                var response = httpClient.GetAsync(BuildUrl($"/message/{ smsId }")).Result;
                string responseString = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new SmsException($"Unable to get SMS status. Gateway response: { responseString }");
                }

                var result = JsonConvert.DeserializeObject<SmsMessageState[]>(responseString);

                return result;
            }
        }

        public string GetId()
        {
            return _smsId;
        }

        public static bool IsValidNumberFormat(string number)
        {
            return Regex.IsMatch(number, @"/^\+?[0-9]{4,14}$/");
        }

        private static string BuildUrl(string resource)
        {
            return "https://api.comilio.it/rest/v1" + resource;
        }
    }
}
