using System;

namespace Console_Runner.AMR
{
	public enum ActivityLevel
    {
		light,
		moderate,
		heavy
    }
	public class AMR
	{
		public bool IsMale { get; set; }
		// weight, height, and age must be non-negative values.
		public int Weight 
		{ 
			get; 
			set
            {
				if (value > 0)
                {
					Weight = value;
                }
            }; 
		}
		public float Height 
		{ 
			get;
            set
            {
				if (value > 0f)
                {
					Weight = value;
                }
            }; 
		}
		public int Age 
		{ 
			get; 
			set
            {
				if(value > 0)
                {
					Age = value;
                }
            }; 
		}
		public ActivityLevel Activity { get; set; }
		public bool IsCustomAMR { get; set; }
		// CustomAMR should only be accessible if IsCustomAMR is true
		public double CustomAMR 
		{
            get
            {
                if (IsCustomAMR)
                {
					return CustomAMR;
                }
				else
                {
					return -1;
                }
            }; 
			set
			{
                if (IsCustomAMR)
                {
					CustomAMR = value;
                }
            }; 
		}
		public AMR()
		{

		}
	}
}

