//Autor: Lukas Barber
//Datum: 23.02.2020
//Dateiname: Mannschaft.cs
//Beschreibung: Klasse Mannschaft

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedTypes.Models
{
    public class Mannschaft
    {
        #region Eigenschaften
        private List<Person> _Personen;
        private string _Name;
        private string _SportArt;
        #endregion

        #region Accessoren/Modifier
        public List<Person> Personen { get => _Personen; set => _Personen = value; }
        public string Name { get => _Name; set => _Name = value; }
        public string SportArt { get => _SportArt; set => _SportArt = value; }
        #endregion

        #region Konstruktoren
        public Mannschaft()
        {
            Personen = new List<Person>();
            Name = "";
            SportArt = "";
        }
        public Mannschaft(Person P)
        {
            Personen.Add(P);
            Name = "";
            SportArt = "";
        }
        public Mannschaft(Person[] PP)
        {
            foreach (Person P in PP)
            {
                Personen.Add(P);
            }
            Name = "";
            SportArt = "";
        }
        #endregion

        #region Worker
        public void Sortieren(int Sorting, int Richtung, int Kriterium)
        {
            if (Sorting == 0) //Bubble Sort
            {
                #region Bubble Sort

                Person TempPerson;
                Person[] TempPersonenArray = new Person[Personen.Count];

                if (Kriterium == 0) //Name
                {
                    if (Richtung == 1) //Aufwärts
                    {
                        for (int t = 0; t < Personen.Count; t++)
                        {
                            TempPersonenArray[t] = Personen[t];
                        }

                        for (int i = 0; i < TempPersonenArray.Length; i++)
                        {
                            for (int j = i; j < TempPersonenArray.Length; j++)
                            {
                                if (string.Compare(TempPersonenArray[i].Name, TempPersonenArray[j].Name) > 0)
                                {
                                    TempPerson = TempPersonenArray[j];
                                    TempPersonenArray[j] = TempPersonenArray[i];
                                    TempPersonenArray[i] = TempPerson;
                                }
                                else
                                {
                                    //weiter
                                }
                            }
                        }

                        Personen.Clear();

                        for (int b = 0; b < TempPersonenArray.Length; b++)
                        {
                            Personen.Add(TempPersonenArray[b]);
                        }
                    }
                    else if (Richtung == 0) //Abwärts
                    {
                        for (int t = 0; t < Personen.Count; t++)
                        {
                            TempPersonenArray[t] = Personen[t];
                        }

                        for (int i = 0; i < TempPersonenArray.Length; i++)
                        {
                            for (int j = i; j < TempPersonenArray.Length; j++)
                            {
                                if (string.Compare(TempPersonenArray[i].Name, TempPersonenArray[j].Name) < 0)
                                {
                                    TempPerson = TempPersonenArray[j];
                                    TempPersonenArray[j] = TempPersonenArray[i];
                                    TempPersonenArray[i] = TempPerson;
                                }
                                else
                                {
                                    //weiter
                                }
                            }
                        }

                        Personen.Clear();

                        for (int b = 0; b < TempPersonenArray.Length; b++)
                        {
                            Personen.Add(TempPersonenArray[b]);
                        }
                    }
                    else
                    {
                        //Nichts
                    }
                }
                else
                {
                    //Nichts
                }
                #endregion
            }
            else if (Sorting == 1)
            {
                #region Merge Sort 

                Person[] ParaPerson = Personen.ToArray();
                bool aufwaerts = false;
                if (Richtung == 1)
                {
                    aufwaerts = true;
                }
                else
                {
                    aufwaerts = false;
                }
                Sort(ref ParaPerson, aufwaerts);

                Personen = ParaPerson.ToList();

                #endregion
            }
            else
            {
                //Nichts
            }
        }
        private void Sort(ref Person[] Menge, bool aufwaerts)
        {

            if (Menge.Length > 1)
            {
                //1.halbieren
                int mitte = Convert.ToInt32(Menge.Length / 2);

                //2.Linke Seite erstellen
                Person[] Neulinks = new Person[mitte];
                for (int i = 0; i <= Neulinks.Length - 1; i++)
                {
                    Neulinks[i] = Menge[i];
                }

                //3.rechte Seite erstellen
                Person[] Neurechts = new Person[Menge.Length - mitte];
                for (int i = mitte; i <= Menge.Length - 1; i++)
                {
                    Neurechts[i - mitte] = Menge[i];
                }

                //4.jeweils sortieren
                Sort(ref Neulinks, aufwaerts);
                Sort(ref Neurechts, aufwaerts);

                //5.Arrays wieder zusammenführen
                Menge = merge(ref Neulinks, ref Neurechts, aufwaerts);
            }
            else
            {
                //Nichts
            }
        }
        private Person[] merge(ref Person[] MengeLinks, ref Person[] MengeRechts, bool aufwaerts)
        {
            Person[] neueArray = new Person[MengeLinks.Length + MengeRechts.Length];
            int indexLinks = 0;
            int indexRechts = 0;
            int indexErgebnis = 0;

            while (indexLinks < MengeLinks.Length && indexRechts < MengeRechts.Length)
            {
                if (aufwaerts)//aufwärts sortieren
                {
                    if (MengeLinks[indexLinks].CompareByName(MengeRechts[indexRechts]) > 0)
                    {
                        neueArray[indexErgebnis] = MengeLinks[indexLinks];
                        indexLinks += 1;
                    }
                    else
                    {
                        neueArray[indexErgebnis] = MengeRechts[indexRechts];
                        indexRechts += 1;
                    }
                    indexErgebnis += 1;
                }
                else
                {
                    //abwärts sortieren
                    if (MengeLinks[indexLinks].CompareByName(MengeRechts[indexRechts]) < 0)
                    {
                        neueArray[indexErgebnis] = MengeLinks[indexLinks];
                        indexLinks += 1;
                    }
                    else
                    {
                        neueArray[indexErgebnis] = MengeRechts[indexRechts];
                        indexRechts += 1;
                    }
                    indexErgebnis += 1;
                }
            }

            while (indexLinks < MengeLinks.Length)
            {
                neueArray[indexErgebnis] = MengeLinks[indexLinks];
                indexLinks += 1;
                indexErgebnis += 1;
            }

            while (indexRechts < MengeRechts.Length)
            {
                neueArray[indexErgebnis] = MengeRechts[indexRechts];
                indexRechts += 1;
                indexErgebnis += 1;
            }

            return neueArray;
        }
        #endregion
    }
}