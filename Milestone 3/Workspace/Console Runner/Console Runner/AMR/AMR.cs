using System;

namespace Console_Runner.AMR
{
	// internal enum - make private, protected, or public???
	public enum ActivityLevel
    {
		None,
		Light,
		Moderate,
		Daily,
		Heavy
    }

	public class AMR
	{
		private int _weight;
		private float _height;
		private int _age;
		private double _customAMR;

		public bool IsMale { get; set; }
		// weight, height, and age must be non-negative values.
		// assume properties are measured in metric units - kg, cm, etc.
		public int Weight
        {
            get => _weight;
            set
            {
				if (value > 0) { _weight = value; }
            } 
		}
		public float Height 
		{ 
			get => _height;
            set
            {
				if (value > 0f) { _height = value; }
            } 
		}
		public int Age 
		{ 
			get => _age; 
			set
            {
				if(value > 0) { _age = value; }
            } 
		}
		public ActivityLevel Activity { get; set; }
		public bool IsCustomAMR { get; set; }
		// CustomAMR should only be accessible if IsCustomAMR is true
		public double CustomAMR 
		{
            get
            {
				return IsCustomAMR ? CustomAMR : -1;
            } 
			set
			{
                if (IsCustomAMR) { CustomAMR = value; }
            } 
		}

		// ctor
		public AMR()
		{
			
		}

		// methods

		/// <summary>
		/// <para>Calculates active metabolic rate (AMR) from this object's properties using the Harris-Benedict equation.
		/// Refer to:
		/// https://www.verywellfit.com/how-many-calories-do-i-need-each-day-2506873
        /// to see the equation.
		/// </para>
		/// </summary>
		/// <returns>A double representing the AMR calculated from the properties in this class.</returns>
		public double CalculateAMR()
		{
			if (IsCustomAMR) { return CustomAMR; }
            else
            {
				// if male, use first equation; if female, use second equation
				double bmr = IsMale ? 66.47 + (13.75 * Weight) + (5.003 * Height) - (6.755 * Age) :
									  655.1 + (9.563 * Weight) + (1.85 * Height) - (4.676 * Age);
				double amr = Activity switch
				{
					ActivityLevel.None => bmr * 1.2,
					ActivityLevel.Light => bmr * 1.375,
					ActivityLevel.Moderate => bmr * 1.55,
					ActivityLevel.Daily => bmr * 1.725,
					ActivityLevel.Heavy => bmr * 1.9,
					_ => bmr
				};
				return amr;
            }
		}
	}
}

