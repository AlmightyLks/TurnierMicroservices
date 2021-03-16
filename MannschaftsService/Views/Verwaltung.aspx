<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Verwaltung.aspx.cs" Inherits="MannschaftsService.Views.Verwaltung" %>

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
                <li class="active"><a href="<%= GetMannschaftsverwaltungsLink() %>">Mannschaftsverwaltung</a></li>
                <li><a href="<%= GetTurnierverwaltungsLink() %>">Turnierverwaltung</a></li>
                <li style="float: right;">
                    <asp:Button CssClass="logout" ID="LogoutButton" runat="server" Text="Abmelden" OnClick="LogoutButton_Click" Height="47px" />
                </li>
                <li style="float: right;"><a href="#">Eingeloggt als: <%= GetLoggedInUsername() %></a></li>
            </ul>
        </nav>
        
            <br />
            <br />
            <asp:Button ID="AddMannschaftButton" runat="server" Visible="false" Text="Mannschaft hinzufügen" OnClick="AddMannschaftButton_Click" />
            <asp:Button ID="CancelButton" runat="server" OnClick="CancelButton_Click" Text="Abbrechen" Visible="False" />
            <br />
            <br />
        <asp:Panel runat="server" ID="FormPanel" Visible="false">
            <br />
            <br />
            <asp:Label ID="AddMannschaftLabel" runat="server" Text="Hinzufügen einer Mannschaft" Font-Bold="True" Font-Size="Large"></asp:Label>
            <br />
            <br />
            <asp:Label ID="EingabeMannschaftsnameLabel" runat="server" Text="Ihr Mannschaftsname:"></asp:Label>
            <br />
            <asp:TextBox ID="EingabeMannschaftsnameTextBox" runat="server"></asp:TextBox>
            <br />
            <br />
            <br />
            <asp:Label ID="MannschaftSportArtLabel" runat="server" Text="Sportart der Mannschaft:"></asp:Label>
            <br />
            <asp:DropDownList ID="MannschaftSportArtenDropDownList" runat="server">
            </asp:DropDownList>
            <br />
            <br />
            <asp:Button ID="SportartBestaetigenButton" runat="server" Text="Sportart bestätigen" OnClick="SportartBestaetigenButton_Click" />
            <br />
            <br />
            <asp:Label ID="AusgewaehlteMitgliederLabel" runat="server" Text="Ausgewählte Mitglieder:"></asp:Label>
            <br />
            <asp:ListBox ID="AusgewaehlteMitgliederListBox" runat="server" Height="192px" Width="220px"></asp:ListBox>
            <br />
            <br />
            <asp:Button ID="MitgliedEntfernenButton" runat="server" Height="23px" OnClick="MitgliedEntfernenButton_Click" Text="Entfernen" />
            <br />
            <br />
            <asp:Button ID="MitgliedAuswaehlenButton" runat="server" Height="23px" OnClick="MitgliedAuswaehlenButton_Click" Text="Auswaehlen" />
            <br />
            <br />
            <asp:Label ID="VerfuegbareMitgliederLabel" runat="server" Text="Verfügbare Mitglieder:"></asp:Label>
            <br />
            <asp:ListBox ID="VerfuegbareMitgliederListBox" runat="server" Height="196px" Width="219px" SelectionMode="Multiple"></asp:ListBox>
            <br />
            <br />
            <asp:Button ID="MannschaftenErstellenButton" runat="server" Text="Mannschaft erstellen" OnClick="MannschaftenErstellenButton_Click" />
            <br />
            <asp:Button ID="EditConfirmButton" runat="server" Text="Mannschaft bearbeiten" OnClick="ConfirmEditButton_Click" Visible="False" />
            <br />
            <br />
            <asp:Label ID="StatusLabel" runat="server" Text="Status: "></asp:Label>
            <br />
            <br />
            <asp:Label ID="MannschaftenLabel" runat="server" Text="Mannschaften" Font-Bold="True" Font-Size="Large"></asp:Label>
            <br />
        </asp:Panel>

        <br />
        <br />
        <br />

        <asp:Table CssClass="datatable" ID="MannschaftsTable" runat="server" BorderStyle="Inset" ForeColor="#003399" GridLines="Both" Height="50px" Width="800px">
            <asp:TableHeaderRow runat="server">
                <asp:TableCell runat="server">Name</asp:TableCell>
                <asp:TableCell runat="server">Sportart</asp:TableCell>
                <asp:TableCell runat="server">Mitglieder</asp:TableCell>
                <asp:TableCell runat="server"></asp:TableCell>
                <asp:TableCell runat="server"></asp:TableCell>
            </asp:TableHeaderRow>
        </asp:Table>

    </form>
</body>
</html>
