using System.Collections.Generic;
using System.Windows.Media;

namespace EWallet.Models
{
    /// <summary>
    /// Статический класс, предоставляющий 
    /// доступ к данным о банках.
    /// </summary>
    public static class BanksData
    {
        #region Enums
        /// <summary>
        /// Список доступных банков.
        /// </summary>
        public enum Banks
        {
            Tinkoff,
            Sberbank,
            AlfaBank,
            Default
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Инициализирует статические поля класса <see cref="BanksData"/>.
        /// </summary>
        static BanksData()
        {
            BankColors = new Dictionary<Banks, Brush>()
            {
                { Banks.Tinkoff, new SolidColorBrush(Color.FromRgb(255, 222, 1)) },
                { Banks.Sberbank, new SolidColorBrush(Color.FromRgb(71, 178, 84)) },
                { Banks.AlfaBank, new SolidColorBrush(Color.FromRgb(239, 49, 36)) },
                { Banks.Default, new SolidColorBrush(Color.FromRgb(255, 255, 255)) }
            };
            BankForegrounds = new Dictionary<Banks, Brush>()
            {
                { Banks.Tinkoff, new SolidColorBrush(Colors.Black) },
                { Banks.Sberbank, new SolidColorBrush(Colors.Black) },
                { Banks.AlfaBank, new SolidColorBrush(Colors.Black) },
                { Banks.Default, new SolidColorBrush(Colors.Black) }
            };
            BankLogos = new Dictionary<Banks, string>()
            {
                { Banks.Tinkoff, "../../Resources/icons/logo_tinkoff.png" },
                { Banks.Sberbank, "../../Resources/icons/logo_sberbank.png" },
                { Banks.AlfaBank, "../../Resources/icons/logo_alfabank.png" },
                { Banks.Default, "../../Resources/icons/logo_defaultbank.png" }
            };
        }
        #endregion

        #region Properties
        /// <summary>
        /// Основные цвета банков.
        /// </summary>
        public static Dictionary<Banks, Brush> BankColors { get; private set; }
        /// <summary>
        /// Цвета шрифтов банков.
        /// </summary>
        public static Dictionary<Banks, Brush> BankForegrounds { get; private set; }
        /// <summary>
        /// Относительные пути к логотипам банков.
        /// </summary>
        public static Dictionary<Banks, string> BankLogos { get; private set; }
        #endregion
    }
}
