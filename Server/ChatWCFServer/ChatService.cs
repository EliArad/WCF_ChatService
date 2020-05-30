using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
 


namespace ChatServiceLib
{
    
    
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
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
            IDuplexServiceCallback Callback = OperationContext.Current.GetCallbackChannel<IDuplexServiceCallback>();

            Console.WriteLine("Registration: " + userName);

            Client client = new Client
            {
                FreeDesc = freedesc,
                Name = userName,
                ServerGuid = serverGuid,
                Time = time 
            };

            /*
            if (listCallback.ContainsKey(baseFieldGuid) == false)
                listCallback.Add(baseFieldGuid, Callback);
            else
                listCallback[baseFieldGuid] = Callback;
            */

            if (!clients.ContainsValue(Callback))
             {
                if (clients.ContainsKey(client) == false)
                {
                    List<Client> toRemove = new List<Client>();
                    foreach (KeyValuePair<Client, IDuplexServiceCallback> p in clients)
                    {
                        if (p.Key.Name == client.Name && p.Key.ServerGuid == client.ServerGuid)
                        {
                            toRemove.Add(p.Key);
                        }
                    }
                    foreach (Client p in toRemove)
                    {
                        clients.Remove(p);
                        //clientList.Remove(p);
                    }
                    clients.Add(client, Callback);
                    //clientList.Add(client);
                    Console.WriteLine("Number of connected clients: " + clients.Count);
                }
                else
                {
                    clients[client] = Callback;
                }

                foreach (Client key in clients.Keys)
                {
                    IDuplexServiceCallback callback = clients[key];
                    try
                    {
                        //callback.RefreshClients(clientList);
                        //callback.UserJoin(client);
                    }
                    catch
                    {
                        clients.Remove(key);
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
 
        }
        public void Disconnect(Client client)
        {
            //lock (m_connectLock)
            {
                foreach (Client c in clients.Keys)
                {
                    if (client.Name == c.Name && client.ServerGuid == c.ServerGuid)
                    {

                        this.clients.Remove(c);
                        //this.clientList.Remove(c);
                        foreach (IDuplexServiceCallback callback in clients.Values)
                        {
                            //callback.RefreshClients(this.clientList);
                            //callback.UserLeave(client);
                        }
                    }
                }
            }
        }
        public string GetVersion()
        {
            return "1";
        }
         
    }
}
