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
    /// Block.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Block : UserControl
    {
        public bool is_finish = false;
        public bool is_yet = false;
        public bool today = false;
        Color c_finish = Color.FromRgb(125, 165, 199);
        Color c_today = Color.FromRgb(222, 228, 95);
        Color c_base = Color.FromRgb(116, 116, 116);
        Color c_yet = Color.FromRgb(236, 113, 113);

        public bool is_today
        {
            get
            {
                return today;
            }
            set
            {
                if (value)
                {
                    rectC(rect, c_today, 0.25);
                    opacAt(rect, 0.1, 0.25);
                }
                today = value;
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

        void rectC(Rectangle rt, Color to, double sec)
        {
            ColorAnimation an = new ColorAnimation();
            an.From = (rt.Fill as SolidColorBrush).Color;
            an.To = to;
            an.Duration = new Duration(TimeSpan.FromSeconds(sec));
            rt.Fill.BeginAnimation(SolidColorBrush.ColorProperty, an);
        }

        public Block()
        {
            InitializeComponent();
        }

        public string Text
        {
            get
            {
                return text.Text;
            }
            set
            {
                text.Text = value;
                box.Text = value;
            }
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!is_finish && !is_yet)
            {
                opacAt(rect, 0.25, 0.25);
            }
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!is_finish && !is_yet)
            {
                opacAt(rect, 0, 0.25);
                if (today)
                {
                    rectC(rect, c_today, 0.25);
                    opacAt(rect, 0.1, 0.25);
                }
            }
        }

        public void HideBox(bool b)
        {
            if (!b)
            {
                box.Visibility = Visibility.Visible;
                text.Visibility = Visibility.Hidden;
            }
            else
            {
                text.Visibility = Visibility.Visible;
                box.Visibility = Visibility.Hidden;
            }
        }

        public void LostFocusBox()
        {
            HideBox(true);
            Text = box.Text;
        }

        public void setFinish()
        {
            rectC(rect, c_finish, 0.25);
            opacAt(rect, 1, 0.25);
            is_finish = true;
        }
        
        public void setYet()
        {
            rectC(rect, c_yet, 0.25);
            opacAt(rect, 1, 0.25);
            is_yet = true;
        }

        public void setBase()
        {
            if (is_today)
                rectC(rect, c_today, 0.25);
            else
                rectC(rect, c_base, 0.25);
            opacAt(rect, 0.25, 0.25);
        }

        public void setBaseU()
        {
            if (is_today)
            {
                rectC(rect, c_today, 0.25);
                opacAt(rect, 0.1, 0.25);
            }
            else
            {
                rectC(rect, c_base, 0.25);
                opacAt(rect, 0, 0.25);
            }
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                if (!box.IsFocused)
                {
                    HideBox(false);
                    box.Text = Text;
                    box.Focus();
                }
                else
                {
                    LostFocusBox();
                }
            }
            else
            {
                LostFocusBox();
            }
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!is_finish)
                {
                    rectC(rect, c_finish, 0.25);
                    opacAt(rect, 1, 0.25);
                    is_finish = true;
                }
                else
                {
                    is_finish = false;
                    setBase();
                }
                is_yet = false;
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (!is_yet)
                {
                    rectC(rect, c_yet, 0.25);
                    opacAt(rect, 1, 0.25);
                    is_yet = true;
                }
                else
                {
                    is_yet = false;
                    setBase();
                }
                is_finish = false;
            }
        }

        private void box_LostFocus(object sender, RoutedEventArgs e)
        {
            LostFocusBox();
        }

        private void box_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter || e.Key == Key.Return)
            {
                LostFocusBox();
            }
        }
    }
}
