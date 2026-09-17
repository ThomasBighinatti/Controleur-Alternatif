using TMPro;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private string inputText;

    [SerializeField] private GameObject reactionGroup;
    [SerializeField] private TMP_Text reactionTextBox;
    [SerializeField] private PasswordGenerator passwordGenerator;

    public void GrabFromInputField(string input)
    {
        inputText = input;
        DisplayReactionToInput();
        CheckWordGuessed();
    }

    private void DisplayReactionToInput()
    {
        
        reactionTextBox.text = "Réponse entrée : " + inputText;
        reactionGroup.SetActive(true);
    }

    private void CheckWordGuessed()
    {
        if (passwordGenerator == null)
        {
            return;
        }
        
        if (string.Equals(inputText, passwordGenerator.MotActuel, System.StringComparison.OrdinalIgnoreCase))
        {
            passwordGenerator.GenerateRandomWord();
        }
    }
}