using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IController
{
    Node SelectedNode { get; }
    Node AttemptToGetPiece(Node detectedNode, Node.Team currentTeam);
}
