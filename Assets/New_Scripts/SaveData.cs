using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class SaveData
{
    public readonly PieceData[,] savedBoard;

    public readonly List<Player> savedPlayers;

    public SaveData(TestPiece[,] board, List<UserModel> players)
    {
        savedPlayers = new List<Player>();
        savedBoard = new PieceData[board.GetLength(0), board.GetLength(1)];
        for (int x = 0; x < board.GetLength(0); x++)
        {
            for (int y = 0; y < board.GetLength(1); y++)
            {

                savedBoard[x, y] = new PieceData(board[x, y]);
            }
        }

        foreach (var player in players)
        {
            Player user = new Player(player);
            savedPlayers.Add(user);

        }
    }

    public SaveData() { }

}

[Serializable]
public struct Vec2Int
{
    public int x, y;

    public Vec2Int(Vector2Int value)
    {
        x = value.x;
        y = value.y;
    }
}

[Serializable]
public struct Vec2
{
    public float x, y;
    public Vec2(Vector2 value)
    {
        x = value.x;
        y = value.y;
    }
}

[Serializable]
public class PieceData
{

    public PieceData(TestPiece piece)
    {
        boardPos = new Vec2Int(piece.pos);
        worldPos = new Vec2(piece.worldPos);
        belongsTo = piece.belongsTo;
    }


    public Vec2Int boardPos;
    public Vec2 worldPos;
    public Team belongsTo;

}

public static class DataHandler
{
    private static readonly string dataPath = $"{Application.persistentDataPath}";

    private static readonly string folderName = "Save", fileName = "SaveData.save";

    private static readonly string filePath = Path.Combine(dataPath, folderName, fileName);
    public static bool Save()
    {
        Debug.Log(Path.Combine(dataPath, folderName));
        if (!FolderExists())
        {
            DirectoryInfo dir = Directory.CreateDirectory(Path.Combine(dataPath, folderName));

        }
        // if (!File.Exists (filePath)) {
        //     File.Create (filePath);
        // }
        SaveData data =  new SaveData();//new SaveData(BoardModel.originalBoard.pieceArray, GameManagercs.allPlayers);
        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
        { //(filePath)) {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, data);
            Debug.Log("Save Successfull");
            return true;
        }

    }

    private static bool FolderExists()
    {
        return Directory.Exists(dataPath + "/" + folderName);
    }

    public static bool Load()
    {

        if (!FolderExists() || !File.Exists(filePath))
        {
            return false;
        }
        using (FileStream fs = new FileStream(filePath, FileMode.Open))
        { //(filePath)) {
            BinaryFormatter formatter = new BinaryFormatter();
            SaveData data = formatter.Deserialize(fs) as SaveData;
            Debug.Log("Load Successfull");
           // GameManagercs.allPlayers = GameModel.StartNewGame(data, GameManagercs.instance.piecePrefab);
            return true;
        }

    }
}