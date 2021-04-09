namespace StendenClicker.Library.Batches
{
	public class BatchedClick
	{
		public int DamageDone { get; set; } = 0;

		public void addClick(int damage)
		{
			DamageDone += damage;
		}

		public int getClicks()
		{
			return DamageDone;
		}
    }
}

