using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Souls
{
    public class GameManager : MonoBehaviour
    {
        [Space(30)]
        static GameManager _instance;
        public InputHandler InputHandler;
        public PlayerStats PlayerStats;


        void Awake()
        {
            if (_instance != null)
            {
                Debug.LogWarning("Found more than 1 GameManager in scene");
            }
            _instance = this;
            PlayerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
            InputHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<InputHandler>();
        }

        void Update()
        {

        }


        public static GameManager GetInstance()
        {
            return _instance;
        }



    }

}
