using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace client
{
    class Program
    {
        static void Main(string[] args)
        {
            int i, j, k;
            string r1, c1, r2, c2;

            int[,] arr1 = new int[50, 50];
            int[,] arr2 = new int[50, 50];
            int[,] ma_tran_tich = new int[50, 50];

            Console.Write("\nNhap so hang va so cot cua ma tran thu nhat:\n");
            Console.Write("Nhap so hang: ");
            r1 = Console.ReadLine();
            Console.Write("Nhap so cot: ");
            c1 = Console.ReadLine();

            Console.Write("\nNhap so hang va so cot cua ma tran thu hai:\n");
            Console.Write("Nhap so hang: ");
            r2 = Console.ReadLine();
            Console.Write("Nhap so cot: ");
            c2 = Console.ReadLine();

            if (c1 != r2)
            {
                Console.Write("Khong the nhan hai ma tran tren !!!");
                Console.Write("\nSo cot cua ma tran thu nhat phai bang so hang cua ma tran thu hai.");
            }
            else
            {
                Console.Write("Nhap cac phan tu cua ma tran thu nhat:\n");
                for (i = 0; i < int.Parse(r1); i++)
                {
                    for (j = 0; j < int.Parse(c1); j++)
                    {
                        Console.Write("Phan tu - [{0}],[{1}]: ", i, j);
                        arr1[i, j] = Convert.ToInt32(Console.ReadLine());
                    }
                }
                Console.Write("Nhap cac phan tu cua ma tran thu hai:\n");
                for (i = 0; i < int.Parse(r2); i++)
                {
                    for (j = 0; j < int.Parse(c2); j++)
                    {
                        Console.Write("Phan tu - [{0}],[{1}]: ", i, j);
                        arr2[i, j] = Convert.ToInt32(Console.ReadLine());
                    }
                }
                Console.Write("\nIn ma tran dau tien:\n");
                for (i = 0; i < int.Parse(r1); i++)
                {
                    Console.Write("\n");
                    for (j = 0; j < int.Parse(c1); j++)
                        Console.Write("{0}\t", arr1[i, j]);
                }

                Console.Write("\nIn ma tran thu hai:\n");
                for (i = 0; i < int.Parse(r2); i++)
                {
                    Console.Write("\n");
                    for (j = 0; j < int.Parse(c2); j++)
                        Console.Write("{0}\t", arr2[i, j]);
                }
            }

                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                ProtocolType.Tcp);
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);
            byte[] data = new byte[1024];
            string stringData;
            int recv;
            sock.Connect(iep);
            Console.WriteLine("Connected to server");
            recv = sock.Receive(data);
            stringData = Encoding.ASCII.GetString(data, 0, recv);
            Console.WriteLine("Received: {0}", stringData);
            while (true)
            {
                
                if (stringData == "exit")
                    break;
                data = Encoding.ASCII.GetBytes(r1);
                sock.Send(data, data.Length, SocketFlags.None);
                byte[] data1 = Encoding.ASCII.GetBytes(r2);
                sock.Send(data1, data1.Length, SocketFlags.None);
                byte[] data2 = Encoding.ASCII.GetBytes(r2);
                sock.Send(data2, data2.Length, SocketFlags.None);
                byte[] data3 = Encoding.ASCII.GetBytes(r2);
                sock.Send(data3, data3.Length, SocketFlags.None);
                data = new byte[1024];
                recv = sock.Receive(data);
                stringData = Encoding.ASCII.GetString(data, 0, recv);
                Console.WriteLine("Received: {0}", stringData);
            }
            sock.Close();
        }
    }
}
