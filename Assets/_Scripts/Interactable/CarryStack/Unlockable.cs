using UnityEngine;
using DG.Tweening;
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

        private readonly float _cargoJumpPower = 7f;
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
            givenObj.transform.DOJump(objectMovePoint.position, _cargoJumpPower, 1, 1.2f).OnComplete(() =>
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