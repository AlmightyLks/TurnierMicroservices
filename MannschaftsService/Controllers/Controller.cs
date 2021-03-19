using Newtonsoft.Json;
using SharedTypes;
using SharedTypes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web;

namespace MannschaftsService.Controllers
{
    public class Controller
    {
        #region Eigenschaften
        private List<User> _users;
        private List<Mannschaft> _mannschaften;
        private List<Mitglied> _mitglieder;
        private List<string> _sportarten;
        private Mannschaft _mannschaftEdit;
        private User _loggedInUser;
        #endregion

        #region Accessoren/Modifier
        public List<Mannschaft> Mannschaften { get => _mannschaften; set => _mannschaften = value; }
        public List<User> Users { get => _users; set => _users = value; }
        public List<Mitglied> Mitglieder { get => _mitglieder; set => _mitglieder = value; }
        public List<string> Sportarten { get => _sportarten; set => _sportarten = value; }
        public Mannschaft MannschaftEdit { get => _mannschaftEdit; set => _mannschaftEdit = value; }
        public User LoggedInUser { get => _loggedInUser; set => _loggedInUser = value; }
        #endregion

        #region Konstruktoren
        public Controller()
        {
            Mannschaften = new List<Mannschaft>();
            Users = new List<User>();
            Mitglieder = new List<Mitglied>();
            MannschaftEdit = null;
            Sportarten = new List<string>()
            {
                "Fussball",
                "Handball",
                "Tennis"
            };
            LoggedInUser = null;
        }
        public Controller(Controller C)
        {
            Mannschaften = C.Mannschaften;
            Users = C.Users;
            Mitglieder = C.Mitglieder;
            Sportarten = C.Sportarten;
            MannschaftEdit = C.MannschaftEdit;
            LoggedInUser = C.LoggedInUser;
        }
        #endregion

        #region Worker
        public void FetchMannschaften()
        {
            List<Mannschaft> mannschaften = new List<Mannschaft>();
            mannschaften.AddRange(FetchMannschaften($"{Microservices.MannschaftsServiceApi}") ?? new List<Mannschaft>());
            if (mannschaften.Count != 0)
            {
                Mannschaften = mannschaften;
            }
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
        public void FetchUsers()
        {
            Users = FetchData<List<User>>(Microservices.LoginServiceApi) ?? Users;
        }

        private List<Mannschaft> FetchMannschaften(string target)
        {
            List<Mannschaft> result = null;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync($"{target}").GetAwaiter().GetResult();
                    string jsonStr = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var jsonStrStr = JsonConvert.DeserializeObject<string>(jsonStr);
                    result = JsonConvert.DeserializeObject<List<Mannschaft>>(jsonStrStr, new JsonSerializerSettings
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
        #endregion
    }
}