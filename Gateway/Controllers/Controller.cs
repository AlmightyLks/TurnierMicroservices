using Gateway.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Gateway.Controllers
{
    public class Controller
    {
        #region Eigenschaften
        private List<User> _sessionUsers;
        #endregion

        #region Accessoren/Modifier
        public List<User> Users { get => _sessionUsers; set => _sessionUsers = value; }
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

        public void FetchUsers()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    Task<HttpResponseMessage> response = client.GetAsync($"https://localhost:44315/api/Message");
                    response.Wait();
                    Task<string> jsonStr = response.Result.Content.ReadAsStringAsync();
                    jsonStr.Wait();
                    Users = JsonConvert.DeserializeObject<List<User>>(jsonStr.Result);
                }
            }
            catch (Exception e)
            {

            }
        }
        #endregion

        #region Worker

        #endregion
    }
}