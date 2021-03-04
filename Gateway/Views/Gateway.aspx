<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Gateway.aspx.cs" Inherits="Gateway.Views.Gateway" %>

<!DOCTYPE html>
<head>
    <link href="../Content/nav-bar.css" type="text/css" rel="stylesheet" />
    <link href="../Content/Site.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="LogoutForm" runat="server">
        <nav class="nav-bar">
            <ul>
                <li class="active"><a href="#">Home</a></li>
                <li><a href="<%= GetUserverwaltungsLink() %>">Userverwaltung</a></li>
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
    <div>
        <h1>Dev note:</h1>
        <br />
        <h3>Mitgliederverwaltung:</h3>
        <table class="dev-note-table">
            <tr>
                <th>Funktion</th>
                <th>Funktionsfähig?</th>
                <th>Admin-only</th>
                <th id="note">Bemerkung</th>
            </tr>
            <tr>
                <td>Anzeigen</td>
                <td>Ja</td>
                <td>Nein</td>
                <td id="note">-</td>
            </tr>
            <tr>
                <td>Hinzufügen</td>
                <td>Ja</td>
                <td>Ja</td>
                <td id="note">-</td>
            </tr>
            <tr>
                <td>Bearbeiten</td>
                <td>Ja</td>
                <td>Ja</td>
                <td id="note">-</td>
            </tr>
            <tr>
                <td>Löschen</td>
                <td>Ja</td>
                <td>Ja</td>
                <td id="note">-</td>
            </tr>
        </table>
        <br />
        <hr />
        <br />
        <h3>Mannschaftsverwaltung:</h3>
        <table class="dev-note-table">
            <tr>
                <th>Funktion</th>
                <th>Funktionsfähig?</th>
                <th>Admin-only</th>
                <th id="note">Bemerkung</th>
            </tr>
            <tr>
                <td>Anzeigen</td>
                <td>Ja</td>
                <td>Nein</td>
                <td id="note">-</td>
            </tr>
            <tr>
                <td>Hinzufügen</td>
                <td>Ja</td>
                <td>Ja</td>
                <td id="note">-</td>
            </tr>
            <tr>
                <td>Bearbeiten</td>
                <td>Teilweise</td>
                <td>Ja</td>
                <td id="note">Die Mitglieder einer Mannschaft werden nicht geupdated.</td>
            </tr>
            <tr>
                <td>Löschen</td>
                <td>Ja</td>
                <td>Ja</td>
                <td id="note">-</td>
            </tr>
        </table>
        <br />
        <hr />
        <br />
        <h3>Allgemein Bemerkung:</h3>
        Ab dem ungefähr 3. mal Laden der Seite wird sie sich wahrscheinlich "aufhängen". <br />
        Die Kommunikation der Pages & der APIs gibt Probleme. Nach dem Neustart funktioniert es wieder. <br />
        Keine Ahnung wie ich das fixen soll - Keine Ahnung wieso das überhaupt passiert.
        <br />
        <br />
    </div>
    <br />
    <br>
    <br>
    <br>
</body>
