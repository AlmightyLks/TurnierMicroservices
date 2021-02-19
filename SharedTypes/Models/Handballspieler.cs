//Autor: Lukas Barber
//Datum: 23.02.2020
//Dateiname: HandballSpieler.cs
//Beschreibung: Klasse HandballSpieler

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedTypes.Models
{
    public class Handballspieler : Spieler
    {
        #region Eigenschaften
        private string _Position;
        #endregion

        #region Accessoren/Modifier
        public string Position { get => _Position; set => _Position = value; }
        #endregion

        #region Konstruktoren
        public Handballspieler()
        {
            Position = "Kreisläufer";
        }
        public Handballspieler(string P, string N) : base("HandballSpieler", N)
        {
            Position = P;
        }
        public Handballspieler(Fussballspieler FS)
        {
            Position = FS.Position;
        }
        #endregion

        #region Worker
        public void Werfen()
        {
            Console.WriteLine("Der Fussball Spieler wirft einen Ball.");
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