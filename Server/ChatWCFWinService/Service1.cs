using ChatServiceLib;
using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceProcess;
using System.Threading;


namespace ChatWCFWinService
{
    public partial class Service1 : ServiceBase
    {
        ServiceHost host = null;
        static string urlMeta, urlService = "";
        string m_ipAddress;
        bool m_running = true;
        Thread m_thread;
        public Service1()
        {
            InitializeComponent();
        }

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
         
        protected override void OnStart(string[] args)
        {
            
        }

        void OpenService()
        {
            try
            {
               
                string ipAddress = "10.0.0.17";

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
                tcpBinding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
                tcpBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
                tcpBinding.Security.Mode = SecurityMode.Transport;
                // <- Very crucial

                // Add a endpoint
                host.AddServiceEndpoint(typeof(ChatServiceLib.IChatService), tcpBinding, urlService);

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
               
               
            }
            catch (Exception err)
            {
                File.AppendAllText("c:\\chatwcf.txt", err.Message + Environment.NewLine);
            }
        }

        protected override void OnStop()
        {
            if (host != null)
            {
                m_running = false;
                if (m_thread != null)
                    m_thread.Join();
                host.Close();
            }
        }
    }
}
