namespace StendenClicker.Library.Models
{
	public class PlayerState
	{
		public int MonstersDefeated { get; set; }
		public int LevelsDefeated { get => (MonstersDefeated / 5) + 1; }
		public int BossesDefeated { get; set; }
	}
}
