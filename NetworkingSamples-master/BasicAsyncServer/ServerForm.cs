////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	serverform.cs
//
// summary:	Implements the serverform class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace BasicAsyncServer
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Form for viewing the server. </summary>
    ///
    /// <remarks>   David Hunt, 8/19/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public partial class ServerForm : Form
    {
        /// <summary>   The server socket. </summary>
        private Socket serverSocket;
        /// <summary>   We will only accept one socket. </summary>
        private Socket clientSocket;
        /// <summary>   The buffer. </summary>
        private byte[] buffer;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   David Hunt, 8/19/2018. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public ServerForm()
        {
            InitializeComponent();
            StartServer();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Shows the error dialog. </summary>
        ///
        /// <remarks>   David Hunt, 8/19/2018. </remarks>
        ///
        /// <param name="message">  The message. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct server socket and bind socket to all local network interfaces, then listen for
        /// connections with a backlog of 10. Which means there can only be 10 pending connections lined
        /// up in the TCP stack at a time. This does not mean the server can handle only 10 connections.
        /// The we begin accepting connections. Meaning if there are connections queued, then we should
        /// process them.
        /// </summary>
        ///
        /// <remarks>   David Hunt, 8/19/2018. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void StartServer()
        {
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, 3333)); // bind on port  3333
                serverSocket.Listen(10); // listening on a backlog of ten pending connections
                serverSocket.BeginAccept(AcceptCallback, null); // start accepting incoming 
            }
            catch (SocketException ex)
            {
                ShowErrorDialog(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog(ex.Message);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Async callback, called on completion of accept callback. </summary>
        ///
        /// <remarks>   David Hunt, 8/19/2018. </remarks>
        ///
        /// <param name="AR">   The result of the asynchronous operation. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void AcceptCallback(IAsyncResult AR)
        {
            try
            {
                clientSocket = serverSocket.EndAccept(AR); // set up the clientsocket
                buffer = new byte[clientSocket.ReceiveBufferSize]; // intialise the buffer to proper buffer size

                // Send a message to the newly connected client.
                var sendData = Encoding.ASCII.GetBytes("Hello");
                clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, SendCallback, null);
                // Listen for client data.
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
                // Continue listening for clients.
                serverSocket.BeginAccept(AcceptCallback, null); 
            }
            catch (SocketException ex)
            {
                ShowErrorDialog(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog(ex.Message);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Async callback, called on completion of send callback. </summary>
        ///
        /// <remarks>   David Hunt, 8/19/2018. </remarks>
        ///
        /// <param name="AR">   The result of the asynchronous operation. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void SendCallback(IAsyncResult AR)
        {
            try
            {
                clientSocket.EndSend(AR);
            }
            catch (SocketException ex)
            {
                ShowErrorDialog(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog(ex.Message);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Async callback, called on completion of receive callback. </summary>
        ///
        /// <remarks>   David Hunt, 8/19/2018. </remarks>
        ///
        /// <param name="AR">   The result of the asynchronous operation. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void ReceiveCallback(IAsyncResult AR)
        {
            try
            {
                // Socket exception will raise here when client closes, as this sample does not
                // demonstrate graceful disconnects for the sake of simplicity.
                int received = clientSocket.EndReceive(AR);

                if (received == 0)
                {
                    return;
                }

                // The received data is deserialized in the PersonPackage ctor.
                PersonPackage person = new PersonPackage(buffer);
                SubmitPersonToDataGrid(person);

                // Start receiving data again.
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }
            // Avoid catching all exceptions handling in cases like these. 
            catch (SocketException ex)
            {
                ShowErrorDialog(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog(ex.Message);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Provides a thread safe way to add a row to the data grid. </summary>
        ///
        /// <remarks>   David Hunt, 8/19/2018. </remarks>
        ///
        /// <param name="person">   The person. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void SubmitPersonToDataGrid(PersonPackage person)
        {
            Invoke((Action)delegate
            {
                dataGridView.Rows.Add(person.Name, person.Age, person.IsMale);
            });
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called by dataGridView for cell content click events. </summary>
        ///
        /// <remarks>   David Hunt, 8/19/2018. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Data grid view cell event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ServerForm_Load(object sender, EventArgs e)
        {

        }
    }
}
