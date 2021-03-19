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
        private User _loggedInUser;

        public List<User> Users { get => _users; set => _users = value; }
        public List<string> Sportarten { get => _sportarten; set => _sportarten = value; }
        public List<Mitglied> Mitglieder { get => _mitglieder; set => _mitglieder = value; }
        public User LoggedInUser { get => _loggedInUser; set => _loggedInUser = value; }

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
            List<Mitglied> mitglieder = new List<Mitglied>();
            mitglieder.AddRange(FetchData<List<Trainer>>($"{Microservices.MitgliederServiceApi}?type=Trainer") ?? new List<Trainer>());
            mitglieder.AddRange(FetchData<List<Physiotherapeut>>($"{Microservices.MitgliederServiceApi}?type=Physiotherapeut") ?? new List<Physiotherapeut>());
            mitglieder.AddRange(FetchData<List<Tennisspieler>>($"{Microservices.MitgliederServiceApi}?type=Tennis") ?? new List<Tennisspieler>());
            mitglieder.AddRange(FetchData<List<Fussballspieler>>($"{Microservices.MitgliederServiceApi}?type=Fussball") ?? new List<Fussballspieler>());
            mitglieder.AddRange(FetchData<List<Handballspieler>>($"{Microservices.MitgliederServiceApi}?type=Handball") ?? new List<Handballspieler>());
            if (mitglieder.Count != 0)
            {
                Mitglieder = mitglieder;
            }
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

        public void DeleteMember(Mitglied mitglied)
        {
            mitglied.Delete();
            Mitglieder.Remove(mitglied);
        }
        public void PutMember(Mitglied mitglied)
        {
            mitglied.Put();
        }

        internal void PostMember(Mitglied mitglied)
        {
            mitglied.Post();
        }
    }
}