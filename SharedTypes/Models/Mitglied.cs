//Autor: Lukas Barber
//Datum: 23.02.2020
//Dateiname: Person.cs
//Beschreibung: Klasse Person

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SharedTypes.Models
{
    public abstract class Mitglied
    {
        #region Eigenschaften
        private int _Id;
        private string _Name;
        #endregion

        #region Accessoren/Modifier
        public string Name { get => _Name; set => _Name = value; }
        public int Id { get => _Id; set => _Id = value; }
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
            string type = "";
            if (this is Fussballspieler)
            {
                type = "Fussball";
            }
            else if (this is Handballspieler)
            {
                type = "Handball";
            }
            else if (this is Tennisspieler)
            {
                type = "Tennis";
            }
            else if (this is Trainer)
            {
                type = "Trainer";
            }
            else if (this is Physiotherapeut)
            {
                type = "Physiotherapeut";
            }
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    StringContent strCon = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PutAsync($"{Microservices.MitgliederServiceApi}/{Id}?type={type}", strCon).GetAwaiter().GetResult();
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
