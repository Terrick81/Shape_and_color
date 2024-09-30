using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class Rewind : MonoBehaviour
{
    private void OnEnable() => YandexGame.CloseVideoEvent += AdTurn;
    private void OnDisable() => YandexGame.CloseVideoEvent -= AdTurn;
    private void AdTurn()
    {
        GameManager.AddTurnStatic();
    }
}
