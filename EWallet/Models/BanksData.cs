using System.Collections.Generic;
using System.Windows.Media;

namespace EWallet.Models
{
    public class BanksData
    {
        public enum Banks
        {
            Tinkoff,
            Sberbank,
            AlfaBank
        }
        public static readonly Dictionary<Banks, Brush> BankBorderColors;
        public static readonly Dictionary<Banks, Brush> BankForegrounds;
        public static readonly Dictionary<Banks, string> BankNames;

        static BanksData()
        {
            BankBorderColors = new Dictionary<Banks, Brush>()
            {
                { Banks.Tinkoff, new SolidColorBrush(Color.FromRgb(255, 222, 1)) },
                { Banks.Sberbank, new SolidColorBrush(Color.FromRgb(0, 125, 64)) },
                { Banks.AlfaBank, new SolidColorBrush(Color.FromArgb(230, 255, 40, 11)) }
            };
            BankForegrounds = new Dictionary<Banks, Brush>()
            {
                { Banks.Tinkoff, new SolidColorBrush(Colors.Black) },
                { Banks.Sberbank, new SolidColorBrush(Colors.White) },
                { Banks.AlfaBank, new SolidColorBrush(Colors.White) }
            };
            BankNames = new Dictionary<Banks, string>()
            {
                { Banks.Tinkoff, "ТИНЬКОФФ БАНК" },
                { Banks.Sberbank, "СБЕРБАНК" },
                { Banks.AlfaBank, "АЛЬФА-БАНК" }
            };
        }
    }
}
