using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cargo.Managers;

namespace Cargo.Interactable
{
    public class Stockpile : MonoBehaviour, IInteractable
    {
        public bool FullCapacity { get; set; }
        public InteractableType Type { get; set; }

        [SerializeField] private Transform stockTransform;

        private List<ObjectData> _objectDataList = new List<ObjectData>();

        private float _localX = 0, _localY = 0, _localZ = 1;

        [SerializeField] private int maxX = 4, maxZ = 2;

        private int _counter;

        private bool _inTheZone;

        private readonly float _gatherRate = 0.05f;

        private readonly float _cargoJumpPower = 4f;
        private void Awake()
        {
            Type = InteractableType.Stockpile;
        }
        public void InitializePositions()
        {
            for (int i = 0; i < GameManager.instance.CargoCapacity; i++)
            {
                _objectDataList.Add(new ObjectData(new Vector3(_localX + stockTransform.position.x,
                    _localY + stockTransform.position.y, _localZ + stockTransform.position.z)));

                if (_localZ > maxZ)
                {
                    _localZ = 0;
                    _localX++;
                }
                if (_localX > maxX)
                {
                    _localX = 0;
                    _localY++;
                }
                _localZ++;
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (_inTheZone) return;
            if (other.GetComponent<IInteractable>() == null) return;
            IInteractable interactable = other.GetComponent<IInteractable>();
            _inTheZone = true;
            if(interactable.Type == InteractableType.Unlockable)
            {
                StartCoroutine(Co_SendCubeTo(interactable));
            }
        }
        private void OnTriggerExit(Collider other)
        {
            _inTheZone = false;
        }

        private IEnumerator Co_SendCubeTo(IInteractable interactable)
        {
            while (_inTheZone)
            {
                if (interactable.FullCapacity)
                {
                    interactable.GiveObject();
                    yield return new WaitForSeconds(_gatherRate);
                }
                else
                {
                    interactable.TakeObject(GiveObject(), null);
                    yield return new WaitForSeconds(_gatherRate);
                }
            }
            yield break;
        }
        public void TakeObject(GameObject givenObj, Transform parent)
        {
            if (_counter < GameManager.instance.CargoCapacity)
            {
                if (givenObj == null) return;
                _objectDataList[_counter].ObjectHeld = givenObj;
                givenObj.transform.rotation = Quaternion.Euler(0, 0, 0);
                givenObj.transform.DOJump(_objectDataList[_counter].ObjectPosition, _cargoJumpPower, 1, 0.1f);
                givenObj.transform.SetParent(transform);
                _counter++;
                if (_counter >= GameManager.instance.CargoCapacity)
                {
                    FullCapacity = true;
                    GameManager.instance.TruckFullyLoaded();
                }
            }
        }
        public GameObject GiveObject()
        {
            FullCapacity = false;
            if (_counter <= 0)
            {
                if(GameManager.instance.CurrentState == GameState.DriveState)
                {
                    GameManager.instance.ChangeState(GameState.GameLost);
                    Debug.Log("game lost");
                }
                else if(GameManager.instance.CurrentState == GameState.DeliverState)
                {
                    GameManager.instance.ChangeState(GameState.GameWon);
                    Debug.Log("game won");
                }
                return null;
            }
            _counter--;
            Debug.Log("game manager artı puan methodu");
            GameObject temp = _objectDataList[_counter].ObjectHeld;
            _objectDataList[_counter].ObjectHeld = null;
            return temp;
        }
    }
}