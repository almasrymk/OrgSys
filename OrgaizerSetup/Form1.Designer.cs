namespace OrgaizerSetup
{
    partial class frmSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSetup));
            this.lblClose = new System.Windows.Forms.Label();
            this.lnlMin = new System.Windows.Forms.Label();
            this.btnStartSetup = new System.Windows.Forms.Button();
            this.g_info2 = new System.Windows.Forms.GroupBox();
            this.pnlBrows = new System.Windows.Forms.Panel();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnBrows = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnlp2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnFinish = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.lblLoad = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.picLoad = new System.Windows.Forms.PictureBox();
            this.pnlBrows.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLoad)).BeginInit();
            this.SuspendLayout();
            // 
            // lblClose
            // 
            this.lblClose.AutoSize = true;
            this.lblClose.BackColor = System.Drawing.Color.Transparent;
            this.lblClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblClose.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClose.ForeColor = System.Drawing.Color.DimGray;
            this.lblClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblClose.Location = new System.Drawing.Point(316, 12);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(20, 23);
            this.lblClose.TabIndex = 130;
            this.lblClose.Tag = "Min";
            this.lblClose.Text = "X";
            this.lblClose.Click += new System.EventHandler(this.lblClose_Click);
            // 
            // lnlMin
            // 
            this.lnlMin.AutoSize = true;
            this.lnlMin.BackColor = System.Drawing.Color.Transparent;
            this.lnlMin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnlMin.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnlMin.ForeColor = System.Drawing.Color.DimGray;
            this.lnlMin.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lnlMin.Location = new System.Drawing.Point(293, 12);
            this.lnlMin.Name = "lnlMin";
            this.lnlMin.Size = new System.Drawing.Size(19, 23);
            this.lnlMin.TabIndex = 130;
            this.lnlMin.Tag = "Min";
            this.lnlMin.Text = "_";
            this.lnlMin.Click += new System.EventHandler(this.lnlMin_Click);
            // 
            // btnStartSetup
            // 
            this.btnStartSetup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.btnStartSetup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartSetup.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartSetup.ForeColor = System.Drawing.Color.White;
            this.btnStartSetup.Location = new System.Drawing.Point(100, 14);
            this.btnStartSetup.Name = "btnStartSetup";
            this.btnStartSetup.Size = new System.Drawing.Size(148, 44);
            this.btnStartSetup.TabIndex = 0;
            this.btnStartSetup.Text = "Download / Setup";
            this.btnStartSetup.UseVisualStyleBackColor = false;
            this.btnStartSetup.Click += new System.EventHandler(this.btnStartSetup_Click);
            // 
            // g_info2
            // 
            this.g_info2.BackColor = System.Drawing.Color.Gainsboro;
            this.g_info2.Dock = System.Windows.Forms.DockStyle.Top;
            this.g_info2.Location = new System.Drawing.Point(0, 150);
            this.g_info2.Name = "g_info2";
            this.g_info2.Size = new System.Drawing.Size(350, 2);
            this.g_info2.TabIndex = 191;
            this.g_info2.TabStop = false;
            // 
            // pnlBrows
            // 
            this.pnlBrows.BackColor = System.Drawing.Color.White;
            this.pnlBrows.Controls.Add(this.txtPath);
            this.pnlBrows.Controls.Add(this.btnBrows);
            this.pnlBrows.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlBrows.Location = new System.Drawing.Point(13, 169);
            this.pnlBrows.Name = "pnlBrows";
            this.pnlBrows.Size = new System.Drawing.Size(327, 162);
            this.pnlBrows.TabIndex = 192;
            // 
            // txtPath
            // 
            this.txtPath.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtPath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPath.Font = new System.Drawing.Font("Calibri", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.txtPath.Location = new System.Drawing.Point(13, 47);
            this.txtPath.Multiline = true;
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(231, 26);
            this.txtPath.TabIndex = 1;
            // 
            // btnBrows
            // 
            this.btnBrows.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.btnBrows.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrows.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrows.ForeColor = System.Drawing.Color.White;
            this.btnBrows.Location = new System.Drawing.Point(243, 46);
            this.btnBrows.Name = "btnBrows";
            this.btnBrows.Size = new System.Drawing.Size(70, 28);
            this.btnBrows.TabIndex = 0;
            this.btnBrows.Text = "Brows";
            this.btnBrows.UseVisualStyleBackColor = false;
            this.btnBrows.Click += new System.EventHandler(this.button3_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel2.Controls.Add(this.lnlMin);
            this.panel2.Controls.Add(this.lblClose);
            this.panel2.Controls.Add(this.pnlp2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(350, 150);
            this.panel2.TabIndex = 193;
            // 
            // pnlp2
            // 
            this.pnlp2.BackColor = System.Drawing.Color.Transparent;
            this.pnlp2.BackgroundImage = global::OrgaizerSetup.Properties.Resources.Logo;
            this.pnlp2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnlp2.Location = new System.Drawing.Point(90, 34);
            this.pnlp2.Name = "pnlp2";
            this.pnlp2.Size = new System.Drawing.Size(170, 112);
            this.pnlp2.TabIndex = 168;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 373);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(350, 2);
            this.groupBox1.TabIndex = 195;
            this.groupBox1.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel3.Controls.Add(this.btnRemove);
            this.panel3.Controls.Add(this.btnModify);
            this.panel3.Controls.Add(this.btnFinish);
            this.panel3.Controls.Add(this.btnStartSetup);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 375);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(350, 75);
            this.panel3.TabIndex = 196;
            // 
            // btnRemove
            // 
            this.btnRemove.BackColor = System.Drawing.Color.Firebrick;
            this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemove.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.ForeColor = System.Drawing.Color.White;
            this.btnRemove.Location = new System.Drawing.Point(173, 17);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(103, 42);
            this.btnRemove.TabIndex = 201;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = false;
            this.btnRemove.Visible = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnModify
            // 
            this.btnModify.BackColor = System.Drawing.Color.SeaGreen;
            this.btnModify.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModify.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModify.ForeColor = System.Drawing.Color.White;
            this.btnModify.Location = new System.Drawing.Point(70, 16);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(103, 42);
            this.btnModify.TabIndex = 200;
            this.btnModify.Text = "Modify";
            this.btnModify.UseVisualStyleBackColor = false;
            this.btnModify.Visible = false;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnFinish
            // 
            this.btnFinish.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.btnFinish.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFinish.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFinish.ForeColor = System.Drawing.Color.White;
            this.btnFinish.Location = new System.Drawing.Point(101, 15);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(148, 44);
            this.btnFinish.TabIndex = 1;
            this.btnFinish.Text = "Finish";
            this.btnFinish.UseVisualStyleBackColor = false;
            this.btnFinish.Visible = false;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // lblLoad
            // 
            this.lblLoad.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoad.ForeColor = System.Drawing.Color.DimGray;
            this.lblLoad.Location = new System.Drawing.Point(96, 328);
            this.lblLoad.Name = "lblLoad";
            this.lblLoad.Size = new System.Drawing.Size(169, 23);
            this.lblLoad.TabIndex = 197;
            this.lblLoad.Text = "0%";
            this.lblLoad.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLoad.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SeaGreen;
            this.label1.Location = new System.Drawing.Point(137, 312);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 29);
            this.label1.TabIndex = 198;
            this.label1.Text = "Done";
            this.label1.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.label3.Location = new System.Drawing.Point(31, 249);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(290, 29);
            this.label3.TabIndex = 200;
            this.label3.Text = "You have another Organizer";
            this.label3.Visible = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::OrgaizerSetup.Properties.Resources.images;
            this.pictureBox3.Location = new System.Drawing.Point(107, 179);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(138, 124);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 201;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Visible = false;
            // 
            // picLoad
            // 
            this.picLoad.Image = global::OrgaizerSetup.Properties.Resources.d27180_8ba5d7d0d8ce459aa955f57c6ff5782b_mv2;
            this.picLoad.Location = new System.Drawing.Point(126, 207);
            this.picLoad.Name = "picLoad";
            this.picLoad.Size = new System.Drawing.Size(102, 97);
            this.picLoad.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLoad.TabIndex = 194;
            this.picLoad.TabStop = false;
            this.picLoad.Visible = false;
            // 
            // frmSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(350, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblLoad);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.picLoad);
            this.Controls.Add(this.g_info2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pnlBrows);
            this.Controls.Add(this.pictureBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSetup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Organizer (Setup)";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnlBrows.ResumeLayout(false);
            this.pnlBrows.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLoad)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lnlMin;
        private System.Windows.Forms.Label lblClose;
        private System.Windows.Forms.Button btnStartSetup;
        private System.Windows.Forms.GroupBox g_info2;
        private System.Windows.Forms.Panel pnlp2;
        private System.Windows.Forms.Panel pnlBrows;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnBrows;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox picLoad;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        public System.Windows.Forms.Label lblLoad;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox3;
    }
}

