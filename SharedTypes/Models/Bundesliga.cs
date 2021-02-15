//Autor: Lukas Barber
//Datum: 23.02.2020
//Dateiname: Bundesliga.cs
//Beschreibung: Klasse Bundesliga

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedTypes.Models
{
    public class Bundesliga
    {
        #region Eigenschaften
        private List<Verein> _Vereine;
        #endregion

        #region Accessoren/Modifier
        public List<Verein> Vereine { get => _Vereine; set => _Vereine = value; }
        #endregion

        #region Konstruktoren
        public Bundesliga()
        {

        }
        public Bundesliga(Verein V)
        {
            Vereine.Add(V);
        }
        public Bundesliga(Verein[] VV)
        {
            foreach (Verein V in VV)
            {
                Vereine.Add(V);
            }
        }
        #endregion

        #region Worker
        public void VereinManagen()
        {
            Console.WriteLine("Manage einen Verein.");
        }
        #endregion
    }
}
