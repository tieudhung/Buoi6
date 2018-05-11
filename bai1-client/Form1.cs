using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace bai1_client
{
    public partial class Form1 : Form
    {
        Socket client;
        IPEndPoint ipclient;
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            connet();
        }

        private void butSend_Click(object sender, EventArgs e)
        {
            Send();
            AddMessage(txtMessage.Text);
        }
        //Kết Nối
        void connet()
        {
            ipclient = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 995);
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                client.Connect(ipclient);
                MessageBox.Show("Kết Nối Thành Công", "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            catch
            {
                MessageBox.Show("Lỗi Không Kết Nối Được Server ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Thread listen = new Thread(Receive);
            listen.IsBackground = true;
            listen.Start();
        }

        void Close()
        {
            client.Close();
        }

        //Nhận Tin
        void Receive()
        {
            try
            {
                while (true)
                {
                    byte[] recv = new byte[2048];
                    client.Receive(recv);
                    string message = "--" + (string)GomManh(recv);
                    AddMessage(message);
                }
            }
            catch
            {

                Close();
            }


        }
        //Gửi Tin
        void Send()
        {
            if (client != null && txtMessage.Text != string.Empty)
            {
                client.Send(PhanManh(txtMessage.Text));

            }
        }

        //Add Message
        void AddMessage(string s)
        {
            listBox1.Items.Add(s);
            txtMessage.Clear();
        }
        //Hàm Phân Mảnh
        byte[] PhanManh(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            return stream.ToArray();
        }
        // Gom Mảnh
        object GomManh(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(stream);
        }

    }
}
