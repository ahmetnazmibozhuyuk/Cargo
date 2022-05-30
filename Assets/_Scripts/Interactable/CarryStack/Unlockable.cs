using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using ObjectPooling;
using Cargo.Managers;

namespace Cargo.Interactable
{
    public class Unlockable : MonoBehaviour, IInteractable
    {
        public bool FullCapacity { get; set; }
        public InteractableType Type { get; set; }

        [SerializeField] private Transform objectMovePoint;
        [SerializeField] private int unlockAmount;

        private void Awake()
        {
            Type = InteractableType.Unlockable;
        }
        public GameObject GiveObject()
        {
            return null;
        }
        public void TakeObject(GameObject givenObj, Transform parent)
        {
            if (givenObj == null) return;
            if (FullCapacity) return;
            givenObj.transform.DORotateQuaternion(objectMovePoint.rotation, 0.5f);
            givenObj.transform.DOJump(objectMovePoint.position, 2, 1, 0.5f).OnComplete(() =>
            {
                givenObj.transform.localScale = new Vector3(0, 0, 0);
                ObjectPool.Despawn(givenObj);
            });
            unlockAmount--;
            GameManager.instance.AddPoint();
            if (unlockAmount <= 0)
            {
                FullCapacity = true;

            }
        }

    }
}