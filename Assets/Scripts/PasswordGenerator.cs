using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        GenerateRandomWord();
        Debug.Log(MotActuel);
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

    
}