//Autor: Lukas Barber
//Datum: 23.02.2020
//Dateiname: Person.cs
//Beschreibung: Klasse Person

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SharedTypes.Models
{
    public abstract class Mitglied : DatabaseObject
    {
        #region Eigenschaften
        private string _Name;
        #endregion

        #region Accessoren/Modifier
        public string Name { get => _Name; set => _Name = value; }
        #endregion

        #region Konstruktoren
        public Mitglied()
        {
            Name = "";
            Id = 0;
        }
        public Mitglied(string N, int id)
        {
            Name = N;
            Id = id;
        }
        public Mitglied(Mitglied M)
        {
            Name = M.Name;
            Id = M.Id;
        }
        #endregion

        #region Worker
        public void Reden(string T)
        {
            Console.WriteLine(T);
        }
        public abstract int CompareByName(Mitglied SP);

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
        #endregion
    }
}
