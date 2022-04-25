
using System.ComponentModel.DataAnnotations.Schema;

namespace Console_Runner.AccountService
{
	/// <summary>
	/// Enum that reflects the five levels of activity used in calculating AMR according to https://www.verywellfit.com/how-many-calories-do-i-need-each-day-2506873.
	/// kg and cm conversions are referenced to https://www.metric-conversions.org/weight/pounds-to-kilograms.htm
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
		public static float BMR_MALE_ADD = 66.47f;
		public static float MALE_BMR_WEIGHT_MULTIPLIER = 13.75f;
		public static float MALE_BMR_HEIGHT_MULTIPLIER = 5.003f;
		public static float MALE_BMR_AGE_MULTIPLIER = -6.755f;

		public static float BMR_FEMALE_ADD = 65.51f;
		public static float FEMALE_BMR_WEIGHT_MULTIPLIER = 9.563f;
		public static float FEMALE_BMR_HEIGHT_MULTIPLIER = 1.850f;
		public static float FEMALE_BMR_AGE_MULTIPLIER = -4.676f;
	}

	public class AMR
	{
		private int _weight;
		private float _height;
		private int _age;
		private float _customAMR;
		

		[ForeignKey("UserID")]
		public int UserID { get; set; }
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
				if (value > 0) { _age = value; }
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

		// Constructors
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
				float bmr = 0;
				if (IsMale == true)
				{
					float mbmr = (Constants.BMR_MALE_ADD + (Constants.MALE_BMR_WEIGHT_MULTIPLIER*Weight)+(Constants.MALE_BMR_HEIGHT_MULTIPLIER*Height)
						-(Constants.MALE_BMR_AGE_MULTIPLIER * Age)); 
					bmr = mbmr;
				}
                else
                {
					float fbmr = (Constants.BMR_FEMALE_ADD + (Constants.FEMALE_BMR_WEIGHT_MULTIPLIER * (Weight)) + (Constants.FEMALE_BMR_HEIGHT_MULTIPLIER * Height)
						- (Constants.FEMALE_BMR_AGE_MULTIPLIER * Age));
					bmr = fbmr;
				}
				
				// equation for male and female are the same without the addend
				//float bmrWithoutAddend = (Constants.BMR_WEIGHT_MULTIPLIER * Weight) + (Constants.BMR_HEIGHT_MULTIPLIER * Height)
				//				+ (Constants.BMR_AGE_MULTIPLIER * Age);
				// add the addend corresponding to whether the associated user is biologically male or female
				//float bmr = bmrWithoutAddend + (IsMale ? Constants.BMR_MALE_ADDEND : Constants.BMR_FEMALE_ADDEND);
				// multiply by factor corresponding to activity level and return that product
				return bmr * Constants.GetActivityLevelMultiplier(Activity);
			}
		}
	}
}
