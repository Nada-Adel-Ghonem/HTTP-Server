using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    public enum RequestMethod
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion
    {
        HTTP10,
        HTTP11,
        HTTP09
    }

    class Request
    {
        string[] requestLines;
        RequestMethod method;
        public string relativeURI;
        Dictionary<string, string> headerLines;

        public Dictionary<string, string> HeaderLines
        {
            get { return headerLines; }
        }

        HTTPVersion httpVersion;
        string requestString;
        string[] contentLines;

        public Request(string requestString)
        {
            this.requestString = requestString;
        }

        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>
        public bool ParseRequest()
        {
            //TODO: parse the receivedRequest using the \r\n delimeter   

            string[] stringSeparators = new string[] { "\r\n" };
            requestLines = requestString.Split(stringSeparators, StringSplitOptions.None);

            // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)
            if (requestLines.Length < 3)
            {
                return false;
            }

            // Parse Request line
            string[] subRequestLine = requestLines[0].Split(' ');
            if (subRequestLine[0] == "GET" || subRequestLine[0] == "get")
            {
                method = RequestMethod.GET;
            }
            else if (subRequestLine[0] == "POST" || subRequestLine[0] == "post")
            {
                method = RequestMethod.POST;
            }
            else
                method = RequestMethod.HEAD;

            relativeURI = subRequestLine[1];

            string fullVersion = subRequestLine[2];
            string version = fullVersion.Substring(fullVersion.Length - 3);
            if (version == "1.1")
                httpVersion = HTTPVersion.HTTP11;
            else if (version == "1.0")
                httpVersion = HTTPVersion.HTTP10;
            else
                httpVersion = HTTPVersion.HTTP09;

            headerLines = new Dictionary<string, string>();

            // Load header lines into HeaderLines dictionary
            int i = 1;
            int j = 0;

            string[] stringSeparators2 = new string[] { ": " };
            while (!string.IsNullOrEmpty(requestLines[i]))
            {
                string headerContent = requestLines[i];
                string[] data = headerContent.Split(stringSeparators2, StringSplitOptions.None);
                headerLines.Add(data[0], data[1]);
                i++;
                j = i;
            }

            // Validate blank line exists
            return string.IsNullOrEmpty(requestLines[j]);
        }

        private bool ParseRequestLine()
        {
            throw new NotImplementedException();
        }

        private bool ValidateIsURI(string uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private bool LoadHeaderLines()
        {
            throw new NotImplementedException();
        }


    }
}
