<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="LoginService.Views.Login" %>

<!DOCTYPE html>
<head>
    <link href="../Content/login.css" type="text/css" rel="stylesheet" />
</head>
<form id="LoginForm" runat="server">
    <asp:panel runat="server"  visible="false">
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
    </asp:panel>

    <asp:panel runat="server" id="UserVerwaltung" visible="false">
        <nav class="nav-bar">
            <ul>
                <li><a href="https://localhost:44338/Views/Gateway">Home</a></li>
                <li style="float: right;">
                    <asp:Button CssClass="logout" ID="LogoutButton" runat="server" Text="Abmelden" OnClick="LogoutButton_Click" Height="47px" />
                </li>
                <li style="float: right;"><a href="#">Eingeloggt als: <%= GetLoggedInUsername() %></a></li>
            </ul>
        </nav>
        <br />
        <br />
        <h1>User Verwaltung</h1>
    </asp:panel>
</form>

