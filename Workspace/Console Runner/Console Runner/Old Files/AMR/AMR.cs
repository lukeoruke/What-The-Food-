using System;
using Console_Runner.User_Management;

namespace Console_Runner.AMRModel
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

	static class Constants
    {
		public static float GetActivityLevelMultiplier(ActivityLevel level)
        {
			return level switch
			{
				ActivityLevel.None => 1.2f,
				ActivityLevel.Light => 1.375f,
				ActivityLevel.Moderate => 1.55f,
				ActivityLevel.Daily => 1.725f,
				ActivityLevel.Heavy => 1.9f,
				_ => 1.0f
			};
		}
		public static float BMR_WEIGHT_MULTIPLIER = 10;
		public static float BMR_HEIGHT_MULTIPLIER = 6.25f;
		public static float BMR_AGE_MULTIPLIER = -5;
		public static int BMR_FEMALE_ADDEND = -161;
		public static int BMR_MALE_ADDEND = 5;
    }

	public class AMR
	{
		private int _weight;
		private float _height;
		private int _age;
		private float _customAMR;

		public string AccountEmail { get; set; }
		public Account Account { get; set; }
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
		public AMR()
        {

        }

		/// <summary>
		/// Constructor for non-custom AMRs.
		/// </summary>
		/// <param name="acct"></param>
		/// <param name="isMale"></param>
		/// <param name="weight"></param>
		/// <param name="height"></param>
		/// <param name="age"></param>
		/// <param name="activity"></param>
		public AMR(Account acct, bool isMale, int weight, float height, int age, ActivityLevel activity)
		{
			Account = acct;
			AccountEmail = acct.Email;
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
		/// <param name="acct"></param>
		/// <param name="isMale"></param>
		/// <param name="weight"></param>
		/// <param name="height"></param>
		/// <param name="age"></param>
		/// <param name="activity"></param>
		/// <param name="customAMR"></param>
		public AMR(Account acct, bool isMale, int weight, float height, int age, ActivityLevel activity, float customAMR)
		{
			Account = acct;
			AccountEmail = acct.Email;
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
		/// https://doi.org/10.1093/ajcn/51.2.241
		/// to see the equation.
		/// </para>
		/// </summary>
		/// <returns>A double representing the AMR calculated from the properties in this class.</returns>
		public float CalculateAMR()
		{
            if (IsCustomAMR) { return _customAMR; }
            else
            {
				// equation for male and female are the same without the addend
				float bmrWithoutAddend = (Constants.BMR_WEIGHT_MULTIPLIER * Weight) + (Constants.BMR_HEIGHT_MULTIPLIER * Height)
								+ (Constants.BMR_AGE_MULTIPLIER * Age);
				// add the addend corresponding to whether the associated user is biologically male or female
				float bmr = bmrWithoutAddend + (IsMale ? Constants.BMR_MALE_ADDEND : Constants.BMR_FEMALE_ADDEND);
				// multiply by factor corresponding to activity level and return that product
				return bmr * Constants.GetActivityLevelMultiplier(Activity);
            }
		}
	}
}

