<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="LoginService.Views.Login" %>

<!DOCTYPE html>
<head>
    <link href="../Content/login.css" type="text/css" rel="stylesheet" />
</head>
<form id="LoginForm" runat="server">
    <h1>Login</h1>
    <br />
    <asp:label runat="server" text="Nutzername"></asp:label>
    <br />
    <asp:textbox runat="server" id="UsernameTextBox"></asp:textbox>
    <br />
    <br />
    <asp:label runat="server" text="Passwort"></asp:label>
    <br />
    <asp:textbox runat="server" id="PasswordTextBox" textmode="Password"></asp:textbox>
    <br />
    <br />
    <asp:button cssclass="LoginButton" runat="server" text="Login" id="LoginButton" onclick="LoginButton_Click" />
    <br />
    <br />
    <asp:label id="ErrorLabel" runat="server" font-bold="True" forecolor="Red" text=" "></asp:label>
</form>

