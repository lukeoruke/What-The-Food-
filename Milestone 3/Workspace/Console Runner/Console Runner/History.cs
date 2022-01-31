using System;
namespace Console_Runner
{
	public class History
	{
		private Queue<string> FoodItems;
		private int queueSize;

		public History()
		{
			FoodItems = new Queue<string>();
			queueSize = 25;
		}

		public int getCount()
        {
			return FoodItems.Count;
        }

		public string remove()
		{
			return FoodItems.Dequeue();
		}

		public void add(string foodItem)
        {
			if(getCount() == queueSize)
            {
				remove();
            }
			FoodItems.Enqueue(foodItem);
        }		
	}
}

