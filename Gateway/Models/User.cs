using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Gateway.Models
{
    public class User
    {
        #region Eigenschaften
        private int _id;
        private string _username;
        private string _password;
        private string _sessionID;
        #endregion

        #region Accessoren/Modifier
        public int Id { get => _id; set => _id = value; }
        public string Username { get => _username; set => _username = value; }
        public string Password { get => _password; set => _password = value; }
        public string SessionID { get => _sessionID; set => _sessionID = value; }
        #endregion

        #region Konstruktoren
        public User()
        {
            Id = 0;
            Username = string.Empty;
            Password = string.Empty;
            SessionID = string.Empty;
        }
        public User(int id, string username, string password, string sessionID)
        {
            Id = id;
            Username = username;
            Password = password;
            SessionID = sessionID;
        }
        public User(User user)
        {
            Id = user.Id;
            Username = user.Username;
            Password = user.Password;
            SessionID = user.SessionID;
        }
        #endregion

        #region Worker
        public void LogIn(string sessionId)
        {
            SessionID = sessionId;
            using (HttpClient client = new HttpClient())
            {
                StringContent jsonContent = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
                Task<HttpResponseMessage> response = client.PutAsync($"https://localhost:44315/api/Message/{Id}", jsonContent);
                response.Wait();
                Task<string> jsonStr = response.Result.Content.ReadAsStringAsync();
            }
        }
        public void LogOut()
        {
            SessionID = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                StringContent jsonContent = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
                Task<HttpResponseMessage> response = client.PutAsync($"https://localhost:44315/api/Message/{Id}", jsonContent);
                response.Wait();
                Task<string> jsonStr = response.Result.Content.ReadAsStringAsync();
            }
        }
        #endregion
    }
}