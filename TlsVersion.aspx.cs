using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TlsVersion : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        TlsVersionCheck();
    }

    protected void TlsVersionCheck()
    {
        try
        {
            string strResponse = new PayuCommunication().getResponse("getTlsVersion", ConfigurationManager.AppSettings["MERCHANT_KEY"], ConfigurationManager.AppSettings["MERCHANT_SALT"], "tls");
            lblMsg.Text = strResponse;
        }
        catch (Exception ex)
        {
            
        }
    }

}