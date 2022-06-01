using PathCreation;
using UnityEngine;

namespace Cargo.Managers
{
    // todo : Instantiate the truck in the select phase; assign the road appropriately. Truck could be a separate 
    // prefab or you could make a data which holds ScriptibleObject and mesh of truck


    public class LevelManager : MonoBehaviour
    {
        #region Constant Keys
        public const string LEVEL = "level";
        public const string SCORE = "score";
        public const string SELECTED_TRUCK = "selectedTruck";
        #endregion

        [SerializeField] private GameObject[] levelArray;
        [SerializeField] private GameObject[] TruckArray;

        [SerializeField] private int singleCargoPoint = 10;


        public Level ActiveLevel { get; private set; }

        public GameObject ActiveTruck { get; private set; }
        

        private void Awake()
        {
            InitializeKeys();
        }
        private void InitializeKeys()
        {

            if (!PlayerPrefs.HasKey(LEVEL))
            {
                PlayerPrefs.SetInt(LEVEL, 1);
            }
            if (!PlayerPrefs.HasKey(SCORE))
            {
                PlayerPrefs.SetInt(SCORE, 0);
            }
            if (!PlayerPrefs.HasKey(SELECTED_TRUCK))
            {
                PlayerPrefs.SetInt(SELECTED_TRUCK, 0); 
            }
        }

        #region Open Level
        private void OpenLevel()
        {
            if (PlayerPrefs.GetInt(LEVEL) <= levelArray.Length)
            {
                OpenCurrentLevel();
            }
            else
            {
                OpenRandomLevel(); // Open a random level from the list if the current level value is more than existing levels.
            }
        }
        private void OpenRandomLevel()
        {
            RemoveLevel();
            ActiveLevel = Instantiate(levelArray[Random.Range(0, levelArray.Length)], Vector3.zero, Quaternion.identity).GetComponent<Level>();
        }
        private void OpenCurrentLevel()
        {
            RemoveLevel();
            ActiveLevel = Instantiate(levelArray[PlayerPrefs.GetInt(LEVEL) - 1], Vector3.zero, Quaternion.identity).GetComponent<Level>();
        }
        private void RemoveLevel()
        {
            if (ActiveLevel != null)
            {
                Destroy(ActiveLevel.gameObject);
                ActiveLevel = null;
            }
        }
        #endregion

        public void StartLevel()
        {
            GameManager.instance.ChangeState(GameState.StackState);
        }
        public void NextLevel()
        {
            PlayerPrefs.SetInt(LEVEL, PlayerPrefs.GetInt(LEVEL) + 1); // Increase CurrentLevel index by one.
            GameManager.instance.ChangeState(GameState.GameAwaitingStart);
        }
        public void RestartLevel()
        {
            GameManager.instance.ChangeState(GameState.GameAwaitingStart);
        }
        public void AddPoint()
        {
            PlayerPrefs.SetInt(SCORE, PlayerPrefs.GetInt(SCORE) + singleCargoPoint);
        }

        private void SpawnTruck()
        {
            if (ActiveTruck != null) 
            {
                Destroy(ActiveTruck);
                ActiveTruck = null;
            } 
            ActiveTruck = Instantiate(TruckArray[PlayerPrefs.GetInt(SELECTED_TRUCK)], 
                ActiveLevel.TruckPath.path.GetPointAtDistance(0, EndOfPathInstruction.Stop), 
                Quaternion.identity);
        }
        public void SelectTruck0()
        {
            PlayerPrefs.SetInt(SELECTED_TRUCK, 0);
        }
        public void SelectTruck1()
        {
            PlayerPrefs.SetInt(SELECTED_TRUCK, 1);
        }

        #region State Methods
        public void GameAwaitingStart()
        {

        }
        public void StackState()
        {
            OpenLevel();
            SpawnTruck();
        }
        public void DriveState()
        {

        }
        public void DeliverState()
        {

        }
        public void GameWon()
        {

        }
        public void GameLost()
        {

        }
        #endregion
    }
}
