<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="LoginService.Views.Login" %>

<!DOCTYPE html>
<head>
    <link href="../Content/login.css" type="text/css" rel="stylesheet" />
</head>
<form id="LoginForm" runat="server">
    <h1>Login</h1>
    <br />
    <asp:Label runat="server" Text="Nutzername"></asp:Label>
    <br />
    <asp:TextBox runat="server" ID="UsernameTextBox"></asp:TextBox>
    <br />
    <br />
    <asp:Label runat="server" Text="Passwort"></asp:Label>
    <br />
    <asp:TextBox runat="server" ID="PasswordTextBox" TextMode="Password"></asp:TextBox>
    <br />
    <br />
    <asp:Button CssClass="LoginButton" runat="server" Text="Login" ID="LoginButton" OnClick="LoginButton_Click" />
    <br />
    <br />
    <asp:Label ID="ErrorLabel" runat="server" Font-Bold="True" ForeColor="Red" Text=" "></asp:Label>
</form>

