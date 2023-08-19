using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibCommon.HttpModels
{
    public static class CheckBaseCompanyInfo
    {
        public static string CheckNull(this string strItem)
        {
            return strItem == null ? "" : strItem;
        }
    }

    public class BaseCompanyInfo
    {
        public int id { get; set; }
        public string username { get; set; } = "";

        /// <summary>
        /// 1 = yes, 0 = no
        /// </summary>
        public string company_name { get; set; } = "";
        public string document { get; set; } = "";
        public int document_index { get; set; } = 0;
        public string address_of_company { get; set; } = "";
        public string account_number { get; set; } = "";
        public string stir_of_company { get; set; } = "";
        public string name_of_bank { get; set; } = "";
        public string bank_code { get; set; } = "";
        /// <summary>
        /// 1 = yes, 0 = no
        /// </summary>
        public int are_you_qqs_payer { get; set; } = 0;
        public string qqs_number { get; set; } = "";
        public string company_phone_number { get; set; } = "";
        public string position_of_signer { get; set; } = "";
        public int position_of_signer_index { get; set; } = 0;
        public string name_of_signer { get; set; } = "";
        /// <summary>
        /// 1 = yes, 0 = no
        /// </summary>
        public int is_accountant_provided { get; set; } = 0;
        public string accountant_name { get; set; } = "";
        /// <summary>
        /// 1 = yes, 0 = no
        /// </summary>
        public int is_legal_counsel_provided { get; set; } = 0;
        public string counsel_name { get; set; } = "";
    }

    public class CompanyInfo : BaseCompanyInfo
    {
        public string company_logo_url { get; set; } = "";
        public string created_date { get; set; } = "";

        public CompanyInfo()
        {

        }

        public CompanyInfo(CompanyInfo other)
        {
            this.Copy(other);
        }

        public void Copy(CompanyInfo other)
        {
            id = other.id;
            username = other.username;
            company_name = other.company_name;
            document = other.document;
            document_index = other.document_index;
            address_of_company = other.address_of_company;
            account_number = other.account_number;
            stir_of_company = other.stir_of_company;
            name_of_bank = other.name_of_bank;
            bank_code = other.bank_code;
            are_you_qqs_payer = other.are_you_qqs_payer;
            qqs_number = other.qqs_number;
            company_phone_number = other.company_phone_number;
            position_of_signer = other.position_of_signer;
            position_of_signer_index = other.position_of_signer_index;
            name_of_signer = other.name_of_signer;
            is_accountant_provided = other.is_accountant_provided;
            accountant_name = other.accountant_name;
            is_legal_counsel_provided = other.is_legal_counsel_provided;
            counsel_name = other.counsel_name;
            company_logo_url = other.company_logo_url;
            created_date = other.created_date;
        }

        public void CheckNull()
        {
            username = username.CheckNull();
            company_name = company_name.CheckNull();
            document = document.CheckNull();
            address_of_company = address_of_company.CheckNull();
            account_number = account_number.CheckNull();
            stir_of_company = stir_of_company.CheckNull();
            name_of_bank = name_of_bank.CheckNull();
            bank_code = bank_code.CheckNull();
            qqs_number = qqs_number.CheckNull();
            company_phone_number = company_phone_number.CheckNull();
            position_of_signer = position_of_signer.CheckNull();
            name_of_signer = name_of_signer.CheckNull();
            accountant_name = accountant_name.CheckNull();
            counsel_name = counsel_name.CheckNull();
            company_logo_url = company_logo_url.CheckNull();
            created_date = created_date.CheckNull();
        }
    }
}
