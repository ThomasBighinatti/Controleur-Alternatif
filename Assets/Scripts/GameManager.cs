using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private char[,] _alphabetTable = new char[6, 5];
    private bool _alphabetTableReady;
    private bool _isGameLost;

    private void Awake()
    { 
        if (Instance != null)
        { 
            Destroy(this.gameObject); 
            return;
        }
        Instance = this; 
        DontDestroyOnLoad(this.gameObject);

        GenerateAlphabetTable();
    }
    
    public char[,] GetAlphabetTable()
    {
        if (!_alphabetTableReady)
        {
            GenerateAlphabetTable();
        }

        return _alphabetTable;
    }

    private void GenerateAlphabetTable()
    {
        const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int rows = _alphabetTable.GetLength(0);
        int cols = _alphabetTable.GetLength(1);
        int totalCases = rows * cols;
        
        List<char> lettres = new List<char>(alphabet.ToCharArray());
        while (lettres.Count < totalCases)
        {
            lettres.Add(alphabet[Random.Range(0, alphabet.Length)]);
        }
        
        //je mélange je mélange je mélange (kaaris)
        for (int i = lettres.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (lettres[i], lettres[j]) = (lettres[j], lettres[i]);
        }

        for (int i = 0; i < totalCases; i++)
        {
            int row = i / cols;
            int col = i % cols;
            _alphabetTable[row, col] = lettres[i];
        }

        _alphabetTableReady = true;
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
    public void QuitGame() => Application.Quit();

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

    public float _timer = 300f;

    public void UpdateTimer()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            _isGameLost = true;
            Debug.unityLogger.Log("Perdu ");
        }
    }
}