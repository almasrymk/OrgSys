
namespace OrgSysApp.Tools
{
    partial class IndexPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tblIndexGride = new System.Windows.Forms.TableLayoutPanel();
            this.flpIndexPanelBody = new System.Windows.Forms.FlowLayoutPanel();
            this.flpIndexPanelHeader = new System.Windows.Forms.Panel();
            this.btnBarMode = new System.Windows.Forms.Label();
            this.btnElementModel = new System.Windows.Forms.Label();
            this.btnGridMode = new System.Windows.Forms.Label();
            this.flpIndexPanelFooter = new System.Windows.Forms.Panel();
            this.tblIndexGride.SuspendLayout();
            this.flpIndexPanelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // tblIndexGride
            // 
            this.tblIndexGride.ColumnCount = 1;
            this.tblIndexGride.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblIndexGride.Controls.Add(this.flpIndexPanelBody, 0, 1);
            this.tblIndexGride.Controls.Add(this.flpIndexPanelHeader, 0, 0);
            this.tblIndexGride.Controls.Add(this.flpIndexPanelFooter, 0, 2);
            this.tblIndexGride.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblIndexGride.Location = new System.Drawing.Point(0, 0);
            this.tblIndexGride.Name = "tblIndexGride";
            this.tblIndexGride.RowCount = 3;
            this.tblIndexGride.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tblIndexGride.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tblIndexGride.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tblIndexGride.Size = new System.Drawing.Size(300, 300);
            this.tblIndexGride.TabIndex = 0;
            // 
            // flpIndexPanelBody
            // 
            this.flpIndexPanelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpIndexPanelBody.Location = new System.Drawing.Point(3, 33);
            this.flpIndexPanelBody.Name = "flpIndexPanelBody";
            this.flpIndexPanelBody.Size = new System.Drawing.Size(294, 234);
            this.flpIndexPanelBody.TabIndex = 0;
            // 
            // flpIndexPanelHeader
            // 
            this.flpIndexPanelHeader.Controls.Add(this.btnBarMode);
            this.flpIndexPanelHeader.Controls.Add(this.btnElementModel);
            this.flpIndexPanelHeader.Controls.Add(this.btnGridMode);
            this.flpIndexPanelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpIndexPanelHeader.Location = new System.Drawing.Point(3, 3);
            this.flpIndexPanelHeader.Name = "flpIndexPanelHeader";
            this.flpIndexPanelHeader.Size = new System.Drawing.Size(294, 24);
            this.flpIndexPanelHeader.TabIndex = 1;
            // 
            // btnBarMode
            // 
            this.btnBarMode.AutoSize = true;
            this.btnBarMode.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnBarMode.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnBarMode.ForeColor = System.Drawing.Color.DimGray;
            this.btnBarMode.Location = new System.Drawing.Point(48, 0);
            this.btnBarMode.Name = "btnBarMode";
            this.btnBarMode.Size = new System.Drawing.Size(24, 26);
            this.btnBarMode.TabIndex = 4;
            this.btnBarMode.Text = "X";
            this.btnBarMode.Click += new System.EventHandler(this.BarMode_Click);
            // 
            // btnElementModel
            // 
            this.btnElementModel.AutoSize = true;
            this.btnElementModel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnElementModel.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnElementModel.ForeColor = System.Drawing.Color.DimGray;
            this.btnElementModel.Location = new System.Drawing.Point(24, 0);
            this.btnElementModel.Name = "btnElementModel";
            this.btnElementModel.Size = new System.Drawing.Size(24, 26);
            this.btnElementModel.TabIndex = 3;
            this.btnElementModel.Text = "X";
            this.btnElementModel.Click += new System.EventHandler(this.ElementModel_Click);
            // 
            // btnGridMode
            // 
            this.btnGridMode.AutoSize = true;
            this.btnGridMode.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnGridMode.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnGridMode.ForeColor = System.Drawing.Color.DimGray;
            this.btnGridMode.Location = new System.Drawing.Point(0, 0);
            this.btnGridMode.Name = "btnGridMode";
            this.btnGridMode.Size = new System.Drawing.Size(24, 26);
            this.btnGridMode.TabIndex = 2;
            this.btnGridMode.Text = "X";
            this.btnGridMode.Click += new System.EventHandler(this.GridMode_Click);
            // 
            // flpIndexPanelFooter
            // 
            this.flpIndexPanelFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpIndexPanelFooter.Location = new System.Drawing.Point(3, 273);
            this.flpIndexPanelFooter.Name = "flpIndexPanelFooter";
            this.flpIndexPanelFooter.Size = new System.Drawing.Size(294, 24);
            this.flpIndexPanelFooter.TabIndex = 2;
            // 
            // IndexPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tblIndexGride);
            this.Name = "IndexPanel";
            this.Size = new System.Drawing.Size(300, 300);
            this.tblIndexGride.ResumeLayout(false);
            this.flpIndexPanelHeader.ResumeLayout(false);
            this.flpIndexPanelHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblIndexGride;
        private System.Windows.Forms.FlowLayoutPanel flpIndexPanelBody;
        private System.Windows.Forms.Panel flpIndexPanelHeader;
        private System.Windows.Forms.Panel flpIndexPanelFooter;
        private System.Windows.Forms.Label btnBarMode;
        private System.Windows.Forms.Label btnElementModel;
        private System.Windows.Forms.Label btnGridMode;
    }
}
