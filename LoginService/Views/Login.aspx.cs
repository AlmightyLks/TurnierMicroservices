using LoginService.Controllers;
using SharedTypes;
using SharedTypes.Models;
using System;

namespace LoginService.Views
{
    public partial class Login : System.Web.UI.Page
    {
        private Controllers.Controller _verwalter;
        public Controller Verwalter { get => _verwalter; set => _verwalter = value; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Verwalter == null)
                Verwalter = Global.Verwalter;

            if (Request.Params["SessionID"] == null)
                Response.Redirect($"{Microservices.GatewayPage}");

            Verwalter.FetchUsers();
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            ErrorLabel.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text) || string.IsNullOrWhiteSpace(UsernameTextBox.Text))
                return;

            User user = Verwalter.CheckCredentials(UsernameTextBox.Text, PasswordTextBox.Text);
            if (user == null)
            {
                ErrorLabel.Text = "Entweder der eingegebene Benutzername oder das Passwort sind falsch.";
            }
            else
            {
                user.LogIn(Request.Params["SessionID"]);
                Response.Redirect($"{Microservices.GatewayPage}");
            }
        }
    }
}