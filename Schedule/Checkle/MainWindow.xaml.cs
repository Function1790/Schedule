using System;
using System.Collections.Generic;
using System.IO;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Checkle
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        Line[] Day = new Line[5];
        const int Block_Size = 60;
        const string File_Name = "Data.json";
        stringArr[] Base_Lesson = new stringArr[5];

        class stringArr
        {
            public string[] text;
            public stringArr(string[] t)
            {
                text = t;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Log("OnLoad", "Entry");
            for (int i = 0; i < Day.Length; i++)
            {
                Day[i] = new Line();
            }
            Day[getDate()].today = true;
            Day[0].lesson = new string[] { "국어", "체육", "역사", "기가", "S.C.", "미술", "미술" };
            Day[1].lesson = new string[] { "과학", "영어", "체육", "기가", "수학", "음악", "" };
            Day[2].lesson = new string[] { "영어", "사회", "수학", "과학", "국어", "창체", "창체" };
            Day[3].lesson = new string[] { "과학", "수학", "영어", "역사", "국어", "체육", "진로" };
            Day[4].lesson = new string[] { "영어", "도덕", "기가", "국어", "사회", "과학", "" };
            for (int i = 0; i < Day.Length; i++)
            {
                Base_Lesson[i] = new stringArr(Day[i].lesson);
                Day[i].Set(World, new Point((Block_Size + 5) * i + 40, 20));
            }
            Load();
            Log("OnLoad", "Finish");
        }

        public static void Log(string title, object content)
        {
            Console.WriteLine($"[{title}] : {content}");
        }

        class Line
        {
            public Block[] blocks;
            public string[] lesson;
            public bool today = false;

            public void Set(Grid grid, Point c)
            {
                //this.lesson = lesson;
                blocks = new Block[this.lesson.Length];
                for(int i=0; i< this.lesson.Length; i++)
                {
                    blocks[i] = new Block();
                    blocks[i].is_today = today;
                    blocks[i].Text = lesson[i];
                    blocks[i].HorizontalAlignment = HorizontalAlignment.Left;
                    blocks[i].VerticalAlignment = VerticalAlignment.Top;
                    blocks[i].Width = Block_Size;
                    blocks[i].Height = Block_Size;
                    blocks[i].HideBox(true);
                    blocks[i].Margin = new Thickness(c.X, i * (Block_Size + 2) + c.Y, 0, 0);
                    grid.Children.Add(blocks[i]);
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            Log("MainWindow", "Start Program");
        }

        int getDate()
        {
            DateTime nowDt = DateTime.Now;

            if (nowDt.DayOfWeek == DayOfWeek.Monday)
                return 0;
            else if (nowDt.DayOfWeek == DayOfWeek.Tuesday)
                return 1;
            else if (nowDt.DayOfWeek == DayOfWeek.Wednesday)
                return 2;
            else if (nowDt.DayOfWeek == DayOfWeek.Thursday)
                return 3;
            else if (nowDt.DayOfWeek == DayOfWeek.Friday)
                return 4;
            Log("getDate", "Err -> WeekEnd");
            return 0;
        }

        void opacAt(FrameworkElement ctr, double to, double sec)
        {
            DoubleAnimation an = new DoubleAnimation();
            an.From = ctr.Opacity;
            an.To = to;
            an.Duration = new Duration(TimeSpan.FromSeconds(sec));
            ctr.BeginAnimation(OpacityProperty, an);
        }

        void opacAt(FrameworkElement ctr, double from, double to, double sec)
        {
            DoubleAnimation an = new DoubleAnimation();
            an.From = from;
            an.To = to;
            an.Duration = new Duration(TimeSpan.FromSeconds(sec));
            ctr.BeginAnimation(OpacityProperty, an);
        }

        void topTo(FrameworkElement ctr, double from, double to, double sec)
        {
            ThicknessAnimation an = new ThicknessAnimation();
            an.From = new Thickness(ctr.Margin.Left, from, 0, 0);
            an.To = new Thickness(ctr.Margin.Left, to, 0, 0);
            an.Duration = new Duration(TimeSpan.FromSeconds(sec));
            ctr.BeginAnimation(MarginProperty, an);
        }

        void leftTo(FrameworkElement ctr, double from, double to, double sec)
        {
            ThicknessAnimation an = new ThicknessAnimation();
            an.From = new Thickness(from, ctr.Margin.Top, 0, 0);
            an.To = new Thickness(to, ctr.Margin.Top, 0, 0);
            an.Duration = new Duration(TimeSpan.FromSeconds(sec));
            ctr.BeginAnimation(MarginProperty, an);
        }

        private void TopPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseRect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Log("MainWindow", "Stop Program");
                Save();
                Close();
            }
        }

        private void CloseRect_MouseEnter(object sender, MouseEventArgs e)
        {
            opacAt(CloseRect, 0.15, 0.25);
        }

        private void CloseRect_MouseLeave(object sender, MouseEventArgs e)
        {
            opacAt(CloseRect, 0.05, 0.25);
        }

        void Save()
        {
            Log("Save", "Entry");
            JObject json = new JObject();
            json.Add("Position", JObject.FromObject(new { x = Left, y = Top }));
            for (int i=0; i<Day.Length; i++)
            {
                JObject day = new JObject();
                for(int j=0; j<Day[i].blocks.Length; j++)
                {
                    day.Add("Lesson" + j, JObject.FromObject(new
                    {
                        name = Day[i].blocks[j].Text,
                        finish = Day[i].blocks[j].is_finish,
                        yet = Day[i].blocks[j].is_yet
                    }));
                }
                json.Add("Day" + i, day);
            }
            File.WriteAllText(File_Name, json.ToString());
            Log("Save", "Complete");
        }

        void Load()
        {
            Log("Load", "Entry");
            JObject json = JObject.Parse(File.ReadAllText(File_Name));
            Left = Convert.ToDouble(json["Position"]["x"]);
            Top = Convert.ToDouble(json["Position"]["y"]);
            for (int i = 0; i < Day.Length; i++)
            {
                for (int j = 0; j < Day[i].blocks.Length; j++)
                {
                    Day[i].blocks[j].Text = json["Day" + i]["Lesson" + j]["name"].ToString();
                    if(Convert.ToBoolean(json["Day" + i]["Lesson" + j]["finish"]))
                    {
                        Day[i].blocks[j].setFinish();
                    }
                    if( Convert.ToBoolean(json["Day" + i]["Lesson" + j]["yet"]))
                    {
                        Day[i].blocks[j].setYet();
                    }
                }
            }
            Log("Load", "Complete");
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ResetBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Log("Button", "Reset");
            for (int i = 0; i < Day.Length; i++)
            {
                Day[i].lesson = Base_Lesson[i].text;
                for(int j=0; j<Day[i].blocks.Length; j++)
                {
                    Day[i].blocks[j].Text = Base_Lesson[i].text[j];
                }
            }
        }

        private void ResetBtn2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Log("Button", "Reset2");
            for (int i = 0; i < Day.Length; i++)
            {
                for (int j = 0; j < Day[i].blocks.Length; j++)
                {
                    Day[i].blocks[j].is_finish = false;
                    Day[i].blocks[j].is_yet = false;
                    Day[i].blocks[j].setBaseU();
                }
            }
        }
    }
}
