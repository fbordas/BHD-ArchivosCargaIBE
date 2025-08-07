using System;
using System.Windows.Forms;

namespace MassTemplateGenerator
{
    public partial class WndAbout : Form
    {
        public WndAbout()
        {
            InitializeComponent();
            string initYear = "2021", curYear = DateTime.Now.Year.ToString();
            string copyright = 
                initYear == curYear ? initYear : (initYear + "-" + curYear);
            lblText.Text = lblText.Text.Replace("###", copyright)
                .Replace("$$$", typeof(WndMain).Assembly.GetName().Version.ToString());
            btnClose.Focus();
        }

        private void BtnClose_Click(object sender, EventArgs e) { Close(); }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { System.Diagnostics.Process.Start("https://www.flaticon.com/authors/andy-horvath"); }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { System.Diagnostics.Process.Start("https://www.flaticon.com"); }

        private void Controls_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
            //{ Close(); }
            switch (e.KeyCode)
            {
                case Keys.Escape:
                case Keys.Enter:
                case Keys.Space:
                    Close(); break;
                default: break;
            }
        }
    }
}
