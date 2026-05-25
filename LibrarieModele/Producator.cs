namespace LibrarieModele
{
    public class Producator
    {
        public int IdProducator { get; set; }
        public string Nume { get; set; }
        public string Tara { get; set; }

        public Producator()
        {
            IdProducator = 0;
            Nume = "";
            Tara = "";
        }

        public Producator(int idProducator, string nume, string tara)
        {
            IdProducator = idProducator;
            Nume = nume;
            Tara = tara;
        }

        public Producator(string linieFisier)
        {
            string[] dateFisier = linieFisier.Split(';');

            IdProducator = int.Parse(dateFisier[0]);
            Nume = dateFisier[1];
            Tara = dateFisier[2];
        }

        public string ConversieLaSirPentruFisier()
        {
            return $"{IdProducator};{Nume};{Tara}";
        }

        public string Info()
        {
            return $"Id: {IdProducator}, Nume: {Nume}, Tara: {Tara}";
        }
    }
}