using System.Collections.Generic;
using UnityEngine;

using static TestBoardModel;



public static class TestGameModel
{


    //Some basic variables to have the class keep track of.

    public static int amountOfPlayers = 0;

    static BoardNode[,] savedBoard;



    /// <summary>
    /// Mainline method: Creates a new match
    /// </summary>
    /// <param name="players">The amount of players participating, this includes what team each player will be as well as if they are computers.</param>
    /// <param name="prefab">A reference to a view piece prefab for spawning each player's pieces.</param>
    /// <returns>A list of active players that are participating in a match.</returns>
    public static List<UserModel> StartNewGame(List<PlayerDefinition> players, PieceObject prefab)
    {
        ResetGame(0);
        amountOfPlayers = players.Count;
        if (players.Count == 2)
        {

            AdaptTheBoard();
        }
        List<UserModel> users = CreatePlayers(players);
        CreatePieces(users, prefab);
        return users;
    }

    /// <summary>
    /// Helper method: Resets the game
    /// </summary>
    /// <param name="customIndex">Which player starts off.</param>
    private static void ResetGame(int customIndex)
    {
        NodeObject.ResetInteractions();
        TestManager.ins.ResetVictoryText();
        if (savedBoard != null)
            test_OriginalBoard.boardArr = savedBoard;
        savedBoard = null;
        if (TestManager.ins.allPlayers.Count != 0)
        {
            TestManager.ins.allPlayers.Clear();

        }
        foreach (var item in MonoBehaviour.FindObjectsOfType<UserModel>())
        {
            MonoBehaviour.Destroy(item.gameObject);
        }
        foreach (var item in MonoBehaviour.FindObjectsOfType<PieceObject>())
        {
            MonoBehaviour.Destroy(item.gameObject);
        }
        currPlayerIndex = customIndex;
    }

    /// <summary>
    /// Mainline: Starts a new game based on savedData
    /// </summary>
    /// <param name="data">The saved data used as a reference to create said game.</param>
    /// <param name="prefab">A prefab reference for spawning in a view piece for each player</param>
    /// <returns></returns>
    public static List<UserModel> StartNewGame(SaveData data, PieceObject prefab)
    {
        ResetGame(data.savedTurn);
        List<PlayerDefinition> players = data.savedPlayers;
        amountOfPlayers = players.Count;
        if (players.Count == 2)
        {

            AdaptTheBoard();
        }
        List<UserModel> users = CreatePlayers(players);
        CreatePieces(users, prefab, data);


        return users;
    }

    #region Helper Methods

    /// <summary>
    /// Helper method: Creates players based on desired data.
    /// </summary>
    /// <param name="players">A reference to the desired data in order to create the players needed.</param>
    /// <returns>A list of active players.</returns>
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

    /// <summary>
    /// Helper method: Creates view pieces depending on player.
    /// </summary>
    /// <param name="players">A list of active players</param>
    /// <param name="prefab">A prefab reference.</param>
    private static void CreatePieces(List<UserModel> players, PieceObject prefab)
    {
        Transform parent = new GameObject("Piece List").transform;
        globalPieceList = new List<BoardPiece>();

        for (int i = 0; i < players.Count; i++)
        {
            players[i].OwnedViewPieces = new List<PieceObject>();
            for (int x = 0; x < test_OriginalBoard.GetLength(0); x++)
            {
                for (int y = 0; y < test_OriginalBoard.GetLength(1); y++)
                {
                    BoardNode foundNode = test_OriginalBoard[x, y];
                    if (players[i].currentTeam == foundNode.belongsTo)
                    {
                        globalPieceList.Add(new BoardPiece(foundNode));
                        PieceObject piece = PieceObject.CreatePieceObject(prefab, foundNode.worldPos, (PieceColor)foundNode.belongsTo, parent, foundNode.pos);
                        players[i].OwnedViewPieces.Add(piece);

                    }
                }
            }

        }


    }

