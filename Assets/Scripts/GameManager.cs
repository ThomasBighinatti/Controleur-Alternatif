using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<List<char>> AlphabetTable =  new List<List<char>>();

    private void Awake()
    { 
        if (Instance != null)
        { 
            Destroy(this.gameObject); 
            return;
        }
        Instance = this; 
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        GenerateAlphabetTable();
        print(AlphabetTable);
    }

    private void GenerateAlphabetTable()
    {
        const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        for (int offset = 0; offset < alphabet.Length; offset++)
        {
            List<char> ligne = new List<char>();
            for (int i = 0; i < alphabet.Length; i++)
            {
                int index = (i + offset) % alphabet.Length;
                ligne.Add(alphabet[index]);
            }
            AlphabetTable.Add(ligne);
        }
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