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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Timers;

namespace RFIDDataReader
{
    public partial class Form2 : Form
    {
        private RFIDMethods rfidMethods;
        private System.Windows.Forms.Timer timer;
        public Form2()
        {
            InitializeComponent();
            this.Resize += new EventHandler(Form2_Resize);
            rfidMethods=new RFIDMethods(this, listView1);
            this.FormClosing += new FormClosingEventHandler(Form2_FormClosing);

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // 1000 millisecondes = 1 secondes
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }

        private void Form2_Load(object sender, EventArgs e)   
        {
            string query = "select DE.DO_PIECE, DE.DO_DATE, DE.DO_TIERS,"+
                "AR_Ref, DL_Design, DL_Qte, DL_PrixUnitaire, DL_MontantHT, DL_MontantTTC, NumSerie from F_DOCLIGNE DL,F_DOCENTETE DE " +
                $"where DE.DO_Domaine=0 and DE.DO_Type = 2 and DE.DO_Piece='{Form1.NumPiece.Text}' "+
                "and DL.DO_Domaine=0 and DL.DO_Type = 2 AND DE.DO_PIECE = DL.DO_PIECE ";
            ShowData(query);

            this.Width = tableLayoutPanel2.Width + listView1.Width + (this.Width - this.ClientSize.Width) -20;
            AdjustFormHeight();

            ResultatCB.Items.Add(Form1.NumClient.Text);
            ResultatCB.Items.Add(Form1.IntitulClient.Text);

            DateCB.Items.Add(Form1.date.Text);

            NPieceCB.Items.Add("Num Piece");

            NPieceRTB.Text = Form1.NPiece.Text;

            OptionCB.SelectedIndex = 0;
            ResultatCB.SelectedIndex = 0;
            DateCB.SelectedIndex = 0;
            NPieceCB.SelectedIndex = 0;

            OptionCB.SelectedIndexChanged += OptionCB_SelectedIndexChanged;

        }

        private void AdjustFormHeight()
        {
            int totalHeight = tableLayoutPanel1.Height + panel1.Height;

            int heightDifference = this.Height - this.ClientSize.Height;

            this.Height = totalHeight + heightDifference +50;
        }

        private void ShowData(string query)
        {
            DataTable result = Form1.SelectDataFromDatabase(query);

            listView1.Items.Clear();

            foreach (DataRow row in result.Rows)
            {
                ListViewItem item = new ListViewItem(row["AR_Ref"].ToString());
                item.SubItems.Add(row["DL_Design"].ToString());
                item.SubItems.Add(row["DL_Qte"].ToString());
                item.SubItems.Add(row["DL_PrixUnitaire"].ToString());
                item.SubItems.Add(row["DL_MontantHT"].ToString());
                item.SubItems.Add(row["DL_MontantTTC"].ToString());
                item.SubItems.Add(row["NumSerie"].ToString());

                listView1.Items.Add(item);
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void OptionCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OptionCB.SelectedIndex == 0) { 
                ResultatCB.SelectedIndex = 0;
            }
            else if (OptionCB.SelectedIndex == 1) 
            {
                ResultatCB.SelectedIndex = 1;
            }
        }

        private void ResultatCB_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Resize(object sender, EventArgs e)
        {
            ResizeListViewColumns();
        }

        private void ResizeListViewColumns()
        {
            int[] columnWidths = { 10, 20, 20, 10, 20, 20,0}; // En pourcentage

            int totalWidth = listView1.ClientSize.Width;

            for (int i = 0; i < listView1.Columns.Count; i++)
            {
                listView1.Columns[i].Width = (totalWidth * columnWidths[i]) / 100;
            }
        }

        private async void LireBT_Click(object sender, EventArgs e)
        {

            rfidMethods.cancellationTokenSource = new CancellationTokenSource();
            rfidMethods.readThread = new Thread(() => rfidMethods.ContinuousRead(rfidMethods.cancellationTokenSource.Token));
            rfidMethods.readThread.Start();
        }

