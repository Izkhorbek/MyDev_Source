using System;
using System.Collections.Generic;
using System.Text;

namespace LibCommonDef
{
    public class DBSetting
    {
        public string HostName;
        public int Port;
        public string Username;
        public string Password;
        public string Schema; 

        public bool IsOk()
        {
            if (string.IsNullOrEmpty(HostName) || string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Schema)) return false;

            return true;
        }
    }
}
