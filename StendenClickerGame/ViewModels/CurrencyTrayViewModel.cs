using StendenClicker.Library.AbstractMonster;
using StendenClicker.Library.AbstractScene;
using StendenClicker.Library.Batches;
using StendenClicker.Library.CurrencyObjects;
using StendenClicker.Library.Factory;
using StendenClicker.Library.Multiplayer;
using StendenClicker.Library.PlayerControls;
using StendenClickerGame.CustomUI;
using StendenClickerGame.Multiplayer;
using System;
using System.Windows.Input;

namespace StendenClickerGame.ViewModels
{
	public class CurrencyTrayViewModel : ViewModelBase
	{
		//event handlers
		public static event EventHandler CurrencyAdded;
		public static event EventHandler CurrencyRemoved;

		public delegate void OnClickAbilityProcessHandler(GamePlatform platform, BatchedClick clicks);

		//click eventhandler
		public static event OnClickAbilityProcessHandler OnClickAbilityProcess;
		public static int AbilityMultiplier;
		public event EventHandler OnMonsterDefeated;

		private ReusableCurrencyPool CurrencyPool { get { return ReusableCurrencyPool.GetInstance(); } }

		//bindables
		public CustomCoinList<Currency> CurrencyInView { get; set; }
		public ICommand TappedEvent { get; set; }
		public int? MonsterHealthPercentage { get { return CurrentLevel?.Monster?.GetHealthPercentage(); } }

		public string LevelProgressString { get { return $"Level voortgang {CurrentScene?.CurrentMonster}/{CurrentScene?.MonsterCount}"; } }
		public string CurrentStageString
		{
			get
			{
				if (CurrentMonster is Boss)
					return $"Stage {CurrentMonster.Name}";
				else
					return $"Stage {CurrentPlayer?.State?.LevelsDefeated}";
			}
		}

		//Context variables
		public GamePlatform CurrentLevel { get { return CurrentSession?.CurrentLevel; } }
		public MultiPlayerSession CurrentSession { get { return MultiplayerHubProxy.Instance?.GetContext(); } }
		public AbstractMonster CurrentMonster { get { return (AbstractMonster)CurrentLevel?.Monster; } }
		public AbstractScene CurrentScene { get { return (AbstractScene)CurrentLevel?.Scene; } }
		public Player CurrentPlayer { get { return MultiplayerHubProxy.Instance.CurrentPlayer; } }
		public PlayerCurrency Wallet { get { return CurrentPlayer?.Wallet; } }

		//clicks collection
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
			//check if there is a monster to click on:
			if (CurrentMonster != null)
			{
				//execute pending abilities
				AbilityMultiplier = 1;
				OnClickAbilityProcess?.Invoke(CurrentLevel, Clicks);

				int damage = 100 * CurrentPlayer.GetDamageFactor() * AbilityMultiplier;
				if (CurrentMonster.GetHealth() >= damage)
				{
					Clicks.AddDamage(damage);
					MonsterClickProcessor(damage);
				}
				else
				{
					while (damage > 0)
					{
						int CurrentMonsterHealth = CurrentMonster.GetHealth();
						int damageToDo = damage - CurrentMonsterHealth;
						if (damageToDo <= 0)
						{
							//to process the last bits of damage without the loop
							Clicks.AddDamage(damage);
							MonsterClickProcessor(damage);
							break;
						}

						damage -= damageToDo;
						Clicks.AddDamage(damageToDo);
						MonsterClickProcessor(damageToDo);
					}
				}
			}
		}

		private void MonsterClickProcessor(int damage)
		{
			CurrentMonster?.DoDamage(damage); //actually attack the monster

			NotifyPropertyChanged("MonsterHealthPercentage");

			if (CurrentMonster.IsDefeated())
			{
				//get the rewards for this monster and generate a new level
				var rewards = CurrentMonster.GetReward();
				for (ulong i = 0; i < rewards.SparkCoin; i++)
				{
					CreateCoin(typeof(SparkCoin), rewards.Factor);
				}
				for (ulong i = 0; i < rewards.EuropeanCredit; i++)
				{
					CreateCoin(typeof(EuropeanCredit), 0);
				}

				UpdatePlayerStatsAfterMonsterDefeat(true);

				//build a new level from the current player list, in singleplayer mode that list contains 1 player.			
				RenderLevel();
				OnMonsterDefeated?.Invoke(CurrentMonster, null);
			}
		}

		public void ProcessMultiplayerDamage(BatchedClick damage)
		{
			int totalDamageToProcess = damage.GetDamage();
			if (CurrentMonster.GetHealth() >= totalDamageToProcess)
			{
				MonsterClickProcessor(totalDamageToProcess);
			}
			else
			{
				while (totalDamageToProcess > 0)
				{
					int damageForCurrentMonster = CurrentMonster.GetHealth();
					int damageToDo = totalDamageToProcess - damageForCurrentMonster;
					if (damageToDo <= 0)
					{
						MonsterClickProcessor(damageToDo);
						break;
					}

					totalDamageToProcess -= damageToDo;
					MonsterClickProcessor(damageToDo);
				}
			}
		}

		private void UpdatePlayerStatsAfterMonsterDefeat(bool MonsterDefeated)
		{
			foreach (Player player in CurrentSession.CurrentPlayerList)
			{
				if (MonsterDefeated)
				{
					if (CurrentMonster is Boss)
					{
						player.State.BossesDefeated++;
					}
					else
					{
						player.State.MonstersDefeated++;
					}
				}
			}
		}

		private void RenderLevel()
		{
			CurrentSession.CurrentLevel = MultiplayerHubProxy.Instance.LevelGenerator.BuildLevel(CurrentSession.CurrentPlayerList); //broadcast the generated thing if you are the host.
			NotifyPropertyChanged("CurrentMonster");
			NotifyPropertyChanged("CurrentScene");
			NotifyPropertyChanged("CurrencyTray.CurrentMonster");
			NotifyPropertyChanged("CurrencyTray");
			NotifyPropertyChanged("MonsterHealthPercentage");
			NotifyPropertyChanged("LevelProgressString");
			NotifyPropertyChanged("CurrentStageString");
		}

		/// <summary>
		/// Handles coin creation through the CurrencyPool
		/// </summary>
		/// <param name="type">The type of coin to create</param>
		public void CreateCoin(Type type, double factor)
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

			int monsterDefeatedCoinValue = CurrentPlayer.State.MonstersDefeated + 1;

			coin.OnCoinHover += (o, e) =>
			{
				if (o is SparkCoin)
				{
					CurrentPlayer.Wallet.SparkCoin += (ulong)(((Currency)o).GetValue(monsterDefeatedCoinValue) * factor);
				}
				else if (o is EuropeanCredit)
				{
					CurrentPlayer.Wallet.EuropeanCredit += ((Currency)o).GetValue(-1);
				}

				//notify the UI that currency amounts have changed:
				NotifyPropertyChanged("Wallet");

				//remove all hover events, this is needed to prevent double event firing after the coin is reused.
				((Currency)o).RemoveHoverEvents();

				//remove the coins from the view and list.
				CurrencyInView.Remove((Currency)o);
				CurrencyPool.ReleaseCurrency((Currency)o);
			};

			coin.SetAutoRemove();

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