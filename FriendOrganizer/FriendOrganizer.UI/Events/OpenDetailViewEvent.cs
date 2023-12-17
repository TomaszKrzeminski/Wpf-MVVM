using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Events
{
    public class OpenDetailViewEvent:PubSubEvent<OpenDetailViewEventArgs>
    {
    }

    public class OpenDetailViewEventArgs
    {
        public int Id { get; set;}
        public string ViewModelName { get; set; }
    }
}
