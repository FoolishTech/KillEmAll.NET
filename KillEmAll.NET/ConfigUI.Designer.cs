
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
            this.boxStartup = new System.Windows.Forms.GroupBox();
            this.chkStartupAutoKill = new System.Windows.Forms.CheckBox();
            this.chkStartupRunAsAdmin = new System.Windows.Forms.CheckBox();
            this.boxSearch = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboSearchEngineName = new System.Windows.Forms.ComboBox();
            this.txtSearchEngineURL = new System.Windows.Forms.TextBox();
            this.chkSearchFileNameOnly = new System.Windows.Forms.CheckBox();
            this.cmdSaveExit = new System.Windows.Forms.Button();
            this.boxDebug = new System.Windows.Forms.GroupBox();
            this.chkDebugAlwaysShowInfo = new System.Windows.Forms.CheckBox();
            this.boxVirusTotal = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtVirusTotalAPIKey = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmdAllowList = new System.Windows.Forms.Button();
            this.cmdGeneralSettings = new System.Windows.Forms.Button();
            this.boxAllowed = new System.Windows.Forms.GroupBox();
            this.txtAllowedList = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.boxStartup.SuspendLayout();
            this.boxSearch.SuspendLayout();
            this.boxDebug.SuspendLayout();
            this.boxVirusTotal.SuspendLayout();
            this.boxAllowed.SuspendLayout();
            this.SuspendLayout();
            // 
            // boxStartup
            // 
            this.boxStartup.Controls.Add(this.chkStartupAutoKill);
            this.boxStartup.Controls.Add(this.chkStartupRunAsAdmin);
            this.boxStartup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boxStartup.Location = new System.Drawing.Point(161, 12);
            this.boxStartup.Name = "boxStartup";
            this.boxStartup.Size = new System.Drawing.Size(382, 69);
            this.boxStartup.TabIndex = 0;
            this.boxStartup.TabStop = false;
            this.boxStartup.Text = "Startup Behavior";
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
            // boxSearch
            // 
            this.boxSearch.Controls.Add(this.label2);
            this.boxSearch.Controls.Add(this.label1);
            this.boxSearch.Controls.Add(this.comboSearchEngineName);
            this.boxSearch.Controls.Add(this.txtSearchEngineURL);
            this.boxSearch.Controls.Add(this.chkSearchFileNameOnly);
            this.boxSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boxSearch.Location = new System.Drawing.Point(161, 155);
            this.boxSearch.Name = "boxSearch";
            this.boxSearch.Size = new System.Drawing.Size(382, 111);
            this.boxSearch.TabIndex = 1;
            this.boxSearch.TabStop = false;
            this.boxSearch.Text = "Search Configuration";
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
            this.cmdSaveExit.Location = new System.Drawing.Point(12, 325);
            this.cmdSaveExit.Name = "cmdSaveExit";
            this.cmdSaveExit.Size = new System.Drawing.Size(133, 30);
            this.cmdSaveExit.TabIndex = 2;
            this.cmdSaveExit.Text = "Save && Exit";
            this.cmdSaveExit.UseVisualStyleBackColor = true;
            this.cmdSaveExit.Click += new System.EventHandler(this.cmdSaveExit_Click);
            // 
            // boxDebug
            // 
            this.boxDebug.Controls.Add(this.chkDebugAlwaysShowInfo);
            this.boxDebug.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boxDebug.Location = new System.Drawing.Point(161, 94);
            this.boxDebug.Name = "boxDebug";
            this.boxDebug.Size = new System.Drawing.Size(382, 46);
            this.boxDebug.TabIndex = 3;
            this.boxDebug.TabStop = false;
            this.boxDebug.Text = "Debug Mode Behavior";
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
            // boxVirusTotal
            // 
            this.boxVirusTotal.Controls.Add(this.label5);
            this.boxVirusTotal.Controls.Add(this.label4);
            this.boxVirusTotal.Controls.Add(this.label3);
            this.boxVirusTotal.Controls.Add(this.txtVirusTotalAPIKey);
            this.boxVirusTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boxVirusTotal.Location = new System.Drawing.Point(161, 281);
            this.boxVirusTotal.Name = "boxVirusTotal";
            this.boxVirusTotal.Size = new System.Drawing.Size(382, 88);
            this.boxVirusTotal.TabIndex = 4;
            this.boxVirusTotal.TabStop = false;
            this.boxVirusTotal.Text = "VirusTotal API Key";
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
            // txtVirusTotalAPIKey
            // 
            this.txtVirusTotalAPIKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVirusTotalAPIKey.Location = new System.Drawing.Point(18, 44);
            this.txtVirusTotalAPIKey.Name = "txtVirusTotalAPIKey";
            this.txtVirusTotalAPIKey.Size = new System.Drawing.Size(348, 20);
            this.txtVirusTotalAPIKey.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(16, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Click Here";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(88, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(176, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "to learn how to obtain your API Key.";
            // 
            // cmdAllowList
            // 
            this.cmdAllowList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdAllowList.Location = new System.Drawing.Point(12, 64);
            this.cmdAllowList.Name = "cmdAllowList";
            this.cmdAllowList.Size = new System.Drawing.Size(133, 30);
            this.cmdAllowList.TabIndex = 5;
            this.cmdAllowList.Text = "Allowed Programs";
            this.cmdAllowList.UseVisualStyleBackColor = true;
            this.cmdAllowList.Click += new System.EventHandler(this.cmdAllowList_Click);
            // 
            // cmdGeneralSettings
            // 
            this.cmdGeneralSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdGeneralSettings.Location = new System.Drawing.Point(12, 18);
            this.cmdGeneralSettings.Name = "cmdGeneralSettings";
            this.cmdGeneralSettings.Size = new System.Drawing.Size(133, 30);
            this.cmdGeneralSettings.TabIndex = 6;
            this.cmdGeneralSettings.Text = "General Settings";
            this.cmdGeneralSettings.UseVisualStyleBackColor = true;
            this.cmdGeneralSettings.Click += new System.EventHandler(this.cmdGeneralSettings_Click);
            // 
            // boxAllowed
            // 
            this.boxAllowed.Controls.Add(this.label6);
            this.boxAllowed.Controls.Add(this.txtAllowedList);
            this.boxAllowed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boxAllowed.Location = new System.Drawing.Point(158, 13);
            this.boxAllowed.Name = "boxAllowed";
            this.boxAllowed.Size = new System.Drawing.Size(385, 356);
            this.boxAllowed.TabIndex = 7;
            this.boxAllowed.TabStop = false;
            this.boxAllowed.Text = "Allowed Programs (these programs will not be terminated.)";
            // 
            // txtAllowedList
            // 
            this.txtAllowedList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAllowedList.Location = new System.Drawing.Point(10, 38);
            this.txtAllowedList.Multiline = true;
            this.txtAllowedList.Name = "txtAllowedList";
            this.txtAllowedList.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtAllowedList.Size = new System.Drawing.Size(367, 312);
            this.txtAllowedList.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(9, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(184, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "One program name per line; no paths.";
            // 
            // ConfigUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 379);
            this.Controls.Add(this.cmdGeneralSettings);
            this.Controls.Add(this.cmdAllowList);
            this.Controls.Add(this.boxVirusTotal);
            this.Controls.Add(this.boxDebug);
            this.Controls.Add(this.cmdSaveExit);
            this.Controls.Add(this.boxSearch);
            this.Controls.Add(this.boxStartup);
            this.Controls.Add(this.boxAllowed);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "KillEmAll.NET Configuration";
            this.Load += new System.EventHandler(this.ConfigUI_Load);
            this.boxStartup.ResumeLayout(false);
            this.boxStartup.PerformLayout();
            this.boxSearch.ResumeLayout(false);
            this.boxSearch.PerformLayout();
            this.boxDebug.ResumeLayout(false);
            this.boxDebug.PerformLayout();
            this.boxVirusTotal.ResumeLayout(false);
            this.boxVirusTotal.PerformLayout();
            this.boxAllowed.ResumeLayout(false);
            this.boxAllowed.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox boxStartup;
        private System.Windows.Forms.CheckBox chkStartupRunAsAdmin;
        private System.Windows.Forms.GroupBox boxSearch;
        private System.Windows.Forms.CheckBox chkSearchFileNameOnly;
        private System.Windows.Forms.ComboBox comboSearchEngineName;
        private System.Windows.Forms.TextBox txtSearchEngineURL;
        private System.Windows.Forms.Button cmdSaveExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkStartupAutoKill;
        private System.Windows.Forms.GroupBox boxDebug;
        private System.Windows.Forms.CheckBox chkDebugAlwaysShowInfo;
        private System.Windows.Forms.GroupBox boxVirusTotal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtVirusTotalAPIKey;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button cmdAllowList;
        private System.Windows.Forms.Button cmdGeneralSettings;
        private System.Windows.Forms.GroupBox boxAllowed;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtAllowedList;
    }
}