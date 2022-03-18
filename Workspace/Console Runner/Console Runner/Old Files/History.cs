using Console_Runner.DAL;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Console_Runner
{
	public class History
	{
		public string email { get; set; }
		public string foodItems { get; set; }

		public History()
        {

        }

		public bool addHistoryItem()
		{
			throw new NotImplementedException();
		}
	}
}

