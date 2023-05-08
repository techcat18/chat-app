namespace ChatApplication.DAL.Entities.Views
{
    public class ChatView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ChatTypeId { get; set; }
        public int MembersCount { get; set; }
    }
}
