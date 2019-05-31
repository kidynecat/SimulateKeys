using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simulatekeys
{
    public partial class frmSetKey : Form
    {

        public event Action<object, SetKeyEventArgs> SetKeyEventHandle;
        

        public frmSetKey()
        {
            InitializeComponent();
        }

        private void frmSetKey_KeyDown(object sender, KeyEventArgs e)
        {
            string str = e.KeyCode.ToString();
            int code = e.KeyValue;
            //MessageBox.Show(str + "------" + code.ToString());

            SetKeyEventHandle?.Invoke(this, new SetKeyEventArgs() { keycode = code.ToString(), keyname = str });
            this.Close();
        }

        private void frmSetKey_Load(object sender, EventArgs e)
        {

        }

        

    }

    public class SetKeyEventArgs:EventArgs
    {
        public string keycode
        {
            get;
            set;
        } = "";
        public string keyname
        {
            get;
            set;
        } = "";
    }
}
