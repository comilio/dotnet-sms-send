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
            string comilioUsername = "your_username_here"; // Please register on https://www.comilio.it
            string comilioPassword = "your_password_here";
            string smsId = "sms_id_here";

            var sms = new SmsMessage();

            sms.Authenticate(comilioUsername, comilioPassword);

            foreach (var status in sms.GetStatus(smsId))
            {
                Console.WriteLine($"Message to { status.PhoneNumber } is in status { status.Status }");
            }
        }
    }
}