        private void StopBT_Click(object sender, EventArgs e)
        {
            using (TcpClient client = new TcpClient(RFIDMethods.ipAddress, RFIDMethods.port))
            using (NetworkStream stream = client.GetStream())
            {
                byte[] stopCommand = new byte[] { 0x00, 0x04, 0x02, 0x02 };
                stream.WriteAsync(stopCommand, 0, stopCommand.Length);
                stopCommand = new byte[] { 0x00, 0x04, 0x02, 0x04 };
                stream.WriteAsync(stopCommand, 0, stopCommand.Length);
            }

            if (rfidMethods.cancellationTokenSource != null)
            {
                rfidMethods.cancellationTokenSource.Cancel();
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            rfidMethods.memoryMap.Clear();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int lignesVertes = CompterLignesVertes(listView1);
            NbrDetecteL.Text = $"Nombre de produits détectés : {lignesVertes}";
            if (lignesVertes > 0)
            {
                ValiderBT.Enabled = true ;
            }
        }

        private int CompterLignesVertes(System.Windows.Forms.ListView listView)
        {
            int count = 0;
            foreach (ListViewItem item in listView.Items)
            {
                if (item.BackColor == Color.Green)
                {
                    count++;
                }
            }
            return count;
        }

        private List<List<string>> ExtractGreenLinesValues(System.Windows.Forms.ListView listView)
        {
            var ar_Ref = new List<string>();
            var num_Serie = new List<string>();

            foreach (ListViewItem item in listView.Items)
            {
                if (item.BackColor == Color.Green)
                {
                    ar_Ref.Add(item.SubItems[0].Text);
                    num_Serie.Add(item.SubItems[item.SubItems.Count - 1].Text);
                }
            }
            var result = new List<List<string>>{
                ar_Ref,num_Serie
            };
            return result;
        }

        private void ValiderBT_Click(object sender, EventArgs e)
        {
            DataTable souche = GetDocumentSouche(NPieceRTB.Text);
            DataRow row = souche.Rows[0];
            Dictionary<string, bool> dico = QuantityOfStockAvailable(listView1, row["de_no"].ToString());
            if (!dico.Values.First())
            {
                MessageBox.Show($"Erreur : La quantite du stock de l'article '{dico.Keys.First()}' n'est plus disponible.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Dictionary<string, bool> dico2 = QueryReturnsRows(row["de_no"].ToString());
            if (! dico2.Values.First())
            {
                MessageBox.Show($"Erreur : Le numero de série suivant est épuiser : '{dico2.Keys.First()}'.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult result = MessageBox.Show("Êtes-vous sûr de vouloir valider la livraison ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string doSouche = row["DO_Souche"].ToString();
                    string newNumPiece = GetNewPieceNumber(doSouche);
                    DuplicateDocumentHeader(NPieceRTB.Text, newNumPiece);

                    List<List<string>> list = ExtractGreenLinesValues(listView1);
                    int iNewIdentity;
                    int DlNo;
                    string DlNoIn;
                    for (int i = 0; i < list[0].Count; i++)
                    {
                        UpdateStockQuantity(list[0][i], row["de_no"].ToString());
                        iNewIdentity = DuplicateDocumentLine(newNumPiece, NPieceRTB.Text, list[0][i], list[1][i]);
                        DlNo = GetDlNoByCbmarq(iNewIdentity);
                        DlNoIn = GetDlNoIn(list[0][i], list[1][i], row["de_no"].ToString());
                        UpdateBatch(list[0][i], list[1][i], row["de_no"].ToString(), DlNo.ToString(), DlNoIn);
                        InsertNewBatch(list[1][i], list[0][i], row["de_no"].ToString(), DlNoIn, DlNo.ToString());
                    }

                    De_NoRTB.Text = row["de_no"].ToString(); 
                    MessageBox.Show("Votre livraison est validée.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (result == DialogResult.No)
                {
                    MessageBox.Show("Commande annulée.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }

        public DataTable GetDocumentSouche(string doPiece)
        {
            using (SqlConnection conn = new SqlConnection(Form1.connectionString))
            {
                string query = "SELECT Strain, DUM_NO FROM TABLE_DOCUMENT_HEADER WHERE DO_Type = 2 AND DO_Piece = @doPiece";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@doPiece", doPiece);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable result = new DataTable();
                adapter.Fill(result);
                return result;
            }
        }

        public string GetNewPieceNumber(string dcSouche)
        {
            using (SqlConnection conn = new SqlConnection(Form1.connectionString))
            {
                string query = "SELECT dc_piece FROM F_DOCCURRENTPIECE WHERE DC_Domaine = 0 AND DC_IdCol = 3 AND DC_Souche = @dcSouche";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@dcSouche", dcSouche);

                conn.Open();
                object result = cmd.ExecuteScalar();
                conn.Close();

                return result?.ToString();
            }
        }

        public void DuplicateDocumentHeader(string oldPiece, string newPiece)
        {
            using (SqlConnection conn = new SqlConnection(Form1.connectionString)) //as contitestock where 
            {
                string tableDocumentHeader = config["TABLE_DOCUMENT_HEADER"];
                string tableSource = config["TABLE_SOURCE"];
                string columnType = config["COLUMN_TYPE"];
                string columnPiece = config["COLUMN_PIECE"];
            string query = $@"
                INSERT INTO {tableDocumentHeader} (
                    [DOMAIN], [TYPE], [PIECE], [DATE], [REFERENCE], [CLIENT], [CO_NO], [PERIOD], [CURRENCY], [EXCHANGE_RATE], [DE_NO], [LI_NO], [PAYOR_CLIENT],
                    [EXPEDIT], [NB_INVOICE], [BL_INVOICE], [DISCOUNT_RATE], [BALANCE], [PRINTED], [ACCOUNT_NUMBER], [COORD1], [COORD2], [COORD3], [COORD4],
                    [SOURCE], [DELIVERY_DATE], [CONDITION], [TARIFF], [PACKAGING], [PACKAGE_TYPE], [TRANSACTION], [LANGUAGE], [DISCREPANCY], [REGIME],
                    [CATEGORY], [VENTILATED], [AB_NO], [START_SUBSCRIPTION], [END_SUBSCRIPTION], [START_PERIOD], [END_PERIOD], [ACCOUNT],
                    [STATUS], [TIME], [ACCOUNT_NO], [CASHIER_NO], [TRANSFERRED], [CLOSED], [WEB_NO], [PENDING], [SOURCE_ORIGIN], [IFRS_ACCOUNT],
                    [MR_NO], [EXPENSE_TYPE], [EXPENSE_VALUE], [EXPENSE_LINE_TYPE], [FREE_TYPE], [FREE_VALUE], [FREE_LINE_TYPE], [TAX1],
                    [TAX1_TYPE], [TAX1_CATEGORY], [TAX2], [TAX2_TYPE], [TAX2_CATEGORY], [TAX3], [TAX3_TYPE], [TAX3_CATEGORY], [ACCOUNTING_UPDATED],
                    [MOTIF], [CENTRAL_CLIENT], [CONTACT], [ELECTRONIC_INVOICE], [TRANSACTION_TYPE], [ACTUAL_DELIVERY_DATE], [EXPEDITION_DATE],
                    [SUPPLIER_INVOICE], [ORIGINAL_PIECE], [GUID], [E_STATUS], [REGULATION_REQUEST], [ET_NO], [VALIDATED], [SAFE], [TAX_CODE1],
                    [TAX_CODE2], [TAX_CODE3], [TOTAL_HT], [BAP_STATUS], [DISCOUNT], [DOC_TYPE], [CALCULATION_TYPE], [INVOICE_FILE], [NET_TOTAL_HT],
                    [TOTAL_TTC], [NET_PAYABLE], [AMOUNT_PAID], [PAYMENT_REFERENCE], [PAYMENT_ADDRESS], [PAYMENT_LINE], [QUOTE_REASON], [CONVERSION],
                    [CONTAINER], [COMMENT], [DUM_DATE], [DUM_NO]
                )
                SELECT
                    [DOMAIN], 3, @newPiece, GETDATE(), [REFERENCE], [CLIENT], [CO_NO], [PERIOD], [CURRENCY], [EXCHANGE_RATE], [DE_NO], [LI_NO], [PAYOR_CLIENT],
                    [EXPEDIT], [NB_INVOICE], [BL_INVOICE], [DISCOUNT_RATE], [BALANCE], [PRINTED], [ACCOUNT_NUMBER], [COORD1], [COORD2], [COORD3], [COORD4],
                    [SOURCE], GETDATE(), [CONDITION], [TARIFF], [PACKAGING], [PACKAGE_TYPE], [TRANSACTION], [LANGUAGE], [DISCREPANCY], [REGIME],
                    [CATEGORY], [VENTILATED], [AB_NO], [START_SUBSCRIPTION], [END_SUBSCRIPTION], [START_PERIOD], [END_PERIOD], [ACCOUNT],
                    [STATUS], [TIME], [ACCOUNT_NO], [CASHIER_NO], [TRANSFERRED], [CLOSED], [WEB_NO], [PENDING], [SOURCE_ORIGIN], [IFRS_ACCOUNT],
                    [MR_NO], [EXPENSE_TYPE], [EXPENSE_VALUE], [EXPENSE_LINE_TYPE], [FREE_TYPE], [FREE_VALUE], [FREE_LINE_TYPE], [TAX1],
                    [TAX1_TYPE], [TAX1_CATEGORY], [TAX2], [TAX2_TYPE], [TAX2_CATEGORY], [TAX3], [TAX3_TYPE], [TAX3_CATEGORY], [ACCOUNTING_UPDATED],
                    [MOTIF], [CENTRAL_CLIENT], [CONTACT], [ELECTRONIC_INVOICE], [TRANSACTION_TYPE], [ACTUAL_DELIVERY_DATE], [EXPEDITION_DATE],
                    [SUPPLIER_INVOICE], [ORIGINAL_PIECE], [GUID], [E_STATUS], [REGULATION_REQUEST], [ET_NO], [VALIDATED], [SAFE], [TAX_CODE1],
                    [TAX_CODE2], [TAX_CODE3], [TOTAL_HT], [BAP_STATUS], [DISCOUNT], [DOC_TYPE], [CALCULATION_TYPE], [INVOICE_FILE], [NET_TOTAL_HT],
                    [TOTAL_TTC], [NET_PAYABLE], [AMOUNT_PAID], [PAYMENT_REFERENCE], [PAYMENT_ADDRESS], [PAYMENT_LINE], [QUOTE_REASON], [CONVERSION],
                    [CONTAINER], [COMMENT], [DUM_DATE], [DUM_NO]
                FROM {tableSource}
                WHERE {columnType} = 2 AND {columnPiece} = @oldPiece";


                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@oldPiece", oldPiece);
                cmd.Parameters.AddWithValue("@newPiece", newPiece);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public Dictionary<string, bool> QuantityOfStockAvailable(System.Windows.Forms.ListView listView, string deNo)
        {
            using (SqlConnection conn = new SqlConnection(Form1.connectionString))
            {
                string tableArticleStock = config["TABLE_ARTICLE_STOCK"];
                string columnStockQuantity = config["COLUMN_STOCK_QUANTITY"];
                string columnArticleReference = config["COLUMN_ARTICLE_REFERENCE"];
                string columnDepotNumber = config["COLUMN_DEPOT_NUMBER"];
                string query = $@"
                    SELECT {columnStockQuantity}
                    FROM {tableArticleStock}
                    WHERE {columnArticleReference} = @arRef AND {columnDepotNumber} = @deNo";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@deNo", deNo); 

                List<List<string>> list = ExtractGreenLinesValues(listView);

                conn.Open(); 
                foreach (string arRef in list[0])
                {
                    cmd.Parameters.Clear(); 
                    cmd.Parameters.AddWithValue("@arRef", arRef); 

                    object result = cmd.ExecuteScalar();
                    int quantity = int.Parse(result?.ToString());
                    if (quantity <= 0)
                    {
                        return new Dictionary<string, bool> { { arRef, false } };
                    }
                }
                conn.Close(); 

                return new Dictionary<string, bool> { { "finito", true } };
            }
        }

        public Dictionary<string, bool> QueryReturnsRows(string deNo)
        {
            using (SqlConnection conn = new SqlConnection(Form1.connectionString))
            {
                List<List<string>> list = ExtractGreenLinesValues(listView1);

                conn.Open(); 

                for (int i = 0; i < list[0].Count && i < list[1].Count; i++)
                {
                    string tableLotSeries = config["TABLE_LOT_SERIES"];
                    string columnLineNumber = config["COLUMN_LINE_NUMBER"];
                    string columnArticleReference = config["COLUMN_ARTICLE_REFERENCE"];
                    string columnDepotNumber = config["COLUMN_DEPOT_NUMBER"];
                    string columnSerialNumber = config["COLUMN_SERIAL_NUMBER"];
                    string columnQuantityRemaining = config["COLUMN_QUANTITY_REMAINING"];
                    string columnStockMovement = config["COLUMN_STOCK_MOVEMENT"];

                    string query = $@"
                        SELECT {columnLineNumber}, *
                        FROM {tableLotSeries}
                        WHERE {columnArticleReference} = @arRef 
                        AND {columnDepotNumber} = @deNo 
                        AND {columnSerialNumber} = @lsNoSerie 
                        AND {columnQuantityRemaining} > 0 
                        AND {columnStockMovement} = 1";


                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@arRef", list[0][i]);
                        cmd.Parameters.AddWithValue("@deNo", deNo);
                        cmd.Parameters.AddWithValue("@lsNoSerie", list[1][i]);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                return new Dictionary<string, bool> { { list[1][i], false } };
                            }
                        }
                    }
                }

                conn.Close(); 
                return new Dictionary<string, bool> { { "finito", true } }; 
            }
        }

        public int DuplicateDocumentLine(string newPiece, string oldPiece, string arRef, string numSerie)
        {
            string query = @"
                INSERT INTO [DOCUMENT_LINES] (
                    [DOMAIN], [DOCUMENT_TYPE], [DOCUMENT_NUMBER], [ORDER_NUMBER], [DELIVERY_NUMBER], [DOCUMENT_DATE], [ORDER_DATE], [DELIVERY_DATE], 
                    [LINE_NUMBER], [REFERENCE], [IS_NOMENCLATURE], [IS_TREMP_PIED], [IS_TREMP_EXEP], [ARTICLE_REFERENCE], [DESIGNATION], [QUANTITY], 
                    [ORDER_QUANTITY], [DELIVERY_QUANTITY], [NET_WEIGHT], [GROSS_WEIGHT], [DISCOUNT1_VALUE], [DISCOUNT1_TYPE], [DISCOUNT3_VALUE], 
                    [DISCOUNT3_TYPE], [UNIT_PRICE], [UNIT_PRICE_BC], [TAX1], [TAX1_RATE_TYPE], [TAX1_TYPE], [TAX2], [TAX2_RATE_TYPE], [TAX2_TYPE], 
                    [SALES_AGENT_NUMBER], [ATTRIBUTE1_NUMBER], [ATTRIBUTE2_NUMBER], [PRICE_REASSESSMENT], [COST], [STOCK_MOVEMENT], [ITEM_NUMBER], 
                    [SUPPLIER_REFERENCE], [UNIT_ENUM], [UNIT_QUANTITY], [INCLUSIVE_TAX], [DEPOT_NUMBER], [REFERENCE_NUMBER], [TYPE_PL], [UNIT_PRICE_DEV], 
                    [UNIT_PRICE_TTC], [LINE_ID], [DELIVERY_DATE_DOC], [ANALYTIC_CODE], [TAX3], [TAX3_RATE_TYPE], [TAX3_TYPE], [CHARGES], [VALUATION], 
                    [COMPOSED_ARTICLE_REFERENCE], [NOT_DELIVERED], [CLIENT_REFERENCE], [AMOUNT_HT], [AMOUNT_TTC], [WEIGHT_INVOICE], [DISCOUNT], 
                    [ORIGINAL_DOCUMENT_NUMBER], [DOCUMENT_DATE], [DELIVERY_QUANTITY], [PACKAGE_NUMBER], [LINK_NUMBER], [RESOURCE_CODE], [RESOURCE_QUANTITY], 
                    [ADVANCEMENT_DATE], [MANUFACTURING_ORDER_NUMBER], [TAX1_CODE], [TAX2_CODE], [TAX3_CODE], [MANUFACTURING_ORDER_PRODUCTION], 
                    [DELIVERY_ORDER_NUMBER], [DELIVERY_DATE_ORDER], [DELIVERY_QUANTITY_ORDER], [OPERATION], [SUBTOTAL_LINE_NUMBER], [ANALYTIC_CODE], 
                    [DOCUMENT_TYPE_CODE]
                )
                SELECT 
                    [DOMAIN], 3, @newDocumentNumber, [ORDER_NUMBER], [DELIVERY_NUMBER], 
                    DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())), [ORDER_DATE], [DELIVERY_DATE], 
                    [LINE_NUMBER], [REFERENCE], [IS_NOMENCLATURE], [IS_TREMP_PIED], [IS_TREMP_EXEP], 
                    [ARTICLE_REFERENCE], [DESIGNATION], [QUANTITY], [ORDER_QUANTITY], [DELIVERY_QUANTITY], 
                    [NET_WEIGHT], [GROSS_WEIGHT], [DISCOUNT1_VALUE], [DISCOUNT1_TYPE], [DISCOUNT3_VALUE], 
                    [DISCOUNT3_TYPE], [UNIT_PRICE], [UNIT_PRICE_BC], [TAX1], [TAX1_RATE_TYPE], [TAX1_TYPE], 
                    [TAX2], [TAX2_RATE_TYPE], [TAX2_TYPE], [SALES_AGENT_NUMBER], [ATTRIBUTE1_NUMBER], [ATTRIBUTE2_NUMBER], 
                    [PRICE_REASSESSMENT], [COST], [STOCK_MOVEMENT], [ITEM_NUMBER], [SUPPLIER_REFERENCE], 
                    [UNIT_ENUM], [UNIT_QUANTITY], [INCLUSIVE_TAX], [DEPOT_NUMBER], [REFERENCE_NUMBER], 
                    [TYPE_PL], [UNIT_PRICE_DEV], [UNIT_PRICE_TTC], 0, DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())), 
                    [ANALYTIC_CODE], [TAX3], [TAX3_RATE_TYPE], [TAX3_TYPE], [CHARGES], [VALUATION], 
                    [COMPOSED_ARTICLE_REFERENCE], [NOT_DELIVERED], [CLIENT_REFERENCE], [AMOUNT_HT], [AMOUNT_TTC], 
                    [WEIGHT_INVOICE], [DISCOUNT], @oldDocumentNumber, [DOCUMENT_DATE], [DELIVERY_QUANTITY], 
                    [PACKAGE_NUMBER], [LINK_NUMBER], [RESOURCE_CODE], [RESOURCE_QUANTITY], [ADVANCEMENT_DATE], 
                    [MANUFACTURING_ORDER_NUMBER], [TAX1_CODE], [TAX2_CODE], [TAX3_CODE], [MANUFACTURING_ORDER_PRODUCTION], 
                    [DELIVERY_ORDER_NUMBER], [DELIVERY_DATE_ORDER], [DELIVERY_QUANTITY_ORDER], [OPERATION], 
                    [SUBTOTAL_LINE_NUMBER], [ANALYTIC_CODE], 3
                FROM [DOCUMENT_LINES]
                WHERE [DOCUMENT_TYPE] = 2 AND [DOCUMENT_NUMBER] = @oldDocumentNumber 
                AND [ARTICLE_REFERENCE] = @articleReference 
                AND [SERIAL_NUMBER] = @serialNumber;

                SELECT SCOPE_IDENTITY();";


            using (SqlConnection connection = new SqlConnection(Form1.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@newPiece", newPiece);
                command.Parameters.AddWithValue("@oldPiece", oldPiece);
                command.Parameters.AddWithValue("@arRef", arRef);
                command.Parameters.AddWithValue("@numSerie", numSerie);

                connection.Open();
                var result = command.ExecuteScalar();
                connection.Close();

                return Convert.ToInt32(result);
            }
        }

        public int GetDlNoByCbmarq(int iNewIdentity)
        {
            string query = "SELECT DUM_NO FROM DOCUMENT_LINES WHERE cbmarq = @InewIdentity";

            using (SqlConnection connection = new SqlConnection(Form1.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@InewIdentity", iNewIdentity);

                connection.Open();

                try
                {
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int dlNo))
                    {
                        return dlNo;
                    }
                    else
                    {
                        throw new Exception("No record found or invalid data returned.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void UpdateBatch(string arRef, string lsNoSerie, string deNo, string DlNoOut, string DlNoIn)
        {
            string query = "UPDATE LOTSERIE SET LS_QteRestant=0, LS_LotEpuise=1, DL_NoOut='@DlNoOut' WHERE AR_Ref=@arRef AND DUM_No=@deNo AND LS_NoSerie=@lsNoSerie  and ls_mvtstock  = 1 and dl_noin = @DlNoIn";

            using (SqlConnection connection = new SqlConnection(Form1.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@arRef", arRef);
                command.Parameters.AddWithValue("@deNo", deNo);
                command.Parameters.AddWithValue("@lsNoSerie", lsNoSerie);
                command.Parameters.AddWithValue("@DlNoIn", DlNoIn);

                connection.Open();

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public string GetDlNoIn(string arRef, string lsNoSerie, string deNo)
        {
            string query = @"
                SELECT DL_NoIn 
                FROM LOTSERIE 
                WHERE AR_Ref = @arRef 
                  AND LS_NoSerie = @lsNoSerie 
                  AND DUM_No = @deNo 
                  AND LS_QteRestant > 0 
                  AND LS_MvtStock = 1";

            using (SqlConnection connection = new SqlConnection(Form1.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@arRef", arRef);
                command.Parameters.AddWithValue("@lsNoSerie", lsNoSerie);
                command.Parameters.AddWithValue("@deNo", deNo);

                connection.Open();

                try
                {
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        return result.ToString();
                    }
                    else
                    {
                        return null; 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                    throw; 
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void InsertNewBatch(string lsNoSerie, string arRef, string deNo, string dlNoIn, string dlNoOut)
        {
            string query = @"
            INSERT INTO F_LOTSERIE (
                ARTICLE_REFERENCE, 
                SERIAL_NUMBER, 
                EXPIRATION_DATE, 
                MANUFACTURE_DATE, 
                QUANTITY, 
                REMAINING_QUANTITY, 
                RESERVED_QUANTITY, 
                IS_LOT_EXHAUSTED, 
                DEPOT_NUMBER, 
                IN_DOCUMENT_LINE_NUMBER, 
                OUT_DOCUMENT_LINE_NUMBER, 
                STOCK_MOVEMENT_TYPE, 
                COMPLEMENTARY_INFO
            )
            VALUES (
                @ArticleReference, 
                @SerialNumber,  
                DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())), 
                DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())), 
                1, 
                0, 
                0, 
                1, 
                @DepotNumber, 
                @InDocumentLineNumber, 
                @OutDocumentLineNumber, 
                3, 
                ''
            );";

            using (SqlConnection connection = new SqlConnection(Form1.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ArticleReference", arRef);
                command.Parameters.AddWithValue("@SerialNumber", lsNoSerie);
                command.Parameters.AddWithValue("@DepotNumber", deNo);
                command.Parameters.AddWithValue("@InDocumentLineNumber", dlNoIn);
                command.Parameters.AddWithValue("@OutDocumentLineNumber", dlNoOut);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateStockQuantity(string arRef, string deNo)
        {
            string query = @"
            UPDATE F_ARTICLE_STOCK 
            SET 
                STOCK_QUANTITY = STOCK_QUANTITY - 1, 
                RESERVED_QUANTITY = RESERVED_QUANTITY - 1 
            WHERE 
                ARTICLE_REFERENCE = '@ArticleReference' 
                AND DEPOT_NUMBER = '@DepotNumber';
            ";

            using (SqlConnection connection = new SqlConnection(Form1.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ArticleReference", arRef);
                command.Parameters.AddWithValue("@DepotNumber", deNo);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        public Dictionary<string, List<string>> GetNewBatch()
        {
            using (SqlConnection conn = new SqlConnection(Form1.connectionString))
            {
                conn.Open(); 
                int i = 0;
                var resultDict = new Dictionary<string, List<string>>
                {
                    { "LS_Qte", new List<string>() },
                    { "LS_QteRestant", new List<string>() },
                    { "LS_LotEpuise", new List<string>() }
                };

                List<List<string>> list = ExtractGreenLinesValues(listView1);
                while (list[0].Count > 0 && list[1].Count > 0)
                {
                    string query = $"select Serie_Number, Batch_Quantity, Remaining_Batch_Quantity, Exhausted_Batch from Batch_SeriesNum where AR_Ref='{list[0][i]}' and Remaining_Batch_Quantity>0 and DUM_No={De_NoRTB.Text} and Batch_SeriesNum='{list[1][i]}';";
                    list[0].RemoveAt(i);
                    list[1].RemoveAt(i);
                    i++;

                    SqlCommand cmd = new SqlCommand(query, conn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultDict["Batch_Quantity"].Add(reader["Batch_Quantity"].ToString());
                            resultDict["Remaining_Batch_Quantity"].Add(reader["Remaining_Batch_Quantity"].ToString());
                            resultDict["Exhausted_Batch"].Add(reader["Exhausted_Batch"].ToString());
                        }
                    }
                }

                return resultDict;
            }
        }

        private void De_NoRTB_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
