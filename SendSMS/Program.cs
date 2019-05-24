using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Messaging.V1;
using Twilio.Rest.Messaging.V1.Service;

namespace SendSMS
{
    class Program
    {
        static void Main(string[] args)
        {
            const string accountSid = "AC0196940cc46b3689d391baf325c326a8";
            const string authToken = "86c7a78b0193c3280bee137d90322847";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: "Bạn vừa đặt hàng thành công trên avita. Mã đơn hàng của bạn là: 1234. Vui lòng truy cập fanpage để xem tình trạng đơn hàng",
                from: new Twilio.Types.PhoneNumber("+12055284295"),
                to: new Twilio.Types.PhoneNumber("+84979002695")
            );
            //var phoneNumber = PhoneNumberResource.Fetch(
            //    pathServiceSid: "MG886606970ef7c5cbeb26cdf1dfa8061c",
            //    pathSid: "+84374105865"
            //);
            //Console.WriteLine(message.Sid);

            //var service = ServiceResource.Create(friendlyName: "friendlyName");
            //Console.WriteLine(service.Sid);

            //    var validationRequest = ValidationRequestResource.Create(
            //    friendlyName: "Duc Anh",
            //    phoneNumber: new Twilio.Types.PhoneNumber("+84979002695"),
            //    callDelay: 5
            //);
            //    Console.WriteLine(validationRequest.ValidationCode);
        }
    }
}
