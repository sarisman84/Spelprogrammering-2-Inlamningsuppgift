using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ChineseCheckers;
using UnityEngine;
[System.Serializable]
public class PlayerMatchup
{
    //public enum Team { Empty, Unoccupied, Red, Blue, Yellow, Green, Magenta, Orange }

    /// <summary>
    /// Checks if a win condition has been reached.
    /// </summary>
    /// <param name="player">An IPlayer variable that is being used to see if all of PlayerBase nodes are full of its opponents pieces.</param>
    /// <returns>True if all pieces in the PlayerBase nodes are of the player's opponent</returns>
    public static bool HasPlayerWon(IPlayer player)
    {
        if (player == null || player.CurrentOpponent == null) return false;
        if (player.CurrentOpponent.TeamBase.All(p => p != null && p.StoredPiece != null && p.StoredPiece.BelongsTo == player.CurrentOpponent.Team))
        {
            Debug.Log($"Player {player.CurrentOpponent} has won!");
            return true;
        }

        return false;
    }

    static IPlayer CreatePlayer(GameManager.GameMode.Match.Player player)
    {
        IPlayer newPlayer = (player.isComputer) ? CompPlayer.CreatePlayer(player.playerTeam) : HumanPlayer.CreatePlayer(player.playerTeam);
        return newPlayer;
    }

    static IPlayer OnCreatingPlayer(GameManager.GameMode.Match.Player player, TeamGenerator team)
    {
        GameManager.GameMode.Match.Player newPlayer = player;
        Team playerTeam = newPlayer.playerTeam;
        switch (playerTeam)
        {
            case Team.Empty:
            case Team.Unoccupied:
                return null;
        }
        IPlayer iPlayer = CreatePlayer(newPlayer);
        iPlayer.CurrentTeam = team;
        return iPlayer;

    }

    /// <summary>
    /// Starts the game with an x amount of players using set parameters as well as helper methods to properly create an exact amount of players.
    /// </summary>
    /// <param name="players">An array of data that tells the method what team the current player is, what color is its opponent and if said player is a computer or not.</param>
    /// <param name="oddAmmPlayers">A boolean that checks if the amount of desiredPlayers is odd. Set this to true if you want odd amounts of players (3,5,7, ect).</param>
    /// <param name="piecePrefab">A prefab reference to the piece gameObject in order to be created. </param>
    /// <param name="modes">A reference to the Gamemodes enum, which is being used to see if the current mode is GameModes.Debug</param>
    /// <returns>A jaggered array of all players (first and second players). </returns>
    public static IPlayer[] StartNewGame(GameManager.GameMode gameMode, Piece piecePrefab)
    {

        List<IPlayer> allPlayers = new List<IPlayer>();
        TeamGenerator[] teams = new TeamGenerator[6];
        for (int t = 0; t < 6; t++)
        {
            teams[t] = new TeamGenerator();
            teams[t].Team = (Team)(t + 2);
            Debug.Log($"{(Team)(t + 2)} at {t} using {t + 2}");
            List<Node> nodeList = new List<Node>();
            for (int y = 0; y < BoardManager.board.GetLength(0); y++)
            {
                for (int x = 0; x < BoardManager.board.GetLength(1); x++)
                {
                    Node node = BoardManager.board[y, x];
                    if (node.BelongsTo == teams[t].Team)
                    {
                        nodeList.Add(node);
                    }
                }
            }
            teams[t].TeamBase = nodeList.ToArray();
        }

        for (int i = 0; i < gameMode.matches.Length; i++)
        {

            GameManager.GameMode.Match.Player newPlayer = gameMode.matches[i].player;
            Debug.Log($"using {i} to spawn this player ({teams[(int)newPlayer.playerTeam - 2].Team})");
            IPlayer _newPlayer = OnCreatingPlayer(newPlayer, teams[(int)newPlayer.playerTeam - 2]);
            PlacePieces(_newPlayer, piecePrefab);
            allPlayers.Add(_newPlayer);
        }
        return allPlayers.ToArray();

        // IPlayer[] secondPlayers = new IPlayer[players.Length];
        // IPlayer[] firstPlayers = new IPlayer[players.Length];
        // if (oddAmmPlayers) {
        //     for (int i = 0; i < players.Length; i++) {

        //         firstPlayers[i] = (players[i].firstPlayer.team == Team.Empty || players[i].firstPlayer.team == Team.Unoccupied) ? null : CreatePlayer (players[i].firstPlayer);
        //         firstPlayers[i].CurrentOpponent = (i - 1 < 0) ? null : firstPlayers[i - 1];
        //         firstPlayers[0].CurrentOpponent = firstPlayers[firstPlayers.Length - 1];
        //         PlacePieces (firstPlayers[i], piecePrefab);
        //     }

        // }

        // for (int i = 0; i < players.Length; i++) {
        //     firstPlayers[i] = (players[i].firstPlayer.team == Team.Empty || players[i].firstPlayer.team == Team.Unoccupied) ? null : CreatePlayer (players[i].firstPlayer, players[i].secondPlayer);
        //     secondPlayers[i] = (players[i].secondPlayer.team == Team.Empty || players[i].secondPlayer.team == Team.Unoccupied) ? null : CreatePlayer (players[i].secondPlayer, players[i].firstPlayer);
        //     PlacePieces (firstPlayers[i], piecePrefab);
        //     if (modes == GameModes.Debug) {
        //         PlacePieces (secondPlayers[i]);
        //         continue;
        //     }
        //     PlacePieces (secondPlayers[i], piecePrefab);

        // }

        // return new IPlayer[][] { firstPlayers, secondPlayers };

    }

    /// <summary>
    /// Helper method that instantiates pieces on the correct nodes as well as saves said pieces in a private array per player.
    /// </summary>
    /// <param name="players"> The current player that owns the pieces. </param>
    /// <param name="piecePrefab">A prefab reference to create said pieces. </param>
    private static void PlacePieces(IPlayer player, Piece piecePrefab)
    {
        if (player == null) return;
        if (player.CurrentTeam == null || player.CurrentTeam.Team == Team.Empty || player.CurrentTeam.Team == Team.Unoccupied) return;
        List<Node> playerBase = new List<Node>();
        for (int y = 0; y < BoardManager.board.GetLength(0); y++)
        {
            for (int x = 0; x < BoardManager.board.GetLength(1); x++)
            {
                Node node = BoardManager.board[y, x];
                if (node.BelongsTo == player.CurrentTeam.Team)
                {
                    node.StoredPiece = Piece.CreatePiece(piecePrefab, TeamGenerator.SetColorBasedOnTeam(player.CurrentTeam.Team), node, player.CurrentTeam.Team);
                    playerBase.Add(node);
                }
            }
        }
        player.CurrentTeam.TeamBase = playerBase.ToArray();
    }

}