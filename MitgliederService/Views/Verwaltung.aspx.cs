using SharedTypes;
using SharedTypes.Models;
using System;
using System.Collections.Generic;
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

            if (!IsPostBack)
            {
                LoadSportarten();
            }
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
            THR.Cells.Add(new TableCell { Text = "Anzahl gespielter Spiele" });
            THR.Cells.Add(new TableCell { Text = "" });
            THR.Cells.Add(new TableCell { Text = "" });

            MyTable.Rows.Add(THR);

            Verwalter.FetchMitglieder();

            int MitgliedIndex = 0;

            foreach (Mitglied Mitglied in Verwalter.Mitglieder)
            {
                TableRow TR = new TableRow();

                TR.Cells.Add(new TableCell { Text = Mitglied.Name });

                if (Mitglied is Fussballspieler)
                {
                    TR.Cells.Add(new TableCell { Text = "Fussballspieler" });
                    TR.Cells.Add(new TableCell { Text = (Mitglied as Fussballspieler).Position });
                    TR.Cells.Add(new TableCell { Text = "-" });
                }
                else if (Mitglied is Handballspieler)
                {
                    TR.Cells.Add(new TableCell { Text = "Handballspieler" });
                    TR.Cells.Add(new TableCell { Text = (Mitglied as Handballspieler).Position });
                    TR.Cells.Add(new TableCell { Text = "-" });
                }
                else if (Mitglied is Tennisspieler)
                {
                    TR.Cells.Add(new TableCell { Text = "Tennisspieler" });
                    TR.Cells.Add(new TableCell { Text = "-" });
                    TR.Cells.Add(new TableCell { Text = (Mitglied as Tennisspieler).JahreErfahrung.ToString() });
                }
                else if (Mitglied is Physiotherapeut)
                {
                    TR.Cells.Add(new TableCell { Text = "Physiotherapeut" });
                    TR.Cells.Add(new TableCell { Text = "-" });
                    TR.Cells.Add(new TableCell { Text = "-" });
                }
                else if (Mitglied is Trainer)
                {
                    TR.Cells.Add(new TableCell { Text = "Trainer" });
                    TR.Cells.Add(new TableCell { Text = "-" });
                    TR.Cells.Add(new TableCell { Text = "-" });
                }
                if (Mitglied is Spieler spieler)
                {
                    TR.Cells.Add(new TableCell { Text = spieler.AnzahlSpiele.ToString() });
                }
                else
                {
                    TR.Cells.Add(new TableCell { Text = "-" });
                }

                var editCell = new TableCell();
                var editButton = new Button();
                editButton.Text = "Edit";
                editButton.ID = "Edit " + MitgliedIndex;
                editButton.Click += EditButtonClick;
                editCell.Controls.Add(editButton);

                var deleteCell = new TableCell();
                var deleteButton = new Button();
                deleteButton.Text = "Delete";
                deleteButton.ID = "Delete " + MitgliedIndex;
                deleteButton.Click += DeleteButtonClick;
                deleteCell.Controls.Add(deleteButton);

                TR.Cells.Add(editCell);
                TR.Cells.Add(deleteCell);

                MyTable.Rows.Add(TR);
                MitgliedIndex++;
            }
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
            AddMemberConfirmButton.Visible = false;
            EditMemberConfirmButton.Visible = false;
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            FormPanel.Visible = false;
            AddMemberButton.Visible = true;
            CancelButton.Visible = false;
            AddMemberConfirmButton.Visible = false;
            EditMemberConfirmButton.Visible = false;
        }
        private void DeleteButtonClick(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int index = int.Parse(button.ID.Split(' ')[1]);
            Verwalter.DeleteMember(Verwalter.Mitglieder[index]);
            string gateSessionId = Request.Params["SessionID"];
            Response.Redirect($"{Microservices.MitgliederServicePage}?SessionID={gateSessionId}");
        }

        private void EditButtonClick(object sender, EventArgs e)
        {
            try
            {
                FormPanel.Visible = true;
                Button button = sender as Button;
                int index = int.Parse(button.ID.Split(' ')[1]);

                Mitglied mitglied = Verwalter.Mitglieder[index];
                ToggleUIInputByType(mitglied);
                PlaceEditInfo(mitglied);
                EditMemberConfirmButton.Visible = true;
            }
            catch (Exception ex)
            {

            }
        }

        protected void EditMemberConfirmButton_Click(object sender, EventArgs e)
        {
            List<TextBox> textboxen = new List<TextBox>();
            textboxen.Add(MitgliedNameTextBox);
            textboxen.Add(MitgliedAnzahlSpieleTextBox);
            textboxen.Add(MitgliedPositionTextBox);
            textboxen.Add(MitgliedErfahrungTextBox);
            if (textboxen.Any(_ => _.Enabled && string.IsNullOrWhiteSpace(_.Text)))
                return;

            try
            {
                Button button = sender as Button;
                int id = int.Parse(MitgliedIdTextBox.Text);
                Mitglied mitglied = Verwalter.Mitglieder.Find(_ => _.Id == id);

                if (mitglied is Fussballspieler fussballspieler)
                {
                    fussballspieler.Name = MitgliedNameTextBox.Text;
                    fussballspieler.AnzahlSpiele = int.Parse(MitgliedAnzahlSpieleTextBox.Text);
                    fussballspieler.Position = MitgliedPositionTextBox.Text;
                }
                else if (mitglied is Handballspieler handballspieler)
                {
                    handballspieler.Name = MitgliedNameTextBox.Text;
                    handballspieler.AnzahlSpiele = int.Parse(MitgliedAnzahlSpieleTextBox.Text);
                    handballspieler.Position = MitgliedPositionTextBox.Text;
                }
                else if (mitglied is Tennisspieler tennisspieler)
                {
                    tennisspieler.Name = MitgliedNameTextBox.Text;
                    tennisspieler.AnzahlSpiele = int.Parse(MitgliedAnzahlSpieleTextBox.Text);
                    tennisspieler.JahreErfahrung = int.Parse(MitgliedErfahrungTextBox.Text);
                }
                else if (mitglied is Trainer trainer)
                {
                    trainer.Name = MitgliedNameTextBox.Text;
                }
                else if (mitglied is Physiotherapeut physiotherapeut)
                {
                    physiotherapeut.Name = MitgliedNameTextBox.Text;
                }

                Verwalter.PutMember(mitglied);
                EditMemberConfirmButton.Visible = false;
            }
            catch (Exception ex)
            {

            }

            FormPanel.Visible = false;
            AddMemberButton.Visible = true;
            CancelButton.Visible = false;
            AddMemberConfirmButton.Visible = false;
            EditMemberConfirmButton.Visible = false;
            string gateSessionId = Request.Params["SessionID"];
            Response.Redirect($"{Microservices.MitgliederServicePage}?SessionID={gateSessionId}");
        }

        protected void AddMemberConfirmButton_Click(object sender, EventArgs e)
        {
            List<TextBox> textboxen = new List<TextBox>();
            textboxen.Add(MitgliedNameTextBox);
            textboxen.Add(MitgliedAnzahlSpieleTextBox);
            textboxen.Add(MitgliedPositionTextBox);
            textboxen.Add(MitgliedErfahrungTextBox);
            if (textboxen.Any(_ => _.Enabled && string.IsNullOrWhiteSpace(_.Text)))
                return;

            Mitglied mitglied = null;

            switch (AddPersonTypeRadioButtons.SelectedValue)
            {
                case "Physiotherapeut":
                    mitglied = new Physiotherapeut()
                    {
                        Id = 0,
                        EigeneMannschaft = null,
                        Name = MitgliedNameTextBox.Text
                    };
                    break;
                case "Trainer":
                    mitglied = new Trainer()
                    {
                        Id = 0,
                        EigeneMannschaft = null,
                        Name = MitgliedNameTextBox.Text
                    };
                    break;
                case "Spieler" when SportArtenListe.SelectedItem.Value == "Fussball":
                    mitglied = new Fussballspieler()
                    {
                        Id = 0,
                        Name = MitgliedNameTextBox.Text,
                        AnzahlSpiele = int.Parse(MitgliedAnzahlSpieleTextBox.Text),
                        Position = MitgliedPositionTextBox.Text,
                        Sportart = "Fussball"
                    };
                    break;
                case "Spieler" when SportArtenListe.SelectedItem.Value == "Tennis":
                    mitglied = new Tennisspieler()
                    {
                        Id = 0,
                        Name = MitgliedNameTextBox.Text,
                        AnzahlSpiele = int.Parse(MitgliedAnzahlSpieleTextBox.Text),
                        JahreErfahrung = int.Parse(MitgliedErfahrungTextBox.Text),
                        Sportart = "Tennis"
                    };
                    break;
                case "Spieler" when SportArtenListe.SelectedItem.Value == "Handball":
                    mitglied = new Handballspieler()
                    {
                        Id = 0,
                        Name = MitgliedNameTextBox.Text,
                        AnzahlSpiele = int.Parse(MitgliedAnzahlSpieleTextBox.Text),
                        Position = MitgliedPositionTextBox.Text,
                        Sportart = "Handball"
                    };
                    break;
            }

            if (mitglied != null)
            {
                Verwalter.PostMember(mitglied);
            }
            FormPanel.Visible = false;
            AddMemberButton.Visible = true;
            CancelButton.Visible = false;
            AddMemberConfirmButton.Visible = false;
            EditMemberConfirmButton.Visible = false;
            string gateSessionId = Request.Params["SessionID"];
            Response.Redirect($"{Microservices.MitgliederServicePage}?SessionID={gateSessionId}");
        }

        private void PlaceEditInfo(Mitglied mitglied)
        {
            MitgliedIdTextBox.Text = mitglied.Id.ToString();
            MitgliedNameTextBox.Text = mitglied.Name;

            if (mitglied is Fussballspieler fussballspieler)
            {
                MitgliedAnzahlSpieleTextBox.Text = fussballspieler.AnzahlSpiele.ToString();
                MitgliedPositionTextBox.Text = fussballspieler.Position;
                MitgliedTypLabel.Text = "Fussballspieler";
            }
            else if (mitglied is Handballspieler handballspieler)
            {
                MitgliedAnzahlSpieleTextBox.Text = handballspieler.AnzahlSpiele.ToString();
                MitgliedPositionTextBox.Text = handballspieler.Position;
                MitgliedTypLabel.Text = "Handballspieler";
            }
            else if (mitglied is Tennisspieler tennisspieler)
            {
                MitgliedAnzahlSpieleTextBox.Text = tennisspieler.AnzahlSpiele.ToString();
                MitgliedErfahrungTextBox.Text = tennisspieler.JahreErfahrung.ToString();
                MitgliedTypLabel.Text = "Tennisspieler";
            }
        }
        protected void ConfirmSportart_Click(object sender, EventArgs e)
        {
            ToggleUIInputByElements();
        }
        private void ToggleUIInputByElements()
        {
            MitgliedNameLabel.Enabled = true;
            MitgliedNameTextBox.Enabled = true;
            MitgliedAnzahlSpieleLabel.Enabled = false;
            MitgliedAnzahlSpieleTextBox.Enabled = false;
            MitgliedErfahrungLabel.Enabled = false;
            MitgliedErfahrungTextBox.Enabled = false;
            MitgliedPositionLabel.Enabled = false;
            MitgliedPositionTextBox.Enabled = false;
            AddMemberConfirmButton.Visible = true;

            switch (AddPersonTypeRadioButtons.SelectedValue)
            {
                case "Spieler" when SportArtenListe.SelectedValue == "Fussball":
                    MitgliedAnzahlSpieleLabel.Enabled = true;
                    MitgliedAnzahlSpieleTextBox.Enabled = true;
                    MitgliedPositionLabel.Enabled = true;
                    MitgliedPositionTextBox.Enabled = true;
                    MitgliedTypLabel.Text = "Fussballspieler";
                    break;
                case "Spieler" when SportArtenListe.SelectedValue == "Tennis":
                    MitgliedAnzahlSpieleLabel.Enabled = true;
                    MitgliedAnzahlSpieleTextBox.Enabled = true;
                    MitgliedErfahrungLabel.Enabled = true;
                    MitgliedErfahrungTextBox.Enabled = true;
                    MitgliedTypLabel.Text = "Tennisspieler";
                    break;
                case "Spieler" when SportArtenListe.SelectedValue == "Handball":
                    MitgliedAnzahlSpieleLabel.Enabled = true;
                    MitgliedAnzahlSpieleTextBox.Enabled = true;
                    MitgliedPositionLabel.Enabled = true;
                    MitgliedPositionTextBox.Enabled = true;
                    MitgliedTypLabel.Text = "Handballspieler";
                    break;
            }
        }
        private void ToggleUIInputByType(Mitglied mitglied)
        {
            MitgliedNameLabel.Enabled = true;
            MitgliedNameTextBox.Enabled = true;
            MitgliedAnzahlSpieleLabel.Enabled = false;
            MitgliedAnzahlSpieleTextBox.Enabled = false;
            MitgliedErfahrungLabel.Enabled = false;
            MitgliedErfahrungTextBox.Enabled = false;
            MitgliedPositionLabel.Enabled = false;
            MitgliedPositionTextBox.Enabled = false;
            AddMemberConfirmButton.Visible = false;

            if (mitglied is Fussballspieler)
            {
                MitgliedAnzahlSpieleLabel.Enabled = true;
                MitgliedAnzahlSpieleTextBox.Enabled = true;
                MitgliedPositionLabel.Enabled = true;
                MitgliedPositionTextBox.Enabled = true;
                MitgliedTypLabel.Text = "Fussballspieler";
            }
            else if (mitglied is Handballspieler handballspieler)
            {
                MitgliedAnzahlSpieleLabel.Enabled = true;
                MitgliedAnzahlSpieleTextBox.Enabled = true;
                MitgliedPositionLabel.Enabled = true;
                MitgliedPositionTextBox.Enabled = true;
                MitgliedTypLabel.Text = "Handballspieler";
            }
            else if (mitglied is Tennisspieler tennisspieler)
            {
                MitgliedAnzahlSpieleLabel.Enabled = true;
                MitgliedAnzahlSpieleTextBox.Enabled = true;
                MitgliedErfahrungLabel.Enabled = true;
                MitgliedErfahrungTextBox.Enabled = true;
                MitgliedTypLabel.Text = "Tennisspieler";
            }
        }
    }
}