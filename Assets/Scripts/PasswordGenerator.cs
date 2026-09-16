using System.Collections.Generic;
using UnityEngine;

public class PasswordGenerator : MonoBehaviour
{
    
    private string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
     [SerializeField] private int _passwordLength;
     [SerializeField] private int _sentenceLength;

    // Le mot que le joueur doit deviner actuellement
    public string MotActuel { get; private set; }

    private void Start()
    {
        GenerateRandomWord();
    }

    private void GeneratePassword()
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
            // 4 à 5 lettres
            "VELO", "JUGE", "KIWI", "TISSU", "LOUPE", "BANJO", "FOUET", "PLOMB", "YACHT", "GIVRE",

            // 6 à 7 lettres
            "CACTUS", "FRELON", "BUREAU", "DONJON", "MOUETTE", "CLAQUE", "POIREAU", "LUSTRE", "BRONZE", "TREFLE",
            "SOUFFLE", "JOCKEY", "WHISKY", "VIANDE", "BALCON",

            // 8 à 9 lettres
            "TABOURET", "CHOUETTE", "RADIATEUR", "BROCOLI", "KANGOUROU", "CUILLERE", "TRAMWAY", "AQUARIUM",
            "POCHETTE", "VENDREDI", "BOUCHON", "CROQUIS",

            // 10 lettres +
            "MONTGOLFIERE", "XYLOPHONE", "AQUARELLE", "TRACTOPELLE", "HIPPOPOTAME",
            "BOULEVERSE", "VENTILATEUR", "MARGUERITE", "QUADRILATERE", "SOUSTRACTIF"
        };
        MotActuel = randomWords[Random.Range(0, randomWords.Count)];
        return MotActuel;
    }
    
    public string EncryptCaesar(string texte, int offset)
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