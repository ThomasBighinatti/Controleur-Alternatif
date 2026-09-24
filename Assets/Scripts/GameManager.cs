using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO.Ports;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private char[,] _alphabetTable = new char[6, 5];
    private bool _alphabetTableReady;

    [Header("Levier (port série)")]
    [SerializeField] private string portSerie = "COM3";
    [SerializeField] private int baudRate = 115200;
    private SerialPort _sp;
    
    public int LeverValue { get; private set; }

    [SerializeField] private float dureeInitiale = 120f; // en secondes
    
    
    public float TimeLeft { get; private set; }
    public bool PartieTerminee { get; private set; }

    public event System.Action OnTempsEcoule;

    public int victoires = 0;

    private void Awake()
    { 
        if (Instance != null)
        { 
            Destroy(gameObject); 
            return;
        }
        Instance = this; 
        DontDestroyOnLoad(gameObject);

        GenerateAlphabetTable();
        TimeLeft = dureeInitiale;
    }

    private void Start()
    {
        try
        {
            _sp = new SerialPort(portSerie, baudRate);
            _sp.ReadTimeout = 50; // court, pour ne jamais bloquer une frame
            _sp.Open();
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Port série ({portSerie}) indisponible, les leviers seront ignorés : {e.Message}");
            _sp = null;
        }
    }

    private void Update()
    {
        LireLevier();

        if (CurrentGameState != GameState.Game || PartieTerminee)
        {
            return;
        }

        TimeLeft -= Time.deltaTime;

        if (TimeLeft <= 0f)
        {
            TimeLeft = 0f;
            EndGame();
        }
        
    }
    private void LireLevier()
    {
        if (_sp == null || !_sp.IsOpen)
        {
            return;
        }

        try
        {
            string ligne = _sp.ReadLine();
            if (int.TryParse(ligne.Trim(), out int valeur))
            {
                LeverValue = Mathf.Clamp(valeur, 0, 15);
            }
        }
        catch (System.TimeoutException)
        {
            // rien de nouveau ce frame, on garde LeverValue tel quel
        }
    }

    private void OnApplicationQuit()
    {
        if (_sp != null && _sp.IsOpen)
        {
            _sp.Close();
        }
    }

    public void LoseTime(float secondes)
    {
        if (PartieTerminee)
        {
            return;
        }

        TimeLeft = Mathf.Max(0f, TimeLeft - secondes);

        if (TimeLeft <= 0f)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        PartieTerminee = true;
        OnTempsEcoule?.Invoke();
    }

    public char[,] GetAlphabetTable()
    {
        if (!_alphabetTableReady)
        {
            GenerateAlphabetTable();
        }

        return _alphabetTable;
    }

    public static bool IsCorner(int row, int col, int rows, int cols)
    {
        return (row == 0 || row == rows - 1) && (col == 0 || col == cols - 1);
    }

    private const char CaseVide = '•'; 

    private void GenerateAlphabetTable()
    {
        const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; 
        int rows = _alphabetTable.GetLength(0);
        int cols = _alphabetTable.GetLength(1);

        List<char> lettres = new List<char>(alphabet.ToCharArray());

        //je mélange je mélange je mélange (kaaris)
        for (int i = lettres.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (lettres[i], lettres[j]) = (lettres[j], lettres[i]);
        }
        
        int index = 0;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (IsCorner(row, col, rows, cols))
                {
                    _alphabetTable[row, col] = CaseVide;
                    continue;
                }

                _alphabetTable[row, col] = lettres[index];
                index++;
            }
        }

        _alphabetTableReady = true;
    }
    
    private string GetSceneByState()
    {
        return CurrentGameState switch
        {
            GameState.Game => "Game", 
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
    
    public void Replay() => ChangeStateToGame();
    
    public void GoToMainMenu() => ChangeStateToMenu();

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public GameState CurrentGameState
    {
        get => _currentGameState;
        set
        {
            Debug.Log(_currentGameState);
            _currentGameState = value;
            Debug.Log(_currentGameState);

            if (value == GameState.Game)
            {
                TimeLeft = dureeInitiale;
                PartieTerminee = false;
            }

            SceneManager.LoadScene(GetSceneByState());
        }
    }
}