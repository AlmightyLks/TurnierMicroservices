<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="RankingService.Views.View" %>

<!DOCTYPE html>

<head>
    <link href="../Content/nav-bar.css" type="text/css" rel="stylesheet" />
    <link href="../Content/Site.css" type="text/css" rel="stylesheet" />
    <link href="../Content/Table-Style.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <nav class="nav-bar">
            <ul>
                <li><a href="https://localhost:44338/Views/Gateway">Home</a></li>
                <li><a href="<%= GetUserverwaltungsLink() %>">Userverwaltung</a></li>
                <li><a href="<%= GetMitgliederverwaltungsLink() %>">Mitgliederverwaltung</a></li>
                <li><a href="<%= GetMannschaftsverwaltungsLink() %>">Mannschaftsverwaltung</a></li>
                <li><a href="<%= GetTurnierverwaltungsLink() %>">Turnierverwaltung</a></li>
                <li class="active"><a href="<%= GetRankingLink() %>">Ranking</a></li>
                <li style="float: right;">
                    <asp:Button CssClass="logout" ID="LogoutButton" runat="server" Text="Abmelden" OnClick="LogoutButton_Click" Height="47px" />
                </li>
                <li style="float: right;"><a href="#">Eingeloggt als: <%= GetLoggedInUsername() %></a></li>
            </ul>
        </nav>

        <br />
        <br />
        <div style="width: 100%; text-align: center;">
            <asp:Label ID="Turniere" Font-Size="X-Large" Font-Bold="true" runat="server" Text="Turniere"></asp:Label>
            <br />
            <br />
            <asp:DropDownList ID="TurnierDropDownList" runat="server" AutoPostBack="True" Height="27px" Width="151px"></asp:DropDownList>
            <br />
            <br />
            <asp:Label ID="SportartLabel" Font-Size="X-Large" Font-Bold="true" runat="server" Text="Turniere:"></asp:Label>
        </div>
        <br />
        <br />

        <asp:Table CssClass="datatable" ID="SpieleTable" runat="server" BorderStyle="Inset" ForeColor="#003399" GridLines="Both" Height="50px" Width="800px">
            <asp:TableHeaderRow runat="server">
                <asp:TableCell runat="server">Teilnehmer 1</asp:TableCell>
                <asp:TableCell runat="server">Teilnehmer 2</asp:TableCell>
                <asp:TableCell runat="server">Punktestand</asp:TableCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </form>
</body>
</html>
