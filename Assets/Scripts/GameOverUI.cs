using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnTempsEcoule += AfficherGameOver;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnTempsEcoule -= AfficherGameOver;
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