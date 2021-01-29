using LoginService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace LoginService.Controllers
{
    public class Controller
    {
        #region Eigenschaften
        private List<User> _users;
        #endregion

        #region Accessoren/Modifier
        public List<User> Users { get => _users; set => _users = value; }
        #endregion

        #region Konstruktoren
        public Controller()
        {
            Users = new List<User>();
        }
        public Controller(Controller controller)
        {
            Users = controller.Users;
        }
        #endregion

        #region Worker
        public async Task<User> CheckCredentials(string username, string password)
        {
            User result = null;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync($"https://localhost:44315/api/Message?username={username}");
                    string jsonStr = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<List<User>>(jsonStr)
                        .Find(_ => _.Password == password);
                }
            }
            catch (Exception e)
            {

            }
            return result;
        }
        #endregion
    }
}