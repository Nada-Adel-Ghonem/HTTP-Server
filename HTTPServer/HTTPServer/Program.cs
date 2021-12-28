using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer {
    class Program {
        static void Main(string[] args) {
            // TODO: Call CreateRedirectionRulesFile() function to create the rules of redirection 
            CreateRedirectionRulesFile();
            //Start server
            string filePath = "D:\\redirectionRules.txt";
            // 1) Make server object on port 1000
            Server server = new Server(1000, filePath);
            // 2) Start Server
            server.StartServer();
        }
        static string[] RedirectionRules = new string[] {
            "aboutus.html,aboutus2.html",
        };
        static void CreateRedirectionRulesFile() {
            // TODO: Create file named redirectionRules.txt
              FileStream redirectionRulesFile = new FileStream("D:\\redirectionRules.txt", FileMode.OpenOrCreate);
              StreamWriter streamWriter = new StreamWriter(redirectionRulesFile);
              // each line in the file specify a redirection rule
              foreach (string rule in RedirectionRules) {
                  // example: "aboutus.html,aboutus2.html"
                  // means that when making request to aboustus.html,, it redirects me to aboutus2
                  streamWriter.WriteLine(rule);
              }
              streamWriter.Close();
              redirectionRulesFile.Close();
        }

    }
}
