using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.ListViewItem;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RFIDDataReader
{
    public partial class Form1 : Form
    {
        public static string connectionString = "Data Source=DELL_5401\\SAGE;Initial Catalog=MED_CARPET;Integrated Security=True";
        public static ListViewSubItem NumPiece;
        public static ListViewSubItem NumClient;
        public static ListViewSubItem IntitulClient;
        public static ListViewSubItem date;
        public static ListViewSubItem NPiece;
        private Dictionary<int, string> DO_Domaine = new Dictionary<int, string>{ 
            { 0,"Vente" },
            {1,"Achat" },
            {2,"Stock" },
            {3, "Ticket" },
            {4, "Document Interne" }
        };
        public Form1()
        {
            InitializeComponent();
            listView1.FullRowSelect = true;
            listView1.ItemActivate += ListView_ItemActivate;
            this.Resize += new EventHandler(Form1_Resize);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string query = "SELECT DE.DO_Piece, DE.DO_Date, DE.DO_Ref, DE.DO_Tiers, T.CT_INTITULE FROM F_DOCENTETE DE, F_COMPTET T WHERE DE.DO_DOMAINE = 0 AND DE.DO_Type = 2 AND DE.DO_TIERS=T.CT_NUM;";
            ShowData(query);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        public static DataTable SelectDataFromDatabase(string query)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                    connection.Close();
                }
            }

            return dataTable;
        }

        private void ShowData(string query)
        {
            DataTable result = SelectDataFromDatabase(query);

            listView1.Items.Clear(); 

            foreach (DataRow row in result.Rows)
            {
                ListViewItem item = new ListViewItem("PL");
                item.SubItems.Add("A livrer");
                item.SubItems.Add(row["DO_Piece"].ToString());
                item.SubItems.Add(row["DO_Ref"].ToString());
                item.SubItems.Add(row["DO_Date"].ToString());
                item.SubItems.Add(row["DO_Tiers"].ToString());
                item.SubItems.Add(row["CT_INTITULE"].ToString());

                listView1.Items.Add(item); 
            }
        }

        private void ListView_ItemActivate(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0) { 
                OuvrirBT.Enabled = true;
                NumPiece = listView1.SelectedItems[0].SubItems[2];
                NumClient = listView1.SelectedItems[0].SubItems[5];
                IntitulClient = listView1.SelectedItems[0].SubItems[6];
                date = listView1.SelectedItems[0].SubItems[4];
                NPiece = listView1.SelectedItems[0].SubItems[2];
            }
        }

        private void OuvrirBT_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();

        }

        private void NouveauBT_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            ResizeListViewColumns();
        }

        private void ResizeListViewColumns()
        {
            int[] columnWidths = { 10, 10, 10, 20, 20, 10, 20 }; // En pourcentage

            int totalWidth = listView1.ClientSize.Width;

            for (int i = 0; i < listView1.Columns.Count; i++)
            {
                listView1.Columns[i].Width = (totalWidth * columnWidths[i]) / 100;
            }
        }

    }
}
