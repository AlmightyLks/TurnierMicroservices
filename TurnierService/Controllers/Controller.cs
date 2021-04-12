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
        private List<Mannschaft> _mannschaften;
        private List<Mitglied> _mitglieder;
        private List<User> _users;
        private User _loggedInUser;

        public List<User> Users { get => _users; set => _users = value; }
        public User LoggedInUser { get => _loggedInUser; set => _loggedInUser = value; }
        public List<Turnier> Turniere { get => _turniere; set => _turniere = value; }
        public List<Mannschaft> Mannschaften { get => _mannschaften; set => _mannschaften = value; }
        public List<Mitglied> Mitglieder { get => _mitglieder; set => _mitglieder = value; }

        public Controller()
        {
            Turniere = new List<Turnier>();
            Users = new List<User>();
            Mannschaften = new List<Mannschaft>();
            Mitglieder = new List<Mitglied>();
            LoggedInUser = null;
        }
        public Controller(Controller c)
        {
            Turniere = c.Turniere;
            Users = c.Users;
            Mannschaften = c.Mannschaften;
            LoggedInUser = c.LoggedInUser;
            Mitglieder = c.Mitglieder;
        }
        public void FetchUsers()
        {
            Users = FetchData<List<User>>(Microservices.LoginServiceApi) ?? Users;
        }

        public void FetchTurniere()
        {
            Turniere = FetchData<List<Turnier>>(Microservices.TurnierServiceApi) ?? Turniere;
        }
        public void FetchMannschaften()
        {
            Mannschaften = FetchData<List<Mannschaft>>(Microservices.MannschaftsServiceApi) ?? Mannschaften;
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