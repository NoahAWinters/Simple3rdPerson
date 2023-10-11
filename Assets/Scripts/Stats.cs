using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Souls
{
    public abstract class Stats : MonoBehaviour
    {
        public int maxHealth;
        public int curHealth;
        public abstract void TakeDamage(int damage);
    }
}