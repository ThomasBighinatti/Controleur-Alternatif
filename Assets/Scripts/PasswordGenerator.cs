using System.Collections.Generic;
using UnityEngine;

public enum ArrowDirection
{
    Up,
    Down,
    Left,
    Right
}

public class PasswordGenerator : MonoBehaviour
{
    
    private string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
     [SerializeField] private int _passwordLength;
     [SerializeField] private int _sentenceLength;

    private List<string> _nombresEnLettres = new List<string>
    {
        "UN", "DEUX", "TROIS", "QUATRE", "CINQ", "SIX", "SEPT", "HUIT", "NEUF", "DIX",
        "ONZE", "DOUZE", "TREIZE", "QUATORZE", "QUINZE", "SEIZE",
        "DIX-SEPT", "DIX-HUIT", "DIX-NEUF", "VINGT",
        "VINGT ET UN", "VINGT-DEUX", "VINGT-TROIS", "VINGT-QUATRE", "VINGT-CINQ",
        "VINGT-SIX", "VINGT-SEPT", "VINGT-HUIT", "VINGT-NEUF", "TRENTE", "TRENTE ET UN"
    };
    
    public string MotActuel { get; private set; }
    public string NombreActuel { get; private set; }
    public string MotCrypte { get; private set; }
    public string NombreCrypte { get; private set; }
    public List<ArrowDirection> FlechesActuelles { get; private set; }
    public int DecalageActuel { get; private set; }
    
    public event System.Action OnNewChallenge; //update l'ui

    private void Start()
    {
        GenerateNewChallenge();
        Debug.Log($"Mot à trouver : {MotActuel} | décalage : {DecalageActuel} | mot crypté : {MotCrypte} | nombre crypté : {NombreCrypte}");
    }


    public void GenerateNewChallenge(int nombreDeFleches = 5)
    {
        GenerateRandomWord();

        
        int index = Random.Range(0, 15);
        NombreActuel = _nombresEnLettres[index];
        DecalageActuel = index + 1;

        FlechesActuelles = GenerateArrows(nombreDeFleches);
        NombreCrypte = EncryptWithArrows(NombreActuel, FlechesActuelles);
        MotCrypte = EncryptCaesar(MotActuel, DecalageActuel);
        Debug.Log(MotActuel);
        OnNewChallenge?.Invoke();
    }

    private void GeneratePassword() //genere un mdp avec lettres uniquement
    {
        string password = "";
        for (int i = 0; i < _passwordLength; i++)
        {
            password += _alphabet[Random.Range(0, _alphabet.Length)];
        }
    }

    
    public string GenerateRandomWord()
    {
        List<string> randomWords = new List<string>
        {
            // 4 a 5 lettres
            "VELO", "JUGE", "KIWI", "TISSU", "LOUPE", "BANJO", "FOUET", "PLOMB", "YACHT", "GIVRE",

            // 6 a 7 lettres
            "CACTUS", "FRELON", "BUREAU", "DONJON", "MOUETTE", "CLAQUE", "POIREAU", "LUSTRE", "BRONZE", "TREFLE",
            "SOUFFLE", "JOCKEY", "WHISKY", "VIANDE", "BALCON",

            // 8 a 9 lettres
            "TABOURET", "CHOUETTE", "RADIATEUR", "BROCOLI", "KANGOUROU", "CUILLERE", "TRAMWAY", "AQUARIUM",
            "POCHETTE", "VENDREDI", "BOUCHON", "CROQUIS",

            // 10 lettres +
            "MONTGOLFIERE", "XYLOPHONE", "AQUARELLE", "TRACTOPELLE", "HIPPOPOTAME",
            "BOULEVERSE", "VENTILATEUR", "MARGUERITE", "QUADRILATERE", "SOUSTRACTIF"
        };
        MotActuel = randomWords[Random.Range(0, randomWords.Count)];
        return MotActuel;
    }

    public string GenerateRandomNumber() //genere un nombre en lettres
    {
        NombreActuel = _nombresEnLettres[Random.Range(0, _nombresEnLettres.Count)];
        return NombreActuel;
    }

    public string GenerateRandomNumberEncrypted()  //nombre en lettre crypté avec césar
    {
        string nombre = GenerateRandomNumber();
        int decalage = Random.Range(1, 32);
        return EncryptCaesar(nombre, decalage);
    }

    public string EncryptCaesar(string texte, int offset) //chiffrement césar 
    {
        string resultat = "";

        foreach (char c in texte.ToUpper())
        {
            int index = _alphabet.IndexOf(c);
            if (index == -1)
            {
                resultat += c;
                continue;
            }

            int newIndex = (index + offset) % _alphabet.Length;
            if (newIndex < 0)
            {
                newIndex += _alphabet.Length;
            }

            resultat += _alphabet[newIndex];
        }

        return resultat;
    }
    
    public List<ArrowDirection> GenerateArrows(int nombreDeFleches = 5)
    {
        var directionsPossibles = (ArrowDirection[])System.Enum.GetValues(typeof(ArrowDirection));
        var fleches = new List<ArrowDirection>();

        for (int i = 0; i < nombreDeFleches; i++)
        {
            fleches.Add(directionsPossibles[Random.Range(0, directionsPossibles.Length)]);
        }

        return fleches;
    }
    
    public (string motCrypte, List<ArrowDirection> fleches) GenerateRandomNumberEncryptedWithArrows(int nombreDeFleches = 5)
    {
        string nombre = GenerateRandomNumber();
        List<ArrowDirection> fleches = GenerateArrows(nombreDeFleches);
        return (EncryptWithArrows(nombre, fleches), fleches);
    }


    public string EncryptWithArrows(string texte, List<ArrowDirection> fleches)
    {
        if (GameManager.Instance == null)
        {
            return texte;
        }

        char[,] table = GameManager.Instance.GetAlphabetTable();
        int rows = table.GetLength(0);
        int cols = table.GetLength(1);

        string resultat = "";

        foreach (char lettre in texte.ToUpper())
        {
            if (lettre == ' ' || lettre == '-')
            {
                resultat += lettre; // garde les tirets
                continue;
            }

            (int row, int col) position = FindLetterPosition(table, lettre);

            foreach (ArrowDirection fleche in fleches)
            {
                position = ApplyArrow(position, fleche, rows, cols);
            }

            resultat += table[position.row, position.col];
        }

        return resultat;
    }

    private (int row, int col) FindLetterPosition(char[,] table, char lettre)
    {
        for (int row = 0; row < table.GetLength(0); row++)
        {
            for (int col = 0; col < table.GetLength(1); col++)
            {
                if (table[row, col] == lettre)
                {
                    return (row, col);
                }
            }
        }

        return (0, 0);
    }
    private (int row, int col) ApplyArrow((int row, int col) position, ArrowDirection direction, int rows, int cols)
    {
        do
        {
            switch (direction)
            {
                case ArrowDirection.Up:
                    position.row = (position.row - 1 + rows) % rows;
                    break;
                case ArrowDirection.Down:
                    position.row = (position.row + 1) % rows;
                    break;
                case ArrowDirection.Left:
                    position.col = (position.col - 1 + cols) % cols;
                    break;
                case ArrowDirection.Right:
                    position.col = (position.col + 1) % cols;
                    break;
            }
        } while (GameManager.IsCorner(position.row, position.col, rows, cols));

        return position;
    }
}