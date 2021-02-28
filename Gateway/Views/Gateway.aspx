<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Gateway.aspx.cs" Inherits="Gateway.Views.Gateway" %>

<!DOCTYPE html>
<head>
    <link href="../Content/nav-bar.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="LogoutForm" runat="server">
        <nav class="nav-bar">
            <ul>
                <li class="active"><a href="#">Home</a></li>
                <li><a href="<%= GetMitgliederverwaltungsLink() %>">Mitgliederverwaltung</a></li>
                <li><a href="<%= GetMannschaftsverwaltungsLink() %>">Mannschaftsverwaltung</a></li>
                <li style="float: right">
                    <asp:Button CssClass="logout" ID="LogoutButton" runat="server" Text="Abmelden" OnClick="LogoutButton_Click" Height="47px" />
                </li>
                <li style="float: right;"><a id="LoggedInAs" href="#">Eingeloggt als: <%= GetLoggedInUsername() %></a></li>
            </ul>
        </nav>
    </form>
    <br />
    <br />
    <br />
    <br />
    <br>
    <br>
    <br>
</body>
