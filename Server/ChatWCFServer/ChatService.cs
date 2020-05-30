using System;
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

        public void Load(string configFileName)
        {
             
        }
        public bool Connect(string userName, string freedesc, Guid serverGuid, DateTime time)
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

        public void Disconnect(string userName, Guid ServerGuid)
        {
            //lock (m_lock)
            {
                foreach (KeyValuePair<Client, IDuplexServiceCallback> p in clients)
                {
                    if (userName == p.Key.Name && ServerGuid == p.Key.ServerGuid)
                    {                       
                        var t = new Thread(() =>
                        {
                            p.Value.UserLeave(userName, ServerGuid, DateTime.Now);
                            this.clients.Remove(p.Key);
                        });
                        t.Start();                            
                       
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

        public bool Broadcast(Message msg)
        {
            //lock (m_lock)
            {
                Client c = null;
                try
                {
                    bool found = false;
                    foreach (KeyValuePair<Client, IDuplexServiceCallback> client in clients)
                    {
                        //if (client.Key.Name == msg.SenderName && client.Key.ServerGuid == msg.FromServerGuid)
                          //  continue;

                        c = client.Key;
                        var t = new Thread(() =>
                        {
                            client.Value.ReceiveBroadcast(msg);                           
                        });
                        t.Start();

                        found = true;
                    }
                    return found;
                }
                catch (Exception err)
                {
                    if (c != null)
                        Disconnect(c.Name, c.ServerGuid);
                    Console.WriteLine(err.Message);
                    return false;
                }
            }
        }

        public bool BroadcastServer(Message msg)
        {
            //lock (m_lock)
            {
                Client c = null;
                try
                {
                    bool found = false;
                    //Console.WriteLine("{0},{1},{2},{3},{4},{5}", msg.SenderName, msg.toReceiverName, msg.Content, msg.FromServerGuid, msg.ToReceiverServerGuid, msg.Time);
                    foreach (KeyValuePair<Client, IDuplexServiceCallback> client in clients)
                    {
                        //if (client.Key.Name == msg.SenderName && client.Key.ServerGuid == msg.FromServerGuid)
                          //  continue;

                        //if (client.Key.ServerGuid == msg.ToReceiverServerGuid)
                        {
                            c = client.Key;
                            var t = new Thread(() =>
                            {
                                client.Value.ReceiveBroadcast(msg);
                            });
                            t.Start();
                            found = true;
                        }
                    }
                    return found;
                }
                catch (Exception err)
                {
                    if (c != null)
                        Disconnect(c.Name, c.ServerGuid);
                    Console.WriteLine(err.Message);
                    return false;
                }
            }
        }

        public bool SendMessage(string fromUserName, 
                                Guid fromServerName, 
                                string toUserName, 
                                Guid toServerName,
                                string message)
        {
            //lock (m_lock)
            {
                Client c = null;
                try
                {                     
                    IDuplexServiceCallback p = GetClient(toUserName, toServerName, out c);
                    if (p != null)
                    {
                        var t = new Thread(() =>
                        {
                            p.NotifyMessage(fromUserName, fromServerName, toUserName, toServerName, message, DateTime.Now);
                            IDuplexServiceCallback p1 = GetClient(fromUserName, fromServerName, out c);
                            if (p1 != null)
                            {
                                p1.NotifyMessageSent(DateTime.Now, true);
                            }
                        });
                        t.Start();
                        return true;
                    }
                    else
                    {
                        IDuplexServiceCallback p1 = GetClient(fromUserName, fromServerName, out c);
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
                        Disconnect(c.Name, c.ServerGuid);
                    Console.WriteLine(err.Message);
                    return false;
                }
            }
        }
        public void Echo(Message msg)
        {
            //lock (m_lock)
            {
                Client c = null;
                try
                {
                    //Console.WriteLine("{0},{1},{2},{3},{4},{5}", msg.SenderName, msg.toReceiverName, msg.Content, msg.FromServerGuid, msg.ToReceiverServerGuid, msg.Time);

                    foreach (KeyValuePair<Client, IDuplexServiceCallback> client in clients)
                    {
                        //if (client.Key.Name == msg.toReceiverName && client.Key.ServerGuid == msg.ToReceiverServerGuid)
                        {
                            c = client.Key;
                            var t = new Thread(() =>
                            {
                                //client.Value.NotifyMessage(msg);
                            });
                            t.Start();
                            break;
                        }
                    }
                }
                catch (Exception err)
                {
                    if (c != null)
                        Disconnect(c.Name, c.ServerGuid);
                    Console.WriteLine(err.Message);
                }
            }
        }

    }
}
