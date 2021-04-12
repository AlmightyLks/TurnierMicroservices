using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharedTypes.Models
{
    public class Spiel : DatabaseObject
    {
        #region Eigenschaften
        private TurnierTeilnehmer _ersterTeilnehmer;
        private TurnierTeilnehmer _zweiterTeilnehmer;
        private int _position;
        #endregion

        #region Accessoren/Modifier
        public TurnierTeilnehmer ErsterTeilnehmer { get => _ersterTeilnehmer; set => _ersterTeilnehmer = value; }
        public TurnierTeilnehmer ZweiterTeilnehmer { get => _zweiterTeilnehmer; set => _zweiterTeilnehmer = value; }
        public int Position { get => _position; set => _position = value; }
        #endregion

        #region Konstruktoren
        public Spiel()
        {
            ErsterTeilnehmer = new TurnierTeilnehmer();
            ZweiterTeilnehmer = new TurnierTeilnehmer();
            Position = 0;
        }
        public Spiel(int teilnehmer1, int teilnehmer2)
        {
            ErsterTeilnehmer = new TurnierTeilnehmer()
            {
                Punkte = 0,
                TeilnehmerId = teilnehmer1
            };
            ZweiterTeilnehmer = new TurnierTeilnehmer()
            {
                Punkte = 0,
                TeilnehmerId = teilnehmer2
            };
            Position = 0;
        }
        public Spiel(Spiel spiel)
        {
            ErsterTeilnehmer = spiel.ErsterTeilnehmer;
            ZweiterTeilnehmer = spiel.ZweiterTeilnehmer;
            Position = spiel.Position;
        }

        #endregion

        #region Worker

        #endregion
    }
}