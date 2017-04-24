namespace EzrealtyToRia
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbRiaExportAll = new System.Windows.Forms.CheckBox();
            this.btSendRiaToFtp = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbMestoExportAll = new System.Windows.Forms.CheckBox();
            this.btSendMestoToFtp = new System.Windows.Forms.Button();
            this.btMakeXmlMesto = new System.Windows.Forms.Button();
            this.gbLun = new System.Windows.Forms.GroupBox();
            this.cbLunSendAll = new System.Windows.Forms.CheckBox();
            this.btLunSendFile = new System.Windows.Forms.Button();
            this.btLunMakeFile = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbGetAllPliz = new System.Windows.Forms.CheckBox();
            this.btSendPlizToFtp = new System.Windows.Forms.Button();
            this.btMakePliz = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gbLun.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(153, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Сформировать файл";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbRiaExportAll);
            this.groupBox1.Controls.Add(this.btSendRiaToFtp);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 100);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "dom.ria.ua";
            // 
            // cbRiaExportAll
            // 
            this.cbRiaExportAll.AutoSize = true;
            this.cbRiaExportAll.Location = new System.Drawing.Point(6, 77);
            this.cbRiaExportAll.Name = "cbRiaExportAll";
            this.cbRiaExportAll.Size = new System.Drawing.Size(171, 17);
            this.cbRiaExportAll.TabIndex = 2;
            this.cbRiaExportAll.Text = "Выгрузить все объявления?";
            this.cbRiaExportAll.UseVisualStyleBackColor = true;
            // 
            // btSendRiaToFtp
            // 
            this.btSendRiaToFtp.Location = new System.Drawing.Point(6, 48);
            this.btSendRiaToFtp.Name = "btSendRiaToFtp";
            this.btSendRiaToFtp.Size = new System.Drawing.Size(153, 23);
            this.btSendRiaToFtp.TabIndex = 1;
            this.btSendRiaToFtp.Text = "Обновить файл на сервере";
            this.btSendRiaToFtp.UseVisualStyleBackColor = true;
            this.btSendRiaToFtp.Click += new System.EventHandler(this.btSendRiaToFtp_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 240);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(549, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbMestoExportAll);
            this.groupBox2.Controls.Add(this.btSendMestoToFtp);
            this.groupBox2.Controls.Add(this.btMakeXmlMesto);
            this.groupBox2.Location = new System.Drawing.Point(12, 118);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(260, 100);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "mesto.ua";
            // 
            // cbMestoExportAll
            // 
            this.cbMestoExportAll.AutoSize = true;
            this.cbMestoExportAll.Location = new System.Drawing.Point(6, 77);
            this.cbMestoExportAll.Name = "cbMestoExportAll";
            this.cbMestoExportAll.Size = new System.Drawing.Size(171, 17);
            this.cbMestoExportAll.TabIndex = 2;
            this.cbMestoExportAll.Text = "Выгрузить все объявления?";
            this.cbMestoExportAll.UseVisualStyleBackColor = true;
            // 
            // btSendMestoToFtp
            // 
            this.btSendMestoToFtp.Location = new System.Drawing.Point(6, 48);
            this.btSendMestoToFtp.Name = "btSendMestoToFtp";
            this.btSendMestoToFtp.Size = new System.Drawing.Size(153, 23);
            this.btSendMestoToFtp.TabIndex = 1;
            this.btSendMestoToFtp.Text = "Обновить файл на сервере";
            this.btSendMestoToFtp.UseVisualStyleBackColor = true;
            this.btSendMestoToFtp.Click += new System.EventHandler(this.btSendMestoToFtp_Click);
            // 
            // btMakeXmlMesto
            // 
            this.btMakeXmlMesto.Location = new System.Drawing.Point(6, 19);
            this.btMakeXmlMesto.Name = "btMakeXmlMesto";
            this.btMakeXmlMesto.Size = new System.Drawing.Size(153, 23);
            this.btMakeXmlMesto.TabIndex = 0;
            this.btMakeXmlMesto.Text = "Сформировать файл";
            this.btMakeXmlMesto.UseVisualStyleBackColor = true;
            this.btMakeXmlMesto.Click += new System.EventHandler(this.btMakeXmlMesto_Click);
            // 
            // gbLun
            // 
            this.gbLun.Controls.Add(this.cbLunSendAll);
            this.gbLun.Controls.Add(this.btLunSendFile);
            this.gbLun.Controls.Add(this.btLunMakeFile);
            this.gbLun.Location = new System.Drawing.Point(278, 12);
            this.gbLun.Name = "gbLun";
            this.gbLun.Size = new System.Drawing.Size(260, 100);
            this.gbLun.TabIndex = 3;
            this.gbLun.TabStop = false;
            this.gbLun.Text = "lun.ua";
            // 
            // cbLunSendAll
            // 
            this.cbLunSendAll.AutoSize = true;
            this.cbLunSendAll.Location = new System.Drawing.Point(6, 77);
            this.cbLunSendAll.Name = "cbLunSendAll";
            this.cbLunSendAll.Size = new System.Drawing.Size(171, 17);
            this.cbLunSendAll.TabIndex = 2;
            this.cbLunSendAll.Text = "Выгрузить все объявления?";
            this.cbLunSendAll.UseVisualStyleBackColor = true;
            // 
            // btLunSendFile
            // 
            this.btLunSendFile.Location = new System.Drawing.Point(6, 48);
            this.btLunSendFile.Name = "btLunSendFile";
            this.btLunSendFile.Size = new System.Drawing.Size(153, 23);
            this.btLunSendFile.TabIndex = 1;
            this.btLunSendFile.Text = "Обновить файл на сервере";
            this.btLunSendFile.UseVisualStyleBackColor = true;
            this.btLunSendFile.Click += new System.EventHandler(this.btLunSendFile_Click);
            // 
            // btLunMakeFile
            // 
            this.btLunMakeFile.Location = new System.Drawing.Point(6, 19);
            this.btLunMakeFile.Name = "btLunMakeFile";
            this.btLunMakeFile.Size = new System.Drawing.Size(153, 23);
            this.btLunMakeFile.TabIndex = 0;
            this.btLunMakeFile.Text = "Сформировать файл";
            this.btLunMakeFile.UseVisualStyleBackColor = true;
            this.btLunMakeFile.Click += new System.EventHandler(this.btLunMakeFile_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbGetAllPliz);
            this.groupBox3.Controls.Add(this.btSendPlizToFtp);
            this.groupBox3.Controls.Add(this.btMakePliz);
            this.groupBox3.Location = new System.Drawing.Point(278, 118);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(260, 100);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "pliz.info";
            // 
            // cbGetAllPliz
            // 
            this.cbGetAllPliz.AutoSize = true;
            this.cbGetAllPliz.Location = new System.Drawing.Point(6, 77);
            this.cbGetAllPliz.Name = "cbGetAllPliz";
            this.cbGetAllPliz.Size = new System.Drawing.Size(171, 17);
            this.cbGetAllPliz.TabIndex = 2;
            this.cbGetAllPliz.Text = "Выгрузить все объявления?";
            this.cbGetAllPliz.UseVisualStyleBackColor = true;
            // 
            // btSendPlizToFtp
            // 
            this.btSendPlizToFtp.Location = new System.Drawing.Point(6, 48);
            this.btSendPlizToFtp.Name = "btSendPlizToFtp";
            this.btSendPlizToFtp.Size = new System.Drawing.Size(153, 23);
            this.btSendPlizToFtp.TabIndex = 1;
            this.btSendPlizToFtp.Text = "Обновить файл на сервере";
            this.btSendPlizToFtp.UseVisualStyleBackColor = true;
            this.btSendPlizToFtp.Click += new System.EventHandler(this.btSendPlizToFtp_Click);
            // 
            // btMakePliz
            // 
            this.btMakePliz.Location = new System.Drawing.Point(6, 19);
            this.btMakePliz.Name = "btMakePliz";
            this.btMakePliz.Size = new System.Drawing.Size(153, 23);
            this.btMakePliz.TabIndex = 0;
            this.btMakePliz.Text = "Сформировать файл";
            this.btMakePliz.UseVisualStyleBackColor = true;
            this.btMakePliz.Click += new System.EventHandler(this.btMakePliz_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 262);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.gbLun);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.gbLun.ResumeLayout(false);
            this.gbLun.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button btSendRiaToFtp;
        private System.Windows.Forms.CheckBox cbRiaExportAll;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbMestoExportAll;
        private System.Windows.Forms.Button btSendMestoToFtp;
        private System.Windows.Forms.Button btMakeXmlMesto;
        private System.Windows.Forms.GroupBox gbLun;
        private System.Windows.Forms.CheckBox cbLunSendAll;
        private System.Windows.Forms.Button btLunSendFile;
        private System.Windows.Forms.Button btLunMakeFile;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox cbGetAllPliz;
        private System.Windows.Forms.Button btSendPlizToFtp;
        private System.Windows.Forms.Button btMakePliz;
    }
}

