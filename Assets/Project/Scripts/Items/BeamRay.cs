using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamRay : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<HealthBehaviour>(out HealthBehaviour hb))
        {
            hb.FullDamage();
            GameController.Instance.RemoveScore(hb.maxhealth, collision.gameObject);
        }
    }
}
