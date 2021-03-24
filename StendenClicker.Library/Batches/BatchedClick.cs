namespace StendenClicker.Library.Batches
{
	public class BatchedClick : IBatchProcessable<BatchedClick>
	{
		private int clickCount { get; set; } = 0;

		public void addClick()
		{
			clickCount++;
		}
		//should use processing context of the player?
        public void processData<T>() where T : BatchedClick
        {
            throw new System.NotImplementedException();
        }
    }

}

