using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private string inputText;

    [SerializeField] private GameObject reactionGroup;
    [SerializeField] private TMP_Text reactionTextBox;
    [SerializeField] private PasswordGenerator passwordGenerator;

    [Header("Feedback mauvaise réponse")]
    [SerializeField] private Image inputFieldBackground; 
    [SerializeField] private float shakeDuration = 0.25f;
    [SerializeField] private float shakeStrength = 6f;
    [SerializeField] private Color couleurErreur = Color.red;
    [SerializeField] private float dureeMonteeRouge = 0.1f;
    [SerializeField] private float dureeRetourBlanc = 0.6f;

    private TMP_InputField _inputField;
    private RectTransform _rectTransform;
    private Vector2 _positionInitiale;
    private Color _couleurInitiale;
    private Coroutine _shakeCoroutine;
    private Coroutine _flashCoroutine;

    private void Awake()
    {
        _inputField = GetComponent<TMP_InputField>();
        _rectTransform = GetComponent<RectTransform>();

        if (_rectTransform != null)
        {
            _positionInitiale = _rectTransform.anchoredPosition;
        }

        if (inputFieldBackground != null)
        {
            _couleurInitiale = inputFieldBackground.color;
        }
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
            SoundManager.Instance.SoundPlay(MainSfx.Valid);
            passwordGenerator.GenerateNewChallenge(); //relance avec nouvelles fleches et nouveau mot mais meme grille
            GameManager.Instance.victoires++;

            if (_inputField != null)
            {
                _inputField.text = "";
                _inputField.ActivateInputField();
                EventSystem.current.SetSelectedGameObject(gameObject);
            }
        }
        else
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoseTime(20f); //-20s sur timer
            }

            DeclencherFeedbackErreur();
        }
    }
    
    private void DeclencherFeedbackErreur()
    {
        if (_rectTransform != null)
        {
            if (_shakeCoroutine != null)
            {
                StopCoroutine(_shakeCoroutine);
                _rectTransform.anchoredPosition = _positionInitiale;
            }

            _shakeCoroutine = StartCoroutine(Shake());
        }

        if (inputFieldBackground != null)
        {
            if (_flashCoroutine != null)
            {
                StopCoroutine(_flashCoroutine);
            }

            _flashCoroutine = StartCoroutine(RedFlashing());
        }
        SoundManager.Instance.SoundPlay(MainSfx.Wrong);
    }

    private IEnumerator Shake()
    {
        float temps = 0f;

        while (temps < shakeDuration)
        {
            float decalageX = Random.Range(-1f, 1f) * shakeStrength;
            _rectTransform.anchoredPosition = _positionInitiale + new Vector2(decalageX, 0f);

            temps += Time.deltaTime;
            yield return null;
        }

        _rectTransform.anchoredPosition = _positionInitiale;
        _shakeCoroutine = null;
    }

    private IEnumerator RedFlashing()
    {
        float temps = 0f;
        while (temps < dureeMonteeRouge)
        {
            inputFieldBackground.color = Color.Lerp(_couleurInitiale, couleurErreur, temps / dureeMonteeRouge);
            temps += Time.deltaTime;
            yield return null;
        }
        inputFieldBackground.color = couleurErreur;

        temps = 0f;
        while (temps < dureeRetourBlanc)
        {
            inputFieldBackground.color = Color.Lerp(couleurErreur, _couleurInitiale, temps / dureeRetourBlanc);
            temps += Time.deltaTime;
            yield return null;
        }

        inputFieldBackground.color = _couleurInitiale;
        _flashCoroutine = null;
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