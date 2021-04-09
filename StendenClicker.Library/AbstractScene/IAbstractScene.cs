namespace StendenClicker.Library.AbstractScene
{
	public interface IAbstractScene
	{
		public int CurrentMonster { get; set; }

		public int MonsterCount { get; set; }

		public string Background { get; set; }

		public string Name { get; set; }
	}
}
