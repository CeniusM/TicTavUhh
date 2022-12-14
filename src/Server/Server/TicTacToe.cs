﻿//using System.Runtime.CompilerServices;
//using System.Runtime.InteropServices;

namespace TicTavUhhServer;

//[StructLayout(LayoutKind.Sequential)]
//[Serializable]
internal struct TicTacToe
{
    internal int[] board = new int[9];
    internal int player = 1;
    internal int winner = 3;
    internal bool isGameOver = false;
    internal int _moves = 0;

    public TicTacToe()
    {

    }

    public bool MakeMove(int move)
    {
        if (board[move] != 0 || isGameOver)
            return false;

        board[move] = player;
        _moves++;

        if (GameOverCheck())
        {
            winner = player;
            isGameOver = true;
            return true;
        }

        if (_moves == 9)
        {
            winner = 0;
            isGameOver = true;
            return true;
        }

        player ^= 0b11; // change player
        return true;
    }

    public bool GameOverCheck()
    {
        for (int i = 0; i < 3; i++)
        {
            if (board[i] != 0 && board[i] == board[i + 3] && board[i + 3] == board[i + 6])
                return true;
        }

        for (int i = 0; i < 3; i++)
        {
            if (board[i * 3] != 0 && board[(i * 3)] == board[(i * 3) + 1] && board[(i * 3) + 1] == board[(i * 3) + 2])
                return true;
        }

        if (board[0] != 0 && board[0] == board[4] && board[4] == board[8])
            return true;

        if (board[2] != 0 && board[2] == board[4] && board[4] == board[6])
            return true;

        return false;
    }
}