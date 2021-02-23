<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Verwaltung.aspx.cs" Inherits="MitgliederService.Views.Verwaltung" %>

<!DOCTYPE html>

<head>
    <link href="../Content/nav-bar.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="LogoutForm" runat="server">

        <nav class="nav-bar">
            <ul>
                <li><a href="https://localhost:44338/Views/Gateway">Home</a></li>
                <li class="active"><a href="#">Mitgliederverwaltung</a></li>
                <li><a href="#">Mannschaftsverwaltung</a></li>
                <li style="float: right;">
                    <asp:Button CssClass="logout" ID="LogoutButton" runat="server" Text="Abmelden" OnClick="LogoutButton_Click" Height="47px" />
                </li>
                <li style="float: right;"><a href="#">Eingeloggt als: <%= GetLoggedInUsername() %></a></li>
            </ul>
        </nav>
        <br />
        <br />
        <br />
        <asp:Button ID="AddMemberButton" runat="server" Text="Mitglied hinzufügen" OnClick="AddMemberButton_Click" />
        <asp:Button ID="CancelButton" runat="server" Visible="false" Text="Abbrechen" OnClick="CancelButton_Click" />

        <asp:Panel runat="server" ID="FormPanel" Visible="false">
            &nbsp;<br />
            <br />
            <asp:Label ID="SportartenListenLabel" runat="server" Text="Sportarten:"></asp:Label>
            <br />
            <asp:DropDownList ID="SportArtenListe" runat="server"></asp:DropDownList>
            <br />
            <br />
            <asp:Label ID="AddPersonTypeLabel" runat="server" Text="Auswahl des Personen Typs:" Font-Size="Medium"></asp:Label>
            <br />
            <br />
            <asp:RadioButtonList ID="AddPersonTypeRadioButtons" runat="server">
                <asp:ListItem>Spieler</asp:ListItem>
                <asp:ListItem>Physiotherapeut</asp:ListItem>
                <asp:ListItem>Trainer</asp:ListItem>
            </asp:RadioButtonList>
            <br />
            <asp:Label ID="PersonenTypLabel" runat="server" Text=" " Font-Bold="True" Font-Overline="False" Font-Underline="True"></asp:Label>
            <br />
            <asp:Label ID="MitgliedNameLabel" runat="server" Text="Name:"></asp:Label>
            <br />
            <asp:TextBox ID="PersonNameTextBox" runat="server" Enabled="False"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="MitgliedAnzahlSpieleLabel" runat="server" Text="Anzahl gespielter Spiele:"></asp:Label>
            <br />
            <asp:TextBox ID="PersonAnzahlSpieleTextBox" type="number" runat="server" Enabled="False"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="MitgliedPositionLabel" runat="server" Text="Position:"></asp:Label>
            <br />
            <asp:TextBox ID="PersonPositionTextBox" runat="server" Enabled="False"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="MitgliedErfahrungLabel" runat="server" Text="Erfahrung:"></asp:Label>
            <br />
            <asp:TextBox ID="PersonErfahrungTextBox" type="number" runat="server" Enabled="False"></asp:TextBox>
            <br />
            <br />
            <br />
            <br />
        </asp:Panel>

        <br />
        <asp:Label ID="Mitglieder" Font-Size="X-Large" Font-Bold="true" runat="server" Text="Mitglieder:"></asp:Label>

        <br />
        <br />
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Sortieren:"></asp:Label>
        <br />
        <br />
        <asp:Label ID="AlphabetischLabel" runat="server" Text="Aufwärts"></asp:Label>
        <br />
        <br />
        <br />
    </form>

    <asp:Table ID="MyTable" runat="server" BorderStyle="Inset" ForeColor="#003399" GridLines="Both" Height="50px" Width="800px">
        <asp:TableHeaderRow runat="server">
            <asp:TableCell runat="server">Name</asp:TableCell>
            <asp:TableCell runat="server">Typ</asp:TableCell>
            <asp:TableCell runat="server">Position</asp:TableCell>
            <asp:TableCell runat="server">Erfahrung (in Jahren)</asp:TableCell>
            <asp:TableCell runat="server">Anzahl gespielter Spiele</asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableHeaderRow>
    </asp:Table>

</body>
