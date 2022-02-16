using System;
namespace Console_Runner
{
	public class History
	{
		public string email { get; set; }
		public string foodItems { get; set; }

		[NotMapped]
		
		public History()
		{
			queueSize = 25;
		}
	}
}

