using MySql.Data.MySqlClient;
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
            var sqlRdr = QueryDB($"Select * from `tennisspieler`");

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
                newMembers.Add(new Trainer()
                {
                    Name = dbTrainer.First(e => e.Key == dbSpieler.First(el => el.Key == trainer.Key).Value.Key).Value
                    EigeneMannschaft
                    Position = fussballspieler.Value,
                    Sportart = "Fussball",
                    AnzahlSpiele = dbSpieler.First(e => e.Key == fussballspieler.Key).Value.Value,
                    
                }));
            }

            sqlRdr.DataReader.Close();
            sqlRdr.Connection.Close();
            IEnumerable<Mitglied> mitglieder = newMembers.Select((e) => e.Value);

            return mitglieder;
        }

        // GET: api/Message/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Message
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Message/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Message/5
        public void Delete(int id)
        {
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
