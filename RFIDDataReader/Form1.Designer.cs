namespace RFIDDataReader
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.OuvrirBT = new System.Windows.Forms.Button();
            this.NouveauBT = new System.Windows.Forms.Button();
            this.SupprimerBT = new System.Windows.Forms.Button();
            this.FermerBT = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Statut = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Npiece = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Ref = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Nclient = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IntituleClient = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // OuvrirBT
            // 
            this.OuvrirBT.Enabled = false;
            this.OuvrirBT.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OuvrirBT.Location = new System.Drawing.Point(1133, 656);
            this.OuvrirBT.Name = "OuvrirBT";
            this.OuvrirBT.Size = new System.Drawing.Size(114, 45);
            this.OuvrirBT.TabIndex = 3;
            this.OuvrirBT.Text = "Ouvrir";
            this.OuvrirBT.UseVisualStyleBackColor = true;
            this.OuvrirBT.Click += new System.EventHandler(this.OuvrirBT_Click);
            // 
            // NouveauBT
            // 
            this.NouveauBT.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NouveauBT.Location = new System.Drawing.Point(1253, 656);
            this.NouveauBT.Name = "NouveauBT";
            this.NouveauBT.Size = new System.Drawing.Size(114, 45);
            this.NouveauBT.TabIndex = 2;
            this.NouveauBT.Text = "Nouveau";
            this.NouveauBT.UseVisualStyleBackColor = true;
            this.NouveauBT.Click += new System.EventHandler(this.NouveauBT_Click);
            // 
            // SupprimerBT
            // 
            this.SupprimerBT.Enabled = false;
            this.SupprimerBT.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SupprimerBT.Location = new System.Drawing.Point(1373, 656);
            this.SupprimerBT.Name = "SupprimerBT";
            this.SupprimerBT.Size = new System.Drawing.Size(114, 45);
            this.SupprimerBT.TabIndex = 1;
            this.SupprimerBT.Text = "Supprimer";
            this.SupprimerBT.UseVisualStyleBackColor = true;
            // 
            // FermerBT
            // 
            this.FermerBT.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FermerBT.Location = new System.Drawing.Point(1493, 656);
            this.FermerBT.Name = "FermerBT";
            this.FermerBT.Size = new System.Drawing.Size(114, 45);
            this.FermerBT.TabIndex = 0;
            this.FermerBT.Text = "Fermer";
            this.FermerBT.UseVisualStyleBackColor = true;
            this.FermerBT.Click += new System.EventHandler(this.button1_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Type,
            this.Statut,
            this.Npiece,
            this.Ref,
            this.Date,
            this.Nclient,
            this.IntituleClient});
            this.tableLayoutPanel1.SetColumnSpan(this.listView1, 5);
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(3, 3);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1604, 647);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // Type
            // 
            this.Type.Text = "Type";
            this.Type.Width = 100;
            // 
            // Statut
            // 
            this.Statut.Text = "Statut";
            this.Statut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Statut.Width = 150;
            // 
            // Npiece
            // 
            this.Npiece.Text = "NumPiece";
            this.Npiece.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Npiece.Width = 150;
            // 
            // Ref
            // 
            this.Ref.Text = "Reference";
            this.Ref.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Ref.Width = 150;
            // 
            // Date
            // 
            this.Date.Text = "Date";
            this.Date.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Date.Width = 150;
            // 
            // Nclient
            // 
            this.Nclient.Text = "NumClient";
            this.Nclient.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Nclient.Width = 100;
            // 
            // IntituleClient
            // 
            this.IntituleClient.Text = "Intitule Client";
            this.IntituleClient.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.IntituleClient.Width = 200;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.Controls.Add(this.FermerBT, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.SupprimerBT, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.NouveauBT, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.listView1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.OuvrirBT, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 51F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1610, 704);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1610, 704);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "RFID Data Reader";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button NouveauBT;
        private System.Windows.Forms.Button SupprimerBT;
        private System.Windows.Forms.Button FermerBT;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader Type;
        private System.Windows.Forms.ColumnHeader Statut;
        private System.Windows.Forms.ColumnHeader Npiece;
        private System.Windows.Forms.ColumnHeader Ref;
        private System.Windows.Forms.ColumnHeader Date;
        private System.Windows.Forms.ColumnHeader Nclient;
        private System.Windows.Forms.ColumnHeader IntituleClient;
        private System.Windows.Forms.Button OuvrirBT;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

