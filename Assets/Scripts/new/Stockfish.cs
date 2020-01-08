using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

public class Stockfish
{
     string str;
     string pathStockfis = Directory.GetCurrentDirectory() + "\\Assets\\stockfish\\StockfishManager.exe";

    //RunSingleFile(Directory.GetCurrentDirectory() + "\\stockfish.exe", "");

    public  string BestMove(string fen)
    {
        Process processToRun = new Process();
        processToRun.StartInfo.FileName = pathStockfis;
        processToRun.StartInfo.Arguments = fen;
        processToRun.StartInfo.UseShellExecute = false;
        processToRun.StartInfo.CreateNoWindow = true;
        processToRun.StartInfo.RedirectStandardOutput = true;
        processToRun.StartInfo.RedirectStandardInput = true;
        processToRun.Start();
        str = processToRun.StandardOutput.ReadLine();
        processToRun.Close();
        return str;
    }

}
