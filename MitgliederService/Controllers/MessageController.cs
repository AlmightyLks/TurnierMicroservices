using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using SharedTypes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MitgliederService.Controllers
{
    public class MessageController : ApiController
    {
        private const string databaseName = "ms_MitgliederService";

        // GET: api/Message
        public IEnumerable<Mitglied> Get()
        {
            var newMembers = new List<KeyValuePair<int, Mitglied>>();                           //Alle Leute

            var dbTennisspieler = new Dictionary<int, int>();                                   //Spieler_ID & JahreErfahrung
            var dbHandballspieler = new Dictionary<int, string>();                              //Spieler_ID & Position
            var dbFussballspieler = new Dictionary<int, string>();                              //Spieler_ID & Position
            var dbTrainer = new List<int>();                                                    //Mitglied_ID
            var dbPhysiotherapeut = new List<int>();                                            //Mitglied_ID
            var dbSpieler = new Dictionary<int, KeyValuePair<int, int>>();                      //ID <Mitglied_ID & AnzahlSpiele>
            var dbMitglied = new Dictionary<int, string>();                                     //ID <Mitglied_ID & AnzahlSpiele>

            //Query Tennisspieler
            (MySqlDataReader DataReader, MySqlConnection Connection) sqlRdr = QueryDB($"Select * from `tennisspieler`");

            while (sqlRdr.DataReader.Read())
                dbTennisspieler.Add((int)sqlRdr.DataReader["Spieler_ID"], (int)sqlRdr.DataReader["JahreErfahrung"]);



            //Query Handballspieler
            sqlRdr = QueryDB($"Select * from `handballspieler`");

            while (sqlRdr.DataReader.Read())
                dbHandballspieler.Add((int)sqlRdr.DataReader["Spieler_ID"], sqlRdr.DataReader["Position"].ToString());



            //Query Fussballspieler
            sqlRdr = QueryDB($"Select * from `fussballspieler`");

            while (sqlRdr.DataReader.Read())
                dbFussballspieler.Add((int)sqlRdr.DataReader["Spieler_ID"], sqlRdr.DataReader["Position"].ToString());


            //Query Spieler
            sqlRdr = QueryDB($"Select * from `spieler`");

            while (sqlRdr.DataReader.Read()) //ID <Mitglied_ID & AnzahlSpiele>
                dbSpieler.Add((int)sqlRdr.DataReader["ID"], new KeyValuePair<int, int>((int)sqlRdr.DataReader["Mitglied_ID"], (int)sqlRdr.DataReader["AnzahlSpiele"]));


            //Query Mitglied
            sqlRdr = QueryDB($"Select * from `mitglied`");

            while (sqlRdr.DataReader.Read())
                dbMitglied.Add((int)sqlRdr.DataReader["ID"], sqlRdr.DataReader["Name"].ToString());


            //Query 
            sqlRdr = QueryDB($"Select * from `trainer`");

            while (sqlRdr.DataReader.Read())
                dbTrainer.Add((int)sqlRdr.DataReader["Mitglied_ID"]);


            //Query Physiotherapeut
            sqlRdr = QueryDB($"Select * from `physiotherapeut`");

            while (sqlRdr.DataReader.Read())
                dbPhysiotherapeut.Add((int)sqlRdr.DataReader["Mitglied_ID"]);

            foreach (var tennisspieler in dbTennisspieler)
            {
                newMembers.Add(new KeyValuePair<int, Mitglied>(tennisspieler.Key, new Tennisspieler()
                {
                    JahreErfahrung = tennisspieler.Value,
                    Sportart = "Tennis",
                    AnzahlSpiele = dbSpieler.First(e => e.Key == tennisspieler.Key).Value.Value,
                    Name = dbMitglied.First(e => e.Key == dbSpieler.First(el => el.Key == tennisspieler.Key).Value.Key).Value
                }));
            }

            foreach (var handballspieler in dbHandballspieler)
            {
                newMembers.Add(new KeyValuePair<int, Mitglied>(handballspieler.Key, new Handballspieler()
                {
                    Position = handballspieler.Value,
                    Sportart = "Handball",
                    AnzahlSpiele = dbSpieler.First(e => e.Key == handballspieler.Key).Value.Value,
                    Name = dbMitglied.First(e => e.Key == dbSpieler.First(el => el.Key == handballspieler.Key).Value.Key).Value
                }));
            }

            foreach (var fussballspieler in dbFussballspieler)
            {
                newMembers.Add(new KeyValuePair<int, Mitglied>(fussballspieler.Key, new Fussballspieler()
                {
                    Position = fussballspieler.Value,
                    Sportart = "Fussball",
                    AnzahlSpiele = dbSpieler.First(e => e.Key == fussballspieler.Key).Value.Value,
                    Name = dbMitglied.First(e => e.Key == dbSpieler.First(el => el.Key == fussballspieler.Key).Value.Key).Value
                }));
            }

            foreach (var trainer in dbTrainer)
            {
                newMembers.Add(new KeyValuePair<int, Mitglied>(trainer, new Trainer()
                {
                    Name = dbMitglied.First(_ => _.Key == trainer).Value,
                    EigeneMannschaft = null
                }));
            }

            sqlRdr.DataReader.Close();
            sqlRdr.Connection.Close();
            IEnumerable<Mitglied> mitglieder = newMembers.Select((e) => e.Value);

            return mitglieder;
        }

        // GET: api/Message?type=fussball
        public IEnumerable<Mitglied> Get(string type)
        {
            var newMembers = new List<KeyValuePair<int, Mitglied>>();                           //Alle Leute

            switch (type)
            {
                case "Fussball":
                    {
                        Dictionary<int, string> dbFussballspieler = new Dictionary<int, string>();                              //Spieler_ID & Position
                        Dictionary<int, KeyValuePair<int, int>> dbSpieler = new Dictionary<int, KeyValuePair<int, int>>();                      //ID <Mitglied_ID & AnzahlSpiele>
                        Dictionary<int, string> dbMitglied = new Dictionary<int, string>();                                     //ID <Mitglied_ID & AnzahlSpiele>

                        //Query Fussballspieler
                        (MySqlDataReader DataReader, MySqlConnection Connection) sqlRdr = QueryDB($"Select * from `fussballspieler`");

                        while (sqlRdr.DataReader.Read())
                            dbFussballspieler.Add((int)sqlRdr.DataReader["Spieler_ID"], sqlRdr.DataReader["Position"].ToString());


                        //Query Spieler
                        sqlRdr = QueryDB($"Select * from `spieler`");

                        while (sqlRdr.DataReader.Read()) //ID <Mitglied_ID & AnzahlSpiele>
                            dbSpieler.Add((int)sqlRdr.DataReader["ID"], new KeyValuePair<int, int>((int)sqlRdr.DataReader["Mitglied_ID"], (int)sqlRdr.DataReader["AnzahlSpiele"]));


                        //Query Mitglied
                        sqlRdr = QueryDB($"Select * from `mitglied`");

                        while (sqlRdr.DataReader.Read())
                            dbMitglied.Add((int)sqlRdr.DataReader["ID"], sqlRdr.DataReader["Name"].ToString());

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

                        sqlRdr.DataReader.Close();
                        sqlRdr.Connection.Close();
                        break;
                    }

                case "Handball":
                    {
                        Dictionary<int, string> dbHandballspieler = new Dictionary<int, string>();                              //Spieler_ID & Position
                        Dictionary<int, KeyValuePair<int, int>> dbSpieler = new Dictionary<int, KeyValuePair<int, int>>();      //ID <Mitglied_ID & AnzahlSpiele>
                        Dictionary<int, string> dbMitglied = new Dictionary<int, string>();                                     //ID <Mitglied_ID & AnzahlSpiele>

                        //Query Handballspieler
                        (MySqlDataReader DataReader, MySqlConnection Connection) sqlRdr = QueryDB($"Select * from `handballspieler`");

                        while (sqlRdr.DataReader.Read())
                            dbHandballspieler.Add((int)sqlRdr.DataReader["Spieler_ID"], sqlRdr.DataReader["Position"].ToString());


                        //Query Spieler
                        sqlRdr = QueryDB($"Select * from `spieler`");

                        while (sqlRdr.DataReader.Read()) //ID <Mitglied_ID & AnzahlSpiele>
                            dbSpieler.Add((int)sqlRdr.DataReader["ID"], new KeyValuePair<int, int>((int)sqlRdr.DataReader["Mitglied_ID"], (int)sqlRdr.DataReader["AnzahlSpiele"]));


                        //Query Mitglied
                        sqlRdr = QueryDB($"Select * from `mitglied`");

                        while (sqlRdr.DataReader.Read())
                            dbMitglied.Add((int)sqlRdr.DataReader["ID"], sqlRdr.DataReader["Name"].ToString());


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

                        sqlRdr.DataReader.Close();
                        sqlRdr.Connection.Close();
                        break;
                    }

                case "Tennis":
                    {
                        Dictionary<int, int> dbTennisspieler = new Dictionary<int, int>();                                   //Spieler_ID & JahreErfahrung
                        Dictionary<int, KeyValuePair<int, int>> dbSpieler = new Dictionary<int, KeyValuePair<int, int>>();   //ID <Mitglied_ID & AnzahlSpiele>
                        Dictionary<int, string> dbMitglied = new Dictionary<int, string>();                                  //ID <Mitglied_ID & AnzahlSpiele>

                        //Query Tennisspieler
                        (MySqlDataReader DataReader, MySqlConnection Connection) sqlRdr = QueryDB($"Select * from `tennisspieler`");

                        while (sqlRdr.DataReader.Read())
                            dbTennisspieler.Add((int)sqlRdr.DataReader["Spieler_ID"], (int)sqlRdr.DataReader["JahreErfahrung"]);


                        //Query Spieler
                        sqlRdr = QueryDB($"Select * from `spieler`");

                        while (sqlRdr.DataReader.Read()) //ID <Mitglied_ID & AnzahlSpiele>
                            dbSpieler.Add((int)sqlRdr.DataReader["ID"], new KeyValuePair<int, int>((int)sqlRdr.DataReader["Mitglied_ID"], (int)sqlRdr.DataReader["AnzahlSpiele"]));


                        //Query Mitglied
                        sqlRdr = QueryDB($"Select * from `mitglied`");

                        while (sqlRdr.DataReader.Read())
                            dbMitglied.Add((int)sqlRdr.DataReader["ID"], sqlRdr.DataReader["Name"].ToString());

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

                        sqlRdr.DataReader.Close();
                        sqlRdr.Connection.Close();
                        break;
                    }

                case "Trainer":
                    {
                        List<int> dbTrainer = new List<int>();                                                    //Mitglied_ID
                        Dictionary<int, string> dbMitglied = new Dictionary<int, string>();                                     //ID <Mitglied_ID & AnzahlSpiele>

                        //Query Mitglied
                        (MySqlDataReader DataReader, MySqlConnection Connection) sqlRdr = QueryDB($"Select * from `mitglied`");

                        while (sqlRdr.DataReader.Read())
                            dbMitglied.Add((int)sqlRdr.DataReader["ID"], sqlRdr.DataReader["Name"].ToString());


                        //Query 
                        sqlRdr = QueryDB($"Select * from `trainer`");

                        while (sqlRdr.DataReader.Read())
                            dbTrainer.Add((int)sqlRdr.DataReader["Mitglied_ID"]);

                        foreach (int trainer in dbTrainer)
                        {
                            newMembers.Add(new KeyValuePair<int, Mitglied>(trainer, new Trainer()
                            {
                                Name = dbMitglied.First(_ => _.Key == trainer).Value,
                                EigeneMannschaft = null,
                                Id = trainer
                            }));
                        }

                        sqlRdr.DataReader.Close();
                        sqlRdr.Connection.Close();
                        break;
                    }

                case "Physiotherapeut":
                    {
                        List<int> dbPhysiotherapeut = new List<int>();                                            //Mitglied_ID
                        Dictionary<int, string> dbMitglied = new Dictionary<int, string>();                                     //ID <Mitglied_ID & AnzahlSpiele>

                        //Query Tennisspieler
                        (MySqlDataReader DataReader, MySqlConnection Connection) sqlRdr = QueryDB($"Select * from `mitglied`");

                        while (sqlRdr.DataReader.Read())
                            dbMitglied.Add((int)sqlRdr.DataReader["ID"], sqlRdr.DataReader["Name"].ToString());

                        //Query Physiotherapeut
                        sqlRdr = QueryDB($"Select * from `physiotherapeut`");

                        while (sqlRdr.DataReader.Read())
                            dbPhysiotherapeut.Add((int)sqlRdr.DataReader["Mitglied_ID"]);


                        foreach (int physiotherapeut in dbPhysiotherapeut)
                        {
                            newMembers.Add(new KeyValuePair<int, Mitglied>(physiotherapeut, new Physiotherapeut()
                            {
                                Name = dbMitglied.First(_ => _.Key == physiotherapeut).Value,
                                EigeneMannschaft = null,
                                Id = physiotherapeut
                            }));
                        }

                        sqlRdr.DataReader.Close();
                        sqlRdr.Connection.Close();
                        break;
                    }
            }

            IEnumerable<Mitglied> mitglieder = newMembers.Select((e) => e.Value);

            return mitglieder;
        }

        // POST: api/Message
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Message/5
        public void Put(int id, string type, [FromBody] Mitglied mitglied)
        {
            //Mitglied mitglied = JsonConvert.DeserializeObject<Mitglied>(jsonStr);
            string updateString = String.Empty;
            KeyValuePair<int, int> personIDs = new KeyValuePair<int, int>();

            (MySqlDataReader DataReader, MySqlConnection Connection) rdrConn = QueryDB($"select p.id as 'PersonID', s.id as 'SpielerID' from person p join spieler s on s.Mitglied_Id = p.id where p.id = (select id from person where name = \"{mitglied.Name}\" limit 1)");
            rdrConn.DataReader.Read();
            personIDs = new KeyValuePair<int, int>((int)rdrConn.DataReader["PersonID"], (int)rdrConn.DataReader["SpielerID"]);
            rdrConn.DataReader.Close();
            rdrConn.Connection.Close();

            switch (type)
            {
                case "Fussball" when mitglied is Fussballspieler fussballspieler:
                    updateString += $"update `fussballspieler` set `Position`='{fussballspieler.Position}' where `Spieler_ID`={personIDs.Value};"
                                  + $"update `Spieler` set `AnzahlSpiele`='{fussballspieler.AnzahlSpiele}' where `Mitglied_Id`={personIDs.Key};"
                                  + $"update `Mitglied` set `Name`='{fussballspieler.Name}' where `id`={personIDs.Key};";
                    break;
                case "Handball" when mitglied is Handballspieler handballspieler:
                    updateString += $"update `handballspieler` set `Position`='{handballspieler.Position}' where `Spieler_ID`={personIDs.Value};"
                                  + $"update `Spieler` set `AnzahlSpiele`='{handballspieler.AnzahlSpiele}' where `Mitglied_Id`={personIDs.Key};"
                                  + $"update `Mitglied` set `Name`='{handballspieler.Name}' where `id`={personIDs.Key};";
                    break;
                case "Tennis" when mitglied is Tennisspieler tennisspieler:
                    updateString += $"update `tennisspieler` set `JahreErfahrung`='{tennisspieler.JahreErfahrung}' where `Spieler_ID`={personIDs.Value};"
                                  + $"update `Spieler` set `AnzahlSpiele`='{tennisspieler.AnzahlSpiele}' where `Mitglied_Id`={personIDs.Key};"
                                  + $"update `Mitglied` set `Name`='{tennisspieler.Name}' where `id`={personIDs.Key};";
                    break;
                case "Trainer" when mitglied is Trainer trainer:
                    updateString += $"update `Mitglied` set `Name`='{trainer.Name}' where `id`={personIDs.Key};";
                    break;
                case "Physiotherapeut" when mitglied is Physiotherapeut physiotherapeut:
                    updateString += $"update `Mitglied` set `Name`='{physiotherapeut.Name}' where `id`={personIDs.Key};";
                    break;
            }
            if (!String.Equals(updateString, String.Empty))
                NonQueryDB(updateString);
        }

        // DELETE: api/Message/5
        public void Delete(int id)
        {
            NonQueryDB($"delete from `mitglied` where `id`={id}");
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
