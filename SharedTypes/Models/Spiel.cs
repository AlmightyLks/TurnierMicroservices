using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharedTypes.Models
{
    public class Spiel
    {
        #region Eigenschaften
        private Punktestand _Punktestand;
        #endregion

        #region Accessoren/Modifier
        public Punktestand Punktestand { get => _Punktestand; set => _Punktestand = value; }
        #endregion

        #region Konstruktoren
        public Spiel()
        {
            Punktestand = new Punktestand()
            {
                Mannschaft = new Mannschaft[2],
                Punkte = new int[2]
            };
        }
        public Spiel(Punktestand myStand)
        {
            Punktestand = myStand;
        }
        public Spiel(Spiel mySpiel)
        {
            Punktestand = mySpiel.Punktestand;
        }
        #endregion

        #region Worker

        #endregion
    }
}