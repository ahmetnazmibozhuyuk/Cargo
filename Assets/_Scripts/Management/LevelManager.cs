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
        #endregion
        [SerializeField] private GameObject[] levelArray;

        [SerializeField] private int singleCargoPoint = 10;

        private GameObject _activeLevel;

        private void Awake()
        {
            InitializeKeys();
        }
        private void InitializeKeys()
        {

            if (!PlayerPrefs.HasKey(LEVEL))
            {
                PlayerPrefs.SetInt(LEVEL, 1); // Initialize CurrentLevel key if not already initialized
            }
            if (!PlayerPrefs.HasKey("CurrentPoints"))
            {
                PlayerPrefs.SetInt("CurrentPoints", 0); // Initialize CurrentPoints key if not already initialized.
            }
        }
        public void OpenLevel()
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
        #region Open Level
        private void OpenRandomLevel()
        {
            if (_activeLevel != null)
            {
                Destroy(_activeLevel);
                _activeLevel = null;
            }
            _activeLevel = Instantiate(levelArray[Random.Range(0, levelArray.Length)], Vector3.zero, Quaternion.identity);
        }
        private void OpenCurrentLevel()
        {
            if (_activeLevel != null)
            {
                Destroy(_activeLevel);
                _activeLevel = null;
            }
            _activeLevel = Instantiate(levelArray[PlayerPrefs.GetInt(LEVEL) - 1], Vector3.zero, Quaternion.identity);
        }
        #endregion

        public void NextLevel()
        {
            PlayerPrefs.SetInt(LEVEL, PlayerPrefs.GetInt(LEVEL) + 1); // Increase CurrentLevel index by one.
        }
        public void AddPoint()
        {
            PlayerPrefs.SetInt(SCORE, PlayerPrefs.GetInt(SCORE) + singleCargoPoint);
        }

        #region State Methods
        public void GameAwaitingStart()
        {

        }
        public void StackState()
        {

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
