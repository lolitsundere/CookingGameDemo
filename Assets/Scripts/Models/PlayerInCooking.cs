using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class PlayerInCooking
    {
        public Food LeftHandFood { get; private set; }
        public Food RightHandFood { get; private set; }

        public bool GetFood(Food food)
        {
            if (LeftHandFood == null)
            {
                LeftHandFood = food;
            }
            else if (RightHandFood == null)
            {
                RightHandFood = food;
            }
            else
            {
                if (LeftHandFood is CookingResult && RightHandFood is CookingResult)
                {
                    return false;
                }
                else if (LeftHandFood is CookingResult)
                {
                    if (RightHandFood.Equals(food))
                    {
                        RightHandFood = null;
                    }
                    else
                    {
                        RightHandFood = food;
                    }
                }
                else if (RightHandFood is CookingResult)
                {
                    if (LeftHandFood.Equals(food))
                    {
                        LeftHandFood = RightHandFood;
                        RightHandFood = null;
                    }
                    else
                    {
                        LeftHandFood = food;
                    }
                }
                else
                {
                    if (LeftHandFood.Equals(food) && RightHandFood.Equals(food))
                    {
                        LeftHandFood = null;
                        RightHandFood = null;
                    }
                    else if (LeftHandFood.Equals(food))
                    {
                        LeftHandFood = RightHandFood;
                        RightHandFood = null;
                    }
                    else if (RightHandFood.Equals(food))
                    {
                        RightHandFood = null;
                    }
                }
            }
            return true;
        }

        public bool InteractWithTool(CookingTool tool)
        {
            if (LeftHandFood is CookingResult && RightHandFood is CookingResult)
            {
                return false;
            }

            //拿取做好的食物
            Food temp = tool.GetCookedFood();
            if (!GetFood(temp))
            {
                return false;
            }

            List<Food> tempList;

            //测试加入左手食物后是否可以合成
            if (LeftHandFood != null)
            {
                tempList = new List<Food>(new Food[] { LeftHandFood });
                tempList = tool.Cook(tempList);
                if (temp != null)
                { 
                    switch (tempList.Count)
                    {
                        case 0:
                            LeftHandFood = null;
                            RightHandFood = null;
                            break;
                        case 1:
                            LeftHandFood = tempList[0];
                            RightHandFood = null;
                            break;
                        case 2:
                            LeftHandFood = tempList[0];
                            RightHandFood = tempList[1];
                            break;
                    }
                    return true;
                }
            }

            //测试加入右手食物后是否可以合成
            if (RightHandFood != null)
            {
                tempList = new List<Food>(new Food[] { RightHandFood });
                tempList = tool.Cook(tempList);
                if (temp != null)
                {
                    switch (tempList.Count)
                    {
                        case 0:
                            LeftHandFood = null;
                            RightHandFood = null;
                            break;
                        case 1:
                            LeftHandFood = tempList[0];
                            RightHandFood = null;
                            break;
                        case 2:
                            LeftHandFood = tempList[0];
                            RightHandFood = tempList[1];
                            break;
                    }
                    return true;
                }
            }

            //测试加入所有食物后是否可以合成
            if (RightHandFood != null && LeftHandFood != null)
            {
                tempList = new List<Food>(new Food[] { RightHandFood, LeftHandFood });
                tempList = tool.Cook(tempList);
                if (temp != null)
                {
                    switch (tempList.Count)
                    {
                        case 0:
                            LeftHandFood = null;
                            RightHandFood = null;
                            break;
                        case 1:
                            LeftHandFood = tempList[0];
                            RightHandFood = null;
                            break;
                        case 2:
                            LeftHandFood = tempList[0];
                            RightHandFood = tempList[1];
                            break;
                    }
                    return true;
                }
            }

            //拿取材料
            temp = tool.GetFoodInside();
            if (!GetFood(temp))
            {
                return false;
            }

            return true;
        }
    }
}
