using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class Customer
    {
        public Food DesiredFood {get; private set;}
        public float CustomerMaxWaitingTime { get; private set; }
        public int MaxTip { get; private set; }

        public void GetServed()
        {

        }
    }
}
