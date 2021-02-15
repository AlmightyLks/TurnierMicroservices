using SharedTypes.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace MitgliederService.Controllers
{
    public sealed class Controller
    {
        private List<User> users;

        public List<User> Users { get => users; set => users = value; }

        public Controller()
        {
            Users = new List<User>();
        }
        public Controller(Controller c)
        {
            Users = c.Users;
        }
        public void FetchUsers()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync($"https://localhost:44315/api/Message").GetAwaiter().GetResult();
                    string jsonStr = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    Users = JsonConvert.DeserializeObject<List<User>>(jsonStr);
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}