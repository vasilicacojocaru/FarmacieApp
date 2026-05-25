using System;

namespace LibrarieModele
{
    public class Medicament
    {
        public string Denumire { get; set; }
        public double Pret { get; set; }
        public int Cantitate { get; set; }
        public string Producator { get; set; }
        public string Optiuni { get; set; }
        public TipMedicament Tip { get; set; }
        public FormaAdministrare Forma { get; set; }

        public DateTime DataExpirare { get; set; }
        public DateTime DataActualizare { get; set; }

        public Medicament()
        {
            Denumire = "";
            Pret = 0;
            Cantitate = 0;
            Producator = "";
            Optiuni = "";
            Tip = TipMedicament.Analgezic;
            Forma = FormaAdministrare.Comprimate;
            DataExpirare = DateTime.Today;
            DataActualizare = DateTime.Today;
        }

        public Medicament(string denumire, double pret, int cantitate, string producator,
            TipMedicament tip, FormaAdministrare forma, DateTime dataExpirare)
        {
            Denumire = denumire;
            Pret = pret;
            Cantitate = cantitate;
            Producator = producator;
            Optiuni = "";
            Tip = tip;
            Forma = forma;
            DataExpirare = dataExpirare;
            DataActualizare = DateTime.Today;
        }

        public Medicament(string linieFisier)
        {
            string[] dateFisier = linieFisier.Split(';');

            Denumire = dateFisier[0];
            Pret = double.Parse(dateFisier[1]);
            Cantitate = int.Parse(dateFisier[2]);
            Producator = dateFisier[3];
            Optiuni = "";
            Tip = (TipMedicament)int.Parse(dateFisier[4]);
            Forma = (FormaAdministrare)int.Parse(dateFisier[5]);

            if (dateFisier.Length > 6)
                DataExpirare = DateTime.Parse(dateFisier[6]);
            else
                DataExpirare = DateTime.Today;

            if (dateFisier.Length > 7)
                DataActualizare = DateTime.Parse(dateFisier[7]);
            else
                DataActualizare = DateTime.Today;
        }

        public string Info()
        {
            return $"Denumire: {Denumire}, Pret: {Pret} lei, Cantitate: {Cantitate}, Producator: {Producator}, Tip: {Tip}, Forma: {Forma}, Data expirare: {DataExpirare.ToShortDateString()}, Data actualizare: {DataActualizare.ToShortDateString()}, Optiuni: {Optiuni}";
        }

        public string ConversieLaSirPentruFisier()
        {
            return $"{Denumire};{Pret};{Cantitate};{Producator};{(int)Tip};{(int)Forma};{DataExpirare};{DataActualizare}";
        }
    }
}