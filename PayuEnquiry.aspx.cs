using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PayuEnquiry : System.Web.UI.Page
{
    static string request_id = string.Empty;
    static string additional_charges = string.Empty;
    static string mihpayid = string.Empty;
    static string bank_ref_num = string.Empty;
    static string amount = string.Empty;
    static string productinfo = string.Empty;
    static string udf1 = string.Empty;
    static string udf2 = string.Empty;
    static string udf3 = string.Empty;
    static string udf4 = string.Empty;
    static string udf5 = string.Empty;
    static string error = string.Empty;
    static string mode = string.Empty;
    static string addedon = string.Empty;
    static string PG_TYPE = string.Empty;
    static string net_amount_debit = string.Empty;
    static string status = string.Empty;
    static string txnid = string.Empty;
    static string UrlRequest = string.Empty;
    static string key = string.Empty;
    static string salt = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnCheck_Click(object sender, EventArgs e)
    {
        string strResponse = new PayuCommunication().getResponse("verify_payment", ConfigurationManager.AppSettings["MERCHANT_KEY"], ConfigurationManager.AppSettings["MERCHANT_SALT"], txtTxnId.Text.Trim());
        if (strResponse != null)
        {
            JObject obj = JObject.Parse(strResponse);
            JObject t_detail = (JObject)obj["transaction_details"];
            string status = (string)obj["status"];
            string message = (string)obj["msg"];
            new DbCommunication().LogWrite(message);
            if (status.Equals("1"))
            {
                foreach (KeyValuePair<string, JToken> x in t_detail)
                {
                    string name = x.Key;
                    JObject value = (JObject)x.Value;

                    bank_ref_num = (string)value["bank_ref_num"];
                    txnid = (string)value["txnid"];
                    addedon = (string)value["addedon"];
                    status = (string)value["status"];
                    mode = (string)value["mode"];
                    udf1 = (string)value["udf1"];
                    udf2 = (string)value["udf2"];
                    udf3 = (string)value["udf3"];
                    udf4 = (string)value["udf4"];
                    udf5 = (string)value["udf5"];
                    amount = (string)value["amt"];
                    mihpayid = (string)value["mihpayid"];

                    //----here you can update your payment status
                    lblMsg.Text = strResponse;

                    clearVariable();
                }
            }

        }
        else
        {
            lblMsg.Text = "Problem occur in API Call";
        }
    }
    private static void clearVariable()
    {
        request_id = string.Empty;
        additional_charges = string.Empty;
        mihpayid = string.Empty;
        bank_ref_num = string.Empty;
        amount = string.Empty;
        productinfo = string.Empty;
        udf1 = string.Empty;
        error = string.Empty;
        mode = string.Empty;
        addedon = string.Empty;
        PG_TYPE = string.Empty;
        net_amount_debit = string.Empty;
        status = string.Empty;
        udf1 = string.Empty;
        udf2 = string.Empty;
        udf3 = string.Empty;
        udf4 = string.Empty;
        udf5 = string.Empty;
        txnid = string.Empty;
    }
}