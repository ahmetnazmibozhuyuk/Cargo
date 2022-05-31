using UnityEngine;

namespace Cargo.Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] levelArray;

        [SerializeField] private float singleCargoPoint = 10;

        private GameObject _activeLevel;
        public void OpenLevel()
        {
            if (!PlayerPrefs.HasKey("CurrentLevel"))
            {
                PlayerPrefs.SetInt("CurrentLevel", 1); // Initialize CurrentLevel key if not already initialized
            }

            if (PlayerPrefs.GetInt("CurrentLevel") <= levelArray.Length)
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
            _activeLevel = Instantiate(levelArray[PlayerPrefs.GetInt("CurrentLevel") - 1], Vector3.zero, Quaternion.identity);
        }
        #endregion

        public void NextLevel()
        {
            PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel") + 1); // Increase CurrentLevel index by one.
        }
        public void AddPoint()
        {

        }
    }
}
