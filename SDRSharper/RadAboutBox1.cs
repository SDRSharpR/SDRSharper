using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Runtime.Versioning;

namespace SDRSharp
{
    partial class RadAboutBox1 : Telerik.WinControls.UI.RadForm
    {
        public static string arch = "";

        public RadAboutBox1()
        {
            InitializeComponent();

            string xresult, xresult_core, xresult_plugins = "";

            #region [Assembly Header]
            //  Initialize the AboutBox to display the product information from the assembly information.
            //  Change assembly information settings for your application through either:
            //  - Project->Properties->Application->Assembly Information
            //  - AssemblyInfo.cs
            
            this.Text = String.Format("About {0}", AssemblyTitle);
            xresult = AssemblyProduct;
            xresult += String.Format(" Version {0}", AssemblyVersion) + "\r\n";
            xresult += AssemblyCopyright + "\r\n\r\n";
            //xresult += AssemblyCompany + "\r\n";
            //xresult += AssemblyDescription + "\r\n\r\n";
            #endregion

            xresult_core = "Core Versions:\r\n";

            #region [SDRSharper Revised]
            bool? SDRSharpMain = UnmanagedDllIs64Bit(Application.ExecutablePath);
            if (SDRSharpMain.HasValue)
            {
                if ((bool)SDRSharpMain)
                {
                    arch = "x64";
                }
                else
                {
                    arch = "x32";
                }
            }
            else
            {
                arch = "Any CPU";
            }

            Assembly attributes = MainForm.UnmanagedDLLAssemblyVer;
            TargetFrameworkAttribute x = (System.Runtime.Versioning.TargetFrameworkAttribute)attributes.GetCustomAttribute(typeof(TargetFrameworkAttribute));
            if (x == null)
            {
                x = new TargetFrameworkAttribute("Pre-.NET Framework 4.0");
                x.FrameworkDisplayName = "Pre-.NET Framework 4.0";
            }

            xresult_core += arch + " - " + x.FrameworkDisplayName.Replace(" Framework ", " ") + " - SDRSharperR.exe\r\n";
            arch = "";
            #endregion

            #region [Radio]
            bool? SDRSharpRadio = UnmanagedDllIs64Bit(Application.StartupPath + "\\SDRSharp.Radio.dll");
            if (SDRSharpRadio.HasValue)
            {
                if ((bool)SDRSharpRadio)
                {
                    arch = "x64";
                }
                else
                {
                    arch = "x32";
                }
            }
            else
            {
                arch = "Any CPU";
            }

            attributes = Radio.Utils.UnmanagedDLLAssemblyVer;
            x = (System.Runtime.Versioning.TargetFrameworkAttribute)attributes.GetCustomAttribute(typeof(TargetFrameworkAttribute));
            if (x == null)
            {
                x = new TargetFrameworkAttribute("Pre-.NET Framework 4.0");
                x.FrameworkDisplayName = "Pre-.NET Framework 4.0";
            }

            xresult_core += arch + " - " + x.FrameworkDisplayName.Replace(" Framework ", " ") + " - SDRSharp.Radio.dll\r\n";
            arch = "";
            #endregion

            #region [Controls]
            bool? SDRSharpControls = UnmanagedDllIs64Bit(Application.StartupPath + "\\SDRSharp.Controls.dll");
            if (SDRSharpControls.HasValue)
            {
                if ((bool)SDRSharpControls)
                {
                    arch = "x64";
                }
                else
                {
                    arch = "x32";
                }
            }
            else
            {
                arch = "Any CPU";
            }

            attributes = SDRSharp.Controls.ControlsUtils.UnmanagedDLLAssemblyVer;
            x = (System.Runtime.Versioning.TargetFrameworkAttribute)attributes.GetCustomAttribute(typeof(TargetFrameworkAttribute));
            if (x == null)
            {
                x = new TargetFrameworkAttribute("Pre-.NET Framework 4.0");
                x.FrameworkDisplayName = "Pre-.NET Framework 4.0";
            }

            xresult_core += arch + " - " + x.FrameworkDisplayName.Replace(" Framework ", " ") + " - SDRSharp.Controls.dll\r\n";
            arch = "";
            #endregion

            #region [CollapsiblePanel]
            bool? SDRSharpCollapsiblePanel = UnmanagedDllIs64Bit(Application.StartupPath + "\\SDRSharp.CollapsiblePanel.dll");
            if (SDRSharpCollapsiblePanel.HasValue)
            {
                if ((bool)SDRSharpCollapsiblePanel)
                {
                    arch = "x64";
                }
                else
                {
                    arch = "x32";
                }
            }
            else
            {
                arch = "Any CPU";
            }

            attributes = CollapsiblePanel.CollapsiblePanelUtils.UnmanagedDLLAssemblyVer;
            x = (System.Runtime.Versioning.TargetFrameworkAttribute)attributes.GetCustomAttribute(typeof(TargetFrameworkAttribute));
            if (x == null)
            {
                x = new TargetFrameworkAttribute("Pre-.NET Framework 4.0");
                x.FrameworkDisplayName = "Pre-.NET Framework 4.0";
            }

            xresult_core += arch + " - " + x.FrameworkDisplayName.Replace(" Framework ", " ") + " - SDRSharp.CollapsiblePanel.dll\r\n";
            arch = "";
            #endregion

            #region [PanView]
            bool? SDRSharpPanView = UnmanagedDllIs64Bit(Application.StartupPath + "\\SDRSharp.PanView.dll");
            if (SDRSharpPanView.HasValue)
            {
                if ((bool)SDRSharpPanView)
                {
                    arch = "x64";
                }
                else
                {
                    arch = "x32";
                }
            }
            else
            {
                arch = "Any CPU";
            }

            attributes = PanView.PanViewUtils.UnmanagedDLLAssemblyVer;
            x = (System.Runtime.Versioning.TargetFrameworkAttribute)attributes.GetCustomAttribute(typeof(TargetFrameworkAttribute));
            if (x == null)
            {
                x = new TargetFrameworkAttribute("Pre-.NET Framework 4.0");
                x.FrameworkDisplayName = "Pre-.NET Framework 4.0";
            }

            xresult_core += arch + " - " + x.FrameworkDisplayName.Replace(" Framework ", " ") + " - SDRSharp.PanView.dll\r\n";
            arch = "";
            #endregion

            #region [Common]
            bool? SDRSharpCommon = UnmanagedDllIs64Bit(Application.StartupPath + "\\SDRSharp.Common.dll");
            if (SDRSharpCommon.HasValue)
            {
                if ((bool)SDRSharpCommon)
                {
                    arch = "x64";
                }
                else
                {
                    arch = "x32";
                }
            }
            else
            {
                arch = "Any CPU";
            }

            attributes = Common.CommonUtils.UnmanagedDLLAssemblyVer;
            x = (System.Runtime.Versioning.TargetFrameworkAttribute)attributes.GetCustomAttribute(typeof(TargetFrameworkAttribute));
            if (x == null)
            {
                x = new TargetFrameworkAttribute("Pre-.NET Framework 4.0");
                x.FrameworkDisplayName = "Pre-.NET Framework 4.0";
            }

            xresult_core += arch + " - " + x.FrameworkDisplayName.Replace(" Framework ", " ") + " - SDRSharp.Common.dll\r\n";
            arch = "";
            #endregion

            xresult_plugins = "Plugin Versions:\r\n";

            #region [FrequencyManager]
            bool? SDRSharpFreqMan = UnmanagedDllIs64Bit(Application.StartupPath + "\\SDRSharp.FrequencyManager.dll");
            if (SDRSharpCommon.HasValue)
            {
                if ((bool)SDRSharpFreqMan)
                {
                    arch = "x64";
                }
                else
                {
                    arch = "x32";
                }
            }
            else
            {
                arch = "Any CPU";
            }

            attributes = FrequencyManager.FreqManUtils.UnmanagedDLLAssemblyVer;
            x = (System.Runtime.Versioning.TargetFrameworkAttribute)attributes.GetCustomAttribute(typeof(TargetFrameworkAttribute));
            if (x == null)
            {
                x = new TargetFrameworkAttribute("Pre-.NET Framework 4.0");
                x.FrameworkDisplayName = "Pre-.NET Framework 4.0";
            }

            xresult_plugins += arch + " - " + x.FrameworkDisplayName.Replace(" Framework ", " ") + " - SDRSharp.FrequencyManager.dll\r\n";
            arch = "";
            #endregion

            radLabel1.Text = xresult;
            radLabel2.Text = xresult_core;
            radLabel3.Text = xresult_plugins;
        }

