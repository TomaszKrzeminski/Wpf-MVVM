using FriendOrganizer.UI.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FriendOrganizer.UI.Wrapper
{
    public class NotifyDataErrorInfoBase:ViewModelBase,INotifyDataErrorInfo
    {
        private Dictionary<string, List<string>> ErrorsByPropertyName = new Dictionary<string, List<string>>();

        public bool HasErrors => ErrorsByPropertyName.Any();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return ErrorsByPropertyName.ContainsKey(propertyName) ? ErrorsByPropertyName[propertyName] : null;
            
        }

        protected virtual void OnErrorsChange(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            base.OnPropertyChanged(nameof(HasErrors));
        }

        protected void AddError(string propertyName, string Error)
        {
            if (!ErrorsByPropertyName.ContainsKey(propertyName))
            {
                ErrorsByPropertyName[propertyName] = new List<string>();
            }

            if (!ErrorsByPropertyName[propertyName].Contains(Error))
            {
                ErrorsByPropertyName[propertyName].Add(Error);
                OnErrorsChange(propertyName);
            }


        }

        protected void ClearErrors(string propertyName)
        {
            if (ErrorsByPropertyName.ContainsKey(propertyName))
            {
                ErrorsByPropertyName.Remove(propertyName);
                OnErrorsChange(propertyName);
            }
        }



    }

}
