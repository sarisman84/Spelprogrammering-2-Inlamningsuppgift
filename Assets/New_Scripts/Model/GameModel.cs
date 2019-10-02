using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using static TestBoardModel;

#region OldGameModel
// public class GameModel
// {


//     private static int currentPlayerIndex = 0;
//     public static List<UserModel> StartNewGame(List<Player> desiredPlayers, PieceObject prefab)
//     {

//         originalBoard.pieceArray = new Piece[originalBoard.boardArray.GetLength(0), originalBoard.boardArray.GetLength(1)];
//         foreach (var obj in originalBoard.pieceViewArray)
//         {
//             if (obj != null)
//                 MonoBehaviour.Destroy(obj.gameObject);
//         }

//         //Create nessesary pieces
//         //Create nessesarry players

//         //Return said players and somehow store the pieces

//         List<UserModel> players = new List<UserModel>();

//         foreach (Node node in originalBoard.boardArray)
//         {
//             if (desiredPlayers.Count == 2)
//             {
//                 if (node.belongsTo == Team.BigRed)
//                 {
//                     originalBoard.boardArray[node.pos.x, node.pos.y].belongsTo = Team.Red;
//                 }
//                 if (node.belongsTo == Team.BigGreen)
//                 {
//                     originalBoard.boardArray[node.pos.x, node.pos.y].belongsTo = Team.Green;
//                 }
//             }
//         }

//         for (int i = 0; i < desiredPlayers.Count; i++)
//         {

//             UserModel model = null;
//             model = UserModel.CreatePlayer(desiredPlayers[i].isComputer, desiredPlayers[i].player);

//             foreach (var node in originalBoard.boardArray)
//             {

//                 if (node.belongsTo == model.currentTeam)
//                 {

//                     Piece newPiece = new Piece(node.pos, node.belongsTo);
//                     model.playerPieces.Add(newPiece);
//                     Vector2 objectPos = node.worldPos;
//                     originalBoard.pieceViewArray[node.pos.x, node.pos.y] = PieceObject.CreatePieceObject(prefab, objectPos, (PieceColor)newPiece.belongsTo, model.transform, newPiece.pos);
//                     newPiece.worldPos = objectPos;
//                     originalBoard.pieceArray[node.pos.x, node.pos.y] = newPiece;
//                 }

//             }

//             players.Add(model);
//         }
//         return players;
//     }

//     public static List<UserModel> StartNewGame(SaveData data, PieceObject prefab)
//     {

//         List<Player> desiredPlayers = data.savedPlayers;
//         foreach (var player in GameObject.FindObjectsOfType<UserModel>())
//         {

//             GameObject.Destroy(player.gameObject);
//         }
//         if (GameManagercs.allPlayers != null)
//             GameManagercs.allPlayers.Clear();

//         //Create nessesary pieces
//         //Create nessesarry players

//         //Return said players and somehow store the pieces

//         List<UserModel> players = new List<UserModel>();

//         foreach (Node node in originalBoard.boardArray)
//         {
//             if (desiredPlayers.Count == 2)
//             {
//                 if (node.belongsTo == Team.BigRed)
//                 {
//                     originalBoard.boardArray[node.pos.x, node.pos.y].belongsTo = Team.Red;
//                 }
//                 if (node.belongsTo == Team.BigGreen)
//                 {
//                     originalBoard.boardArray[node.pos.x, node.pos.y].belongsTo = Team.Green;
//                 }
//             }
//         }

//         for (int i = 0; i < desiredPlayers.Count; i++)
//         {
//             UserModel model = UserModel.CreatePlayer(desiredPlayers[i].isComputer, desiredPlayers[i].player);

//             players.Add(model);
//         }
//         originalBoard.SetupLoadedBoard(data, players, prefab);

//         return players;
//     }
//     public static IEnumerator GameRuntime(List<UserModel> players)
//     {
//         // int i = 0;
//         // foreach (var player in players)
//         // {
//         //     switch (player)
//         //     {

//         //         case HumanPlayer human:
//         //             human.OnGameAwake();
//         //             break;

