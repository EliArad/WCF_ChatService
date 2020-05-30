using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
 


namespace ChatServiceLib
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TestService" in both code and config file together.
    
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ChatService : IChatService 
    {
        
          
        public Dictionary<string, IDuplexServiceCallback> listCallback = new Dictionary<string,IDuplexServiceCallback>();
      
       

         
        public ChatService()
        {
            
        }

        public void Load(string configFileName)
        {
             
        } 
        public void Registration(string baseFieldGuid)
        {
            IDuplexServiceCallback Callback = OperationContext.Current.GetCallbackChannel<IDuplexServiceCallback>();

            Console.WriteLine("Registration: " + baseFieldGuid);

            if (listCallback.ContainsKey(baseFieldGuid) == false)
                listCallback.Add(baseFieldGuid, Callback);
            else
                listCallback[baseFieldGuid] = Callback;
 
        }
       
        
        void BroadcastMessage(int h)
        {
            again:
            int count = listCallback.Count;
            //Console.WriteLine("broadcast to: {0}", count);

            foreach (KeyValuePair<string, IDuplexServiceCallback> entry in listCallback)            
            {
                try
                {
                    try
                    {
                        //entry.Value.NotifyDataCallback(m_fieldGuid, m_ipAddress, chat_WCF_SERVER,(int)code, buf, size, DateTime.Now);
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine(err.Message);
                        listCallback.Remove(entry.Key);
                        goto again;
                    }
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                }
            }
        }
        
     
        void BroadcastMessage()
        {
        again:
            //Console.WriteLine("broadcast to: {0}", listCallback.Count);

 
           
            foreach (KeyValuePair<string, IDuplexServiceCallback> entry in listCallback)  
            {
                try
                {
                    try
                    {
                        //entry.Value.NotifyDataCallback(m_fieldGuid, 
                        //                             m_ipAddress,
                        //                           chat_WCF_SERVER, 
                        //                         (int)PhidgetNotify.PhidgetPortStatus,
                        //                      buffer,
                        //                       buffer.Length, DateTime.Now);
                    }
                    catch (Exception err)
                    {
                        listCallback.Remove(entry.Key);
                        goto again;
                    }
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                }
            }
        }
  
        public string GetVersion()
        {
            return "1";
        }

        void BroadcastMessage(int code, string msg)
        {
            again:
            int count = listCallback.Count;
          
            //Console.WriteLine("broadcast to: {0}", count);
            foreach (KeyValuePair<string, IDuplexServiceCallback> entry in listCallback)  
            {
                try
                {
                    //Console.WriteLine("{0} , {1}", code, msg);
                    try
                    {
                        //entry.Value.NotifyCallbackMessage(m_fieldGuid, m_ipAddress,chat_WCF_SERVER, code, msg, DateTime.Now);
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine(err.Message);
                        listCallback.Remove(entry.Key);
                        goto again;
                    }
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                }
            }
        }
    }
}
