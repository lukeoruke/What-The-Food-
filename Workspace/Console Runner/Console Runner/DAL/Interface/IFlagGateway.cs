using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.Food;

namespace Console_Runner.DAL
{
    public interface IFlagGateway
    {
        /// <summary>
        /// Checks whether a FoodFlag associated with the email and ingredientID exists on the database.
        /// </summary>
        /// <param name="email">The email address associated with the Account to search for.</param>
        /// <param name="ingredientID">The ingredient identifier associated with the Ingredient to search for a flag of.</param>
        /// <returns>True if the given email's Account has a food flag with the IngredientID associated with it.</returns>
        public bool AccountHasFlag(string email, string ingredientID);
        /// <summary>
        /// Removes the FoodFlag associated with email and ingredientID from the database.
        /// </summary>
        /// <param name="email">The email address whose Account the FoodFlag is associated with.</param>
        /// <param name="ingredientID">The identifier of the Ingredient that the FoodFlag is associated with.</param>
        /// <returns>True if the FoodFlag associated with email and ingredientID is removed from the database</returns>
        public bool RemoveFoodFlag(string email, string ingredientID);
        /// <summary>
        /// Gets all FoodFlags associated with email.
        /// </summary>
        /// <param name="email">The email address to find FoodFlags associated with.</param>
        /// <returns>A List\<FoodFlag> containing all FoodFlags associated with email.</returns>
        public List<FoodFlag> GetAllAccountFlags(string email);
        /// <summary>
        /// Adds the FoodFlag onto the database.
        /// </summary>
        /// <param name="flag">The FoodFlag to add.</param>
        /// <returns>True if the FoodFlag was added to the database, otherwise false.</returns>
        public bool AddFlag(FoodFlag flag);
    }
}
