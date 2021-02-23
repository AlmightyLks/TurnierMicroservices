using SharedTypes.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using SharedTypes;

namespace MitgliederService.Controllers
{
    public sealed class Controller
    {
        private List<User> _users;
        private List<Mitglied> _mitglieder;
        private List<string> _sportarten; 

        public List<User> Users { get => _users; set => _users = value; }
        public List<string> Sportarten { get => _sportarten; set => _sportarten = value; }
        public List<Mitglied> Mitglieder { get => _mitglieder; set => _mitglieder = value; }

        public Controller()
        {
            Users = new List<User>();
            Mitglieder = new List<Mitglied>();
            Sportarten = new List<string>()
            {
                "Fussball",
                "Handball",
                "Tennis"
            };
        }
        public Controller(Controller c)
        {
            Users = c.Users;
            Sportarten = c.Sportarten;
            Mitglieder = c.Mitglieder;
        }
        public void FetchUsers()
        {
            Users = FetchData<List<User>>(Microservices.LoginServiceApi) ?? Users;
        }

        public void FetchMitglieder()
        {
            Mitglieder = FetchData<List<Mitglied>>(Microservices.MitgliederServiceApi) ?? Mitglieder;
        }
        private T FetchData<T>(string target) where T : class
        {
            T result = null;
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
    }
}