namespace AppManage
{
    partial class GengDuoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GengDuoForm));
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoYym = new System.Windows.Forms.RadioButton();
            this.rdoYxcs = new System.Windows.Forms.RadioButton();
            this.rdoTjsx = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkLabel3.LinkColor = System.Drawing.Color.DimGray;
            this.linkLabel3.Location = new System.Drawing.Point(204, 268);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(53, 12);
            this.linkLabel3.TabIndex = 2;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "关于软件";
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoYym);
            this.groupBox1.Controls.Add(this.rdoYxcs);
            this.groupBox1.Controls.Add(this.rdoTjsx);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.groupBox1.Location = new System.Drawing.Point(24, 43);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(229, 62);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "排序规则";
            // 
            // rdoYym
            // 
            this.rdoYym.AutoSize = true;
            this.rdoYym.ForeColor = System.Drawing.Color.Black;
            this.rdoYym.Location = new System.Drawing.Point(164, 24);
            this.rdoYym.Name = "rdoYym";
            this.rdoYym.Size = new System.Drawing.Size(59, 16);
            this.rdoYym.TabIndex = 0;
            this.rdoYym.Text = "应用名";
            this.rdoYym.UseVisualStyleBackColor = true;
            this.rdoYym.Click += new System.EventHandler(this.rdo_Click);
            // 
            // rdoYxcs
            // 
            this.rdoYxcs.AutoSize = true;
            this.rdoYxcs.ForeColor = System.Drawing.Color.Black;
            this.rdoYxcs.Location = new System.Drawing.Point(84, 24);
            this.rdoYxcs.Name = "rdoYxcs";
            this.rdoYxcs.Size = new System.Drawing.Size(71, 16);
            this.rdoYxcs.TabIndex = 0;
            this.rdoYxcs.Text = "运行次数";
            this.rdoYxcs.UseVisualStyleBackColor = true;
            this.rdoYxcs.Click += new System.EventHandler(this.rdo_Click);
            // 
            // rdoTjsx
            // 
            this.rdoTjsx.AutoSize = true;
            this.rdoTjsx.Checked = true;
            this.rdoTjsx.ForeColor = System.Drawing.Color.Black;
            this.rdoTjsx.Location = new System.Drawing.Point(7, 24);
            this.rdoTjsx.Name = "rdoTjsx";
            this.rdoTjsx.Size = new System.Drawing.Size(71, 16);
            this.rdoTjsx.TabIndex = 0;
            this.rdoTjsx.TabStop = true;
            this.rdoTjsx.Text = "添加顺序";
            this.rdoTjsx.UseVisualStyleBackColor = true;
            this.rdoTjsx.Click += new System.EventHandler(this.rdo_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(60, 201);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(157, 35);
            this.button1.TabIndex = 4;
            this.button1.Text = "应用软件生成快捷方式";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(60, 134);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(157, 36);
            this.button2.TabIndex = 4;
            this.button2.Text = "修改软件启动rousev密令";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // GengDuoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(269, 284);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.linkLabel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GengDuoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "更多";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GengDuoForm_FormClosing);
            this.Load += new System.EventHandler(this.GengDuoForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoYym;
        private System.Windows.Forms.RadioButton rdoYxcs;
        private System.Windows.Forms.RadioButton rdoTjsx;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}