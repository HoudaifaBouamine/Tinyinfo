﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using IniParser;
using IniParser.Model;

namespace Tinyinfo
{
	public partial class SettingsWindow : Form
	{
		public SettingsWindow()
		{
			InitializeComponent();

			//	apply saved theme
			refreshTheme();

			//	Create ini parser and read ini file
			var parser = new FileIniDataParser();
			IniData data = parser.ReadFile("./tinyinfo.ini");

			//	load saved theme setting
			if (data.GetKey("tinyinfo.theme") == "light")
			{
				//	light theme
				lightThemeRadioButton.Checked = true;
				darkThemeRadioButton.Checked = false;
				this.BackColor = Color.White;
				this.ForeColor = Color.Black;
				themeTab.BackColor = Color.White;
				themeTab.ForeColor = Color.Black;
				generalTab.BackColor = Color.White;
				generalTab.ForeColor = Color.Black;
				fontButton.ForeColor = Color.Black;
				applyButton.ForeColor = Color.Black;
				cancelButton.ForeColor = Color.Black;

			}
			else
			{
				//	dark theme
				lightThemeRadioButton.Checked = false;
				darkThemeRadioButton.Checked = true;
				ActiveForm.BackColor = Color.Gray;
				ActiveForm.ForeColor = Color.White;
				themeTab.BackColor = Color.DimGray;
				themeTab.ForeColor = Color.White;
				generalTab.BackColor = Color.DimGray;
				generalTab.ForeColor = Color.White;
				fontButton.ForeColor = Color.Black;
				applyButton.ForeColor = Color.Black;
				cancelButton.ForeColor = Color.Black;
			}

			//	load font size setting
			fontSizeUpDown.Value = Convert.ToInt32(data.GetKey("tinyinfo.font"));

			//	load refresh rate setting
			refreshRateUpDown.Value = Convert.ToInt32(data.GetKey("tinyinfo.refresh"));
		}

		public void refreshTheme()
		{
			//	Check if file exists, if it doesnt create it with default settings
			if (File.Exists("./tinyinfo.ini") == false)
			{
				File.WriteAllText("./tinyinfo.ini", "[tinyinfo]\ntheme=light\nfont=10\nrefresh=500");
			}

			//	Create ini parser and read ini file
			var parser = new FileIniDataParser();
			IniData data = parser.ReadFile("./tinyinfo.ini");

			//	Read Settings
			//	Set theme
			if (data.GetKey("tinyinfo.theme") == "dark")
			{
				//	Dark theme
				ForeColor = Color.White;
				BackColor = Color.Gray;
				themeTab.BackColor = Color.DimGray;
				themeTab.ForeColor = Color.White;
				generalTab.BackColor = Color.DimGray;
				generalTab.ForeColor = Color.White;
				fontButton.ForeColor = Color.Black;
				applyButton.ForeColor = Color.Black;
				cancelButton.ForeColor = Color.Black;
			}
			else
			{
				//	Light theme
				ForeColor = Color.Black;
				BackColor = Color.White;
				themeTab.BackColor = Color.White;
				themeTab.ForeColor = Color.Black;
				generalTab.BackColor = Color.White;
				generalTab.ForeColor = Color.Black;
				fontButton.ForeColor = Color.Black;
				applyButton.ForeColor = Color.Black;
				cancelButton.ForeColor = Color.Black;
			}
		}

		private void applyButton_Click(object sender, EventArgs e)
		{
			//	Create ini parser and read ini file
			var parser = new FileIniDataParser();
			IniData data = parser.ReadFile("./tinyinfo.ini");

			//	write theme mode into ini
			if (lightThemeRadioButton.Checked)
			{
				//	light mode
				data["tinyinfo"]["theme"] = "light";
				parser.WriteFile("./tinyinfo.ini", data);
			}
			else
			{
				//	dark mode
				data["tinyinfo"]["theme"] = "dark";
				parser.WriteFile("./tinyinfo.ini", data);
			}

			//	write font size into ini file
			data["tinyinfo"]["font"] = fontSizeUpDown.Value.ToString();
			parser.WriteFile("./tinyinfo.ini", data);

			//	wrire refresh rate into ini file
			data["tinyinfo"]["refresh"] = refreshRateUpDown.Value.ToString();
			parser.WriteFile("./tinyinfo.ini", data);

			//	reload theme
			refreshTheme();
		}

		private void fontButton_Click(object sender, EventArgs e)
		{
			fontDialog.ShowDialog();
		}
	}
}
