using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simulatekeys
{
    public static class sysParam
    {
        public static Dictionary<string,hotkeyData> hotkeyDatas = new Dictionary<string, hotkeyData>();
    }


    public class hotkeyData
    {
        public string hotKeyCode;
        public string hotKeyName;
        public Dictionary<string,keyHandle> keyHandles;
    }

    public class keyHandle
    {
        public int startInterval = 0;
        public int endInterval = 0;
        public string hitKeyName;
        public string hitKeyCode;
 
    }
}
