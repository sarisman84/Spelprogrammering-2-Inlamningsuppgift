using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ChineseCheckers;
[System.Serializable]
public class PlayerMatchup
{
    //public enum Team { Empty, Unoccupied, Red, Blue, Yellow, Green, Magenta, Orange }









    static IPlayer CreatePlayer(GameManager.Player firstPlayer, GameManager.Player secondPlayer)
    {
        IPlayer player = (firstPlayer.isComputer) ? CompPlayer.CreatePlayer(firstPlayer.team, secondPlayer.team) : HumanPlayer.CreatePlayer(firstPlayer.team, secondPlayer.team);
        return player;


    }


    public static bool HasPlayerWon(IPlayer player)
    {
        if (player == null) return false;
        if (player.PlayerBase.All(p => p != null && p.StoredPiece != null && p.StoredPiece.BelongsTo == player.CurrentOpponent))
        {
            Debug.Log($"Player {player.CurrentOpponent} has won!");
            return true;
        }


        return false;
    }


    public static IPlayer[][] StartNewGame(GameManager.Matchups[] players, bool oddAmmPlayers, Piece piecePrefab, GameModes modes)
    {
        IPlayer[] secondPlayers = new IPlayer[players.Length];
        IPlayer[] firstPlayers = new IPlayer[players.Length];
        if (oddAmmPlayers)
        {
            for (int i = 0; i < players.Length; i++)
            {
                firstPlayers[i] = (players[i].firstPlayer.team == Team.Empty || players[i].firstPlayer.team == Team.Unoccupied) ? null : CreatePlayer(players[i].firstPlayer, players[i].secondPlayer);
                PlacePieces(firstPlayers[i], piecePrefab);
            }
        }

        for (int i = 0; i < players.Length; i++)
        {
            firstPlayers[i] = (players[i].firstPlayer.team == Team.Empty || players[i].firstPlayer.team == Team.Unoccupied) ? null : CreatePlayer(players[i].firstPlayer, players[i].secondPlayer);
            secondPlayers[i] = (players[i].secondPlayer.team == Team.Empty || players[i].secondPlayer.team == Team.Unoccupied) ? null : CreatePlayer(players[i].secondPlayer, players[i].firstPlayer);
            PlacePieces(firstPlayers[i], piecePrefab);
            if (modes != GameModes.Debug)
                PlacePieces(secondPlayers[i], piecePrefab);
            if (modes == GameModes.Debug)
            {
                PlacePieces(secondPlayers[i]);
            }

        }

        return new IPlayer[][] { firstPlayers, secondPlayers };

    }

    private static void PlacePieces(IPlayer players, Piece piecePrefab)
    {
        if (players == null || players.BelongsTo == Team.Empty || players.BelongsTo == Team.Unoccupied) return;
        List<Node> gettingPlayerBase = new List<Node>();
        for (int y = 0; y < BoardManager.board.GetLength(0); y++)
        {

            for (int x = 0; x < BoardManager.board.GetLength(1); x++)
            {
                Node node = BoardManager.board[y, x];

                if (node.BelongsTo == players.BelongsTo)
                {
                    node.StoredPiece = Piece.CreatePiece(piecePrefab, Color.black, node, players.BelongsTo);
                    gettingPlayerBase.Add(node);
                }
            }

        }
        players.PlayerBase = gettingPlayerBase.ToArray();

    }

    private static void PlacePieces(IPlayer players)
    {
        if (players == null || players.BelongsTo == Team.Empty || players.BelongsTo == Team.Unoccupied) return;
        List<Node> gettingPlayerBase = new List<Node>();
        for (int y = 0; y < BoardManager.board.GetLength(0); y++)
        {

            for (int x = 0; x < BoardManager.board.GetLength(1); x++)
            {
                Node node = BoardManager.board[y, x];

                if (node.BelongsTo == players.BelongsTo)
                {
                    gettingPlayerBase.Add(node);
                }
            }

        }
        players.PlayerBase = gettingPlayerBase.ToArray();

    }

}
