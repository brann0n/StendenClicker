using System.Windows.Input;

namespace StendenClickerGame.Data
{
	public class SearchPlayerObject
	{
		public string PlayerName { get; set; }
		public string PlayerGuid { get; set; }
		public ICommand OnAddFriend { get; set; }
	}
}
