using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Models
{
    public class PlayerInCooking
    {
        public Food LeftHandFood { get; private set; }
        public Food RightHandFood { get; private set; }

        /// <summary>
        /// 拿取食物(从素材或工具中)
        /// </summary>
        /// <param name="food"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 与工具互动(合成/放入/拿取)
        /// </summary>
        /// <param name="tool"></param>
        /// <returns></returns>
        public bool InteractWithTool(CookingTool tool)
        {
            if (LeftHandFood is CookingResult && RightHandFood is CookingResult)
            {
                return false;
            }

            if (tool.IsCooking && !tool.IsDone)
            {
                return false;
            }

            //拿取做好的食物
            if (tool.IsDone)
            {
                Food temp = tool.GetCookedFood();
                if (temp != null && !GetFood(temp))
                {
                    return false;
                }
            }

            List<Food> tempList;
            //测试加入左手食物后是否可以合成
            if (LeftHandFood != null)
            {
                tempList = new List<Food>(new Food[] { LeftHandFood });
                tempList = tool.Cook(tempList);
                if (tempList != null)
                { 
                    switch (tempList.Count)
                    {
                        case 0:
                            LeftHandFood = null;
                            break;
                        case 1:
                            LeftHandFood = tempList[0];
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    return true;
                }
            }

            //测试加入右手食物后是否可以合成
            if (RightHandFood != null)
            {
                tempList = new List<Food>(new Food[] { RightHandFood });
                tempList = tool.Cook(tempList);
                if (tempList != null)
                {
                    switch (tempList.Count)
                    {
                        case 0:
                            RightHandFood = null;
                            break;
                        case 1:
                            RightHandFood = tempList[0];
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    return true;
                }
            }

            //测试加入所有食物后是否可以合成
            if (RightHandFood != null && LeftHandFood != null)
            {
                tempList = new List<Food>(new Food[] { RightHandFood, LeftHandFood });
                tempList = tool.Cook(tempList);
                if (tempList != null)
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
                        default:
                            throw new NotImplementedException();
                    }
                    return true;
                }
            }

            bool put = false;
            //查看食物是否可以加入食材
            if (LeftHandFood != null && tool.PutFood(LeftHandFood))
            {
                put = true;
            }
            if (RightHandFood != null && tool.PutFood(RightHandFood))
            {
                put = true;
            }


            //拿取材料
            if (!put)
            {
                Food temp = tool.GetFoodInside();
                if (!GetFood(temp))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 将食物交给顾客
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public bool ServeFood(Customer customer)
        {
            if (RightHandFood != null && customer.DesiredFood.Equals(RightHandFood))
            {
                RightHandFood = null;
                customer.GetServed();
                return true;
            }
            if (LeftHandFood != null && customer.DesiredFood.Equals(LeftHandFood))
            {
                LeftHandFood = RightHandFood;
                RightHandFood = null;
                customer.GetServed();
                return true;
            }
            return false;
        }
    }
}
