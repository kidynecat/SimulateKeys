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
    public partial class frmLoad : Form
    {
        public frmLoad()
        {
            InitializeComponent();
        }

        public event Action<object, DataLoadEventArgs> DataLoadHandle;

        private void frmLoad_Load(object sender, EventArgs e)
        {

            foreach (var sd in sysParam.saveData.dicSaveData)
            {
                listBox1.Items.Add(sd.Key);
                if(sysParam.saveData.defaultData == sd.Key)
                {
                    listBox1.SelectedItem = sd.Key;
                }
            }

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataLoadHandle?.Invoke(this, new DataLoadEventArgs(listBox1.SelectedItem.ToString()));
            this.Close();
        }

        public class DataLoadEventArgs : EventArgs{
            public DataLoadEventArgs(string dataName)
            {
                DataName = dataName;
            }

            public string DataName { get; set; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedItem == null)
            {
                return;
            }

            sysParam.saveData.dicSaveData.Remove(listBox1.SelectedItem.ToString());

            if(sysParam.saveData.dicSaveData.Count == 0)
            {
                sysParam.saveData.dicSaveData.Add("默认", new Dictionary<string, HotkeyData>());
            }

            listBox1.Items.Clear();
            foreach (var sd in sysParam.saveData.dicSaveData)
            {
                listBox1.Items.Add(sd.Key);
            }
            listBox1.SelectedIndex = 0;
            DataLoadHandle?.Invoke(this, new DataLoadEventArgs(listBox1.SelectedItem.ToString()));
        }
    }
}
