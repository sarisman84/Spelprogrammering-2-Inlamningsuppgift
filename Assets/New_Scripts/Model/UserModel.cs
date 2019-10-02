using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using static TestBoardModel;

public enum Team { None, Unoccupied, Red, Orange, Yellow, Green, Blue, Magenta, BigRed, BigGreen = 11 }
public abstract class UserModel : MonoBehaviour
{

    #region OldUserModel
    // public Team currentTeam;
    // public List<Piece> playerPieces;

    // public List<Vector2Int> opponentsBase;

    // public Vector2Int opponentGoal;

    // public bool HasWon => (opponentsBase != null) ? opponentsBase.All(pos => originalBoard.pieceArray[pos.x, pos.y] != null && originalBoard.pieceArray[pos.x, pos.y].belongsTo == currentTeam) : false;

    // public List<Node> GetPath(Node source, List<Node> savedResults, bool searchForGeneralMoves, bool highlight)
    // {
    //     for (int curDir = 0; curDir < 6; curDir++)
    //     {
    //         Node potentialNode = GetValidMove(source, savedResults, curDir);
    //         if (potentialNode == null) continue;
    //         if (originalBoard.GetPiece(potentialNode.pos) != null)
    //         {
    //             Node newSource = GetValidMove(potentialNode, savedResults, curDir);
    //             if (newSource == null) continue;
    //             if (originalBoard.GetPiece(newSource.pos) == null)
    //             {
    //                 savedResults.Add(newSource);
    //                 if (highlight)
    //                     originalBoard.boardViewArray[newSource.pos.x, newSource.pos.y].OnInteract("#ffcc99");
    //                 List<Node> newResults = GetPath(newSource, savedResults, false, highlight);
    //                 savedResults.AddRange(newResults);

    //             }
    //             continue;
    //         }
    //         if (searchForGeneralMoves)
    //         {
    //             savedResults.Add(potentialNode);
    //             if (highlight)
    //                 originalBoard.boardViewArray[potentialNode.pos.x, potentialNode.pos.y].OnInteract("#ffcc99");
    //         }

    //     }

    //     return savedResults;
    // }

    // Node GetValidMove(Node source, List<Node> savedResults, int index)
    // {
    //     if (source == null || OutOfBounds(source.pos)) return null;
    //     Vector2Int boardDir = source.pos + BoardDirection(source.pos, index);
    //     if (OutOfBounds(boardDir)) return null;
    //     Node foundNode = BoardModel.originalBoard.GetNode(boardDir);
    //     if (foundNode == source || foundNode.belongsTo == Team.None || savedResults.Any(p => p == foundNode)) return null;
    //     return foundNode;
    // }

    // private bool OutOfBounds(Vector2Int source)
    // {
    //     return (source.x >= BoardModel.originalBoard.boardArray.GetLength(0) || source.x < 0 || source.y >= BoardModel.originalBoard.boardArray.GetLength(1) || source.y < 0);
    // }

    // Vector2Int BoardDirection(Vector2Int source, int index)
    // {

    //     switch (index)
    //     {

    //         case 0:
    //             return new Vector2Int(0, -1); // Left

    //         case 1:
    //             return new Vector2Int(0, 1); // Right

    //         case 2:
    //             if (source.x % 2 == 0)
    //                 return new Vector2Int(-1, 0);
    //             return new Vector2Int(-1, 1); //Top Right
    //         case 3:
    //             if (source.x % 2 == 0)
    //                 return new Vector2Int(-1, -1);
    //             return new Vector2Int(-1, 0); //Top left

    //         case 4:
    //             if (source.x % 2 == 0)
    //                 return new Vector2Int(1, 0);
    //             return new Vector2Int(1, 1); //Bottom Right
    //         case 5:
    //             if (source.x % 2 == 0)
    //                 return new Vector2Int(1, -1);
    //             return new Vector2Int(1, 0); //Bottom Left
    //     }
    //     return Vector2Int.zero;
    // }

