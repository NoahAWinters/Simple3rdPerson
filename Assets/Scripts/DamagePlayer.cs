using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class DamagePlayer : MonoBehaviour
    {
        public int damage = 5;
        PlayerStats _player;

        void Start()
        {
            _player = GameManager.GetInstance().PlayerStats;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
                if (_player != null)
                    _player.TakeDamage(damage);
        }
    }
}
