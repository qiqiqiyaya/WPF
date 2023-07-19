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
            var localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey rk = localMachine.OpenSubKey(uninstallKey))
            {
                foreach (string skName in rk.GetSubKeyNames())
                {
                    using (RegistryKey sk = rk.OpenSubKey(skName))
                    {
                        try
                        {

                            var displayName = sk.GetValue("DisplayName") ?? "";
                            if (string.IsNullOrWhiteSpace(displayName.ToString()))
                            {
                                continue;
                            }
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
                            var DisplayIcon = sk.GetValue("DisplayIcon") ?? "";

                            var HelpTelephone = sk.GetValue("HelpTelephone") ?? "";
                            var InstallSource = sk.GetValue("InstallSource") ?? "";
                            var URLInfoAbout = sk.GetValue("URLInfoAbout") ?? "";
                            var URLUpdateInfo = sk.GetValue("URLUpdateInfo") ?? "";
                            var AuthorizedCDFPrefix = sk.GetValue("AuthorizedCDFPrefix") ?? "";
                            var Contact = sk.GetValue("Contact") ?? "";
                            var Comments = sk.GetValue("Comments") ?? "";
                            var Language = sk.GetValue("Language") ?? "";
                            var ModifyPath = sk.GetValue("ModifyPath") ?? "";
                            var Readme = sk.GetValue("Readme") ?? "";
                            var SettingsIdentifier = sk.GetValue("SettingsIdentifier") ?? "";

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
                                DisplayIcon.ToString(),

                                HelpTelephone.ToString(),
                                InstallSource.ToString(),
                                URLInfoAbout.ToString(),
                                URLUpdateInfo.ToString(),
                                AuthorizedCDFPrefix.ToString(),
                                Contact.ToString(),
                                Comments.ToString(),
                                Language.ToString(),
                                ModifyPath.ToString(),
                                Readme.ToString(),
                                SettingsIdentifier.ToString(),
                            });

                            lstDisplayHardware.Items.Add(item);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }


            var localMachine32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            using (RegistryKey rk = localMachine32.OpenSubKey(uninstallKey))
            {
                foreach (string skName in rk.GetSubKeyNames())
                {
                    using (RegistryKey sk = rk.OpenSubKey(skName))
                    {
                        try
                        {
                            var displayName = sk.GetValue("DisplayName") ?? "";
                            if (string.IsNullOrWhiteSpace(displayName.ToString()))
                            {
                                continue;
                            }
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
                            var DisplayIcon = sk.GetValue("DisplayIcon") ?? "";

                            var HelpTelephone = sk.GetValue("HelpTelephone") ?? "";
                            var InstallSource = sk.GetValue("InstallSource") ?? "";
                            var URLInfoAbout = sk.GetValue("URLInfoAbout") ?? "";
                            var URLUpdateInfo = sk.GetValue("URLUpdateInfo") ?? "";
                            var AuthorizedCDFPrefix = sk.GetValue("AuthorizedCDFPrefix") ?? "";
                            var Contact = sk.GetValue("Contact") ?? "";
                            var Comments = sk.GetValue("Comments") ?? "";
                            var Language = sk.GetValue("Language") ?? "";
                            var ModifyPath = sk.GetValue("ModifyPath") ?? "";
                            var Readme = sk.GetValue("Readme") ?? "";
                            var SettingsIdentifier = sk.GetValue("SettingsIdentifier") ?? "";

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
                                DisplayIcon.ToString(),

                                HelpTelephone.ToString(),
                                InstallSource.ToString(),
                                URLInfoAbout.ToString(),
                                URLUpdateInfo.ToString(),
                                AuthorizedCDFPrefix.ToString(),
                                Contact.ToString(),
                                Comments.ToString(),
                                Language.ToString(),
                                ModifyPath.ToString(),
                                Readme.ToString(),
                                SettingsIdentifier.ToString(),
                            });

                            lstDisplayHardware_32.Items.Add(item);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
        }

    }
}
