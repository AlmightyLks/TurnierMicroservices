using Gateway.Controllers;
using SharedTypes;
using SharedTypes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gateway.Views
{
    public partial class Gateway : System.Web.UI.Page
    {
        private Controllers.Controller _verwalter;

        public Controller Verwalter { get => _verwalter; set => _verwalter = value; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Verwalter"] == null)
            {
                Verwalter = Global.Verwalter;
                Session["Verwalter"] = Verwalter;
            }
            else
            {
                Verwalter = Session["Verwalter"] as Controllers.Controller;
            }

            RedirectUnauthenticatedUser();
        }
        private void RedirectUnauthenticatedUser()
        {
            Verwalter.FetchUsers();

            //If no User with that SessionID is known
            if (!Verwalter.Users.Any(_ => _.SessionID == Session.SessionID))
                Response.Redirect($"{Microservices.LoginServicePage}?SessionID={Session.SessionID}");
        }
        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            User user = Verwalter.Users.Find(_ => _.SessionID == Session.SessionID);

            if (user != null)
                user.LogOut();

            RedirectUnauthenticatedUser();
        }
        public string GetLoggedInUsername()
            => Verwalter.Users.Find(_ => _.SessionID == Session.SessionID)?.Username ?? "Unbekannt";
        public string GetMitgliederverwaltungsLink()
            => $"{Microservices.MitgliederServicePage}?SessionID={Session.SessionID}";
        public string GetMannschaftsverwaltungsLink()
            => $"{Microservices.MannschaftsServicePage}?SessionID={Session.SessionID}";
    }
}