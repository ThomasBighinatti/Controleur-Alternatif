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
        bool correct = CheckWordGuessed();
        DisplayReactionToInput(correct);

        if (correct)
        {
            passwordGenerator.GenerateNewChallenge(); //relance avec nouvelles fleches et nouveau mot mais meme grille
            //TODO
        }
    }

    private void DisplayReactionToInput(bool correct)
    {
        reactionTextBox.text = correct
            ? $"Bravo, la réponse était \"{passwordGenerator.MotActuel}\" ."
            : $"Ce n'est pas : {inputText} — dommage."; //-20s sur timer
        reactionGroup.SetActive(true);
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