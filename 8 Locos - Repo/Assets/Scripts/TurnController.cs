using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    void OnEnable() 
    {
        SeatsController.currentSC.ShowEveryoneWhosTurnItIs(GameController.gameController.mySeat);
        GameController.gameController.isMyTurn = true;
    }

    void OnDisble() 
    {
        GameController.gameController.isMyTurn = false;
    }
}
