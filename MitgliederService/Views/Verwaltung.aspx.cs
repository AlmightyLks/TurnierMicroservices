using SharedTypes;
using SharedTypes.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace MitgliederService.Views
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

            LoadSportarten();
            LoadMitglieder();
            RedirectUnauthenticatedUser();
        }

        private void LoadMitglieder()
        {
            MyTable.Rows.Clear();

            TableHeaderRow THR = new TableHeaderRow();
            THR.Cells.Add(new TableCell { Text = "Name" });
            THR.Cells.Add(new TableCell { Text = "Typ" });
            THR.Cells.Add(new TableCell { Text = "Position" });
            THR.Cells.Add(new TableCell { Text = "Erfahrung (in Jahren)" });
            THR.Cells.Add(new TableCell { Text = "Mannschaftsname" });
            THR.Cells.Add(new TableCell { Text = "Anzahl gespielter Spiele" });
            THR.Cells.Add(new TableCell { Text = "" });
            THR.Cells.Add(new TableCell { Text = "" });

            MyTable.Rows.Add(THR);

            Verwalter.FetchMitglieder();

            int personIndex = 0;

            foreach (Mitglied person in Verwalter.Mitglieder)
            {
                TableRow TR = new TableRow();

                TR.Cells.Add(new TableCell { Text = person.Name });

                if (person is Fussballspieler)
                {
                    TR.Cells.Add(new TableCell { Text = "Fussballspieler" });
                    TR.Cells.Add(new TableCell { Text = (person as Fussballspieler).Position });
                    TR.Cells.Add(new TableCell { Text = "-" });
                }
                else if (person is Handballspieler)
                {
                    TR.Cells.Add(new TableCell { Text = "Handballspieler" });
                    TR.Cells.Add(new TableCell { Text = (person as Handballspieler).Position });
                    TR.Cells.Add(new TableCell { Text = "-" });
                }
                else if (person is Tennisspieler)
                {
                    TR.Cells.Add(new TableCell { Text = "Tennisspieler" });
                    TR.Cells.Add(new TableCell { Text = "-" });
                    TR.Cells.Add(new TableCell { Text = (person as Tennisspieler).JahreErfahrung.ToString() });
                }
                else if (person is Physiotherapeut)
                {
                    TR.Cells.Add(new TableCell { Text = "Physiotherapeut" });
                    TR.Cells.Add(new TableCell { Text = "-" });
                    TR.Cells.Add(new TableCell { Text = "-" });
                }
                else if (person is Trainer)
                {
                    TR.Cells.Add(new TableCell { Text = "Trainer" });
                    TR.Cells.Add(new TableCell { Text = "-" });
                    TR.Cells.Add(new TableCell { Text = "-" });
                }

                bool inMannschaft = true;
                /*
                foreach (Mannschaft M in Verwalter.Mannschaften)
                {
                    if (M.Personen.Contains(person))
                    {
                        TR.Cells.Add(new TableCell { Text = M.Name });
                        inMannschaft = true;
                        break;
                    }
                    else
                        inMannschaft = false;
                }
                */
                if (!inMannschaft)
                {
                    TR.Cells.Add(new TableCell { Text = "-" });
                }

                if (person is Tennisspieler tennisSpieler)
                {
                    TR.Cells.Add(new TableCell { Text = tennisSpieler.AnzahlSpiele.ToString() });
                }
                else
                {
                    TR.Cells.Add(new TableCell { Text = "-" });
                }

                var editCell = new TableCell();
                var editButton = new Button();
                editButton.Text = "Edit";
                editButton.ID = "Edit " + personIndex;
                editButton.Click += EditButtonClick;
                editCell.Controls.Add(editButton);

                var deleteCell = new TableCell();
                var deleteButton = new Button();
                deleteButton.Text = "Delete";
                deleteButton.ID = "Delete " + personIndex;
                deleteButton.Click += DeleteButtonClick;
                deleteCell.Controls.Add(deleteButton);

                TR.Cells.Add(editCell);
                TR.Cells.Add(deleteCell);

                MyTable.Rows.Add(TR);
                personIndex++;
            }
        }

        private void DeleteButtonClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void EditButtonClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LoadSportarten()
        {
            SportArtenListe.Items.Clear();
            SportArtenListe.Items.AddRange(
                Verwalter.Sportarten.Select(_ => new ListItem(_)).ToArray()
                );
        }

        private void RedirectUnauthenticatedUser()
        {
            Verwalter.FetchUsers();
            string gateSessionId = Request.Params["SessionID"];

            //If no User with that SessionID is known
            if (string.IsNullOrWhiteSpace(gateSessionId)) //redirect to gateway
            {
                if (!Verwalter.Users.Any(_ => _.SessionID == gateSessionId))
                    Response.Redirect($"{Microservices.LoginServicePage}?SessionID={gateSessionId}");
            }
            else //redirect to login
            {
                if (!Verwalter.Users.Any(_ => _.SessionID == gateSessionId))
                    Response.Redirect($"{Microservices.GatewayPage}");
            }
        }
        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            User user = Verwalter.Users.Find(_ => _.SessionID == Request.Params["SessionID"]);

            if (user != null)
                user.LogOut();

            RedirectUnauthenticatedUser();
        }
        public string GetLoggedInUsername()
            => Verwalter.Users.Find(_ => _.SessionID == Request.Params["SessionID"])?.Username ?? "Unbekannt";

        protected void AddMemberButton_Click(object sender, EventArgs e)
        {
            FormPanel.Visible = true;
            AddMemberButton.Visible = false;
            CancelButton.Visible = true;
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            FormPanel.Visible = false;
            AddMemberButton.Visible = true;
            CancelButton.Visible = false;
        }
    }
}