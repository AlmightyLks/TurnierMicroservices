using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace SharedTypes.Models
{
    public class Turnier
    {
        #region Eigenschaften
        private int _id;
        private string _titel;
        private string _sportart;
        private List<Spiel> _spiele;
        #endregion
        
        #region Accessoren/Modifier
        public List<Spiel> Spiele { get => _spiele; set => _spiele = value; }
        public string Titel { get => _titel; set => _titel = value; }
        public int Id { get => _id; set => _id = value; }
        public string Sportart { get => _sportart; set => _sportart = value; }
        #endregion

        #region Konstruktoren
        public Turnier()
        {
            Id = 0;
            Titel = "";
            Sportart = "";
            Spiele = new List<Spiel>();
        }
        public Turnier(int id, List<Spiel> mySpiele, string myTitel, string sportart)
        {
            Id = id;
            Titel = myTitel;
            Sportart = sportart;
            Spiele = mySpiele;
        }
        public Turnier(Turnier turn)
        {
            Id = turn.Id;
            Titel = turn.Titel;
            Sportart = turn.Sportart;
            Spiele = turn.Spiele;
        }
        #endregion

        #region Worker
        public void Post()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        string jsonStr = JsonConvert.SerializeObject(JsonConvert.SerializeObject(
                                this,
                                Formatting.None,
                                settings: new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All }
                                ));
                        
                        StringContent strCon = new StringContent(
                            jsonStr,
                            Encoding.UTF8,
                            "application/json"
                            );

                        HttpResponseMessage response = client.PostAsync($"{Microservices.MitgliederServiceApi}", strCon).GetAwaiter().GetResult();
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        public void Put()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        string jsonStr = JsonConvert.SerializeObject(JsonConvert.SerializeObject(
                                this,
                                Formatting.None,
                                settings: new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All }
                                ));

                        StringContent strCon = new StringContent(
                            jsonStr,
                            Encoding.UTF8,
                            "application/json");

                        HttpResponseMessage response = client.PutAsync($"{Microservices.MitgliederServiceApi}/{Id}", strCon).GetAwaiter().GetResult();
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        public void Delete()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.DeleteAsync($"{Microservices.MitgliederServiceApi}/{Id}").GetAwaiter().GetResult();
                    string jsonStr = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }
            }
            catch (Exception e)
            {

            }
        }
        #endregion
    }
}