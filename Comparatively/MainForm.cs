using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

using static Comparatively.Helpers;

namespace Comparatively
{
    public partial class MainForm : Form
    {
        private Color InfoColour = Color.FromArgb(160, 240, 255);
        private Color WarningColour = Color.FromArgb(255, 212, 121);
        private Color DangerColour = Color.FromArgb(243, 103, 103);

        private bool UnsavedChanges = false;

        public BindingList<Setting3Tiers> AppSettings { get; set; }
        private List<Setting3Tiers> AppSettingBacking;
        private string SortOrder;

        public string LastDevPath
        {
            get { return GetAppSetting("DevPath"); }
            set { SetAppSetting("DevPath", value); }
        }

        public string LastQaPath
        {
            get { return GetAppSetting("QaPath"); }
            set { SetAppSetting("QaPath", value); }
        }

        public string LastProdPath
        {
            get { return GetAppSetting("ProdPath"); }
            set { SetAppSetting("ProdPath", value); }
        }




        public MainForm()
        {
            InitializeComponent();
            InitializeLists();

            bool show = true;
            if (bool.TryParse(GetAppSetting("ShowTipsAtStartup"), out show) == false | show)
            {
                var dlg = new TipsForm();
                dlg.TopMost = true;
                dlg.Show();
            }
        }

        private void InitializeLists()
        {
            AppSettingBacking = new List<Setting3Tiers>();
            AppSettings = new BindingList<Setting3Tiers>(AppSettingBacking);
            dataGridView1.DataSource = AppSettings;
            txtDevPath.Text = LastDevPath ?? "";
            txtQaPath.Text = LastQaPath ?? "";
            txtProdPath.Text = LastProdPath ?? "";
        }

        /// <summary>
        /// GO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGo_Click(object sender, EventArgs e)
        {
            // tidy up first.
            if (chkClear.Checked)
            {
                AppSettings.Clear();
            }

            // dev and titles
            XElement x = XElement.Load(txtDevPath.Text);
            LastDevPath = txtDevPath.Text;
            GetXmlValues(x, Tier.Dev, AppSettingBacking);

            // QA
            x = XElement.Load(txtQaPath.Text);
            LastQaPath = txtQaPath.Text;
            GetXmlValues(x, Tier.Qa, AppSettingBacking);

            // Prod
            x = XElement.Load(txtProdPath.Text);
            LastProdPath = txtProdPath.Text;
            GetXmlValues(x, Tier.Prod, AppSettingBacking);

            AppSettings = new BindingList<Setting3Tiers>(AppSettingBacking);
            dataGridView1.DataSource = AppSettings;

            RefreshGridColours();
        }

        private void GetXmlValues(XElement x, Tier tier, List<Setting3Tiers> settingsList)
        {
            string Folder = Directory.GetParent(LastDevPath).Name;
            var ConnStrings = x.Descendants("connectionStrings").DescendantNodes();
            foreach (XElement setting in ConnStrings.Where(n => n.NodeType == XmlNodeType.Element))
            {
                var Match = settingsList.FirstOrDefault(s => s.Key == setting.Attribute("name").Value && s.FolderName == Folder);
                bool MatchWasNull = Match == null;
                if (MatchWasNull) { Match = new Setting3Tiers(Folder, setting.Attribute("name").Value); }
                switch (tier)
                {
                    case Tier.Dev:
                        Match.ValueDev = setting.Attribute("connectionString").Value;
                        break;
                    case Tier.Qa:
                        Match.ValueQa = setting.Attribute("connectionString").Value;
                        break;
                    case Tier.Prod:
                        Match.ValueProd = setting.Attribute("connectionString").Value;
                        break;
                    default:
                        break;
                }
                if (MatchWasNull) { settingsList.Add(Match); }
            }
            var Settings = x.Descendants("appSettings").DescendantNodes();
            foreach (XElement setting in Settings.Where(n => n.NodeType == XmlNodeType.Element))
            {
                var Match = settingsList.FirstOrDefault(s => s.Key == setting.Attribute("key").Value && s.FolderName == Folder);
                bool MatchWasNull = Match == null;
                if (MatchWasNull) { Match = new Setting3Tiers(Folder, setting.Attribute("key").Value); }
                switch (tier)
                {
                    case Tier.Dev:
                        Match.ValueDev = setting.Attribute("value").Value;
                        break;
                    case Tier.Qa:
                        Match.ValueQa = setting.Attribute("value").Value;
                        break;
                    case Tier.Prod:
                        Match.ValueProd = setting.Attribute("value").Value;
                        break;
                    default:
                        break;
                }
                if (MatchWasNull) { settingsList.Add(Match); }
            }
        }


