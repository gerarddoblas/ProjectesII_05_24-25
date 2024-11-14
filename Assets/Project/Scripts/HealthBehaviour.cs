using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthBehaviour : MonoBehaviour
{
    public bool invencibility = false;
    public int health, maxhealth;
    public UnityEvent<int, int> OnAlterHealth;
    public UnityEvent OnDie;
    private void Awake()
    {
        this.health = this.maxhealth;
        this.OnAlterHealth.Invoke(this.health, this.maxhealth);
    }
    public void Heal(){ 
        this.health++;
        if(this.health>this.maxhealth)
            this.health = this.maxhealth;
        this.OnAlterHealth.Invoke(this.health, this.maxhealth);
    }
    public void Heal(int healing) {
        this.health+=healing;
        if (this.health > this.maxhealth)
            this.health = this.maxhealth;
        this.OnAlterHealth.Invoke(this.health, this.maxhealth);
    }
    public void FullHeal()
    {
        this.health = this.maxhealth;
        this.OnAlterHealth.Invoke(this.health, this.maxhealth);
    }
    public void Damage() {
        if (!invencibility)
        {
            this.health--;
            if (this.health <= 0)
            {
                this.health = 0;
                this.OnDie.Invoke();
            }
            this.OnAlterHealth.Invoke(this.health, this.maxhealth);
        }
    }
    public void Damage(int damage) {
        if (!invencibility)
        {
            this.health -= damage;
            if (this.health <= 0)
            {
                this.health = 0;
                this.OnDie.Invoke();
            }
            this.OnAlterHealth.Invoke(this.health, this.maxhealth);
        }
    }
    public void FullDamage()
    {
        if (!invencibility)
        {
            this.health = 0;
            this.OnAlterHealth.Invoke(this.health, this.maxhealth);
            this.OnDie.Invoke();
        }
    }
    public void AddMaxHealth()
    {
        this.maxhealth++;
        this.OnAlterHealth.Invoke(this.health, this.maxhealth);
    }
    public void AddMaxHealth(bool recover)
    {
        this.maxhealth++;
        if (recover)
        {
            FullHeal();
            return;
        }
        this.OnAlterHealth.Invoke(this.health, this.maxhealth);
    }
    public void AddMaxHealth(int healthToAdd)
    {
        this.maxhealth+= healthToAdd;
        this.OnAlterHealth.Invoke(this.health, this.maxhealth);
    }
    public void AddMaxHealth(int healthToAdd,bool recover)
    {
        this.maxhealth+= healthToAdd;
        if (recover)
        {
            FullHeal();
            return;
        }
        this.OnAlterHealth.Invoke(this.health, this.maxhealth);
    }
    public void ReduceMaxHealth()
    {
        this.maxhealth--;
        if (this.maxhealth <= 0)
            this.maxhealth = 1;
        if(this.maxhealth < this.health)
            this.health = this.maxhealth;
        this.OnAlterHealth.Invoke(this.health, this.maxhealth);
    }
    public void ReduceMaxHealth(int healthToQuit)
    {
        this.maxhealth-=healthToQuit;
        if (this.maxhealth <= 0)
            this.maxhealth = 1;
        if (this.maxhealth < this.health)
            this.health = this.maxhealth;
        this.OnAlterHealth.Invoke(this.health, this.maxhealth);
    }
    public IEnumerator SetInvencibility(float invencibilitySeconds)
    {
        this.invencibility = true;
        yield return new WaitForSeconds(invencibilitySeconds);
        this.invencibility = false;
    }
}
