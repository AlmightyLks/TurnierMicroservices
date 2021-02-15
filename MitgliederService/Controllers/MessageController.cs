using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace MitgliederService.Controllers
{
    public class MessageController : ApiController
    {
        private const string databaseName = "ms_MitgliederService";

        // GET: api/Message
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
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
