using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class FileManager : MonoBehaviour {


    public void OnSaveEvent () {
        DataHandler.Save ();
    }

    public void OnLoadEvent () {
        DataHandler.Load ();
        if (TestManager.ins.allPlayers != null && TestManager.ins.allPlayers.Count != 0) {

           
        }

    }
}