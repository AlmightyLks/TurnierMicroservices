using SharedTypes;
using SharedTypes.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            {
                LoadTurniere();
                LoadTeilnehmer();
            }
            LoadSpiele();

            if (Verwalter.LoggedInUser?.Type != UserType.Admin)
            {
                DeleteButton.Visible = false;
                AddTeilnehmerButton.Visible = false;
                TeilnehmerDropDownList.Visible = false;
            }
        }

        private void LoadSpiele()
        {
            for (int i = SpieleTable.Rows.Count; i > 1; i--)
            {
                SpieleTable.Rows.RemoveAt(i);
            }
            Turnier turnier = Verwalter.Turniere.Find(_ => _.Titel == TurnierDropDownList.SelectedValue);
            if (turnier == null)
                return;
            foreach (Spiel spiel in turnier.Spiele)
            {
                TableRow TR = new TableRow();
                if (turnier.Sportart == "Tennis")
                {
                    Mitglied teilnehmerOne = Verwalter.Mitglieder.Find(_ => _.Id == spiel.ErsterTeilnehmer.TeilnehmerId);
                    Mitglied teilnehmerTwo = Verwalter.Mitglieder.Find(_ => _.Id == spiel.ErsterTeilnehmer.TeilnehmerId);
                    TR.Cells.Add(new TableCell { Text = teilnehmerOne?.Name });
                    TR.Cells.Add(new TableCell { Text = teilnehmerTwo?.Name });
                    TR.Cells.Add(new TableCell { Text = $"{spiel.ErsterTeilnehmer.Punkte}:{spiel.ZweiterTeilnehmer.Punkte}" });
                }
                else
                {
                    Mannschaft teilnehmerOne = Verwalter.Mannschaften.Find(_ => _.Id == spiel.ErsterTeilnehmer.TeilnehmerId);
                    Mannschaft teilnehmerTwo = Verwalter.Mannschaften.Find(_ => _.Id == spiel.ZweiterTeilnehmer.TeilnehmerId);
                    string mannschaft1Name = teilnehmerOne?.Name;
                    string mannschaft2Name = teilnehmerTwo?.Name;

                    TR.Cells.Add(new TableCell { Text = mannschaft1Name});
                    TR.Cells.Add(new TableCell { Text = mannschaft2Name });
                    TR.Cells.Add(new TableCell { Text = $"{spiel.ErsterTeilnehmer.Punkte}:{spiel.ZweiterTeilnehmer.Punkte}" });
                }
                if (Verwalter.LoggedInUser?.Type == UserType.Admin)
                {
                    var editCell = new TableCell();
                    var editButton = new Button();
                    editButton.Text = "Edit";
                    editButton.ID = "Edit " + turnier.Id + " " + spiel.Id;
                    editButton.Click += EditSpielButton_Click;
                    editButton.Font.Size = FontUnit.Medium;
                    editButton.BackColor = Color.LightBlue;
                    editCell.Controls.Add(editButton);

                    var deleteCell = new TableCell();
                    var deleteButton = new Button();
                    deleteButton.Text = "Delete";
                    deleteButton.ID = "Delete " + turnier.Id + " " + spiel.Id;
                    deleteButton.Click += DeleteSpielButton_Click;
                    deleteButton.BackColor = Color.IndianRed;
                    deleteButton.Font.Size = FontUnit.Medium;
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
            TeilnehmerDropDownList.Items.Clear();
            if (currentTurnier.Sportart == "Tennis")
            {
                IEnumerable<Tennisspieler> smth = Verwalter.Mitglieder
                    .Where(_ => _ is Tennisspieler tennisspieler).Select(_ => _ as Tennisspieler);
                foreach (Tennisspieler spieler in smth)
                {
                    if (currentTurnier.Spiele.Any(e => e.ErsterTeilnehmer.TeilnehmerId != spieler.Id && e.ZweiterTeilnehmer.TeilnehmerId != spieler.Id))
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
                    if (currentTurnier.Spiele.Any(e => e.ErsterTeilnehmer.TeilnehmerId != mannschaft.Id && e.ZweiterTeilnehmer.TeilnehmerId != mannschaft.Id))
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
        public string GetRankingLink()
            => $"{Microservices.RankingServicePage}?SessionID={Request.Params["SessionID"]}";

        protected void DeleteSpielButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            string[] elements = button.ID.Split(' ');
            int turnierId = int.Parse(elements[1]);
            int spielId = int.Parse(elements[2]);
            
            Turnier turnier = Verwalter.Turniere.Find(_ => _.Id == turnierId);
            turnier.Spiele.RemoveAll(_ => _.Id == spielId);
            turnier.Put();

            Response.Redirect($"{Microservices.TurnierServicePage}?SessionID={Request.Params["SessionID"]}");
        }

        protected void DeleteTurnierButton_Click(object sender, EventArgs e)
        {
            Turnier turnier = Verwalter.Turniere.Find(_ => _.Titel == TurnierDropDownList.SelectedValue);
            Verwalter.DeleteTurnier(turnier);
            Response.Redirect($"{Microservices.TurnierServicePage}?SessionID={Request.Params["SessionID"]}");
        }
    }
}