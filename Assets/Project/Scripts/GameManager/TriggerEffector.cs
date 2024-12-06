using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class TriggerEffector : MonoBehaviour
{
   public UnityEvent onEntry,onStay,onExit;
    private void OnTriggerEnter2D(Collider2D collision){ onEntry.Invoke();}
    private void OnTriggerStay2D(Collider2D collision){onStay.Invoke();}
    private void OnTriggerExit2D(Collider2D collision){onExit.Invoke();}

}