    // public static UserModel CreatePlayer(bool isComputer, Team team)
    // {

    //     if (isComputer)
    //     {
    //         ComputerPlayer computer = new GameObject($"Player {team}").AddComponent<ComputerPlayer>();
    //         computer.currentTeam = team;
    //         Team compOpponent = GetOpponent(team);
    //         computer.opponentsBase = GetTeamNodes(compOpponent, team);
    //         computer.playerPieces = new List<Piece>();
    //         computer.opponentGoal = GetBestGoal(GetPlayerPositions(team), GetPlayerPositions(compOpponent));
    //         return computer;
    //     }

    //     HumanPlayer human = new GameObject($"Player {team}").AddComponent<HumanPlayer>();
    //     human.currentTeam = team;
    //     Team humanOpponent = GetOpponent(team);
    //     human.opponentsBase = GetTeamNodes(humanOpponent, team);
    //     human.playerPieces = new List<Piece>();

    //     human.opponentGoal = GetBestGoal(GetPlayerPositions(team), GetPlayerPositions(humanOpponent));
    //     return human;

    // }






    // private static List<Vector2Int> GetPlayerPositions(Team desiredTeam)
    // {
    //     List<Vector2Int> team = new List<Vector2Int>();
    //     foreach (Node node in originalBoard.boardArray)
    //     {
    //         if (node.belongsTo == desiredTeam)
    //         {
    //             team.Add(node.pos);

    //         }
    //     }
    //     return team;
    // }



    // public static List<Vector2Int> GetPlayerPositions(Team desiredTeam, BoardModel.Board customBoard)
    // {
    //     List<Vector2Int> team = new List<Vector2Int>();
    //     foreach (Piece piece in customBoard.pieceArray)
    //     {
    //         if (piece != null && piece.belongsTo == desiredTeam)
    //         {
    //             team.Add(piece.pos);

    //         }
    //     }
    //     return team;
    // }



    // private static Vector2Int GetBestGoal(List<Vector2Int> searchArea, List<Vector2Int> targetArea)
    // {
    //     Vector2Int result = Vector2Int.zero;
    //     float dist = 0;
    //     foreach (Vector2Int firstPos in searchArea)
    //     {
    //         foreach (Vector2Int target in targetArea)
    //         {
    //             float temp = Vector2Int.Distance(firstPos, target);
    //             if (dist < temp)
    //             {
    //                 result = target;
    //                 dist = temp;
    //             }
    //         }
    //     }
    //     return result;
    // }
    // private static List<Vector2Int> GetTeamNodes(Team desiredTeam, Team color)
    // {
    //     List<Vector2Int> team = new List<Vector2Int>();
    //     foreach (Node node in originalBoard.boardArray)
    //     {
    //         if (node.belongsTo == desiredTeam)
    //         {
    //             team.Add(node.pos);
    //             originalBoard.GetVisualNode(node.pos).OnInteract(SetColor(color));

    //         }
    //     }
    //     return team;
    // }

    // private static List<Vector2Int> Test_GetTeamNodes(Team desiredTeam, Team color)
    // {
    //     List<Vector2Int> team = new List<Vector2Int>();
    //     foreach (TestNode node in test_OriginalBoard)
    //     {
    //         if (node.BelongsTo == desiredTeam)
    //         {
    //             team.Add(node.Pos);
    //         }
    //     }
    //     return team;
    // }

    // static string SetColor(Team team)
    // {
    //     switch (team)
    //     {

    //         case Team.Red:
    //             return "#ff5050";

    //         case Team.Orange:
    //             return "#ffbf00";

    //         case Team.Yellow:
    //             return "#ffd633";

    //         case Team.Green:
    //             return "#b3ff66";

    //         case Team.Blue:
    //             return "#0066ff";

    //         case Team.Magenta:
    //             return "#ff66ff";
    //     }
    //     return "";
    // }

