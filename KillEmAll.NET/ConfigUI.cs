﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace KillEmAll.NET
{
    public partial class ConfigUI : Form
    {
        private const string scSearchEngine_Custom = "Custom URL";
        private const string scSearchEngine_Bing = "Bing";
        private const string scSearchURL_Bing = "https://www.bing.com/search?q=";
        private const string scSearchEngine_DuckDuckGo = "DuckDuckGo";
        private const string scSearchURL_DuckDuckGo = "https://duckduckgo.com/?q=";
        private const string scSearchEngine_Google = "Google";
        private const string scSearchURL_Google = "https://www.google.com/search?q=";
        private const string scSearchEngine_GoogleEng = "Google (English)";
        private const string scSearchURL_GoogleEng = "https://www.google.com/search?hl=en&q=";
        private const string scSearchEngine_GoogleEnc = "Google (Encrypted)";
        private const string scSearchURL_GoogleEnc = "https://encrypted.google.com/search?aq=f&ie=UTF-8&q=";
        private const string scSearchEngine_GoogleEncEng = "Google (Encrypted/English)";
        private const string scSearchURL_GoogleEncEng = "https://encrypted.google.com/search?hl=en&aq=f&ie=UTF-8&q=";

        public ConfigUI()
        {
            InitializeComponent();
        }

        private void ConfigUI_Load(object sender, EventArgs e)
        {
            // populate search combo
            comboSearchEngineName.Items.Add(scSearchEngine_Custom);
            comboSearchEngineName.Items.Add(scSearchEngine_Bing);
            comboSearchEngineName.Items.Add(scSearchEngine_DuckDuckGo);
            comboSearchEngineName.Items.Add(scSearchEngine_Google);
            comboSearchEngineName.Items.Add(scSearchEngine_GoogleEng);
            comboSearchEngineName.Items.Add(scSearchEngine_GoogleEnc);
            comboSearchEngineName.Items.Add(scSearchEngine_GoogleEncEng);

            // load settings from INI
            if (Program.IniRead("Startup", "ForceAdmin") == "1")
                chkStartupRunAsAdmin.Checked = true;

            if (Program.IniRead("Startup", "AutoKill") == "1")
                chkStartupAutoKill.Checked = true;

            if (Program.IniRead("DebugMode", "ShowFileInfo") == "1")
                chkDebugAlwaysShowInfo.Checked = true;

            if (Program.IniRead("Search", "FileNameOnly") == "1")
                chkSearchFileNameOnly.Checked = true;

            txtSearchEngineURL.Text = Program.IniRead("Search", "URL");

            // if nothing, set a default
            if (txtSearchEngineURL.Text.Trim().Length < 1)
            {
                comboSearchEngineName.Text = scSearchEngine_GoogleEng;
                txtSearchEngineURL.Text = scSearchURL_GoogleEng;
            }
        }

        private void cmdSaveExit_Click(object sender, EventArgs e)
        {
            string value = "";
            bool success = false;

            if (chkStartupRunAsAdmin.Checked)
                value = "1";
            else
                value = "0";
            success = Program.IniWrite("Startup", "ForceAdmin", value);
            if (!success)
            {
                // there was a problem writing the INI file; inform user and exit
                MessageBox.Show("There was a problem saving the INI file; make sure the path is writable!");
                return;
            }

            // continue writing settings since we were able to save the first one successfully.

            if (chkStartupAutoKill.Checked)
                value = "1";
            else
                value = "0";
            Program.IniWrite("Startup", "AutoKill", value);

            if (chkDebugAlwaysShowInfo.Checked)
                value = "1";
            else
                value = "0";
            Program.IniWrite("DebugMode", "ShowFileInfo", value);

            if (chkSearchFileNameOnly.Checked)
                value = "1";
            else
                value = "0";
            Program.IniWrite("Search", "FileNameOnly", value);

            Program.IniWrite("Search", "URL", txtSearchEngineURL.Text);

            this.Close();
        }

        private void comboSearchEngineName_SelectedIndexChanged(object sender, EventArgs e)
        {
            searchEngineComboChange();
        }

        private void searchEngineComboChange()
        {
            switch (comboSearchEngineName.Text)
            {
                case scSearchEngine_Custom:
                    // do nothing
                    break;
                case scSearchEngine_Bing:
                    txtSearchEngineURL.Text = scSearchURL_Bing;
                    break;
                case scSearchEngine_DuckDuckGo:
                    txtSearchEngineURL.Text = scSearchURL_DuckDuckGo;
                    break;
                case scSearchEngine_Google:
                    txtSearchEngineURL.Text = scSearchURL_Google;
                    break;
                case scSearchEngine_GoogleEng:
                    txtSearchEngineURL.Text = scSearchURL_GoogleEng;
                    break;
                case scSearchEngine_GoogleEnc:
                    txtSearchEngineURL.Text = scSearchURL_GoogleEnc;
                    break;
                case scSearchEngine_GoogleEncEng:
                    txtSearchEngineURL.Text = scSearchURL_GoogleEncEng;
                    break;
                default:
                    break;
            }
        }

        private void txtSearchEngineURL_TextChanged(object sender, EventArgs e)
        {
            searchEngineTextChange();
        }

        private void searchEngineTextChange()
        {
            switch (txtSearchEngineURL.Text)
            {
                case scSearchURL_Bing:
                    comboSearchEngineName.Text = scSearchEngine_Bing;
                    break;
                case scSearchURL_DuckDuckGo:
                    comboSearchEngineName.Text = scSearchEngine_DuckDuckGo;
                    break;
                case scSearchURL_Google:
                    comboSearchEngineName.Text = scSearchEngine_Google;
                    break;
                case scSearchURL_GoogleEng:
                    comboSearchEngineName.Text = scSearchEngine_GoogleEng;
                    break;
                case scSearchURL_GoogleEnc:
                    comboSearchEngineName.Text = scSearchEngine_GoogleEnc;
                    break;
                case scSearchURL_GoogleEncEng:
                    comboSearchEngineName.Text = scSearchEngine_GoogleEncEng;
                    break;
                default:
                    comboSearchEngineName.Text = scSearchEngine_Custom;
                    break;
            }
        }
    }
}
