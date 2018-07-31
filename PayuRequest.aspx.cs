using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PayuRequest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ReferenceNo"] = "123456";
        GoPayment("HDFC", ConfigurationManager.AppSettings["MERCHANT_KEY"], ConfigurationManager.AppSettings["MERCHANT_SALT"]);
    }

    protected void GoPayment(string strBankName,string strKey,string strSalt)
    {
        try
        {
            string strTxnId;
            DateTime dtNow = DateTime.Now;
            strTxnId = dtNow.ToString("yyyyMMdd") + "_" + dtNow.ToString("HHmmssfff") + "_" + Session["ReferenceNo"];

            string[] hashVarsSeq = ConfigurationManager.AppSettings["PAYU_HASH"].Split('|');
            string hash_string = string.Empty;
            hash_string = "";

            string strAmount = "1";
            string strProductInfo = "Testing";
            string strFirstName = "firstname";
            string strLastName = "lastName";
            string strEmail = "yourEmail@gmail.com";
            string strPhone = "9999999999";
            string strAddress1 = "address1";
            string strAddress2 = "address2";
            string strUdf1 = "udf1";
            string strUdf2 = "udf2";
            string strUdf3 = "udf3";
            string strUdf4 = "udf4";
            string strUdf5 = "udf5";
            foreach (string hash_var in hashVarsSeq)
            {
                if (hash_var == "key")
                {
                    hash_string = hash_string + strKey;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "txnid")
                {
                    hash_string = hash_string + strTxnId;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "amount")
                {
                    hash_string = hash_string + strAmount;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "productinfo")
                {
                    hash_string = hash_string + strProductInfo;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "firstname")
                {
                    hash_string = hash_string + strFirstName;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "email")
                {
                    hash_string = hash_string + strEmail;//"abc@xyz.com";//
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "phone")
                {
                    hash_string = hash_string + strPhone;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "udf2")
                {
                    hash_string = hash_string + strUdf2;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "udf3")
                {
                    hash_string = hash_string + strUdf3;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "udf4")
                {
                    hash_string = hash_string + strUdf4;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "udf5")
                {
                    hash_string = hash_string + strUdf5;
                    hash_string = hash_string + '|';
                }
                else
                {

                    hash_string = hash_string + (Request.Form[hash_var] != null ? Request.Form[hash_var] : "");// isset if else
                    hash_string = hash_string + '|';
                }
            }

            hash_string += strSalt;// appending SALT

            string hash =new PayuCommunication().Generatehash512(hash_string).ToLower();
            string strFullUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            string strAction = ConfigurationManager.AppSettings["PAYU_BASE_URL"];

            System.Collections.Hashtable data = new System.Collections.Hashtable(); // adding values in hash table for data post
            data.Add("hash", hash);
            data.Add("key", strKey);
            data.Add("txnid", strTxnId);
            data.Add("amount", strAmount);
            data.Add("productinfo", strProductInfo);
            data.Add("firstname", strFirstName);
            data.Add("lastname", strLastName);
            data.Add("email", strEmail);
            data.Add("phone", strPhone);
            data.Add("address1", strAddress1);
            data.Add("address2", strAddress2);
            data.Add("city", strFullUrl);
            data.Add("udf2", strUdf2);
            data.Add("udf3", strUdf3);
            data.Add("udf4", strUdf4);
            data.Add("udf5", strUdf5);
            data.Add("surl", "http://localhost:49253/PayuResponse.aspx");
            data.Add("furl", "http://localhost:49253/PayuResponse.aspx");
            data.Add("curl", "http://localhost:49253/PayuResponse.aspx");

            string query = "INSERT INTO PayuRequestLog(TxnId,InstituteId,PayuId,BankRefNo, RequestTime, Status) VALUES('" + strTxnId + "','" + strFirstName + "','','', 'getdate()', 'IN_PROGESS')";
            int iResult=new DbCommunication().ExecuteQuery(query);

            string strForm =new PayuCommunication().PostFormSeamless(strAction, data);
            Page.Controls.Add(new LiteralControl(strForm));
        }
        catch (Exception ex)
        {
        }
    }
}