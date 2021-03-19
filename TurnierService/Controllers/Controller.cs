using Newtonsoft.Json;
using SharedTypes;
using SharedTypes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace TurnierService.Controllers
{
    public class Controller
    {
        private List<Turnier> _turniere;
        private List<User> _users;
        private User _loggedInUser;

        public List<User> Users { get => _users; set => _users = value; }
        public User LoggedInUser { get => _loggedInUser; set => _loggedInUser = value; }
        public List<Turnier> Turniere { get => _turniere; set => _turniere = value; }

        public Controller()
        {
            Turniere = new List<Turnier>();
            Users = new List<User>();
            LoggedInUser = null;
        }
        public Controller(Controller c)
        {
            Turniere = c.Turniere;
            Users = c.Users;
            LoggedInUser = c.LoggedInUser;
        }
        public void FetchUsers()
        {
            Users = FetchData<List<User>>(Microservices.LoginServiceApi) ?? Users;
        }

        public void FetchTurniere()
        {
            Turniere = FetchData<List<Turnier>>(Microservices.TurnierServiceApi) ?? Turniere;
        }

        private T FetchData<T>(string target) 
        {
            T result = default(T);
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync($"{target}").GetAwaiter().GetResult();
                    string jsonStr = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    result = JsonConvert.DeserializeObject<T>(jsonStr, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
                }
            }
            catch (Exception e)
            {

            }
            return result;
        }

        public void DeleteTurnier(Turnier turnier)
        {
            turnier.Delete();
        }
        public void PostTurnier(Turnier turnier)
        {
            turnier.Post();
        }
        public void PutTurnier(Turnier turnier)
        {
            turnier.Put();
        }
    }
}