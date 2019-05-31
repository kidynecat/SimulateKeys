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

namespace simulatekeys
{
    public partial class Form1 : Form
    {

      

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Tr.AutoReset = true;
            Tr.Interval = 500;
            Tr.Elapsed += hotkeyhander;
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
                    switch (m.WParam.ToInt32())
                    {
                        case F7: //热键ID
                            MessageBox.Show("我按了F7");
                            StartOff();
                            //keybd_event(0x61, 0, 0, 0);
                            //keybd_event(0x61, 0, 2, 0);
                            break;
                        default:
                            break;
                    }
                    break;
                case WM_CREATE: //窗口消息-创建
                    string res = AppHotKey.RegKey(Handle, F7, 0, Keys.F7);
                    Console.WriteLine(res);

                    Tr.AutoReset = true;
                    Tr.Interval = 500;
                    Tr.Elapsed += hotkeyhander;

                    break;
                case WM_DESTROY: //窗口消息-销毁
                    AppHotKey.UnRegKey(Handle, F7); //销毁热键
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

        public void StartOff()
        {
            if(Tr.Enabled == false)
            {
                Tr.Start();
            }
            else
            {
                Tr.Stop();
            }
        }

        private void hotkeyhander(object sender, System.Timers.ElapsedEventArgs e)
        {
            keybd_event(0x61, 0, 0, 0);
            keybd_event(0x61, 0, 2, 0);
            Thread.Sleep(100);
            keybd_event(0x62, 0, 0, 0);
            keybd_event(0x62, 0, 2, 0);
            Thread.Sleep(100);
            keybd_event(0x63, 0, 0, 0);
            keybd_event(0x63, 0, 2, 0);
            Thread.Sleep(100);
            keybd_event(0x64, 0, 0, 0);
            keybd_event(0x64, 0, 2, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmSetKey fs = new frmSetKey();
            fs.SetKeyEventHandle += Fs_SetKeyEventHandle;
            fs.ShowDialog();
            
        }

        private void Fs_SetKeyEventHandle(object arg1, SetKeyEventArgs arg2)
        {
            MessageBox.Show(arg2.keycode + arg2.keyname);
        }
    }
}
