//Autor: Lukas Barber
//Datum: 23.02.2020
//Dateiname: TennisSpieler.cs
//Beschreibung: Klasse Tennisspieler

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedTypes.Models
{
    public class Tennisspieler : Spieler
    {
        #region Eigenschaften
        private int _JahreErfahrung;
        #endregion

        #region Accessoren/Modifier
        public int JahreErfahrung { get => _JahreErfahrung; set => _JahreErfahrung = value; }
        #endregion

        #region Konstruktoren
        public Tennisspieler()
        {
            JahreErfahrung = 0;
        }
        public Tennisspieler(string N, int JE, int id) : base("TennisSpieler", N, id)
        {
            JahreErfahrung = JE;
        }
        public Tennisspieler(Tennisspieler TS)
        {
            JahreErfahrung = TS.JahreErfahrung;
        }
        #endregion

        #region Worker
        public void Schlagen()
        {
            Console.WriteLine("Der Tennis Spieler schlägt einen Ball.");
        }
        public override int CompareByName(Mitglied P)
        {
            int ergebnis = Name.CompareTo(P.Name);
            return ergebnis;
        }
        public override int CompareBySpiele(Spieler SP)
        {
            int ergebnis = 0;

            if (AnzahlSpiele > SP.AnzahlSpiele)
            {
                ergebnis = 1;
            }
            else if (AnzahlSpiele == SP.AnzahlSpiele)
            {
                ergebnis = 0;
            }
            else
            {
                ergebnis = -1;
            }

            return ergebnis;
        }
        #endregion
    }
}