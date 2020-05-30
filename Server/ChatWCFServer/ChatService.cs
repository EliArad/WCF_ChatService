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
 
            
            if (IsClientConnected(client) == false)
            {
                IDuplexServiceCallback callback = OperationContext.Current.GetCallbackChannel<IDuplexServiceCallback>();
                clients.Add(client, callback);             
                Console.WriteLine("Number of connected clients: " + clients.Count);
                var t = new Thread(() =>
                {
                    callback.UserJoin(client);
                });
                t.Start();
            }
            else
            {            
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
            //lock (m_connectLock)
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
            //lock (m_connectLock)
            {
                Client c = null;
                try
                {
                    Console.WriteLine("{0},{1},{2},{3},{4},{5}", msg.SenderName, msg.toReceiverName, msg.Content, msg.FromServerGuid, msg.ToReceiverServerGuid, msg.Time);
                    bool found = false;
                    foreach (KeyValuePair<Client, IDuplexServiceCallback> client in clients)
                    {
                        if (client.Key.Name == msg.SenderName && client.Key.ServerGuid == msg.FromServerGuid)
                            continue;

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
            //lock (m_connectLock)
            {
                Client c = null;
                try
                {
                    bool found = false;
                    Console.WriteLine("{0},{1},{2},{3},{4},{5}", msg.SenderName, msg.toReceiverName, msg.Content, msg.FromServerGuid, msg.ToReceiverServerGuid, msg.Time);
                    foreach (KeyValuePair<Client, IDuplexServiceCallback> client in clients)
                    {
                        if (client.Key.Name == msg.SenderName && client.Key.ServerGuid == msg.FromServerGuid)
                            continue;

                        if (client.Key.ServerGuid == msg.ToReceiverServerGuid)
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

        public bool Say(Message msg)
        {
            //lock (m_connectLock)
            {
                Client c = null;
                try
                {
                    Console.WriteLine("{0},{1},{2},{3},{4},{5}", msg.SenderName, msg.toReceiverName, msg.Content, msg.FromServerGuid, msg.ToReceiverServerGuid, msg.Time);
                    bool found = false;
                    foreach (KeyValuePair<Client, IDuplexServiceCallback> client in clients)
                    {
                        if (client.Key.Name == msg.toReceiverName && client.Key.ServerGuid == msg.ToReceiverServerGuid)
                            continue;

                        if (client.Key.Name == msg.SenderName && client.Key.ServerGuid == msg.FromServerGuid)
                        {
                            IDuplexServiceCallback p = GetClient(msg.toReceiverName, msg.ToReceiverServerGuid, out c);
                            if (p != null)
                            {
                                var t = new Thread(() =>
                                {
                                    p.Receive(msg);
                                });
                                t.Start();
                                
                                found = true;
                            }
                        }
                    }
                    if (found == false)
                    {
                        IDuplexServiceCallback p = GetClient(msg.SenderName, msg.FromServerGuid, out c);
                        if (p != null)
                        {
                            return false;
                        }
                    }
                    return true;
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
            //lock (m_connectLock)
            {
                Client c = null;
                try
                {
                    Console.WriteLine("{0},{1},{2},{3},{4},{5}", msg.SenderName, msg.toReceiverName, msg.Content, msg.FromServerGuid, msg.ToReceiverServerGuid, msg.Time);

                    foreach (KeyValuePair<Client, IDuplexServiceCallback> client in clients)
                    {
                        if (client.Key.Name == msg.toReceiverName && client.Key.ServerGuid == msg.ToReceiverServerGuid)
                        {
                            c = client.Key;
                            var t = new Thread(() =>
                            {
                                client.Value.Receive(msg);
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
