using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace _119_Karpovich.ViewModels
{
    /// <summary>
    /// Базовый класс для ViewModel.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <inheritdoc cref="PropertyChangedEventHandler"/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc cref="PropertyChanged"/>
        /// <param name="property">
        /// Имя свойства.
        /// </param>
        public void OnPropertyChanged([CallerMemberName] string property = null) 
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }
}
