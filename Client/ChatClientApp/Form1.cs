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

        public void UserJoin(Client client, bool newUser)
        {
            if (newUser == true)
                txtStatus.AppendText("User: " + client.Name + " has joined" + Environment.NewLine);
            else
            {
                txtStatus.AppendText("User: " + client.Name + " already joined" + Environment.NewLine);

            }
        }
 
        public void RefreshClients(Client[] clients)
        {

        }

        public void ReceiveBroadcast(ChatWCFClientApi.ServiceReference1.Message msg)
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

        public void NotifyMessage(string fromUserName, Guid fromServerName, string toUserName, Guid toServerName, string message, DateTime date)
        {
            txtStatus.AppendText("Message arrived from: " + fromUserName + " to: " + toUserName + " :" + message + Environment.NewLine);
        }

        public void NotifyMessageSent(DateTime date, bool sent)
        {

        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            if (cmbFromUserName.SelectedIndex == -1)
                return;
            if (cmbToUserName.SelectedIndex == -1)
                return;


            bool b = m_client[cmbFromUserName.SelectedIndex].SendMessage(m_client[cmbToUserName.SelectedIndex].UserName,                                        
                                                                         m_client[cmbToUserName.SelectedIndex].ServerGuid,
                                                                         txtMessageToSend.Text,
                                                                         out string outMessage);
            if (b == false)
            {

            }
        }
    }
}
