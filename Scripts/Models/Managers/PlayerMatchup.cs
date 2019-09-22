using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ChineseCheckers;
using UnityEngine;
[System.Serializable]
public class PlayerMatchup {
    //public enum Team { Empty, Unoccupied, Red, Blue, Yellow, Green, Magenta, Orange }

    /// <summary>
    /// Checks if a win condition has been reached.
    /// </summary>
    /// <param name="player">An IPlayer variable that is being used to see if all of PlayerBase nodes are full of its opponents pieces.</param>
    /// <returns>True if all pieces in the PlayerBase nodes are of the player's opponent</returns>
    public static bool HasPlayerWon (IPlayer player) {
        if (player == null || player.CurrentOpponent == null) return false;
        if (player.CurrentOpponent.CurrentTeam.TeamBase.All (p => p != null && p.StoredPiece != null && p.StoredPiece.BelongsTo == player.CurrentOpponent.CurrentTeam.Team)) {
            Debug.Log ($"Player {player.CurrentOpponent} has won!");
            return true;
        }

        return false;
    }

    static IPlayer CreatePlayer (GameManager.GameMode.Match.Player player, TeamGenerator team) {
        IPlayer newPlayer = (player.isComputer) ? CompPlayer.CreatePlayer (player.playerTeam, team) : HumanPlayer.CreatePlayer (player.playerTeam, team);
        return newPlayer;
    }

    static IPlayer OnCreatingPlayer (GameManager.GameMode.Match.Player player, TeamGenerator team) {
        GameManager.GameMode.Match.Player newPlayer = player;
        Team playerTeam = newPlayer.playerTeam;
        switch (playerTeam) {
            case Team.Empty:
                return null;
        }
        IPlayer iPlayer = CreatePlayer (newPlayer, team);

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
    public static IPlayer[] StartNewGame (GameManager.GameMode gameMode, PieceObject piecePrefab) {

        List<IPlayer> allPlayers = new List<IPlayer> ();
        TeamGenerator[] teams = new TeamGenerator[15];
        for (int t = 0; t < teams.Length; t++) {
            teams[t] = new TeamGenerator ();
            teams[t].Team = (Team) (t);
            // Debug.Log($"{(Team)(t)} at {t} using {t}");
            List<Node> nodeList = new List<Node> ();
            for (int y = 0; y < BoardManager.originalBoard.GetLength (0); y++) {
                for (int x = 0; x < BoardManager.originalBoard.GetLength (1); x++) {
                    Node node = BoardManager.originalBoard[y, x];
                    if (gameMode.matches.Length == 2) {
                        node.BelongsTo = (node.BelongsTo == Team.BigRed || node.BelongsTo == Team.BigRedToOrange || node.BelongsTo == Team.BigMagentaToRed) ? Team.Red : node.BelongsTo;
                        node.BelongsTo = (node.BelongsTo == Team.BigBlue || node.BelongsTo == Team.BigGreenToBlue || node.BelongsTo == Team.BigBlueToYellow) ? Team.Blue : node.BelongsTo;
                    } else {
                        node.BelongsTo = (node.BelongsTo == Team.BigRed || node.BelongsTo == Team.BigRedToOrange || node.BelongsTo == Team.BigMagentaToRed) ? Team.Unoccupied : node.BelongsTo;
                        node.BelongsTo = (node.BelongsTo == Team.BigBlue || node.BelongsTo == Team.BigGreenToBlue || node.BelongsTo == Team.BigBlueToYellow) ? Team.Unoccupied : node.BelongsTo;
                    }
                    if (node.BelongsTo == teams[t].Team) {
                        nodeList.Add (node);
                    }
                }
            }
            teams[t].TeamBase = nodeList.ToArray ();
        }

        for (int i = 0; i < gameMode.matches.Length; i++) {

            GameManager.GameMode.Match.Player newPlayer = gameMode.matches[i].player;
            //Debug.Log($"using {i} to spawn this player ({teams[(int)newPlayer.playerTeam - 2].Team})");
            IPlayer _newPlayer = OnCreatingPlayer (newPlayer, teams[(int) newPlayer.playerTeam]);
            PlacePieces (_newPlayer, piecePrefab);
            allPlayers.Add (_newPlayer);
        }

        return allPlayers.ToArray ();

    }

    public static IEnumerator TurnSystem () {
        IPlayer[] allPlayers = GameManager.allPlayers;
        int i = 0;
        while (true) {
            if (i == allPlayers.Length) i = 0;

            switch (allPlayers[i]) {

                case HumanPlayer human:
                    if (Input.GetMouseButtonDown (0))
                        UserManager.OnActionTaken (human);
                    break;
                case CompPlayer computer:
                    UserManager.OnActionTaken (computer);
                    break;
            }
            //yield return new WaitForSeconds (0.2f);
            if (allPlayers[i].EndTurn) {

                allPlayers[i].EndTurn = false;
                i++;
            }
            yield return null;

        }
    }

    /// <summary>
    /// Helper method that instantiates pieces on the correct nodes as well as saves said pieces in a private array per player.
    /// </summary>
    /// <param name="players"> The current player that owns the pieces. </param>
    /// <param name="piecePrefab">A prefab reference to create said pieces. </param>
    private static void PlacePieces (IPlayer player, PieceObject piecePrefab) {
        if (player == null) return;
        if (player.CurrentTeam == null || player.CurrentTeam.Team == Team.Empty) return;
        List<Piece> playerPieces = new List<Piece> ();
        Transform parent = new GameObject ($"{player}'s pieces").transform;
        foreach (var node in player.CurrentTeam.TeamBase) {
            if (node.BelongsTo == player.CurrentTeam.Team) {
                node.StoredPiece = new Piece (piecePrefab, TeamGenerator.SetColorBasedOnTeam (player.CurrentTeam.Team), node, player.CurrentTeam.Team, parent);
                //Piece.CreatePiece (piecePrefab, TeamGenerator.SetColorBasedOnTeam (player.CurrentTeam.Team), node, player.CurrentTeam.Team);
                playerPieces.Add (node.StoredPiece);
            }
        }

        player.CurrentTeam.TeamsPieces = playerPieces;

    }

}