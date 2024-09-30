using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using YG;

public class GameManager : MonoBehaviour
{
    #region Fields

    public const int MAX_TESTING_LVL = 2000;
    private const int REWAND_TURN = 7;

    [SerializeField] GameObject _cellGrid;
    [SerializeField] GameObject _figureGrid;
    [SerializeField] GameObject _cellPrefab;
    [SerializeField] GameObject _figurePrefab;
    [SerializeField] SFXManager _sfxManager;
    [SerializeField] GameObject _menuPage;
    [SerializeField] GameObject _gamePage;
    [SerializeField] GameObject _loadingPage;
    [SerializeField] GameObject _winPage;
    [SerializeField] GameObject _defeatPage;
    [SerializeField] GameObject _adPage;
    [SerializeField] GameObject _leaderboardPage;
    [SerializeField] TextMeshProUGUI[] _text;
    [SerializeField] Animator _turnAnimator;

    private Level _level;
    private bool _isPlaying = false;

    private static GameManager _gameManagerScript;
    private static int _turn = 0;
    private static int _maxTurn = 0;
    private static bool _win = false;
    private static YandexGame _yandexGameScript;

    public static int currentLevel = 1;
    public static StreamWriter file;
    public static string nameFile;
    public static bool generationTest = false;
    public static bool safeScreenshot = false;
    
    
    #endregion
    
    void Start()
    {
        currentLevel = SavesYG.GetCompletedLevel() + 1;
        _yandexGameScript = GameObject.Find("YandexGame").GetComponent<YandexGame>();
        _gameManagerScript = GetComponent<GameManager>();
        OpenMenuPage();
        Forms.MyAwake();
        if (generationTest) StartCoroutine(GenerateTestLevel());
    }

    IEnumerator GenerateTestLevel()
    {
        nameFile = DateTime.Now.ToString("HH-mm-ss");
        UnityEngine.Debug.unityLogger.logEnabled = false;
        System.IO.File.AppendAllText("C:\\Users\\Terre\\OneDrive\\Рабочий стол\\" + nameFile +  ".txt", "!!Список неудачных уровней!!");
        if (safeScreenshot) OpenGamePage(_menuPage);
        for (int i = 0; i < MAX_TESTING_LVL; i++)
        {
            if (safeScreenshot)
            {
                NextLevel();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                ScreenCapture.CaptureScreenshot("C:\\Users\\Terre\\OneDrive\\Рабочий стол\\screenshot\\" + currentLevel + ".png");
            }
            else
            {
                LevelStorage.LevelDownload(i);
                _text[0].text = i.ToString();
                yield return null;
            }
        }
    }

    public static int GetTurn()
    {
        return _turn;
    }
    public void SetTurn(int turn)
    {
        if(currentLevel < 200) turn += turn * (200 - currentLevel) / 400;
        else turn += 4;
        if (_level.GetHiddenMode()) turn += 2; 
        
        turn += 1;
        _turn = turn;
        _maxTurn = turn;
        _turnAnimator.writeDefaultValuesOnDisable = true;
        _turnAnimator.enabled = false;
        _text[4].text = _turn.ToString();
        Localization.TurnTextStatic(_turn);
    }

    public static void AddTurnStatic()
    {
        _gameManagerScript.AddTurn();
    }
    
    private void AddTurn()
    {
        _adPage.SetActive(false);
        _turn += REWAND_TURN;
        _text[4].text = _turn.ToString();
        Localization.TurnTextStatic(_turn);
        if (_turn > _maxTurn * 0.2f)
        {
            _turnAnimator.WriteDefaultValues();
            _turnAnimator.enabled = false;
        }
    }


    public static void TurnStatic()
    {
        _gameManagerScript.Turn();
    }
    public void Turn()
    {
        _turn--;
        if (_turn <= 0)
        {
            if (!_win) { _adPage.SetActive(true); }
        }
        else
        {
            _text[4].text = _turn.ToString();
            Localization.TurnTextStatic(_turn);
            if (_turn < _maxTurn * 0.2f)
            {
                _turnAnimator.writeDefaultValuesOnDisable = true;
                _turnAnimator.enabled = true;
            }
        }      
    }

