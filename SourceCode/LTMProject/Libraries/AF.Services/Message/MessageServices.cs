using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Services.SMSMessage;

namespace AF.Services.Message
{
    public class MessageServices : IMessageServices
    {
        /// <summary>
        /// Sends the MSG.
        /// </summary>
        /// <param name="phonenumber">The phonenumber.</param>
        /// <param name="message">The message.  例如："尊敬的用户，您好！您的短信验证码是：" + code + " 【乐冲刺】</param>
        public void SendMsg(string phonenumber,string message)
        {
            using (SmsAPIPortTypeClient clientservice = new SmsAPIPortTypeClient())
            {
                clientservice.SendMessageAsync("62737", "chinaisi", phonenumber, message, string.Empty);
            }
        }


    }
}
