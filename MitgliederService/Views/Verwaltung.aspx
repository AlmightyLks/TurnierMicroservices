<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Verwaltung.aspx.cs" Inherits="MitgliederService.Views.Verwaltung" %>

<!DOCTYPE html>

<head>
    <link href="../Content/nav-bar.css" type="text/css" rel="stylesheet" />
    <link href="../Content/Site.css" type="text/css" rel="stylesheet" />
    <link href="../Content/Table-Style.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="LogoutForm" runat="server">

        <nav class="nav-bar">
            <ul>
                <li><a href="https://localhost:44338/Views/Gateway">Home</a></li>
                <li><a href="<%= GetUserverwaltungsLink() %>">Userverwaltung</a></li>
                <li class="active"><a href="<%= GetMitgliederverwaltungsLink() %>">Mitgliederverwaltung</a></li>
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
        <div style="width: 100%; text-align: center;">
            <asp:Label ID="Mitglieder" Font-Size="X-Large" Font-Bold="true" runat="server" Text="Mitglieder"></asp:Label>
            <br />
            <br />
            <asp:Button ID="AddMemberButton" runat="server" Text="Mitglied hinzufügen" OnClick="AddMemberButton_Click" Height="30px" Width="169px" />
            <asp:Button ID="CancelButton" runat="server" Visible="false" Text="Abbrechen" OnClick="CancelButton_Click" Height="27px" Width="94px" />
        </div>

        <asp:Panel runat="server" ID="FormPanel" Visible="false">
            &nbsp;<br />
            <asp:Label ID="SportartenListenLabel" runat="server" Text="Sportarten:"></asp:Label>
            <br />
            <asp:DropDownList ID="SportArtenListe" runat="server" Height="25px" Width="105px"></asp:DropDownList>
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
            <asp:Button ID="ConfirmMitgliedTyp" runat="server" OnClick="ConfirmSportart_Click" Text="Sportart bestätigen" Height="30px" Width="168px" />
            <br />
            <br />
            <asp:Label ID="MitgliedTypLabel" runat="server" Text=" " Font-Bold="True" Font-Overline="False" Font-Underline="True"></asp:Label>
            <asp:Label ID="MitgliedIdLabel" runat="server" CssClass="AddLabel" Text="ID:"></asp:Label>
            <br />
            <asp:TextBox ID="MitgliedIdTextBox" runat="server" CssClass="AddTextbox" Enabled="False" TextMode="Number" Height="22px" Width="128px"></asp:TextBox>
            <br />
            <asp:Label ID="MitgliedNameLabel" CssClass="AddLabel" runat="server" Text="Name:"></asp:Label>
            <br />
            <asp:TextBox ID="MitgliedNameTextBox" CssClass="AddTextbox" runat="server" Enabled="False" Height="24px" Width="128px"></asp:TextBox>
            <br />
            <asp:Label ID="MitgliedAnzahlSpieleLabel" runat="server" CssClass="AddLabel" Text="Anzahl gespielter Spiele:"></asp:Label>
            <br />
            <asp:TextBox ID="MitgliedAnzahlSpieleTextBox" runat="server" CssClass="AddTextbox" Enabled="False" type="number" Height="22px" Width="127px"></asp:TextBox>
            <br />
            <asp:Label ID="MitgliedPositionLabel" runat="server" Text="Position:"></asp:Label>
            <br />
            <asp:TextBox ID="MitgliedPositionTextBox" runat="server" Enabled="False" Height="23px" Width="129px"></asp:TextBox>
            <br />
            <asp:Label ID="MitgliedErfahrungLabel" runat="server" Text="Erfahrung:"></asp:Label>
            <br />
            <asp:TextBox ID="MitgliedErfahrungTextBox" runat="server" Enabled="False" type="number" Height="23px" Width="129px"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="AddMemberConfirmButton" runat="server" Text="Mitglied hinzufügen" OnClick="AddMemberConfirmButton_Click" />
            <br />
            <asp:Button ID="EditMemberConfirmButton" runat="server" Text="Bestätigen" OnClick="EditMemberConfirmButton_Click" />
            <br />
        </asp:Panel>


        <div style="width: 100%; text-align: center;">
            <br />
            <br />
            <br />
            <br />
            <h5>Mitglied suchen...</h5>
            <br />
            <asp:TextBox ID="NameInputTextBox" Style="text-align: center" runat="server" Height="24px" Width="160px"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="SearchMemberButton" runat="server" Text="Mitglieder suchen" OnClick="SearchMemberButton_Click" Height="26px" Width="156px" />
            <br />
            <br />
            <br />
        </div>

        <asp:Table CssClass="datatable" ID="MyTable" runat="server" BorderStyle="Inset" ForeColor="#003399" GridLines="Both" Height="50px" Width="800px">
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
    </form>

</body>
