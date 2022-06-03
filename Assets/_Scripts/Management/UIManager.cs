using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Cargo.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI endGameScoreText;
        [SerializeField] private TextMeshProUGUI carryUpgradeCostText;
        [SerializeField] private TextMeshProUGUI truckUpgradeCostText;
        [SerializeField] private TextMeshProUGUI capacityText;
        [SerializeField] private TextMeshProUGUI levelText;

        [SerializeField] private GameObject inGamePanel;
        [SerializeField] private GameObject gameWonPanel;
        [SerializeField] private GameObject gameLostPanel;
        [SerializeField] private GameObject shopPanel;

        [SerializeField] private Button truckUpgradeButton;
        [SerializeField] private Button carryUpgradeButton;

        [SerializeField] private int truckUpgradeCostScale = 4000;
        [SerializeField] private int carryUpgradeCostScale = 500;

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
            ShopButtonActivation();
            levelText.SetText("Level: "+PlayerPrefs.GetInt(LevelManager.LEVEL).ToString());
        }
        public void StackState()
        {
            shopPanel.SetActive(false);
            inGamePanel.SetActive(true);
        }
        //public void DriveState()
        //{

        //}
        //public void DeliverState()
        //{

        //}
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
            scoreText.SetText("$: " + PlayerPrefs.GetInt(LevelManager.SCORE));
            endGameScoreText.SetText("$: " + PlayerPrefs.GetInt(LevelManager.SCORE));
        }

        public void UpdateCapacityCounter(string capacity)
        {
            capacityText.SetText(capacity);
        }

        #region Upgrade System
        public void UpgradeTruck()
        {
            PlayerPrefs.SetInt(LevelManager.SELECTED_TRUCK_INDEX, PlayerPrefs.GetInt(LevelManager.SELECTED_TRUCK_INDEX) + 1); // Upgrade Truck

            PlayerPrefs.SetInt(LevelManager.SCORE, PlayerPrefs.GetInt(LevelManager.SCORE) - PlayerPrefs.GetInt(LevelManager.TRUCK_UPGRADE_COST)); // Set Remaining Money
            PlayerPrefs.SetInt(LevelManager.TRUCK_UPGRADE_COST, PlayerPrefs.GetInt(LevelManager.TRUCK_UPGRADE_COST)+truckUpgradeCostScale);     //Revaluate the Next Upgrade

           ShopButtonActivation(); // Check if the player has enough money for the next upgrade; disable the button if not
        }
        public void UpgradeCarryCapacity()
        {
            PlayerPrefs.SetInt(LevelManager.BACKPACK_CAPACITY, PlayerPrefs.GetInt(LevelManager.BACKPACK_CAPACITY) + 1); // Upgrade carry capacity

            PlayerPrefs.SetInt(LevelManager.SCORE, PlayerPrefs.GetInt(LevelManager.SCORE) - PlayerPrefs.GetInt(LevelManager.CARRY_UPGRADE_COST)); // Set Remaining Money
            PlayerPrefs.SetInt(LevelManager.CARRY_UPGRADE_COST, PlayerPrefs.GetInt(LevelManager.CARRY_UPGRADE_COST) + carryUpgradeCostScale);       //Revaluate the Next Upgrade

            ShopButtonActivation(); // Check if the player has enough money for the next upgrade; disable the button if not
        }
        private void ShopButtonActivation()
        {
            carryUpgradeCostText.SetText(PlayerPrefs.GetInt(LevelManager.CARRY_UPGRADE_COST)+"$");
            truckUpgradeCostText.SetText(PlayerPrefs.GetInt(LevelManager.TRUCK_UPGRADE_COST) + "$");
            UpdateScore();

            if (PlayerPrefs.GetInt(LevelManager.TRUCK_UPGRADE_COST) > PlayerPrefs.GetInt(LevelManager.SCORE))
            {
                truckUpgradeButton.interactable = false;
            }
            else
            {
                truckUpgradeButton.interactable = true;
            }

            if (PlayerPrefs.GetInt(LevelManager.CARRY_UPGRADE_COST) > PlayerPrefs.GetInt(LevelManager.SCORE))
            {
                carryUpgradeButton.interactable = false;
            }
            else
            {
                carryUpgradeButton.interactable = true;
            }
        }
        #endregion
    }
}