    // public static UserModel GetOpponent(UserModel currentPlayer)
    // {
    //     foreach (var player in GameManagercs.allPlayers)
    //     {
    //         switch (currentPlayer.currentTeam)
    //         {

    //             case Team.Red:
    //                 if (player.currentTeam == Team.Green) return player;
    //                 break;

    //             case Team.Orange:
    //                 if (player.currentTeam == Team.Blue) return player;
    //                 break;

    //             case Team.Yellow:
    //                 if (player.currentTeam == Team.Magenta) return player;
    //                 break;

    //             case Team.Green:
    //                 if (player.currentTeam == Team.Red) return player;
    //                 break;

    //             case Team.Blue:
    //                 if (player.currentTeam == Team.Orange) return player;
    //                 break;

    //             case Team.Magenta:
    //                 if (player.currentTeam == Team.Yellow) return player;
    //                 break;
    //         }
    //     }
    //     return null;
    // }

    // protected void MovePiece(ref Piece selectedPiece, ref Node selectedNode, ref List<Node> path)
    // {
    //     if (selectedPiece == null || selectedNode == null) return;

    //     Piece pieceToMove = selectedPiece;
    //     Node target = selectedNode;
    //     this.playerPieces.Remove(selectedPiece);
    //     PieceObject pieceViewToMove = originalBoard.GetVisualPiece(pieceToMove.pos);
    //     selectedNode = null;
    //     selectedPiece = null;

    //     //Update pieceArray
    //     originalBoard.RemovePieceAt(pieceToMove.pos);
    //     pieceToMove.pos = target.pos;
    //     originalBoard.InsertPieceAt(pieceToMove.pos, pieceToMove);
    //     this.playerPieces.Add(pieceToMove);

    //     //Update Visuals
    //     originalBoard.RemovePieceViewAt(pieceViewToMove.currentBoardPosition);
    //     pieceViewToMove.currentBoardPosition = target.pos;
    //     pieceViewToMove.transform.position = target.worldPos;
    //     originalBoard.InsertPieceViewAt(pieceViewToMove.currentBoardPosition, pieceViewToMove);

    //     ResetPath(ref path);
    // }
    // protected void MovePiece(Piece selectedPiece, Node selectedNode)
    // {

    //     if (selectedPiece == null || selectedNode == null) return;

    //     Piece pieceToMove = selectedPiece;
    //     Node target = selectedNode;
    //     this.playerPieces.Remove(selectedPiece);
    //     PieceObject pieceViewToMove = originalBoard.GetVisualPiece(pieceToMove.pos);
    //     selectedNode = null;
    //     selectedPiece = null;

    //     //Update pieceArray
    //     originalBoard.RemovePieceAt(pieceToMove.pos);
    //     pieceToMove.pos = target.pos;
    //     pieceToMove.worldPos = target.worldPos;
    //     originalBoard.InsertPieceAt(pieceToMove.pos, pieceToMove);
    //     this.playerPieces.Add(pieceToMove);

    //     //Update Visuals
    //     originalBoard.RemovePieceViewAt(pieceViewToMove.currentBoardPosition);
    //     pieceViewToMove.currentBoardPosition = target.pos;
    //     pieceViewToMove.transform.position = target.worldPos;
    //     originalBoard.InsertPieceViewAt(pieceViewToMove.currentBoardPosition, pieceViewToMove);

    // }

    // public void SimulateMove(Piece selectedPiece, Node selectedNode, Board simulatedBoard)
    // {
    //     if (selectedPiece == null || selectedNode == null) return;

    //     //Update pieceArray
    //     simulatedBoard.RemovePieceAt(selectedPiece.pos);
    //     selectedPiece.pos = selectedNode.pos;
    //     simulatedBoard.InsertPieceAt(selectedPiece.pos, selectedPiece);

    // }

    // protected void ResetPath(ref List<Node> path)
    // {
    //     if (path != null)
    //     {
    //         foreach (Node nodeObj in path)
    //         {
    //             originalBoard.boardViewArray[nodeObj.pos.x, nodeObj.pos.y].OnInteract();
    //         }
    //         path.Clear();
    //     }
    // }

