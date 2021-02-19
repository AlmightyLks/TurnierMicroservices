//Autor: Lukas Barber
//Datum: 23.02.2020
//Dateiname: Spieler.cs
//Beschreibung: Klasse Spieler

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedTypes.Models
{
    public abstract class Spieler : Mitglied
    {
        #region Eigenschaften
        private string _Sportart;
        private int _AnzahlSpiele;
        #endregion

        #region Accessoren/Modifier
        public string Sportart { get => _Sportart; set => _Sportart = value; }
        public int AnzahlSpiele { get => _AnzahlSpiele; set => _AnzahlSpiele = value; }
        #endregion

        #region Konstruktoren
        public Spieler() : base()
        {
            AnzahlSpiele = 0;
            Sportart = "";
        }
        public Spieler(string SA) : base()
        {
            AnzahlSpiele = 0;
            Sportart = SA;
        }
        public Spieler(string SA, string name) : base(name)
        {
            AnzahlSpiele = 0;
            Sportart = SA;
        }
        public Spieler(string SA, int AS, string Name) : base(Name)
        {
            AnzahlSpiele = AS;
            Sportart = SA;
        }
        public Spieler(Spieler SP) : base()
        {
            Sportart = SP.Sportart;
        }
        #endregion

        #region Worker
        public void Spielen()
        {
            Console.WriteLine("Der Spieler spielt.");
        }
        public abstract override int CompareByName(Mitglied P);
        public abstract int CompareBySpiele(Spieler S);
        #endregion
    }
}
