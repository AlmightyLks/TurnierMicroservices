﻿using SharedTypes.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SharedTypes;

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
        public void FetchUsers()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    Task<HttpResponseMessage> response = client.GetAsync($"{Microservices.LoginServiceApi}");
                    response.Wait();
                    Task<string> jsonStr = response.Result.Content.ReadAsStringAsync();
                    jsonStr.Wait();
                    Users = JsonConvert.DeserializeObject<List<User>>(jsonStr.Result, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
                }
            }
            catch (Exception e)
            {

            }
        }
        public User CheckCredentials(string username, string password)
        {
            User result = null;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    Task<HttpResponseMessage> response = client.GetAsync($"https://localhost:44315/api/Message?username={username}");
                    response.Wait();
                    Task<string> jsonStr = response.Result.Content.ReadAsStringAsync();
                    jsonStr.Wait();
                    result = JsonConvert.DeserializeObject<List<User>>(jsonStr.Result, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    })
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