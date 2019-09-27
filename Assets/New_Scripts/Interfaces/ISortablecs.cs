using UnityEngine;
using System;
public interface ISortable<T> where T : IComparable
{
    T Value { get; }

    int CompareTo(object obj);
}
