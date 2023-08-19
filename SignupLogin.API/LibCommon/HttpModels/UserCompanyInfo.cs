using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibCommon.HttpModels
{
    public class UserCompanyInfo :CompanyInfo
    {
        public UserCompanyInfo() { }

        public UserCompanyInfo(UserCompanyInfo other)
        {
            Copy(other);
        }
    }

}
