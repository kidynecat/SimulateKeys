using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using Newtonsoft.Json;
using System.IO;

namespace simulatekeys
{
    public partial class Form1 : Form
    {

        Dictionary<string, System.Timers.Timer> TrsPool = new Dictionary<string, System.Timers.Timer>();

        List<int> RegisteredHotKey = new List<int>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button3_Click(this, null);


            //Tr.AutoReset = true;
            //Tr.Interval = 500;
            //Tr.Elapsed += hotkeyhander;

            



        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            clearTrpool();
            cancelHotKey();
        }



        private const int WM_HOTKEY = 0x0312; //窗口消息-热键*
        private const int WM_CREATE = 0x1; //窗口消息-创建
        private const int WM_DESTROY = 0x2; //窗口消息-销毁
        private const int F7 = 100;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case WM_HOTKEY: //窗口消息-热键ID
                    //switch (m.WParam.ToInt32())
                    //{
                    //    case F7: //热键ID
                    //        //MessageBox.Show("我按了F7");
                    //        StartOff();

                    //        break;
                    //    default:
                    //        break;
                    //}
                    startHotKeyHandle(m.WParam.ToInt32());
                    break;
                case WM_CREATE: //窗口消息-创建
                    //string res = AppHotKey.RegKey(Handle, F7, 0, Keys.F7);
                    //Console.WriteLine(res);

                    //Tr.AutoReset = true;
                    //Tr.Interval = 500;
                    //Tr.Elapsed += hotkeyhander;

                    break;
                case WM_DESTROY: //窗口消息-销毁

