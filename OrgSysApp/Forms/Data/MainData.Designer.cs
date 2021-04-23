
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
            this.btnSetting = new OrgSysApp.Tools.ButtonImage();
            this.btnUser = new OrgSysApp.Tools.ButtonImage();
            this.btnProduct = new OrgSysApp.Tools.ButtonImage();
            this.btnCategory = new OrgSysApp.Tools.ButtonImage();
            this.btnUnit = new OrgSysApp.Tools.ButtonImage();
            this.pnlDataPages = new System.Windows.Forms.Panel();
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
            this.tblMainData.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tblMainData.ColumnCount = 2;
            this.tblMainData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tblMainData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 85.71429F));
            this.tblMainData.Controls.Add(this.pnlMainDataMenu, 0, 1);
            this.tblMainData.Controls.Add(this.pnlDataPages, 1, 1);
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
            this.pnlMainDataMenu.Controls.Add(this.btnSetting);
            this.pnlMainDataMenu.Controls.Add(this.btnUser);
            this.pnlMainDataMenu.Controls.Add(this.btnProduct);
            this.pnlMainDataMenu.Controls.Add(this.btnCategory);
            this.pnlMainDataMenu.Controls.Add(this.btnUnit);
            this.pnlMainDataMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainDataMenu.Location = new System.Drawing.Point(10, 13);
            this.pnlMainDataMenu.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.pnlMainDataMenu.Name = "pnlMainDataMenu";
            this.pnlMainDataMenu.Size = new System.Drawing.Size(104, 419);
            this.pnlMainDataMenu.TabIndex = 0;
            // 
            // btnSetting
            // 
            this.btnSetting.active = false;
            this.btnSetting.activeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.btnSetting.BackColor = System.Drawing.Color.Transparent;
            this.btnSetting.ButtonName = "Setting";
            this.btnSetting.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSetting.font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSetting.fontColor = System.Drawing.Color.Gray;
            this.btnSetting.fontColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.btnSetting.image = global::OrgSysApp.Properties.Resources.output_ico;
            this.btnSetting.imageActive = global::OrgSysApp.Properties.Resources.outpu;
            this.btnSetting.layOut = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSetting.Location = new System.Drawing.Point(0, 320);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(104, 80);
            this.btnSetting.TabIndex = 5;
            this.btnSetting.Title = "Setting";
            this.btnSetting.click += new System.EventHandler(this.btn_click);
            // 
            // btnUser
            // 
            this.btnUser.active = false;
            this.btnUser.activeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.btnUser.BackColor = System.Drawing.Color.Transparent;
            this.btnUser.ButtonName = "User";
            this.btnUser.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUser.font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnUser.fontColor = System.Drawing.Color.Gray;
            this.btnUser.fontColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.btnUser.image = global::OrgSysApp.Properties.Resources.Users__1_;
            this.btnUser.imageActive = global::OrgSysApp.Properties.Resources.Users;
            this.btnUser.layOut = System.Windows.Forms.ImageLayout.Zoom;
            this.btnUser.Location = new System.Drawing.Point(0, 240);
            this.btnUser.Name = "btnUser";
            this.btnUser.Size = new System.Drawing.Size(104, 80);
            this.btnUser.TabIndex = 4;
            this.btnUser.Title = "Users";
            this.btnUser.click += new System.EventHandler(this.btn_click);
            // 
            // btnProduct
            // 
            this.btnProduct.active = false;
            this.btnProduct.activeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.btnProduct.BackColor = System.Drawing.Color.Transparent;
            this.btnProduct.ButtonName = "Product";
            this.btnProduct.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnProduct.font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnProduct.fontColor = System.Drawing.Color.Gray;
            this.btnProduct.fontColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.btnProduct.image = global::OrgSysApp.Properties.Resources.item114a;
            this.btnProduct.imageActive = global::OrgSysApp.Properties.Resources.item1141;
            this.btnProduct.layOut = System.Windows.Forms.ImageLayout.Zoom;
            this.btnProduct.Location = new System.Drawing.Point(0, 160);
            this.btnProduct.Name = "btnProduct";
            this.btnProduct.Size = new System.Drawing.Size(104, 80);
            this.btnProduct.TabIndex = 3;
            this.btnProduct.Title = "Products";
            this.btnProduct.click += new System.EventHandler(this.btn_click);
            // 
            // btnCategory
            // 
            this.btnCategory.active = false;
            this.btnCategory.activeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.btnCategory.BackColor = System.Drawing.Color.Transparent;
            this.btnCategory.ButtonName = "Category";
            this.btnCategory.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCategory.font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCategory.fontColor = System.Drawing.Color.Gray;
            this.btnCategory.fontColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.btnCategory.image = global::OrgSysApp.Properties.Resources.itemgroup3;
            this.btnCategory.imageActive = global::OrgSysApp.Properties.Resources.itemgroup3b;
            this.btnCategory.layOut = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCategory.Location = new System.Drawing.Point(0, 80);
            this.btnCategory.Name = "btnCategory";
            this.btnCategory.Size = new System.Drawing.Size(104, 80);
            this.btnCategory.TabIndex = 2;
            this.btnCategory.Title = "Categories";
            this.btnCategory.click += new System.EventHandler(this.btn_click);
            // 
            // btnUnit
            // 
            this.btnUnit.active = false;
            this.btnUnit.activeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.btnUnit.BackColor = System.Drawing.Color.Transparent;
            this.btnUnit.ButtonName = "Unit";
            this.btnUnit.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUnit.font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnUnit.fontColor = System.Drawing.Color.Gray;
            this.btnUnit.fontColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(250)))));
            this.btnUnit.image = global::OrgSysApp.Properties.Resources.unit;
            this.btnUnit.imageActive = global::OrgSysApp.Properties.Resources.unitb;
            this.btnUnit.layOut = System.Windows.Forms.ImageLayout.Zoom;
            this.btnUnit.Location = new System.Drawing.Point(0, 0);
            this.btnUnit.Name = "btnUnit";
            this.btnUnit.Size = new System.Drawing.Size(104, 80);
            this.btnUnit.TabIndex = 1;
            this.btnUnit.Title = "Units";
            this.btnUnit.click += new System.EventHandler(this.btn_click);
            // 
            // pnlDataPages
            // 
            this.pnlDataPages.BackColor = System.Drawing.Color.White;
            this.pnlDataPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDataPages.Location = new System.Drawing.Point(114, 13);
            this.pnlDataPages.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.pnlDataPages.Name = "pnlDataPages";
            this.pnlDataPages.Size = new System.Drawing.Size(681, 419);
            this.pnlDataPages.TabIndex = 1;
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
        private Tools.ButtonImage btnUnit;
        private System.Windows.Forms.Panel pnlDataPages;
        private Tools.ButtonImage btnCategory;
        private Tools.ButtonImage btnSetting;
        private Tools.ButtonImage btnUser;
        private Tools.ButtonImage btnProduct;
    }
}