using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Wrapper
{
    public class FriendWrapper:ModelWrapper<Friend>
    {
        
        public int Id { get=>Model.Id;  }

        private string firstName;
        public int? FavoriteLanguageId
        {
            get { return GetValue<int?>(); }
            set { SetValue(value); }
        }
        public string FirstName
        {
            get { return GetValue<string>(); }
            set { SetValue(value);}              
               
            
        }
               

        protected override IEnumerable<string> ValidatePropertyName(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(FirstName):
                    if (string.Equals(FirstName, "Robot", StringComparison.OrdinalIgnoreCase))
                    {
                       yield return "Robots are not valid friends";
                    }
                    break;

                default:
                    break;
            }
        }

        private string lastName;

        public FriendWrapper(Friend model) : base(model)
        {
        }

        public string LastName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        private string email;

        public string Email
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }


    }

}
