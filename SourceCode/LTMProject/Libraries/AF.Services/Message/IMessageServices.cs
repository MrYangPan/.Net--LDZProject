using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.Services.Message
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMessageServices
    {
        /// <summary>
        /// Sends the MSG.
        /// </summary>
        /// <param name="phonenumber">The phonenumber.</param>
        /// <param name="message">The message.</param>
        void SendMsg(string phonenumber, string message);
    }
}