    /// <summary>
    /// Helper method: Creates view pieces depending on player with information form a saved data file 
    /// </summary>
    /// <param name="players">A list of active players</param>
    /// <param name="prefab">A prefab reference</param>
    /// <param name="data">A data file reference</param>
    private static void CreatePieces(List<UserModel> players, PieceObject prefab, SaveData data)
    {
        //This method is an overload of the original CreatePieces method.
        //It uses data info to create pieces with their positions and teams intact.
        Transform parent = new GameObject("Piece List").transform;
        globalPieceList = new List<BoardPiece>();
        for (int i = 0; i < players.Count; i++)
        {
            players[i].OwnedViewPieces = new List<PieceObject>();
            foreach (var item in data.savedBoard)
            {
                if (item.belongsTo == players[i].currentTeam)
                {
                    BoardNode foundNode = new BoardNode(new Vector2Int(item.boardPos.x, item.boardPos.y), new Vector2(item.worldPos.x, item.worldPos.y), item.belongsTo);
                    globalPieceList.Add(new BoardPiece(foundNode));
                    PieceObject piece = PieceObject.CreatePieceObject(prefab, foundNode.worldPos, (PieceColor)foundNode.belongsTo, parent, foundNode.pos);
                    players[i].OwnedViewPieces.Add(piece);

                }

            }


        }


    }

    /// <summary>
    /// Changes the board to fit the 2 player format. (saves the original board in a seperate variable).
    /// </summary>
    private static void AdaptTheBoard()
    {
        savedBoard = test_OriginalBoard.SaveBoard();
        Debug.Log(test_OriginalBoard.Length);
        foreach (BoardNode node in test_OriginalBoard)
        {
            BoardNode foundNode = test_OriginalBoard.FindReference(node.pos);
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

    //The following code is representing a game runtime.


    public static int currPlayerIndex = 0;

    /// <summary>
    /// Gets called whenever a player is done in their previous turn (or gets called at the beginning of the game via TestManager)
    /// </summary>
    /// <param name="players">A list of active players</param>
    /// <param name="sign">A reference to a text UI element to show the players turn.</param>
    public static void GetNextTurn(List<UserModel> players, TMPro.TMP_Text sign)
    {
        
        if (players[currPlayerIndex].HasPlayerWon()) NextPlayer(players);
        sign.text = $"Turn: {players[currPlayerIndex].currentTeam}";
        if(players[currPlayerIndex].currentTeam == Team.Red && players[currPlayerIndex].GetType() != typeof(ComputerPlayer)) sign.text += " (You!)";
        sign.color = UtilityClass.GetColor((UtilityClass.tColor)players[currPlayerIndex].currentTeam);
        players[currPlayerIndex].StartTurn();

    }

    /// <summary>
    /// Helper method: Gets the next player within the list.
    /// </summary>
    /// <param name="players">List of active players</param>
    private static void NextPlayer(List<UserModel> players)
    {
        currPlayerIndex++;
        currPlayerIndex = (currPlayerIndex >= players.Count || players == null || currPlayerIndex < 0) ? 0 : currPlayerIndex;
    }

    /// <summary>
    /// Gets called whenever someones turn is over.
    /// </summary>
    public static void PlayerDone()
    {
        if (TestManager.ins.allPlayers.FindAll(p => p.HasPlayerWon() == false).Count == 1)
        {
            EndGameRuntime(TestManager.ins.allPlayers.FindAll(p => p.HasPlayerWon() == true));
            return;
        }
        //Debug.LogError("Player Done!");
        NextPlayer(TestManager.ins.allPlayers);

        TestManager.ins.TurnEnded();
    }

    /// <summary>
    /// Gets called when all but one player has not won which will end the loop.
    /// </summary>
    /// <param name="winners">The list of all players that have won!</param>
    private static void EndGameRuntime(List<UserModel> winners)
    {
        //End the game.
        string message = "<Winners: ";
        for (int i = 0; i < winners.Count; i++)
        {
            
            if(i == winners.Count - 1){
                message += $"{winners[i].currentTeam}";
                continue;
            }
            message += $"{winners[i].currentTeam},";

        }
        message += ">";
        TestManager.ins.InsertVictoryMessage(message);
    }

    #endregion
}