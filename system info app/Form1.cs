using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Management;

namespace system_info_app
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // filling textboxes
            Processor();
            Graphic_Card();
            textBox_user.Text = Environment.UserName;
            textBox_os.Text = Environment.OSVersion.ToString();
            textBox_pc.Text = Environment.MachineName.ToString();
            textBox_motherboard.Text = MotherBoard();
            textBox_bios.Text = BIOS();
            textBox_ram.Text = RAM();
        }

        private void Processor()
        {
            RegistryKey processor_name = Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor\0", RegistryKeyPermissionCheck.ReadSubTree);   //This registry entry contains entry for processor info.

            if (processor_name != null)
            {
                if (processor_name.GetValue("ProcessorNameString") != null)
                {
                    textBox_processor.Text = processor_name.GetValue("ProcessorNameString").ToString();
                }
            }
            textBox_cores.Text = "кол-во ядер: " + Environment.ProcessorCount.ToString();
        }
        private void Graphic_Card()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DisplayConfiguration");

            //string graphicsCard = string.Empty;
            foreach (ManagementObject mo in searcher.Get())
            {
                foreach (PropertyData property in mo.Properties)
                {
                    if (property.Name == "Description")
                    {
                        textBox_graphics.Text = property.Value.ToString();
                    }
                }
            }
        }
        private string MotherBoard()
        {
            ManagementObjectSearcher baseboardSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
            ManagementObjectSearcher motherboardSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_MotherboardDevice");
            try
            {
                foreach (ManagementObject queryObj in baseboardSearcher.Get())
                {
                    return queryObj["Product"].ToString();
                }
                return "";
            }
            catch (Exception e)
            {
                return "";
            }
        }
        private string BIOS()
        {
            ManagementObjectSearcher searcher1 = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
            ManagementObjectCollection collection = searcher1.Get();
            foreach (ManagementObject obj in collection)
            {
                if (((string[])obj["BIOSVersion"]).Length > 1)
                    return ("BIOS VERSION: " + ((string[])obj["BIOSVersion"])[0] + " - " + ((string[])obj["BIOSVersion"])[1]);
                else
                   return ("BIOS VERSION: " + ((string[])obj["BIOSVersion"])[0]);
            }
            return "";
        }
        private string RAM()
        {
            ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
            ManagementObjectCollection results = searcher.Get();

            foreach (ManagementObject result in results)
            {
                //res = Convert.ToDouble(result["TotalVisibleMemorySize"]);
                double ram = Math.Round((Convert.ToDouble(result["TotalVisibleMemorySize"]) / (1024 * 1024)), 0);
                return (ram + " GB");
                //Console.WriteLine("Total usable memory size: " + res + "KB");
            }
            return "";
        }
    }

}
