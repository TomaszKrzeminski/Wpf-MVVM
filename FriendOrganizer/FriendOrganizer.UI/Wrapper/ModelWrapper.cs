using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Wrapper
{
    public class ModelWrapper<T>:NotifyDataErrorInfoBase
    {
        public ModelWrapper(T model)
        {
            Model = model;
        }

        public T Model { get;  set; }
        protected virtual TValue GetValue<TValue>([CallerMemberName] string propertyName = null)
        {
            return (TValue)typeof(T).GetProperty(propertyName).GetValue(Model);
        }

        protected virtual void SetValue<TValue>(TValue value,[CallerMemberName] string propertyName = null)
        {
           typeof(T).GetProperty(propertyName).SetValue(Model,value);
            OnPropertyChanged(propertyName);
            ValidatePropertyInternal(propertyName,value);
        }

        private void ValidatePropertyInternal(string propertyName,object currentValue)
        {
            ValidateDataAnnotations(propertyName, currentValue);

            ValidateCustomErrors(propertyName);

        }

        private void ValidateDataAnnotations(string propertyName, object currentValue)
        {
            ClearErrors(propertyName);
            var context = new ValidationContext(Model) { MemberName = propertyName };
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateProperty(currentValue, context, validationResults);

            foreach (var v in validationResults)
            {
                AddError(propertyName, v.ErrorMessage);
            }
        }

        private void ValidateCustomErrors(string propertyName)
        {
            var errors = ValidatePropertyName(propertyName);
            if (errors != null)
            {
                foreach (var error in errors)
                {
                    AddError(propertyName, error);
                }
            }
        }

        protected virtual IEnumerable<string> ValidatePropertyName(string propertyName)
        {
            return null;
        }
    }
}
