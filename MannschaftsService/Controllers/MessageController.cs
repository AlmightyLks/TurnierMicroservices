using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using SharedTypes;
using SharedTypes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            List<Mannschaft> result = new List<Mannschaft>();

            var newMannschaften = new List<Mannschaft>();                                       //Alle Mannschaften
            var dbMitglied = new Dictionary<int, string>();                                     //ID <Mitglied_ID & AnzahlSpiele>
            var dbMannschaften = new List<KeyValuePair<int, KeyValuePair<KeyValuePair<string, string>, List<Mitglied>>>>();           //ID, <Name, Sportart>

            List<Mitglied> allMembers = FetchMitglieder();

            //Query Mannschaften
            var sqlRdr = QueryDB("select * from `mannschaft`");

            //Alle Mannschaften fetchen
            while (sqlRdr.DataReader.Read())
            {
                dbMannschaften.Add(new KeyValuePair<int, KeyValuePair<KeyValuePair<string, string>, List<Mitglied>>>((int)sqlRdr.DataReader["id"],
                    new KeyValuePair<KeyValuePair<string, string>, List<Mitglied>>(
                    new KeyValuePair<string, string>(sqlRdr.DataReader["Name"].ToString(), sqlRdr.DataReader["Sportart"].ToString()), new List<Mitglied>())));
            }

            //Query MannschaftMitglied
            sqlRdr = QueryDB("select * from `mannschaftmitglied`");

            //Alle MannschaftMitglieden & Mannschaften erstellen fetchen
            while (sqlRdr.DataReader.Read())
            {
                var mannschaft = new Mannschaft();
                mannschaft.Id = dbMannschaften.FirstOrDefault((el) => el.Key == (int)sqlRdr.DataReader["Mannschaft_ID"]).Key;
                mannschaft.Name = dbMannschaften.FirstOrDefault((el) => el.Key == (int)sqlRdr.DataReader["Mannschaft_ID"]).Value.Key.Key;
                mannschaft.SportArt = dbMannschaften.FirstOrDefault((el) => el.Key == (int)sqlRdr.DataReader["Mannschaft_ID"]).Value.Key.Value;
                var mitgliedId = (int)sqlRdr.DataReader["Mitglied_ID"];
                var alleMitglieder = allMembers.Where((member) => member.Id == mitgliedId);
                mannschaft.Mitglieder.AddRange(alleMitglieder);
                newMannschaften.Add(mannschaft);
            }

            foreach (var dbMannschaft in dbMannschaften)
            {
                if (!newMannschaften.Any((el) => el.Name == dbMannschaft.Value.Key.Key))
                {
                    var mannschaft = new Mannschaft();
                    mannschaft.Name = dbMannschaft.Value.Key.Key;
                    mannschaft.SportArt = dbMannschaft.Value.Key.Value;
                    mannschaft.Mitglieder = new List<Mitglied>();
                    newMannschaften.Add(mannschaft);
                }
            }

            sqlRdr.DataReader.Close();
            sqlRdr.Connection.Close();

            result.AddRange(newMannschaften);

            string resultJson = JsonConvert.SerializeObject(result, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            });
            return resultJson;
        }

        // POST: api/Message
        [HttpPost]
        [Route("api/message")] 
        public void Post([FromBody] string jsonStr)
        {
            Mannschaft mannschaft = JsonConvert.DeserializeObject<Mannschaft>(jsonStr, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            });

            _ = NonQueryDB($"insert into `mannschaft`(`Name`,`Sportart`) values('{mannschaft.Name}','{mannschaft.SportArt}')");
            (MySqlDataReader DataReader, MySqlConnection Connection) temp = QueryDB("select max(id) as 'latestid' from mannschaft");
            temp.DataReader.Read();
            int latestId = (int)temp.DataReader["latestid"];
            temp.DataReader.Close();
            temp.Connection.Close();

            if (mannschaft.Mitglieder.Count != 0)
            {
                string mannschaftsMitgliedInsert = "insert into `mannschaftMitglied`(`Mannschaft_ID`, `Mitglied_ID`) values ";
                IEnumerable<string> edited = mannschaft.Mitglieder.Select(_ => $"('{latestId}', '{_.Id}')");
                mannschaftsMitgliedInsert += string.Join(",", edited) + ";";
                _ = NonQueryDB(mannschaftsMitgliedInsert);
            }
        }

        // PUT: api/Message/5
        [HttpPut]
        [Route("api/message")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Message/5
        [HttpDelete]
        [Route("api/message/{id}")]
        public void Delete(int id)
        {
            NonQueryDB($"delete from `mannschaft` where `id`='{id}'");
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
        private (MySqlDataReader DataReader, MySqlConnection Connection) QueryDB(string sqlStr)
        {
            (MySqlDataReader DataReader, MySqlConnection Connection) result = (null, null);

            try
            {
                result.Connection = ConnectToDB();
                if (result.Connection == null)
                    return result;
                MySqlCommand cmd = new MySqlCommand(sqlStr, result.Connection);
                result.DataReader = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                result = (null, null);
            }

            return result;
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
