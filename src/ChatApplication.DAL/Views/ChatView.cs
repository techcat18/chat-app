using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApplication.DAL.Views
{
    public class ChatView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ChatTypeId { get; set; }
        public int MembersCount { get; set; }
    }
}
