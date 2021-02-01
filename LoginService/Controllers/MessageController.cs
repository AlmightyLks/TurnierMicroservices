using LoginService.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LoginService.Controllers
{
    public class MessageController : ApiController
    {
        private const string databaseName = "ms_login";

        // GET: api/Message
        public IEnumerable<User> Get()
        {
            List<User> result = new List<User>();
            MySqlDataReader dataReader;
            MySqlConnection connection;

            (dataReader, connection) = QueryDB($"select * from users;");

            if ((dataReader, connection) == (null, null))
                return result;

            while (dataReader.Read())
            {
                User user = new User()
                {
                    Id = (int)dataReader["id"],
                    SessionID = (string)dataReader["SessionID"],
                    Password = (string)dataReader["password"],
                    Username = (string)dataReader["username"]
                };
                result.Add(user);
            }

            dataReader.Close();
            connection.Close();
            return result;
        }

        // GET: api/Message?username=name
        public IEnumerable<User> Get(string username)
        {
            List<User> result = new List<User>();
            MySqlDataReader dataReader;
            MySqlConnection connection;

            (dataReader, connection) = QueryDB($"select * from users where username='{username}';");

            if ((dataReader, connection) == (null, null))
                return result;

            while (dataReader.Read())
            {
                User user = new User()
                {
                    Id = (int)dataReader["id"],
                    SessionID = (string)dataReader["SessionID"],
                    Password = (string)dataReader["password"],
                    Username = (string)dataReader["username"]
                };
                result.Add(user);
            }

            dataReader.Close();
            connection.Close();
            return result;
        }

        // PUT: api/Message/5
        public bool Put(int id, [FromBody] string sessionId)
        {
            int affectedRows = 0;
            try
            {
                affectedRows = NonQueryDB($"update users set `sessionId`='{sessionId}' where id='{id}';");
            }
            catch (Exception e)
            {

            }
            return affectedRows != 0;
        }

        #region Nicht in Benutzung
        // POST: api/Message
        //public void Post([FromBody] string value)
        //{
        //}

        // DELETE: api/Message/5
        //public void Delete(int id)
        //{
        //}
        #endregion

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
