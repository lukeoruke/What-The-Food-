using System;

namespace Console_Runner.AMR
{
	/// <summary>
	/// Enum that reflects the five levels of activity used in calculating AMR according to https://www.verywellfit.com/how-many-calories-do-i-need-each-day-2506873.
	/// </summary>
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
		private float _customAMR;

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
		public float CustomAMR 
		{
            get
            {
				return IsCustomAMR ? _customAMR : -1;
            } 
			set
			{
                if (IsCustomAMR) { _customAMR = value; }
            } 
		}

		// constructors

		/// <summary>
		/// Constructor for non-custom AMRs.
		/// </summary>
		/// <param name="isMale"></param>
		/// <param name="weight"></param>
		/// <param name="height"></param>
		/// <param name="age"></param>
		/// <param name="activity"></param>
		public AMR(bool isMale, int weight, float height, int age, ActivityLevel activity)
		{
			IsMale = isMale;
			Weight = weight;
			Height = height;
			Age = age;
			Activity = activity;
			IsCustomAMR = false;
			CustomAMR = 0;
		}

		/// <summary>
		/// Constructor for custom AMRs.
		/// </summary>
		/// <param name="isMale"></param>
		/// <param name="weight"></param>
		/// <param name="height"></param>
		/// <param name="age"></param>
		/// <param name="activity"></param>
		/// <param name="customAMR"></param>
		public AMR(bool isMale, int weight, float height, int age, ActivityLevel activity, float customAMR)
		{
			IsMale = isMale;
			Weight = weight;
			Height = height;
			Age = age;
			Activity = activity;
			IsCustomAMR = true;
			CustomAMR = customAMR;
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
		public float CalculateAMR()
		{
            if (IsCustomAMR) { return _customAMR; }
            else
            {
				// if male, use first equation; if female, use second equation
				float bmr = IsMale ? 66.47f + (13.75f * Weight) + (5.003f * Height) - (6.755f * Age) :
									  655.1f + (9.563f * Weight) + (1.85f * Height) - (4.676f * Age);
				// multiply by factor corresponding to activity level and return that product
				return Activity switch
				{
					ActivityLevel.None => bmr * 1.2f,
					ActivityLevel.Light => bmr * 1.375f,
					ActivityLevel.Moderate => bmr * 1.55f,
					ActivityLevel.Daily => bmr * 1.725f,
					ActivityLevel.Heavy => bmr * 1.9f,
					_ => bmr
				};
            }
		}
	}
}

