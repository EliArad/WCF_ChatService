using ChatWCFClientApi;
using System;
 
using System.Threading;
  
namespace ChatWCFClientTestApp
{
    
    class Program
    {
        static void MsgCallback(string fieldGuid, string ipAddress , int portNumber ,  int code , string msg, DateTime date)
        {
            
         
        }
        static void DataCallback(string fieldGuid, string ipAddress, int portNumber, int code,byte[] buf, int size, DateTime date)
        {
             

        }
        static void Main(string[] args)
        {
            ChatWCFClient p = null; 
            try
            {
                while (true)
                {
                    try
                    {
                        p = new ChatWCFClient("10.0.0.17" , "1" , "2");
                        break;
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine("Failed to open: " + err.Message);
                        Thread.Sleep(1000); 
                    }
                }
                p.Connect("user1", "freedesc", new Guid(), DateTime.Now);
               
                 
                Console.WriteLine("Opened");
                Console.ReadKey();
            }
            catch (Exception err)
            {
                Console.WriteLine();
                Console.ReadKey();
            }
            p.CloseClient();
            
        }
    }
}
