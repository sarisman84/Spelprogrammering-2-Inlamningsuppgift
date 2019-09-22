
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static BoardModel;
public class GameModel
{

    public static List<UserModel> StartNewGame(List<Player> desiredPlayers, PieceObject prefab)
    {

        //Create nessesary pieces
        //Create nessesarry players

        //Return said players and somehow store the pieces



        List<UserModel> players = new List<UserModel>();

        foreach (Node node in originalBoard.boardArray)
        {
            if (desiredPlayers.Count == 2)
            {
                if (node.belongsTo == Team.BigRed)
                {
                    originalBoard.boardArray[node.currentPosition.x, node.currentPosition.y].belongsTo = Team.Red;
                }
                if (node.belongsTo == Team.BigGreen)
                {
                    originalBoard.boardArray[node.currentPosition.x, node.currentPosition.y].belongsTo = Team.Green;
                }
            }
        }

        for (int i = 0; i < desiredPlayers.Count; i++)
        {


            UserModel model = null;
            model = NewPlayer(desiredPlayers, i);

            foreach (var node in originalBoard.boardArray)
            {

                if (node.belongsTo == model.currentTeam)
                {

                    Piece newPiece = new Piece(node.currentPosition, node.belongsTo);
                    model.playerPieces.Add(newPiece);
                    int xPos = node.currentPosition.y - node.currentPosition.x / 2;
                    Vector2 objectPos = node.worldPosition;
                    originalBoard.pieceViewArray[node.currentPosition.x, node.currentPosition.y] = PieceObject.CreatePieceObject(prefab, objectPos, (PieceColor)newPiece.belongsTo, model.transform, newPiece.currentPosition);
                    originalBoard.pieceArray[node.currentPosition.x, node.currentPosition.y] = newPiece;
                }

            }

            players.Add(model);
        }
        return players;
    }


    public static IEnumerator GameRuntime(List<UserModel> players)
    {
        while (true)
        {
            foreach (var player in players)
            {
                player.OnTurnTaken();
            }
            yield return null;
        }


    }
    private static UserModel NewPlayer(List<Player> desiredPlayers, int i)
    {
        UserModel model = null;
        switch (desiredPlayers[i].isComputer)
        {

            case true:
                model = new GameObject($"Player {desiredPlayers[i].player}").AddComponent<ComputerPlayer>();
                break;

            case false:
                model = new GameObject($"Player {desiredPlayers[i].player}").AddComponent<HumanPlayer>();
                break;
        }
        model.playerPieces = new List<Piece>();
        model.currentTeam = desiredPlayers[i].player;

        return model;
    }
}
