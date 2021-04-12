using MySql.Data.MySqlClient;
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

            MySqlDataReader reader = QueryDB("", out MySqlConnection connection);

            return result;
        }

        // GET: api/Message/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Message
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Message/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Message/5
        public void Delete(int id)
        {
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
