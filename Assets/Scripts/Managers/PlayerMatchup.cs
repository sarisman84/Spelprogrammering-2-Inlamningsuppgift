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
    /// Creates a player based on what the GameManager.Player data has indicated.
    /// </summary>
    /// <returns>An IPlayer that correlates in either the CompPlayer script or the HumanPlayer script.</returns>
    static IPlayer CreatePlayer(GameManager.Player firstPlayer, GameManager.Player secondPlayer)
    {
        IPlayer player = (firstPlayer.isComputer) ? CompPlayer.CreatePlayer(firstPlayer.team, secondPlayer.team) : HumanPlayer.CreatePlayer(firstPlayer.team, secondPlayer.team);
        return player;

    }

    /// <summary>
    /// Checks if a win condition has been reached.
    /// </summary>
    /// <param name="player">An IPlayer variable that is being used to see if all of PlayerBase nodes are full of its opponents pieces.</param>
    /// <returns>True if all pieces in the PlayerBase nodes are of the player's opponent</returns>
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

    /// <summary>
    /// Starts the game with an x amount of players using set parameters as well as helper methods to properly create an exact amount of players.
    /// </summary>
    /// <param name="players">An array of data that tells the method what team the current player is, what color is its opponent and if said player is a computer or not.</param>
    /// <param name="oddAmmPlayers">A boolean that checks if the amount of desiredPlayers is odd. Set this to true if you want odd amounts of players (3,5,7, ect).</param>
    /// <param name="piecePrefab">A prefab reference to the piece gameObject in order to be created. </param>
    /// <param name="modes">A reference to the Gamemodes enum, which is being used to see if the current mode is GameModes.Debug</param>
    /// <returns>A jaggered array of all players (first and second players). </returns>
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
            if (modes == GameModes.Debug)
            {
                PlacePieces(secondPlayers[i]);
                continue;
            }
            PlacePieces(secondPlayers[i], piecePrefab);

        }

        return new IPlayer[][] { firstPlayers, secondPlayers };

    }
    /// <summary>
    /// Helper method that instantiates pieces on the correct nodes as well as saves said pieces in a private array per player.
    /// </summary>
    /// <param name="players"> The current player that owns the pieces. </param>
    /// <param name="piecePrefab">A prefab reference to create said pieces. </param>
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

    /// <summary>
    /// Helper method that saves  pieces in a private array per player. Is meant as a Debug helper.
    /// </summary>
    /// <param name="players">The current player that owns the pieces.</param>
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