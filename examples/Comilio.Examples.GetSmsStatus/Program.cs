using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comilio.Examples.GetSmsStatus
{
    class Program
    {
        static void Main(string[] args)
        {
            string comilioUsername = "nodejs-dev"; // Please register on https://www.comilio.it
            string comilioPassword = "j89w437uu";
            string smsId = "825B73C4C263433BACA4F6FE775F2116";

            var sms = new SmsMessage();

            sms.Authenticate(comilioUsername, comilioPassword);

            foreach (var status in sms.GetStatus(smsId))
            {
                Console.WriteLine($"Message to { status.PhoneNumber } is in status { status.Status }");
            }
        }
    }
}
