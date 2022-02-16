using System;
namespace Console_Runner
{
	public class History
	{
		public string email { get; set; }
		public string foodItems { get; set; }

		public Queue<string> foodItemsQueue;
		private int queueSize;

		
		public History()
		{
			foodItemsQueue = new Queue<string>();
			queueSize = 25;
		}

		public int getCount()
        {
			return foodItemsQueue.Count;
        }

		public string remove()
		{
			return foodItemsQueue.Dequeue();
		}

		public void add(string foodItem)
        {
			if(getCount() == queueSize)
            {
				remove();
            }
			foodItemsQueue.Enqueue(foodItem);
        }		
	}
}

