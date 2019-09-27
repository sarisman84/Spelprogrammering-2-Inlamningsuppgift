using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
                    originalBoard.boardArray[node.pos.x, node.pos.y].belongsTo = Team.Red;
                }
                if (node.belongsTo == Team.BigGreen)
                {
                    originalBoard.boardArray[node.pos.x, node.pos.y].belongsTo = Team.Green;
                }
            }
        }

        for (int i = 0; i < desiredPlayers.Count; i++)
        {

            UserModel model = null;
            model = UserModel.CreatePlayer(desiredPlayers[i].isComputer, desiredPlayers[i].player);

            foreach (var node in originalBoard.boardArray)
            {

                if (node.belongsTo == model.currentTeam)
                {

                    Piece newPiece = new Piece(node.pos, node.belongsTo);
                    model.playerPieces.Add(newPiece);
                    int xPos = node.pos.y - node.pos.x / 2;
                    Vector2 objectPos = node.worldPos;
                    originalBoard.pieceViewArray[node.pos.x, node.pos.y] = PieceObject.CreatePieceObject(prefab, objectPos, (PieceColor)newPiece.belongsTo, model.transform, newPiece.pos);
                    originalBoard.pieceArray[node.pos.x, node.pos.y] = newPiece;
                }

            }

            players.Add(model);
        }
        return players;
    }

    public static IEnumerator GameRuntime(List<UserModel> players)
    {
        int i = 0;

        while (players.FindAll(p => p.HasWon == false).Count != 1)
        {
            if (i >= players.Count) i = 0;
            switch (players[i])
            {

                case HumanPlayer human:
                    human.OnTurnTaken(ref i);
                    break;

                case ComputerPlayer computer:
                    computer.OnTurnTaken(ref i);
                    yield return new WaitForSeconds(0.1f);
                    break;
            }

        }
        yield return null;

    }
   
}