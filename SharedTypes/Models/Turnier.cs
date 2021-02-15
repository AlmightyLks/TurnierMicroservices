using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharedTypes.Models
{
    public class Turnier
    {
        #region Eigenschaften
        private string _TurnierTitel;
        private List<Spiel> _Spiele;
        #endregion

        #region Accessoren/Modifier
        public List<Spiel> Spiele { get => _Spiele; set => _Spiele = value; }
        public string TurnierTitel { get => _TurnierTitel; set => _TurnierTitel = value; }
        #endregion

        #region Konstruktoren
        public Turnier()
        {
            TurnierTitel = "";
            Spiele = new List<Spiel>();
        }
        public Turnier(List<Spiel> mySpiele, string myTitel)
        {
            TurnierTitel = myTitel;
            Spiele = mySpiele;
        }
        public Turnier(Turnier Turn)
        {
            TurnierTitel = Turn.TurnierTitel;
            Spiele = Turn.Spiele;
        }
        #endregion

        #region Worker
        //idk
        #endregion
    }
}