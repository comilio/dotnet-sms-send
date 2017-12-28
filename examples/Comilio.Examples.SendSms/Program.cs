using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comilio.Examples.SendSms
{
    class Program
    {
        static void Main(string[] args)
        {
            string comilioUsername = "nodejs-dev"; // Please register on https://www.comilio.it
            string comilioPassword = "j89w437uu";
            string sender = "ComilioTest";
            string[] recipients = { "+393400000000", "+393499999999" };
            string text = "Hello World!";

            var sms = new SmsMessage();

            sms.Authenticate(comilioUsername, comilioPassword)
                .SetSender(sender)
                .SetType(SmsMessage.SMS_TYPE_SMARTPRO)
                .SetRecipients(recipients);

            if (sms.Send(text))
            {
                Console.WriteLine($"Sent SMS Id: { sms.GetId() }");

                foreach (var status in sms.GetStatus())
                {
                    Console.WriteLine($"Message to { status.PhoneNumber } is in status { status.Status }");
                }
            }
        }
    }
}
