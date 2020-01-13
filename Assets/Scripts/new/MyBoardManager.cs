using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Chess;

public class MyBoardManager : MonoBehaviour
{

    public static MyBoardManager Instance
    {
        set; get;
    }
    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private Quaternion orientation = Quaternion.Euler(-90, 0, 0);

    private int selectionX = -1;
    private int selectionY = -1;

    public List<GameObject> chessmanSet;
    public GameObject highlight;
    public Chess.Chess chess;
    public int click;
    public string from;
    public string to;
    public string figure;
    public string color;

    void Start()
    {
        Instance = this;
        chess = new Chess.Chess();
        color = chess.fen.Split()[1];
        SpawnAllChessman();


    }

    void Update()
    {
        UpdateSelection();

        if (Input.GetMouseButtonDown(0))
        {
            switch (click)
            {
                case 0:
                    if (selectionX >= 0 && selectionY >= 0 && chess.fen.Split()[1] == color && chess.GetAllMoves().Count != 0)
                    {
                        from = GetSquare();
                        figure = chess.GetFigureAt(selectionX, selectionY).ToString();
                        var posMoves = PossibleMoves(figure, from);
                        DrawAllHighlight(ref posMoves);
                        click++;
                    }
                    break;
                case 1:
                    if (selectionX >= 0 && selectionY >= 0 && chess.fen.Split()[1] == color)
                    {
                        to = GetSquare();
                        string move;
                        if (figure == "p" && to[1] == '1')
                        {
                            move = figure + from + to + "q";
                        }
                        else if (figure == "P" && to[1] == '8')
                        {
                            move = figure + from + to + "Q";

                        }
                        else
                        {
                            move = figure + from + to;
                        }

                        var fen = chess.fen;
                        chess = chess.Move(move);
                        var newFen = chess.fen;

                        if (fen != newFen)
                        {
                            SpawnAllChessman();
                            IsCheckMate();
                            click = 0;
                            DestroyHighlight();
                            color = color == "b" ? "w" : "b";

                        }
                    }
                    break;

                default:
                    click = 0;
                    DestroyHighlight();
                    break;
            }

        }
        else if (Input.GetMouseButtonDown(1))
        {
            click = 0;
            DestroyHighlight();
            SpawnAllChessman();
        }
    }

    string GetBestMove(string forsythEdwardsNotationString)
    {
        //var bestMoveInAlgebraicNotation = p.StandardOutput.ReadLine();
        //int n = bestMoveInAlgebraicNotation.IndexOf("bestmove");
        //if (n != -1)
        //    bestMoveInAlgebraicNotation = bestMoveInAlgebraicNotation.Substring(n + 9, 4);

        var p = new System.Diagnostics.Process();
        p.StartInfo.FileName = Directory.GetCurrentDirectory() + "\\Assets\\stockfish\\stockfish.exe";
        p.StartInfo.CreateNoWindow = true;
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.Start();
        String setupString = "position fen " + forsythEdwardsNotationString;
        p.StandardInput.WriteLine(setupString);

        // Process for 5 seconds
        String processString = "go movetime 5000";

        // Process 20 deep
        // String processString = "go depth 20";

        p.StandardInput.WriteLine(processString);

        String bestMoveInAlgebraicNotation = p.StandardOutput.ReadLine();

        p.Close();

        return bestMoveInAlgebraicNotation;

    }

    private List<string> PossibleMoves(string fig, string from)
    {
        List<string> a = new List<string>();

        foreach (var i in chess.GetAllMoves())
        {
            if (i.IndexOf(fig + from) != -1)
                a.Add(i);
        }

        return a;
    }

    private string ChessToAscii(Chess.Chess chess)
    {
        string text = "  +----------------+\n";
        for (int y = 7; y >= 0; y--)
        {
            text += y + 1;
            text += " | ";
            for (int x = 0; x < 8; x++)
            {
                text += chess.GetFigureAt(x, y) + " ";
            }
            text += "|\n";
        }
        text += "  +----------------+\n";
        text += "    a b c d e f g h\n";
        return text;
    }

