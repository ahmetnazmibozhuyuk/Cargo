using UnityEngine;
using Cargo.Control;

namespace Cargo.Managers
{
    [RequireComponent(typeof(UIManager), typeof(LevelManager))]
    public class GameManager : Singleton<GameManager>
    {
        public GameState CurrentState { get; private set; }

        public GameObject Player { get; private set; }

        public BasicTruckControl MainTruck { get; private set; }
        public Transform CamFollowTarget { get; private set; }

        public int CargoCapacity { get; private set; }

        private UIManager _uiManager;
        private LevelManager _levelManager;

        protected override void Awake()
        {
            base.Awake();
            _uiManager = GetComponent<UIManager>();
            _levelManager = GetComponent<LevelManager>();
        }
        private void Start()
        {
            if(PlayerPrefs.GetInt(LevelManager.LEVEL) > 1)
            {
                ChangeState(GameState.GameAwaitingStart); 
            }
            else
            {
                ChangeState(GameState.StackState); // The first level automatically starts without asking if the player wants to upgrade.
            }
        }
        #region Game States
        public void ChangeState(GameState newState)
        {
            if (CurrentState == newState) return;

            CurrentState = newState;
            switch (newState)
            {
                case GameState.GameAwaitingStart:
                    GameAwaitingStartState();
                    break;
                case GameState.StackState:
                    StackState();
                    break;
                case GameState.DriveState:
                    DriveState();
                    break;
                case GameState.DeliverState:
                    //DeliverState();
                    break;
                case GameState.GameWon:
                    GameWonState();
                    break;
                case GameState.GameLost:
                    GameLostState();
                    break;
                default:
                    throw new System.ArgumentException("Invalid game state selection.");
            }
        }
        private void GameAwaitingStartState()
        {
            _uiManager.GameAwaitingStart();
        }
        private void StackState()
        {
            _levelManager.StackState();
            _uiManager.StackState();
        }
        private void DriveState()
        {
            CamFollowTarget = MainTruck.transform;
        }
        //private void DeliverState()
        //{

        //}

        private void GameWonState()
        {
            _uiManager.GameWon();

        }
        private void GameLostState()
        {
            _uiManager.GameLost();
        }
        #endregion

        public void AssignTruck(GameObject truckObject, int capacity) 
        {
            MainTruck = truckObject.GetComponent<BasicTruckControl>();
            CargoCapacity = capacity;
            MainTruck.TrackPathCreator = _levelManager.ActiveLevel.TruckPath;
            UpdateCapacityCounter("0"+ " / " + capacity);
        }
        public void AssignPlayer(GameObject player)
        {
            Player = player;
            CamFollowTarget = Player.transform;
        }
        public void AddPoint()
        {
            _levelManager.AddPoint();
            _uiManager.UpdateScore();
        }
        public void UpdateCapacityCounter(string capacity)
        {
            _uiManager.UpdateCapacityCounter(capacity);
        }
    }
    public enum GameState
    {
        GamePreStart = 0,
        GameAwaitingStart = 1,
        StackState = 2,
        DriveState = 3,
        DeliverState = 4,
        GameWon = 5,
        GameLost = 6,
    }
}