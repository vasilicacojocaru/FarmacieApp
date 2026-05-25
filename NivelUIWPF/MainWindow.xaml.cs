using LibrarieModele;
using NivelStocareDate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NivelUIWPF
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const int LUNGIME_MAXIMA_TEXT = 15;

        private ObservableCollection<Medicament> medicamente =
            new ObservableCollection<Medicament>();
        private AdministrareMedicamenteFisierText adminMedicamente;

        public ObservableCollection<Producator> Producatori { get; set; } =
            new ObservableCollection<Producator>();

        private string numeProducatorBinding = "";

        public string NumeProducatorBinding
        {
            get
            {
                return numeProducatorBinding;
            }
            set
            {
                numeProducatorBinding = value;
                OnPropertyChanged();
            }
        }

        private List<string> caracteristiciDisponibile = new List<string>
        {
            "Necesita reteta",
            "Compensat",
            "Disponibil in stoc"
        };

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            adminMedicamente =
                new AdministrareMedicamenteFisierText("medicamente.txt");

            List<Medicament> listaDinFisier =
                adminMedicamente.GetMedicamente();

            foreach (Medicament medicament in listaDinFisier)
            {
                medicamente.Add(medicament);
            }

            dtpDataExpirare.SelectedDate = DateTime.Today;
            dtpDataExpirareModificare.SelectedDate = DateTime.Today;

            lstCaracteristici.ItemsSource = caracteristiciDisponibile;

            dgMedicamente.ItemsSource = medicamente;
            cmbMedicamente.ItemsSource = medicamente;
            cmbProducatori.ItemsSource = Producatori;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AscundePanouri()
        {
            panelAdministrare.Visibility = Visibility.Collapsed;
            panelCautare.Visibility = Visibility.Collapsed;
            panelModificare.Visibility = Visibility.Collapsed;
            panelProducatori.Visibility = Visibility.Collapsed;
        }

        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            AscundePanouri();
            panelAdministrare.Visibility = Visibility.Visible;

            lblMesaj.Content = "";
            lblMesajModificare.Content = "";
            lblMesajProducator.Content = "";

            dgMedicamente.Visibility = Visibility.Visible;
        }

        private void btnCautaMeniu_Click(object sender, RoutedEventArgs e)
        {
            AscundePanouri();
            panelCautare.Visibility = Visibility.Visible;

            lblMesaj.Content = "";
            lblMesajModificare.Content = "";
            lblMesajProducator.Content = "";

            dgMedicamente.Visibility = Visibility.Visible;
        }

        private void btnModificaMeniu_Click(object sender, RoutedEventArgs e)
        {
            AscundePanouri();
            panelModificare.Visibility = Visibility.Visible;

            lblMesaj.Content = "";
            lblMesajModificare.Content = "";
            lblMesajProducator.Content = "";

            dgMedicamente.Visibility = Visibility.Visible;
        }

        private void btnProducatoriMeniu_Click(object sender, RoutedEventArgs e)
        {
            AscundePanouri();
            panelProducatori.Visibility = Visibility.Visible;

            lblMesaj.Content = "";
            lblMesajModificare.Content = "";
            lblMesajProducator.Content = "";

            dgMedicamente.Visibility = Visibility.Collapsed;
        }

        private void btnAdauga_Click(object sender, RoutedEventArgs e)
        {
            if (!ValideazaDateMedicament())
            {
                return;
            }

            double pret = double.Parse(txtPret.Text);
            int cantitate = int.Parse(txtCantitate.Text);

            TipMedicament tip = GetTipMedicament();
            FormaAdministrare forma = GetFormaAdministrare();

            DateTime dataExpirare = dtpDataExpirare.SelectedDate ?? DateTime.Today;

            Medicament medicament = new Medicament(
                txtDenumire.Text,
                pret,
                cantitate,
                txtProducator.Text,
                tip,
                forma,
                dataExpirare
            );

            medicament.Optiuni = GetOptiuniCheckBox();

            medicamente.Add(medicament);
            adminMedicamente.AdaugaMedicament(medicament);

            MessageBox.Show("Medicament adaugat cu succes!");

            CurataCampuri();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            CurataCampuri();
            ResetValidare();
        }

        private void btnAfiseaza_Click(object sender, RoutedEventArgs e)
        {
            dgMedicamente.ItemsSource = medicamente;
        }

        private void btnCauta_Click(object sender, RoutedEventArgs e)
        {
            string cautare = txtCautare.Text.ToLower();

            var rezultate = medicamente
                .Where(m => m.Denumire.ToLower().Contains(cautare))
                .ToList();

            dgMedicamente.ItemsSource = rezultate;

            if (rezultate.Count == 0)
                lblMesaj.Content = "Nu a fost gasit niciun medicament.";
            else
                lblMesaj.Content = "Medicament gasit.";
        }

        private void cmbMedicamente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Medicament medicament = cmbMedicamente.SelectedItem as Medicament;

            if (medicament == null)
                return;

            txtDenumire.Text = medicament.Denumire;
            txtPret.Text = medicament.Pret.ToString();
            txtCantitate.Text = medicament.Cantitate.ToString();
            txtProducator.Text = medicament.Producator;

            SeteazaTipMedicament(medicament.Tip);
            SeteazaFormaAdministrare(medicament.Forma);

            dtpDataExpirareModificare.SelectedDate = medicament.DataExpirare;

            SelecteazaCaracteristiciInListBox(medicament.Optiuni);
        }

        private void btnActualizeaza_Click(object sender, RoutedEventArgs e)
        {
            Medicament medicament = cmbMedicamente.SelectedItem as Medicament;

            if (medicament == null)
            {
                lblMesajModificare.Content = "Selecteaza un medicament pentru modificare.";
                return;
            }

            if (!ValideazaDateMedicament())
            {
                return;
            }

            medicament.Denumire = txtDenumire.Text;
            medicament.Pret = double.Parse(txtPret.Text);
            medicament.Cantitate = int.Parse(txtCantitate.Text);
            medicament.Producator = txtProducator.Text;
            medicament.Tip = GetTipMedicament();
            medicament.Forma = GetFormaAdministrare();
            medicament.DataExpirare = dtpDataExpirareModificare.SelectedDate ?? DateTime.Today;
            medicament.DataActualizare = DateTime.Today;
            medicament.Optiuni = GetOptiuniListBox();
            adminMedicamente.ModificaMedicament(
                cmbMedicamente.Text,
                medicament
                );

            dgMedicamente.Items.Refresh();
            cmbMedicamente.Items.Refresh();

            lblMesajModificare.Content = "Medicament actualizat cu succes.";
        }

        private bool ValideazaDateMedicament()
        {
            ResetValidare();

            bool valid = true;

            if (txtDenumire.Text.Trim() == "")
            {
                lblDenumire.Foreground = Brushes.Red;
                txtMesajEroare.Text = "Denumirea este obligatorie.";
                valid = false;
            }
            else if (txtDenumire.Text.Length > LUNGIME_MAXIMA_TEXT)
            {
                lblDenumire.Foreground = Brushes.Red;
                txtMesajEroare.Text = "Denumirea nu poate avea mai mult de 15 caractere.";
                valid = false;
            }

            if (txtPret.Text.Trim() == "")
            {
                lblPret.Foreground = Brushes.Red;
                txtMesajEroare.Text = "Pretul este obligatoriu.";
                valid = false;
            }
            else if (!double.TryParse(txtPret.Text, out double pret) || pret <= 0)
            {
                lblPret.Foreground = Brushes.Red;
                txtMesajEroare.Text = "Pretul trebuie sa fie un numar pozitiv.";
                valid = false;
            }

            if (txtCantitate.Text.Trim() == "")
            {
                lblCantitate.Foreground = Brushes.Red;
                txtMesajEroare.Text = "Cantitatea este obligatorie.";
                valid = false;
            }
            else if (!int.TryParse(txtCantitate.Text, out int cantitate) || cantitate < 0)
            {
                lblCantitate.Foreground = Brushes.Red;
                txtMesajEroare.Text = "Cantitatea trebuie sa fie numar intreg pozitiv.";
                valid = false;
            }

            if (txtProducator.Text.Trim() == "")
            {
                lblProducator.Foreground = Brushes.Red;
                txtMesajEroare.Text = "Producatorul este obligatoriu.";
                valid = false;
            }
            else if (txtProducator.Text.Length > LUNGIME_MAXIMA_TEXT)
            {
                lblProducator.Foreground = Brushes.Red;
                txtMesajEroare.Text = "Producatorul nu poate avea mai mult de 15 caractere.";
                valid = false;
            }

            return valid;
        }

        private void ResetValidare()
        {
            lblDenumire.Foreground = Brushes.Black;
            lblPret.Foreground = Brushes.Black;
            lblCantitate.Foreground = Brushes.Black;
            lblProducator.Foreground = Brushes.Black;

            txtMesajEroare.Text = "";
        }

        private TipMedicament GetTipMedicament()
        {
            if (rbAnalgezic.IsChecked == true)
                return TipMedicament.Analgezic;

            if (rbVitamine.IsChecked == true)
                return TipMedicament.Vitamine;

            return TipMedicament.Antibiotic;
        }

        private FormaAdministrare GetFormaAdministrare()
        {
            if (rbSirop.IsChecked == true)
                return FormaAdministrare.Sirop;

            if (rbInjectabil.IsChecked == true)
                return FormaAdministrare.Injectabil;

            return FormaAdministrare.Comprimate;
        }

        private string GetOptiuniCheckBox()
        {
            List<string> optiuni = new List<string>();

            if (ckbReteta.IsChecked == true)
                optiuni.Add("Necesita reteta");

            if (ckbCompensat.IsChecked == true)
                optiuni.Add("Compensat");

            if (ckbStocDisponibil.IsChecked == true)
                optiuni.Add("Disponibil in stoc");

            return string.Join(", ", optiuni);
        }

        private string GetOptiuniListBox()
        {
            List<string> optiuni = new List<string>();

            foreach (string optiune in lstCaracteristici.SelectedItems)
            {
                optiuni.Add(optiune);
            }

            return string.Join(", ", optiuni);
        }

        private void SelecteazaCaracteristiciInListBox(string optiuni)
        {
            lstCaracteristici.SelectedItems.Clear();

            if (string.IsNullOrEmpty(optiuni))
                return;

            string[] valori = optiuni.Split(',');

            foreach (string valoare in valori)
            {
                string optiuneCurata = valoare.Trim();

                if (caracteristiciDisponibile.Contains(optiuneCurata))
                {
                    lstCaracteristici.SelectedItems.Add(optiuneCurata);
                }
            }
        }

        private void SeteazaTipMedicament(TipMedicament tip)
        {
            rbAntibiotic.IsChecked = tip == TipMedicament.Antibiotic;
            rbAnalgezic.IsChecked = tip == TipMedicament.Analgezic;
            rbVitamine.IsChecked = tip == TipMedicament.Vitamine;
        }

        private void SeteazaFormaAdministrare(FormaAdministrare forma)
        {
            rbComprimate.IsChecked = forma == FormaAdministrare.Comprimate;
            rbSirop.IsChecked = forma == FormaAdministrare.Sirop;
            rbInjectabil.IsChecked = forma == FormaAdministrare.Injectabil;
        }

        private void CurataCampuri()
        {
            txtDenumire.Clear();
            txtPret.Clear();
            txtCantitate.Clear();
            txtProducator.Clear();

            rbAntibiotic.IsChecked = true;
            rbComprimate.IsChecked = true;

            ckbReteta.IsChecked = false;
            ckbCompensat.IsChecked = false;
            ckbStocDisponibil.IsChecked = false;

            dtpDataExpirare.SelectedDate = DateTime.Today;
        }

        private void btnAdaugaProducator_Click(object sender, RoutedEventArgs e)
        {
            if (!ValideazaProducator())
                return;

            int id = int.Parse(txtIdProducator.Text);

            Producator producator = new Producator(
                id,
                txtNumeProducator.Text,
                txtTaraProducator.Text
            );

            Producatori.Add(producator);

            CurataCampuriProducator();

            lblMesajProducator.Content = "Producator adaugat cu succes.";
        }

        private void btnAfiseazaProducatori_Click(object sender, RoutedEventArgs e)
        {
            dgProducatori.ItemsSource = Producatori;
        }

        private void btnModificaProducator_Click(object sender, RoutedEventArgs e)
        {
            Producator producator = cmbProducatori.SelectedItem as Producator;

            if (producator == null)
            {
                lblMesajProducator.Content = "Selecteaza un producator.";
                return;
            }

            if (!ValideazaProducator())
                return;

            producator.IdProducator = int.Parse(txtIdProducator.Text);
            producator.Nume = txtNumeProducator.Text;
            producator.Tara = txtTaraProducator.Text;

            dgProducatori.Items.Refresh();
            cmbProducatori.Items.Refresh();

            lblMesajProducator.Content = "Producator modificat cu succes.";
        }

        private void btnStergeProducator_Click(object sender, RoutedEventArgs e)
        {
            Producator producator = cmbProducatori.SelectedItem as Producator;

            if (producator == null)
            {
                lblMesajProducator.Content = "Selecteaza un producator.";
                return;
            }

            Producatori.Remove(producator);

            CurataCampuriProducator();

            lblMesajProducator.Content = "Producator sters cu succes.";
        }

        private void cmbProducatori_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Producator producator = cmbProducatori.SelectedItem as Producator;

            if (producator == null)
                return;

            txtIdProducator.Text = producator.IdProducator.ToString();
            NumeProducatorBinding = producator.Nume;
            txtTaraProducator.Text = producator.Tara;
        }

        private bool ValideazaProducator()
        {
            if (txtIdProducator.Text.Trim() == "" ||
                txtNumeProducator.Text.Trim() == "" ||
                txtTaraProducator.Text.Trim() == "")
            {
                lblMesajProducator.Content = "Completeaza toate campurile.";
                return false;
            }

            if (!int.TryParse(txtIdProducator.Text, out int id) || id <= 0)
            {
                lblMesajProducator.Content = "Id-ul trebuie sa fie numar pozitiv.";
                return false;
            }

            if (txtNumeProducator.Text.Length > LUNGIME_MAXIMA_TEXT)
            {
                lblMesajProducator.Content = "Numele producatorului este prea lung.";
                return false;
            }

            if (txtTaraProducator.Text.Length > LUNGIME_MAXIMA_TEXT)
            {
                lblMesajProducator.Content = "Tara este prea lunga.";
                return false;
            }

            return true;
        }

        private void CurataCampuriProducator()
        {
            txtIdProducator.Clear();
            NumeProducatorBinding = "";
            txtTaraProducator.Clear();
            cmbProducatori.SelectedItem = null;
        }
    }
}