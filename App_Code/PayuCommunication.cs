using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for PayuCommunication
/// </summary>
public class PayuCommunication
{
    public PayuCommunication()
    {
        //
        // TODO: Add constructor logic here
        //
    } 
    
    /// <summary>
    /// Generate HASH for encrypt all parameter passing while transaction
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public string Generatehash512(string text)
    {
        byte[] message = Encoding.UTF8.GetBytes(text);
        UnicodeEncoding UE = new UnicodeEncoding();
        byte[] hashValue;
        SHA512Managed hashString = new SHA512Managed();
        string hex = "";
        hashValue = hashString.ComputeHash(message);
        foreach (byte x in hashValue)
        {
            hex += String.Format("{0:x2}", x);
        }
        return hex;
    }

    /// <summary>
    /// This method is used to get api response from payu
    /// </summary>
    /// <param name="strCommandName"></param>
    /// <param name="strKey"></param>
    /// <param name="strSalt"></param>
    /// <param name="strVar1"></param>
    /// <returns></returns>
    public string getResponse(string strCommandName, string strKey, string strSalt, string strVar1)
    {
        string strResponse = null;
        string[] hashVarsSeq = ConfigurationManager.AppSettings["PAYU_VERIFICATION_HASH"].Split('|'); // spliting hash sequence from config
        string UrlRequest = ConfigurationManager.AppSettings["PAYU_VERIFICATION_URL"];
        string hash_string = "";
        try
        {
            //"key|command|var1"
            foreach (string hash_var in hashVarsSeq)
            {
                if (hash_var == "key")
                {
                    hash_string = hash_string + strKey;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "command")
                {
                    hash_string = hash_string + strCommandName;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "var1")
                {
                    hash_string = hash_string + strVar1;
                    hash_string = hash_string + '|';
                }
            }

            hash_string += strSalt;// appending SALT

            string hash = Generatehash512(hash_string).ToLower();
            NameValueCollection myNameValueCollection = new NameValueCollection();
            myNameValueCollection.Add("hash", hash);
            myNameValueCollection.Add("key", strKey);
            myNameValueCollection.Add("command", strCommandName);
            myNameValueCollection.Add("var1", strVar1);

            using (WebClient client = new WebClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                byte[] response = client.UploadValues(UrlRequest, myNameValueCollection);
                strResponse = Encoding.UTF8.GetString(response);

            }
        }
        catch (Exception ex)
        {
            strResponse = null;
            new DbCommunication().LogWrite("Exception in Payu Communication GetResponse "+ex.Message.ToString());
        }
        return strResponse;
    }

    /// <summary>
    /// This method is used to cancel or refund payment
    /// </summary>
    /// <param name="strKey"></param>
    /// <param name="strSalt"></param>
    /// <param name="strPayuId"></param>
    /// <param name="strTxnId"></param>
    /// <param name="strAmount"></param>
    /// <returns></returns>
    public string cancelRefundTransaction(string strKey, string strSalt, string strPayuId, string strTxnId,string strAmount)
    {
        string strResponse = null;
        string[] hashVarsSeq = ConfigurationManager.AppSettings["PAYU_VERIFICATION_HASH"].Split('|'); // spliting hash sequence from config
        string UrlRequest = ConfigurationManager.AppSettings["PAYU_VERIFICATION_URL"];
        string hash_string = "";
        try
        {
            //"key|command|var1"
            foreach (string hash_var in hashVarsSeq)
            {
                if (hash_var == "key")
                {
                    hash_string = hash_string + strKey;
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "command")
                {
                    hash_string = hash_string + "cancel_refund_transaction";
                    hash_string = hash_string + '|';
                }
                else if (hash_var == "var1")
                {
                    hash_string = hash_string + strPayuId;
                    hash_string = hash_string + '|';
                }
            }

            hash_string += strSalt;// appending SALT

            string hash = Generatehash512(hash_string).ToLower();
            NameValueCollection myNameValueCollection = new NameValueCollection();
            myNameValueCollection.Add("hash", hash);
            myNameValueCollection.Add("key", strKey);
            myNameValueCollection.Add("command", "cancel_refund_transaction");
            myNameValueCollection.Add("var1", strPayuId);
            myNameValueCollection.Add("var2", strTxnId);
            myNameValueCollection.Add("var3", strAmount);

            using (WebClient client = new WebClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                byte[] response = client.UploadValues(UrlRequest, myNameValueCollection);
                strResponse = Encoding.UTF8.GetString(response);

            }
        }
        catch (Exception ex)
        {
            strResponse = null;
            new DbCommunication().LogWrite("Exception in Payu Communication cancelRefundTransaction " + ex.Message.ToString());
        }
        return strResponse;
    }

    /// <summary>
    /// This method is used to prepare html form to post
    /// </summary>
    /// <param name="strUrl"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public string PostFormSeamless(string strUrl, Hashtable data)
    {
        //Set a name for the form
        string formID = "PostFormSeamless";
        //Build the form using the specified data to be posted.
        StringBuilder strForm = new StringBuilder();
        strForm.Append("<form id=\"" + formID + "\" name=\"" +
                       formID + "\" action=\"" + strUrl +
                       "\" method=\"POST\">");

        foreach (System.Collections.DictionaryEntry key in data)
        {
            strForm.Append("<input type=\"hidden\" name=\"" + key.Key +
                           "\" value=\"" + key.Value + "\">");
        }        
        strForm.Append("</form>");
        //Build the JavaScript which will do the Posting operation.
        StringBuilder strScript = new StringBuilder();
        strScript.Append("<script language='javascript'>");
        strScript.Append("var v" + formID + " = document." +
                         formID + ";");
        strScript.Append("v" + formID + ".submit();");
        strScript.Append("</script>");
        //Return the form and the script concatenated.
        //(The order is important, Form then JavaScript)
        return strForm.ToString() + strScript.ToString();
    }


}