namespace FriendOrganizer.UI.Events
{
    public class AfterSaveDetailEventArgs
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }
        public string ViewModelName { get; set; }
    }
}