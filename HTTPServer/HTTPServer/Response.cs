using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{

    public enum StatusCode
    {
        OK = 200,
        InternalServerError = 500,
        NotFound = 404,
        BadRequest = 400,
        Redirect = 301
    }

    class Response
    {
        string responseString;
        public string ResponseString
        {
            get
            {
                return responseString;
            }
        }

        public Response(StatusCode code, string contentType, string content, string redirectoinPath)
        {

            // TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])
            responseString.Concat(GetStatusLine(code) + "\r\n");
            responseString.Concat("Content-Type: " + contentType + "\r\n");
            responseString.Concat("Content-Length: " + content.Length + "\r\n");
            responseString.Concat("Date:" + System.DateTime.Now.ToUniversalTime().ToString("r") + "\r\n");

            if (code.Equals(StatusCode.Redirect))
                responseString.Concat("Location: " + redirectoinPath + "\r\n");

            responseString.Concat("\r\n" + content);

            // TODO: Create the request string ??

        }

        private string GetStatusLine(StatusCode code)
        {
            // TODO: Create the response status line and return it
            string statusLine = string.Empty;
            statusLine.Concat("HTTP/1.1" + ' ' + (int) code + ' ' + code);
            return statusLine;
        }
    }
}
