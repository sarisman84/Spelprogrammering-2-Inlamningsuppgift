using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Board {
    public class GameManager : MonoBehaviour {

        //This class will handle the general systems such as board generation, piece handling as well as movement for the players!

        #region Singleton Property
        private static GameManager instance;
        public static GameManager BoardSystem {
            get {
                //If instance is null, either find an already existing GameManager or create a new GameObject and assign that to the instance.
                if (instance == null) {
                    instance = FindObjectOfType<GameManager> () ?? new GameObject ("GameManager").AddComponent<GameManager> ();
                }
                //Return the instance
                return instance;
            }
        }
        #endregion

    }
}