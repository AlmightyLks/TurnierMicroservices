//Autor: Lukas Barber
//Datum: 23.02.2020
//Dateiname: Mannschaft.cs
//Beschreibung: Klasse Mannschaft

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SharedTypes.Models
{
    public class Mannschaft
    {
        #region Eigenschaften
        private int _Id;
        private List<Mitglied> _Mitglieder;
        private string _Name;
        private string _SportArt;
        #endregion

        #region Accessoren/Modifier
        public List<Mitglied> Mitglieder { get => _Mitglieder; set => _Mitglieder = value; }
        public string Name { get => _Name; set => _Name = value; }
        public string SportArt { get => _SportArt; set => _SportArt = value; }
        public int Id { get => _Id; set => _Id = value; }
        #endregion

        #region Konstruktoren
        public Mannschaft()
        {
            Mitglieder = new List<Mitglied>();
            Name = "";
            SportArt = "";
            Id = 0;
        }
        public Mannschaft(Mitglied P)
        {
            Mitglieder.Add(P);
            Name = "";
            SportArt = "";
            Id = 0;
        }
        public Mannschaft(Mitglied[] PP)
        {
            foreach (Mitglied P in PP)
            {
                Mitglieder.Add(P);
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

                Mitglied TempPerson;
                Mitglied[] TempPersonenArray = new Mitglied[Mitglieder.Count];

                if (Kriterium == 0) //Name
                {
                    if (Richtung == 1) //Aufwärts
                    {
                        for (int t = 0; t < Mitglieder.Count; t++)
                        {
                            TempPersonenArray[t] = Mitglieder[t];
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

                        Mitglieder.Clear();

                        for (int b = 0; b < TempPersonenArray.Length; b++)
                        {
                            Mitglieder.Add(TempPersonenArray[b]);
                        }
                    }
                    else if (Richtung == 0) //Abwärts
                    {
                        for (int t = 0; t < Mitglieder.Count; t++)
                        {
                            TempPersonenArray[t] = Mitglieder[t];
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

                        Mitglieder.Clear();

                        for (int b = 0; b < TempPersonenArray.Length; b++)
                        {
                            Mitglieder.Add(TempPersonenArray[b]);
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

                Mitglied[] ParaPerson = Mitglieder.ToArray();
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

                Mitglieder = ParaPerson.ToList();

                #endregion
            }
            else
            {
                //Nichts
            }
        }
        private void Sort(ref Mitglied[] Menge, bool aufwaerts)
        {

            if (Menge.Length > 1)
            {
                //1.halbieren
                int mitte = Convert.ToInt32(Menge.Length / 2);

                //2.Linke Seite erstellen
                Mitglied[] Neulinks = new Mitglied[mitte];
                for (int i = 0; i <= Neulinks.Length - 1; i++)
                {
                    Neulinks[i] = Menge[i];
                }

                //3.rechte Seite erstellen
                Mitglied[] Neurechts = new Mitglied[Menge.Length - mitte];
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
        private Mitglied[] merge(ref Mitglied[] MengeLinks, ref Mitglied[] MengeRechts, bool aufwaerts)
        {
            Mitglied[] neueArray = new Mitglied[MengeLinks.Length + MengeRechts.Length];
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

        public bool Post()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    StringContent jsonContent = new StringContent(
                        JsonConvert.SerializeObject(JsonConvert.SerializeObject(this, new JsonSerializerSettings()
                        {
                            TypeNameHandling = TypeNameHandling.All
                        })),
                        Encoding.UTF8,
                        "application/json");

                    HttpResponseMessage response = client.PostAsync($"{Microservices.MannschaftsServiceApi}", jsonContent).GetAwaiter().GetResult();
                    string result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    return JsonConvert.DeserializeObject<bool>(result);
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool Delete()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.DeleteAsync($"{Microservices.MannschaftsServiceApi}/{Id}").GetAwaiter().GetResult();
                    string result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    return JsonConvert.DeserializeObject<bool>(result);
                }
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public bool Put()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    StringContent jsonContent = new StringContent(
                        JsonConvert.SerializeObject(JsonConvert.SerializeObject(this, new JsonSerializerSettings()
                        {
                            TypeNameHandling = TypeNameHandling.All
                        })),
                        Encoding.UTF8,
                        "application/json");

                    HttpResponseMessage response = client.PutAsync($"{Microservices.MannschaftsServiceApi}/{Id}", jsonContent).GetAwaiter().GetResult();
                    string result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    return JsonConvert.DeserializeObject<bool>(result);
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion
    }
}