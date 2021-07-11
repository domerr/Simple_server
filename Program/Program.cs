using System;
using System.Collections;
using System.Collections.Generic;

namespace ProgramNS
{
    public enum Modifier
    {
        Time,
        ToUpper
    }

    public enum ResponseType
    {
        ECHO,
        PING
    }

    interface ModificationInterface
    {
        string MakeModification(string request);
    }

    public class ModificationPing : ModificationInterface
    {
        private const string Pong = "Pong";
        public string MakeModification(string request)
        {
            return Pong;
        }
    }

    public class ModificationEcho : ModificationInterface
    {
        public string MakeModification(string request)
        {
            int i = request.IndexOf(" ") + 1;
            return request[i..];
        }
    }

    public class ModificationTimeFlag : ModificationInterface
    {
        public string MakeModification(string response)
        {
            string localDate = DateTime.Now.ToString("h:mm:ss");
            response = localDate + " " + response;

            return response;
        }
    }

    public class ModificationToUpper : ModificationInterface
    {
        public string MakeModification(string response)
        {

            return response.ToUpper();
        }
    }

    //Class responsible for indicating server modifiers
    public class ServerStatus
    {
        private const string Time = "TIME";
        private const string ToUpper = "TOUPPER";

        public static ArrayList GetStatus(string[] modifiers)
        {
            var serverStatus = new ArrayList();

            foreach (string element in modifiers)
            {
                SetModifier(serverStatus, element);
            }
            return serverStatus;
        }

        //To add server modifier add if
        private static void SetModifier(ArrayList serverStatus, string element)
        {
            if (element == Time)
            {
                serverStatus.Add(Modifier.Time);
            }

            if (element == ToUpper)
            {
                serverStatus.Add(Modifier.ToUpper);
            }
        }
    }

    //Class responsible for managing all responses
    public class Response
    {
        private const string BadRequest = "Bad request";
        private ArrayList serverStatus;
        private ResponseType responseType;
        ModificationPing modificationPing = new ModificationPing();
        ModificationEcho modificationEcho = new ModificationEcho();
        ModificationTimeFlag modificationTimeFlag = new ModificationTimeFlag();
        ModificationToUpper modificationToUpper = new ModificationToUpper();

        public Response(ArrayList serverStatus, ResponseType responseType)
        {
            this.serverStatus = serverStatus;
            this.responseType = responseType;
        }

        public string GetResponse(string request)
        {
            var response = MakeResponse(request);

            //Add if for serever modifiers
            if (serverStatus.Contains(Modifier.Time))
            {
                response = modificationTimeFlag.MakeModification(response);
            }

            if (serverStatus.Contains(Modifier.ToUpper))
            {
                response = modificationToUpper.MakeModification(response);
            }

            return response;
        }

        private string MakeResponse(string request)
        {
            //Add if for message modifiers
            if (responseType.Equals(ResponseType.PING))
            {
                return modificationPing.MakeModification(request);
            }

            if (responseType.Equals(ResponseType.ECHO))
            {
                return modificationEcho.MakeModification(request);
            }

            return BadRequest;
        }
    }

    public class RequestException : Exception
    {
        public RequestException(string message) : base(message)
        {

        }
    }

    //Class responsible for checking requests
    public class Request
    {
        private const string Ping = "PING";
        private const string Echo = "ECHO";
        private const string BadRequest = "Bad request";

        public static ResponseType CheckRequest(string request)
        {
            //Add if for message modifiers
            if (request.StartsWith(Echo))
            {
                return ResponseType.ECHO;
            }

            if (request.StartsWith(Ping))
            {
                return ResponseType.PING;
            }

            throw new RequestException(BadRequest);
        }
    }

    public class Server
    {
        private ArrayList serverStatus;


        public Server(params string[] modifiers)
        {
            serverStatus = ServerStatus.GetStatus(modifiers);
        }

        public string Process(string request)
        {
            // TODO: Implement a simple server that can process incoming requests and respond with apropriate responses
            // Requests are strings containing one command and optionally command arguments.

            // There are two commands:
            // PING - responds with "Pong"
            // ECHO [argument] - responds with the value of the argument, example: "ECHO Foo" -> "Foo"

            // Requests that cannot be parsed as valid commands should return "Bad request"

            // In additon there are two command modifiers that the server can be initialized with:
            // TIME - adds a timestamp before the response of all command: "PING" -> "10:20:03 Pong"
            // TOUPPER - converts the response of all commands to uppercase, example: "ECHO Foo" -> "FOO"

            // When implementing the server, assume that the list of both commands and modifiers will be extended in the future.

            // Note: This task is not about implementing HTTP server but rather providing an implementation of Server class
            // Note: Code can be provided in any way that you prefer. You don't need to use dotnetfiddle.net
            ResponseType responseType;

            try
            {
                responseType = Request.CheckRequest(request);
            }
            catch (RequestException e)
            {
                return e.Message;
            }
            var response = new Response(serverStatus, responseType);
            return response.GetResponse(request);
        }

    }

    public class Program
    {
        public static void Main()
        {
            //Console.WriteLine("Server1");
            var server1 = new Server();
            Console.WriteLine(server1.Process("ECHO Hello world!"));    // Hello world!
            Console.WriteLine(server1.Process("PING"));                 // Pong
            Console.WriteLine(server1.Process("XYZV"));                 // Bad request				

            //Console.WriteLine("Server2");
            var server2 = new Server("TOUPPER");
            Console.WriteLine(server2.Process("ECHO Hello world!"));    // HELLO WORLD!
            Console.WriteLine(server2.Process("PING"));                 // PONG

            //Console.WriteLine("Server3");
            var server3 = new Server("TIME");
            Console.WriteLine(server3.Process("ECHO Hello world!"));    // 10:20:30 Hello world!
            Console.WriteLine(server3.Process("PING"));                 // 10:20:30 Pong

            //Console.WriteLine("Server4");
            var server4 = new Server("TIME", "TOUPPER");
            Console.WriteLine(server4.Process("ECHO Hello world!"));    // 10:20:30 HELLO WORLD!
            Console.WriteLine(server4.Process("PING"));                 // 10:20:30 PONG		
        }
    }
}