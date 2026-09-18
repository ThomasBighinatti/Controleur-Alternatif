using System.Text;
using TMPro;
using UnityEngine;

public class PuzzleUI : MonoBehaviour
{
    [SerializeField] private TMP_Text grilleText;
    [SerializeField] private TMP_Text motCrypteText;
    [SerializeField] private TMP_Text nombreCrypteText;
    [SerializeField] private TMP_Text flechesText;
    [SerializeField] private TMP_Text timerText;
    
    private bool _isSubscribed;

    private void Start()
    {
        SubscribeToChallenge();
        RefreshDisplay();
    }

    private void OnEnable()
    {
        SubscribeToChallenge();
        RefreshDisplay();
    }

    private void OnDisable()
    {
        UnsubscribeFromChallenge();
    }

    private void SubscribeToChallenge()
    {
        if (!_isSubscribed && PasswordGenerator.Instance != null)
        {
            PasswordGenerator.Instance.OnNewChallenge += RefreshDisplay;
            _isSubscribed = true;
        }
    }

    private void UnsubscribeFromChallenge()
    {
        if (_isSubscribed && PasswordGenerator.Instance != null)
        {
            PasswordGenerator.Instance.OnNewChallenge -= RefreshDisplay;
            _isSubscribed = false;
        }
    }

    private void Update()
    {
        Updatetimer();
    }

    private void Updatetimer()
    {
        //timerText.text = GameManager.Instance._timer.ToString();
    }
    
    public void RefreshDisplay()
    {
        if (PasswordGenerator.Instance == null)
        {
            return;
        }

        if (motCrypteText != null)
        {
            motCrypteText.text = PasswordGenerator.Instance.MotCrypte;
        }

        if (nombreCrypteText != null)
        {
            nombreCrypteText.text = PasswordGenerator.Instance.NombreCrypte;
        }

        if (flechesText != null)
        {
            flechesText.text = FormatFleches(PasswordGenerator.Instance.FlechesActuelles);
        }

        if (grilleText != null && GameManager.Instance != null)
        {
            grilleText.text = FormatGrid(GameManager.Instance.GetAlphabetTable());
        }
    }

    // inverse des flèches utilisée pour le chiffrement
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
                    texte.Append("  "); // pour mieux lire
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