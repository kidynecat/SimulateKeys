using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simulatekeys
{
    public static class sysParam
    {
        public static Dictionary<string,HotkeyData> hotKeyDatas = new Dictionary<string, HotkeyData>();

        //public static Dictionary<string, Dictionary<string, HotkeyData>> dicSaveData = new Dictionary<string, Dictionary<string, HotkeyData>>();

        public static SaveData saveData;
    }

    
    public class SaveData
    {
        public SaveData(string _defaultData, Dictionary<string, Dictionary<string, HotkeyData>> _dicSaveData)
        {
            dicSaveData = _dicSaveData;
            defaultData = _defaultData;
        }

        public string defaultData = "";
        public Dictionary<string, Dictionary<string, HotkeyData>> dicSaveData;
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
        public int endDelay = 100;
        public string hitKeyName;
        public string hitKeyCode;
        public int type = 0;
 
    }
}
