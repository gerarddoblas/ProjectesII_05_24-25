using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Item
{
    enum EHealingType {Heal, FullHeal, Invincivility, InvincivilityAndHeal, InvincivilityAndFullHeal }
    [SerializeField] EHealingType healingType;
    [SerializeField] int healthAmount = 5;
    [SerializeField] float invincibilityTime = 2.5f;
    [SerializeField] private GameObject particles;
    override public IEnumerator Effect(GameObject target)
    {
        Instantiate(particles, this.transform.position, Quaternion.identity);
        switch (healingType)
        {
            case EHealingType.Heal:
                target.GetComponent<HealthBehaviour>().Heal(healthAmount);
                break;
            case EHealingType.FullHeal: 
                target.GetComponent<HealthBehaviour>().FullHeal();
                break;
            case EHealingType.Invincivility:
                target.GetComponent<HealthBehaviour>().SetInvincibility(invincibilityTime);
                break;
            case EHealingType.InvincivilityAndHeal:
                target.GetComponent<HealthBehaviour>().Heal(healthAmount);
                target.GetComponent<HealthBehaviour>().SetInvincibility(invincibilityTime);
                break;
            case EHealingType.InvincivilityAndFullHeal: 
                target.GetComponent<HealthBehaviour>().FullHeal();
                target.GetComponent<HealthBehaviour>().SetInvincibility(invincibilityTime);
                break;
        }
        yield return null;
    }
}
