using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ChineseCheckers;

public class CompPlayer: IPlayer
{
    public Team BelongsTo { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public Team CurrentOpponent { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public Node[] PlayerBase { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public static IPlayer CreatePlayer(Team currentTeam, Team opponent)
    {
        IPlayer player = new CompPlayer();
        player.BelongsTo = currentTeam;
        player.CurrentOpponent = opponent;
        return player;
    }
}
