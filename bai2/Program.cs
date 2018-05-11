using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace bai2
{
    class Program
    {
        static void Main(string[] args)
        {
            static Socket listen;
            static Socket client;
            static IPEndPoint ipe;
            static int connections = 0;

            listen = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                    ProtocolType.Tcp);
            ipe = new IPEndPoint(IPAddress.Any, 9050);
            listen.Bind(ipe);
            listen.Listen(10);
            Console.WriteLine("Waiting for clients...");
            while (true)
            {
                if (listen.Poll(1000000, SelectMode.SelectRead))
                {
                    client = listen.Accept();
                    Thread newthread = new Thread(new ThreadStart(HandleConnection));
                    newthread.Start();
                }
            }
            while (true)
            {
                int i, j, k, r1, c1, r2, c2, sum = 0;

                int[,] arr1 = new int[50, 50];
                int[,] arr2 = new int[50, 50];
                int[,] ma_tran_tich = new int[50, 50];

                Console.Write("\nNhan hai ma tran trong C#:\n");
                Console.Write("----------------------------------\n");

                Console.Write("\nNhap so hang va so cot cua ma tran thu nhat:\n");
                Console.Write("Nhap so hang: ");
                r1 = Convert.ToInt32(Console.ReadLine());
                Console.Write("Nhap so cot: ");
                c1 = Convert.ToInt32(Console.ReadLine());

                Console.Write("\nNhap so hang va so cot cua ma tran thu hai:\n");
                Console.Write("Nhap so hang: ");
                r2 = Convert.ToInt32(Console.ReadLine());
                Console.Write("Nhap so cot: ");
                c2 = Convert.ToInt32(Console.ReadLine());

                if (c1 != r2)
                {
                    Console.Write("Khong the nhan hai ma tran tren !!!");
                    Console.Write("\nSo cot cua ma tran thu nhat phai bang so hang cua ma tran thu hai.");
                }
                else
                {
                    Console.Write("Nhap cac phan tu cua ma tran thu nhat:\n");
                    for (i = 0; i < r1; i++)
                    {
                        for (j = 0; j < c1; j++)
                        {
                            Console.Write("Phan tu - [{0}],[{1}]: ", i, j);
                            arr1[i, j] = Convert.ToInt32(Console.ReadLine());
                        }
                    }
                    Console.Write("Nhap cac phan tu cua ma tran thu hai:\n");
                    for (i = 0; i < r2; i++)
                    {
                        for (j = 0; j < c2; j++)
                        {
                            Console.Write("Phan tu - [{0}],[{1}]: ", i, j);
                            arr2[i, j] = Convert.ToInt32(Console.ReadLine());
                        }
                    }
                    Console.Write("\nIn ma tran dau tien:\n");
                    for (i = 0; i < r1; i++)
                    {
                        Console.Write("\n");
                        for (j = 0; j < c1; j++)
                            Console.Write("{0}\t", arr1[i, j]);
                    }

                    Console.Write("\nIn ma tran thu hai:\n");
                    for (i = 0; i < r2; i++)
                    {
                        Console.Write("\n");
                        for (j = 0; j < c2; j++)
                            Console.Write("{0}\t", arr2[i, j]);
                    }
                    //nhan hai ma tran
                    for (i = 0; i < r1; i++)
                        for (j = 0; j < c2; j++)
                            ma_tran_tich[i, j] = 0;
                    for (i = 0; i < r1; i++)    //hang cua ma tran thu nhat 
                    {
                        for (j = 0; j < c2; j++)    //cot cua ma tran thu hai 
                        {
                            sum = 0;
                            for (k = 0; k < c1; k++)
                                sum = sum + arr1[i, k] * arr2[k, j];
                            ma_tran_tich[i, j] = sum;
                        }
                    }
                    Console.Write("\nMa tran tich cua hai ma tran tren la: \n");
                    for (i = 0; i < r1; i++)
                    {
                        Console.Write("\n");
                        for (j = 0; j < c2; j++)
                        {
                            Console.Write("{0}\t", ma_tran_tich[i, j]);
                        }
                    }
                }
                Console.Write("\n\n");
            }
            
            
            


            Console.ReadKey();
        }
        static void HandleConnection()
        {



            int recv;
            byte[] data = new byte[1024];
            NetworkStream ns = new NetworkStream(client);
            connections++;
            Console.WriteLine("New client accepted: {0} active connections",
            connections);
            string welcome = "Welcome to my test server";
            data = Encoding.ASCII.GetBytes(welcome);
            ns.Write(data, 0, data.Length);
            while (true)
            {
                data = new byte[1024];
                recv = ns.Read(data, 0, data.Length);
                if (recv == 0)
                    break;
                ns.Write(data, 0, recv);
            }
            ns.Close();
            client.Close();
            connections--;
            Console.WriteLine("Client disconnected: {0} active connections",
            connections);
        }
    }
}
