using System;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance
    {
        set; get;
    }

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int selectionX = -1;
    private int selectionY = -1;

    public List<GameObject> chessmanPrefabs;
    private List<GameObject> activeChessman;
    private Quaternion orientation = Quaternion.Euler(-90, 0, 0);
    public Chessman[,] Chessmans
    {
        set; get;
    }

    private Chess.Chess chess;
    private int click;
    private string from;
    private string to;
    private string figure;

    private void Start()
    {
        Instance = this;
        chess = new Chess.Chess();
        click = 0;

        //ShowFigure();
    }

    private int tmpX, tmpY;

    private void Update()
    {
        UpdateSelection();
        // DrawChessboard();

        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (selectionX >= 0 && selectionY >= 0)
        //    {
        //        switch (click)
        //        {
        //            case 0:
        //                tmpX = selectionX;
        //                tmpY = selectionY;
        //                from = GetSquare();
        //                figure = chess.GetFigureAt(selectionX, selectionY).ToString();
        //                click++;
        //                break;
        //            case 1:
        //                to = GetSquare();
        //                string move = figure + from + to;
        //                chess = chess.Move(move);
        //                MoveChessman(figure, tmpX, tmpY);
        //                click = 0;
        //                break;

        //            default:
        //                click = 0;
        //                break;
        //        }
        //    }

        //}

        PlaceFigure("K", 0, 0);
        if (Input.GetMouseButtonDown(0))
        {
            MoveChessman("K", 3, 3);
        }
    }

    private void ShowFigure()
    {
        activeChessman = new List<GameObject>();
        Chessmans = new Chessman[8, 8];

        for (int y = 0; y < 8; y++)
            for (int x = 0; x < 8; x++)
            {
                string figure = chess.GetFigureAt(x, y).ToString();
                if (figure == ".")
                    continue;
                PlaceFigure(figure, x, y);
            }
    }


    void MoveChessman(string fig, int x, int y)
    {
        Chessman c = Chessmans[x, y];
        Destroy(c.gameObject);

        PlaceFigure(fig, selectionX, selectionY);
    }

    private void PlaceFigure(string figure, int x, int y)
    {
        GameObject go = Instantiate(chessmanPrefabs[FigureToIndexChessman(figure)], GetTitleCenter(x, y), orientation) as GameObject;
        go.transform.SetParent(transform);
        Chessmans[x, y] = go.GetComponent<Chessman>();
        Chessmans[x, y].SetPosition(x, y);
        activeChessman.Add(go);
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


    //private GameObject SpawnChessman(GameObject figure, int x, int y)
    //{
    //    GameObject go = Instantiate(chessmanPrefabs[FigureToIndexChessman(figure)], GetTitleCenter(x, y), orientation) as GameObject;
    //    go.transform.SetParent(transform);
    //    Chessmans[x, y] = go.GetComponent<Chessman>();
    //    Chessmans[x, y].SetPosition(x, y);
    //    activeChessman.Add(go);
    //}

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

    private string GetSquare()
    {
        selectionX = Convert.ToInt32(selectionX);
        selectionY = Convert.ToInt32(selectionY);

        return ((char)('a' + selectionX)).ToString() + (selectionY + 1).ToString();
    }

    private Vector3 GetTitleCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x = (TILE_SIZE * x) + TILE_OFFSET;
        origin.z = (TILE_SIZE * y) + TILE_OFFSET;

        return origin;
    }

    private void DrawChessboard()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heightLine = Vector3.forward * 8;

        for (int i = 0; i <= 8; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for (int j = 0; j <= 8; j++)
            {

                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }

        }

        // Draw the selection 

        if (selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(
                Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
                Vector3.forward * selectionY + Vector3.right * (selectionX + 1)
            );

            Debug.DrawLine(
                Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1)
            );
        }

        Debug.Log(selectionX + " " + selectionY);

    }

    //public void RestartGame()
    //{
    //    foreach (GameObject go in activeChessman)
    //        Destroy(go);

    //    BoardHighlights.Instance.Hidehighlights();
    //    //SpawnAllChessman();
    //}

    //public Text LostText;
    //public Button RestartBtn;
    //public Button ExitBtn;

    //private void EndGame()
    //{
    //    LostText.gameObject.SetActive(true);
    //    RestartBtn.gameObject.SetActive(true);
    //    ExitBtn.gameObject.SetActive(true);

    //    if (isWhiteTrue)
    //    {
    //        LostText.text = "Победили белые";

    //    }
    //    else
    //    {
    //        LostText.text = "Победили черные";
    //    }
    //    //RestartGame ();
    //}
}