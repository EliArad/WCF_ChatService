using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.IO;
using ChatServiceLib; 
using System.ServiceModel.Description;
using System.Net;
using System.Diagnostics;
using RegistryClassApi;
using Microsoft.Win32;
using System.Threading;


//http://www.c-sharpcorner.com/UploadFile/12b2b4/wcf-example-in-C-Sharp/
namespace TestServiceHost
{
    
    class Program
    {

        static string urlMeta, urlService = "";
        static ServiceHost host = null;

        
        static void host_Closed(object sender, EventArgs e)
        {
            Console.WriteLine("Service closed");
        }

        static void host_Closing(object sender, EventArgs e)
        {
            Console.WriteLine("Service closing ... stand by");
        }

        static void host_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("Service opened.");
            Console.WriteLine("Service URL:\t" + urlService);
            Console.WriteLine("Meta URL:\t" + urlMeta + " (Not that relevant)");
            Console.WriteLine("Waiting for clients...");
        }
        static void host_Opening(object sender, EventArgs e)
        {
            Console.WriteLine("Service opening ... Stand by");
        }         
        static void Main()
        {
            try
            {

                var t = new Thread(() =>
                {

                

                string ipAddress = "10.0.0.17";
                const string MegaPopServerRegistry = "SOFTWARE\\Eli\\ChatServer";
                clsRegistry reg = new clsRegistry();
                ipAddress = reg.GetStringValue(Registry.LocalMachine, MegaPopServerRegistry, "LocalIpAddress");

                // Create the url that is needed to specify
                // where the service should be started
                urlService = "net.tcp://" + ipAddress + ":8099/ChatService";

                // Instruct the ServiceHost that the type
                // that is used is a ServiceLibrary.service1
                host = new ServiceHost(typeof(ChatService));
                host.Opening += new EventHandler(host_Opening);
                host.Opened += new EventHandler(host_Opened);
                host.Closing += new EventHandler(host_Closing);
                host.Closed += new EventHandler(host_Closed);

                // The binding is where we can choose what
                // transport layer we want to use. HTTP, TCP ect.
                NetTcpBinding tcpBinding = new NetTcpBinding();
                tcpBinding.TransactionFlow = false;
                //tcpBinding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
                //tcpBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;
                //tcpBinding.Security.Mode = SecurityMode.Transport;
                // <- Very crucial


                // Add a endpoint
                host.AddServiceEndpoint(typeof(IChatService), tcpBinding, urlService);

                // A channel to describe the service.
                // Used with the proxy scvutil.exe tool
                ServiceMetadataBehavior metadataBehavior;
                metadataBehavior =
                  host.Description.Behaviors.Find<ServiceMetadataBehavior>();
                if (metadataBehavior == null)
                {
                    // This is how I create the proxy object
                    // that is generated via the svcutil.exe tool
                    metadataBehavior = new ServiceMetadataBehavior();
                    metadataBehavior.HttpGetUrl = new Uri("http://" + ipAddress + ":8002/ChatService");
                    metadataBehavior.HttpGetEnabled = true;
                    metadataBehavior.ToString();
                    host.Description.Behaviors.Add(metadataBehavior);
                    urlMeta = metadataBehavior.HttpGetUrl.ToString();
                }

                host.Open();
                });
                t.IsBackground = true;
                t.Start();
                Console.WriteLine("Chat WCF Host service strated @" + DateTime.Now.ToString());
                Console.ReadLine();
                host.Close();
                System.Threading.Thread.Sleep(1000);

            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                Console.WriteLine("Press enter to exit..");
                Console.ReadLine();
            }
        }
    }
}
