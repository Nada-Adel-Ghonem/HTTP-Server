using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace HTTPServer
{
    class Server
    {
        Socket serverSocket;

        public Server(int portNumber, string redirectionMatrixPath)
        {
            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            this.LoadRedirectionRules(redirectionMatrixPath);
            //TODO: initialize this.serverSocket
            this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, portNumber);
            this.serverSocket.Bind(ipEndPoint);
        }

        public void StartServer()
        {
            // TODO: Listen to connections, with large backlog.
            this.serverSocket.Listen(1000);

            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
            while (true)
            {
                //TODO: accept connections and start thread for each accepted connection.
                Socket clientSocket = this.serverSocket.Accept();
                Thread thread = new Thread(new ParameterizedThreadStart(HandleConnection));
                thread.Start(clientSocket);
            }
        }

        public void HandleConnection(object obj)
        {
            // TODO: Create client socket   
            Socket clientSocket = (Socket)obj;

            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
            clientSocket.ReceiveTimeout = 0;

            // TODO: receive requests in while true until remote client closes the socket.
            while (true)
            {
                try
                {
                    // TODO: Receive request
                    byte[] dataReceived = new byte[1024 * 1024];
                    int receivedLen = clientSocket.Receive(dataReceived);

                    // TODO: break the while loop if receivedLen==0
                    if (receivedLen == 0)
                        break;

                    // TODO: Create a Request object using received request string
                    string receivedString = Encoding.ASCII.GetString(dataReceived);
                    Request request = new Request(receivedString);


                    // TODO: Call HandleRequest Method that returns the response
                    Response response = HandleRequest(request);

                    // TODO: Send Response back to client
                    byte[] dataToSend = Encoding.ASCII.GetBytes(response.ResponseString);
                    clientSocket.Send(dataToSend);

                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class
                    Logger.LogException(ex);
                }
            }

            // TODO: close client socket
            clientSocket.Close();
        }

        Response HandleRequest(Request request)
        {
            // throw new NotImplementedException();
            string content;

            Response response;

            try
            {
                //TODO: check for bad request 
                bool isGoodRequest = request.ParseRequest();
                if (!isGoodRequest)
                {
                    content = LoadDefaultPage(Configuration.BadRequestDefaultPageName);
                    response = new Response(StatusCode.BadRequest, "text/html", content, null);
                    return response;
                }

                //TODO: map the relativeURI in request to get the physical path of the resource.
                //TODO: check for redirect

                if (Configuration.RedirectionRules.ContainsKey(request.relativeURI.Substring(1)))
                {
                    string redirectedToPathFile = Configuration.RootPath + '/' + GetRedirectionPagePathIFExist(request.relativeURI.Substring(1));
                    if (File.Exists(redirectedToPathFile))
                    {
                        content = LoadDefaultPage(GetRedirectionPagePathIFExist(request.relativeURI.Substring(1)));
                        response = new Response(StatusCode.Redirect, "text/html", content, Configuration.RedirectionRules[request.relativeURI.Substring(1)]);
                        return response;
                    }
                    else
                    {
                        content = LoadDefaultPage(Configuration.NotFoundDefaultPageName);
                        response = new Response(StatusCode.NotFound, "text/html", content, null);
                        return response;
                    }
                }

                //TODO: check file exists
                string filePath = Path.Combine(Configuration.RootPath, request.relativeURI.Substring(1));
                if (!File.Exists(filePath))
                {
                    content = LoadDefaultPage(Configuration.NotFoundDefaultPageName);
                    response = new Response(StatusCode.NotFound, "text/html", content, null);
                    return response;
                }
                else
                {
                    //TODO: read the physical file
                    content = File.ReadAllText(filePath);

                    // Create OK response
                    response = new Response(StatusCode.OK, "text/html", content, null);
                    return response;
                }

            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                // TODO: in case of exception, return Internal Server Error. 
                content = LoadDefaultPage(Configuration.InternalErrorDefaultPageName);
                response = new Response(StatusCode.InternalServerError, "text/html", content, null);
                return response;
            }

        }

        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty
            if (Configuration.RedirectionRules.ContainsKey(relativePath))
                return Configuration.RedirectionRules[relativePath];

            return string.Empty;
        }

        private string LoadDefaultPage(string defaultPageName)
        {
            string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
            // TODO: check if filepath not exist log exception using Logger class and return empty string
            // else read file and return its content


            // if(File.Exists(filePath))
            try
            {
                string content = File.ReadAllText(filePath);
                return content;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return string.Empty;
            }
        }

        private void LoadRedirectionRules(string filePath)
        {
            try
            {
                // TODO: using the filepath paramter read the redirection rules from file 
                string[] redirectionRules = File.ReadAllLines(filePath);
                Configuration.RedirectionRules = new Dictionary<string, string>();

                foreach (string line in redirectionRules)
                {
                    string[] keyValuePair = line.Split(',');
                    // then fill Configuration.RedirectionRules dictionary 
                    Configuration.RedirectionRules.Add(keyValuePair[0], keyValuePair[1]);
                }

            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);

                Environment.Exit(1);
            }
        }
    }
}
