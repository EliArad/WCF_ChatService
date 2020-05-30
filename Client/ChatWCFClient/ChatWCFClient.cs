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
    public class ChatWCFClient : IChatServiceCallback
    {
        IChatService m_client = null;
        DuplexChannelFactory<IChatService> pipeFactory;

        public delegate void NotifyCallbackMessageCallback(string fieldGuid, string ipAddress, int portNumber, int code, string msg, DateTime date);
        public delegate void NotifyCallbackDataCallback(string fieldGuid, string ipAddress, int portNumber, int code, byte[] buf, int size, DateTime date);

        public event NotifyCallbackMessageCallback msgEvent = null;
        public event NotifyCallbackDataCallback    dataEvent = null;

        Dictionary<string, NotifyCallbackMessageCallback> pMsgCallback = new Dictionary<string, NotifyCallbackMessageCallback>();
        Dictionary<string, NotifyCallbackDataCallback> pDataCallback = new Dictionary<string, NotifyCallbackDataCallback>();
        string m_ipAddress;
        bool m_IsConnected = false;

        public delegate void ClientCallbackMessage(string fieldGuid, string ipAddress, int code, string msg, DateTime startTime);
        ClientCallbackMessage pClientCallback;
        string m_fieldGuid; 

        public ChatWCFClient(string ipAddress, string userName, string password, ClientCallbackMessage callback = null)
        {
            try
            {
                pClientCallback = callback;
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
                     new InstanceContext(this),
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
          
        public void Registration(string baseGuid)
        {
            if (m_client != null)
            {
                try
                {
                     
                    m_client.Registration(baseGuid);
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
                ServiceClose(m_fieldGuid, m_ipAddress);
            });

            ((ICommunicationObject)m_client).Faulted += new EventHandler(delegate
            {

                ServiceFault(m_fieldGuid, m_ipAddress);
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
        public void setMsgCallback(string Controller, NotifyCallbackMessageCallback p)
        {
            if (pMsgCallback.ContainsKey(Controller) == false)
            {
                pMsgCallback.Add(Controller, p);
            }
            else
            {
                pMsgCallback[Controller] = p;
            }
        }
         

        public void setDataCallback(string Controller, NotifyCallbackDataCallback p)
        {
            if (pDataCallback.ContainsKey(Controller) == false)
            {
                pDataCallback.Add(Controller, p);
            }
            else
            {
                pDataCallback[Controller] = p;
            }
        }

        public void RemoveDataCallback(string Controller, NotifyCallbackDataCallback p)
        {
            if (pDataCallback.ContainsKey(Controller) == false)
                pDataCallback.Remove(Controller);
        }

        public void RemoveMsgCallback(string Controller, NotifyCallbackMessageCallback p)
        {
            if (pMsgCallback.ContainsKey(Controller) == false)
                pMsgCallback.Remove(Controller);
        }
        public void SendMsgCallback(string fieldGuid, string Controller, string serverAddress,  int portNumber, int code, string msg, DateTime date)
        {
            if (pMsgCallback.ContainsKey(Controller) == true)
            {
                pMsgCallback[Controller](fieldGuid,serverAddress, portNumber, code, msg, date);
            }
        }
        public void SendDataCallback(string fieldGuid, string Controller, int portNumber, int code, string serverAddress, byte[] buf, int size, DateTime date)
        {
            if (pDataCallback.ContainsKey(Controller) == true)
            {
                pDataCallback[Controller](fieldGuid,serverAddress, portNumber, code, buf, size, date);
            }
        }

        public void NotifyDataCallback(string fieldGuid, string ipAddress, int portNumber, int code, byte[] buf, int size, DateTime date)
        {
            if (dataEvent != null)
                dataEvent.Invoke(fieldGuid, ipAddress, portNumber, code, buf, size, date);

            SendAllDataCallbacks(fieldGuid, ipAddress, portNumber,code, buf, size, date);
        }
        public void SendAllDataCallbacks(string fieldGuid, string serverAddress, int portNumber, int code, byte[] buf, int size, DateTime date)
        {
            List<NotifyCallbackDataCallback> items = new List<NotifyCallbackDataCallback>();
            items.AddRange(pDataCallback.Values);
            for (int i = 0; i < items.Count; i++)
                items[i](fieldGuid, serverAddress, portNumber,code, buf, size, date);

        }
        public void SendAllMsgCallbacks(string fieldGuid, string serverAddress, int portNumber, int code, string msg, DateTime date)
        {
            List<NotifyCallbackMessageCallback> items = new List<NotifyCallbackMessageCallback>();
            items.AddRange(pMsgCallback.Values);
            for (int i = 0; i < items.Count; i++)
                items[i](fieldGuid, serverAddress, portNumber, code, msg, date);

        }

        public void NotifyCallbackMessage(string fieldGuid,string ipAddress, int portNumber, int code, string msg, DateTime date)
        {
            if (msgEvent != null)
                msgEvent.Invoke(fieldGuid, ipAddress, portNumber, code, msg, date);

            SendAllMsgCallbacks(fieldGuid, ipAddress, portNumber , code, msg, date);
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
