using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Item
{
    enum EHealingType {Heal, FullHeal, Invincivility, InvincivilityAndHeal, InvincivilityAndFullHeal }
    [SerializeField] EHealingType healingType;
    [SerializeField] int healthAmount = 5;
    [SerializeField] float invincibilityTime = 2.5f;
    [SerializeField] private GameObject invencibleParticles;
    [SerializeField] private GameObject healParticles;
    override public IEnumerator Effect(GameObject target)
    {
        
        switch (healingType)
        {
            case EHealingType.Heal:
                target.GetComponent<HealthBehaviour>().Heal(healthAmount);
                Instantiate(healParticles, this.transform.position, Quaternion.identity);
                break;
            case EHealingType.FullHeal: 
                target.GetComponent<HealthBehaviour>().FullHeal();
                Instantiate(healParticles, this.transform.position, Quaternion.identity);
                break;
            case EHealingType.Invincivility:
                target.GetComponent<HealthBehaviour>().SetInvincibility(invincibilityTime);
                Instantiate(invencibleParticles, this.transform.position, Quaternion.identity);
                break;
            case EHealingType.InvincivilityAndHeal:
                target.GetComponent<HealthBehaviour>().Heal(healthAmount);
                target.GetComponent<HealthBehaviour>().SetInvincibility(invincibilityTime);
                Instantiate(invencibleParticles, this.transform.position, Quaternion.identity);
                Instantiate(healParticles, this.transform.position, Quaternion.identity);
                break;
            case EHealingType.InvincivilityAndFullHeal: 
                target.GetComponent<HealthBehaviour>().FullHeal();
                target.GetComponent<HealthBehaviour>().SetInvincibility(invincibilityTime);
                Instantiate(invencibleParticles, this.transform.position, Quaternion.identity);
                Instantiate(healParticles, this.transform.position, Quaternion.identity);
                break;
        }
        yield return null;
    }
}
