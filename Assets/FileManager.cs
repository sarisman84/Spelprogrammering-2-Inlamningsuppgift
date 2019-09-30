
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


    public void OnSaveEvent()
    {
        DataHandler.Save();
    }



    public void OnLoadEvent()
    {
        DataHandler.Load();
        if (GameManagercs.allPlayers != null)
            StartCoroutine(GameModel.GameRuntime(GameManagercs.allPlayers));

    }
}
