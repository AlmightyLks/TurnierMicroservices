﻿//Autor: Lukas Barber
//Datum: 23.02.2020
//Dateiname: Trainer.cs
//Beschreibung: Klasse Trainer

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedTypes.Models
{
    public class Trainer : Mitglied
    {
        #region Eigenschaften
        private Mannschaft _EigeneMannschaft;
        #endregion

        #region Accessoren/Modifier
        public Mannschaft EigeneMannschaft { get => _EigeneMannschaft; set => _EigeneMannschaft = value; }
        #endregion

        #region Konstruktoren
        public Trainer()
        {
            EigeneMannschaft = new Mannschaft();
        }
        public Trainer(Mannschaft EM, string Name, int id) :base(Name, id)
        {
            EigeneMannschaft = EM;
        }
        public Trainer(Trainer T)
        {
            EigeneMannschaft = T.EigeneMannschaft;
        }
        #endregion

        #region Worker
        public void Coachen()
        {
            Console.WriteLine("Trainer ist am Coachen.");
        }

        public override int CompareByName(Mitglied P)
        {
            int ergebnis = 0;

            int Lng = Math.Min(Name.Length, P.Name.Length);

            char[] ThisCharArray = Name.ToCharArray();
            char[] SPCharArray = P.Name.ToCharArray();

            for (int i = 0; i < Lng; i++)
            {
                if (char.Parse(Convert.ToString(ThisCharArray[i])) != char.Parse(Convert.ToString(SPCharArray[i])))
                {
                    if (char.Parse(Convert.ToString(ThisCharArray[i])) > char.Parse(Convert.ToString(SPCharArray[i])))
                    {
                        ergebnis = 1; //"This"'s Char Wert ist höher - gehört unter "SP"
                    }
                    else
                    {
                        ergebnis = -1; //"PS"'s Char Wert ist höher - gehört unter "This"
                    }
                }
                else
                {
                    ergebnis = 0; //"PS"'s Char Wert ist gleich mit dem von "This".
                }
            }
            return ergebnis;
        }
        #endregion
    }
}