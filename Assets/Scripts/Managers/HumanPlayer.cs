using System;
using UnityEngine;

public class HumanPlayer : MonoBehaviour, IPlayer
{
    [SerializeField] Team currentTeam, opponent;
    [SerializeField] Node[] playerBase;
    public Team BelongsTo { get => currentTeam; set => currentTeam = value; }
    public Team CurrentOpponent { get => opponent; set => opponent = value; }
    public Node[] PlayerBase { get => playerBase; set => playerBase = value; }
    public static IPlayer CreatePlayer(Team currentTeam, Team opponent)
    {
        IPlayer player = new GameObject($"Player {currentTeam}: Human").AddComponent<HumanPlayer>();
        player.BelongsTo = currentTeam;
        player.CurrentOpponent = opponent;
        return player;
    }
}
