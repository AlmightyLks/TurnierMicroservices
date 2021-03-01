using MannschaftsService.Controllers;
using SharedTypes;
using SharedTypes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MannschaftsService.Views
{
    public partial class Verwaltung : System.Web.UI.Page
    {
        private Controller _verwalter;
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
            Verwalter.FetchMannschaften();
            Verwalter.FetchMitglieder();
            RedirectUnauthenticatedUser();

            if (!IsPostBack)
            {
                LoadSportarten();
                if (Verwalter.Users.Find(_ => _.SessionID == Request.Params["SessionID"]).Username.ToLower() == "admin")
                {
                    AddMannschaftButton.Visible = true;
                }
            }
            LoadMannschaften();

        }
        public string GetLoggedInUsername()
            => Verwalter.Users.Find(_ => _.SessionID == Request.Params["SessionID"])?.Username ?? "Unbekannt";

        private void LoadMannschaften()
        {
            MannschaftsTable.Rows.Clear();

            TableHeaderRow THR = new TableHeaderRow();
            THR.Cells.Add(new TableCell { Text = "Name" });
            THR.Cells.Add(new TableCell { Text = "Sportart" });
            THR.Cells.Add(new TableCell { Text = "Mitglieder" });

            if (Verwalter.Users.Find(_ => _.SessionID == Request.Params["SessionID"])?.Username.ToLower() == "admin")
            {
                THR.Cells.Add(new TableCell { Text = "" });
                THR.Cells.Add(new TableCell { Text = "" });
            }

            MannschaftsTable.Rows.Add(THR);

            int personIndex = 0;
            foreach (Mannschaft mannschaft in Verwalter.Mannschaften)
            {
                TableRow TR = new TableRow();

                TR.Cells.Add(new TableCell { Text = mannschaft.Name });
                TR.Cells.Add(new TableCell { Text = mannschaft.SportArt });
                DropDownList DDL = new DropDownList();
                foreach (Mitglied mitglied in mannschaft.Mitglieder)
                {
                    DDL.Items.Add(mitglied.Name);
                }

                TableCell TC = new TableCell();
                TC.Controls.Add(DDL);
                TR.Cells.Add(TC);

                if (Verwalter.Users.Find(_ => _.SessionID == Request.Params["SessionID"]).Username.ToLower() == "admin")
                {
                    var editCell = new TableCell();
                    var editButton = new Button();
                    editButton.Text = "Edit";
                    editButton.ID = "Edit " + personIndex;
                    editButton.Click += EditButton_Click;
                    editCell.Controls.Add(editButton);

                    var deleteCell = new TableCell();
                    var deleteButton = new Button();
                    deleteButton.Text = "Delete";
                    deleteButton.ID = "Delete " + personIndex;
                    deleteButton.Click += DeleteButton_Click;
                    deleteCell.Controls.Add(deleteButton);

                    TR.Cells.Add(editCell);
                    TR.Cells.Add(deleteCell);
                }

                MannschaftsTable.Rows.Add(TR);
                personIndex++;
            }
        }
        private void LoadSportarten()
        {
            MannschaftSportArtenDropDownList.Items.Clear();
            MannschaftSportArtenDropDownList.Items.AddRange(
                Verwalter.Sportarten.Select(_ => new ListItem(_)).ToArray()
                );
        }
        private void EditButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == null)
                return;

            int index = Int32.Parse(button.ID.Split(' ')[1]);

            Mannschaft mannschaft = Verwalter.Mannschaften[index];
            Verwalter.MannschaftEdit = mannschaft;

            EingabeMannschaftsnameTextBox.Text = mannschaft.Name;

            foreach (var person in mannschaft.Mitglieder)
            {
                AusgewaehlteMitgliederListBox.Items.Add(person.Name);
            }

            MannschaftSportArtenDropDownList.SelectedValue = mannschaft.SportArt;
            EditConfirmButton.Visible = true;
            AddMannschaftButton.Visible = false;
            CancelButton.Visible = true;
            FormPanel.Visible = true;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;

            int index = Int32.Parse(button.ID.Split(' ')[1]);
            Mannschaft mannschaft = Verwalter.Mannschaften[index];
            mannschaft.Delete();
            Response.Redirect($"{Microservices.MannschaftsServicePage}?SessionID={Request.Params["SessionID"]}");
        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            User user = Verwalter.Users.Find(_ => _.SessionID == Request.Params["SessionID"]);

            if (user != null)
                user.LogOut();

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
                if (!Verwalter.Users.Any(_ => _.SessionID == gateSessionId))
                {
                    Response.Redirect($"{Microservices.GatewayPage}");
                }
            }
        }

        private void LoadMitglieder()
        {
            VerfuegbareMitgliederListBox.Items.Clear();

            List<Mitglied> tempList = new List<Mitglied>();
            foreach (Mitglied mitglied in Verwalter.Mitglieder)
            {
                tempList.Add(mitglied);
            }

            foreach (Mitglied mitglied in Verwalter.Mitglieder)
            {
                foreach (Mannschaft mannschaft in Verwalter.Mannschaften)
                {
                    if (mannschaft.Mitglieder.Contains(mitglied))
                    {
                        tempList.Remove(mitglied);
                    }
                    else
                    {
                        if (MannschaftSportArtenDropDownList.SelectedIndex == 0 && mitglied is Fussballspieler ||
                            MannschaftSportArtenDropDownList.SelectedIndex == 1 && mitglied is Tennisspieler ||
                            MannschaftSportArtenDropDownList.SelectedIndex == 2 && mitglied is Handballspieler ||
                            (mitglied is Physiotherapeut || mitglied is Trainer))
                        {

                        }
                        else
                        {
                            tempList.Remove(mitglied);
                        }
                    }
                }
            }

            VerfuegbareMitgliederListBox.Items.AddRange(tempList.Select((e) => new ListItem(e.Name)).ToArray());
        }
        private Mannschaft ValidationCheck()
        {
            List<bool> AllChecks = new List<bool>();

            if (EingabeMannschaftsnameTextBox.Text != "")
            {
                AllChecks.Add(true);
            }
            else
            {
                AllChecks.Add(false);
            }

            if (AusgewaehlteMitgliederListBox.Items.Count != 0)
            {
                AllChecks.Add(true);
            }
            else
            {
                AllChecks.Add(false);
            }

            if (!AllChecks.Contains(false))
            {
                Mannschaft erstellteMannschaft = new Mannschaft();
                erstellteMannschaft.Name = EingabeMannschaftsnameTextBox.Text;

                foreach (ListItem LI in AusgewaehlteMitgliederListBox.Items)
                {
                    foreach (Mitglied M in Verwalter.Mitglieder)
                    {
                        if (M.Name == LI.Value)
                        {
                            erstellteMannschaft.Mitglieder.Add(M);
                        }
                    }
                }
                erstellteMannschaft.SportArt = MannschaftSportArtenDropDownList.Text;
                Verwalter.Mannschaften.Add(erstellteMannschaft);
                AusgewaehlteMitgliederListBox.Items.Clear();
                VerfuegbareMitgliederListBox.Items.Clear();
                EingabeMannschaftsnameTextBox.Text = "";
                LoadMitglieder();
                LoadMannschaften();
                return erstellteMannschaft;
            }
            else
            {
                return null;
            }
        }

        protected void MitgliedEntfernenButton_Click(object sender, EventArgs e)
        {
            List<ListItem> myItems = new List<ListItem>();

            foreach (int myInd in AusgewaehlteMitgliederListBox.GetSelectedIndices())
            {
                myItems.Add(AusgewaehlteMitgliederListBox.Items[myInd]);
            }

            foreach (ListItem myListItem in myItems)
            {
                AusgewaehlteMitgliederListBox.Items.Remove(myListItem);
            }

            VerfuegbareMitgliederListBox.Items.AddRange(myItems.ToArray());

            AusgewaehlteMitgliederListBox.SelectedIndex = -1;
            VerfuegbareMitgliederListBox.SelectedIndex = -1;
        }
        protected void MitgliedAuswaehlenButton_Click(object sender, EventArgs e)
        {
            List<ListItem> myItems = new List<ListItem>();

            foreach (int myInd in VerfuegbareMitgliederListBox.GetSelectedIndices())
            {
                myItems.Add(VerfuegbareMitgliederListBox.Items[myInd]);
            }

            foreach (ListItem myListItem in myItems)
            {
                VerfuegbareMitgliederListBox.Items.Remove(myListItem);
            }

            AusgewaehlteMitgliederListBox.Items.AddRange(myItems.ToArray());


            VerfuegbareMitgliederListBox.SelectedIndex = -1;
            AusgewaehlteMitgliederListBox.SelectedIndex = -1;
        }

        protected void MannschaftenErstellenButton_Click(object sender, EventArgs e)
        {
            Mannschaft mannschaft = ValidationCheck();
            if (mannschaft == null)
                return;
            StatusLabel.Text = mannschaft.Post() ? "Erfolgreich" : "Fehlgeschlagen";
        }
        protected void SportartBestaetigenButton_Click(object sender, EventArgs e)
        {
            LoadMitglieder();
        }
        protected void ConfirmEditButton_Click(object sender, EventArgs e)
        {
            if (Verwalter.MannschaftEdit == null)
                return;
            string updateString = String.Empty;

            Verwalter.MannschaftEdit.Name = EingabeMannschaftsnameTextBox.Text;
            Verwalter.MannschaftEdit.SportArt = MannschaftSportArtenDropDownList.SelectedValue;
            Verwalter.MannschaftEdit.Mitglieder = Verwalter.Mitglieder
                .Where(_ => AusgewaehlteMitgliederListBox.Items.Contains(new ListItem(_.Id.ToString())))
                .ToList();
            if (Verwalter.MannschaftEdit.Put())
            {
                StatusLabel.Text = "Erfolgreich";
            }
            else
            {
                StatusLabel.Text = "Erfolgreich";
            }
            Verwalter.MannschaftEdit = null;
        }

        public string GetMitgliederverwaltungsLink()
            => $"{Microservices.MitgliederServicePage}?SessionID={Request.Params["SessionID"]}";
        public string GetMannschaftsverwaltungsLink()
            => $"#";

        protected void AddMannschaftButton_Click(object sender, EventArgs e)
        {
            FormPanel.Visible = true;
            AddMannschaftButton.Visible = false;
            CancelButton.Visible = true;
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            FormPanel.Visible = false;
            AddMannschaftButton.Visible = true;
            CancelButton.Visible = false;
            Verwalter.MannschaftEdit = null;
        }
    }
}