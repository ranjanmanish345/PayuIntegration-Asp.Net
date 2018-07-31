using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PayuResponse : System.Web.UI.Page
{
    string key, txnid, amount, productinfo, firstname, email, phone, lastname, address1, address2, city, state, country,
        zipcode, udf1, udf2, udf3, udf4, udf5, surl, furl, discount, hash, mihpayid, mode, status, error, bankcode, PG_TYPE,
        bank_ref_num,unmappedstatus,salt;
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!Page.IsPostBack)
        {
            try
            {
                key = ConfigurationManager.AppSettings["MERCHANT_KEY"];
                salt = ConfigurationManager.AppSettings["MERCHANT_SALT"];

                string[] merc_hash_vars_seq= ConfigurationManager.AppSettings["PAYU_HASH"].Split('|');
                string merc_hash_string = string.Empty;
                string merc_hash = string.Empty;
                if (Request.Form["status"] == "success")
                {
                    Array.Reverse(merc_hash_vars_seq);
                    if (Request.Form["additionalCharges"] != null)
                        merc_hash_string = Request.Form["additionalCharges"] + "|" + salt + "|" + Request.Form["status"];
                    else
                        merc_hash_string = salt + "|" + Request.Form["status"];

                    
                    foreach (string merc_hash_var in merc_hash_vars_seq)
                    {
                        merc_hash_string += "|";
                        merc_hash_string = merc_hash_string + (Request.Form[merc_hash_var] != null ? Request.Form[merc_hash_var] : "");

                    }
                    merc_hash =new PayuCommunication().Generatehash512(merc_hash_string).ToLower();
                    
                    if (merc_hash == Request.Form["hash"])
                    {
                        txnid = Request.Form["txnid"];
                        amount = Request.Form["amount"];
                        productinfo = Request.Form["productinfo"];
                        firstname = Request.Form["firstname"];
                        email = Request.Form["email"];
                        phone = Request.Form["phone"];
                        lastname = Request.Form["lastname"];
                        address1 = Request.Form["address1"];
                        address2 = Request.Form["address2"];
                        city = Request.Form["city"];
                        state = Request.Form["state"];
                        country = Request.Form["country"];
                        zipcode = Request.Form["zipcode"];
                        udf1 = Request.Form["udf1"];
                        udf2 = Request.Form["udf2"];
                        udf3 = Request.Form["udf3"];
                        udf4 = Request.Form["udf4"];
                        udf5 = Request.Form["udf5"];
                        mihpayid = Request.Form["mihpayid"];
                        mode = Request.Form["mode"];
                        status = Request.Form["status"];
                        error = Request.Form["error"];
                        PG_TYPE = Request.Form["PG_TYPE"];
                        bank_ref_num = Request.Form["bank_ref_num"];
                        unmappedstatus = Request.Form["unmappedstatus"];

                        lblMsg.Text = " Your Transaction Status is " + status + " and TxnId id " + txnid+" and PayuId id "+mihpayid;

                        //string query = "update PayuRequestLog set PayuId='"+mihpayid+"',BankRefNo='"+bank_ref_num+"', Status='success' where TxnId='"+txnid+"' ";
                        //int iResult = new DbCommunication().ExecuteQuery(query);
                    }
                    else
                    {
                        txnid = Request.Form["txnid"];
                        mihpayid = Request.Form["mihpayid"];
                        bank_ref_num = Request.Form["bank_ref_num"];
                        lblMsg.Text = " Your Transaction Status is " + status + " (Secure Hash Not Matched) and TxnId id " + txnid + " and PayuId id " + mihpayid;
                        //hash not matched
                    }
                }

                else
                {
                    txnid = Request.Form["txnid"];
                    mihpayid = Request.Form["mihpayid"];
                    bank_ref_num = Request.Form["bank_ref_num"];
                    lblMsg.Text = " Your Transaction Status is " + status + " and TxnId id " + txnid + " and PayuId id " + mihpayid;
                    //fail
                }
            }
            catch
            {
            }
        }
    }
}