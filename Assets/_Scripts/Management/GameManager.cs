using UnityEngine;
//using ObjectPooling;

namespace Cargo.Managers
{
    [RequireComponent(typeof(UIManager), typeof(LevelManager))]
    public class GameManager : Singleton<GameManager>
    {
        public GameState CurrentState { get; private set; }

        public GameObject Player
        {
            get { return player; }
            set { player = value; }
        }
        [SerializeField] private GameObject player;

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
            ChangeState(GameState.GameAwaitingStart);
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
                    DeliverState();
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
            _uiManager.StackState();
        }
        private void DriveState()
        {
            _uiManager.DriveState();
        }
        private void DeliverState()
        {
            _uiManager.DeliverState();
        }

        private void GameWonState()
        {
            _uiManager.GameWon();

        }
        private void GameLostState()
        {
            _uiManager.GameLost();
        }
        #endregion


        public void InitializeCargoCapacity(int capacity) // should be called from the truck script 
        {
            CargoCapacity = capacity;
        }
        public void FullCapacity()
        {
            Debug.Log("CAPACTIY IS FULL");
        }
        public void TruckFullyLoaded()
        {
            Debug.Log("truck is fully loaded");
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