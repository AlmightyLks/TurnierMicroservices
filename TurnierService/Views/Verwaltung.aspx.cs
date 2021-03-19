using SharedTypes;
using SharedTypes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TurnierService.Views
{
    public partial class Verwaltung : System.Web.UI.Page
    {
        private Controllers.Controller _verwalter;

        public Controllers.Controller Verwalter { get => _verwalter; set => _verwalter = value; }

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
            Verwalter.FetchTurniere();
            LoadTurniere();

            if (Verwalter.LoggedInUser?.Type != UserType.Admin)
            {
                DeleteButton.Visible = false;
            }
        }

        private void LoadTurniere()
        {
            for (int i = TurnierTable.Rows.Count; i > 1; i--)
            {
                TurnierTable.Rows.RemoveAt(i);
            }

            int turnierIndex = 0;
            foreach (Turnier turnier in Verwalter.Turniere)
            {
                foreach (Spiel spiel in turnier.Spiele)
                {
                    TableRow TR = new TableRow();

                    TR.Cells.Add(new TableCell { Text = spiel.Punktestand.Mannschaft[0].Name });
                    TR.Cells.Add(new TableCell { Text = spiel.Punktestand.Mannschaft[1].Name });
                    TR.Cells.Add(new TableCell { Text = $"{spiel.Punktestand.Punkte[0]}:{spiel.Punktestand.Punkte[1]}" });

                    if (Verwalter.LoggedInUser?.Type == UserType.Admin)
                    {
                        var editCell = new TableCell();
                        var editButton = new Button();
                        editButton.Text = "Edit";
                        editButton.ID = "Edit " + turnierIndex;
                        editButton.Click += EditSpielButton_Click;
                        editCell.Controls.Add(editButton);

                        var deleteCell = new TableCell();
                        var deleteButton = new Button();
                        deleteButton.Text = "Delete";
                        deleteButton.ID = "Delete " + turnierIndex;
                        deleteButton.Click += DeleteSpielButton_Click;
                        deleteCell.Controls.Add(deleteButton);

                        TR.Cells.Add(editCell);
                        TR.Cells.Add(deleteCell);
                    }

                    TurnierTable.Rows.Add(TR);
                }
                turnierIndex++;
            }
        }

        private void EditSpielButton_Click(object sender, EventArgs e)
        {

        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            if (Verwalter.LoggedInUser != null)
            {
                Verwalter.LoggedInUser.LogOut();
            }

            RedirectUnauthenticatedUser();
        }
        private void RedirectUnauthenticatedUser()
        {
            Verwalter.FetchUsers();
            string gateSessionId = Request.Params["SessionID"];

            //If no User with that SessionID is known
            if (string.IsNullOrWhiteSpace(gateSessionId)) //redirect to gateway
            {
                Response.Redirect($"{Microservices.GatewayPage}");
            }
            else
            {
                Verwalter.LoggedInUser = Verwalter.Users.Find(_ => _.SessionID == Request.Params["SessionID"]);
                if (Verwalter.LoggedInUser != null)
                {
                    Response.Redirect($"{Microservices.GatewayPage}");
                }
            }
        }
        public string GetTurnierverwaltungsLink()
            => $"#";
        public string GetLoggedInUsername()
            => Verwalter.Users.Find(_ => _.SessionID == Request.Params["SessionID"])?.Username ?? "Unbekannt";
        public string GetUserverwaltungsLink()
            => $"{Microservices.LoginServiceVerwalterPage}?SessionID={Request.Params["SessionID"]}";
        public string GetMitgliederverwaltungsLink()
            => $"{Microservices.MitgliederServicePage}?SessionID={Request.Params["SessionID"]}";
        public string GetMannschaftsverwaltungsLink()
            => $"{Microservices.MannschaftsServicePage}?SessionID={Request.Params["SessionID"]}";

        protected void DeleteSpielButton_Click(object sender, EventArgs e)
        {
        }

        protected void DeleteTurnierButton_Click(object sender, EventArgs e)
        {
            Turnier turnier = Verwalter.Turniere.Find(_ => _.Titel == TurnierDropDownList.SelectedValue);
            Verwalter.DeleteTurnier(turnier);
        }
    }
}