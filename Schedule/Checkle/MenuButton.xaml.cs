using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Checkle
{
    /// <summary>
    /// MenuButton.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MenuButton : UserControl
    {
        public MenuButton()
        {
            InitializeComponent();
        }

        public string Text
        {
            get
            {
                return tb.Text;
            }
            set
            {
                tb.Text = value;
            }
        }

        void opacAt(FrameworkElement ctr, double to, double sec)
        {
            DoubleAnimation an = new DoubleAnimation();
            an.From = ctr.Opacity;
            an.To = to;
            an.Duration = new Duration(TimeSpan.FromSeconds(sec));
            ctr.BeginAnimation(OpacityProperty, an);
        }

        private void tb_MouseEnter(object sender, MouseEventArgs e)
        {
            opacAt(tb, 1, 0.25);
        }

        private void tb_MouseLeave(object sender, MouseEventArgs e)
        {
            opacAt(tb, 0.5, 0.25);
        }
    }
}
