namespace ORG.UIL.Desktop
{
    partial class frm_TemplateTransation
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_TemplateTransation));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.bttnCancel = new ORG.Tools.Desktop.Tools.BottunOrg();
            this.bttnDelete = new ORG.Tools.Desktop.Tools.BottunOrg();
            this.bttnPrint = new ORG.Tools.Desktop.Tools.BottunOrg();
            this.bttnSave = new ORG.Tools.Desktop.Tools.BottunOrg();
            this.bttnSavePrint = new ORG.Tools.Desktop.Tools.BottunOrg();
            this.bttnSaveNew = new ORG.Tools.Desktop.Tools.BottunOrg();
            this.bttnNewSavePrint = new ORG.Tools.Desktop.Tools.BottunOrg();
            this.pnlControlNew = new System.Windows.Forms.Panel();
            this.bttnNew = new ORG.Tools.Desktop.Tools.BottunOrg();
            this.pnlAll = new System.Windows.Forms.Panel();
            this.g_info2 = new System.Windows.Forms.GroupBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlItemBody = new System.Windows.Forms.Panel();
            this.toolCashier1 = new ORG.UIL.Desktop.toolCashier();
            this.toolSearchInfo1 = new ORG.UIL.Desktop.Froms.General.toolSearchInfo();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.pnlControls.SuspendLayout();
            this.pnlControlNew.SuspendLayout();
            this.pnlAll.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlItemBody.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlControls
            // 
            this.pnlControls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.pnlControls.Controls.Add(this.bttnCancel);
            this.pnlControls.Controls.Add(this.bttnDelete);
            this.pnlControls.Controls.Add(this.bttnPrint);
            this.pnlControls.Controls.Add(this.bttnSave);
            this.pnlControls.Controls.Add(this.bttnSavePrint);
            this.pnlControls.Controls.Add(this.bttnSaveNew);
            this.pnlControls.Controls.Add(this.bttnNewSavePrint);
            resources.ApplyResources(this.pnlControls, "pnlControls");
            this.pnlControls.Name = "pnlControls";
            // 
            // bttnCancel
            // 
            this.bttnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnCancel.Color = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnCancel.ColorEnter = System.Drawing.Color.White;
            resources.ApplyResources(this.bttnCancel, "bttnCancel");
            this.bttnCancel.FontColor = System.Drawing.Color.White;
            this.bttnCancel.FontColorEnter = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnCancel.Image = global::ORG.UIL.Desktop.Properties.Resources._02Search;
            this.bttnCancel.ImageEnter = global::ORG.UIL.Desktop.Properties.Resources._02Searchb;
            this.bttnCancel.ImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bttnCancel.Name = "bttnCancel";
            this.bttnCancel.WithSplitter = false;
            this.bttnCancel.WithTitle = true;
            this.bttnCancel.Click += new System.EventHandler(this.CancelFile);
            // 
            // bttnDelete
            // 
            this.bttnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnDelete.Color = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnDelete.ColorEnter = System.Drawing.Color.White;
            resources.ApplyResources(this.bttnDelete, "bttnDelete");
            this.bttnDelete.FontColor = System.Drawing.Color.White;
            this.bttnDelete.FontColorEnter = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnDelete.Image = global::ORG.UIL.Desktop.Properties.Resources._02Delete;
            this.bttnDelete.ImageEnter = global::ORG.UIL.Desktop.Properties.Resources._02Deleteb;
            this.bttnDelete.ImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bttnDelete.Name = "bttnDelete";
            this.bttnDelete.WithSplitter = true;
            this.bttnDelete.WithTitle = true;
            this.bttnDelete.Click += new System.EventHandler(this.DeleteFile);
            // 
            // bttnPrint
            // 
            this.bttnPrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnPrint.Color = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnPrint.ColorEnter = System.Drawing.Color.White;
            resources.ApplyResources(this.bttnPrint, "bttnPrint");
            this.bttnPrint.FontColor = System.Drawing.Color.White;
            this.bttnPrint.FontColorEnter = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnPrint.Image = global::ORG.UIL.Desktop.Properties.Resources._02Print;
            this.bttnPrint.ImageEnter = global::ORG.UIL.Desktop.Properties.Resources._02Printb;
            this.bttnPrint.ImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bttnPrint.Name = "bttnPrint";
            this.bttnPrint.WithSplitter = true;
            this.bttnPrint.WithTitle = true;
            this.bttnPrint.Click += new System.EventHandler(this.PrintFile);
            // 
            // bttnSave
            // 
            this.bttnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnSave.Color = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnSave.ColorEnter = System.Drawing.Color.White;
            resources.ApplyResources(this.bttnSave, "bttnSave");
            this.bttnSave.FontColor = System.Drawing.Color.White;
            this.bttnSave.FontColorEnter = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnSave.Image = global::ORG.UIL.Desktop.Properties.Resources._02Save;
            this.bttnSave.ImageEnter = global::ORG.UIL.Desktop.Properties.Resources._02Saveb;
            this.bttnSave.ImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bttnSave.Name = "bttnSave";
            this.bttnSave.WithSplitter = true;
            this.bttnSave.WithTitle = true;
            this.bttnSave.Click += new System.EventHandler(this.SaveFile);
            // 
            // bttnSavePrint
            // 
            this.bttnSavePrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnSavePrint.Color = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnSavePrint.ColorEnter = System.Drawing.Color.White;
            resources.ApplyResources(this.bttnSavePrint, "bttnSavePrint");
            this.bttnSavePrint.FontColor = System.Drawing.Color.White;
            this.bttnSavePrint.FontColorEnter = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnSavePrint.Image = global::ORG.UIL.Desktop.Properties.Resources._02SavePrint;
            this.bttnSavePrint.ImageEnter = global::ORG.UIL.Desktop.Properties.Resources._02SavePrintb;
            this.bttnSavePrint.ImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bttnSavePrint.Name = "bttnSavePrint";
            this.bttnSavePrint.WithSplitter = true;
            this.bttnSavePrint.WithTitle = true;
            this.bttnSavePrint.Click += new System.EventHandler(this.SavePrintFile);
            // 
            // bttnSaveNew
            // 
            this.bttnSaveNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnSaveNew.Color = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnSaveNew.ColorEnter = System.Drawing.Color.White;
            resources.ApplyResources(this.bttnSaveNew, "bttnSaveNew");
            this.bttnSaveNew.FontColor = System.Drawing.Color.White;
            this.bttnSaveNew.FontColorEnter = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnSaveNew.Image = global::ORG.UIL.Desktop.Properties.Resources.SaveNew22;
            this.bttnSaveNew.ImageEnter = global::ORG.UIL.Desktop.Properties.Resources.SaveNew22b;
            this.bttnSaveNew.ImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bttnSaveNew.Name = "bttnSaveNew";
            this.bttnSaveNew.WithSplitter = true;
            this.bttnSaveNew.WithTitle = true;
            this.bttnSaveNew.Click += new System.EventHandler(this.NewSaveFile);
            // 
            // bttnNewSavePrint
            // 
            this.bttnNewSavePrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnNewSavePrint.Color = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnNewSavePrint.ColorEnter = System.Drawing.Color.White;
            resources.ApplyResources(this.bttnNewSavePrint, "bttnNewSavePrint");
            this.bttnNewSavePrint.FontColor = System.Drawing.Color.White;
            this.bttnNewSavePrint.FontColorEnter = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnNewSavePrint.Image = global::ORG.UIL.Desktop.Properties.Resources._02SavePrintNew;
            this.bttnNewSavePrint.ImageEnter = global::ORG.UIL.Desktop.Properties.Resources._02SavePrintNewb;
            this.bttnNewSavePrint.ImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bttnNewSavePrint.Name = "bttnNewSavePrint";
            this.bttnNewSavePrint.WithSplitter = true;
            this.bttnNewSavePrint.WithTitle = true;
            this.bttnNewSavePrint.Click += new System.EventHandler(this.NewSavePrintFile);
            // 
            // pnlControlNew
            // 
            this.pnlControlNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.pnlControlNew.Controls.Add(this.pnlControls);
            this.pnlControlNew.Controls.Add(this.bttnNew);
            resources.ApplyResources(this.pnlControlNew, "pnlControlNew");
            this.pnlControlNew.Name = "pnlControlNew";
            // 
            // bttnNew
            // 
            this.bttnNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnNew.Color = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnNew.ColorEnter = System.Drawing.Color.White;
            resources.ApplyResources(this.bttnNew, "bttnNew");
            this.bttnNew.FontColor = System.Drawing.Color.White;
            this.bttnNew.FontColorEnter = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnNew.Image = global::ORG.UIL.Desktop.Properties.Resources._02New;
            this.bttnNew.ImageEnter = global::ORG.UIL.Desktop.Properties.Resources._02Newb;
            this.bttnNew.ImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bttnNew.Name = "bttnNew";
            this.bttnNew.WithSplitter = false;
            this.bttnNew.WithTitle = true;
            this.bttnNew.Click += new System.EventHandler(this.NewFile);
            // 
            // pnlAll
            // 
            this.pnlAll.Controls.Add(this.g_info2);
            this.pnlAll.Controls.Add(this.pnlControlNew);
            resources.ApplyResources(this.pnlAll, "pnlAll");
            this.pnlAll.Name = "pnlAll";
            // 
            // g_info2
            // 
            this.g_info2.BackColor = System.Drawing.Color.Gainsboro;
            resources.ApplyResources(this.g_info2, "g_info2");
            this.g_info2.Name = "g_info2";
            this.g_info2.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Gainsboro;
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.pnlItemBody, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnlAll, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // pnlItemBody
            // 
            this.pnlItemBody.BackColor = System.Drawing.Color.White;
            this.pnlItemBody.Controls.Add(this.toolCashier1);
            this.pnlItemBody.Controls.Add(this.toolSearchInfo1);
            resources.ApplyResources(this.pnlItemBody, "pnlItemBody");
            this.pnlItemBody.Name = "pnlItemBody";
            // 
            // toolCashier1
            // 
            this.toolCashier1.BackColor = System.Drawing.Color.White;
            this.toolCashier1.category = null;
            this.toolCashier1.CountItems = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            this.toolCashier1.CurrItemView = ((ORGEntity.Item)(resources.GetObject("toolCashier1.CurrItemView")));
            this.toolCashier1.Discount = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            resources.ApplyResources(this.toolCashier1, "toolCashier1");
            this.toolCashier1.ID = 0;
            this.toolCashier1.IsAdmin = false;
            this.toolCashier1.ItemGroup = null;
            this.toolCashier1.ItemListView = null;
            this.toolCashier1.ItemListViewDetail = null;
            this.toolCashier1.Name = "toolCashier1";
            this.toolCashier1.Net = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            this.toolCashier1.Service = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            this.toolCashier1.Tag = "";
            this.toolCashier1.Tax = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            this.toolCashier1.Total = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            this.toolCashier1.TypeID = ((long)(0));
            this.toolCashier1.UnitDescription = "";
            this.toolCashier1.UnitList = null;
            this.toolCashier1._Refrash += new ORG.UIL._evet(this.toolCashier1__Refrash);
            // 
            // toolSearchInfo1
            // 
            this.toolSearchInfo1.BackColor = System.Drawing.Color.White;
            this.toolSearchInfo1.DefulteImage = null;
            resources.ApplyResources(this.toolSearchInfo1, "toolSearchInfo1");
            this.toolSearchInfo1.EMPImage = null;
            this.toolSearchInfo1.FromImage = null;
            this.toolSearchInfo1.InvoiceView = ((System.Collections.Generic.List<ORGEntity.Invoice>)(resources.GetObject("toolSearchInfo1.InvoiceView")));
            this.toolSearchInfo1.Name = "toolSearchInfo1";
            this.toolSearchInfo1.Paging = 0;
            this.toolSearchInfo1.ToImage = null;
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.DataPropertyName = "TypeInvImage";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(150)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle1.NullValue = null;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridViewImageColumn1.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.dataGridViewImageColumn1, "dataGridViewImageColumn1");
            this.dataGridViewImageColumn1.Image = global::ORG.UIL.Desktop.Properties.Resources._02Preview;
            this.dataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.ReadOnly = true;
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // frm_TemplateTransation
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_TemplateTransation";
            this.Load += new System.EventHandler(this.LoadForm);
            this.pnlControls.ResumeLayout(false);
            this.pnlControlNew.ResumeLayout(false);
            this.pnlAll.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlItemBody.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        public System.Windows.Forms.Panel pnlControls;
        public System.Windows.Forms.Panel pnlControlNew;
        public Froms.General.toolSearchInfo toolSearchInfo1;
        public toolCashier toolCashier1;
        private System.Windows.Forms.Panel pnlAll;
        public System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox g_info2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnlItemBody;
        public Tools.Desktop.Tools.BottunOrg bttnNewSavePrint;
        public Tools.Desktop.Tools.BottunOrg bttnSaveNew;
        public Tools.Desktop.Tools.BottunOrg bttnSavePrint;
        public Tools.Desktop.Tools.BottunOrg bttnCancel;
        public Tools.Desktop.Tools.BottunOrg bttnDelete;
        public Tools.Desktop.Tools.BottunOrg bttnPrint;
        public Tools.Desktop.Tools.BottunOrg bttnSave;
        private Tools.Desktop.Tools.BottunOrg bttnNew;
    }
}