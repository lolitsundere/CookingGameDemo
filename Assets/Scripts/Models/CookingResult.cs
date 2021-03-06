﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class CookingResult : Food
    {
        public List<Food> IngredientsList { get; private set; }
        public float CookingTime { get; private set; }
        public bool CanBunrt { get; private set; }
        public float BunrtTime { get; private set; }

        public bool CanCookWith(List<Food> foods)
        {
            foreach (var item in IngredientsList)
            {
                if (!foods.Contains(item))
                {
                    return false;   
                }
            }
            return true;
        }
    }
}
