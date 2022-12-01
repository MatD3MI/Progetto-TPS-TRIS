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
using System.Net;
using System.Threading;
using System.Diagnostics;

namespace CLIENT
{
    public partial class CLIENT : Form
    {
        int mov;
        int movX;
        int movY;

        public CLIENT()
        {
            InitializeComponent();
        }

        public void CLIENT_load(object sender, EventArgs e)
        {
            this.Location = Screen.AllScreens[1].WorkingArea.Location;
        }

        Socket SOCKET;
        Button B;
        List<Button> LIST = new List<Button>();
        int count = -1;
        int[] ConteggioMosse = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form1 home = new Form1();
            this.Hide();
            home.ShowDialog();
        }

        private void buttonDesign()
        {

        }


       private void ricevutaMossa() 
       {
      
                byte[] bytes = new byte[1024];
                int bytesRECEIVE = SOCKET.Receive(bytes);

        
            string A = Encoding.ASCII.GetString(bytes, 0, bytesRECEIVE);
            Console.WriteLine(A);
            string mossaServer = A.Split(':')[1];
           

            
            foreach (var b in LIST)
            {
                    if (b.Name == mossaServer)
                    {

                        Image bkg = Image.FromFile(@"..\..\..\..\IMMAGINI\LOGOO.png");

                        b.Image = bkg;
                        ConteggioMosse[Int32.Parse(b.Name)] = 2;

                    }
            }

            
 

       }   

        private bool checkWin(int[] mosse)
        {
            
            if(mosse[0]==1 && mosse[0]== mosse[1] && mosse[0] == mosse[2])
            {
                return true;
            } 
            else if (mosse[3] == 1 && mosse[3] == mosse[4] && mosse[3] == mosse[5])
            {
                return true;
            }
            else if (mosse[6] == 1 && mosse[6] == mosse[7] && mosse[6] == mosse[8])
            {
                return true;
            }
            else if (mosse[0] == 1 && mosse[0] == mosse[3] && mosse[0] == mosse[6])
            {
                return true;
            }
            else if (mosse[1] == 1 && mosse[1] == mosse[4] && mosse[1] == mosse[7])
            {
                return true;
            }
            else if (mosse[2] == 1 && mosse[2] == mosse[5] && mosse[2] == mosse[8])
            {
                return true;
            }
            else if (mosse[0] == 1 && mosse[0] == mosse[4] && mosse[0] == mosse[8])
            {
                return true;
            }
            else if (mosse[2] == 1 && mosse[2] == mosse[4] && mosse[2] == mosse[6])
            {
                return true;
            }
            return false;
        }
        private bool checkLose(int[] mosse)
        {
            
            
            if (mosse[0] == 2 && mosse[0] == mosse[1] && mosse[0] == mosse[2])
            {
                return true;
            }
            else if (mosse[3] == 2 && mosse[3] == mosse[4] && mosse[3] == mosse[5])
            {
                return true;
            }
            else if (mosse[6] == 2 && mosse[6] == mosse[7] && mosse[6] == mosse[8])
            {
                return true;
            }
            else if (mosse[0] == 2 && mosse[0] == mosse[3] && mosse[0] == mosse[6])
            {
                return true;
            }
            else if (mosse[1] == 2 && mosse[1] == mosse[4] && mosse[1] == mosse[7])
            {
                return true;
            }
            else if (mosse[2] == 2 && mosse[2] == mosse[5] && mosse[2] == mosse[8])
            {
                return true;
            }
            else if (mosse[0] == 2 && mosse[0] == mosse[4] && mosse[0] == mosse[8])
            {
                return true;
            }
            else if (mosse[2] == 2 && mosse[2] == mosse[4] && mosse[2] == mosse[6])
            {
                return true;
            }
            return false;
        }
        private void inviaMossa(string mossa)
        {
            byte[] bytes = new byte[1024];
            string inviare = "#200:" + mossa + ":<EOF>";

            byte[] msg = Encoding.ASCII.GetBytes(inviare); 

            
            int bytesSENT = SOCKET.Send(msg);
            if (checkWin(ConteggioMosse))
            {
               MessageBox.Show("HAI VINTO LA PARTITA");
                Form1 home = new Form1();
                this.Hide();
                home.ShowDialog();
   

            } 
            else
            {
                ricevutaMossa();
            }

        }
       
