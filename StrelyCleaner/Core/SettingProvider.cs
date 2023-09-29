using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XylonV2.Core.File;

namespace StrelyCleaner.Core
{
    public class SettingProvider
    {

        [System.Runtime.InteropServices.DllImport("kernel32")]
        static extern int GetPrivateProfileStringA(string lpAppName, string lpKeyName, string lpDefault, System.Text.StringBuilder lpReturnedString, int nSize, string lpFileName);
       
        [System.Runtime.InteropServices.DllImport("kernel32")]
        static extern int WritePrivateProfileStringA(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        public string FileSetting { get; set; }

        public SettingProvider(string IniFile) {
            FileSetting = IniFile;
            if (System.IO.File.Exists(IniFile) == false) { System.IO.File.WriteAllText(IniFile, ""); }
        }

        public string ReadIni(string Section, string Key, string DefaultValue = null)
        {
            System.Text.StringBuilder buffer = new System.Text.StringBuilder(260);
            GetPrivateProfileStringA(Section, Key, DefaultValue, buffer, buffer.Capacity, FileSetting);
            return buffer.ToString();
        }

        public bool WriteIni(string Section, string Key, string Value)
        {
            return (WritePrivateProfileStringA(Section, Key, Value, FileSetting) != 0);
        }

        public bool WriteIniList(string Section, string Key, List<string> values, string Delimiter = ",")
        {
            string joinedValues = string.Join(Delimiter, values);
            return WriteIni(Section, Key, joinedValues);
        }

        public List<string> ReadIniList(string Section, string Key, string Delimiter = ",")
        {
            string joinedValues = ReadIni(Section, Key);
            if (!string.IsNullOrEmpty(joinedValues))
            {
                return joinedValues.Split(Delimiter.ToCharArray().FirstOrDefault()).ToList();
            }
            return new List<string>();
        }
    }
}
