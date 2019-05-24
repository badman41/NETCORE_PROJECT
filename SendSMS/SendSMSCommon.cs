using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SendSMS
{
    public class SendSMSCommon
    {
        public static bool SendSMS(string message, string phoneNumber)
        {
            const string accountSid = "AC0196940cc46b3689d391baf325c326a8";
            const string authToken = "86c7a78b0193c3280bee137d90322847";

            TwilioClient.Init(accountSid, authToken);
            try
            {
                MessageResource.Create(
                    body: message,
                    from: new Twilio.Types.PhoneNumber("+12055284295"),
                    to: new Twilio.Types.PhoneNumber(phoneNumber)
                );
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
