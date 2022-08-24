using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Jpsys.SagyoManage.Boot
{
    public partial class LoadingFrame : Form
    {
        public LoadingFrame()
        {
            InitializeComponent();

            //2014/01/23 UEYAMA
            //this.pictureBox1.Image =
            //    global::Sunegg.FeedOrderManage.Framework.Boot.Properties.Resources.AppLogo;

        }

        public void DrowLoadingText(string text)
        {
            this.lblLoadingText.Text = text;
            this.Refresh();
        }
    }
}
