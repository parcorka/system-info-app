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
        }

        private void Processor()
        {
            RegistryKey processor_name = Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor\0", RegistryKeyPermissionCheck.ReadSubTree);   //This registry entry contains entry for processor info.

            if (processor_name != null)
            {
                if (processor_name.GetValue("ProcessorNameString") != null)
                {
                    textBox1.Text = processor_name.GetValue("ProcessorNameString").ToString();
                }
            }
            //textBox1.Text = Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER");
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
                        textBox2.Text = property.Value.ToString();
                    }
                }
            }
        }
    }

}
