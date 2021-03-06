﻿using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using SharedTypes;
using SharedTypes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace MannschaftsService.Controllers
{
    public class MessageController : ApiController
    {
        private const string databaseName = "ms_MannschaftsService";

        // GET: api/Message
        [HttpGet]
        [Route("api/message")]//Need this, because for whatever reason WebForms thinks its funny to not accept posts if I don't specify the route explicitly here
        public string Get()
        {
            try
            {
                List<Mannschaft> result = new List<Mannschaft>();

                //Why this mess?
                //Because we are not allowed to use ORM from i.e. EF Core, meaning we have to map our data ourselves, by hand with raw queries.
                List<Mannschaft> newMannschaften = new List<Mannschaft>();                                       //Alle Mannschaften
                Dictionary<int, string> dbMitglied = new Dictionary<int, string>();                                     //ID <Mitglied_ID & AnzahlSpiele>
                List<KeyValuePair<int, KeyValuePair<KeyValuePair<string, string>, List<Mitglied>>>> dbMannschaften = new List<KeyValuePair<int, KeyValuePair<KeyValuePair<string, string>, List<Mitglied>>>>();           //ID, <Name, Sportart>

                List<Mitglied> allMembers = FetchMitglieder();

                //Query Mannschaften
                MySqlDataReader rdr = QueryDB("select * from `mannschaft`", out MySqlConnection connection);

                //Alle Mannschaften fetchen
                while (rdr.Read())
                {
                    dbMannschaften.Add(new KeyValuePair<int, KeyValuePair<KeyValuePair<string, string>, List<Mitglied>>>((int)rdr["id"],
                        new KeyValuePair<KeyValuePair<string, string>, List<Mitglied>>(
                        new KeyValuePair<string, string>(rdr["Name"].ToString(), rdr["Sportart"].ToString()), new List<Mitglied>())));
                }
                connection.Close();

                //Query MannschaftMitglied
                rdr = QueryDB("select * from `mannschaftmitglied`", out connection);

                //Alle MannschaftMitglieden & Mannschaften erstellen fetchen
                while (rdr.Read())
                {
                    Mannschaft mannschaft = new Mannschaft();
                    mannschaft.Id = dbMannschaften.FirstOrDefault((el) => el.Key == (int)rdr["Mannschaft_ID"]).Key;
                    mannschaft.Name = dbMannschaften.FirstOrDefault((el) => el.Key == (int)rdr["Mannschaft_ID"]).Value.Key.Key;
                    mannschaft.SportArt = dbMannschaften.FirstOrDefault((el) => el.Key == (int)rdr["Mannschaft_ID"]).Value.Key.Value;
                    int mitgliedId = (int)rdr["Mitglied_ID"];
                    IEnumerable<Mitglied> alleMitglieder = allMembers.Where((member) => member.Id == mitgliedId);
                    mannschaft.Mitglieder.AddRange(alleMitglieder);
                    newMannschaften.Add(mannschaft);
                }

                foreach (var dbMannschaft in dbMannschaften)
                {
                    if (!newMannschaften.Any((el) => el.Name == dbMannschaft.Value.Key.Key))
                    {
                        var mannschaft = new Mannschaft();
                        mannschaft.Id = dbMannschaft.Key;
                        mannschaft.Name = dbMannschaft.Value.Key.Key;
                        mannschaft.SportArt = dbMannschaft.Value.Key.Value;
                        mannschaft.Mitglieder = new List<Mitglied>();
                        newMannschaften.Add(mannschaft);
                    }
                }

                rdr.Close();
                connection.Close();

                result.AddRange(newMannschaften);

                string resultJson = JsonConvert.SerializeObject(result, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All
                });
                return resultJson;
            }
            catch
            {
                return string.Empty;
            }
        }

        // POST: api/Message
        [HttpPost]
        [Route("api/message")]
        public bool Post([FromBody] string jsonStr)
        {
            try
            {
                Mannschaft mannschaft = JsonConvert.DeserializeObject<Mannschaft>(jsonStr, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All
                });

                _ = NonQueryDB($"insert into `mannschaft`(`Name`,`Sportart`) values('{mannschaft.Name}','{mannschaft.SportArt}')");
                MySqlDataReader reader = QueryDB("select max(id) as 'latestid' from mannschaft", out MySqlConnection connection);
                reader.Read();
                int latestId = (int)reader["latestid"];
                reader.Close();
                connection.Close();

                if (mannschaft.Mitglieder.Count != 0)
                {
                    string mannschaftsMitgliedInsert = "insert into `mannschaftMitglied`(`Mannschaft_ID`, `Mitglied_ID`) values ";
                    IEnumerable<string> edited = mannschaft.Mitglieder.Select(_ => $"('{latestId}', '{_.Id}')");
                    mannschaftsMitgliedInsert += string.Join(",", edited) + ";";
                    _ = NonQueryDB(mannschaftsMitgliedInsert);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        // PUT: api/Message/5
        [HttpPut]
        [Route("api/message/{id}")]
        public bool Put(int id, [FromBody] string jsonStr)
        {
            try
            {
                Mannschaft mannschaft = JsonConvert.DeserializeObject<Mannschaft>(jsonStr, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All
                });

                _ = NonQueryDB($"update `mannschaft` set `Name`='{mannschaft.Name}', `Sportart`='{mannschaft.SportArt}' where `id`='{mannschaft.Id}';");
                //mannschaftMitglied wird nicht geupdated.
                return true;
            }
            catch
            {
                return false;
            }
        }

        // DELETE: api/Message/5
        [HttpDelete]
        [Route("api/message/{id}")]
        public bool Delete(int id)
        {
            int result = NonQueryDB($"delete from `mannschaft` where `id`='{id}';");
            return result != 0;
        }

        public List<Mitglied> FetchMitglieder()
        {
            List<Mitglied> mitglieder = new List<Mitglied>();
            mitglieder.AddRange(FetchData<List<Trainer>>($"{Microservices.MitgliederServiceApi}?type=Trainer") ?? new List<Trainer>());
            mitglieder.AddRange(FetchData<List<Physiotherapeut>>($"{Microservices.MitgliederServiceApi}?type=Physiotherapeut") ?? new List<Physiotherapeut>());
            mitglieder.AddRange(FetchData<List<Tennisspieler>>($"{Microservices.MitgliederServiceApi}?type=Tennis") ?? new List<Tennisspieler>());
            mitglieder.AddRange(FetchData<List<Fussballspieler>>($"{Microservices.MitgliederServiceApi}?type=Fussball") ?? new List<Fussballspieler>());
            mitglieder.AddRange(FetchData<List<Handballspieler>>($"{Microservices.MitgliederServiceApi}?type=Handball") ?? new List<Handballspieler>());

            return mitglieder;
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
        private MySqlDataReader QueryDB(string sqlStr, out MySqlConnection connection)
        {
            connection = null;
            MySqlDataReader dataReader = null;

            try
            {
                connection = ConnectToDB();
                if (connection == null)
                    return dataReader;
                MySqlCommand cmd = new MySqlCommand(sqlStr, connection);
                dataReader = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                connection = null;
                dataReader = null;
            }

            return dataReader;
        }
        private int NonQueryDB(string sqlStr)
        {
            int result = -1;

            try
            {
                MySqlConnection conn = ConnectToDB();
                if (conn == null)
                    return result;
                MySqlCommand cmd = new MySqlCommand(sqlStr, conn);
                result = cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                result = -1;
            }

            return result;
        }
        private MySqlConnection ConnectToDB()
        {
            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnection($"server=127.0.0.1;uid=user;pwd=user;database={databaseName}");
                conn.Open();
            }
            catch (Exception e)
            {
                conn = null;
            }
            return conn;
        }
    }
}
