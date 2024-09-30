using UnityEngine;
using YG;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private GameObject _authPanel;
    [SerializeField] private LeaderboardYG _leaderboardYG;

    private int _lastLevel = 0;


    private void OnEnable()
    {
        if (!YandexGame.auth)
        {
            _authPanel.SetActive(true);
        }
        else
        {
            _authPanel.SetActive(false);
            if (_lastLevel != GameManager.currentLevel)
            {
                _lastLevel = GameManager.currentLevel;  
                _leaderboardYG.UpdateLB();
            }
        }
    }
    
}
