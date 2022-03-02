using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_Runner.Food;

namespace Console_Runner.DAL
{
    public interface IIngredientGateway
    {
        public List<Ingredient> retrieveIngredientList(string labelID);
        public bool addIngredient(Ingredient ingredient);
        public bool removeIngredient(Ingredient ingredient);
    }
}
