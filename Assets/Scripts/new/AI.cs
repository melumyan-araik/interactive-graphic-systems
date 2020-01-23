using System;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor.VersionControl;
#endif
using UnityEngine;
using UnityEngine.UI;

public class AI : MonoBehaviour {

    public Button buttonAI;
    Stockfish stockfish;
    string str;

    AsyncOperation move;

    void Start()
    {
        stockfish = new Stockfish();
    }


    public void MoveAi()
    {
        str = MyBoardManager.Instance.chess.fen;

        str = stockfish.BestMove(str);

        MyBoardManager.Instance.figure = MyBoardManager.Instance.chess.GetFigureAt(
                    MyBoardManager.Instance.GetCoordinate("" + str[0] + str[1])[0],
                    MyBoardManager.Instance.GetCoordinate("" + str[0] + str[1])[1]
            ).ToString();

        string move = MyBoardManager.Instance.figure + str;
        if (move[0] == 'p' && move[4] == '1')
        {
            move = move + "q";
        }
        else if (move[0] == 'P' && move[4] == '8')
        {
            move = move + "Q";

        }
        var fen = MyBoardManager.Instance.chess.fen;

        MyBoardManager.Instance.chess = MyBoardManager.Instance.chess.Move(move);
        var newFen = MyBoardManager.Instance.chess.fen;

        if (fen != newFen)
        {
            MyBoardManager.Instance.SpawnAllChessman();
            MyBoardManager.Instance.IsCheckMate();
            MyBoardManager.Instance.click = 0;
            MyBoardManager.Instance.DestroyHighlight();
            MyBoardManager.Instance.color = MyBoardManager.Instance.color == "b" ? "w" : "b";
            str = "";
        }
    }

 
}
