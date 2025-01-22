using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RFIDDataReader
{
    public class RFIDMethods
    {
        public static string ipAddress = "169.254.10.12";
        public static int port = 10000;
        public CancellationTokenSource cancellationTokenSource;
        public Thread readThread; // Thread for continuous reading
        public Dictionary<int, HashSet<string>> memoryMap = new Dictionary<int, HashSet<string>>();
        public static int mapId=0;
        private ListView listView;
        private Form parentForm;

        public RFIDMethods(Form parentForm ,ListView listView) {
            this.listView = listView;
            this.parentForm = parentForm;
        }


        public async Task SetTypeCommandAsync(NetworkStream stream, byte[] setTypeCommand)
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

        public async Task ReadDataAsync(NetworkStream stream, int channel, bool singleRead)
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
                parentForm.Invoke((MethodInvoker)delegate
                {
                    if (finalResponse[3] == 0xD2)
                    {
                        memoryMap.Add(mapId++, new HashSet<string> { datas[1], datas[0] });
                        ColorListViewItem(listView, datas[1], datas[0], System.Drawing.Color.Red);
                    }
                    else
                    {
                        foreach(int id in memoryMap.Keys){
                            if (memoryMap[id].Contains(datas[1]) && memoryMap[id].Contains(datas[0]))
                            {
                                ColorListViewItem(listView, datas[1], datas[0], System.Drawing.Color.Green);
                            }
                        }
                    }

                });
            }

        }

        private void ColorListViewItem(ListView listView, string refArt, string NumSerie, System.Drawing.Color color)
        {
            foreach (ListViewItem item in listView.Items)
            {
                if (item.SubItems[0].Text == refArt && item.SubItems[6].Text == NumSerie)
                {
                    item.BackColor = color;

                    foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                    {
                        subItem.BackColor = color;
                    }
                }
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

        public async void ContinuousRead(CancellationToken cancellationToken)
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
                        await ReadDataAsync(stream, 1, false);
                        await ReadDataAsync(stream, 2, false);
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

    }
}
