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

        string m_userName;
        string m_freedesc;
        Guid m_serverGuid;

        public string UserName
        {
            get
            {
                return m_userName;
            }
        }

        public Guid ServerGuid
        {
            get
            {
                return m_serverGuid;
            }
        }

        public ChatWCFClient(string ipAddress, IChatServiceCallback callback)
        {
            try
            {

              
                m_ipAddress = ipAddress;
                NetTcpBinding tcpBinding = new NetTcpBinding();
                tcpBinding.OpenTimeout = TimeSpan.FromSeconds(5);
                tcpBinding.ReceiveTimeout = TimeSpan.FromSeconds(5);
                tcpBinding.SendTimeout = TimeSpan.FromSeconds(5);
                tcpBinding.CloseTimeout = TimeSpan.FromSeconds(5);
                tcpBinding.MaxConnections = 2000;

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

        public bool Leave(out string outMessage)
        {
            outMessage = string.Empty;
            if (m_client != null && m_IsConnected == true)
            {
                try
                {
                    m_client.Disconnect(m_userName, m_serverGuid, true);
                    return true;
                }
                catch (Exception err)
                {
                    outMessage = err.Message;
                    return false;
                }
            }
            else
            {
                outMessage = "Not initialized";
                return false;
            }

        }

        public bool SendMessage(string toUserName, Guid toServerName, string message, out string outMessage)
        {
            outMessage = string.Empty;
            if (m_client != null && m_IsConnected == true)
            {
                try
                {
                    bool b = m_client.SendMessage(m_userName, m_serverGuid, toUserName, toServerName, message);
                    return b;
                }
                catch (Exception err)
                {
                    outMessage = err.Message;
                    return false;
                }
            }
            else
            {
                outMessage = "Not initialized";
                return false;
            }

        }
        public bool Connect(string userName, string freedesc, Guid serverGuid, DateTime time, out string outMessage)
        {
            outMessage = string.Empty;
            if (m_client != null)
            {
                try
                {
                    m_userName = userName;
                    m_freedesc = freedesc;
                    m_serverGuid = serverGuid;
                    Task<bool> b = m_client.ConnectAsync(userName, freedesc, serverGuid, time);
                    m_IsConnected = true;
                    return b.Result;
                }
                catch (Exception err)
                {
                    outMessage = err.Message;
                    return false;
                }
            }
            else
            {
                outMessage = "Not initialized";
                return false;
            }

        }
         

        public string GetVersion()
        {
            if (m_client != null && m_IsConnected == true)
            {
                return m_client.GetVersion();
            }
            else
            {
                throw (new SystemException("Not initialized"));
            }
        }


        public bool Broadcast(string msg, out string outMessage)
        {

            outMessage = string.Empty;
            if (m_client != null && m_IsConnected == true)
            {
                try
                {
                    bool b = m_client.Broadcast(m_userName, m_serverGuid, msg);
                    return b;
                }
                catch (Exception err)
                {
                    outMessage = err.Message;
                    return false;
                }
            }
            else
            {
                outMessage = "Not initialized";
                return false; 
            }
        }
        public bool Close(out string outMessage)
        {
            outMessage = string.Empty;
            try
            {
                m_client.Disconnect(m_userName, m_serverGuid,false);
                CloseClient();
                return true;
            }
            catch (Exception err)
            {
                outMessage = err.Message;
                return false;
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
