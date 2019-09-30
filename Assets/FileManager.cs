
using System.Linq;
using System.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class FileManager : MonoBehaviour
{


    DirectoryInfo currDir;
    string textFilePath = @"Assets\Resources\saveFile.text";
    public void SaveCurrentGame()
    {

        DataHandler.Save();
        // string savedGameState = "";

        // //Create a new Directory
        // currDir = currDir ?? new DirectoryInfo(@"Assets\Resources");

        // //Save the data needed to a string format.

        // savedGameState = SaveState(savedGameState);

        // Debug.Log(savedGameState);

        // if (File.Exists(textFilePath))
        // {
        //     File.Delete(textFilePath);
        // }
        // using (FileStream fs = File.Create(textFilePath))
        // {
        //     byte[] rsByteArr = Encoding.Default.GetBytes(savedGameState);
        //     fs.Write(rsByteArr, 0, rsByteArr.Length);

        // }



        //Saves Game
    }

    private static string SaveState(string savedGameState)
    {
        savedGameState += $"|M{GameManagercs.selectedMode}|";

        //Save each position in the pieceArray.
        foreach (Piece piece in BoardModel.originalBoard.pieceArray)
        {
            if (piece == null) continue;
            savedGameState += $"|T{(int)piece.belongsTo}X{piece.pos.x}Y{piece.pos.y}-wX{piece.worldPos.x}wY{piece.worldPos.y}|\n";
        }

        return savedGameState;
    }

    public void LoadLatestGame()
    {
        //Loads Game

        DataHandler.Load();
        // string foundData = "";
        // currDir = currDir ?? new DirectoryInfo(@"Assets\Resources");
        // using (FileStream fs = File.Open(textFilePath, FileMode.Open))
        // {
        //     byte[] loadedByteArr = new byte[fs.Length];

        //     for (int i = 0; i < fs.Length; i++)
        //     {
        //         loadedByteArr[i] = (byte)fs.ReadByte();
        //     }
        //     foundData = Encoding.Default.GetString(loadedByteArr);
        // }

        // string modePattern = @"\d{1}";
        // Match mode = Regex.Match(foundData, modePattern);


        // GameManagercs.allPlayers = GameModel.StartNewGame(GetComponent<GameManagercs>().mode[int.Parse(mode.Value)].ammOfPlayers, GetComponent<GameManagercs>().piecePrefab);

        // string coordinatePattern = @"\D\d\D\d\D\d";

        // MatchCollection result = Regex.Matches(foundData, coordinatePattern);
        // foreach (var value in result)
        // {
        //     int xPos = int.Parse(new String(Regex.Match(value.ToString(), @"X\d").ToString().Where(Char.IsDigit).ToArray()));

        //     int yPos = int.Parse(new String(Regex.Match(value.ToString(), @"Y\d").ToString().Where(Char.IsDigit).ToArray()));

        //     int team = int.Parse(new String(Regex.Match(value.ToString(), @"T\d").ToString().Where(Char.IsDigit).ToArray()));
        //     Debug.Log($"Owned by: {(Team)team}. Lies in:({xPos},{yPos})");
        // }





    }
}
