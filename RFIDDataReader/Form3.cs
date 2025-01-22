using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RFIDDataReader
{
    public partial class Form3 : Form
    {
        private const string ipAddress = "169.254.10.12";
        private const int port = 10000;
        private CancellationTokenSource cancellationTokenSource;
        private Thread readThread; // Thread for continuous reading
        private int channel;
        private Dictionary<string, HashSet<string>> idChannelMap = new Dictionary<string, HashSet<string>>();
        private const string connectionString = "Data Source=DELL_5401\\SAGE;Initial Catalog=master;Integrated Security=True";

        public Form3()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }


        private void ClearBT_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count > 0)
            {
                listView1.Items.Clear();
            }
            idChannelMap.Clear();
        }

        private async Task SetTypeCommandAsync(NetworkStream stream, byte[] setTypeCommand)
        {
            await stream.WriteAsync(setTypeCommand, 0, setTypeCommand.Length);

            byte[] setTypeResponse = new byte[1024];
            int bytesRead = await stream.ReadAsync(setTypeResponse, 0, setTypeResponse.Length);

            if (setTypeResponse[4] == 0xFF)
            {
                Console.WriteLine("Type de support de données paramétré avec succès.");
            }
            else
            {
                Console.WriteLine($"Erreur lors du paramétrage du type de support de données: {setTypeResponse[4]}");
                return;
            }
        }

        private async Task ReadDataAsync(NetworkStream stream, int channel, bool singleRead)
        {
            byte[] readDataCommand;
            if (channel == 1 && singleRead)
            {
                readDataCommand = new byte[] { 0x00, 0x06, 0x10, 0x92, 0x00, 0x00 };
            }
            else if (channel == 2 && singleRead)
            {
                readDataCommand = new byte[] { 0x00, 0x06, 0x10, 0x94, 0x00, 0x00 };
            }
            else if (channel == 1 && !singleRead)
            {
                readDataCommand = new byte[] { 0x00, 0x06, 0x19, 0x92, 0x00, 0x00 };
            }
            else
            {
                readDataCommand = new byte[] { 0x00, 0x06, 0x19, 0x94, 0x00, 0x00 };
            }

            await stream.WriteAsync(readDataCommand, 0, readDataCommand.Length);

            byte[] readDataResponse = new byte[1024];
            int bytesRead = await stream.ReadAsync(readDataResponse, 0, readDataResponse.Length);

            if (readDataResponse[4] != 0xFF && singleRead)
            {
                MessageBox.Show($"Erreur lors de la confirmation de lecture : {readDataResponse[4]}");
                return;
            }

            byte[] finalResponse = await ReceiveResponseAsync(stream);

            if (finalResponse[4] == 0x00)
            {
                string idInHex = BitConverter.ToString(finalResponse, 6, finalResponse.Length - 44);
                string idInAscii = ConvertToAscii(finalResponse, 6, finalResponse.Length - 44);
                string dataInHex = BitConverter.ToString(finalResponse, 24, finalResponse.Length - 24);
                string data = ConvertToAscii(finalResponse, 24, finalResponse.Length - 24);

                List<String> datas = SeparateWords(data);

                DateTime detectionTime = DateTime.Now;
                string formattedTime = detectionTime.ToString("yyyy-MM-dd HH:mm:ss.fff");

                // Ajouter les données dans la ListView sur le thread principal
                Invoke((MethodInvoker)delegate
                {
                    if (finalResponse[3] == 0xD2)
                    {
                        AddToListView(idInHex, datas[0], datas[1], datas[2], datas[3], formattedTime, "1");
                    }
                    else
                    {
                        AddToListView(idInHex, datas[0], datas[1], datas[2], datas[3], formattedTime, "2");
                    }

                });
            }

        }

        private List<String> SeparateWords(string data)
        {
            List<string> result = new List<string>();

            // Split de la chaîne d'entrée en utilisant ';' comme séparateur
            string[] words = data.Split(';');

            // Boucle sur chaque mot obtenu après le split et ajout à la liste
            foreach (string word in words)
            {
                // Trim pour enlever les espaces en début et fin de chaîne
                result.Add(word.Trim());
            }

            return result;
        }

        private string ConvertToAscii(byte[] data, int startIndex, int length)
        {
            StringBuilder ascii = new StringBuilder(length);
            for (int i = startIndex; i < startIndex + length; i++)
            {
                if (data[i] >= 32 && data[i] <= 126) // ASCII printable characters range
                {
                    ascii.Append((char)data[i]);
                }
                else
                {
                    ascii.Append('?'); // Use '?' for non-ASCII characters
                }
            }
            return ascii.ToString();
        }

        private async Task<byte[]> ReceiveResponseAsync(NetworkStream stream)
        {
            byte[] responseBuffer = new byte[1024];
            int bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);
            byte[] response = new byte[bytesRead];
            Array.Copy(responseBuffer, response, bytesRead);
            return response;
        }

        private void InsertDataIntoDatabase(string id, string data1, string data2, string data3, string data4, string detectionTime, string channel)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO RFIDData (ID, NumSerie, RefArticle, Largeur, Longueur, DetectionTime, Channel) VALUES (@ID, @NumSerie, @RefArticle, @Largeur, @Longueur, @DetectionTime, @Channel)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.Parameters.AddWithValue("@NumSerie", data1);
                    command.Parameters.AddWithValue("@RefArticle", data2);
                    command.Parameters.AddWithValue("@Largeur", data3);
                    command.Parameters.AddWithValue("@Longueur", data4);
                    command.Parameters.AddWithValue("@DetectionTime", detectionTime);
                    command.Parameters.AddWithValue("@Channel", channel);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }


        private void AddToListView(string id, string data1, string data2, string data3, string data4, string detectionTime, string channel)
        {
            if (!idChannelMap.ContainsKey(id))
            {
                idChannelMap[id] = new HashSet<string>();
            }
            if (!idChannelMap[id].Contains(channel))
            {
                idChannelMap[id].Add(channel);

                ListViewItem item = new ListViewItem(id);
                item.SubItems.Add(data1);
                item.SubItems.Add(data2);
                item.SubItems.Add(data3);
                item.SubItems.Add(data4);
                item.SubItems.Add(detectionTime);
                item.SubItems.Add(channel);
                listView1.Items.Add(item);

            }

        }

        private void doNothing() { }

        private async void ReadBT_Click(object sender, EventArgs e)
        {
            if (!StopBT.Visible)
            {
                // Mode de lecture unique
                try
                {
                    using (TcpClient client = new TcpClient(ipAddress, port))
                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] setTypeCommand = new byte[] { 0x00, 0x06, 0x04, 0x02, 0x38, 0x30 };
                        await SetTypeCommandAsync(stream, setTypeCommand);

                        if (channel == 3)
                        {
                            await ReadDataAsync(stream, 1, true);
                            await ReadDataAsync(stream, 2, true);
                        }
                        else
                        {
                            await ReadDataAsync(stream, channel, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Exception: {ex.Message}");
                }
            }
            else
            {
                // Mode de lecture continue
                cancellationTokenSource = new CancellationTokenSource();
                readThread = new Thread(() => ContinuousRead(cancellationTokenSource.Token));
                readThread.Start();
            }
        }

        private async void ContinuousRead(CancellationToken cancellationToken)
        {
            try
            {
                using (TcpClient client = new TcpClient(ipAddress, port))
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] setTypeCommand = new byte[] { 0x00, 0x06, 0x04, 0x02, 0x38, 0x30 };
                    await SetTypeCommandAsync(stream, setTypeCommand);

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        if (channel == 3)
                        {
                            await ReadDataAsync(stream, 1, false);
                            await ReadDataAsync(stream, 2, false);
                        }
                        else
                        {
                            await ReadDataAsync(stream, channel, false);
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Cancellation requested, exit gracefully
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message}");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 1)
            {
                StopBT.Visible = true;
            }
            else
            {
                StopBT.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (TcpClient client = new TcpClient(ipAddress, port))
            using (NetworkStream stream = client.GetStream())
            {
                byte[] stopCommand = new byte[] { 0x00, 0x04, 0x02, 0x02 };
                stream.WriteAsync(stopCommand, 0, stopCommand.Length);
                stopCommand = new byte[] { 0x00, 0x04, 0x02, 0x04 };
                stream.WriteAsync(stopCommand, 0, stopCommand.Length);
            }

            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();


            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            channel = comboBox2.SelectedIndex + 1;

        }

        private void SubmitBT_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.Items.Count > 0)
                {
                    foreach (ListViewItem item in listView1.Items)
                    {
                        string id = item.SubItems[0].Text;
                        string data1 = item.SubItems[1].Text;
                        string data2 = item.SubItems[2].Text;
                        string data3 = item.SubItems[3].Text;
                        string data4 = item.SubItems[4].Text;
                        string detectionTime = item.SubItems[5].Text;
                        string channel = item.SubItems[6].Text;

                        InsertDataIntoDatabase(id, data1, data2, data3, data4, detectionTime, channel);
                    }

                    MessageBox.Show("Tous les éléments ont été insérés dans la base de données avec succès.");
                }
                else
                {
                    MessageBox.Show("Il n'y a aucun élément a insérés dans la base de données.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'insertion des données : {ex.Message}");
            }

            listView1.Items.Clear();
        }
    }
}
