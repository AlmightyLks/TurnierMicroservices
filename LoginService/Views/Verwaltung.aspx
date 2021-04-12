<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Verwaltung.aspx.cs" Inherits="LoginService.Views.Verwaltung" %>

<!DOCTYPE html>
<html>

<head runat="server">
    <title></title>
    <link href="../Content/nav-bar.css" type="text/css" rel="stylesheet" />
    <link href="../Content/Site.css" type="text/css" rel="stylesheet" />
    <link href="../Content/Table-Style.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="DingDaForm" runat="server">
        <nav class="nav-bar">
            <ul>
                <li><a href="https://localhost:44338/Views/Gateway">Home</a></li>
                <li class="active"><a href="#">Userverwaltung</a></li>
                <li><a href="<%= GetMitgliederverwaltungsLink() %>">Mitgliederverwaltung</a></li>
                <li><a href="<%= GetMannschaftsverwaltungsLink() %>">Mannschaftsverwaltung</a></li>
                <li><a href="<%= GetTurnierverwaltungsLink() %>">Turnierverwaltung</a></li>
                <li><a href="<%= GetRankingLink() %>">Ranking</a></li>
                <li style="float: right;">
                    <asp:Button CssClass="logout" ID="LogoutButton" runat="server" Text="Abmelden" OnClick="LogoutButton_Click" Height="47px" />
                </li>
                <li style="float: right;"><a href="#">Eingeloggt als: <%= GetLoggedInUsername() %></a></li>
            </ul>
        </nav>
        <br />
        <br />
        <br />
        <main>
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
            <br />
            <asp:DropDownList ID="UserTypDropDown" runat="server"></asp:DropDownList>
            <br />
            <br />
            <asp:Button ID="AddUserButton" runat="server" Text="Hinzufügen" OnClick="AddUserButton_Click" Height="31px" Width="100px" />
            <br />
            <asp:Button ID="EditConfirmButton" runat="server" Text="Bearbeiten bestätigen" OnClick="EditConfirmButton_Click" Visible="False" Height="27px" Width="190px" />
            <br />
            <br />
        </main>

        <asp:Table CssClass="datatable" ID="UserTable" runat="server" BorderStyle="Inset" ForeColor="#003399" GridLines="Both">
            <asp:TableHeaderRow runat="server">
                <asp:TableCell runat="server"></asp:TableCell>
                <asp:TableCell runat="server">Database-ID</asp:TableCell>
                <asp:TableCell runat="server">Username</asp:TableCell>
                <asp:TableCell runat="server">Typ</asp:TableCell>
                <asp:TableCell runat="server"></asp:TableCell>
                <asp:TableCell runat="server"></asp:TableCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </form>
</body>
</html>
