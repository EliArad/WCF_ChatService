using ChatWCFClientApi;
using ChatWCFClientApi.ServiceReference1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClientApp
{
    public partial class Form1 : Form,  IChatServiceCallback
    {

        ChatWCFClient m_client = null;
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        void MsgCallback(string fieldGuid, string ipAddress, int portNumber, int code, string msg, DateTime date)
        {


        }
        void DataCallback(string fieldGuid, string ipAddress, int portNumber, int code, byte[] buf, int size, DateTime date)
        {


        }
        private void btnConnect_Click(object sender, EventArgs e)
        {

            try
            {
                m_client = new ChatWCFClient("10.0.0.17", "1", "2", this);                 
                m_client.Connect(txtUserName.Text, "freedesc" ,new Guid(), DateTime.Now);
            }
            catch (Exception err)
            {
                Console.WriteLine("Failed to open: " + err.Message);                
            }
        }

        public void UserJoin(Client client)
        {
            txtStatus.AppendText("User: " + client.Name + " has joined");
        }

        public void UserLeave(Client client)
        {
           
        }

        public void RefreshClients(Client[] clients)
        {

        }

        public void ReceiveBroadcast(ChatWCFClientApi.ServiceReference1.Message msg)
        {

        }

        public void Receive(ChatWCFClientApi.ServiceReference1.Message msg)
        {

        }
    }
}
