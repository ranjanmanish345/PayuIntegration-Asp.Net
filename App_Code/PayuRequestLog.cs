using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PayuRequestLog
/// </summary>
public class PayuRequestLog
{
    public PayuRequestLog()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public string Id { get; set; }
    public string TxnId { get; set; }
    public string InstituteId { get; set; }
    public string PayuId { get; set; }
    public string BankRefNo { get; set; }
    public string Status { get; set; }
    public string RequestTime { get; set; }
    public string PaymentTime { get; set; }
    public string ResponseTime { get; set; }
    public string ResponseFrom { get; set; }
}