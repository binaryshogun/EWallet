using System.Collections.Generic;
using System.Windows.Media;

namespace EWallet.Models
{
    public class BanksData
    {
        #region Enums
        public enum Banks
        {
            Tinkoff,
            Sberbank,
            AlfaBank
        }
        #endregion

        #region Constructors
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
        #endregion

        #region Properties
        public static Dictionary<Banks, Brush> BankBorderColors { get; private set; }
        public static Dictionary<Banks, Brush> BankForegrounds { get; private set; }
        public static Dictionary<Banks, string> BankNames { get; private set; }
        #endregion
    }
}
