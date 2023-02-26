namespace ORG.UIL.Desktop
{
    partial class frm_ItemGroup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_ItemGroup));
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pnlBorder.SuspendLayout();
            this.pnlControl2.SuspendLayout();
            this.pnlControl1.SuspendLayout();
            this.pnlMainData.SuspendLayout();
            this.pnlLoadData.SuspendLayout();
            this.pnlLoadGroup.SuspendLayout();
            this.pnlDisplay.SuspendLayout();
            this.pnlControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMainData
            // 
            this.pnlMainData.Controls.Add(this.txtName);
            this.pnlMainData.Controls.Add(this.lblName);
            resources.ApplyResources(this.pnlMainData, "pnlMainData");
            this.pnlMainData.Controls.SetChildIndex(this.picImage, 0);
            this.pnlMainData.Controls.SetChildIndex(this.lblName, 0);
            this.pnlMainData.Controls.SetChildIndex(this.txtName, 0);
            this.pnlMainData.Controls.SetChildIndex(this.txtmyGroup, 0);
            this.pnlMainData.Controls.SetChildIndex(this.lblmyGroup, 0);
            // 
            // pnlLoadData
            // 
            resources.ApplyResources(this.pnlLoadData, "pnlLoadData");
            // 
            // panelUnitTool1
            // 
            this.panelUnitTool1.FontDescription = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelUnitTool1.FontTitle = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelUnitTool1.HeightItem = 160;
            resources.ApplyResources(this.panelUnitTool1, "panelUnitTool1");
            this.panelUnitTool1.WidthItem = 100;
            this.panelUnitTool1.pagging += new ORG.Tools.Desktop._Pagging(this.panelUnitTool1_pagging);
            // 
            // pnlInfo
            // 
            resources.ApplyResources(this.pnlInfo, "pnlInfo");
            // 
            // pnlDisplay
            // 
            resources.ApplyResources(this.pnlDisplay, "pnlDisplay");
            // 
            // picImage
            // 
            resources.ApplyResources(this.picImage, "picImage");
            // 
            // lblKeyInfo
            // 
            resources.ApplyResources(this.lblKeyInfo, "lblKeyInfo");
            // 
            // bttnRefrach
            // 
            resources.ApplyResources(this.bttnRefrach, "bttnRefrach");
            // 
            // txtName
            // 
            resources.ApplyResources(this.txtName, "txtName");
            this.txtName.ForeColor = System.Drawing.Color.DimGray;
            this.txtName.Name = "txtName";
            // 
            // lblName
            // 
            resources.ApplyResources(this.lblName, "lblName");
            this.lblName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.lblName.Name = "lblName";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frm_ItemGroup
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ByGroup = false;
            this.ByImage = true;
            this.Name = "frm_ItemGroup";
            this.OpenClose = true;
            this.UsedImportFile = true;
            this.pnlBorder.ResumeLayout(false);
            this.pnlControl2.ResumeLayout(false);
            this.pnlControl1.ResumeLayout(false);
            this.pnlControl1.PerformLayout();
            this.pnlMainData.ResumeLayout(false);
            this.pnlMainData.PerformLayout();
            this.pnlLoadData.ResumeLayout(false);
            this.pnlLoadGroup.ResumeLayout(false);
            this.pnlDisplay.ResumeLayout(false);
            this.pnlControls.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Timer timer1;
    }
}