        public void pulsantePremuto(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (ConteggioMosse[Int32.Parse(b.Name)] ==0)
            {
                Image bkg = Image.FromFile(@"..\..\..\..\IMMAGINI\LOGO1.png");


                
                b.Image = bkg;
                Debug.WriteLine("#200:"+ b.Name);
                ConteggioMosse[Int32.Parse(b.Name)] = 1;
                inviaMossa(b.Name);
                if (checkLose(ConteggioMosse))
                {
                    MessageBox.Show("HAI PERSO LA PARTITA");
                    Form1 home = new Form1();
                    this.Hide();
                    home.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("mossa non validda");
            }

        }
        public void button1_Click(object sender, EventArgs e)
        {


           

            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPAddress ipAddress = System.Net.IPAddress.Parse("127.0.0.1");
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 5000);

                // Create a TCP/IP  socket.  
                 SOCKET = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    SOCKET.Connect(remoteEP);

                    Console.WriteLine("#100 CONESSIONE COL SERVER  {0} STABILITA",
                        SOCKET.RemoteEndPoint.ToString());

                   

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception a)
                {
                    Console.WriteLine("Unexpected exception : {0}", a.ToString());
                }

            }
            catch (Exception a)
            {
                Console.WriteLine(a.ToString());
            }




            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            int count = 0;
            int X = 0;
            int y = 0;
          
            Image bkg = Image.FromFile(@"..\..\..\..\IMMAGINI\LOGOO.png");



            for (int i = 0; i < 3; i++)
            {
                Button btnUserInput = new Button();
                
                //btnUserInput.Image = bkg;
                btnUserInput.Size = new Size(70, 70);
                btnUserInput.Name = count.ToString();
                //btnUserInput.Text = count.ToString();
                btnUserInput.FlatStyle = FlatStyle.Flat;
                btnUserInput.FlatAppearance.BorderColor = Color.Black;
                btnUserInput.Location = new Point(75 * (X + 1), 110);
                X++;
                count++;
                Controls.Add(btnUserInput);
                btnUserInput.Click += new EventHandler(pulsantePremuto);
                LIST.Add(btnUserInput);

            }
            X = 0;
            for (int i = 3; i < 6; i++)
            {
                Button btnUserInput = new Button();
                ;
                //btnUserInput.Image = bkg;
                btnUserInput.Size = new Size(70, 70);
                btnUserInput.Name = count.ToString();
                // btnUserInput.Text = count.ToString();
                btnUserInput.FlatStyle = FlatStyle.Flat;
                btnUserInput.FlatAppearance.BorderColor = Color.Black;
                btnUserInput.Location = new Point(75 * (X + 1), 185);
                X++;
                count++;
                Controls.Add(btnUserInput);
                btnUserInput.Click += new EventHandler(pulsantePremuto);
                LIST.Add(btnUserInput);

            }
            X = 0;
            for (int i = 5; i < 8; i++)
            {
                Button btnUserInput = new Button();
                btnUserInput.BringToFront();
                //btnUserInput.Image = bkg;
                btnUserInput.Size = new Size(70, 70);
                btnUserInput.Name = count.ToString();
                // btnUserInput.Text = count.ToString();
                btnUserInput.FlatStyle = FlatStyle.Flat;
                btnUserInput.FlatAppearance.BorderColor = Color.Black;
                btnUserInput.Location = new Point(75 * (X + 1), 260);
                X++;
                count++;
                Controls.Add(btnUserInput);

                btnUserInput.Click += new EventHandler(pulsantePremuto);
                LIST.Add(btnUserInput);
            }

            

        }
        


    private void CLIENT_Load(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            mov = 1;
            movX = e.X;
            movY = e.Y;
        }

        private void flowLayoutPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            if(mov == 1)
            {
                this.SetDesktopLocation(MousePosition.X - movX, MousePosition.Y - movY);
            }
        }

        private void flowLayoutPanel1_MouseUp(object sender, MouseEventArgs e)
        {
            mov= 0;
        }
    } }