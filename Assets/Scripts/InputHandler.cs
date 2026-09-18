using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private string inputText;

    [SerializeField] private GameObject reactionGroup;
    [SerializeField] private TMP_Text reactionTextBox;
    [SerializeField] private PasswordGenerator passwordGenerator;

    TMP_InputField inputfield;
    private void Awake()
    {
        inputfield = GetComponent<TMP_InputField>();
    }
    

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
        EventSystem.current.SetSelectedGameObject(gameObject);
        
        inputfield.text = "";
        inputfield.ActivateInputField();
        
    }
    
    
    private void DisplayReactionToInput(bool correct)
    {
        reactionTextBox.text = correct
            ? $"Bravo, la réponse était \"{passwordGenerator.MotActuel}\" ."
            : $"Ce n'est pas : {inputText} — dommage."; //-20s sur timer
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