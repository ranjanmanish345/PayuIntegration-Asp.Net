<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayuEnquiry.aspx.cs" Inherits="PayuEnquiry" %>

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
                <b>Enquiry Request</b>
            </div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-md-3">

                    </div>
                    <div class="col-md-2 text-right">
                        <b>TxnId:</b>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="txtTxnId" runat="server" Text="20180726_104413835_123456" CssClass="form-control" required="true"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnCheck" runat="server" CssClass="btn btn-primary" Text="Check" OnClick="btnCheck_Click" />
                    </div>
                </div>
            </div>
        </div>
        <br />
        <div class="panel panel-default">
            <div class="panel-heading text-center">
                <b>Enquiry Response</b>
            </div>
            <div class="panel-body">
                <asp:Label ID="lblMsg" runat="server" CssClass="text-warning"></asp:Label>
            </div>
        </div>
    </div>
    </form>
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
</body>
</html>
