using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class PlayerStats : Stats
    {
        AnimHandler _animHandler;


        [Header("Vigor")]
        public int healthLevel = 10;
        public HealthBar healthBar;




        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            curHealth = maxHealth;
            healthBar.SetMaxValue(maxHealth);


            Transform tempPlayer = PlayerManager.GetInstance().transform;
            _animHandler = tempPlayer.GetComponentInChildren<AnimHandler>();
        }


        //Vigor
        int SetMaxHealthFromHealthLevel()
        {
            return healthLevel * 10;
        }

        public override void TakeDamage(int damage)
        {
            curHealth -= damage;
            healthBar.SetCurrentValue(curHealth);

            if (curHealth <= 0)
            {
                curHealth = 0;
                _animHandler.PlayTargetAnim("Die", true);
            }
            else
            {
                _animHandler.PlayTargetAnim("Hit", true);
            }

        }
    }
}
