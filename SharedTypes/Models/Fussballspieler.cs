//Autor: Lukas Barber
//Datum: 23.02.2020
//Dateiname: FussballSpieler.cs
//Beschreibung: Klasse FussballSpieler

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedTypes.Models
{
    public class Fussballspieler : Spieler
    {
        #region Eigenschaften
        private string _Position;
        #endregion

        #region Accessoren/Modifier
        public string Position { get => _Position; set => _Position = value; }
        #endregion

        #region Konstruktoren
        public Fussballspieler()
        {
            Position = "Mittelfeld";
        }
        public Fussballspieler(string P, string N):base("FussballSpieler", N)
        {
            Position = P;
        }
        public Fussballspieler(Fussballspieler FS)
        {
            Position = FS.Position;
        }
        #endregion

        #region Worker
        public void Schiessen()
        {
            Console.WriteLine("Der Fussball Spieler schiesst einen Ball.");
        }
        public override int CompareByName(Person P)
        {
            int ergebnis = Name.CompareTo(P.Name);
            return ergebnis;
        }
        public override int CompareBySpiele(Spieler SP)
        {
            int ergebnis = 0;

            if(AnzahlSpiele > SP.AnzahlSpiele)
            {
                ergebnis = 1;
            }
            else if(AnzahlSpiele == SP.AnzahlSpiele)
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
