//Autor: Lukas Barber
//Datum: 23.02.2020
//Dateiname: Person.cs
//Beschreibung: Klasse Person

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedTypes.Models
{
    public abstract class Mitglied
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
        }
        public Mitglied(string N)
        {
            Name = N;
        }
        public Mitglied(Mitglied P)
        {
            Name = P.Name;
        }
        #endregion

        #region Worker
        public void Reden(string T)
        {
            Console.WriteLine(T);
        }
        public abstract int CompareByName(Mitglied SP);
        #endregion
    }
}
