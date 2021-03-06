﻿using StendenClicker.Library.Factory;
using StendenClicker.Library.Multiplayer;
using StendenClicker.Library.PlayerControls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StendenClickerApi.Helpers
{
	/// <summary>
	/// Extension class for the session dictionary that provides locking for all access to the dictionary
	/// However this does not lock read/write access to the fields inside the objects.
	/// </summary>
	public static class SessionExtensions
	{
		private static readonly Dictionary<string, MultiPlayerSession> Sessions = new Dictionary<string, MultiPlayerSession>();

		private static readonly object AccessLock = new object();

		public static List<MultiPlayerSession> Get()
		{
			lock (AccessLock)
				return Sessions.Values.ToList();
		}
		public static MultiPlayerSession Get(string key)
		{
			lock (AccessLock)
				return Sessions[key];
		}

		public static void UpdatePlayers(string key, List<Player> players)
		{
			lock (AccessLock)
				Sessions[key].CurrentPlayerList = players;
		}

		public static void UpdateLevel(string key, GamePlatform gamePlatform)
		{
			lock (AccessLock)
				Sessions[key].CurrentLevel = gamePlatform;
		}

		public static void Add(string key, MultiPlayerSession Session)
		{
			Session.CurrentPlayerList.Add(new Player { UserId = Guid.Empty });
			lock (AccessLock)
				Sessions.Add(key, Session);
		}

		public static void Remove(string key)
		{
			lock (AccessLock)
				Sessions.Remove(key);
		}

		public static bool ContainsKey(string key)
		{
			lock (AccessLock)
				return Sessions.ContainsKey(key);
		}

		public static MultiPlayerSession GetSessionByAnyClientId(string key)
		{
			lock (AccessLock)
			{
				var session = Sessions.Values.Where(n => n.CurrentPlayerList.FirstOrDefault(m => m.UserId.ToString().Equals(key)) != null);
				if (session.Count() == 1)
				{
					return session.First();
				}
				else return null;
			}
		}
	}
}