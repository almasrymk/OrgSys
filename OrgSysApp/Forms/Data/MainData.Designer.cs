
namespace OrgSysApp.Forms.Data
{
    partial class MainData
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
            this.pnl1 = new System.Windows.Forms.Panel();
            this.pnl2 = new System.Windows.Forms.Panel();
            this.tblMainData = new System.Windows.Forms.TableLayoutPanel();
            this.pnlMainDataMenu = new System.Windows.Forms.Panel();
            this.btnData = new OrgSysApp.Tools.ButtonImage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tblMainData.SuspendLayout();
            this.pnlMainDataMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnl1
            // 
            this.pnl1.BackColor = System.Drawing.Color.Silver;
            this.pnl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl1.Location = new System.Drawing.Point(0, 0);
            this.pnl1.Name = "pnl1";
            this.pnl1.Size = new System.Drawing.Size(800, 2);
            this.pnl1.TabIndex = 0;
            // 
            // pnl2
            // 
            this.pnl2.BackColor = System.Drawing.Color.Silver;
            this.pnl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnl2.Location = new System.Drawing.Point(0, 448);
            this.pnl2.Name = "pnl2";
            this.pnl2.Size = new System.Drawing.Size(800, 2);
            this.pnl2.TabIndex = 1;
            // 
            // tblMainData
            // 
            this.tblMainData.BackColor = System.Drawing.Color.White;
            this.tblMainData.ColumnCount = 2;
            this.tblMainData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tblMainData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tblMainData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblMainData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblMainData.Controls.Add(this.pnlMainDataMenu, 0, 1);
            this.tblMainData.Controls.Add(this.panel1, 1, 1);
            this.tblMainData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMainData.Location = new System.Drawing.Point(0, 2);
            this.tblMainData.Name = "tblMainData";
            this.tblMainData.RowCount = 3;
            this.tblMainData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3F));
            this.tblMainData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94F));
            this.tblMainData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3F));
            this.tblMainData.Size = new System.Drawing.Size(800, 446);
            this.tblMainData.TabIndex = 2;
            // 
            // pnlMainDataMenu
            // 
            this.pnlMainDataMenu.AutoScroll = true;
            this.pnlMainDataMenu.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlMainDataMenu.Controls.Add(this.btnData);
            this.pnlMainDataMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainDataMenu.Location = new System.Drawing.Point(10, 13);
            this.pnlMainDataMenu.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.pnlMainDataMenu.Name = "pnlMainDataMenu";
            this.pnlMainDataMenu.Size = new System.Drawing.Size(70, 419);
            this.pnlMainDataMenu.TabIndex = 0;
            // 
            // btnData
            // 
            this.btnData.active = false;
            this.btnData.activeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.btnData.BackColor = System.Drawing.Color.Transparent;
            this.btnData.ButtonName = "Data";
            this.btnData.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnData.font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnData.fontColor = System.Drawing.Color.Gray;
            this.btnData.fontColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.btnData.image = global::OrgSysApp.Properties.Resources.Data1;
            this.btnData.imageActive = global::OrgSysApp.Properties.Resources.Data;
            this.btnData.layOut = System.Windows.Forms.ImageLayout.Zoom;
            this.btnData.Location = new System.Drawing.Point(0, 0);
            this.btnData.Name = "btnData";
            this.btnData.Size = new System.Drawing.Size(70, 78);
            this.btnData.TabIndex = 1;
            this.btnData.Title = "Data";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(80, 13);
            this.panel1.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(715, 419);
            this.panel1.TabIndex = 1;
            // 
            // MainData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tblMainData);
            this.Controls.Add(this.pnl2);
            this.Controls.Add(this.pnl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainData";
            this.Text = "MainData";
            this.tblMainData.ResumeLayout(false);
            this.pnlMainDataMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl1;
        private System.Windows.Forms.Panel pnl2;
        private System.Windows.Forms.TableLayoutPanel tblMainData;
        private System.Windows.Forms.Panel pnlMainDataMenu;
        private Tools.ButtonImage btnData;
        private System.Windows.Forms.Panel panel1;
    }
}