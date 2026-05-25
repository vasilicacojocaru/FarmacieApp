using System.Collections.Generic;
using System.Linq;
using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrareMedicamenteMemorie
    {
        private List<Medicament> medicamente;

        public AdministrareMedicamenteMemorie()
        {
            medicamente = new List<Medicament>();
        }

        public void AdaugaMedicament(Medicament medicament)
        {
            medicamente.Add(medicament);
        }

        public List<Medicament> GetMedicamente()
        {
            return medicamente;
        }

        public List<Medicament> CautaDupaDenumire(string text)
        {
            return medicamente
                .Where(m => m.Denumire.ToLower().Contains(text.ToLower()))
                .ToList();
        }
    }
}