using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chessGame_console.ChessBoard.Enums;

namespace chessGame_console.ChessBoard
{
    abstract class Piece
    {
        public Position Position { get; set; }
        public Color Color { get; protected set; }
        public int MovementNumber { get; protected set; }
        public Board Board { get; set; }

        public Piece(Color color, Board board)
        {            
            Color = color;            
            Board = board;
            Position = null;
            MovementNumber = 0;
        }
        public void IncrementMovementNumber()
        {
            MovementNumber++;
        }
        public void DecrementMovementNumber()
        {
            MovementNumber--;
        }
        public bool IsMovePossible()
        {
            bool[,] possibleMovements = PossibleMovements();
            for(int i = 0; i < Board.Rows; i++)
            {
                for(int j = 0; j < Board.Columns; j++)
                {
                    if (possibleMovements[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public abstract bool[,] PossibleMovements();
    }
}
