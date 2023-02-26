namespace Farm.Tools
{
    partial class txtBox
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
            this.txt1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.btn1 = new Farm.Tools.btn();
            this.SuspendLayout();
            // 
            // txt1
            // 
            this.txt1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt1.Font = new System.Drawing.Font("Arial", 9.75F);
            this.txt1.ForeColor = System.Drawing.Color.DimGray;
            this.txt1.Location = new System.Drawing.Point(37, 0);
            this.txt1.Multiline = true;
            this.txt1.Name = "txt1";
            this.txt1.Size = new System.Drawing.Size(73, 25);
            this.txt1.TabIndex = 1;
            this.txt1.TextChanged += new System.EventHandler(this.txtBox_TextChanged);
            this.txt1.Enter += new System.EventHandler(this.txt1_Enter);
            this.txt1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt1_KeyDown);
            this.txt1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBox_KeyPress);
            this.txt1.Leave += new System.EventHandler(this.txtBox_Leave);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(135, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "*";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Visible = false;
            // 
            // txtCode
            // 
            this.txtCode.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtCode.Font = new System.Drawing.Font("Arial", 9.75F);
            this.txtCode.ForeColor = System.Drawing.Color.DimGray;
            this.txtCode.Location = new System.Drawing.Point(0, 0);
            this.txtCode.Multiline = true;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(37, 25);
            this.txtCode.TabIndex = 0;
            this.txtCode.TextChanged += new System.EventHandler(this.txtCode_TextChanged);
            this.txtCode.Enter += new System.EventHandler(this.txt1_Enter);
            this.txtCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCode_KeyPress);
            this.txtCode.Leave += new System.EventHandler(this.txtCode_Leave);
            // 
            // btn1
            // 
            this.btn1.BackColor = System.Drawing.Color.Transparent;
            this.btn1.BackgroundImage = global::ORG.Tools.Desktop.Properties.Resources._02Searchb;
            this.btn1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn1.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn1.ForeColor = System.Drawing.Color.White;
            this.btn1.ForeColorTitle = System.Drawing.Color.Black;
            this.btn1.Location = new System.Drawing.Point(110, 0);
            this.btn1.MaximumSize = new System.Drawing.Size(25, 25);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(25, 25);
            this.btn1.TabIndex = 2;
            this.btn1.TabStop = false;
            this.btn1.Title = "Title";
            this.btn1.Clicked += new System.EventHandler(this.btn1_Clicked);
            // 
            // txtBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.txt1);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.btn1);
            this.Controls.Add(this.label1);
            this.Name = "txtBox";
            this.Size = new System.Drawing.Size(150, 25);
            this.RightToLeftChanged += new System.EventHandler(this.txtBox_RightToLeftChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox txt1;
        private System.Windows.Forms.Label label1;
        private btn btn1;
        public System.Windows.Forms.TextBox txtCode;
    }
}
