
namespace StendenClickerGame.Batches
{
	public interface IBatchProcessable<in Ta>
	{
		void processData<T>() where T : Ta;
	}
}

