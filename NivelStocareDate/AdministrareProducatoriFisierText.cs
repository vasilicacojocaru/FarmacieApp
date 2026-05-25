using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrareProducatoriFisierText
    {
        private string numeFisier;

        public AdministrareProducatoriFisierText(string numeFisier)
        {
            this.numeFisier = numeFisier;

            if (!File.Exists(numeFisier))
            {
                File.Create(numeFisier).Close();
            }
        }

        public void AdaugaProducator(Producator producator)
        {
            using (StreamWriter sw = new StreamWriter(numeFisier, true))
            {
                sw.WriteLine(producator.ConversieLaSirPentruFisier());
            }
        }

        public List<Producator> GetProducatori()
        {
            List<Producator> producatori = new List<Producator>();

            using (StreamReader sr = new StreamReader(numeFisier))
            {
                string linie;
                while ((linie = sr.ReadLine()) != null)
                {
                    producatori.Add(new Producator(linie));
                }
            }

            return producatori;
        }

        public List<Producator> CautaDupaNume(string text)
        {
            List<Producator> toate = GetProducatori();

            return toate
                .Where(p => p.Nume.ToLower().Contains(text.ToLower()))
                .ToList();
        }

        public bool ModificaProducator(string numeCautat, Producator producatorNou)
        {
            List<Producator> toate = GetProducatori();
            bool modificat = false;

            for (int i = 0; i < toate.Count; i++)
            {
                if (toate[i].Nume.ToLower() == numeCautat.ToLower())
                {
                    toate[i] = producatorNou;
                    modificat = true;
                    break;
                }
            }

            if (modificat)
            {
                using (StreamWriter sw = new StreamWriter(numeFisier, false))
                {
                    foreach (Producator producator in toate)
                    {
                        sw.WriteLine(producator.ConversieLaSirPentruFisier());
                    }
                }
            }

            return modificat;
        }
    }
}
