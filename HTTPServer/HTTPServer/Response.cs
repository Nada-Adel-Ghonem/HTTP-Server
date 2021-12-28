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
            string header = "";
            // TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])
            //header.Concat(GetStatusLine(code) + "\r\n");
            header = string.Concat(header, "Content-Type: " + contentType + "\r\n");
            header = string.Concat(header, "Content-Length: " + content.Length + "\r\n");
            header = string.Concat(header, "Date:" + DateTime.Now.ToString() + "\r\n");
           
            if (redirectoinPath != null)
                header = string.Concat(header, "Location: " + redirectoinPath + "\r\n");

            responseString = GetStatusLine(code) + "\r\n" + header + "\r\n" + content;

            // TODO: Create the request string ??

        }

        private string GetStatusLine(StatusCode code)
        {

            // TODO: Create the response status line and return it
            string statusLine = string.Empty;
            string errorMessage;
            switch (code)
            {
                case StatusCode.BadRequest:
                    errorMessage = "Bad Request";
                    break;
                case StatusCode.OK:
                    errorMessage = "OK";
                    break;
                case StatusCode.InternalServerError:
                    errorMessage = "Internal Server Error";
                    break;
                case StatusCode.Redirect:
                    errorMessage = "Redirect";
                    break;
                case StatusCode.NotFound:
                    errorMessage = "Not Found";
                    break;
                default:
                    errorMessage = "";
                    break;

            }

            statusLine = string.Concat(Configuration.ServerHTTPVersion + " " + (int)code + " " + errorMessage);

            return statusLine;
        }
    }
}
