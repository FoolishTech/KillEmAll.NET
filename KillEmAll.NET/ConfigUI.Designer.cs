
namespace KillEmAll.NET
{
    partial class ConfigUI
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkStartupAutoKill = new System.Windows.Forms.CheckBox();
            this.chkStartupRunAsAdmin = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboSearchEngineName = new System.Windows.Forms.ComboBox();
            this.txtSearchEngineURL = new System.Windows.Forms.TextBox();
            this.chkSearchFileNameOnly = new System.Windows.Forms.CheckBox();
            this.cmdSaveExit = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkDebugAlwaysShowInfo = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtVirusTotalAPIKey = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkStartupAutoKill);
            this.groupBox1.Controls.Add(this.chkStartupRunAsAdmin);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(19, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(382, 69);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Startup Behavior";
            // 
            // chkStartupAutoKill
            // 
            this.chkStartupAutoKill.AutoSize = true;
            this.chkStartupAutoKill.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkStartupAutoKill.Location = new System.Drawing.Point(18, 42);
            this.chkStartupAutoKill.Name = "chkStartupAutoKill";
            this.chkStartupAutoKill.Size = new System.Drawing.Size(276, 17);
            this.chkStartupAutoKill.TabIndex = 1;
            this.chkStartupAutoKill.Text = "Always terminate programs immediately before prompt";
            this.chkStartupAutoKill.UseVisualStyleBackColor = true;
            // 
            // chkStartupRunAsAdmin
            // 
            this.chkStartupRunAsAdmin.AutoSize = true;
            this.chkStartupRunAsAdmin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkStartupRunAsAdmin.Location = new System.Drawing.Point(19, 19);
            this.chkStartupRunAsAdmin.Name = "chkStartupRunAsAdmin";
            this.chkStartupRunAsAdmin.Size = new System.Drawing.Size(186, 17);
            this.chkStartupRunAsAdmin.TabIndex = 0;
            this.chkStartupRunAsAdmin.Text = "Always force Run as Administrator";
            this.chkStartupRunAsAdmin.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.comboSearchEngineName);
            this.groupBox2.Controls.Add(this.txtSearchEngineURL);
            this.groupBox2.Controls.Add(this.chkSearchFileNameOnly);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(19, 157);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(382, 111);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Search Configuration";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(15, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(359, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Enter a Search URL below or select a pre-defined URL from the list above.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(202, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Pre-defined Search Engine URLs";
            // 
            // comboSearchEngineName
            // 
            this.comboSearchEngineName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSearchEngineName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboSearchEngineName.FormattingEnabled = true;
            this.comboSearchEngineName.Location = new System.Drawing.Point(202, 35);
            this.comboSearchEngineName.Name = "comboSearchEngineName";
            this.comboSearchEngineName.Size = new System.Drawing.Size(164, 21);
            this.comboSearchEngineName.TabIndex = 3;
            this.comboSearchEngineName.SelectedIndexChanged += new System.EventHandler(this.comboSearchEngineName_SelectedIndexChanged);
            // 
            // txtSearchEngineURL
            // 
            this.txtSearchEngineURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchEngineURL.Location = new System.Drawing.Point(18, 76);
            this.txtSearchEngineURL.Name = "txtSearchEngineURL";
            this.txtSearchEngineURL.Size = new System.Drawing.Size(348, 20);
            this.txtSearchEngineURL.TabIndex = 2;
            this.txtSearchEngineURL.TextChanged += new System.EventHandler(this.txtSearchEngineURL_TextChanged);
            // 
            // chkSearchFileNameOnly
            // 
            this.chkSearchFileNameOnly.AutoSize = true;
            this.chkSearchFileNameOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSearchFileNameOnly.Location = new System.Drawing.Point(18, 19);
            this.chkSearchFileNameOnly.Name = "chkSearchFileNameOnly";
            this.chkSearchFileNameOnly.Size = new System.Drawing.Size(166, 17);
            this.chkSearchFileNameOnly.TabIndex = 1;
            this.chkSearchFileNameOnly.Text = "Always search File Name only";
            this.chkSearchFileNameOnly.UseVisualStyleBackColor = true;
            // 
            // cmdSaveExit
            // 
            this.cmdSaveExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSaveExit.Location = new System.Drawing.Point(269, 370);
            this.cmdSaveExit.Name = "cmdSaveExit";
            this.cmdSaveExit.Size = new System.Drawing.Size(124, 30);
            this.cmdSaveExit.TabIndex = 2;
            this.cmdSaveExit.Text = "Save && Exit";
            this.cmdSaveExit.UseVisualStyleBackColor = true;
            this.cmdSaveExit.Click += new System.EventHandler(this.cmdSaveExit_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkDebugAlwaysShowInfo);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(19, 96);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(382, 46);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Debug Mode Behavior";
            // 
            // chkDebugAlwaysShowInfo
            // 
            this.chkDebugAlwaysShowInfo.AutoSize = true;
            this.chkDebugAlwaysShowInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDebugAlwaysShowInfo.Location = new System.Drawing.Point(19, 19);
            this.chkDebugAlwaysShowInfo.Name = "chkDebugAlwaysShowInfo";
            this.chkDebugAlwaysShowInfo.Size = new System.Drawing.Size(323, 17);
            this.chkDebugAlwaysShowInfo.TabIndex = 0;
            this.chkDebugAlwaysShowInfo.Text = "Always show extended file information (don\'t require pressing \'I\')";
            this.chkDebugAlwaysShowInfo.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.txtVirusTotalAPIKey);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(19, 283);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(382, 74);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "VirusTotal API Key";
            // 
            // txtVirusTotalAPIKey
            // 
            this.txtVirusTotalAPIKey.Location = new System.Drawing.Point(18, 44);
            this.txtVirusTotalAPIKey.Name = "txtVirusTotalAPIKey";
            this.txtVirusTotalAPIKey.Size = new System.Drawing.Size(348, 20);
            this.txtVirusTotalAPIKey.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(15, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(330, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "This enables the Debug mode option to Press \'V\' to query VirusTotal.";
            // 
            // ConfigUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 412);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.cmdSaveExit);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "KillEmAll.NET Configuration";
            this.Load += new System.EventHandler(this.ConfigUI_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkStartupRunAsAdmin;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkSearchFileNameOnly;
        private System.Windows.Forms.ComboBox comboSearchEngineName;
        private System.Windows.Forms.TextBox txtSearchEngineURL;
        private System.Windows.Forms.Button cmdSaveExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkStartupAutoKill;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkDebugAlwaysShowInfo;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtVirusTotalAPIKey;
    }
}