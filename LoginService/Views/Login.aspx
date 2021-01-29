<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="LoginService.Views.Login" %>

<!DOCTYPE html>
<form id="form1" runat="server">
    <asp:Label ID="Label1" runat="server" Text="Login"></asp:Label>
    <br />
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
    <asp:Button runat="server" Text="Login" ID="LoginButton" OnClick="LoginButton_Click" />
    <br />
    <br />
    <asp:Label ID="ErrorLabel" runat="server" Font-Bold="True" ForeColor="Red" Text=" "></asp:Label>
</form>