    // protected static Team GetOpponent(Team currentTeam)
    // {
    //     switch (currentTeam)
    //     {

    //         case Team.Red:
    //             return Team.Green;

    //         case Team.Orange:
    //             return Team.Blue;

    //         case Team.Yellow:
    //             return Team.Magenta;

    //         case Team.Green:
    //             return Team.Red;

    //         case Team.Blue:
    //             return Team.Orange;

    //         case Team.Magenta:
    //             return Team.Yellow;

    //     }
    //     return currentTeam;
    // }

    // public virtual void StartTurn()
    // {

    // }

    // public virtual void EndTurn()
    // {

    // }






    // public List<TestPiece> ownedPieces = new List<TestPiece>();

    // private static List<Vector2Int> Test_GetPlayerPositions(Team desiredTeam)
    // {
    //     List<Vector2Int> team = new List<Vector2Int>();
    //     foreach (TestNode node in test_OriginalBoard)
    //     {
    //         if (node.BelongsTo == desiredTeam)
    //         {
    //             team.Add(node.Pos);

    //         }
    //     }
    //     return team;
    // }

    // public static List<Vector2Int> Test_GetPlayerPositions(Team desiredTeam, List<TestPiece> customBoard)
    // {
    //     List<Vector2Int> team = new List<Vector2Int>();
    //     foreach (TestPiece piece in customBoard)
    //     {
    //         if (piece.BelongsTo == desiredTeam)
    //         {
    //             team.Add(piece.BoardPosition);
    //         }
    //     }
    //     return team;
    // }

    // public static UserModel Test_CreatePlayer(bool isComputer, Team team)
    // {

    //     if (isComputer)
    //     {
    //         ComputerPlayer computer = new GameObject($"Player {team}").AddComponent<ComputerPlayer>();
    //         computer.currentTeam = team;
    //         Team compOpponent = GetOpponent(team);
    //         computer.opponentsBase = GetTeamNodes(compOpponent, team);
    //         //computer.playerPieces = new List<Piece> ();
    //         computer.opponentGoal = GetBestGoal(Test_GetPlayerPositions(team), Test_GetPlayerPositions(compOpponent));
    //         return computer;
    //     }

    //     HumanPlayer human = new GameObject($"Player {team}").AddComponent<HumanPlayer>();
    //     human.currentTeam = team;
    //     Team humanOpponent = GetOpponent(team);
    //     human.opponentsBase = GetTeamNodes(humanOpponent, team);
    //     //human.playerPieces = new List<Piece> ();

    //     human.opponentGoal = GetBestGoal(Test_GetPlayerPositions(team), Test_GetPlayerPositions(humanOpponent));
    //     return human;

    // }
    #endregion


    public Team currentTeam;
    

    public abstract List<TestPiece> OwnedPieces { set; get; }
    public abstract List<PieceObject> VisualOwnedPieces { set; get; }

    #region Static Methods

    public static UserModel CreatePlayer(bool computerDriven, Team team)
    {

        if (computerDriven)
        {
            //Create Object
            ComputerPlayer computerEntity = new GameObject($"(AI) Player {team}").AddComponent<ComputerPlayer>();
            //Set its team
            computerEntity.currentTeam = team;
            //Set its target for minimax.
            computerEntity.desiredGoal = GetDesiredGoal(computerEntity);
            return computerEntity;
        }

        HumanPlayer normalPlayer = new GameObject($"Player {team}").AddComponent<HumanPlayer>();

        normalPlayer.currentTeam = team;

        return normalPlayer;
    }

