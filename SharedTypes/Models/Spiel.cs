using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharedTypes.Models
{
    public class Spiel
    {
        #region Eigenschaften
        private TurnierTeilnehmer _ersterTeilnehmer;
        private TurnierTeilnehmer _zweiterTeilnehmer;
        #endregion

        #region Accessoren/Modifier
        public TurnierTeilnehmer ErsterTeilnehmer { get => _ersterTeilnehmer; set => _ersterTeilnehmer = value; }
        public TurnierTeilnehmer ZweiterTeilnehmer { get => _zweiterTeilnehmer; set => _zweiterTeilnehmer = value; }
        #endregion

        #region Konstruktoren
        public Spiel()
        {
            ErsterTeilnehmer = new TurnierTeilnehmer();
            ZweiterTeilnehmer = new TurnierTeilnehmer();
        }
        public Spiel(object teilnehmer1, object teilnehmer2)
        {
            ErsterTeilnehmer = new TurnierTeilnehmer()
            {
                Punkte = 0,
                Teilnehmer = teilnehmer1
            };
            ZweiterTeilnehmer = new TurnierTeilnehmer()
            {
                Punkte = 0,
                Teilnehmer = teilnehmer2
            };
        }
        public Spiel(Spiel spiel)
        {
            ErsterTeilnehmer = spiel.ErsterTeilnehmer;
            ZweiterTeilnehmer = spiel.ZweiterTeilnehmer;
        }

        #endregion

        #region Worker

        #endregion
    }
}