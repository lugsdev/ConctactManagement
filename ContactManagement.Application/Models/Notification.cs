using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Models
{
    public class Notification
    {
        public Notification(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            Message = message;
            HttpStatusCode = statusCode;
        }

        public string Message { get; }
        public HttpStatusCode HttpStatusCode { get; }
    }
}
