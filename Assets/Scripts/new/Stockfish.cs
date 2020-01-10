using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class Stockfish : MonoBehaviour {
    string str;
    string pathStockfis;
    Process processToRun;
    public Stockfish()
    {
        str = "none";
        pathStockfis = Directory.GetCurrentDirectory() + "\\Assets\\stockfish\\stockfish.exe";

        processToRun = new Process();
        processToRun.StartInfo.FileName = pathStockfis;
        //processToRun.StartInfo.Arguments = fen;
        processToRun.StartInfo.UseShellExecute = false;
        processToRun.StartInfo.CreateNoWindow = true;
        processToRun.StartInfo.RedirectStandardOutput = true;
        processToRun.StartInfo.RedirectStandardInput = true;
        processToRun.Start();
    }

    public string BestMove(string fen)
    {
        String setupString = "position fen " + fen;
        processToRun.StandardInput.WriteLine(setupString);

        String processString = "go movetime 1000";
        processToRun.StandardInput.WriteLine(processString);

        do
        {
            str = processToRun.StandardOutput.ReadLine();
            BestMove();
        } while (BestMove() == "none");
 
        int n = str.IndexOf("bestmove");
        if (n != -1)
        {
            str = str.Substring(n + 9, 4);
        }

        return str;
    }

    ~Stockfish()
    {
        processToRun.Close();
    }

    private string BestMove()
    {
        int n = str.IndexOf("bestmove");
        if (n != -1)
        {
            return str.Substring(n + 9, 4);
        }
        return "none";
    }


}
