using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWin : MonoBehaviour
{ 
    void OnTriggerEnter2D(Collider2D other)
   {
       if (other.CompareTag("Player"))
       {
           GameManager.instance.gameStateManager.ChangeState(GameStateManager.GameState.GameWin);
       }
   }
}
