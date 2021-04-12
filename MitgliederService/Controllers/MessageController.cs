using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using SharedTypes.Models;
using SharedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace MitgliederService.Controllers
{
    public class MessageController : ApiController
    {
        private const string databaseName = "ms_MitgliederService";

        // GET: api/Message?type=fussball
        public IEnumerable<Mitglied> Get(string type)
        {
            IEnumerable<Mitglied> mitglieder = new List<Mitglied>();
            try
            {
                var newMembers = new List<KeyValuePair<int, Mitglied>>();                           //Alle Leute
                switch (type)
                {
                    case "Fussball":
                        {
                            //Why this mess?
                            //Because we are not allowed to use ORM from i.e. EF Core, meaning we have to map our data ourselves, by hand with raw queries.
                            Dictionary<int, string> dbFussballspieler = new Dictionary<int, string>();                              //Spieler_ID & Position
                            Dictionary<int, KeyValuePair<int, int>> dbSpieler = new Dictionary<int, KeyValuePair<int, int>>();                      //ID <Mitglied_ID & AnzahlSpiele>
                            Dictionary<int, string> dbMitglied = new Dictionary<int, string>();                                     //ID <Mitglied_ID & AnzahlSpiele>

                            //Query Fussballspieler
                            MySqlDataReader reader = QueryDB($"Select * from `fussballspieler`", out MySqlConnection connection);

                            while (reader.Read())
                                dbFussballspieler.Add((int)reader["Spieler_ID"], reader["Position"].ToString());

                            connection.Close();

                            //Query Spieler
                            reader = QueryDB($"Select * from `spieler`", out connection);

                            while (reader.Read()) //ID <Mitglied_ID & AnzahlSpiele>
                                dbSpieler.Add((int)reader["ID"], new KeyValuePair<int, int>((int)reader["Mitglied_ID"], (int)reader["AnzahlSpiele"]));


                            connection.Close();
                            //Query Mitglied
                            reader = QueryDB($"Select * from `mitglied`", out connection);

                            while (reader.Read())
                                dbMitglied.Add((int)reader["ID"], reader["Name"].ToString());

                            foreach (KeyValuePair<int, string> fussballspieler in dbFussballspieler)
                            {
                                newMembers.Add(new KeyValuePair<int, Mitglied>(fussballspieler.Key, new Fussballspieler()
                                {
                                    Position = fussballspieler.Value,
                                    Sportart = "Fussball",
                                    AnzahlSpiele = dbSpieler.First(e => e.Key == fussballspieler.Key).Value.Value,
                                    Name = dbMitglied.First(e => e.Key == dbSpieler.First(el => el.Key == fussballspieler.Key).Value.Key).Value,
                                    Id = dbSpieler.First(_ => _.Key == fussballspieler.Key).Value.Key
                                }));
                            }

                            reader.Close();
                            connection.Close();
                            break;
                        }

                    case "Handball":
                        {
                            //Why this mess?
                            //Because we are not allowed to use ORM from i.e. EF Core, meaning we have to map our data ourselves, by hand with raw queries.
                            Dictionary<int, string> dbHandballspieler = new Dictionary<int, string>();                              //Spieler_ID & Position
                            Dictionary<int, KeyValuePair<int, int>> dbSpieler = new Dictionary<int, KeyValuePair<int, int>>();      //ID <Mitglied_ID & AnzahlSpiele>
                            Dictionary<int, string> dbMitglied = new Dictionary<int, string>();                                     //ID <Mitglied_ID & AnzahlSpiele>

                            //Query Handballspieler
                            MySqlDataReader reader = QueryDB($"Select * from `handballspieler`", out MySqlConnection connection);

                            while (reader.Read())
                                dbHandballspieler.Add((int)reader["Spieler_ID"], reader["Position"].ToString());

                            connection.Close();

                            //Query Spieler
                            reader = QueryDB($"Select * from `spieler`", out connection);

                            while (reader.Read()) //ID <Mitglied_ID & AnzahlSpiele>
                                dbSpieler.Add((int)reader["ID"], new KeyValuePair<int, int>((int)reader["Mitglied_ID"], (int)reader["AnzahlSpiele"]));

                            connection.Close();

                            //Query Mitglied
                            reader = QueryDB($"Select * from `mitglied`", out connection);

                            while (reader.Read())
                                dbMitglied.Add((int)reader["ID"], reader["Name"].ToString());


                            foreach (KeyValuePair<int, string> handballspieler in dbHandballspieler)
                            {
                                newMembers.Add(new KeyValuePair<int, Mitglied>(handballspieler.Key, new Handballspieler()
                                {
                                    Position = handballspieler.Value,
                                    Sportart = "Handball",
                                    AnzahlSpiele = dbSpieler.First(e => e.Key == handballspieler.Key).Value.Value,
                                    Name = dbMitglied.First(e => e.Key == dbSpieler.First(el => el.Key == handballspieler.Key).Value.Key).Value,
                                    Id = dbSpieler.First(_ => _.Key == handballspieler.Key).Value.Key
                                }));
                            }

                            reader.Close();
                            connection.Close();
                            break;
                        }

                    case "Tennis":
                        {
                            //Why this mess?
                            //Because we are not allowed to use ORM from i.e. EF Core, meaning we have to map our data ourselves, by hand with raw queries.
                            Dictionary<int, int> dbTennisspieler = new Dictionary<int, int>();                                   //Spieler_ID & JahreErfahrung
                            Dictionary<int, KeyValuePair<int, int>> dbSpieler = new Dictionary<int, KeyValuePair<int, int>>();   //ID <Mitglied_ID & AnzahlSpiele>
                            Dictionary<int, string> dbMitglied = new Dictionary<int, string>();                                  //ID <Mitglied_ID & AnzahlSpiele>

                            //Query Tennisspieler
                            MySqlDataReader reader = QueryDB($"Select * from `tennisspieler`", out MySqlConnection connection);

                            while (reader.Read())
                                dbTennisspieler.Add((int)reader["Spieler_ID"], (int)reader["JahreErfahrung"]);

                            connection.Close();

                            //Query Spieler
                            reader = QueryDB($"Select * from `spieler`", out connection);

                            while (reader.Read()) //ID <Mitglied_ID & AnzahlSpiele>
                                dbSpieler.Add((int)reader["ID"], new KeyValuePair<int, int>((int)reader["Mitglied_ID"], (int)reader["AnzahlSpiele"]));

                            connection.Close();

                            //Query Mitglied
                            reader = QueryDB($"Select * from `mitglied`", out connection);

                            while (reader.Read())
                                dbMitglied.Add((int)reader["ID"], reader["Name"].ToString());

                            foreach (KeyValuePair<int, int> tennisspieler in dbTennisspieler)
                            {
                                newMembers.Add(new KeyValuePair<int, Mitglied>(tennisspieler.Key, new Tennisspieler()
                                {
                                    JahreErfahrung = tennisspieler.Value,
                                    Sportart = "Tennis",
                                    AnzahlSpiele = dbSpieler.First(e => e.Key == tennisspieler.Key).Value.Value,
                                    Name = dbMitglied.First(e => e.Key == dbSpieler.First(el => el.Key == tennisspieler.Key).Value.Key).Value,
                                    Id = dbSpieler.First(_ => _.Key == tennisspieler.Key).Value.Key
                                }));
                            }

                            reader.Close();
                            connection.Close();
                            break;
                        }

                    case "Trainer":
                        {
                            //Why this mess?
                            //Because we are not allowed to use ORM from i.e. EF Core, meaning we have to map our data ourselves, by hand with raw queries.
                            List<int> dbTrainer = new List<int>();                                                    //Mitglied_ID
                            Dictionary<int, string> dbMitglied = new Dictionary<int, string>();                                     //ID <Mitglied_ID & AnzahlSpiele>

                            //Query Mitglied
                            MySqlDataReader reader = QueryDB($"Select * from `mitglied`", out MySqlConnection connection);

                            while (reader.Read())
                                dbMitglied.Add((int)reader["ID"], reader["Name"].ToString());

                            connection.Close();

                            //Query 
                            reader = QueryDB($"Select * from `trainer`", out connection);

                            while (reader.Read())
                                dbTrainer.Add((int)reader["Mitglied_ID"]);

                            foreach (int trainer in dbTrainer)
                            {
                                newMembers.Add(new KeyValuePair<int, Mitglied>(trainer, new Trainer()
                                {
                                    Name = dbMitglied.First(_ => _.Key == trainer).Value,
                                    EigeneMannschaft = null,
                                    Id = trainer
                                }));
                            }

                            reader.Close();
                            connection.Close();
                            break;
                        }

                    case "Physiotherapeut":
                        {
                            //Why this mess?
                            //Because we are not allowed to use ORM from i.e. EF Core, meaning we have to map our data ourselves, by hand with raw queries.
                            List<int> dbPhysiotherapeut = new List<int>();                                            //Mitglied_ID
                            Dictionary<int, string> dbMitglied = new Dictionary<int, string>();                                     //ID <Mitglied_ID & AnzahlSpiele>

                            //Query Tennisspieler
                            MySqlDataReader reader = QueryDB($"Select * from `mitglied`", out MySqlConnection connection);

                            while (reader.Read())
                                dbMitglied.Add((int)reader["ID"], reader["Name"].ToString());

                            connection.Close();

                            //Query Physiotherapeut
                            reader = QueryDB($"Select * from `physiotherapeut`", out connection);

                            while (reader.Read())
                                dbPhysiotherapeut.Add((int)reader["Mitglied_ID"]);

                            foreach (int physiotherapeut in dbPhysiotherapeut)
                            {
                                newMembers.Add(new KeyValuePair<int, Mitglied>(physiotherapeut, new Physiotherapeut()
                                {
                                    Name = dbMitglied.First(_ => _.Key == physiotherapeut).Value,
                                    EigeneMannschaft = null,
                                    Id = physiotherapeut
                                }));
                            }

                            reader.Close();
                            connection.Close();
                            break;
                        }
                }

                mitglieder = newMembers.Select((e) => e.Value);

                return mitglieder;
            }
            catch
            {
                return mitglieder;
            }
        }

        // POST: api/Message
        public void Post([FromBody] string jsonStr)
        {
            Mitglied mitglied = JsonConvert.DeserializeObject<Mitglied>(jsonStr,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All }
                );
            string updateString = String.Empty;
            KeyValuePair<int, int> MitgliedIDs = new KeyValuePair<int, int>();

            //Check if Person Name is unique
            MySqlDataReader reader = QueryDB($"select `id` from `mitglied` where `Name`= '{mitglied.Name}';", out MySqlConnection connection);


            if (reader.HasRows) //If not unique, leave.
            {
                reader.Close();
                connection.Close();
                return;
            }

            reader.Close();
            connection.Close();

            string insertString = $"insert into `Mitglied` (`Name`) values ('{mitglied.Name}');";

            if (mitglied is Fussballspieler fussballspieler)
            {
                insertString += $"insert into `spieler`(`AnzahlSpiele`,`Sportart`,`Mitglied_ID`) values ({fussballspieler.AnzahlSpiele},'{fussballspieler.Sportart}',(select `id` from `Mitglied` where `name`= '{fussballspieler.Name}'));";
                insertString += $"insert into `fussballspieler`(`Position`,`Spieler_ID`) values ('{fussballspieler.Position}',(select `id` from `spieler` where `Mitglied_id`= (select `id` from `Mitglied` where `name`= '{fussballspieler.Name}')));";
            }
            else if (mitglied is Handballspieler handballspieler)
            {
                insertString += $"insert into `spieler`(`AnzahlSpiele`,`Sportart`,`Mitglied_ID`) values ({handballspieler.AnzahlSpiele},'{handballspieler.Sportart}',(select `id` from `Mitglied` where `name`= '{handballspieler.Name}'));";
                insertString += $"insert into `handballspieler`(`Position`,`Spieler_ID`) values ('{handballspieler.Position}',(select `id` from `spieler` where `Mitglied_id`= (select `id` from `Mitglied` where `name`= '{handballspieler.Name}')));";
            }
            else if (mitglied is Tennisspieler tennisspieler)
            {
                insertString += $"insert into `spieler`(`AnzahlSpiele`,`Sportart`,`Mitglied_ID`) values ({tennisspieler.AnzahlSpiele},'{tennisspieler.Sportart}',(select `id` from `Mitglied` where `name`= '{tennisspieler.Name}'));";
                insertString += $"insert into `tennisspieler`(`JahreErfahrung`,`Spieler_ID`) values ('{tennisspieler.JahreErfahrung}',(select `id` from `spieler` where `Mitglied_id`= (select `id` from `Mitglied` where `name`= '{tennisspieler.Name}')));";
            }
            else if (mitglied is Trainer trainer)
            {
                insertString += $"insert into `trainer` (`Mitglied_ID`) values((select `id` from `Mitglied` where `name`= '{trainer.Name}'));";
            }
            else if (mitglied is Physiotherapeut physiotherapeut)
            {
                insertString += $"insert into `physiotherapeut` (`Mitglied_ID`) values((select `id` from `Mitglied` where `name`= '{physiotherapeut.Name}'));";
            }
            NonQueryDB(insertString);
        }

        // PUT: api/Message/5
        public void Put(int id, [FromBody] string jsonStr)
        {
            Mitglied mitglied = JsonConvert.DeserializeObject<Mitglied>(jsonStr,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All }
                );
            string updateString = String.Empty;

            MySqlDataReader reader = QueryDB($"select p.id as 'MitgliedID', s.id as 'SpielerID' from Mitglied p join spieler s on s.Mitglied_Id = p.id where p.id = '{id}'", out MySqlConnection connection);
            reader.Read();
            KeyValuePair<int, int> MitgliedIDs = new KeyValuePair<int, int>((int)reader["MitgliedID"], (int)reader["SpielerID"]);
            reader.Close();
            connection.Close();

            if (mitglied is Fussballspieler fussballspieler)
            {
                updateString += $"update `fussballspieler` set `Position`='{fussballspieler.Position}' where `Spieler_ID`={MitgliedIDs.Value};"
                              + $"update `Spieler` set `AnzahlSpiele`='{fussballspieler.AnzahlSpiele}' where `Mitglied_Id`={MitgliedIDs.Key};"
                              + $"update `Mitglied` set `Name`='{fussballspieler.Name}' where `id`={MitgliedIDs.Key};";
            }
            else if (mitglied is Handballspieler handballspieler)
            {
                updateString += $"update `handballspieler` set `Position`='{handballspieler.Position}' where `Spieler_ID`={MitgliedIDs.Value};"
                              + $"update `Spieler` set `AnzahlSpiele`='{handballspieler.AnzahlSpiele}' where `Mitglied_Id`={MitgliedIDs.Key};"
                              + $"update `Mitglied` set `Name`='{handballspieler.Name}' where `id`={MitgliedIDs.Key};";
            }
            else if (mitglied is Tennisspieler tennisspieler)
            {
                updateString += $"update `tennisspieler` set `JahreErfahrung`='{tennisspieler.JahreErfahrung}' where `Spieler_ID`={MitgliedIDs.Value};"
                              + $"update `Spieler` set `AnzahlSpiele`='{tennisspieler.AnzahlSpiele}' where `Mitglied_Id`={MitgliedIDs.Key};"
                              + $"update `Mitglied` set `Name`='{tennisspieler.Name}' where `id`={MitgliedIDs.Key};";
            }
            else if (mitglied is Trainer trainer)
            {
                updateString += $"update `Mitglied` set `Name`='{trainer.Name}' where `id`={MitgliedIDs.Key};";
            }
            else if (mitglied is Physiotherapeut physiotherapeut)
            {
                updateString += $"update `Mitglied` set `Name`='{physiotherapeut.Name}' where `id`={MitgliedIDs.Key};";
            }
            if (updateString != string.Empty)
                NonQueryDB(updateString);
        }

        // DELETE: api/Message/5
        public void Delete(int id)
        {
            NonQueryDB($"delete from `mitglied` where `id`={id}");
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
