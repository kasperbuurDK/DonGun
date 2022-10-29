using SharedClassLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlayerSide
{
    public class CharacterViewModel : INotifyPropertyChanged
    {
        private MauiPlayer _character;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        public MauiPlayer Character
        {
            get => _character;
            set 
            {
                _character = value;
                OnPropertyChanged(new PropertyChangedEventArgs(string.Empty));
            } 
        }
    }
}

