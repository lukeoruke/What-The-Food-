
using System.ComponentModel.DataAnnotations.Schema;

namespace Console_Runner.AccountService
{
	
	//Enum that reflects the five levels of activity used in calculating AMR according to https://www.verywellfit.com/how-many-calories-do-i-need-each-day-2506873.
	//kg and cm conversions are referenced to https://www.metric-conversions.org/weight/pounds-to-kilograms.htm
	public enum ActivityLevel
	{
		//Activities are declared
		None,
		Light,
		Moderate,
		Daily,
		Heavy
	}

	static class Constants
	{
		//Switch case to check which activity level was selected
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

		//Values in the equation are declared
		//Calculations will vary off of gender
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
		//weight, height, and age must be non-negative values.
		//assume properties are measured in metric units - kg, cm, etc.

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
		//CustomAMR should only be accessible if IsCustomAMR is true
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

		//Constructors
		public AMR()
		{

		}


		//Constructor for non-custom AMRs.
		//<param name="acct"></param>
		//<param name="isMale"></param>
		//<param name="weight"></param>
		//<param name="height"></param>
		//<param name="age"></param>
		//<param name="activity"></param>
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


		//Constructor for custom AMRs.
		//<param name="isMale"></param>
		//<param name="weight"></param>
		//<param name="height"></param>
		//<param name="age"></param>
		//<param name="activity"></param>
		//<param name="customAMR"></param>
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


		//Calculates the AMR based off of https://www.verywellfit.com/how-many-calories-do-i-need-each-day-2506873.
		//A double representing the AMR calculated from the properties in this class.
		public float CalculateAMR()
		{
			if (IsCustomAMR) { return _customAMR; }
			else
			{
				float bmr = 0;
				if (IsMale == true)
				{
					//Calculations if the user is male
					float mbmr = (Constants.BMR_MALE_ADD + (Constants.MALE_BMR_WEIGHT_MULTIPLIER*Weight)+(Constants.MALE_BMR_HEIGHT_MULTIPLIER*Height)
						-(Constants.MALE_BMR_AGE_MULTIPLIER * Age)); 
					bmr = mbmr;
				}
                else
                {
					//Calculations if the user is female
					float fbmr = (Constants.BMR_FEMALE_ADD + (Constants.FEMALE_BMR_WEIGHT_MULTIPLIER * (Weight)) + (Constants.FEMALE_BMR_HEIGHT_MULTIPLIER * Height)
						- (Constants.FEMALE_BMR_AGE_MULTIPLIER * Age));
					bmr = fbmr;
				}

				return bmr * Constants.GetActivityLevelMultiplier(Activity);
			}
		}
	}
}
