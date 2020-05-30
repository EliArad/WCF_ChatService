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

        ChatWCFClient[] m_client = new ChatWCFClient[3];
        Guid m_serverGuid = new Guid();
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            for (int i = 0; i < 3; i++)
            {
                m_client[i] = new ChatWCFClient("10.0.0.17", this);
            }
        }
        
        private void btnConnect_Click(object sender, EventArgs e)
        {

            try
            {                
                m_client[0].Connect(txtUserName.Text, "freedesc" , m_serverGuid, DateTime.Now, out string outMessage);
            }
            catch (Exception err)
            {
                Console.WriteLine("Failed to connect: " + err.Message);                
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
            m_client[0].Leave(out string outMessage);
        }

        public void UserLeave(string userName, Guid serverGuid, DateTime time)
        {
            txtStatus.AppendText("User: " + userName + " has left: " + time.ToString() + Environment.NewLine);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                m_client[1].Connect(textBox1.Text, "freedesc", m_serverGuid, DateTime.Now, out string outMessage);
            }
            catch (Exception err)
            {
                Console.WriteLine("Failed to connect: " + err.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                m_client[2].Connect(textBox2.Text, "freedesc", m_serverGuid, DateTime.Now, out string outMessage);
            }
            catch (Exception err)
            {
                Console.WriteLine("Failed to connect: " + err.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_client[1].Leave(out string outMessage);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            m_client[2].Leave(out string outMessage);
        }
    }
}