        private void RefreshGridColours()
        {
            foreach (Setting3Tiers setting in AppSettings)
            {
                if (!setting.ValuesInternallyEqual) { setting.Result = SettingComparisonResult.DifferentInTiers; }
                if (AppSettings.Any(s => s.Key == setting.Key && s.FolderName != setting.FolderName && setting.SameValuesAs(s) == false))
                {
                    if (setting.ValuesInternallyEqual)
                    {
                        setting.Result = SettingComparisonResult.DifferentAcrossFolders;
                    }
                    else
                    {
                        setting.Result = SettingComparisonResult.DifferentBothTiersAndFolders;
                    }

                }
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    switch ((SettingComparisonResult)row.Cells["Result"].Value)
                    {
                        case SettingComparisonResult.DifferentBothTiersAndFolders:
                            foreach (DataGridViewCell cell in row.Cells) { cell.Style.BackColor = DangerColour; }
                            break;
                        case SettingComparisonResult.DifferentInTiers:
                            foreach (DataGridViewCell cell in row.Cells) { cell.Style.BackColor = InfoColour; }
                            break;
                        case SettingComparisonResult.DifferentAcrossFolders:
                            foreach (DataGridViewCell cell in row.Cells) { cell.Style.BackColor = WarningColour; }
                            break;
                        case SettingComparisonResult.Same:
                        default:
                            foreach (DataGridViewCell cell in row.Cells) { cell.Style.BackColor = Color.White; }
                            break;
                    }
                }
            }
        }

        /// <summary>
        ///  dev browse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowseDev_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                if (!string.IsNullOrEmpty(LastDevPath))
                {
                    dlg.InitialDirectory = Directory.GetParent(LastDevPath).FullName;
                }

                dlg.Filter = "Config files|*.config|All files (*.*)|*.*";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtDevPath.Text = dlg.FileName;
                }
            }
        }

        /// <summary>
        /// QA brows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowseQa_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                if (!string.IsNullOrEmpty(LastQaPath))
                {
                    dlg.InitialDirectory = Directory.GetParent(LastQaPath).FullName;
                }
                dlg.Filter = "Config files|*.config|All files (*.*)|*.*";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtQaPath.Text = dlg.FileName;
                }
            }
        }

        /// <summary>
        /// Prod bros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowseProd_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                if (!string.IsNullOrEmpty(LastProdPath))
                {
                    dlg.InitialDirectory = Directory.GetParent(LastProdPath).FullName;
                }
                dlg.Filter = "Config files|*.config|All files (*.*)|*.*";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtProdPath.Text = dlg.FileName;
                }
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var Grid = sender as DataGridView;
            if (e.RowIndex < 0)
            {
                Grid.Columns[e.ColumnIndex].Width = Grid.Columns[e.ColumnIndex].GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCellsExceptHeader, true);
            }
            else
            {
                DataGridViewCell Cell = Grid[e.ColumnIndex, e.RowIndex];
                if (!string.IsNullOrEmpty(Cell.Value.ToString()))
                {
                    Clipboard.SetText(Cell.Value.ToString());
                }
            }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            SortOrder = dataGridView1.Columns[e.ColumnIndex].DataPropertyName;
            switch (SortOrder)
            {
                case "FolderName":
                    AppSettings = new BindingList<Setting3Tiers>(AppSettingBacking.OrderBy(x => x.FolderName).ToList());
                    break;
                case "Key":
                    AppSettings = new BindingList<Setting3Tiers>(AppSettingBacking.OrderBy(x => x.Key).ToList());
                    break;
                case "ValueDev":
                    AppSettings = new BindingList<Setting3Tiers>(AppSettingBacking.OrderBy(x => x.ValueDev).ToList());
                    break;
                case "ValueQa":
                    AppSettings = new BindingList<Setting3Tiers>(AppSettingBacking.OrderBy(x => x.ValueQa).ToList());
                    break;
                case "ValueProd":
                    AppSettings = new BindingList<Setting3Tiers>(AppSettingBacking.OrderBy(x => x.ValueProd).ToList());
                    break;
                default:
                    break;
            }
            dataGridView1.DataSource = AppSettings;
            RefreshGridColours();
        }

        private void textBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                var T = sender as TextBox;
                T.Text = files[0];
            }
        }

        private void textBox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new TipsForm();
            dlg.Show();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!UnsavedChanges || MessageBox.Show("You have unsaved changes. This will discard those. OK?", "Unsaved changes",MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                InitializeLists();
            }
        }

        private void SaveSettings(string path)
        {
           var ComparisonSettings = new ComparisonSettings()
            {
                ClearOnGo = chkClear.Checked,
                DevPath = txtDevPath.Text,
                QaPath = txtQaPath.Text,
                ProdPath = txtProdPath.Text,
                Result = AppSettingBacking,
                SortOrder = SortOrder
            };

            // TODO actualkly do the saving.
        }
    }
}
