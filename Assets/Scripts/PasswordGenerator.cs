using System.Collections.Generic;
using UnityEngine;

public class PasswordGenerator : MonoBehaviour
{
    
    private string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
     [SerializeField] private int _passwordLength;
     [SerializeField] private int _sentenceLength;
    private void GeneratePassword()
    {
        string password = "";
        for (int i = 0; i < _passwordLength; i++)
        {
            password += _alphabet[Random.Range(0, _alphabet.Length)];
        }
    }

    
    private string GenerateRandomWord()
    {
        List<string> motsAleatoires = new List<string>
        {
            // 4 à 5 lettres
            "VELO", "JUGE", "KIWI", "TISSU", "LOUPE", "BANJO", "FOUET", "PLOMB", "YACHT", "GIVRE",

            // 6 à 7 lettres
            "CACTUS", "FRELON", "BUREAU", "DONJON", "MOUETTE", "CLAQUE", "POIREAU", "LUSTRE", "BRONZE", "TREFLE",
            "SOUFFLE", "JOCKEY", "WHISKY", "VIANDE", "BALCON",

            // 8 à 9 lettres
            "TABOURET", "CHOUETTE", "RADIATEUR", "BROCOLI", "KANGOUROU", "CUILLERE", "TRAMWAY", "AQUARIUM",
            "POCHETTE", "VENDREDI", "BOUCHON", "CROQUIS",

            // 10 lettres et plus
            "MONTGOLFIERE", "XYLOPHONE", "AQUARELLE", "TRACTOPELLE", "HIPPOPOTAME",
            "BOULEVERSE", "VENTILATEUR", "MARGUERITE", "QUADRILATERE", "SOUSTRACTIF"
        };
        return motsAleatoires[Random.Range(0, motsAleatoires.Count)];
    }
    
}
