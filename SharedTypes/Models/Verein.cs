//Autor: Lukas Barber
//Datum: 23.02.2020
//Dateiname: Verein.cs
//Beschreibung: Klasse Verein

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedTypes.Models
{
    public class Verein
    {
        #region Eigenschaften
        private List<Mannschaft> _Mannschaften;
        private string _Name;
        #endregion

        #region Accessoren/Modifier
        public List<Mannschaft> Mannschaften { get => _Mannschaften; set => _Mannschaften = value; }
        public string Name { get => _Name; set => _Name = value; }
        #endregion

        #region Konstruktoren
        public Verein()
        {
            Mannschaften = new List<Mannschaft>();
            Name = String.Empty;
        }
        public Verein(Mannschaft pMannschaft, string pName = "")
        {
            Mannschaften.Add(pMannschaft);
            Name = pName;
        }
        public Verein(Mannschaft[] MM, string pName = "")
        {
            foreach (Mannschaft M in MM)
            {
                Mannschaften.Add(M);
            }
            Name = pName;
        }
        #endregion

        #region Worker
        public void MannschaftManagen()
        {
            Console.WriteLine("Manage die Mannschaft.");
        }
        #endregion
    }
}