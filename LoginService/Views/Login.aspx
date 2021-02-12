<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="LoginService.Views.Login" %>

<!DOCTYPE html>
<head>
    <link href="../Content/login.css" type="text/css" rel="stylesheet" />
</head>
<form id="form1" runat="server">
    <asp:Label ID="Label1" runat="server" Text="Login"></asp:Label>
    <br />
    <br />

    <br />
    <br />
    <asp:Label runat="server" Text="Passwort"></asp:Label>
    <br />
    <asp:TextBox runat="server" ID="PasswordTextBox" TextMode="Password"></asp:TextBox>
    <br />
    <br />
    <asp:Button runat="server" Text="Login" ID="LoginButton" OnClick="LoginButton_Click" />
    <br />
    <br />
    <asp:Label ID="ErrorLabel" runat="server" Font-Bold="True" ForeColor="Red" Text=" "></asp:Label>

    <div class="main">
        <p class="sign" align="center">Sign in</p>
        <form class="form1">
            <asp:Label runat="server" Text="Nutzername"></asp:Label>
            <asp:TextBox CssClass="un" runat="server" ID="UsernameTextBox"></asp:TextBox>
            <input class="pass" type="password" align="center" placeholder="Password">
            <a class="submit" align="center">Sign in</a>
            <p class="forgot" align="center"><a href="#">Forgot Password?</p>
        </form>
    </div>
</form>

