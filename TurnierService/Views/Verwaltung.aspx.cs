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
            Verwalter.FetchMannschaften();
            Verwalter.FetchMitglieder();
            if (!IsPostBack)
                Verwalter.Turniere.Add(new Turnier() { Id = 4, Sportart = "Tennis", Titel = "Foo" });
            LoadTurniere();
            LoadSpiele();
            LoadTeilnehmer();

            if (Verwalter.LoggedInUser?.Type != UserType.Admin)
            {
                DeleteButton.Visible = false;
            }
        }

        private void LoadSpiele()
        {
            for (int i = SpieleTable.Rows.Count; i > 1; i--)
            {
                SpieleTable.Rows.RemoveAt(i);
            }
            Turnier turnier = Verwalter.Turniere.Find(_ => _.Titel == TurnierDropDownList.SelectedValue);
            foreach (Spiel spiel in turnier?.Spiele)
            {
                TableRow TR = new TableRow();
                if (turnier.Sportart == "Tennis")
                {
                    TR.Cells.Add(new TableCell { Text = (spiel.ErsterTeilnehmer.Teilnehmer as Tennisspieler)?.Name });
                    TR.Cells.Add(new TableCell { Text = (spiel.ZweiterTeilnehmer.Teilnehmer as Tennisspieler)?.Name });
                    TR.Cells.Add(new TableCell { Text = $"{spiel.ErsterTeilnehmer.Punkte}:{spiel.ZweiterTeilnehmer.Punkte}" });
                }
                if (Verwalter.LoggedInUser?.Type == UserType.Admin)
                {
                    var editCell = new TableCell();
                    var editButton = new Button();
                    editButton.Text = "Edit";
                    editButton.ID = "Edit " + turnier.Id;
                    editButton.Click += EditSpielButton_Click;
                    editCell.Controls.Add(editButton);

                    var deleteCell = new TableCell();
                    var deleteButton = new Button();
                    deleteButton.Text = "Delete";
                    deleteButton.ID = "Delete " + turnier.Id;
                    deleteButton.Click += DeleteSpielButton_Click;
                    deleteCell.Controls.Add(deleteButton);

                    TR.Cells.Add(editCell);
                    TR.Cells.Add(deleteCell);
                }

                SpieleTable.Rows.Add(TR);
            }
        }

        private void LoadTeilnehmer()
        {
            Turnier currentTurnier = Verwalter.Turniere.Find(_ => _.Titel == TurnierDropDownList.SelectedValue);
            if (currentTurnier == null)
            {
                return;
            }
            if (currentTurnier.Sportart == "Tennis")
            {
                IEnumerable<Tennisspieler> smth = Verwalter.Mitglieder
                    .Where(_ => _ is Tennisspieler tennisspieler).Select(_ => _ as Tennisspieler);
                foreach (Tennisspieler spieler in smth)
                {
                    if (!currentTurnier.Spiele
                        .Any(e => (e.ErsterTeilnehmer.Teilnehmer as Tennisspieler) == spieler || (e.ZweiterTeilnehmer.Teilnehmer as Tennisspieler) == spieler))
                    {
                        TeilnehmerDropDownList.Items.Add($"{spieler.Id} {spieler.Name}");
                    }
                }
            }
            else
            {
                IEnumerable<Mannschaft> smth = Verwalter.Mannschaften
                    .Where(_ => _.SportArt == currentTurnier.Sportart);
                foreach (Mannschaft mannschaft in smth)
                {
                    if (!currentTurnier.Spiele
                        .Any(e => (e.ErsterTeilnehmer.Teilnehmer as Mannschaft) == mannschaft || (e.ZweiterTeilnehmer.Teilnehmer as Mannschaft) == mannschaft))
                    {
                        TeilnehmerDropDownList.Items.Add($"{mannschaft.Id} {mannschaft.Name}");
                    }
                }
            }

        }

        private void LoadTurniere()
        {
            TurnierDropDownList.Items.Clear();
            
            foreach (Turnier turnier in Verwalter.Turniere)
            {
                TurnierDropDownList.Items.Add(turnier.Titel);
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
                if (Verwalter.LoggedInUser == null)
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