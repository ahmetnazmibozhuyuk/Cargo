using UnityEngine;
using TMPro;

namespace Cargo.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;

        [SerializeField] private GameObject inGamePanel;
        [SerializeField] private GameObject gameWonPanel;
        [SerializeField] private GameObject gameLostPanel;
        [SerializeField] private GameObject shopPanel;

        private void Start()
        {
            InitializeGame();
        }
        private void InitializeGame()
        {
            UpdateScore();
        }
        
        #region State Methods
        public void GameAwaitingStart()
        {
            inGamePanel.SetActive(false);
            gameWonPanel.SetActive(false);
            gameLostPanel.SetActive(false);

            shopPanel.SetActive(true);
        }
        public void StackState()
        {
            shopPanel.SetActive(false);
            inGamePanel.SetActive(true);
        }
        public void DriveState()
        {

        }
        public void DeliverState()
        {

        }
        public void GameWon()
        {
            inGamePanel.SetActive(false);
            gameWonPanel.SetActive(true);
        }
        public void GameLost()
        {
            inGamePanel.SetActive(false);
            gameLostPanel.SetActive(true);
        }

        #endregion

        public void UpdateScore()
        {
            scoreText.SetText("Score: " + PlayerPrefs.GetInt(LevelManager.SCORE));
        }
    }
}
