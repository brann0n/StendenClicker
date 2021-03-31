using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StendenClickerGame.Data
{
    public class FriendshipListObject
    {
        public string Name { get; set; }
        public string Guid { get; set; }
        public ICommand InviteCommand { get; set; }
    }
}
