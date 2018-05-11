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

namespace Bai1_Server
{
    public partial class Form1 : Form
    {
        Socket server;
        IPEndPoint ipserver;
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            connet();
        }

        private void ButSend_Click(object sender, EventArgs e)
        {
            foreach (Socket item in clientList)
            {
                Send(item);
            }
            AddMessage("server :" + txtMessage.Text);
            txtMessage.Clear();
        }
        List<Socket> clientList;

        void connet()
        {
            clientList = new List<Socket>();
            ipserver = new IPEndPoint(IPAddress.Any, 995);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            server.Bind(ipserver);

            Thread listen = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        server.Listen(10);
                        Socket client = server.Accept();
                        clientList.Add(client);

                        Thread recv = new Thread(Receive);
                        recv.IsBackground = true;
                        recv.Start(client);
                    }

                }
                catch
                {
                    ipserver = new IPEndPoint(IPAddress.Any, 995);
                    server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }


            });
            listen.IsBackground = true;
            listen.Start();

        }

        void Close()
        {
            server.Close();
        }

        //Nhận Tin
        void Receive(object obj)
        {
            Socket client = obj as Socket;
            try
            {
                while (true)
                {
                    byte[] recv = new byte[2048];
                    client.Receive(recv);
                    string message = "Client :" + (string)GomManh(recv);

                    foreach (Socket item in clientList)
                    {
                        if (item != null && item != client)
                            item.Send(PhanManh(message));
                    }
                    AddMessage(message);
                }
            }
            catch
            {
                clientList.Remove(client);
                client.Close();
            }


        }
        //Gửi Tin
        void Send(Socket client)
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
