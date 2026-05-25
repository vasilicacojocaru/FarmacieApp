using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrareMedicamenteFisierText
    {
        private string numeFisier;

        public AdministrareMedicamenteFisierText(string numeFisier)
        {
            this.numeFisier = numeFisier;

            if (!File.Exists(numeFisier))
            {
                File.Create(numeFisier).Close();
            }
        }

        public void AdaugaMedicament(Medicament medicament)
        {
            using (StreamWriter sw = new StreamWriter(numeFisier, true))
            {
                sw.WriteLine(medicament.ConversieLaSirPentruFisier());
            }
        }

        public List<Medicament> GetMedicamente()
        {
            List<Medicament> medicamente = new List<Medicament>();

            using (StreamReader sr = new StreamReader(numeFisier))
            {
                string linie;

                while ((linie = sr.ReadLine()) != null)
                {
                    if (linie.Trim() != "")
                    {
                        medicamente.Add(new Medicament(linie));
                    }
                }
            }

            return medicamente;
        }

        public List<Medicament> CautaDupaDenumire(string text)
        {
            List<Medicament> toate = GetMedicamente();

            return toate
                .Where(m => m.Denumire.ToLower().Contains(text.ToLower()))
                .ToList();
        }

        public bool ModificaMedicament(string denumireCautata, Medicament medicamentNou)
        {
            List<Medicament> toate = GetMedicamente();
            bool modificat = false;

            for (int i = 0; i < toate.Count; i++)
            {
                if (toate[i].Denumire.ToLower() == denumireCautata.ToLower())
                {
                    toate[i] = medicamentNou;
                    modificat = true;
                    break;
                }
            }

            if (modificat)
            {
                SalveazaToateMedicamentele(toate);
            }

            return modificat;
        }

        public bool StergeMedicament(string denumireCautata)
        {
            List<Medicament> toate = GetMedicamente();

            Medicament medicamentDeSters = toate
                .FirstOrDefault(m => m.Denumire.ToLower() == denumireCautata.ToLower());

            if (medicamentDeSters == null)
            {
                return false;
            }

            toate.Remove(medicamentDeSters);

            SalveazaToateMedicamentele(toate);

            return true;
        }

        private void SalveazaToateMedicamentele(List<Medicament> medicamente)
        {
            using (StreamWriter sw = new StreamWriter(numeFisier, false))
            {
                foreach (Medicament medicament in medicamente)
                {
                    sw.WriteLine(medicament.ConversieLaSirPentruFisier());
                }
            }
        }
    }
}