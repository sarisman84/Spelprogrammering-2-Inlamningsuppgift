using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Callback ();

namespace Spyro {

    //This is meant as a side class that has some usefull functions that make life easier for most of the scripts in this project.
    public static class Utility {

        #region Object Pooling

        //This section holds two functions that make the entire Object Pooling of this project to work. PoolObject creates a list of the desired amount using a desired prefab and sorts them in a Transform for convinience.

        //The second function, GetDesiredPooledObject, goes through all lists until it finds a list that containts the right type of object, which will return one of the inactive objects in said list.

        //Both of these objects will replace the Instantiate function since you can 1. Call PoolObjects at the start of the game and make the desired objects needed and 2. Use GetDesiredPooledObject to get an unused object which optimises the game a bit more. 

        static List<List<GameObject>> libaryOfPooledObjects = new List<List<GameObject>> ();

        /// <summary>
        /// Allows the ability to create a bunch of gameObjects and store the later.
        /// </summary>
        /// <param name="go"> The desired object in question </param>
        /// <param name="amount"> The desired amount that is inputed </param>
        /// <param name="parent"> The desired Transform that allows you to sort to a specific transform. </param>
        public static void PoolObject (GameObject go, int amount, Transform parent) {
            List<GameObject> pooledObjects = new List<GameObject> ();
            for (int i = 0; i < amount; i++) {
                GameObject clone = MonoBehaviour.Instantiate (go, parent);
                pooledObjects.Add (clone);
                clone.SetActive (false);
            }
            libaryOfPooledObjects.Add (pooledObjects);
        }

        /// <summary>
        /// Gets a inactive gameObject in a list that contains said object. The function searches using a Type that the gameObject contains.
        /// </summary>
        /// <returns> An inactive gameObject </returns>
        public static GameObject GetDesiredPooledObject<T> () {
            foreach (var pooledObjects in libaryOfPooledObjects) {
                if (pooledObjects[0].GetComponent<T> () == null) continue;
                foreach (var go in pooledObjects) {
                    if (!go.activeInHierarchy) {
                        return go;
                    }
                }

                GameObject clone = MonoBehaviour.Instantiate (pooledObjects[0], pooledObjects[0].transform.parent);
                pooledObjects.Add (clone);
                return clone;
            }
            return null;
        }

        #endregion

        /// <summary>
        /// Enables the ability to delay an execution of a parameterless void function by x seconds every frame (meant to be used in a Update Loop)
        /// </summary>
        /// <param name="seconds"> The amount of seconds needed to be passed. </param>
        /// <param name="counter">A reference to a local variable in the script that is currently in. </param>
        /// <param name="callback"> The desired method to delay execution. </param>
        public static void DelayExecution (float seconds, ref float counter, Callback callback) {
            counter += Time.deltaTime;
            if (counter > seconds) {
                callback ();
                counter = 0;
            }
        }
    }
}