                    break;
                default:
                    break;
            }
        }


        //public void KeyTaskHander()
        //{

        //}


        const int WM_KEYDOWN = 0x0100;
        const int WM_KEYUP = 0x0101;
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);


        System.Timers.Timer Tr = new System.Timers.Timer();

        //public void StartOff()
        //{
        //    if(Tr.Enabled == false)
        //    {
        //        Tr.Start();
        //    }
        //    else
        //    {
        //        Tr.Stop();
        //    }
        //}

        //private void hotkeyhander(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    //keybd_event(0x61, 0, 0, 0);
        //    //keybd_event(0x61, 0, 2, 0);
        //    //Thread.Sleep(100);
        //    //keybd_event(0x62, 0, 0, 0);
        //    //keybd_event(0x62, 0, 2, 0);
        //    //Thread.Sleep(100);
        //    //keybd_event(0x63, 0, 0, 0);
        //    //keybd_event(0x63, 0, 2, 0);
        //    Thread.Sleep(1000);
        //    keybd_event(0x64, 0, 0, 0);
        //    keybd_event(0x64, 0, 2, 0);
        //}

        //添加按钮
        private void button1_Click(object sender, EventArgs e)
        {
            frmSetKey fs = new frmSetKey();
            fs.SetKeyEventHandle += Fs_SetKeyEventHandle;
            fs.ShowDialog();
            
        }
        //添加完成事件
        private void Fs_SetKeyEventHandle(object arg1, SetKeyEventArgs arg2)
        {
            if (sysParam.hotkeyDatas.Keys.Contains(arg2.keycode))
            {
                MessageBox.Show("按键重复");
                return;
            }

            sysParam.hotkeyDatas.Add(arg2.keycode, new HotkeyData() { hotKeyCode = arg2.keycode, hotKeyName = arg2.keyname, keyHandles = new Dictionary<string, KeyHandle>() });

            AddHotKey(arg2.keycode,arg2.keyname);

            //Console.WriteLine(sysParam.hotkeyDatas);
        }

        //添加启动热键
        private void AddHotKey(string code,string name)
        {
            Label lb = new Label();
            lb.Text = name + "/" + code;
            lb.Location = new Point(10, 20);

            Button b2 = new Button();
            b2.Text = "添加处理";
            b2.Click += AddKeyHandleClick;
            b2.Width = 100;
            b2.Height = 30;
            b2.Name = code;
            b2.Location = new Point(460, 10);

            Button b3 = new Button();
            b3.Text = "删除热键";
            b3.Click += DeletKeyHandleClick;
            b3.Width = 100;
            b3.Height = 30;
            b3.Location = new Point(10, 50);

            FlowLayoutPanel flp = new FlowLayoutPanel();
            flp.Width = 300;
            flp.Location = new Point(130, 0);
            flp.Height = 120;
            flp.BackColor = Color.White;
            flp.Name = "flp" + code;
            flp.AutoScroll = true;

            Panel p = new Panel();
            p.Name = "hotkey" + code;
            p.Height = 100;
            p.Controls.Add(lb);
            p.Controls.Add(b2);
            p.Controls.Add(b3);
            p.Controls.Add(flp);
            p.Width = flowLayoutPanel1.Width - 25;
            p.BorderStyle = BorderStyle.FixedSingle;

            this.flowLayoutPanel1.Controls.Add(p);
        }

        string nowHotKeyCode = "";
        private void AddKeyHandleClick(object sender, EventArgs e)
        {
            this.nowHotKeyCode = (sender as Button).Name;
            frmSetKey fs = new frmSetKey();
            fs.SetKeyEventHandle += b2_SetKeyEventHandle;
            fs.ShowDialog();
        }

        private void DeletKeyHandleClick(object sender, EventArgs e)
        {
            string handlekeycode = (sender as Button).Parent.Name.Substring(6);

            sysParam.hotkeyDatas.Remove(handlekeycode);

            (sender as Button).Parent.Parent.Controls.Remove((sender as Button).Parent);
        }

        private void b2_SetKeyEventHandle(object arg1, SetKeyEventArgs arg2)
        {
            var hotkey = sysParam.hotkeyDatas[nowHotKeyCode];
            if(hotkey.keyHandles.ContainsKey(arg2.keycode))
            {
                MessageBox.Show("Sorry不能重复添加");
                return;
            }
            hotkey.keyHandles.Add(arg2.keycode, new KeyHandle() {hitKeyCode = arg2.keycode,hitKeyName = arg2.keyname});
            AddHandleKey(nowHotKeyCode, arg2.keycode, arg2.keyname,"0","100","1000");

        }

        //添加热键对应执行
        private void AddHandleKey(string HotKeyCode,string HandleKeyCode ,string HandleKeyName,string StartDelay,string EndDelay,string Interval)
            
        {
            var flp = flowLayoutPanel1.Controls.Find("flp" + HotKeyCode, true)[0];

            Label lb = new Label();
            lb.Text = HandleKeyName + "/" + HandleKeyCode;
            lb.Location = new Point(0, 3);

            TextBox tb1 = new TextBox();
            tb1.Width = 45;
            tb1.Height = 30;
            tb1.KeyPress += textNumberKeyPress;
            tb1.Text = StartDelay;
            tb1.Font = new Font("宋体", 12f);
            tb1.Location = new Point(50, 0);
            tb1.TextChanged += startIChange;

            TextBox tb2 = new TextBox();
            tb2.Width = 45;
            tb2.Height = 30;
            tb2.KeyPress += textNumberKeyPress;
            tb2.Text = EndDelay;
            tb2.Font = new Font("宋体", 12f);
            tb2.Location = new Point(100, 0);
            tb2.TextChanged += endIChange;


            TextBox tb3 = new TextBox();
            tb3.Width = 45;
            tb3.Height = 30;
            tb3.KeyPress += textNumberKeyPress;
            tb3.Text = Interval;
            tb3.Font = new Font("宋体", 12f);
            tb3.Location = new Point(150, 0);
            tb3.TextChanged += IntervalChange;

            Button btn1 = new Button();
            btn1.Width = 50;
            btn1.Height = 28;
            btn1.Text = "X";
            btn1.Location = new Point(220);
            btn1.Click += deleteHandleKeyClick;

            Panel p = new Panel();
            p.Name = "H" + HandleKeyCode;
            p.BackColor = Color.AliceBlue;
            p.Width = 280;
            p.Height = 28;
            p.Controls.Add(tb1);
            p.Controls.Add(lb);
            p.Controls.Add(tb2);
            p.Controls.Add(tb3);
            p.Controls.Add(btn1);

            (flp as FlowLayoutPanel).Controls.Add(p);
        }

        private void deleteHandleKeyClick(object sender, EventArgs e)
        {
            string handlekeycode = (sender as Button).Parent.Name.Substring(1);
            string hotkeycode = (sender as Button).Parent.Parent.Name.Substring(3);

            sysParam.hotkeyDatas[hotkeycode].keyHandles.Remove(handlekeycode);

            (sender as Button).Parent.Parent.Controls.Remove((sender as Button).Parent);
        }

        private void startIChange(object sender, EventArgs e)
        {
            string handlekeycode = (sender as TextBox).Parent.Name.Substring(1);
            string hotkeycode = (sender as TextBox).Parent.Parent.Name.Substring(3);


            int v = 0;
            if( int.TryParse((sender as TextBox).Text,out v))
            {
                sysParam.hotkeyDatas[hotkeycode].keyHandles[handlekeycode].startDelay = v;
            }
            else
            {
                sysParam.hotkeyDatas[hotkeycode].keyHandles[handlekeycode].startDelay = 0;
               (sender as TextBox).Text = "0";
            }

            
        }

        private void endIChange(object sender, EventArgs e)
        {
            string handlekeycode = (sender as TextBox).Parent.Name.Substring(1);
            string hotkeycode = (sender as TextBox).Parent.Parent.Name.Substring(3);

            int v = 0;
            if (int.TryParse((sender as TextBox).Text, out v))
            {
                sysParam.hotkeyDatas[hotkeycode].keyHandles[handlekeycode].endDelay = v;
            }
            else
            {
                sysParam.hotkeyDatas[hotkeycode].keyHandles[handlekeycode].endDelay = 0;
               (sender as TextBox).Text = "0";
            }
        }


        private void IntervalChange(object sender, EventArgs e)
        {
            string handlekeycode = (sender as TextBox).Parent.Name.Substring(1);
            string hotkeycode = (sender as TextBox).Parent.Parent.Name.Substring(3);

            int v = 0;
            if (int.TryParse((sender as TextBox).Text, out v))
            {
                sysParam.hotkeyDatas[hotkeycode].keyHandles[handlekeycode].Interval = v;
            }
            else
            {
                sysParam.hotkeyDatas[hotkeycode].keyHandles[handlekeycode].Interval = 0;
                (sender as TextBox).Text = "0";
            }
        }

        private void textNumberKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//这是允许输入退格键  
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9'))//这是允许输入0-9数字  
                {
                    e.Handled = true;
                }
            }
        }

        string keydatastring = "";

        //保存
        private void button2_Click(object sender, EventArgs e)
        {
            //Console.WriteLine(sysParam.hotkeyDatas);

            string data = JsonConvert.SerializeObject(sysParam.hotkeyDatas);
            keydatastring = data;
            Console.WriteLine(data);

            var path = System.AppDomain.CurrentDomain.BaseDirectory;
            cancelHotKey();
            savefile(keydatastring, path, "sav");
            saveTrpool();
            registerHotKey();

        }

        //加载
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "sav.txt"))
                {
                    keydatastring = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\sav.txt");
                }
                else
                {
                    keydatastring = "";
                }

                sysParam.hotkeyDatas = JsonConvert.DeserializeObject<Dictionary<string, HotkeyData>>(keydatastring);
            }
            catch
            {
                sysParam.hotkeyDatas = new Dictionary<string, HotkeyData>();
            }
            cancelHotKey();
            reflashKeydatas();
            saveTrpool();
            registerHotKey();

        }

        //保存文件
        public static void savefile(string data, string FilePatch, string FileName)
        {
            //string path = @"D:\Logs";

            if (!Directory.Exists(FilePatch))
                Directory.CreateDirectory(FilePatch);

            string fileFullPath = FilePatch + "\\"  + FileName + ".txt";
            StringBuilder str = new StringBuilder();
            str.Append(data);
           
            StreamWriter sw;
            if (!File.Exists(fileFullPath))
            {
                sw = File.CreateText(fileFullPath);
            }
            else
            {
                File.Delete(fileFullPath);
                sw = File.CreateText(fileFullPath);
            }
            sw.WriteLine(str.ToString());
            sw.Close();
        }

        //保存TrsPool
        public void saveTrpool()
        {
            clearTrpool();

            Dictionary<string, List<KeyHandle>> dictrhd = new Dictionary<string, List<KeyHandle>>();

            foreach (var hk in sysParam.hotkeyDatas)
            {
                string hkname = hk.Key;

                foreach (var khandle in hk.Value.keyHandles)
                {
                    var inv = khandle.Value.Interval;

                    if (!TrsPool.ContainsKey(hkname + "|" + inv))
                    {
                        System.Timers.Timer tr;
                        tr = new System.Timers.Timer();

                        tr.Interval = khandle.Value.Interval;
                        TrsPool.Add(hkname + "|" + inv, tr);


                        var x = new List<KeyHandle>();
                        x.Add(khandle.Value);
                        dictrhd.Add(hkname + "|" + inv, x);

                    }
                    else
                    {
                        dictrhd[hkname + "|" + inv].Add(khandle.Value);

                    }

                }
            }

            foreach (var khk in dictrhd.Keys)
            {

                TrsPool[khk].Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
                {


                    foreach (var kn in dictrhd[khk])
                    {
                        if (kn.startDelay > 0)
                        {
                            Thread.Sleep(kn.startDelay);
                        }

                        keybd_event(byte.Parse(kn.hitKeyCode), 0, 0, 0);
                        keybd_event(byte.Parse(kn.hitKeyCode), 0, 2, 0);
                        if (kn.endDelay > 0)
                        {
                            Thread.Sleep(kn.endDelay);
                        }
                    }
                };
            }


        }

        //清空Trpool
        private void clearTrpool()
        {
            foreach (var tr in TrsPool.Values)
            {
                tr.Stop();
                tr.Dispose();
            }

            TrsPool.Clear();
        }

        //刷新热键数据
        private void reflashKeydatas()
        {

            flowLayoutPanel1.Controls.Clear();
            
            foreach(var hkd in sysParam.hotkeyDatas.Values )
            {
                AddHotKey(hkd.hotKeyCode, hkd.hotKeyName);
                foreach (var hk in hkd.keyHandles.Values)
                {
                    AddHandleKey(hkd.hotKeyCode, hk.hitKeyCode, hk.hitKeyName,hk.startDelay.ToString(),hk.endDelay.ToString(),hk.Interval.ToString());
                }
            }

        }

        //注册热键
        private void registerHotKey()
        {
            RegisteredHotKey.Clear();
            foreach (var hotk in sysParam.hotkeyDatas)
            {
                string res = AppHotKey.RegKey(Handle, int.Parse( hotk.Value.hotKeyCode), 0, (Keys)int.Parse(hotk.Value.hotKeyCode));
                Console.WriteLine(res);
                RegisteredHotKey.Add(int.Parse(hotk.Value.hotKeyCode));
            }
        }

        //注销热键
        private void cancelHotKey()
        {
            foreach (var hotk in RegisteredHotKey)
            {
                AppHotKey.UnRegKey(Handle, hotk);
            }
        }


        //触发热键
        private void startHotKeyHandle(int hkv)
        {
            foreach(var tr in TrsPool)
            {
                if(tr.Key.StartsWith(hkv.ToString() + "|"))
                {
                    if (tr.Value.Enabled == false)
                    {
                        tr.Value.Start();
                    }
                    else
                    {
                        tr.Value.Stop();
                    }
                }
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            foreach(var t in TrsPool.Values)
            {
                t.Start();
            }
        }

        private void SKT_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                //还原窗体显示    
                WindowState = FormWindowState.Normal;
                //激活窗体并给予它焦点
                this.Activate();
                //任务栏区显示图标
                this.ShowInTaskbar = true;
                //托盘区图标隐藏
                SKT.Visible = false;
            }
            cancelHotKey();
            registerHotKey();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            //判断是否选择的是最小化按钮
            if (WindowState == FormWindowState.Minimized)
            {
                //隐藏任务栏区图标
                this.ShowInTaskbar = false;
                //图标显示在托盘区
                SKT.Visible = true;
            }
            cancelHotKey();
            registerHotKey();
        }
    }
}
