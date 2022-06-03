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
        public Level ActiveLevel { get; private set; }
        public GameObject ActiveTruck { get; private set; }

        [SerializeField] private GameObject[] levelArray;
        [SerializeField] private GameObject[] TruckArray;

        [SerializeField] private int singleCargoPoint = 10;

        [SerializeField] private int initialCarryCapacity = 2;
        [SerializeField] private int firstCarryUpgradeCost = 80;
        [SerializeField] private int firstTruckUpgradeCost = 200;

        private int _previousRandomLevelIndex;
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
                PlayerPrefs.SetInt(BACKPACK_CAPACITY, initialCarryCapacity);
            }
            if (!PlayerPrefs.HasKey(CARRY_UPGRADE_COST))
            {
                PlayerPrefs.SetInt(CARRY_UPGRADE_COST, firstCarryUpgradeCost);
            }
            if (!PlayerPrefs.HasKey(TRUCK_UPGRADE_COST))
            {
                PlayerPrefs.SetInt(TRUCK_UPGRADE_COST, firstTruckUpgradeCost);
            }
        }
        public void ResetKeys() // The method called from ResetAllKeys button. 
        {
            PlayerPrefs.SetInt(LEVEL, 1);
            PlayerPrefs.SetInt(SCORE, 0);
            PlayerPrefs.SetInt(SELECTED_TRUCK_INDEX, 0);
            PlayerPrefs.SetInt(BACKPACK_CAPACITY, initialCarryCapacity);
            PlayerPrefs.SetInt(CARRY_UPGRADE_COST, firstCarryUpgradeCost);
            PlayerPrefs.SetInt(TRUCK_UPGRADE_COST, firstTruckUpgradeCost);
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            // Only instance of restarting the scene in the game. It is for testing and this method will not be in the build.
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
            // To prevent the same random level spawning twice.
            int randomLevelIndex = Random.Range(0, levelArray.Length);
            if(_previousRandomLevelIndex == randomLevelIndex)
            {
                if(randomLevelIndex >= levelArray.Length - 1)
                {
                    randomLevelIndex = 0;
                }
                else
                {
                    randomLevelIndex++;
                }
            }
            _previousRandomLevelIndex = randomLevelIndex;
            ActiveLevel = Instantiate(levelArray[randomLevelIndex], Vector3.zero, Quaternion.identity).GetComponent<Level>();
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

        #region Change Level
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
        #endregion

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
        //public void GameAwaitingStart()
        //{

        //}
        public void StackState()
        {
            OpenLevel();
            SpawnTruck();
        }
        //public void DriveState()
        //{

        //}
        //public void DeliverState()
        //{

        //}
        //public void GameWon()
        //{

        //}
        //public void GameLost()
        //{

        //}
        #endregion
    }
}