    private static Vector2Int GetDesiredGoal(ComputerPlayer computerEntity)
    {
        Vector2Int[] ownedBase = SearchForBase(computerEntity.currentTeam);
        Vector2Int[] enemyBase = SearchForBase(GetOpponent(computerEntity));

        float dist = 0;
        Vector2Int target = Vector2Int.zero;
        for (int a = 0; a < ownedBase.Length; a++)
        {
            for (int b = 0; b < enemyBase.Length; b++)
            {
                float result = Vector2Int.Distance(ownedBase[a], enemyBase[b]);
                if (dist < result)
                {
                    dist = result;
                    target = enemyBase[b];
                }
            }
        }
        return target;
    }

    private static Vector2Int[] SearchForBase(Team computerEntity)
    {
        Vector2Int[] results = (TestGameModel.amountOfPlayers == 2) ? new Vector2Int[15] : new Vector2Int[10];
        int index = 0;
        for (int x = 0; x < test_OriginalBoard.GetLength(0); x++)
        {
            for (int y = 0; y < test_OriginalBoard.GetLength(1); y++)
            {
                TestNode foundNode = test_OriginalBoard[x, y];
                if (foundNode.belongsTo == computerEntity)
                {
                    results[index] = foundNode.pos;
                    index++;
                }
            }
        }

        return results;
    }


    public static Team GetOpponent(UserModel user)
    {
        switch (user.currentTeam)
        {

            case Team.Red:
                return Team.Green;

            case Team.Orange:
                return Team.Blue;

            case Team.Yellow:
                return Team.Magenta;

            case Team.Green:
                return Team.Red;

            case Team.Blue:
                return Team.Orange;

            case Team.Magenta:
                return Team.Yellow;
        }
        return Team.None;
    }





    #endregion

    public List<Vector2Int> PathOfMoves(Vector2Int source, List<Vector2Int> savedResults, bool isFirstSearch)
    {
        for (int curDir = 0; curDir < 6; curDir++)
        {
            Vector2Int potentialPath = AttemptToGetPath(source, curDir, savedResults);
            if (potentialPath == -Vector2Int.one) continue;
            if (globalPieceList.Any(p => p.pos == potentialPath))
            {
                Vector2Int branch = AttemptToGetPath(potentialPath, curDir, savedResults);
                if (branch == -Vector2Int.one) continue;
                if (!globalPieceList.Any(p => p.pos == branch))
                {
                    savedResults.Add(branch);
                    List<Vector2Int> newResults = PathOfMoves(branch, savedResults, false);
                    savedResults.AddRange(newResults);

                }
            }
            if (!globalPieceList.Any(p => p.pos == potentialPath))
            {
                if (isFirstSearch)
                {
                    savedResults.Add(potentialPath);
                }
            }

        }
        return savedResults;
    }

    private Vector2Int AttemptToGetPath(Vector2Int source, int curDir, List<Vector2Int> savedResults)
    {
        Vector2Int result = source + BoardDirection(source, curDir);
        if (OutOfBounds(result) || test_OriginalBoard[result.x, result.y].belongsTo == Team.None || savedResults.Any(p => p == result)) return -Vector2Int.one;
        return result;

    }

    private bool OutOfBounds(Vector2Int result) => (result.x >= test_OriginalBoard.GetLength(0) || result.y >= test_OriginalBoard.GetLength(1) || result.x < 0 || result.y < 0);


    Vector2Int BoardDirection(Vector2Int source, int index)
    {

        switch (index)
        {

            case 0:
                return new Vector2Int(0, -1); // Left

            case 1:
                return new Vector2Int(0, 1); // Right

            case 2:
                if (source.x % 2 == 0)
                    return new Vector2Int(-1, 0);
                return new Vector2Int(-1, 1); //Top Right
            case 3:
                if (source.x % 2 == 0)
                    return new Vector2Int(-1, -1);
                return new Vector2Int(-1, 0); //Top left

            case 4:
                if (source.x % 2 == 0)
                    return new Vector2Int(1, 0);
                return new Vector2Int(1, 1); //Bottom Right
            case 5:
                if (source.x % 2 == 0)
                    return new Vector2Int(1, -1);
                return new Vector2Int(1, 0); //Bottom Left
        }
        return Vector2Int.zero;
    }



}