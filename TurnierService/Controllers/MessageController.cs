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

namespace TurnierService.Controllers
{
    public class MessageController : ApiController
    {
        private const string databaseName = "ms_TurnierService";

        // GET: api/Message
        public IEnumerable<Turnier> Get()
        {
            List<Turnier> result = new List<Turnier>();

            MySqlDataReader reader = QueryDB("select * from turnier;", out MySqlConnection connection);

            if (reader == null || connection == null || !reader.HasRows)
            {
                reader?.Close();
                connection?.Close();
                return result;
            }

            while (reader.Read())
            {
                Turnier turnier = new Turnier();
                turnier.Id = (int)reader["id"];
                turnier.Titel = reader["titel"].ToString();
                turnier.Sportart = reader["sportart"].ToString();
                result.Add(turnier);
            }
            reader?.Close();
            connection?.Close();


            reader = QueryDB("select * from TurnierSpiel;", out connection);

            if (reader == null || connection == null || !reader.HasRows)
            {
                reader?.Close();
                connection?.Close();
                return result;
            }

            while (reader.Read())
            {
                using (HttpClient client = new HttpClient())
                {
                    Turnier turnier = result.Find(_ => _.Id == (int)reader["TurnierId"]);
                    if (turnier == null)
                    {
                        continue;
                    }
                    Spiel spiel = new Spiel();
                    spiel.ErsterTeilnehmer = new TurnierTeilnehmer()
                    {
                        TeilnehmerId = (int)reader["ErsterTeilnehmerId"],
                        Punkte = (int)reader["ErsterTeilnehmerPunkte"]
                    };
                    spiel.ZweiterTeilnehmer = new TurnierTeilnehmer()
                    {
                        TeilnehmerId = (int)reader["ZweiterTeilnehmerId"],
                        Punkte = (int)reader["ZweiterTeilnehmerPunkte"]
                    };
                    spiel.Id = (int)reader["Id"];
                    spiel.Position = (int)reader["Position"];

                    turnier.Spiele.Add(spiel);
                }
            }

            reader?.Close();
            connection?.Close();

            return result;
        }

        // POST: api/Message
        public void Post([FromBody] Turnier value)
        {
            string insertQuery = string.Empty;

            insertQuery += $"INSERT INTO `turnier`(`Titel`, `Sportart`) VALUES ('{value.Titel}','{value.Sportart}');";

            if (value.Spiele.Count != 0)
            {
                insertQuery += $"INSERT INTO `turnierspiel`(`TurnierId`, `ErsterTeilnehmerId`, `ErsterTeilnehmerPunkte`, `ZweiterTeilnehmerId`, `ZweiterTeilnehmerPunkte`, `Position`) VALUES ";
                insertQuery += $"{string.Join(",", value.Spiele.Select(_ => $"('{value.Id}', '{_.ErsterTeilnehmer.TeilnehmerId}', '{_.ErsterTeilnehmer.Punkte}', '{_.ZweiterTeilnehmer.TeilnehmerId}', '{_.ZweiterTeilnehmer.Punkte}', '{_.Position}')"))};";
            }

            _ = NonQueryDB(insertQuery);
        }

        // PUT: api/Message/5
        public void Put(int id, [FromBody] Turnier value)
        {
            string updateStr = string.Empty;

            updateStr += $"update `turnier` set `Titel`='{value.Titel}', `Sportart`='{value.Sportart}' where `id`='{value.Id}';";
            updateStr += $"delete from `turnierspiel` where `TurnierId`='{id}';";

            if (value.Spiele.Count != 0)
            {
                updateStr += $"INSERT INTO `turnierspiel`(`TurnierId`, `ErsterTeilnehmerId`, `ErsterTeilnehmerPunkte`, `ZweiterTeilnehmerId`, `ZweiterTeilnehmerPunkte`, `Position`) VALUES ";
                updateStr += $"{string.Join(",", value.Spiele.Select(_ => $"('{value.Id}', '{_.ErsterTeilnehmer.TeilnehmerId}', '{_.ErsterTeilnehmer.Punkte}', '{_.ZweiterTeilnehmer.TeilnehmerId}', '{_.ZweiterTeilnehmer.Punkte}', '{_.Position}')"))};";
            }

            _ = NonQueryDB(updateStr);
        }

        // DELETE: api/Message/5
        public void Delete(int id)
        {
            _ = NonQueryDB($"delete from `turnier` where `id`='{id}'");
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
