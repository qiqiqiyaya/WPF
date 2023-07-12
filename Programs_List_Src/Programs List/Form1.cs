using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Programs_List
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void action_btn_get_Click(object sender, EventArgs e)
        {
            string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(uninstallKey))
            {
                foreach (string skName in rk.GetSubKeyNames())
                {
                    using (RegistryKey sk = rk.OpenSubKey(skName))
                    {
                        try
                        {

                            var displayName = sk.GetValue("DisplayName") ?? "";
                            var size = sk.GetValue("EstimatedSize") ?? "";
                            var displayVersion = sk.GetValue("DisplayVersion") ?? "";
                            var installDate = sk.GetValue("InstallDate") ?? "";
                            var publisher = sk.GetValue("Publisher") ?? "";
                            var installLocation = sk.GetValue("InstallLocation") ?? "";
                            var version = sk.GetValue("Version") ?? "";
                            var versionMinor = sk.GetValue("VersionMinor") ?? "";
                            var versionMajor = sk.GetValue("VersionMajor") ?? "";
                            var uninstallString = sk.GetValue("UninstallString") ?? "";
                            var helpLink = sk.GetValue("HelpLink") ?? "";
                            var displayIcon = sk.GetValue("DisplayIcon") ?? "";

                            ListViewItem item = new ListViewItem(new string[]
                            {
                                displayName.ToString(),
                                size.ToString(),
                                displayVersion.ToString(),
                                installDate.ToString(),
                                publisher.ToString(),
                                installLocation.ToString(),
                                versionMajor.ToString(),
                                version.ToString(),
                                versionMinor.ToString(),
                                uninstallString.ToString(),
                                helpLink.ToString(),
                                displayIcon.ToString(),
                            });

                            lstDisplayHardware.Items.Add(item);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                label1.Text += " (" + lstDisplayHardware.Items.Count.ToString() + ")";
            }
        }

    }
}
