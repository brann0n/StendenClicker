using StendenClicker.Library.Models;
using StendenClicker.Library.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StendenClicker.Library.PlayerControls
{
	public class Player
	{
		/// <summary>
		/// Generated in the database
		/// </summary>
		public Guid UserId { get; set; }

		public string Username { get; set; }

		public PlayerCurrency Wallet { get; set; }

		/// <summary>
		/// This is the physical ID of your device (not MAC)
		/// </summary>
		public string DeviceId { get; set; }

		public PlayerState State { get; set; }

		public List<PlayerHero> Heroes { get; set; }

		public static bool IsPlayerObjectEmpty(Player player)
		{
			if (player == null) return true;

			if (player.UserId == Guid.Empty) return true;

			if (string.IsNullOrEmpty(player.Username)) return true;

			if (string.IsNullOrEmpty(player.DeviceId)) return true;

			return false;
		}

		public static implicit operator Player(Models.DatabaseModels.Player player)
		{
			return new Player
			{
				DeviceId = player.DeviceId,
				Username = player.PlayerName,
				UserId = Guid.Parse(player.PlayerGuid),
				State = new PlayerState { BossesDefeated = player.BossesDefreated, MonstersDefeated = player.MonstersDefeated },
				Wallet = new PlayerCurrency { EuropeanCredit = player.EuropeanCredits, SparkCoin = player.SparkCoins },
				Heroes = player.Heroes.ToList()
			};
		}
		public int GetDamageFactor()
		{
			int damagefactor = 1;
			foreach(PlayerHero h in Heroes)
			{
				damagefactor += (h.HeroUpgradeLevel * h.Hero.HeroId);
			}

			return damagefactor;
		}
	}
}
