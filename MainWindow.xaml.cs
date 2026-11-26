using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;

namespace Kalkulator_wynagrodzen_desktop
{
    public class WpisRejestru
    {
        public string Data { get; set; } = string.Empty;
        public decimal KwotaBrutto { get; set; }
        public string Umowa { get; set; } = string.Empty;
        public decimal Netto { get; set; }
        public string Ulga { get; set; } = string.Empty;
    }

    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<WpisRejestru> _rejestr = new ObservableCollection<WpisRejestru>();

        public MainWindow()
        {
            InitializeComponent();
            RejestrDataGrid.ItemsSource = _rejestr;
        }

        private void ObliczIZapisz_Click(object sender, RoutedEventArgs e)
        {
            string tekstKwoty = KwotaBruttoTextBox.Text.Trim().Replace(',', '.');

            if (!decimal.TryParse(tekstKwoty, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal kwotaBrutto))
            {
                MessageBox.Show("Wpisz poprawna kwote brutto.", "Blad danych", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            TypUmowy typUmowy = UmowaZlecenieRadio.IsChecked == true
                ? TypUmowy.UmowaZlecenie
                : TypUmowy.UmowaOPrace;

            bool ulgaMlodych = UlgaMlodychCheckBox.IsChecked == true;

            KalkulatorWynagrodzenia kalkulator = new KalkulatorWynagrodzenia(kwotaBrutto, typUmowy, ulgaMlodych);

            if (!kalkulator.CzyKwotaBruttoPoprawna())
            {
                MessageBox.Show("Kwota brutto musi byc wieksza od zera.", "Blad walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            decimal skladkiZus = Math.Round(kalkulator.SkladkiSpoleczne, 2);
            decimal skladkaZdrowotna = Math.Round(kalkulator.SkladkaZdrowotna, 2);
            decimal pit = Math.Round(kalkulator.ZaliczkaPit, 2);
            decimal netto = Math.Round(kalkulator.WynagrodzenieNetto, 2);

            WynikNettoTextBlock.Text = $"Wynagrodzenie netto: {netto:0.00} PLN";
            SkladkiZusTextBlock.Text = $"Skladki ZUS (spoleczne): {skladkiZus:0.00} PLN";
            SkladkaZdrowotnaTextBlock.Text = $"Skladka zdrowotna: {skladkaZdrowotna:0.00} PLN";
            ZaliczkaPitTextBlock.Text = $"Zaliczka na podatek PIT: {pit:0.00} PLN";

            _rejestr.Add(new WpisRejestru
            {
                Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                KwotaBrutto = Math.Round(kwotaBrutto, 2),
                Umowa = typUmowy == TypUmowy.UmowaOPrace ? "Umowa o prace" : "Umowa zlecenie",
                Netto = netto,
                Ulga = ulgaMlodych ? "Tak" : "Nie"
            });
        }
    }
}