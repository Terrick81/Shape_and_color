using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Education : MonoBehaviour
{
    private const int MAX_LANGUAGE = 3;
    private const int MAX_DESCRIPTION_POSITION = 6;
    
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _educationLayer;
    [SerializeField] private GameObject _WinPage;
    [SerializeField] private GameObject _gamePage;
    [SerializeField] private TextMeshProUGUI _text;

    private static int _seed = 0;
    private static Education _education;
    private Dictionary<int, int> _idDescription =
        new Dictionary<int, int> { 
            {  1, 0 }, 
            {  3, 1 }, 
            {  8, 2 },
            { 11, 3 },
            { 12, 4 },
            { 20, 5 },
        };

    private void Start()
    {
        _education = GetComponent<Education>();
    }

    private string[,] _description = new string[MAX_DESCRIPTION_POSITION, MAX_LANGUAGE]
    {
        {"нажмите на фигуру, а затем на свободное место", "click on the shape and then on the free space", "sekle ve ardından bos alana tıklayın" },
        {"расставьте фигуры по цветам", "arrange the shapes according to the colors", "sekilleri renklere göre düzenleyin" },
        {"расставьте фигуры по формам", "arrange the shapes according to the shapes", "sekilleri sekillere göre düzenleyin" },
        {"фигура может переместиться, только если ее форма или цвет совпадает с местом установки", "The shape can only move if its shape or color matches the installation location", "Bir sekil ancak sekli veya rengi kurulum yeriyle aynıysa hareket edebilir"},
        {"расставьте все фигуры по цветам и формам", "arrange all the shapes according to colors and shapes", "tüm sekilleri renklere ve sekillere göre düzenleyin" },
        {"нажмите на фигуру чтоб проявить цвет", "click on the shape to show the color", "rengi göstermek için sekle tıklayın" },
    };


    private IEnumerator _StartEducation() 
    {
        _educationLayer.SetActive(true);
        while (!_animator.isInitialized)
        {
            yield return null;
        }
        _animator.WriteDefaultValues();
        _animator.SetInteger("Seed", _seed);
        _text.text = _description[_idDescription[_seed], Localization.GetLng()];

    }
    private void  SetHandlers() 
    {
        _WinPage.AddComponent<HandlerWinPage>();
        _gamePage.AddComponent<HandlerGamePage>();
    }
    public static void EducationLayerOff()
    {
        _education._educationLayer.SetActive(false);
        Destroy(_education._gamePage.GetComponent<HandlerGamePage>());
        _seed = 0;
    }
    public static void UpdateLayer()
    {
        if (_seed > 0) 
            _education.StartCoroutine(_education._StartEducation());
    }
    public static void StartEducation(int seed)
    {
        _seed = seed;
        _education.StartCoroutine(_education._StartEducation());
        _education.SetHandlers();
    }
}