    IEnumerator StartLevel()
    {
        _win = false;
        _gamePage.SetActive(false);
        _loadingPage.SetActive(true);
        yield return new WaitForEndOfFrame();
        if (_level == null)
            _level = LevelStorage.LevelDownload(currentLevel);
        ClearGrid();
        CreateForms();
        yield return new WaitForEndOfFrame();
        _yandexGameScript._FullscreenShow();
        yield return new WaitForSeconds(0.3f);
        _loadingPage.SetActive(false);
        OpenGamePageSecond();
    }

    public YandexGame GetYGScript()
    {
        return _yandexGameScript;
    }

    public void Restart()
    {
        StartCoroutine(StartLevel());
    }
    private void ClearGrid()
    {
        foreach (Transform child in _cellGrid.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in _figureGrid.transform)
        {
            Destroy(child.gameObject);
        }
    }
    private void CreateForms()
    {
        Vector2Int mapSize = _level.GetSize();
        Vector2 sizeGrid = _cellGrid.GetComponent<RectTransform>().sizeDelta;
        int sizeFormY = (int)(sizeGrid[1] / mapSize[0]);
        int sizeFormX = (int)(sizeGrid[0] / mapSize[1]);
        int sizeForm = System.Math.Min(sizeFormY, sizeFormX);
        Forms.SetParameters(sizeForm, mapSize, sizeForm / 4, _level.GetHiddenMode());
        CD[,] fieldPlan = _level.GetMap();
        for (int i = 0; i < mapSize[0]; i++)
        {
            for (int j = 0; j < mapSize[1]; j++)
            {
                CD cellData = fieldPlan[i, j];
                GameObject cell = Instantiate(_cellPrefab, _cellGrid.transform);
                cell.GetComponent<cell>().Create( coord: new Vector2Int(i, j),
                                                 colors: cellData.downColor, 
                                                 shapes: cellData.downShape);
                if (cellData.figureShape)
                {
                    GameObject figure = Instantiate(_figurePrefab, _figureGrid.transform);
                    figure.GetComponent<Figure>().Create( coord: new Vector2Int(i, j),
                                                         colors: cellData.upColor,
                                                         shapes: cellData.upShape,
                                                           cell: cell.GetComponent<cell>());
                }
            }
        }
        SetTurn(Forms.GetOutOfPlace());
    }

    #region PageMetods
    public void OpenMenuPage(GameObject oldPage)
    {
        oldPage.SetActive(false);
        _menuPage.SetActive(true);
        OpenMenuPage();
    }
    public void OpenMenuPage()
    {
        _text[0].text = currentLevel.ToString();
    }
    public void OpenWinPage()
    {
        _win = true;
        _isPlaying = false;
        _gamePage.SetActive(false);
        _winPage.SetActive(true);
        _text[2].text = currentLevel.ToString();
        _sfxManager.PlayClipWithStop(nameClip.win);
        SavesYG.SetCompletedLevel(currentLevel);
        YandexGame.NewLeaderboardScores("Leaderboard", currentLevel);
        currentLevel++;
        _level = null;
    }
    public static void OpenWinPageStatic() 
    {
        _gameManagerScript.OpenWinPage();
    }
    public void OpenGamePage(GameObject oldPage) 
    {
        oldPage.SetActive(false);
        if (!_isPlaying)
        {
            _isPlaying = true;
            StartCoroutine(StartLevel());
        }
        else
            OpenGamePageSecond();
    }
    public void OpenGamePageSecond()
    {
        _gamePage.SetActive(true);
        _text[1].text = currentLevel.ToString();
    }

    public void OpenDefeatPage() 
    {
        if (!_win)
        {
            _isPlaying = false;
            _gamePage.SetActive(false);
            _adPage.SetActive(false);
            _defeatPage.SetActive(true);
            _text[3].text = currentLevel.ToString();
            _sfxManager.PlayClipWithStop(nameClip.defeat);
        }
    }
    public void NextLevel()
    {
        OpenWinPage();
        OpenGamePage(_winPage);
    }
    public void OpenAdPage()
    {
        _adPage.SetActive(true);
    }

    public void OpenLeaderboardPage()
    {
        _menuPage.SetActive(false);
        _leaderboardPage.SetActive(true);
    }

    #endregion
}
