using StendenClicker.Library;
using StendenClicker.Library.Models;
using StendenClicker.Library.Models.DatabaseModels;
using StendenClickerGame.Data;
using StendenClickerGame.Multiplayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace StendenClickerGame.ViewModels
{
	public class FriendshipPanelViewmodel : ViewModelBase
	{
		public ObservableCollection<FriendshipListObject> ObservableFriendship { get; }
		public ObservableCollection<SearchPlayerObject> ObservableSearchPlayerList { get; }
		public ObservableCollection<InviteModel> ObservablePendingInvites { get; }


		public ICommand SearchFriendsCommand { get; set; }
		public string FriendSearchbar { get; set; }

		public FriendshipPanelViewmodel()
		{
			ObservableFriendship = new ObservableCollection<FriendshipListObject>();
			ObservableSearchPlayerList = new ObservableCollection<SearchPlayerObject>();
			ObservablePendingInvites = new ObservableCollection<InviteModel>();

			SearchFriendsCommand = new RelayCommand(SearchFriends);
		}

		public async void AddPendingInvite(InviteModel invite)
		{
			var dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
			await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {

				//check if the user already has an invite for this session
				if(ObservablePendingInvites.FirstOrDefault(n => n.UserGuid == invite.UserGuid) == null)
				{
					invite.OnAccept = new RelayCommand(async () => 
					{
						//do the signalR join session command.
						await MultiplayerHubProxy.Instance.JoinFriend(invite.UserGuid);
					});
					invite.OnDecline = new RelayCommand(() => 
					{
						//remove this object from the Observable list
						ObservablePendingInvites.Remove(invite);
					});
					ObservablePendingInvites.Add(invite);
				}
				else
				{
					//show a notification ??
				}				
			});
		}

		private async void SearchFriends()
		{
			if (!string.IsNullOrEmpty(FriendSearchbar))
			{
				ObservableSearchPlayerList.Clear();

				//do web request and get the possible friends back. -> api/player/GetAccountsByNameSearch
				Dictionary<string, string> parameters = new Dictionary<string, string>
				{
					{ "name", FriendSearchbar.Trim() },
					{ "user", MultiplayerHubProxy.Instance.CurrentPlayer.UserId.ToString() }
				};

				var response = await RestHelper.GetRequestAsync("api/player/GetAccountsByNameSearch", parameters);
				if (response.StatusCode == System.Net.HttpStatusCode.OK)
				{
					List<Player> foundPlayers = RestHelper.ConvertJsonToObject<List<Player>>(response.Content);
					if (foundPlayers != null && foundPlayers?.Count != 0)
					{
						foreach (Player p in foundPlayers)
						{
							SearchPlayerObject SearchPlayer = new SearchPlayerObject()
							{
								PlayerName = p.PlayerName,
								PlayerGuid = p.PlayerGuid,
								OnAddFriend = new RelayCommand(() => CreateFriendShip(p))
							};
							ObservableSearchPlayerList.Add(SearchPlayer);
						}
					}
				}

			}
		}

		private async void CreateFriendShip(Player player)
		{
			List<Player> Friends = new List<Player> { MultiplayerHubProxy.Instance.CurrentPlayer, player };

			var response = await RestHelper.PostRequestAsync("api/player/CreateFriendship", Friends);
			if (response.StatusCode == System.Net.HttpStatusCode.OK)
			{
				//friend has been added, update friendlist at the top:
				await UpdateFriendships(MultiplayerHubProxy.Instance.CurrentPlayer.UserId.ToString());
				ObservableSearchPlayerList.Clear();
				FriendSearchbar = "";
				NotifyPropertyChanged("FriendSearchbar");
			}
			else
			{
				//show prompt to user that adding friend failed.
			}
		}

		protected async Task UpdateFriendships(string userguid)
		{
			//empty the list.
			ObservableFriendship.Clear();

			Dictionary<string, string> parameters = new Dictionary<string, string>
				{
					{ "PlayerId", userguid }
				};


			var response = await RestHelper.GetRequestAsync("api/player/Friendships", parameters);
			if (response.StatusCode == System.Net.HttpStatusCode.OK)
			{
				List<Friendship> friend = RestHelper.ConvertJsonToObject<List<Friendship>>(response.Content);

				if (friend == null) return;

				List<Player> pList = new List<Player>();
				pList.AddRange(friend.Where(n => n.Player1.PlayerGuid != userguid).Select(n => n.Player1));
				pList.AddRange(friend.Where(n => n.Player2.PlayerGuid != userguid).Select(n => n.Player2));

				foreach (Player f in pList)
				{
					FriendshipListObject friendUI = new FriendshipListObject
					{
						Name = f.PlayerName,
						Guid = f.PlayerGuid,
						InviteCommand = new RelayCommand(async () =>
						{
							await MultiplayerHubProxy.Instance.SendInvite(f.PlayerGuid);
						})
					};

					ObservableFriendship.Add(friendUI);
				}


				NotifyPropertyChanged("ObservableFriendship");
			}
		}

		public async void InitializeFriendship(string userguid) => await UpdateFriendships(userguid);
	}
}
