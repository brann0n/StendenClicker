namespace StendenClicker.Library.Batches
{
	public class BatchedClick : IBatchProcessable<BatchedClick>
	{
		private int clickCount;

		public void addClick()
		{
			
		}

        public void processData<T>() where T : BatchedClick
        {
            throw new System.NotImplementedException();
        }
    }

}

