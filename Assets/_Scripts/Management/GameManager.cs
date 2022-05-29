using UnityEngine;
//using ObjectPooling;

namespace Template.Managers
{

    //yeni birşey eklemen gerektiğinde (mesela sabit duran kamyona controller vs
    //dependency injection kullanmayı dene, düzgün instantiate veya addcomponent yap
    //burada get; private set bir variable'a at ve o variable'da init methodunu kullan)


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
        public void ChangeState(GameState newState)
        {
            if (CurrentState == newState) return;

            CurrentState = newState;
            switch (newState)
            {
                case GameState.GameAwaitingStart:
                    GameAwaitingStartState();
                    break;
                case GameState.GameStarted:
                    GameStartedState();
                    break;
                case GameState.GameCheckingResults:
                    GameCheckingResultsState();
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
        private void GameStartedState()
        {
            _uiManager.GameStarted();
        }
        private void GameCheckingResultsState()
        {

        }
        private void GameWonState()
        {
            _uiManager.GameWon();

        }
        private void GameLostState()
        {
            _uiManager.GameLost();
        }
        private void StopMovement()
        {

        }
    }
    public enum GameState
    {
        GamePreStart = 0,
        GameAwaitingStart = 1,
        GameStarted = 2,
        GameCheckingResults = 3,
        GameWon = 4,
        GameLost = 5,
    }
}