using System;
using UnityEngine;

namespace Game.Scripts.Customers
{
    public class CustomerReaction : MonoBehaviour
    {
        public event Action OnSuccessOrder;
    }
}