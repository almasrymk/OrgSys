namespace ORG.UIL.Desktop
{
    partial class frm_TemplateData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_TemplateData));
            this.pnlBorder = new System.Windows.Forms.Panel();
            this.pnlMainData = new System.Windows.Forms.Panel();
            this.lblmyGroup = new System.Windows.Forms.Label();
            this.txtmyGroup = new System.Windows.Forms.ComboBox();
            this.picImage = new ORG.Tools.Desktop.PicImageTool();
            this.pnlDisplay = new System.Windows.Forms.Panel();
            this.pnlLoadData = new System.Windows.Forms.Panel();
            this.panelUnitTool1 = new ORG.Tools.Desktop.Tools.PanelUnitTool();
            this.lblKeyInfo = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pnlLoadGroup = new System.Windows.Forms.Panel();
            this.panelGroupTool1 = new ORG.Tools.Desktop.Tools.PanelGroupTool();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.pnlControl1 = new System.Windows.Forms.Panel();
            this.bttnRefrach = new ORG.Tools.Desktop.Tools.BottunOrg();
            this.bottunOrg5 = new ORG.Tools.Desktop.Tools.BottunOrg();
            this.bottunOrg4 = new ORG.Tools.Desktop.Tools.BottunOrg();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.bttnNew = new ORG.Tools.Desktop.Tools.BottunOrg();
            this.picSetting = new ORG.Tools.Desktop.Tools.BottunOrg();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pnlControl2 = new System.Windows.Forms.Panel();
            this.bttnCancel = new ORG.Tools.Desktop.Tools.BottunOrg();
            this.bttnDelete = new ORG.Tools.Desktop.Tools.BottunOrg();
            this.bttnSave = new ORG.Tools.Desktop.Tools.BottunOrg();
            this.bttnSaveNew = new ORG.Tools.Desktop.Tools.BottunOrg();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlBorder.SuspendLayout();
            this.pnlMainData.SuspendLayout();
            this.pnlDisplay.SuspendLayout();
            this.pnlLoadData.SuspendLayout();
            this.pnlLoadGroup.SuspendLayout();
            this.pnlControls.SuspendLayout();
            this.pnlControl1.SuspendLayout();
            this.pnlControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBorder
            // 
            this.pnlBorder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(150)))), ((int)(((byte)(200)))));
            this.pnlBorder.Controls.Add(this.pnlMainData);
            this.pnlBorder.Controls.Add(this.pnlDisplay);
            this.pnlBorder.Controls.Add(this.panel2);
            this.pnlBorder.Controls.Add(this.pnlControls);
            this.pnlBorder.Controls.Add(this.panel1);
            resources.ApplyResources(this.pnlBorder, "pnlBorder");
            this.pnlBorder.Name = "pnlBorder";
            // 
            // pnlMainData
            // 
            this.pnlMainData.BackColor = System.Drawing.Color.White;
            this.pnlMainData.Controls.Add(this.lblmyGroup);
            this.pnlMainData.Controls.Add(this.txtmyGroup);
            this.pnlMainData.Controls.Add(this.picImage);
            resources.ApplyResources(this.pnlMainData, "pnlMainData");
            this.pnlMainData.Name = "pnlMainData";
            // 
            // lblmyGroup
            // 
            resources.ApplyResources(this.lblmyGroup, "lblmyGroup");
            this.lblmyGroup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.lblmyGroup.Name = "lblmyGroup";
            // 
            // txtmyGroup
            // 
            this.txtmyGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.txtmyGroup, "txtmyGroup");
            this.txtmyGroup.ForeColor = System.Drawing.Color.DimGray;
            this.txtmyGroup.FormattingEnabled = true;
            this.txtmyGroup.Name = "txtmyGroup";
            this.txtmyGroup.SelectedIndexChanged += new System.EventHandler(this.txtmyGroup_SelectedIndexChanged);
            // 
            // picImage
            // 
            resources.ApplyResources(this.picImage, "picImage");
            this.picImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.picImage.Name = "picImage";
            this.picImage.ShowImage = null;
            // 
            // pnlDisplay
            // 
            this.pnlDisplay.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlDisplay.Controls.Add(this.pnlLoadData);
            this.pnlDisplay.Controls.Add(this.pnlLoadGroup);
            this.pnlDisplay.Controls.Add(this.pnlInfo);
            resources.ApplyResources(this.pnlDisplay, "pnlDisplay");
            this.pnlDisplay.Name = "pnlDisplay";
            // 
            // pnlLoadData
            // 
            this.pnlLoadData.BackColor = System.Drawing.Color.White;
            this.pnlLoadData.Controls.Add(this.panelUnitTool1);
            this.pnlLoadData.Controls.Add(this.lblKeyInfo);
            this.pnlLoadData.Controls.Add(this.panel3);
            resources.ApplyResources(this.pnlLoadData, "pnlLoadData");
            this.pnlLoadData.Name = "pnlLoadData";
            // 
            // panelUnitTool1
            // 
            resources.ApplyResources(this.panelUnitTool1, "panelUnitTool1");
            this.panelUnitTool1.BackColor = System.Drawing.Color.White;
            this.panelUnitTool1.ColorBody = System.Drawing.Color.White;
            this.panelUnitTool1.ColorPic = System.Drawing.Color.WhiteSmoke;
            this.panelUnitTool1.FontDescription = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelUnitTool1.FontTitle = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelUnitTool1.HeightItem = 135;
            this.panelUnitTool1.Name = "panelUnitTool1";
            this.panelUnitTool1.NullImage = null;
            this.panelUnitTool1.UnitDescription = "";
            this.panelUnitTool1.WidthItem = 85;
            this.panelUnitTool1.SelectITem += new System.EventHandler(this.panelUnitTool1_SelectITem);
            // 
            // lblKeyInfo
            // 
            this.lblKeyInfo.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblKeyInfo, "lblKeyInfo");
            this.lblKeyInfo.Name = "lblKeyInfo";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // pnlLoadGroup
            // 
            this.pnlLoadGroup.BackColor = System.Drawing.Color.White;
            this.pnlLoadGroup.Controls.Add(this.panelGroupTool1);
            resources.ApplyResources(this.pnlLoadGroup, "pnlLoadGroup");
            this.pnlLoadGroup.Name = "pnlLoadGroup";
            // 
            // panelGroupTool1
            // 
            resources.ApplyResources(this.panelGroupTool1, "panelGroupTool1");
            this.panelGroupTool1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelGroupTool1.ColorControl = System.Drawing.Color.WhiteSmoke;
            this.panelGroupTool1.FontTitle = new System.Drawing.Font("Calibri", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelGroupTool1.HeightItem = 75;
            this.panelGroupTool1.Name = "panelGroupTool1";
            this.panelGroupTool1.NullImage = null;
            this.panelGroupTool1.Select_Image = null;
            this.panelGroupTool1.WidthItem = 112;
            this.panelGroupTool1.SelectITem += new System.EventHandler(this.panelGroupTool1_SelectITem);
            // 
            // pnlInfo
            // 
            this.pnlInfo.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.pnlInfo, "pnlInfo");
            this.pnlInfo.Name = "pnlInfo";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // pnlControls
            // 
            this.pnlControls.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlControls.Controls.Add(this.pnlControl1);
            this.pnlControls.Controls.Add(this.pnlControl2);
            resources.ApplyResources(this.pnlControls, "pnlControls");
            this.pnlControls.Name = "pnlControls";
            // 
            // pnlControl1
            // 
            this.pnlControl1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlControl1.Controls.Add(this.bttnRefrach);
            this.pnlControl1.Controls.Add(this.bottunOrg5);
            this.pnlControl1.Controls.Add(this.bottunOrg4);
            this.pnlControl1.Controls.Add(this.txtSearch);
            this.pnlControl1.Controls.Add(this.bttnNew);
            this.pnlControl1.Controls.Add(this.picSetting);
            this.pnlControl1.Controls.Add(this.panel4);
            resources.ApplyResources(this.pnlControl1, "pnlControl1");
            this.pnlControl1.Name = "pnlControl1";
            // 
            // bttnRefrach
            // 
            this.bttnRefrach.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnRefrach.Color = System.Drawing.Color.WhiteSmoke;
            this.bttnRefrach.ColorEnter = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            resources.ApplyResources(this.bttnRefrach, "bttnRefrach");
            this.bttnRefrach.FontColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnRefrach.FontColorEnter = System.Drawing.Color.White;
            this.bttnRefrach.Image = global::ORG.UIL.Desktop.Properties.Resources._02Refrashb;
            this.bttnRefrach.ImageEnter = global::ORG.UIL.Desktop.Properties.Resources._02Refrash;
            this.bttnRefrach.ImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bttnRefrach.Name = "bttnRefrach";
            this.bttnRefrach.WithSplitter = false;
            this.bttnRefrach.WithTitle = true;
            this.bttnRefrach.Click += new System.EventHandler(this.RefrashRow);
            // 
            // bottunOrg5
            // 
            this.bottunOrg5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bottunOrg5.Color = System.Drawing.Color.WhiteSmoke;
            this.bottunOrg5.ColorEnter = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            resources.ApplyResources(this.bottunOrg5, "bottunOrg5");
            this.bottunOrg5.FontColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bottunOrg5.FontColorEnter = System.Drawing.Color.WhiteSmoke;
            this.bottunOrg5.Image = global::ORG.UIL.Desktop.Properties.Resources.Importb;
            this.bottunOrg5.ImageEnter = global::ORG.UIL.Desktop.Properties.Resources.Import;
            this.bottunOrg5.ImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bottunOrg5.Name = "bottunOrg5";
            this.bottunOrg5.WithSplitter = true;
            this.bottunOrg5.WithTitle = true;
            this.bottunOrg5.Click += new System.EventHandler(this.ImportRow);
            // 
            // bottunOrg4
            // 
            this.bottunOrg4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bottunOrg4.Color = System.Drawing.Color.WhiteSmoke;
            this.bottunOrg4.ColorEnter = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            resources.ApplyResources(this.bottunOrg4, "bottunOrg4");
            this.bottunOrg4.FontColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bottunOrg4.FontColorEnter = System.Drawing.Color.WhiteSmoke;
            this.bottunOrg4.Image = global::ORG.UIL.Desktop.Properties.Resources.Templateb;
            this.bottunOrg4.ImageEnter = global::ORG.UIL.Desktop.Properties.Resources.Template;
            this.bottunOrg4.ImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bottunOrg4.Name = "bottunOrg4";
            this.bottunOrg4.WithSplitter = true;
            this.bottunOrg4.WithTitle = true;
            this.bottunOrg4.Click += new System.EventHandler(this.TemplateRow);
            // 
            // txtSearch
            // 
            resources.ApplyResources(this.txtSearch, "txtSearch");
            this.txtSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // bttnNew
            // 
            this.bttnNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnNew.Color = System.Drawing.Color.WhiteSmoke;
            this.bttnNew.ColorEnter = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            resources.ApplyResources(this.bttnNew, "bttnNew");
            this.bttnNew.FontColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnNew.FontColorEnter = System.Drawing.Color.White;
            this.bttnNew.Image = global::ORG.UIL.Desktop.Properties.Resources._02Newb;
            this.bttnNew.ImageEnter = global::ORG.UIL.Desktop.Properties.Resources._02New;
            this.bttnNew.ImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bttnNew.Name = "bttnNew";
            this.bttnNew.Tag = "";
            this.bttnNew.WithSplitter = true;
            this.bttnNew.WithTitle = true;
            this.bttnNew.Click += new System.EventHandler(this.NewRow);
            // 
            // picSetting
            // 
            resources.ApplyResources(this.picSetting, "picSetting");
            this.picSetting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.picSetting.Color = System.Drawing.Color.WhiteSmoke;
            this.picSetting.ColorEnter = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.picSetting.FontColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.picSetting.FontColorEnter = System.Drawing.Color.White;
            this.picSetting.Image = global::ORG.UIL.Desktop.Properties.Resources._02Searchb;
            this.picSetting.ImageEnter = global::ORG.UIL.Desktop.Properties.Resources._02Search;
            this.picSetting.ImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picSetting.Name = "picSetting";
            this.picSetting.WithSplitter = false;
            this.picSetting.WithTitle = false;
            this.picSetting.Click += new System.EventHandler(this.SearchRow);
            // 
            // panel4
            // 
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // pnlControl2
            // 
            this.pnlControl2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlControl2.Controls.Add(this.bttnCancel);
            this.pnlControl2.Controls.Add(this.bttnDelete);
            this.pnlControl2.Controls.Add(this.bttnSave);
            this.pnlControl2.Controls.Add(this.bttnSaveNew);
            resources.ApplyResources(this.pnlControl2, "pnlControl2");
            this.pnlControl2.Name = "pnlControl2";
            // 
            // bttnCancel
            // 
            this.bttnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnCancel.Color = System.Drawing.Color.WhiteSmoke;
            this.bttnCancel.ColorEnter = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            resources.ApplyResources(this.bttnCancel, "bttnCancel");
            this.bttnCancel.FontColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnCancel.FontColorEnter = System.Drawing.Color.White;
            this.bttnCancel.Image = global::ORG.UIL.Desktop.Properties.Resources._02Cancelb;
            this.bttnCancel.ImageEnter = global::ORG.UIL.Desktop.Properties.Resources._02Cancel;
            this.bttnCancel.ImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bttnCancel.Name = "bttnCancel";
            this.bttnCancel.WithSplitter = false;
            this.bttnCancel.WithTitle = true;
            this.bttnCancel.Click += new System.EventHandler(this.CancelRow);
            // 
            // bttnDelete
            // 
            this.bttnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnDelete.Color = System.Drawing.Color.WhiteSmoke;
            this.bttnDelete.ColorEnter = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            resources.ApplyResources(this.bttnDelete, "bttnDelete");
            this.bttnDelete.FontColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnDelete.FontColorEnter = System.Drawing.Color.White;
            this.bttnDelete.Image = global::ORG.UIL.Desktop.Properties.Resources._02Deleteb;
            this.bttnDelete.ImageEnter = global::ORG.UIL.Desktop.Properties.Resources._02Delete;
            this.bttnDelete.ImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bttnDelete.Name = "bttnDelete";
            this.bttnDelete.WithSplitter = true;
            this.bttnDelete.WithTitle = true;
            this.bttnDelete.Click += new System.EventHandler(this.DeleteRow);
            // 
            // bttnSave
            // 
            this.bttnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnSave.Color = System.Drawing.Color.WhiteSmoke;
            this.bttnSave.ColorEnter = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            resources.ApplyResources(this.bttnSave, "bttnSave");
            this.bttnSave.FontColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnSave.FontColorEnter = System.Drawing.Color.White;
            this.bttnSave.Image = global::ORG.UIL.Desktop.Properties.Resources._02Saveb;
            this.bttnSave.ImageEnter = global::ORG.UIL.Desktop.Properties.Resources._02Save;
            this.bttnSave.ImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bttnSave.Name = "bttnSave";
            this.bttnSave.WithSplitter = true;
            this.bttnSave.WithTitle = true;
            this.bttnSave.Click += new System.EventHandler(this.SaveRow);
            // 
            // bttnSaveNew
            // 
            this.bttnSaveNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnSaveNew.Color = System.Drawing.Color.WhiteSmoke;
            this.bttnSaveNew.ColorEnter = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            resources.ApplyResources(this.bttnSaveNew, "bttnSaveNew");
            this.bttnSaveNew.FontColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.bttnSaveNew.FontColorEnter = System.Drawing.Color.White;
            this.bttnSaveNew.Image = global::ORG.UIL.Desktop.Properties.Resources.SaveNew22b;
            this.bttnSaveNew.ImageEnter = global::ORG.UIL.Desktop.Properties.Resources.SaveNew22;
            this.bttnSaveNew.ImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bttnSaveNew.Name = "bttnSaveNew";
            this.bttnSaveNew.WithSplitter = true;
            this.bttnSaveNew.WithTitle = true;
            this.bttnSaveNew.Click += new System.EventHandler(this.SaveNewRow);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // frm_TemplateData
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlBorder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_TemplateData";
            this.Load += new System.EventHandler(this.LoadForm);
            this.pnlBorder.ResumeLayout(false);
            this.pnlMainData.ResumeLayout(false);
            this.pnlMainData.PerformLayout();
            this.pnlDisplay.ResumeLayout(false);
            this.pnlLoadData.ResumeLayout(false);
            this.pnlLoadGroup.ResumeLayout(false);
            this.pnlControls.ResumeLayout(false);
            this.pnlControl1.ResumeLayout(false);
            this.pnlControl1.PerformLayout();
            this.pnlControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel pnlBorder;
        public System.Windows.Forms.Panel pnlControl2;
        public System.Windows.Forms.Panel pnlControl1;
        public System.Windows.Forms.TextBox txtSearch;
        public System.Windows.Forms.Panel pnlMainData;
        public System.Windows.Forms.Panel pnlLoadData;
        public System.Windows.Forms.Panel pnlLoadGroup;
        public Tools.Desktop.Tools.PanelUnitTool panelUnitTool1;
        public System.Windows.Forms.Panel pnlInfo;
        public System.Windows.Forms.Panel pnlDisplay;
        public System.Windows.Forms.Panel pnlControls;
        public Tools.Desktop.PicImageTool picImage;
        public System.Windows.Forms.Label lblmyGroup;
        public System.Windows.Forms.ComboBox txtmyGroup;
        public System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        public System.Windows.Forms.Label lblKeyInfo;
        private System.Windows.Forms.Panel panel4;
        public Tools.Desktop.Tools.BottunOrg bttnCancel;
        public Tools.Desktop.Tools.BottunOrg bttnDelete;
        public Tools.Desktop.Tools.BottunOrg bttnSave;
        public Tools.Desktop.Tools.BottunOrg bttnSaveNew;
        public Tools.Desktop.Tools.BottunOrg bttnNew;
        public Tools.Desktop.Tools.BottunOrg bttnRefrach;
        public Tools.Desktop.Tools.BottunOrg picSetting;
        private Tools.Desktop.Tools.BottunOrg bottunOrg5;
        private Tools.Desktop.Tools.BottunOrg bottunOrg4;
        public Tools.Desktop.Tools.PanelGroupTool panelGroupTool1;
    }
}