using _TeamTheDream._Delivery;
using DG.Tweening;
using UnityEngine;

public class EndGameUiController: MonoBehaviour
{
    [SerializeField] private CanvasGroup _screen;

    private void Start()
    {
        EndGameLauncher.OnEndGame += EndGameLauncher_OnEndGame;
        _screen.gameObject.SetActive(false);
    }
    
    private void OnDestroy()
    {
        EndGameLauncher.OnEndGame -= EndGameLauncher_OnEndGame;
    }

    private void EndGameLauncher_OnEndGame()
    {
        _screen.alpha = 0;
        _screen.gameObject.SetActive(true);
        _screen.DOFade(1f, 1f);
    }
}