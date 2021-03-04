using SharedTypes;
using SharedTypes.Models;
using System;
using System.Web.UI.WebControls;

namespace LoginService.Views
{
    public partial class Verwaltung : System.Web.UI.Page
    {
        private Controllers.Controller _verwalter;
        public Controllers.Controller Verwalter { get => _verwalter; set => _verwalter = value; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Verwalter == null)
                Verwalter = Global.Verwalter;

            RedirectUnauthenticatedUser();
            PopulateTable();
            if (!IsPostBack)
            {
                PopulateDropDown();
            }
        }

        private void PopulateDropDown()
        {
            UserTypDropDown.Items.Clear();
            UserTypDropDown.Items.Add(new ListItem("User"));
            UserTypDropDown.Items.Add(new ListItem("Admin"));
        }

        private void PopulateTable()
        {
            for (int i = UserTable.Rows.Count; i > 1; i--)
            {
                UserTable.Rows.RemoveAt(i);
            }

            int userIndex = 0;
            foreach (User user in Verwalter.Users)
            {
                TableRow row = new TableRow();
                row.Cells.Add(new TableCell() { Text = user.Username });
                row.Cells.Add(new TableCell() { Text = user.Type.ToString() });
                UserTable.Rows.Add(row);

                var editCell = new TableCell();
                var editButton = new Button();
                editButton.Text = "Edit";
                editButton.ID = "Edit " + userIndex;
                editButton.Click += EditButtonClick;
                editCell.Controls.Add(editButton);

                var deleteCell = new TableCell();
                var deleteButton = new Button();
                deleteButton.Text = "Delete";
                deleteButton.ID = "Delete " + userIndex;
                deleteButton.Click += DeleteButtonClick;
                deleteCell.Controls.Add(deleteButton);

                row.Cells.Add(editCell);
                row.Cells.Add(deleteCell);
                userIndex++;
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
            string sessionId = Request.Params["SessionID"];
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                Response.Redirect($"{Microservices.GatewayPage}");
            }
            else
            {
                //If no User with that SessionID is known or if the user is not an admin.
                User user = Verwalter.Users.Find(_ => _.SessionID == sessionId);
                if (user == null || user.Type == UserType.User)
                {
                    Response.Redirect($"{Microservices.GatewayPage}");
                }
            }
        }
        public string GetUserverwaltungsLink()
            => $"{Microservices.LoginServiceVerwalterPage}?SessionID={Session.SessionID}";
        public string GetMitgliederverwaltungsLink()
            => $"{Microservices.MitgliederServicePage}?SessionID={Request.Params["SessionID"]}";
        public string GetMannschaftsverwaltungsLink()
            => $"{Microservices.MannschaftsServicePage}?SessionID={Request.Params["SessionID"]}";
        public string GetLoggedInUsername()
            => Verwalter.Users.Find(_ => _.SessionID == Request.Params["SessionID"])?.Username ?? "Unbekannt";

        protected void AddUserButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text) || string.IsNullOrWhiteSpace(PasswordTextBox.Text))
                return;
            User user = new User
            {
                Username = UsernameTextBox.Text,
                Password = PasswordTextBox.Text,
                Type = UserTypDropDown.SelectedValue == "Admin" ? UserType.Admin : UserType.User 
            };
            user.Post();
            string gateSessionId = Request.Params["SessionID"];
            Response.Redirect($"{Microservices.LoginServiceVerwalterPage}?SessionID={gateSessionId}");
        }
    }
}