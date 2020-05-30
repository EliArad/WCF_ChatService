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
        //Dictionary<Client, IDuplexServiceCallback> clients = new Dictionary<Client, IDuplexServiceCallback>();
        Dictionary<Tuple<string, Guid>, IDuplexServiceCallback> clients = new Dictionary<Tuple<string, Guid>, IDuplexServiceCallback>(); 

        public ChatService()
        {
        }

        
        public bool Connect(string userName, string freedesc, Guid serverGuid, DateTime time)
        {

            lock (m_lock)
            {
                
                IDuplexServiceCallback callback = OperationContext.Current.GetCallbackChannel<IDuplexServiceCallback>();
                if (IsClientConnected(userName , serverGuid) == false)
                {
                    clients.Add(new Tuple<string, Guid>(userName , serverGuid), callback);
                    Console.WriteLine("Number of connected clients: " + clients.Count);
                    var t = new Thread(() =>
                    {
                        callback.UserJoin(userName, serverGuid, true);
                    });
                    t.Start();
                }
                else
                {
                     
                    clients[new Tuple<string, Guid>(userName, serverGuid)] = callback;
                    Console.WriteLine("Number of connected clients: " + clients.Count);
                    var t = new Thread(() =>
                    {
                        callback.UserJoin(userName, serverGuid, false);
                    });
                    t.Start();
                    return true;
                }
                return true;
            }            
        }
        
        bool IsClientConnected(string userName, Guid serverGuid)
        {
            foreach (KeyValuePair<Tuple<string, Guid>, IDuplexServiceCallback> p in clients)
            {
                if (p.Key.Item1 == userName && p.Key.Item2 == serverGuid)
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
                foreach (KeyValuePair<Tuple<string, Guid>, IDuplexServiceCallback> p in clients)
                {
                    if (userName == p.Key.Item1 && ServerGuid == p.Key.Item2)
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
           
            
            foreach (KeyValuePair<Tuple<string, Guid>, IDuplexServiceCallback> p in clients)
            {
                if (userName == p.Key.Item1 && ServerGuid == p.Key.Item2)
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
        public string GetVersion()
        {
            return "1";
        }

        IDuplexServiceCallback GetClient(string name, Guid server)
        {

            foreach (KeyValuePair<Tuple<string, Guid>, IDuplexServiceCallback> client in clients)
            {
                if (client.Key.Item1 == name && client.Key.Item2 == server)                    
                    return client.Value;
            }
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
                foreach (KeyValuePair<Tuple<string, Guid>, IDuplexServiceCallback> client in clients)
                {
                    //Client c = client.Key;
                    try
                    {
                        IDuplexServiceCallback callback = client.Value;
                        if (client.Key.Item1 == fromUserName && client.Key.Item2 == fromServerGuid)
                            broadcast = false;
                        else
                            broadcast = true;

                        callback.ReceiveBroadcast(client.Key.Item1, client.Key.Item2, fromUserName, fromServerGuid, message, broadcast, DateTime.Now);
                       
                    }
                    catch (Exception err)
                    {                         
                        _Disconnect(client.Key.Item1, client.Key.Item2, true);
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
               
                try
                {
                    bool broadcast;
                    foreach (KeyValuePair<Tuple<string, Guid>, IDuplexServiceCallback> client in clients)
                    {
                        if (client.Key.Item1 == fromUserName && client.Key.Item2 == fromServerGuid)
                            broadcast = false;
                        else 
                            broadcast = true;

                        if (client.Key.Item2 == toServerGuid)
                        {
                            
                            var t = new Thread(() =>
                            {
                                client.Value.ReceiveBroadcast(client.Key.Item1, client.Key.Item2, fromUserName, fromServerGuid, message, broadcast, DateTime.Now);
                            });
                            t.Start();
                        }
                    }
                    return true;
                }
                catch (Exception err)
                {                    
                    _Disconnect(fromUserName, fromServerGuid, true);
                    Console.WriteLine(err.Message);
                    return false;
                }
            }
            
        }

        public bool SendMessage(string fromUserName, 
                                Guid fromServerGuid, 
                                string toUserName, 
                                Guid toServerGuid,
                                string message)
        {

            lock (m_lock)
            {               
                try
                {                     
                    IDuplexServiceCallback p = GetClient(toUserName, toServerGuid);
                    if (p != null)
                    {                                                                           
                        IDuplexServiceCallback p1 = GetClient(fromUserName, fromServerGuid);
                        p.NotifyMessage(fromUserName, fromServerGuid, toUserName, toServerGuid, message, DateTime.Now);                         
                        return true;
                    }
                    else
                    {
                        IDuplexServiceCallback p1 = GetClient(fromUserName, fromServerGuid);
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
                    _Disconnect(fromUserName, fromServerGuid, true);
                    Console.WriteLine(err.Message);
                    return false;
                }
            }

        }
    }
}
