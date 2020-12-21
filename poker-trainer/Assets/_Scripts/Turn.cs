using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour
{
    private int gameStage = 0;
    private void OnMouseDown()
    {
        switch (gameStage)
        {
            case 0:
                GameController.S.Flop();
                gameStage++;
                break;
            case 1:
                GameController.S.Tern();
                gameStage++;
                break;
            case 2:
                GameController.S.River();
                gameStage++;
                break;
        }
    }
}
