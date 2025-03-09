using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Logic.PurchaseAreaLogic
{
    public class PurchasableArea : MonoBehaviour
    {
        [SerializeField] private PurchasableAreaTypeId _areaTypeId;
        [SerializeField] private List<GameObject> _triggerPoints;
        
        [Header("AreaObjects")]
        [SerializeField] private GameObject _objectsToShow;
        [SerializeField] private GameObject _objectsToHide;
        
        public PurchasableAreaTypeId AreaTypeId => _areaTypeId;
        public List<GameObject> TriggerPoints => _triggerPoints;
        public GameObject ObjectsToShow => _objectsToShow;
        public GameObject ObjectsToHide => _objectsToHide;
    }
}