using StendenClicker.Library;
using StendenClicker.Library.Models.DatabaseModels;
using StendenClickerGame.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StendenClickerGame.ViewModels
{
    public class FriendshipPanelViewmodel : ViewModelBase
    {
        public ObservableCollection<FriendshipListObject> ObservableFriendship { get; }
        public FriendshipPanelViewmodel()
        {
            ObservableFriendship = new ObservableCollection<FriendshipListObject>();           
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
                    InviteCommand = new RelayCommand(() => 
                    {
                        //todo: perform invite.
                    })
                };

                ObservableFriendship.Add(friendUI);
            }
               

            NotifyPropertyChanged("ObservableFriendship");
        }

        public async void InitializeFriendship(string userguid) => await UpdateFriendships(userguid);
    }
}
