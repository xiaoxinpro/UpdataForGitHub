namespace UpdataApp
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnUpdata = new System.Windows.Forms.Button();
            this.btnReflash = new System.Windows.Forms.Button();
            this.proUpdata = new System.Windows.Forms.ProgressBar();
            this.txtUpdataInfo = new System.Windows.Forms.TextBox();
            this.labelInfo = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelInfo);
            this.groupBox1.Controls.Add(this.txtUpdataInfo);
            this.groupBox1.Controls.Add(this.proUpdata);
            this.groupBox1.Controls.Add(this.btnUpdata);
            this.groupBox1.Controls.Add(this.btnReflash);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(342, 426);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "软件升级";
            // 
            // btnUpdata
            // 
            this.btnUpdata.Enabled = false;
            this.btnUpdata.Location = new System.Drawing.Point(261, 20);
            this.btnUpdata.Name = "btnUpdata";
            this.btnUpdata.Size = new System.Drawing.Size(75, 23);
            this.btnUpdata.TabIndex = 3;
            this.btnUpdata.Text = "更新";
            this.btnUpdata.UseVisualStyleBackColor = true;
            this.btnUpdata.Click += new System.EventHandler(this.btnUpdata_Click);
            // 
            // btnReflash
            // 
            this.btnReflash.Location = new System.Drawing.Point(6, 20);
            this.btnReflash.Name = "btnReflash";
            this.btnReflash.Size = new System.Drawing.Size(75, 23);
            this.btnReflash.TabIndex = 2;
            this.btnReflash.Text = "刷新";
            this.btnReflash.UseVisualStyleBackColor = true;
            this.btnReflash.Click += new System.EventHandler(this.btnReflash_Click);
            // 
            // proUpdata
            // 
            this.proUpdata.Location = new System.Drawing.Point(6, 49);
            this.proUpdata.Name = "proUpdata";
            this.proUpdata.Size = new System.Drawing.Size(330, 23);
            this.proUpdata.TabIndex = 4;
            // 
            // txtUpdataInfo
            // 
            this.txtUpdataInfo.Location = new System.Drawing.Point(7, 79);
            this.txtUpdataInfo.Multiline = true;
            this.txtUpdataInfo.Name = "txtUpdataInfo";
            this.txtUpdataInfo.Size = new System.Drawing.Size(329, 341);
            this.txtUpdataInfo.TabIndex = 5;
            // 
            // labelInfo
            // 
            this.labelInfo.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelInfo.Location = new System.Drawing.Point(87, 20);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(168, 23);
            this.labelInfo.TabIndex = 6;
            this.labelInfo.Text = "待获取更新";
            this.labelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 450);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "软件升级工具";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtUpdataInfo;
        private System.Windows.Forms.ProgressBar proUpdata;
        private System.Windows.Forms.Button btnUpdata;
        private System.Windows.Forms.Button btnReflash;
        private System.Windows.Forms.Label labelInfo;
    }
}

