using PathCreation;
using UnityEngine;

namespace Cargo.Managers
{
    public class LevelManager : MonoBehaviour
    {
        #region Constant Keys
        public const string LEVEL = "level";
        public const string SCORE = "score";
        public const string SELECTED_TRUCK_INDEX = "selectedTruckIndex";
        public const string BACKPACK_CAPACITY = "backpackCapacity";
        public const string TRUCK_UPGRADE_COST = "truckUpgradeCost";
        public const string CARRY_UPGRADE_COST = "carryUpgradeCost";
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
            if (!PlayerPrefs.HasKey(SELECTED_TRUCK_INDEX))
            {
                PlayerPrefs.SetInt(SELECTED_TRUCK_INDEX, 0); 
            }
            if (!PlayerPrefs.HasKey(BACKPACK_CAPACITY))
            {
                PlayerPrefs.SetInt(BACKPACK_CAPACITY, 2);
            }
            if (!PlayerPrefs.HasKey(TRUCK_UPGRADE_COST))
            {
                PlayerPrefs.SetInt(TRUCK_UPGRADE_COST, 100);
            }
            if (!PlayerPrefs.HasKey(CARRY_UPGRADE_COST))
            {
                PlayerPrefs.SetInt(CARRY_UPGRADE_COST, 300);
            }
        }
        public void ResetKeys()
        {
            PlayerPrefs.SetInt(LEVEL, 1);
            PlayerPrefs.SetInt(SCORE, 0);
            PlayerPrefs.SetInt(SELECTED_TRUCK_INDEX, 0);
            PlayerPrefs.SetInt(BACKPACK_CAPACITY, 2);
            PlayerPrefs.SetInt(CARRY_UPGRADE_COST, 100);
            PlayerPrefs.SetInt(TRUCK_UPGRADE_COST, 300);
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
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
            GameManager.instance.ChangeState(GameState.StackState); // called from start game button
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
            if(PlayerPrefs.GetInt(SELECTED_TRUCK_INDEX) < TruckArray.Length)
            {
                ActiveTruck = Instantiate(TruckArray[PlayerPrefs.GetInt(SELECTED_TRUCK_INDEX)],
    ActiveLevel.TruckPath.path.GetPointAtDistance(0, EndOfPathInstruction.Stop),
    Quaternion.identity);
            }
            else
            {
                ActiveTruck = Instantiate(TruckArray[TruckArray.Length-1], //if selected trunk index is greater than the length of array; select the last truck
ActiveLevel.TruckPath.path.GetPointAtDistance(0, EndOfPathInstruction.Stop),
Quaternion.identity);
            }

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
