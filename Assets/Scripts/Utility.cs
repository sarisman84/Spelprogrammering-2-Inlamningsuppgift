using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Spyro
{
    public static class Utility
    {
        public static T[] OverlapCircleAll<T>(Vector2 point, float radius, LayerMask maskLayer)
        {
            Collider2D[] arr = Physics2D.OverlapCircleAll(point, radius, maskLayer.value);
            T[] newArr = new T[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                newArr[i] = arr[i].GetComponent<T>();
            }
            return newArr;
        }
    }
}