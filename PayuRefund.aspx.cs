using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PayuRefund : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnRefund_Click(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        try
        {
            Session["ReferenceNo"] = "123456";
            string strTxnId;
            DateTime dtNow = DateTime.Now;
            strTxnId = dtNow.ToString("yyyyMMdd") + "_" + dtNow.ToString("HHmmssfff") + "_" + Session["ReferenceNo"];

            //------Here you can store log of refund request.

            string strResult = new PayuCommunication().cancelRefundTransaction(ConfigurationManager.AppSettings["MERCHANT_KEY"], ConfigurationManager.AppSettings["MERCHANT_SALT"], txtPayuId.Text, strTxnId, txtAmountToRefund.Text);
            if (strResult != null)
            {
                JObject obj = JObject.Parse(strResult);
                string status = (string)obj["status"];
                if (status.Equals("1"))
                {
                    string msg = (string)obj["msg"];
                    string request_id = (string)obj["request_id"];
                    string bank_ref_num = (string)obj["bank_ref_num"];
                    string mihpayid = (string)obj["mihpayid"];

                    //-----Here you can update your refund log table with txn_update_id and bank_ref_num both id is different from payment request response.
                    lblMsg.Text = msg + ", Request ID=" + request_id + ", Bank Reference Number=" + bank_ref_num + ", PayU Transaction id=" + mihpayid;
                }
                else
                {
                    lblMsg.Text= (string)obj["msg"];
                }

            }
            else
            {
                lblMsg.Text = "Problem occur in API Call";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message.ToString();
        }
    }
}