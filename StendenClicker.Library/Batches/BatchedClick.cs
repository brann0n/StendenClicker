namespace StendenClicker.Library.Batches
{
	public class BatchedClick
	{
		public int ClickCount { get; set; } = 0;

		public void addClick()
		{
			ClickCount++;
		}

		public int getClicks()
		{
			return ClickCount;
		}
    }
}

