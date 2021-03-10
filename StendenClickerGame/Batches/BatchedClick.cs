namespace StendenClickerGame.Batches
{
	public class BatchedClick : IBatchProcessable<BatchedClick>
	{
		private int clickCount;

		public void addClick()
		{
			processData<BatchedClick>();
		}

        public void processData<T>() where T : BatchedClick
        {
            throw new System.NotImplementedException();
        }
    }

}

