﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Hardware.Info;

namespace Tinyinfo
{
	public partial class MainWindow : Form
	{
		static readonly IHardwareInfo hardwareInfo = new HardwareInfo();
		private delegate void SafeCallDelegate(string text);
		public MainWindow()
		{
			InitializeComponent();
		}

		public  void getdata()
		{
			var nl = Environment.NewLine;
			while (true)
			{
				hardwareInfo.RefreshCPUList(true);
				hardwareInfo.RefreshMemoryList();
			
				foreach (var cpu in hardwareInfo.CpuList)
				{
					//	CPU Info
					WriteTextSafe("CPU:" + nl);
					
					//	CPU ID
					AppendTextSafe("\tID: " + cpu.ProcessorId + nl);

					//	Manufacturer and Model
					AppendTextSafe("\tManufacturer: " + cpu.Manufacturer + nl);
					AppendTextSafe("\tModel: " + cpu.Name.Replace("  ", "") + nl);

					// Description
					AppendTextSafe("\tDescription: " + cpu.Description + nl);

					//	Socket
					AppendTextSafe("\tSocket: " + cpu.SocketDesignation + nl);

					//	Cores and Threads
					AppendTextSafe("\tCore Amount: " + cpu.NumberOfCores + " Physical, " + cpu.NumberOfLogicalProcessors + " Logical" + nl);

					//	VM Firmware
					AppendTextSafe("\tVirtualization Firmware Enabled: " + cpu.VirtualizationFirmwareEnabled + nl);


					//	Clockspeeds
					AppendTextSafe("\tClockspeeds:" + nl);
					AppendTextSafe("\t\t" + cpu.CurrentClockSpeed + "mHz Current" + nl);
					AppendTextSafe("\t\t" + cpu.MaxClockSpeed +"mHz Base");
					
					//	Memory;
					AppendTextSafe(nl + nl + "Memory:");
					foreach (var memory in hardwareInfo.MemoryList)
					{
						float memcap = memory.Capacity;

						AppendTextSafe(nl + "\t" + memory.BankLabel + ":" + nl+ "\t\tManufacturer: " + memory.Manufacturer + nl +"\t\t\tSize: " + memcap / 1073741824 + "GB" + nl + "\t\t\tSpeed: " + memory.Speed + "mT/s" + nl + "\t\t\tPart No.: " + memory.PartNumber + nl +"\t\t\tForm Factor: " + memory.FormFactor + nl +"\t\t\tMin. Voltage: " + memory.MinVoltage + "mV" + nl + "\t\t\tMax. Voltage: " + memory.MaxVoltage + "mV");
					}
				}
				
			}
		}
		Thread thread;
		private void WriteTextSafe(string text)
		{
			if (textBox1.InvokeRequired)
			{
				var d = new SafeCallDelegate(WriteTextSafe);
				textBox1.Invoke(d, new object[] { text });
			}
			else
			{
				textBox1.Text = text;
			}
		}
		private void AppendTextSafe(string text)
		{
			if (textBox1.InvokeRequired)
			{
				var d = new SafeCallDelegate(AppendTextSafe);
				textBox1.Invoke(d, new object[] { text });
			}
			else
			{
				textBox1.AppendText(text);
			}
		}

		public void button1_Click(object sender, EventArgs e)
		{
			label1.Visible = true;
			progressBar1.Visible = true;
			button1.Enabled = false;
			label1.Text = "Loading System Info.";
			progressBar1.Value = 25;
			hardwareInfo.RefreshCPUList();
			button2.Enabled = true;
			label1.Text = "Loading System Info..";
			progressBar1.Value = 50;
			hardwareInfo.RefreshOperatingSystem();
			button3.Enabled = true;
			Thread.Sleep(100);
			label1.Text = "Loading System Info...";
			progressBar1.Value = 75;
			thread = new Thread(new ThreadStart(getdata));
			Thread.Sleep(150);
			progressBar1.Value = 85;
			thread.IsBackground = true;
			label1.Text = "Loading System Info....";
			Thread.Sleep(150);
			progressBar1.Value = 100;
			label1.Visible = false;
			thread.Start();
			Thread.Sleep(500);
			progressBar1.Visible = false;

			/*
			Debug.WriteLine(hardwareInfo.CpuList.Count.ToString());

			foreach(var cpu in hardwareInfo.CpuList)
			{
				WriteTextSafe(textBox1.Text + " \n" + cpu.ProcessorId + "" + cpu.Manufacturer + " " + cpu.Name + "\n");
			}
			*/
		}

		private void button2_Click(object sender, EventArgs e)
		{
			button3.Enabled = false;
			thread.Abort();
			button2.Enabled = false;
			button1.Enabled = true;
		}

		private void button3_Click(object sender, EventArgs e)
		{
			if (thread.ThreadState == System.Threading.ThreadState.Stopped) {
				thread.Start();
			}
			else
			{
				thread.Abort();
			}
		}
	}
}
