using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class Food
    {
        public string Name { get; private set; }
        public int SellPrice { get; private set; }

        public override bool Equals(object obj)
        {
            var Food = obj as Food;
            if (Food == null || !Food.Name.Equals(Name))
            {
                return false;
            }
            return true;
        }
    }
}
