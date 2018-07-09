using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class CookingTool
    {
        public string Name { get; private set; }
        public List<CookingResult> FoodCanBeMade { get; private set; }
        public List<Food> FoodInside { get; private set; }
        public bool IsCooking { get; private set; }
        public Food ServedFood { get; private set; }
        public bool IsDone { get; private set; }
        public Food GetCookedFood()
        {
            Food result;
            if (IsCooking)
            {
                if (IsDone)
                {
                    result = ServedFood;
                    ServedFood = null;
                    IsDone = false;
                    return result;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public Food GetFoodInside()
        {
            Food result;
            if (FoodInside.Count > 0)
            {
                result = FoodInside[0];
                FoodInside.RemoveAt(0);
                return result;
            }
            return null;
        }

        public List<Food> Cook(List<Food> foods)
        {
            foods.AddRange(FoodInside);
            if (!IsCooking)
            {
                foreach (var food in FoodCanBeMade)
                {
                    if (food.CanCookWith(foods))
                    {
                        IsCooking = true;
                        foreach(var ingredient in food.IngredientsList)
                        {
                            foods.Remove(ingredient);
                        }
                        StartCook(food);
                        return foods;
                    }
                }
            }
            return null;
        }

        public void StartCook(CookingResult food)
        {

        }
    }
}
