using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simulatekeys
{
    public static class sysParam
    {
        public static Dictionary<string,HotkeyData> hotkeyDatas = new Dictionary<string, HotkeyData>();
    }


    public class HotkeyData
    {
        public string hotKeyCode;
        public string hotKeyName;
        public Dictionary<string,KeyHandle> keyHandles;
    }


    public class KeyHandle
    {
        public int Interval = 1000;
        public int startDelay = 0;
        public int endDelay = 0;
        public string hitKeyName;
        public string hitKeyCode;
        public int type = 0;
 
    }
}
