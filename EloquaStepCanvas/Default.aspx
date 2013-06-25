<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EloquaStepCanvas.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    <asp:Timer ID="Timer1" runat="server" Interval="10000" ontick="Timer1_Tick" 
        Enabled="False">
    </asp:Timer>
    <br />
    <br />
    <asp:Label ID="lblTimertime" runat="server"></asp:Label>
    <br />
    <asp:Label ID="lblContact" runat="server"></asp:Label>
    <br />
    <asp:Label ID="lblCompany" runat="server"></asp:Label>
    <br />
    <asp:Label ID="lblStepID" runat="server"></asp:Label>
    <br />
    <asp:Label ID="lstTotalContact" runat="server"></asp:Label>
    <br />
    <asp:Label ID="lstChangeStatus" runat="server"></asp:Label>
    <br />
    <asp:Label ID="lstInsertCDO" runat="server"></asp:Label>
    </form>
</body>
</html>
