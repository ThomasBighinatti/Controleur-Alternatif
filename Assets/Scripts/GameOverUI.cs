using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private bool _abonne;

    private void OnEnable()
    {
        SAbonner();
    }
    
    private void Start()
    {
        SAbonner();
    }

    private void SAbonner()
    {
        if (_abonne || GameManager.Instance == null)
        {
            return;
        }

        GameManager.Instance.OnTempsEcoule += AfficherGameOver;
        _abonne = true;
    }

    private void OnDisable()
    {
        if (_abonne && GameManager.Instance != null)
        {
            GameManager.Instance.OnTempsEcoule -= AfficherGameOver;
            _abonne = false;
        }
    }

    private void AfficherGameOver()
    {
        if (panel != null)
        {
            panel.SetActive(true);
        }
    }

    public void Replay()
    {
        GameManager.Instance.Replay();
    }
    
    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
    
    public void OnClicMenu()
    {
        GameManager.Instance.GoToMainMenu();
    }
}