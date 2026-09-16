using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    { 
        if (Instance != null)
        { 
            Destroy(transform.parent.gameObject); 
            return;
        }
        Instance = this; 
        DontDestroyOnLoad(transform.parent);
    }
    
    private string GetSceneByState()
    {
        return CurrentGameState switch
        {
            GameState.Game => "Game", 
            GameState.Menu => "Menu",
            _ => ""
        };
    }
    
    private bool _wasInPause;
    private GameState _currentGameState;
    
    [SerializeField] private GameState gameState;

    public enum GameState
    {
        Game,
        Menu
    }

    public void ChangeStateToGame() => CurrentGameState = GameState.Game;
    public void ChangeStateToMenu() => CurrentGameState = GameState.Menu;

    public GameState CurrentGameState
    {
        get => _currentGameState;
        set
        {
            Debug.Log(_currentGameState);
            _currentGameState = value;
            Debug.Log(_currentGameState);
            SceneManager.LoadScene(GetSceneByState());
            
        }
    }
}