    private void DestroyAllChessman()
    {
        GameObject board = GameObject.Find("ChessBoard");
        foreach (Transform child in board.transform)
        {
            if (child.name == "Board")
                continue;
            Destroy(child.gameObject);
        }
    }

    public void DrawAllHighlight(ref List<string> moves)
    {
        foreach (var i in moves)
        {
            var m = GetCoordinate("" + i[3] + i[4]);
            DrawHighlight(m[0], m[1]);
        }
    }

    private void DrawHighlight(int x, int y)
    {
        GameObject go = Instantiate(highlight, GetTitleCenter(x, y), Quaternion.Euler(0, 0, 0)) as GameObject;
        go.transform.SetParent(transform);
    }

    public void DestroyHighlight()
    {
        GameObject board = GameObject.Find("ChessBoard");
        foreach (Transform child in board.transform)
        {
            if (child.name == "Highlight")
                Destroy(child.gameObject);
        }
    }

    private void SpawnChessman(string fig, int x, int y)
    {
        int index = FigureToIndexChessman(fig);
        GameObject go = Instantiate(chessmanSet[index], GetTitleCenter(x, y), orientation) as GameObject;
        go.transform.SetParent(transform);

    }

    public void SpawnAllChessman()
    {
        DestroyAllChessman();
        for (int y = 0; y < 8; y++)
            for (int x = 0; x < 8; x++)
            {
                string figure = chess.GetFigureAt(x, y).ToString();
                if (figure == ".")
                    continue;
                SpawnChessman(figure, x, y);
            }
    }

    private string GetNameFigureGameObj(string fig)
    {
        switch (fig)
        {
            case "K":
                return "KingWhite";
            case "Q":
                return "QueenWhite";
            case "R":
                return "RookWhite";
            case "B":
                return "BishopWhite";
            case "N":
                return "KnightWhite";
            case "P":
                return "PawnWhite";
            case "k":
                return "KingBlack";
            case "q":
                return "QueenBlack";
            case "r":
                return "RookBlack";
            case "b":
                return "BishopBlack";
            case "p":
                return "PawnBlack";
        }
        return "";
    }

    private int FigureToIndexChessman(string fig)
    {
        switch (fig)
        {
            case "K":
                return 0;
            case "Q":
                return 1;
            case "R":
                return 2;
            case "B":
                return 3;
            case "N":
                return 4;
            case "P":
                return 5;
            case "k":
                return 6;
            case "q":
                return 7;
            case "r":
                return 8;
            case "b":
                return 9;
            case "n":
                return 10;
            case "p":
                return 11;
        }
        return -1;
    }

    private string GetSquare()
    {
        selectionX = Convert.ToInt32(selectionX);
        selectionY = Convert.ToInt32(selectionY);

        return ((char)('a' + selectionX)).ToString() + (selectionY + 1).ToString();
    }

    public int[] GetCoordinate(string move)
    {
        return new int[2] { Convert.ToInt32((char)(move[0]) - 'a'), Convert.ToInt32(move[1] - '0') - 1 };
    }

    private Vector3 GetTitleCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x = (TILE_SIZE * x) + TILE_OFFSET;
        origin.z = (TILE_SIZE * y) + TILE_OFFSET;

        return origin;
    }

    private void UpdateSelection()
    {
        if (!Camera.main)
            return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("ChessPlane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    public Text LostText;
    public Text CheckText;
    public Button RestartBtn;
    public Button ExitBtn;
    public Button AI;

    public void IsCheckMate()
    {
        if (chess.IsShah() && chess.GetAllMoves().Count == 0)
        {
            LostText.gameObject.SetActive(true);
            RestartBtn.gameObject.SetActive(true);
            ExitBtn.gameObject.SetActive(true);
            AI.gameObject.SetActive(false);
        }
        else if (chess.IsShah())
        {
            CheckText.gameObject.SetActive(true);
        }
        else
        {
            CheckText.gameObject.SetActive(false);
        }
    }

}
