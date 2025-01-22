namespace RFIDDataReader
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SubmitBT = new System.Windows.Forms.Button();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.StopBT = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.ClearBT = new System.Windows.Forms.Button();
            this.ReadBT = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.ID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Data = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.X = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.largeur = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Longueur = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.detection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CH = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // SubmitBT
            // 
            this.SubmitBT.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SubmitBT.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.SubmitBT.Location = new System.Drawing.Point(629, 472);
            this.SubmitBT.Name = "SubmitBT";
            this.SubmitBT.Size = new System.Drawing.Size(159, 51);
            this.SubmitBT.TabIndex = 13;
            this.SubmitBT.Text = "Submit";
            this.SubmitBT.UseVisualStyleBackColor = true;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Channel 1",
            "Channel 2",
            "Both"});
            this.comboBox2.Location = new System.Drawing.Point(262, 474);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(160, 28);
            this.comboBox2.TabIndex = 12;
            // 
            // StopBT
            // 
            this.StopBT.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StopBT.Location = new System.Drawing.Point(843, 472);
            this.StopBT.Name = "StopBT";
            this.StopBT.Size = new System.Drawing.Size(138, 51);
            this.StopBT.TabIndex = 11;
            this.StopBT.Text = "Stop";
            this.StopBT.UseVisualStyleBackColor = true;
            this.StopBT.Visible = false;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Single Read",
            "Enhanced Read"});
            this.comboBox1.Location = new System.Drawing.Point(12, 474);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(160, 28);
            this.comboBox1.TabIndex = 10;
            // 
            // ClearBT
            // 
            this.ClearBT.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClearBT.Location = new System.Drawing.Point(1082, 472);
            this.ClearBT.Name = "ClearBT";
            this.ClearBT.Size = new System.Drawing.Size(150, 51);
            this.ClearBT.TabIndex = 9;
            this.ClearBT.Text = "Clear";
            this.ClearBT.UseVisualStyleBackColor = true;
            // 
            // ReadBT
            // 
            this.ReadBT.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReadBT.Location = new System.Drawing.Point(1301, 472);
            this.ReadBT.Name = "ReadBT";
            this.ReadBT.Size = new System.Drawing.Size(150, 51);
            this.ReadBT.TabIndex = 8;
            this.ReadBT.Text = "Read";
            this.ReadBT.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ID,
            this.Data,
            this.X,
            this.largeur,
            this.Longueur,
            this.detection,
            this.CH});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 12);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1429, 422);
            this.listView1.TabIndex = 7;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // ID
            // 
            this.ID.Text = "ID";
            this.ID.Width = 180;
            // 
            // Data
            // 
            this.Data.Text = "Numero Serie";
            this.Data.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Data.Width = 150;
            // 
            // X
            // 
            this.X.Text = "Reference Article";
            this.X.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.X.Width = 150;
            // 
            // largeur
            // 
            this.largeur.Text = "Largeur";
            this.largeur.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.largeur.Width = 80;
            // 
            // Longueur
            // 
            this.Longueur.Text = "Longueur";
            this.Longueur.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Longueur.Width = 80;
            // 
            // detection
            // 
            this.detection.Text = "Date de Detection";
            this.detection.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.detection.Width = 200;
            // 
            // CH
            // 
            this.CH.Text = "CH";
            this.CH.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1924, 574);
            this.Controls.Add(this.SubmitBT);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.StopBT);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.ClearBT);
            this.Controls.Add(this.ReadBT);
            this.Controls.Add(this.listView1);
            this.Name = "Form3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RFID Data Reader : Nouvelle Vente";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SubmitBT;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button StopBT;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button ClearBT;
        private System.Windows.Forms.Button ReadBT;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader ID;
        private System.Windows.Forms.ColumnHeader Data;
        private System.Windows.Forms.ColumnHeader X;
        private System.Windows.Forms.ColumnHeader largeur;
        private System.Windows.Forms.ColumnHeader Longueur;
        private System.Windows.Forms.ColumnHeader detection;
        private System.Windows.Forms.ColumnHeader CH;
    }
}