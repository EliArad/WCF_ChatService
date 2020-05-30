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
        Dictionary<Client, IDuplexServiceCallback> clients = new Dictionary<Client, IDuplexServiceCallback>();
         
        public ChatService()
        {
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
                };

                IDuplexServiceCallback callback = OperationContext.Current.GetCallbackChannel<IDuplexServiceCallback>();
                if (IsClientConnected(client) == false)
                {
                    if (clients.Count >= 4)
                    {
                        Console.Write("e");
                    }
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
                    clients.Remove(client);
                    clients.Add(client, callback);
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

        bool IsClientConnected(string userName, Guid serverGuid)
        {
            foreach (KeyValuePair<Client, IDuplexServiceCallback> p in clients)
            {
                if (p.Key.Name == userName && p.Key.ServerGuid == serverGuid)
                {
                    return true;
                }
            }
            return false;
        }

        public void Disconnect(string userName, Guid ServerGuid, bool notify)
        {
            lock (m_lock)
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
                                Console.WriteLine("Number of connected clients: " + clients.Count);
                            });
                            t.Start();
                        }       
                        else
                        {
                            this.clients.Remove(p.Key);
                            Console.WriteLine("Number of connected clients: " + clients.Count);
                        }
                    }
                }
            }
        }

        public void _Disconnect(string userName, Guid ServerGuid, bool notify)
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
                                Console.WriteLine("Number of connected clients: " + clients.Count);
                            });
                            t.Start();
                        }
                        else
                        {
                            this.clients.Remove(p.Key);
                            Console.WriteLine("Number of connected clients: " + clients.Count);
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

        public bool Broadcast(string fromUserName, Guid fromServerGuid, string message)
        {
            lock (m_lock)
            {

                if (IsClientConnected(fromUserName, fromServerGuid) == false)
                {
                    return false;
                }


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

                        callback.ReceiveBroadcast(c.Name, c.ServerGuid, fromUserName, fromServerGuid, message, broadcast, DateTime.Now);

                       
                    }
                    catch (Exception err)
                    {
                        if (c != null)
                            _Disconnect(c.Name, c.ServerGuid, true);
                        Console.WriteLine(err.Message);
                        return false;
                    }
                }
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
                        _Disconnect(c.Name, c.ServerGuid, true);
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

                        p.NotifyMessage(c.Name, c.ServerGuid, fromUserName, fromServerGuid, message, DateTime.Now);

                        /*
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
                       */

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
                        _Disconnect(c.Name, c.ServerGuid, true);
                    Console.WriteLine(err.Message);
                    return false;
                }
            }
        }
    }
}
