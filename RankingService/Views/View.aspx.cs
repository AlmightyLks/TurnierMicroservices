using RankingService.Controllers;
using SharedTypes;
using SharedTypes.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RankingService.Views
{
    public partial class View : System.Web.UI.Page
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
            Verwalter.FetchTurniere();
            Verwalter.FetchMannschaften();
            Verwalter.FetchMitglieder();
            if (!IsPostBack)
            {
                LoadTurniere();
            }
            LoadSpiele();
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
            SportartLabel.Text = turnier.Sportart;
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

                    TR.Cells.Add(new TableCell { Text = mannschaft1Name });
                    TR.Cells.Add(new TableCell { Text = mannschaft2Name });
                    TR.Cells.Add(new TableCell { Text = $"{spiel.ErsterTeilnehmer.Punkte}:{spiel.ZweiterTeilnehmer.Punkte}" });
                }

                SpieleTable.Rows.Add(TR);
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
        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            User user = Verwalter.Users.Find(_ => _.SessionID == Request.Params["SessionID"]);

            if (user != null)
                user.LogOut();

            RedirectUnauthenticatedUser();
        }
        public string GetTurnierverwaltungsLink()
            => $"{Microservices.TurnierServicePage}?SessionID={Request.Params["SessionID"]}";
        public string GetLoggedInUsername()
            => Verwalter.Users.Find(_ => _.SessionID == Request.Params["SessionID"])?.Username ?? "Unbekannt";
        public string GetUserverwaltungsLink()
            => $"{Microservices.LoginServiceVerwalterPage}?SessionID={Request.Params["SessionID"]}";
        public string GetMitgliederverwaltungsLink()
            => $"{Microservices.MitgliederServicePage}?SessionID={Request.Params["SessionID"]}";
        public string GetMannschaftsverwaltungsLink()
            => $"{Microservices.MannschaftsServicePage}?SessionID={Request.Params["SessionID"]}";
        public string GetRankingLink()
            => $"#";


    }
}