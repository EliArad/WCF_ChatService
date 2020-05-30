using ChatWCFClientApi.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatWCFClientApi
{
    public class ChatWCFClient 
    {
        IChatService m_client = null;
        DuplexChannelFactory<IChatService> pipeFactory;
         
        string m_ipAddress;
        bool m_IsConnected = false;

        public delegate void ClientCallbackMessage(string fieldGuid, string ipAddress, int code, string msg, DateTime startTime);
        ClientCallbackMessage pClientCallback;
  

        public ChatWCFClient(string ipAddress, string userName, string password, IChatServiceCallback callback = null)
        {
            try
            {
               
                m_ipAddress = ipAddress;
                NetTcpBinding tcpBinding = new NetTcpBinding();
                tcpBinding.OpenTimeout = TimeSpan.FromSeconds(5);
                tcpBinding.ReceiveTimeout = TimeSpan.FromSeconds(5);
                tcpBinding.SendTimeout = TimeSpan.FromSeconds(5);
                tcpBinding.CloseTimeout = TimeSpan.FromSeconds(5);

                tcpBinding.TransactionFlow = false;
                //tcpBinding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
                //tcpBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;
                //tcpBinding.Security.Mode = SecurityMode.Transport;
                 
                pipeFactory =
                 new DuplexChannelFactory<IChatService>(
                     new InstanceContext(callback),
                    tcpBinding,
                     new EndpointAddress("net.tcp://" + ipAddress + ":8099/ChatService"));


                m_client = pipeFactory.CreateChannel();
 
            }
            catch (Exception err)
            {
                pipeFactory.Close();
                throw (new SystemException(err.Message));
            }
        }
          
        public void Connect(string userName, string freedesc, Guid serverGuid, DateTime time)
        {
            if (m_client != null)
            {
                try
                {                     
                    m_client.Connect(userName, freedesc, serverGuid, time);
                    m_IsConnected = true;
                }
                catch (Exception err)
                {
                    throw (new SystemException(err.Message));
                }
            }
            else
            {
                throw (new SystemException("Not initialized"));
            }

            ((ICommunicationObject)m_client).Closed += new EventHandler(delegate
            {
                //ServiceClose(m_fieldGuid, m_ipAddress);
            });

            ((ICommunicationObject)m_client).Faulted += new EventHandler(delegate
            {

                //ServiceFault(m_fieldGuid, m_ipAddress);
            });
        }
        private void ServiceClose(string fieldGuid, string ipAddress)
        {
            m_IsConnected = false;
            Console.WriteLine("Service closed!");
            if (pClientCallback != null)
            {
                pClientCallback(fieldGuid, m_ipAddress, 88, ipAddress, DateTime.Now);
            }
        }
        private void ServiceFault(string fieldGuid, string ipAddress)
        {
            m_IsConnected = false;
            Console.WriteLine("Service faulted!");
            ((ICommunicationObject)m_client).Faulted -= new EventHandler(delegate
            {

            });
            Thread t = new Thread(() =>
            {
                if (pClientCallback != null)
                {
                    pClientCallback(fieldGuid, m_ipAddress, 77, ipAddress, DateTime.Now);
                }
            });
            t.Start();
        }
          
        public string GetVersion()
        {
            if (m_client != null)
            {
                return m_client.GetVersion();
            }
            else
            {
                throw (new SystemException("Not initialized"));
            }
        }
          
        
        public void CloseClient()
        {
            try
            {
                pipeFactory.Close();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }
    }
}
