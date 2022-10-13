using System.Windows.Forms;

using static Comparatively.Helpers;

namespace Comparatively
{
    public partial class TipsForm : Form
    {
        public TipsForm()
        {
            InitializeComponent();
            bool show = true;
            if ( bool.TryParse(GetAppSetting("ShowTipsAtStartup"), out show))
            {
                ShowTips.Checked = show;
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            SetAppSetting("ShowTipsAtStartup", ShowTips.Checked.ToString());
            Close();
        }
    }
}