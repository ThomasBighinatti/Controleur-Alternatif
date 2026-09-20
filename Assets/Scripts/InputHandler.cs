using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private string inputText;

    [SerializeField] private GameObject reactionGroup;
    [SerializeField] private TMP_Text reactionTextBox;
    [SerializeField] private PasswordGenerator passwordGenerator;

    private TMP_InputField _inputField;

    private void Awake()
    {
        _inputField = GetComponent<TMP_InputField>();
    }

    public void GrabFromInputField(string input)
    {
        if (GameManager.Instance != null && GameManager.Instance.PartieTerminee)
        {
            return; 
        }

        inputText = input;
        bool correct = CheckWordGuessed();
        DisplayReactionToInput(correct);

        if (correct)
        {
            passwordGenerator.GenerateNewChallenge(); //relance avec nouvelles fleches et nouveau mot mais meme grille

            if (_inputField != null)
            {
                _inputField.text = "";
                _inputField.ActivateInputField();
                EventSystem.current.SetSelectedGameObject(gameObject);
            }
        }
        else if (GameManager.Instance != null)
        {
            GameManager.Instance.LoseTime(20f); //-20s sur timer
        }
    }

    private void DisplayReactionToInput(bool correct)
    {
        reactionTextBox.text = correct
            ? $"Bravo, la réponse était \"{passwordGenerator.MotActuel}\" ."
            : $"Ce n'est pas : {inputText} — dommage. (-20s)";
        //reactionGroup.SetActive(true);
    }

    private bool CheckWordGuessed()
    {
        if (passwordGenerator == null)
        {
            return false;
        }

        return string.Equals(inputText, passwordGenerator.MotActuel, System.StringComparison.OrdinalIgnoreCase);
    }
}