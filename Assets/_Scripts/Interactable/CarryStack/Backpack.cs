using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cargo.Managers;
using DG.Tweening;

namespace Cargo.Interactable
{

    // interactable interface'ini ikiye bölebilirsin; collectable tarzı değiştir crashable interfacei getir


    public class Backpack : MonoBehaviour, IInteractable
    {
        public bool FullCapacity { get; set; }
        public InteractableType Type { get; set; }
        public int Counter { get; private set; }

        [SerializeField] private Transform backpackTransform;

        [SerializeField] private int backpackCapacity;



        [SerializeField] private GathererType gathererType;

        private List<ObjectData> _objectDataList = new List<ObjectData>();

        private Animator _animator;

        private readonly float gatherRate = 0.05f;

        private float _localY;
        private float _objectTransferSpeed = 0.2f;

        private readonly float _cargoJumpPower = 4f;

        private bool _inTheZone;

        private int _cargoGathered = 0;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        private void Start()
        {
            InitializePositions();
        }
        private void InitializePositions()
        {
            for (int i = 0; i < backpackCapacity; i++)
            {
                _localY++;
                _objectDataList.Add(new ObjectData(new Vector3(0, _localY, 0)));
            }
        }
        public void TakeObject(GameObject givenObj, Transform parent)
        {
            if (Counter < backpackCapacity)
            {
                if (givenObj == null) return;
                _animator.SetBool("IsCarrying", true);
                _objectDataList[Counter].ObjectHeld = givenObj;
                givenObj.transform.rotation = transform.rotation;
                givenObj.transform.SetParent(transform);

                givenObj.transform.DOJump(new Vector3(backpackTransform.position.x,
                    backpackTransform.position.y + _objectDataList[Counter].ObjectPosition.y,
                    backpackTransform.position.z), _cargoJumpPower, 1, _objectTransferSpeed);




                StartCoroutine(Co_CorrectCubePosition(givenObj, Counter));
                Counter++;
                if (Counter >= backpackCapacity) FullCapacity = true;
            }
        }
        private IEnumerator Co_CorrectCubePosition(GameObject go, int position)
        {
            yield return new WaitForSeconds(_objectTransferSpeed);
            go.transform.position = new Vector3(backpackTransform.position.x,
                    backpackTransform.position.y + _objectDataList[position].ObjectPosition.y,
                    backpackTransform.position.z);
        }
        public GameObject GiveObject()
        {
            if (Counter <= 0)
            {
                return null;
            }
            Counter--;
            if (Counter <= 0)
            {
                _animator.SetBool("IsCarrying", false);
            }
            FullCapacity = false;

            GameObject temp = _objectDataList[Counter].ObjectHeld;
            temp.transform.SetParent(null);
            _objectDataList[Counter].ObjectHeld = null;
            return temp;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (_inTheZone) return;
            if (other.GetComponent<IInteractable>() == null) return;
            IInteractable interactable = other.GetComponent<IInteractable>();
            _inTheZone = true;
            switch (interactable.Type)
            {
                case InteractableType.Generator:
                    StartCoroutine(Co_GetCubeFrom(interactable));
                    break;
                case InteractableType.Stockpile:
                    StartCoroutine(Co_SendCubeTo(interactable));
                    break;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            _inTheZone = false;
        }
        private IEnumerator Co_GetCubeFrom(IInteractable interactable)
        {
            while (_inTheZone && !FullCapacity)
            {
                if (_cargoGathered >= GameManager.instance.CargoCapacity)
                {
                    GameManager.instance.FullCapacity();
                    yield break;
                }
                GameObject takenObject = interactable.GiveObject();
                TakeObject(takenObject, transform);
                if (takenObject != null)
                    _cargoGathered++;
                yield return new WaitForSeconds(gatherRate);

            }
            yield break;
        }
        private IEnumerator Co_SendCubeTo(IInteractable interactable)
        {
            while (_inTheZone)
            {
                if (interactable.FullCapacity)
                {
                    yield return new WaitForSeconds(gatherRate);
                }
                else
                {
                    interactable.TakeObject(GiveObject(), null);
                    yield return new WaitForSeconds(gatherRate);
                }
            }
            yield break;
        }
    }
}
namespace Cargo
{
    [System.Serializable]
    public class ObjectData
    {
        public Vector3 ObjectPosition;
        public GameObject ObjectHeld;
        public ObjectData(Vector3 pos, GameObject obj)
        {
            ObjectPosition = pos;
            ObjectHeld = obj;
        }
        public ObjectData(Vector3 pos)
        {
            ObjectPosition = pos;
            ObjectHeld = null;
        }
    }
    public interface IInteractable
    {
        public void TakeObject(GameObject givenObj, Transform parent);
        public GameObject GiveObject();
        public bool FullCapacity { get; set; }
        public InteractableType Type { get; set; }

    }
    public enum InteractableType
    {
        Stockpile = 0, Generator = 1, Unlockable = 2
    }
    public enum GathererType
    {
        Player = 0, DeliveryPont = 1
    }
}