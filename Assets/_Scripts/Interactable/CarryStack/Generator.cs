using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPooling;
using DG.Tweening;

namespace Cargo.Interactable
{
    public class Generator : MonoBehaviour, IInteractable
    {
        public bool FullCapacity { get; set; }
        public InteractableType Type { get; set; }

        [SerializeField] private GameObject objectToSpawn;

        [SerializeField] private int maxCapacity;

        [SerializeField] private float spawnRate;

        private List<ObjectData> _objectDataList = new List<ObjectData>();

        private int _counter;

        private float _localX = 1, _localY = 0, _localZ = 0;

        private void Awake()
        {
            Type = InteractableType.Generator;
        }
        private void Start()
        {
            InitializePositions();
            StartCoroutine(SpawnObject());
        }
        private void InitializePositions()
        {
            for (int i = 0; i < maxCapacity; i++)
            {
                _objectDataList.Add(new ObjectData(new Vector3(_localX + transform.position.x,
                    _localY + transform.position.y,
                    _localZ + transform.position.z)));

                if (_localX > 3)
                {
                    _localY++;
                    _localX = 0;
                }
                if (_localY > 1)
                {
                    _localZ--;
                    _localY = 0;
                }
                _localX++;
            }
        }
        private IEnumerator SpawnObject() // might want to consider using a simple timer on update rather than an IEnumerator
        {
            if (_counter < maxCapacity)
            {
                var temp = ObjectPool.Spawn(objectToSpawn, transform.position, transform.rotation);
                _objectDataList[_counter].ObjectHeld = temp;
                _objectDataList[_counter].ObjectHeld.transform.position = _objectDataList[_counter].ObjectPosition;
                temp.transform.DOScale(1, 0.5f).SetEase(Ease.OutBounce);
                temp.transform.SetParent(transform);
                _counter++;
                yield return new WaitForSeconds(spawnRate);
                StartCoroutine(SpawnObject());
            }
            else
            {
                yield return new WaitForSeconds(spawnRate);
                StartCoroutine(SpawnObject());
            }
        }
        public void TakeObject(GameObject givenObj, Transform parent)
        {
            Debug.Log("An object is given to the generator.");
        }
        public GameObject GiveObject()
        {
            if (_counter <= 0)
            {
                return null;
            }
            _counter--;
            GameObject temp = _objectDataList[_counter].ObjectHeld;
            _objectDataList[_counter].ObjectHeld = null;
            return temp;
        }
    }
}