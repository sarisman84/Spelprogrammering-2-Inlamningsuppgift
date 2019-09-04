using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Team { Red, Blue, Yellow, Green, Orange, White }
public interface IUsers
{
    Team CurrentTeam { get; set; }
}