//         //         case ComputerPlayer computer:

//         //             computer.OnGameAwake();
//         //             break;
//         //     }
//         // }
//         // while (players.FindAll(p => p.HasWon == false).Count != 1)
//         // {

//         //     if (!isReady)
//         //     {
//         //         yield return null;
//         //         continue;
//         //     }
//         //     if (i >= players.Count) i = 0;
//         //     if (players.Count <= i) break;

//         //     switch (players[i])
//         //     {

//         //         case HumanPlayer human:
//         //             human.OnTurnTaken(ref i);
//         //             break;

//         //         case ComputerPlayer computer:

//         //             yield return new WaitForSeconds(0.1f);
//         //             computer.OnTurnTaken(ref i);
//         //             break;
//         //     }

//         // }
//         yield return null;

//     }



//     public static void GetNextTurn(List<UserModel> players)
//     {
//         //Debug.Log ($"{GameManagercs.allPlayers[currentPlayerIndex]}:{currentPlayerIndex}");

//         players[currentPlayerIndex].StartTurn();

//     }

//     public static void PlayerDone()
//     {
//         if (GameManagercs.allPlayers.FindAll(p => p.HasWon == false).Count == 1)
//         {
//             EndGameRuntime();
//             return;
//         }
//         //Debug.LogError("Player Done!");
//         currentPlayerIndex++;

//         if (currentPlayerIndex >= GameManagercs.allPlayers.Count) currentPlayerIndex = 0;
//         GameManagercs.instance.TurnEnded();
//     }

//     private static void EndGameRuntime()
//     {
//         //End the game.
//         Debug.Log("Game over!");
//     }

// }
#endregion

public static class TestGameModel
{

    public static int amountOfPlayers = 0;
    public static List<UserModel> StartNewGame(List<PlayerDefinition> players, PieceObject prefab)
    {
        amountOfPlayers = players.Count;
        if (players.Count == 2)
        {
            
            AdaptTheBoard();
        }
        List<UserModel> users = CreatePlayers(players);
        CreatePieces(users, prefab);
        return users;
    }



    #region Helper Methods
    private static List<UserModel> CreatePlayers(List<PlayerDefinition> players)
    {

        List<UserModel> result = new List<UserModel>();
        for (int i = 0; i < players.Count; i++)
        {
            PlayerDefinition player = players[i];
            UserModel user = UserModel.CreatePlayer(player.computerDriven, player.player);
            result.Add(user);

        }

        return result;
    }



    private static void CreatePieces(List<UserModel> players, PieceObject prefab)
    {
        Transform parent = new GameObject("Piece List").transform;
        List<TestPiece> allPieces = new List<TestPiece>();
        for (int i = 0; i < players.Count; i++)
        {
            for (int x = 0; x < test_OriginalBoard.GetLength(0); x++)
            {
                for (int y = 0; y < test_OriginalBoard.GetLength(1); y++)
                {
                    TestNode foundNode = test_OriginalBoard[x, y];
                    if (players[i].currentTeam == foundNode.belongsTo)
                    {
                        players[i].OwnedPieces.Add(new TestPiece(foundNode));
                        PieceObject piece = PieceObject.CreatePieceObject(prefab, foundNode.worldPos, (PieceColor)foundNode.belongsTo, parent, foundNode.pos);
                        players[i].VisualOwnedPieces.Add(piece);
                    }
                }
            }
            allPieces.AddRange(players[i].OwnedPieces);
        }
        globalPieceList = allPieces;


    }

    private static void AdaptTheBoard()
    {
        foreach (TestNode node in test_OriginalBoard)
        {
            TestNode foundNode = test_OriginalBoard.FindReference(node.pos);
            if (node.belongsTo == Team.BigRed)
            {
                test_OriginalBoard.boardArr[node.pos.x, node.pos.y].belongsTo = Team.Red;
            }
            if (node.belongsTo == Team.BigGreen)
            {
                test_OriginalBoard.boardArr[node.pos.x, node.pos.y].belongsTo = Team.Green;
            }

        }
    }
    #endregion
}