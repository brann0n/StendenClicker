using System.Windows.Input;

namespace StendenClickerGame.Data
{
	public class FriendshipListObject
	{
		public string Name { get; set; }
		public string Guid { get; set; }
		public ICommand InviteCommand { get; set; }

		public ICommand DeleteFriend { get; set; }
	}
}
