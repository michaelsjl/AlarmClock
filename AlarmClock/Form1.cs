using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlarmClock
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private DateTime _setClockTime;
        private string _messageText;

        private void Form1_Load(object sender, EventArgs e)
        {
            _messageText = "hello world!";
            this.button1.Text = "开启";
            //控制日期或时间的显示格式
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            //使用自定义格式
            this.dateTimePicker1.Format = DateTimePickerFormat.Custom;
            //时间控件的启用
            this.dateTimePicker1.ShowUpDown = true;

            timer1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            showMessageForm();
            if (JudgeAlarmIsOk()==false)
            {
                return;
            }

            if (this.button1.Text == "开启")
            {
                this.button1.Text = "暂停";
                StartClock();
            }
            else
            {
                this.button1.Text = "开启";
                StopClock();
            }

            DateTime clock = this.dateTimePicker1.Value;        
        }

        private bool JudgeAlarmIsOk()
        {
            bool res = true;
            int time = DateTime.Compare(DateTime.Now, this.dateTimePicker1.Value);
            if(time>0)
            {
                res = false;
            }

            return res;
        }

        private void StartClock()
        {
            _setClockTime = this.dateTimePicker1.Value;
            _messageText = this.textBox1.Text;
            timer1.Interval = 1000;
            timer1.Enabled = true;

            this.textBox1.Visible = false;
            this.dateTimePicker1.Visible = false;
        }

        private void StopClock()
        {
            timer1.Enabled = false;

            this.textBox1.Visible = true;
            this.dateTimePicker1.Visible = true;
        }

        private void showMessageForm()
        {
            Form2 frmShowWarning = new Form2(_messageText);//要弹出的窗体（提示框），
            Point p = new Point(Screen.PrimaryScreen.WorkingArea.Width - frmShowWarning.Width, Screen.PrimaryScreen.WorkingArea.Height);
            frmShowWarning.PointToScreen(p);
            frmShowWarning.Location = p;
            frmShowWarning.TopMost = true;
            frmShowWarning.Show();
            for (int i = 0; i <= frmShowWarning.Height; i++)
            {
                frmShowWarning.Location = new Point(p.X, p.Y - i);
                Thread.Sleep(10);//将线程沉睡时间调的越小升起的越快
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        { 
            int a = DateDiffInt(DateTime.Now, _setClockTime);
            this.warnLabel.Text = a.ToString();
            if (a == 0)
            {
                StopClock();
                Task.Factory.StartNew(() =>
                {
                    this.Invoke((EventHandler)delegate 
                    {
                        showMessageForm();
                    });       
                });             
                this.button1.Text = "开启";
            }
        }

        private string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            try
            {
                TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                dateDiff = ts.Days.ToString() + "天"
                + ts.Hours.ToString() + "小时"
                + ts.Minutes.ToString() + "分钟"
                + ts.Seconds.ToString() + "秒";
            }
            catch
            {
            }
            return dateDiff;
        }

        private int DateDiffInt(DateTime DateTime1, DateTime DateTime2)
        {
            int dateDiff = 0;
            try
            {
                TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                dateDiff = ts.Days * 86400
                + ts.Hours * 3600
                + ts.Minutes * 60
                + ts.Seconds;
            }
            catch
            {
            }
            return dateDiff;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)//当用户点击窗体右上角X按钮或(Alt + F4)时 发生          
            {
                e.Cancel = true;
                this.ShowInTaskbar = false;
                this.myIcon.Icon = this.Icon;
                this.Hide();
            }
        }

        private void myIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point p = MousePosition;
                this.myMenu.Show(p);
            }

            if (e.Button == MouseButtons.Left)
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void myMenu_MouseClick(object sender, MouseEventArgs e)
        {
            Application.Exit();
        }

        //////////////////////////////////////////
        /*private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            for (int i = 0; i <= this.Height; i++)
            {
                Point p = new Point(this.Location.X, this.Location.Y + i);//弹出框向下移动消失
                this.PointToScreen(p);//即时转换成屏幕坐标
                this.Location = p;// new Point(this.Location.X, this.Location.Y + i);
                System.Threading.Thread.Sleep(10);//线程睡眠时间调的越小向下消失的速度越快。

            }
            this.Close();//记得关闭此弹出框哦。OK
        }*/

        /*private void dou()
        {
            Random ran = new Random((int)DateTime.Now.Ticks);

            Point point = this.Location;

            for (int i = 0; i < 40; i++)
            {
                this.Location = new Point(point.X + ran.Next(8) - 4, point.Y + ran.Next(8) - 4);

                System.Threading.Thread.Sleep(15);

                this.Location = point;

                System.Threading.Thread.Sleep(15);
            }
        }*/

    }
}
