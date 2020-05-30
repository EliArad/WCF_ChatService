using ChatWCFClientApi;
using ChatWCFClientApi.ServiceReference1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClientApp
{
    public partial class Form1 : Form,  IChatServiceCallback
    {

        ChatWCFClient[] m_client = new ChatWCFClient[4];

        Guid[] m_serverGuid = new Guid[4];

        public Form1()
        {
            InitializeComponent();
             
            for (int i = 0; i < m_client.Length; i++)
            {
                m_client[i] = new ChatWCFClient("10.0.0.17", this);
                m_serverGuid[i] = Guid.NewGuid();
            }
        }
        
        private async void btnConnect_Click(object sender, EventArgs e)
        {

            await Task.Run(() =>
            {
                try
                {
                    m_client[0].Connect(txtUserName.Text, "freedesc", m_serverGuid[0], DateTime.Now, out string outMessage);
                }
                catch (Exception err)
                {
                    Console.WriteLine("Failed to connect: " + err.Message);
                }
            });

        }

        public void UserJoin(string userName , Guid serverGuid, bool newUser)
        {
            if (newUser == true)
                txtStatus.AppendText("User: " + userName + " has joined" + Environment.NewLine);
            else
            {
                txtStatus.AppendText("User: " + userName + " already joined" + Environment.NewLine);

            }
        }
 
        public void ClientsList(Tuple<string, System.Guid>[] clients)
        {

        }

        int m_bn = 0;
        public void ReceiveBroadcast(string toUserName,  Guid toServerGuid, string fromUserName, Guid fromServerGuid, string message, bool broadcast, DateTime date)
        {
            this.BeginInvoke((Action)(() =>
            {
                if (broadcast == true)
                    txtStatus.AppendText(m_bn.ToString() + "  User: " + fromUserName + " broadcast to you: " + toUserName + " : " + message + Environment.NewLine);
                m_bn++;
            }));
        }

        private async void btnJoin_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                m_client[0].Leave(out string outMessage);
            });
        }

        public void UserLeave(string userName, Guid serverGuid, DateTime time)
        {
            txtStatus.AppendText("User: " + userName + " has left: " + time.ToString() + Environment.NewLine);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = textBox1.Text;
                await Task.Run(() =>
                {
                    m_client[1].Connect(userName, "freedesc", m_serverGuid[1], DateTime.Now, out string outMessage);
                });
            }
            catch (Exception err)
            {
                Console.WriteLine("Failed to connect: " + err.Message);
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = textBox2.Text;
                await Task.Run(() =>
                {
                    m_client[2].Connect(userName, "freedesc", m_serverGuid[2], DateTime.Now, out string outMessage);
                });
            }
            catch (Exception err)
            {
                Console.WriteLine("Failed to connect: " + err.Message);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                m_client[1].Leave(out string outMessage);
            });
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                m_client[2].Leave(out string outMessage);
            });
        }

        int m_nm = 0;
        public void NotifyMessage(string fromUserName, Guid fromServerName, string toUserName, Guid toServerName, string message, DateTime date)
        {
            this.BeginInvoke((Action)(() =>
            {
                txtStatus.AppendText(m_nm.ToString() + "  Message arrived from: " + fromUserName + " to: " + toUserName + " :" + message + Environment.NewLine);
                m_nm++;
            }));
            
            //Console.WriteLine("Message arrived from: " + fromUserName + " to: " + toUserName + " :" + message + Environment.NewLine);
        }

        public void NotifyMessageSent(DateTime date, bool sent)
        {

        }

        private async void btnSendMessage_Click(object sender, EventArgs e)
        {
            int x1 = cmbFromUserName.SelectedIndex;
            int x2 = cmbToUserName.SelectedIndex;
            if (x1 == -1 || x2 == -1)
                return;

            string str = txtMessageToSend.Text;
            await Task.Run(() =>
            {

                bool b = m_client[x1].SendMessage(m_client[x2].UserName,
                                                    m_client[x2].ServerGuid,
                                                    str,
                                                    out string outMessage);
                if (b == false)
                {

                }                
            });
            timer2.Enabled = true;
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            try
            {
                await Task.Run(() =>
                {
                    m_client[3].Connect(textBox3.Text, "freedesc", m_serverGuid[3], DateTime.Now, out string outMessage);
                });
            }
            catch (Exception err)
            {
                Console.WriteLine("Failed to connect: " + err.Message);
            }
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                m_client[3].Leave(out string outMessage);
            });
        }

        private async void button7_Click(object sender, EventArgs e)
        {
            string msg = textBox4.Text;
            await Task.Run(() =>
            {
                m_client[3].Broadcast(msg, out string outMessage);
               
            });
            timer1.Enabled = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (ChatWCFClient c in m_client)
            {
                c.Close(out string outMessage);
            }
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            string msg = textBox4.Text;
            await Task.Run(() =>
            {
                m_client[3].Broadcast(msg, out string outMessage);
            });
        }

        private async void timer2_Tick(object sender, EventArgs e)
        {
            int x = cmbFromUserName.SelectedIndex;
            int x1 = cmbToUserName.SelectedIndex;
            string msg = txtMessageToSend.Text;
            await Task.Run(() =>
            {
                bool b = m_client[x].SendMessage(m_client[x1].UserName,
                                                m_client[x1].ServerGuid,
                                                msg,
                                                out string outMessage);
            });
        }
    }
}
