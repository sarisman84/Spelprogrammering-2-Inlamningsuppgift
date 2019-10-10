using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
//This class handles the save data as well as defines what "save data" really is.
public class SaveData {

    //Since we cant convert Vector2ints and Vector2s into binary, i have created a custom class that converts said data into something acceptable.
    public readonly List<PieceData> savedBoard; // <-- Talking bout this.

    public readonly List<PlayerDefinition> savedPlayers;

    public readonly int savedTurn;

    public SaveData (List<BoardPiece> board, List<UserModel> players) {
        savedPlayers = new List<PlayerDefinition> ();
        savedBoard = new List<PieceData> ();
        for (int x = 0; x < board.Count; x++) {
            savedBoard.Add (new PieceData (board[x]));
        }

        foreach (var player in players) {
            PlayerDefinition user = new PlayerDefinition (player);
            savedPlayers.Add (user);

        }

        savedTurn = TestGameModel.currPlayerIndex;
    }

    public SaveData () { }

}


//A conversion to a bootleg Vector2Int struct.
[Serializable]
public struct Vec2Int {
    public int x, y;

    public Vec2Int (Vector2Int value) {
        x = value.x;
        y = value.y;
    }
}


//A conversion to a bootleg Vector2 struct.
[Serializable]
public struct Vec2 {
    public float x, y;
    public Vec2 (Vector2 value) {
        x = value.x;
        y = value.y;
    }
}


//A conversion to a bootleg BoardPiece data.
[Serializable]
public class PieceData {

    public PieceData (BoardPiece piece) {
        boardPos = new Vec2Int (piece.pos);
        worldPos = new Vec2 (piece.worldPos);
        belongsTo = piece.belongsTo;
    }

    public Vec2Int boardPos;
    public Vec2 worldPos;
    public Team belongsTo;

}

public static class DataHandler {

    // These variables store the save location and names of the file.
    private static readonly string dataPath = $"{Application.persistentDataPath}";

    private static readonly string folderName = "Save", fileName = "SaveData.save";

    private static readonly string filePath = Path.Combine (dataPath, folderName, fileName);
   
   /// <summary>
   /// Mainline method: Saves information into a file via Binary Formatter.
   /// </summary>
   /// <returns>If said save attempt was succesfull, return true.</returns>
    public static bool Save () {
        Debug.Log (Path.Combine (dataPath, folderName));
        if (!FolderExists ()) {
            DirectoryInfo dir = Directory.CreateDirectory (Path.Combine (dataPath, folderName));

        }

        //We feed in the current board, player and piece data and store it in a temporary variable.
        SaveData data = new SaveData (TestBoardModel.globalPieceList, TestManager.ins.allPlayers);
        using (FileStream fs = new FileStream (filePath, FileMode.OpenOrCreate)) { 
            BinaryFormatter formatter = new BinaryFormatter ();
            //We serialize said data into the file that we just created and return true as it succeded.
            formatter.Serialize (fs, data);
            Debug.Log ("Save Successfull");
            return true;
        }

    }

    /// <summary>
    /// A check to see if the folder in said path exists,
    /// </summary>
    /// <returns></returns>
    private static bool FolderExists () {
        return Directory.Exists (dataPath + "/" + folderName);
    }

    /// <summary>
    /// Mainline method: Loads the file and extracts the data using a BinaryFortmatter
    /// </summary>
    /// <returns>If said load attempt was successfull, return true</returns>
    public static bool Load () {

        //Either the folder or file dont exist, return false to show a failed attempt.
        if (!FolderExists () || !File.Exists (filePath)) {
            return false;
        }
        using (FileStream fs = new FileStream (filePath, FileMode.Open)) { 
            BinaryFormatter formatter = new BinaryFormatter ();
            SaveData data = formatter.Deserialize (fs) as SaveData;
            //Using the same logic, we desearlise the information from the file into the class.
            Debug.Log ("Load Successfull");

            //We then feed in the newrly aquired information to restart the game.
            GameManagercs.allPlayers = TestManager.ins.allPlayers = TestGameModel.StartNewGame (data, TestManager.ins.piecePrefab);
            TestGameModel.GetNextTurn (TestManager.ins.allPlayers, TestManager.ins.turnText);
            return true;
        }

    }
}