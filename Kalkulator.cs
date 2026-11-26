using System;

namespace Kalkulator_wynagrodzen_desktop
{
    public enum TypUmowy
    {
        UmowaOPrace,
        UmowaZlecenie
    }

    public class KalkulatorWynagrodzenia
    {
        private decimal _kwotaBrutto;

        public decimal KwotaBrutto
        {
            get => _kwotaBrutto;
            set => _kwotaBrutto = value;
        }

        public TypUmowy TypUmowy { get; set; }

        public bool UlgaDlaMlodych { get; set; }

        public decimal SkladkiSpoleczne => Math.Round(KwotaBrutto * 0.1371m, 2);

        public decimal PodstawaZdrowotnego => KwotaBrutto - SkladkiSpoleczne;

        public decimal SkladkaZdrowotna => Math.Round(PodstawaZdrowotnego * 0.09m, 2);

        public decimal ZaliczkaPit => UlgaDlaMlodych
            ? 0.00m
            : Math.Round((KwotaBrutto - SkladkiSpoleczne) * 0.12m, 2);

        public decimal WynagrodzenieNetto => KwotaBrutto - SkladkiSpoleczne - SkladkaZdrowotna - ZaliczkaPit;

        public KalkulatorWynagrodzenia(decimal kwotaBrutto, TypUmowy typUmowy, bool ulgaDlaMlodych)
        {
            KwotaBrutto = kwotaBrutto;
            TypUmowy = typUmowy;
            UlgaDlaMlodych = ulgaDlaMlodych;
        }

        public bool CzyKwotaBruttoPoprawna()
        {
            return KwotaBrutto > 0m;
        }
    }
}