        public static MachineType GetDllMachineType(string dllPath)
        {
            //see http://www.microsoft.com/whdc/system/platform/firmware/PECOFF.mspx
            //offset to PE header is always at 0x3C
            //PE header starts with "PE\0\0" =  0x50 0x45 0x00 0x00
            //followed by 2-byte machine type field (see document above for enum)
            FileStream fs = new FileStream(dllPath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            fs.Seek(0x3c, SeekOrigin.Begin);
            Int32 peOffset = br.ReadInt32();
            fs.Seek(peOffset, SeekOrigin.Begin);
            UInt32 peHead = br.ReadUInt32();

            if (peHead != 0x00004550) // "PE\0\0", little-endian
                throw new Exception("Can't find PE header");

            MachineType machineType = (MachineType)br.ReadUInt16();

            br.Close();
            fs.Close();

            return machineType;
        }

        public enum MachineType : ushort
        {
            IMAGE_FILE_MACHINE_UNKNOWN = 0x0,
            IMAGE_FILE_MACHINE_AM33 = 0x1d3,
            IMAGE_FILE_MACHINE_AMD64 = 0x8664,
            IMAGE_FILE_MACHINE_ARM = 0x1c0,
            IMAGE_FILE_MACHINE_EBC = 0xebc,
            IMAGE_FILE_MACHINE_I386 = 0x14c,
            IMAGE_FILE_MACHINE_IA64 = 0x200,
            IMAGE_FILE_MACHINE_M32R = 0x9041,
            IMAGE_FILE_MACHINE_MIPS16 = 0x266,
            IMAGE_FILE_MACHINE_MIPSFPU = 0x366,
            IMAGE_FILE_MACHINE_MIPSFPU16 = 0x466,
            IMAGE_FILE_MACHINE_POWERPC = 0x1f0,
            IMAGE_FILE_MACHINE_POWERPCFP = 0x1f1,
            IMAGE_FILE_MACHINE_R4000 = 0x166,
            IMAGE_FILE_MACHINE_SH3 = 0x1a2,
            IMAGE_FILE_MACHINE_SH3DSP = 0x1a3,
            IMAGE_FILE_MACHINE_SH4 = 0x1a6,
            IMAGE_FILE_MACHINE_SH5 = 0x1a8,
            IMAGE_FILE_MACHINE_THUMB = 0x1c2,
            IMAGE_FILE_MACHINE_WCEMIPSV2 = 0x169,
        }
        
        public static bool? UnmanagedDllIs64Bit(string dllPath) // returns true if the dll is 64-bit, false if 32-bit, and null if unknown
        {
            switch (GetDllMachineType(dllPath))
            {
                case MachineType.IMAGE_FILE_MACHINE_AMD64:
                case MachineType.IMAGE_FILE_MACHINE_IA64:
                    return true;
                case MachineType.IMAGE_FILE_MACHINE_I386:
                    return false;
                default:
                    return null;
            }
        }

        public static string getArch(string path)
        {
            MachineType dlltype = GetDllMachineType(path);

            if (dlltype.Equals(MachineType.IMAGE_FILE_MACHINE_I386))
            {
                Console.WriteLine("Dll architecture: x86/32bit");
                arch = "x86";
            }
            else if (dlltype.Equals(MachineType.IMAGE_FILE_MACHINE_AMD64))
            {
                Console.WriteLine("Dll architecture: x64/64bit");
                arch = "x64";
            }

            return arch;
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                // Get all Title attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                // If there is at least one Title attribute
                if (attributes.Length > 0)
                {
                    // Select the first one
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    // If it is not an empty string, return it
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }
                // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                // Get all Description attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                // If there aren't any Description attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Description attribute, return its value
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                // Get all Product attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                // If there aren't any Product attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Product attribute, return its value
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                // Get all Copyright attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                // If there aren't any Copyright attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Copyright attribute, return its value
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                // Get all Company attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                // If there aren't any Company attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Company attribute, return its value
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion


    }
}
