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
    public partial class frmAddNew : Form
    {
        public frmAddNew()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "")
            {
                MessageBox.Show("请输入名称");
                return;
            }
            if(sysParam.saveData.dicSaveData.ContainsKey(textBox1.Text))
            {
                MessageBox.Show("名称已存在");
                return;
            }

            sysParam.saveData.dicSaveData.Add(textBox1.Text,new Dictionary<string, HotkeyData>());
            sysParam.hotKeyDatas = sysParam.saveData.dicSaveData[textBox1.Text];
            this.Close();
            (this.Owner as frmMain).label1.Text = textBox1.Text;

        }


    }
}
