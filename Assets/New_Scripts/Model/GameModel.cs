using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BoardModel;
public class GameModel {

    public static List<UserModel> StartNewGame (List<Player> desiredPlayers, PieceObject prefab) {

        //Create nessesary pieces
        //Create nessesarry players

        //Return said players and somehow store the pieces

        List<UserModel> players = new List<UserModel> ();

        foreach (Node node in originalBoard.boardArray) {
            if (desiredPlayers.Count == 2) {
                if (node.belongsTo == Team.BigRed) {
                    originalBoard.boardArray[node.pos.x, node.pos.y].belongsTo = Team.Red;
                }
                if (node.belongsTo == Team.BigGreen) {
                    originalBoard.boardArray[node.pos.x, node.pos.y].belongsTo = Team.Green;
                }
            }
        }

        for (int i = 0; i < desiredPlayers.Count; i++) {

            UserModel model = null;
            model = UserModel.CreatePlayer (desiredPlayers[i].isComputer, desiredPlayers[i].player);

            foreach (var node in originalBoard.boardArray) {

                if (node.belongsTo == model.currentTeam) {

                    Piece newPiece = new Piece (node.pos, node.belongsTo);
                    model.playerPieces.Add (newPiece);
                    int xPos = node.pos.y - node.pos.x / 2;
                    Vector2 objectPos = node.worldPos;
                    originalBoard.pieceViewArray[node.pos.x, node.pos.y] = PieceObject.CreatePieceObject (prefab, objectPos, (PieceColor) newPiece.belongsTo, model.transform, newPiece.pos);
                    originalBoard.pieceArray[node.pos.x, node.pos.y] = newPiece;
                }

            }

            players.Add (model);
        }
        return players;
    }

    public static IEnumerator GameRuntime (List<UserModel> players) {
        players[0].OnTurnTaken ();
        // while (true) {
        //     switch (players[0]) {

        //         case HumanPlayer human:
        //             human.OnTurnTaken ();
        //             break;
        //     }
        //     yield return null;
        // }
        yield return null;

    }
    private static UserModel NewPlayer (List<Player> desiredPlayers, int i) {
        UserModel model = null;
        switch (desiredPlayers[i].isComputer) {

            case true:
                model = new GameObject ($"Player {desiredPlayers[i].player}").AddComponent<ComputerPlayer> ();
                break;

            case false:
                model = new GameObject ($"Player {desiredPlayers[i].player}").AddComponent<HumanPlayer> ();
                break;
        }
        model.playerPieces = new List<Piece> ();
        model.currentTeam = desiredPlayers[i].player;

        return model;
    }
}