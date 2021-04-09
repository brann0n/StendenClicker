namespace StendenClicker.Library.Batches
{
	public class BatchedClick
	{
		public int DamageDone { get; set; } = 0;

		public void AddDamage(int damage)
		{
			DamageDone += damage;
		}

		public int GetDamage()
		{
			return DamageDone;
		}
	}
}