using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chessGame_console.ChessBoard;
using chessGame_console.ChessBoard.Enums;

namespace chessGame_console.ChessGame
{
    class Knight : Piece
    {
        public Knight(Color color, Board board) : base(color, board)
        {

        }
        public override bool[,] PossibleMovements()
        {
            bool[,] matrixOfPossibleMovements = new bool[Board.Rows, Board.Columns];

            Position position = new Position(0, 0);
            position.SetPosition(Position.Row - 1, Position.Column + 2);
            if (Board.IsPositionValid(position) && CanMove(position))
            {
                matrixOfPossibleMovements[position.Row, position.Column] = true;
            }
            position.SetPosition(Position.Row - 1, Position.Column -2);
            if (Board.IsPositionValid(position) && CanMove(position))
            {
                matrixOfPossibleMovements[position.Row, position.Column] = true;
            }
            position.SetPosition(Position.Row + 1, Position.Column + 2);
            if (Board.IsPositionValid(position) && CanMove(position))
            {
                matrixOfPossibleMovements[position.Row, position.Column] = true;
            }
            position.SetPosition(Position.Row + 1, Position.Column - 2);
            if (Board.IsPositionValid(position) && CanMove(position))
            {
                matrixOfPossibleMovements[position.Row, position.Column] = true;
            }
            position.SetPosition(Position.Row - 2, Position.Column + 1);
            if (Board.IsPositionValid(position) && CanMove(position))
            {
                matrixOfPossibleMovements[position.Row, position.Column] = true;
            }
            position.SetPosition(Position.Row - 2, Position.Column - 1);
            if (Board.IsPositionValid(position) && CanMove(position))
            {
                matrixOfPossibleMovements[position.Row, position.Column] = true;
            }
            position.SetPosition(Position.Row + 2, Position.Column + 1);
            if (Board.IsPositionValid(position) && CanMove(position))
            {
                matrixOfPossibleMovements[position.Row, position.Column] = true;
            }
            position.SetPosition(Position.Row + 2, Position.Column - 1);
            if (Board.IsPositionValid(position) && CanMove(position))
            {
                matrixOfPossibleMovements[position.Row, position.Column] = true;
            }
            
            return matrixOfPossibleMovements;
        }
        private bool CanMove(Position position)
        {
            Piece piece = Board.GetPiece(position);
            return piece == null || piece.Color != Color;
        }

        public override string ToString()
        {
            return "H";
        }
    }
}
