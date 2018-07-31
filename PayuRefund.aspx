<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayuRefund.aspx.cs" Inherits="PayuRefund" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="container">
        <div class="panel panel-default">
            <div class="panel-heading text-center">
                <b>Refund Request</b>
            </div>
            <div class="panel-body">
                <div class="row" >
                    <div class="col-md-3 text-right">
                        <b>Payu ID:</b>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="txtPayuId" runat="server" CssClass="form-control" placeholder="Payu ID"></asp:TextBox>
                    </div>
                    <div class="col-md-3 text-right">
                        <b>Amount to Refund:</b>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="txtAmountToRefund" runat="server" CssClass="form-control" placeholder="Amount to Refund"></asp:TextBox>
                    </div>
                </div>
                <br />
                <div class="row" >
                    <div class="col-md-5">

                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnRefund" Text="Refund" runat="server" CssClass="btn btn-info" OnClick="btnRefund_Click" />
                    </div>
                    <div class="col-md-5">

                    </div>
                </div>
            </div>
        </div>
        <br />
        <div class="panel panel-default">
            <div class="panel-heading text-center">
                <b>Refund Response</b>
            </div>
            <div class="panel-body">
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </div>
        </div>
    </div>
    </form>
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
</body>
</html>
