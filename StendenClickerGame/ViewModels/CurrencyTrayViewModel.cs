using StendenClicker.Library.AbstractMonster;
using StendenClicker.Library.AbstractPlatform;
using StendenClicker.Library.AbstractScene;
using StendenClicker.Library.Batches;
using StendenClicker.Library.CurrencyObjects;
using StendenClicker.Library.Factory;
using StendenClicker.Library.Multiplayer;
using StendenClicker.Library.PlayerControls;
using StendenClickerGame.CustomUI;
using StendenClickerGame.Multiplayer;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace StendenClickerGame.ViewModels
{

	public class CurrencyTrayViewModel : ViewModelBase
	{
		//event handlers
		public static event EventHandler CurrencyAdded;
		public static event EventHandler CurrencyRemoved;

		private ReusableCurrencyPool CurrencyPool { get { return ReusableCurrencyPool.GetInstance(); } }

		//bindables
		public CustomCoinList<Currency> CurrencyInView { get; set; }
		public ICommand TappedEvent { get; set; }
		public int? MonsterHealthPercentage { get { return CurrentLevel?.Monster?.GetHealthPercentage(); } }

		//Context variables
		public GamePlatform CurrentLevel { get { return CurrentSession?.CurrentLevel; } }
		public MultiPlayerSession CurrentSession { get { return MultiplayerHubProxy.Instance?.getContext(); } }
		public AbstractMonster CurrentMonster { get { return (AbstractMonster)CurrentLevel.Monster; } }
		public AbstractScene CurrentScene { get { return (AbstractScene)CurrentLevel.Scene; } }
		private Player CurrentPlayer { get { return MultiplayerHubProxy.Instance.CurrentPlayer; } }
		

		private BatchedClick Clicks;

		public CurrencyTrayViewModel()
		{
			TappedEvent = new RelayCommand(MonsterClicked);

			Clicks = new BatchedClick();

			CurrencyInView = new CustomCoinList<Currency>();
			CurrencyInView.OnCoinAdded += CurrencyInView_OnCoinAdded;
			CurrencyInView.OnCoinRemoved += CurrencyInView_OnCoinRemoved;
		}

		private void CurrencyInView_OnCoinRemoved(object sender, EventArgs e)
		{
			CurrencyRemoved?.Invoke(sender, e);
		}

		private void CurrencyInView_OnCoinAdded(object sender, System.EventArgs e)
		{
			CurrencyAdded?.Invoke(sender, e);
		}

		/// <summary>
		/// Most important function that handles all the game clicks
		/// </summary>
		public void MonsterClicked()
		{
			//todo: check if some of this stuff can run async to speed up the game.
			//check if there is a monster to click on:
			if (CurrentMonster != null)
			{
				//batch collect the clicks
				Clicks.addClick();

				//damage monster and set its damage multiplier
				CurrentMonster.DamageFactor = 1;// CurrentPlayer.GetDamageFactor();
				CurrentMonster?.DoDamage(100); //todo: change this back to 10  * (int)Math.Ceiling(((double)CurrentMonster.MonsterLevel / (double)2))
				NotifyPropertyChanged("MonsterHealthPercentage");

				if (CurrentMonster.IsDefeated())
				{
					//get the rewards for this monster and generate a new level
					var rewards = CurrentMonster.GetReward();
					for (ulong i = 0; i < rewards.SparkCoin; i++)
					{
						CreateCoin(typeof(SparkCoin));
					}
					for (ulong i = 0; i < rewards.EuropeanCredit; i++)
					{
						CreateCoin(typeof(EuropeanCredit));
					}

					//todo: update all the user accounts and the current session that a monster has been defeated.
					UpdatePlayerStatsAfterMonsterDefeat(true, false);


					//build a new level from the current player list, in singleplayer mode that list contains 1 player.
					RenderLevel();
				}
			}
		}

		private void UpdatePlayerStatsAfterMonsterDefeat(bool MonsterDefeated, bool SceneDefeated)
        {
			foreach(Player player in CurrentSession.CurrentPlayerList)
            {
				if(MonsterDefeated)
					player.State.MonstersDefeated++;

				if (SceneDefeated)
					player.State.LevelsDefeated++;
            }
        }

		private void RenderLevel()
		{
			CurrentSession.CurrentLevel = MultiplayerHubProxy.Instance.LevelGenerator.BuildLevel(CurrentSession.CurrentPlayerList);
			NotifyPropertyChanged("CurrentMonster");
			NotifyPropertyChanged("CurrencyTray.CurrentMonster");
			NotifyPropertyChanged("CurrencyTray");
			NotifyPropertyChanged("MonsterHealthPercentage");
		}

		/// <summary>
		/// Handles coin creation through the CurrencyPool
		/// </summary>
		/// <param name="type">The type of coin to create</param>
		public void CreateCoin(Type type)
		{
			if (type.BaseType != typeof(Currency))
			{
				throw new Exception("No coin type was passed into the create coin function.");
			}

			Currency coin;
			if (type == typeof(SparkCoin))
			{
				coin = CurrencyPool.AcquireReusable(true);
			}
			else if (type == typeof(EuropeanCredit))
			{
				coin = CurrencyPool.AcquireReusable(false);
			}
			else
			{
				throw new Exception("Unkown coin type was passed into this create coin function.");
			}

			coin.OnCoinHover += (o, e) =>
			{
				//todo: add new value to wallet (possible lagswitch)

				//remove all hover events, this is needed to prevent double event firing after the coin is reused.
				((Currency)o).RemoveHoverEvents();

				//remove the coins from the view and list.
				CurrencyInView.Remove((Currency)o);
				CurrencyPool.ReleaseCurrency((Currency)o);
			};

			CurrencyInView.Add(coin);
		}

		public BatchedClick GetBatchedClick()
		{
			BatchedClick oldClicks = Clicks;
			Clicks = new BatchedClick();
			return oldClicks;
		}
	}

}

