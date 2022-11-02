using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedClassLibrary;

namespace PlayerSide
{
    public class MauiPlayer : Player, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        protected override void SetPropertyField<T>(string propertyName, ref T field, T newValue)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool Validate(string propertyName, out string EMsg)
        {
            bool isValid;
            EMsg = string.Empty;
            switch (propertyName)
            {
                case nameof(Name):
                    EMsg = string.IsNullOrEmpty(Name) ? $"\"{Name}\" is not a valid name" 
                        : Name.Any(char.IsDigit) ? $"\"{Name}\" can not contain numbers" 
                        : string.Empty;
                    isValid = string.IsNullOrEmpty(EMsg);
                    break;
                default:
                    isValid = true;
                    break;
            }
            return isValid;
        }
    }
}
