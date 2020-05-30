using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;

namespace ChatServiceLib
{


    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
    ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class ChatService : IChatService 
    {

        object m_lock = new object();
        Thread m_broadcastThread;
        Dictionary<Client, IDuplexServiceCallback> clients = new Dictionary<Client, IDuplexServiceCallback>();
         
        public ChatService()
        {
            m_broadcastThread = new Thread(BroadcastThread);
            m_broadcastThread.Start();
        }

        
        public bool Connect(string userName, string freedesc, Guid serverGuid, DateTime time)
        {

            lock (m_lock)
            {
                Client client = new Client
                {
                    FreeDesc = freedesc,
                    Name = userName,
                    ServerGuid = serverGuid,
                    Time = time
                };

                IDuplexServiceCallback callback = OperationContext.Current.GetCallbackChannel<IDuplexServiceCallback>();
                if (IsClientConnected(client) == false)
                {
                    clients.Add(client, callback);
                    Console.WriteLine("Number of connected clients: " + clients.Count);
                    var t = new Thread(() =>
                    {
                        callback.UserJoin(client, true);
                    });
                    t.Start();
                }
                else
                {
                    clients[client] = callback;
                    var t = new Thread(() =>
                    {
                        callback.UserJoin(client, false);
                    });
                    t.Start();
                    return true;
                }
                return true;
            }
            
        }
        bool IsClientConnected(Client client)
        {
            foreach (KeyValuePair<Client, IDuplexServiceCallback> p in clients)
            {
                if (p.Key.Name == client.Name && p.Key.ServerGuid == client.ServerGuid)
                {
                    return true;
                }
            }
            return false;
        }

        public void Disconnect(string userName, Guid ServerGuid, bool notify)
        {
            //lock (m_lock)
            {
                foreach (KeyValuePair<Client, IDuplexServiceCallback> p in clients)
                {
                    if (userName == p.Key.Name && ServerGuid == p.Key.ServerGuid)
                    {
                        if (notify == true)
                        {
                            var t = new Thread(() =>
                            {
                                p.Value.UserLeave(userName, ServerGuid, DateTime.Now);
                                this.clients.Remove(p.Key);
                            });
                            t.Start();
                        }       
                        else
                        {
                            this.clients.Remove(p.Key);
                        }
                    }
                }
            }
        }
        public string GetVersion()
        {
            return "1";
        }
        IDuplexServiceCallback GetClient(string toReceiverName, Guid ToReceiverServerGuid, out Client c)
        {
            foreach (KeyValuePair<Client, IDuplexServiceCallback> client in clients)
            {
                if (client.Key.Name != toReceiverName || client.Key.ServerGuid != ToReceiverServerGuid)
                    continue;
                c = client.Key;
                return client.Value;
            }
            c = null;
            return null;
        }

        enum MSG_TYPE
        {
            BROADCAST,
            SEND_MESSAGE
        }
        struct Message
        {
            public string Name;
            public Guid ServerGuid;
            public string fromUserName;
            public Guid fromServerGuid;
            public string message;
            public bool broadcast;
            public MSG_TYPE msgType;
            public IDuplexServiceCallback callback;
        }
        ConcurrentQueue<Message> m_broadcastMessage = new ConcurrentQueue<Message>();
        AutoResetEvent m_broadcastEvent = new AutoResetEvent(false);
        bool m_running = true;
        void BroadcastThread()
        {
            while (m_running)
            {
                m_broadcastEvent.WaitOne();
                if (m_running == false)
                    return;
                do
                {
                    bool b = m_broadcastMessage.TryDequeue(out Message msg);
                    if (b == true)
                    {
                        try
                        {
                            if (msg.msgType == MSG_TYPE.BROADCAST)
                            {
                                msg.callback.ReceiveBroadcast(msg.Name, msg.ServerGuid, msg.fromUserName, msg.fromServerGuid, msg.message, msg.broadcast, DateTime.Now);
                            } else                            
                            if (msg.msgType == MSG_TYPE.SEND_MESSAGE)
                            {
                                msg.callback.NotifyMessage(msg.Name, msg.ServerGuid, msg.fromUserName, msg.fromServerGuid, msg.message, DateTime.Now);
                                //Thread.Sleep(10);
                                //msg.callback.NotifyMessageSent(DateTime.Now, true);
                            }
                        }
                        catch (Exception err)
                        {

                        }
                        Thread.Sleep(1);
                    }
                } while (m_broadcastMessage.Count > 0);
            }
        }
        public bool Broadcast(string fromUserName, Guid fromServerGuid, string message)
        {
            lock (m_lock)
            {
                
                bool broadcast;
                foreach (KeyValuePair<Client, IDuplexServiceCallback> client in clients)
                {
                    Client c = client.Key;
                    try
                    {
                        IDuplexServiceCallback callback = client.Value;
                        if (client.Key.Name == fromUserName && client.Key.ServerGuid == fromServerGuid)
                            broadcast = false;
                        else
                            broadcast = true;


                        Message m = new Message {
                            Name = c.Name,
                            ServerGuid = c.ServerGuid,
                            fromUserName = fromUserName,
                            fromServerGuid = fromServerGuid,
                            message = message,
                            msgType = MSG_TYPE.BROADCAST,
                            broadcast = broadcast
                        };
                        m.callback = callback;
                        m_broadcastMessage.Enqueue(m);
                    }
                    catch (Exception err)
                    {
                        if (c != null)
                            Disconnect(c.Name, c.ServerGuid, true);
                        Console.WriteLine(err.Message);
                        return false;
                    }
                }
                m_broadcastEvent.Set();
                return true;
                 
            }
        }

        public bool BroadcastServer(string fromUserName, Guid fromServerGuid, Guid toServerGuid, string message)
        {
            lock (m_lock)
            {
                Client c = null;
                try
                {
                    bool broadcast;
                    foreach (KeyValuePair<Client, IDuplexServiceCallback> client in clients)
                    {
                        if (client.Key.Name == fromUserName && client.Key.ServerGuid == fromServerGuid)
                            broadcast = false;
                        else 
                            broadcast = true;

                        if (client.Key.ServerGuid == toServerGuid)
                        {
                            c = client.Key;
                            var t = new Thread(() =>
                            {
                                client.Value.ReceiveBroadcast(client.Key.Name, client.Key.ServerGuid, fromUserName, fromServerGuid, message, broadcast, DateTime.Now);
                            });
                            t.Start();
                        }
                    }
                    return true;
                }
                catch (Exception err)
                {
                    if (c != null)
                        Disconnect(c.Name, c.ServerGuid, true);
                    Console.WriteLine(err.Message);
                    return false;
                }
            }
        }

        public bool SendMessage(string fromUserName, 
                                Guid fromServerGuid, 
                                string toUserName, 
                                Guid toServerName,
                                string message)
        {
            lock (m_lock)
            {
                Client c = null;
                try
                {                     
                    IDuplexServiceCallback p = GetClient(toUserName, toServerName, out c);
                    if (p != null)
                    {                         
                         
                        p = GetClient(toUserName, toServerName, out c);
                        IDuplexServiceCallback p1 = GetClient(fromUserName, fromServerGuid, out c);

                        Message m = new Message
                        {
                            Name = c.Name,
                            ServerGuid = c.ServerGuid,
                            fromUserName = fromUserName,
                            fromServerGuid = fromServerGuid,
                            message = message,
                            msgType = MSG_TYPE.SEND_MESSAGE,
                            broadcast = false
                        };
                        m.callback = p;
                        m_broadcastMessage.Enqueue(m);
                        m_broadcastEvent.Set();                             
                       
                       
                        return true;
                    }
                    else
                    {
                        IDuplexServiceCallback p1 = GetClient(fromUserName, fromServerGuid, out c);
                        if (p1 != null)
                        {
                            p1.NotifyMessageSent(DateTime.Now, false);
                            return true;
                        }
                        return false;
                    }                                             
                }
                catch (Exception err)
                {
                    if (c != null)
                        Disconnect(c.Name, c.ServerGuid, true);
                    Console.WriteLine(err.Message);
                    return false;
                }
            }
        }
    }
}
