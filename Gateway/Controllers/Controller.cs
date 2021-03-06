﻿using SharedTypes.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using SharedTypes;

namespace Gateway.Controllers
{
    public class Controller
    {
        #region Eigenschaften
        private List<User> _sessionUsers;
        private User _loggedInUser;
        #endregion

        #region Accessoren/Modifier
        public List<User> Users { get => _sessionUsers; set => _sessionUsers = value; }
        public User LoggedInUser { get => _loggedInUser; set => _loggedInUser = value; }
        #endregion

        #region Konstruktoren
        public Controller()
        {
            LoggedInUser = null;
            Users = new List<User>();
        }
        public Controller(Controller controller)
        {
            LoggedInUser = controller.LoggedInUser;
            Users = controller.Users;
        }
        #endregion

        #region Worker
        public void FetchUsers()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync($"{Microservices.LoginServiceApi}").GetAwaiter().GetResult();
                    string jsonStr = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    Users = JsonConvert.DeserializeObject<List<User>>(jsonStr, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
                }
            }
            catch (Exception e)
            {

            }
        }
        #endregion
    }
}