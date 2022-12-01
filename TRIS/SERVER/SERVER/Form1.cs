using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace SERVER
{
    public partial class Form1 : Form
    {

        int mov;
        int movX;
        int movY;
        public Form1()
        {
            InitializeComponent();
        }
        
        public static string Mossa = null;
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Location = Screen.AllScreens[1].WorkingArea.Location;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.  
            // Dns.GetHostName returns the name of the   
            // host running the application.  
            IPAddress ipAddress = System.Net.IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 5000);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and   
            // listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Start listening for connections.  
                while (true)
                {
                    Console.WriteLine("#50 in attesa di una connessione...");
                   
                    // Program is suspended while waiting for an incoming connection.  
                    Socket handler = listener.Accept();
                    Console.WriteLine("#100 connessione stabilita");


                    ClientManager clientThread = new ClientManager(handler);
                    Thread t = new Thread(new ThreadStart(clientThread.doClient));
                    t.Start();

                }

            }
            catch (Exception a)
            {
                Console.WriteLine(a.ToString());
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mov = 1;
            movX = e.X;
            movY = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mov == 1)
            {
                this.SetDesktopLocation(MousePosition.X - movX, MousePosition.Y - movY);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mov = 0;
        }
    }

    public class ClientManager
    {

        Socket handler;
        byte[] bytes = new Byte[1024];
        String Mossa = "";
        int estratto = 0;
        bool[] MossaFinale;
        Random rand;

        public ClientManager(Socket clientSocket)
        {
            this.handler = clientSocket;
            MossaFinale = new bool[9];
            for(int i=0; i<MossaFinale.Length; i++)
            {
                MossaFinale[i] = false;
            }
            rand = new Random();
        }

        public void doClient()
        {

            while (Mossa != "Quit")
            {
                // An incoming connection needs to be processed.  
                string Mossa = null;
                while (true)
                {
                    
                    int bytesRec = handler.Receive(bytes);
                    Mossa += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    if (Mossa.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }

                }
               // Debug.WriteLine(Mossa);

                string NumCasella = Mossa.Split(':')[1];
                Debug.WriteLine("#200:" + NumCasella);

                int NumCasella2 = Int32.Parse(NumCasella);
              //  Debug.WriteLine(NumCasella2);


                MossaFinale[NumCasella2] =true;

                estratto = rand.Next(0, 8);

                while (MossaFinale[estratto]==true)
                {
                        estratto = rand.Next(0, 8);

                    
                }
               

                MossaFinale[estratto] = true;

                Debug.WriteLine("#201:" + estratto.ToString());
                byte[] msg = Encoding.ASCII.GetBytes("#201:" + estratto.ToString());

               
                handler.Send(msg);
            }
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
            Mossa = "";

        }
    }
}
