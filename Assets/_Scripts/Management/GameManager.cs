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
            //Switch to awaiting start; then use menu to start and switch to stack state
            ChangeState(GameState.StackState);
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
            _levelManager.OpenLevel();
        }
        private void DriveState()
        {

            CamFollowTarget = MainTruck.transform;
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


        public void AssignTruck(GameObject truckObject, int capacity) 
        {
            MainTruck = truckObject.GetComponent<BasicTruckControl>();
            CargoCapacity = capacity;
        }
        public void AssignPlayer(GameObject player)
        {
            Player = player;
            CamFollowTarget = Player.transform;
        }
        public void FullCapacity()
        {
            Debug.Log("CAPACTIY IS FULL");
        }

        public void AddPoint()
        {
            Debug.Log("point added");
            _levelManager.AddPoint();
            _uiManager.UpdateScore();
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