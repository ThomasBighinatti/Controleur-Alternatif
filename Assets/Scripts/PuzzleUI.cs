using System.Text;
using TMPro;
using UnityEngine;

public class PuzzleUI : MonoBehaviour
{
    [SerializeField] private PasswordGenerator passwordGenerator;
    [SerializeField] private TMP_Text grilleText;
    [SerializeField] private TMP_Text motCrypteText;
    [SerializeField] private TMP_Text nombreCrypteText;
    [SerializeField] private TMP_Text flechesText;

    private void OnEnable()
    {
        if (passwordGenerator != null)
        {
            passwordGenerator.OnNewChallenge += RefreshDisplay;
        }
    }

    private void OnDisable()
    {
        if (passwordGenerator != null)
        {
            passwordGenerator.OnNewChallenge -= RefreshDisplay;
        }
    }

    private void Start()
    {
        RefreshDisplay();
    }
    
    public void RefreshDisplay()
    {
        if (passwordGenerator == null)
        {
            return;
        }

        if (motCrypteText != null)
        {
            motCrypteText.text = passwordGenerator.MotCrypte;
        }

        if (nombreCrypteText != null)
        {
            nombreCrypteText.text = passwordGenerator.NombreCrypte;
        }

        if (flechesText != null)
        {
            flechesText.text = FormatFleches(passwordGenerator.FlechesActuelles);
        }

        if (grilleText != null && GameManager.Instance != null)
        {
            grilleText.text = FormatGrid(GameManager.Instance.GetAlphabetTable());
        }
    }

    //inverse des flèches utilisée pour le chiffrement
    private string FormatFleches(System.Collections.Generic.List<ArrowDirection> fleches)
    {
        if (fleches == null)
        {
            return "";
        }

        var texte = new StringBuilder();

        foreach (ArrowDirection fleche in fleches)
        {
            texte.Append(InverserFleche(fleche) switch
            {
                ArrowDirection.Up => "↑",
                ArrowDirection.Down => "↓",
                ArrowDirection.Left => "←",
                ArrowDirection.Right => "→",
                _ => "?"
            });
            texte.Append(' ');
        }

        return texte.ToString().TrimEnd();
    }

    private ArrowDirection InverserFleche(ArrowDirection fleche)
    {
        return fleche switch
        {
            ArrowDirection.Up => ArrowDirection.Down,
            ArrowDirection.Down => ArrowDirection.Up,
            ArrowDirection.Left => ArrowDirection.Right,
            ArrowDirection.Right => ArrowDirection.Left,
            _ => fleche
        };
    }

    private string FormatGrid(char[,] table)
    {
        int rows = table.GetLength(0);
        int cols = table.GetLength(1);
        var texte = new StringBuilder();

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                texte.Append(table[row, col]);
                if (col < cols - 1)
                {
                    texte.Append("  "); //pour mieux lire
                }
            }

            if (row < rows - 1)
            {
                texte.Append('\n');
            }
        }

        return texte.ToString();
    }
}