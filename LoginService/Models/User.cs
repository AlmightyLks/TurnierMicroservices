using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LoginService.Models
{
    public class User
    {
        #region Eigenschaften
        private int _id;
        private string _username;
        private string _password;
        private bool _loggedIn;
        #endregion

        #region Accessoren/Modifier
        public int Id { get => _id; set => _id = value; }
        public string Username { get => _username; set => _username = value; }
        public string Password { get => _password; set => _password = value; }
        public bool LoggedIn { get => _loggedIn; set => _loggedIn = value; }
        #endregion

        #region Konstruktoren
        public User()
        {
            Id = 0;
            Username = string.Empty;
            Password = string.Empty;
            LoggedIn = false;
        }
        public User(int id, string username, string password, bool loggedIn)
        {
            Id = id;
            Username = username;
            Password = password;
            LoggedIn = loggedIn;
        }
        public User(User user)
        {
            Id = user.Id;
            Username = user.Username;
            Password = user.Password;
            LoggedIn = user.LoggedIn;
        }
        #endregion

        #region Worker
        public async Task LogIn()
        {
            LoggedIn = true;
            using (HttpClient client = new HttpClient())
            {
                StringContent jsonContent = new StringContent($"\"loginStatus\":1", Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync($"https://localhost:44315/api/Message/{Id}", jsonContent);
                string jsonStr = await response.Content.ReadAsStringAsync();
            }
        }
        public async Task LogOut()
        {
            LoggedIn = true;
            using (HttpClient client = new HttpClient())
            {
                StringContent jsonContent = new StringContent($"\"loginStatus\":0", Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync($"https://localhost:44315/api/Message/{Id}", jsonContent);
                string jsonStr = await response.Content.ReadAsStringAsync();
            }
        }
        #endregion
    }
}