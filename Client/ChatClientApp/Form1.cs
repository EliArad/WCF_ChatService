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
        Guid m_serverGuid = new Guid();
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            m_client = new ChatWCFClient("10.0.0.17", this);
        }
        
        private void btnConnect_Click(object sender, EventArgs e)
        {

            try
            {                
                m_client.Connect(txtUserName.Text, "freedesc" , m_serverGuid, DateTime.Now, out string outMessage);
            }
            catch (Exception err)
            {
                Console.WriteLine("Failed to open: " + err.Message);                
            }
        }

        public void UserJoin(Client client)
        {
            txtStatus.AppendText("User: " + client.Name + " has joined" + Environment.NewLine);
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

        private void btnJoin_Click(object sender, EventArgs e)
        {
            m_client.Leave(txtUserName.Text , m_serverGuid, out string outMessage);
        }

        public void UserLeave(string userName, Guid serverGuid, DateTime time)
        {
            txtStatus.AppendText("User: " + userName + " has left: " + time.ToString() + Environment.NewLine);
        }
    }
}
