using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chessGame_console.ChessBoard;
using chessGame_console.ChessBoard.Enums;

namespace chessGame_console.ChessGame
{
    class Rook : Piece
    {
        public Rook(Color color, Board board) : base(color, board)
        {

        }

        public override bool[,] PossibleMovements()
        {
            bool[,] matrixOfPossibleMovements = new bool[Board.Rows, Board.Columns];

            Position position = new Position(0, 0);
            // above
            position.SetPosition(Position.Row - 1, Position.Column);
            while(Board.IsPositionValid(position) && CanMove(position))
            {
                matrixOfPossibleMovements[position.Row, position.Column] = true;
                if (Board.GetPiece(position) != null && Board.GetPiece(position).Color != Color)
                {
                    break;
                }
                position.Row -= 1;
            }
            // below
            position.SetPosition(Position.Row + 1, Position.Column);
            while (Board.IsPositionValid(position) && CanMove(position))
            {
                matrixOfPossibleMovements[position.Row, position.Column] = true;
                if (Board.GetPiece(position) != null && Board.GetPiece(position).Color != Color)
                {
                    break;
                }
                position.Row += 1;
            }
            // right
            position.SetPosition(Position.Row, Position.Column + 1);
            while (Board.IsPositionValid(position) && CanMove(position))
            {
                matrixOfPossibleMovements[position.Row, position.Column] = true;
                if (Board.GetPiece(position) != null && Board.GetPiece(position).Color != Color)
                {
                    break;
                }
                position.Column += 1;
            }
            // left
            position.SetPosition(Position.Row, Position.Column - 1);
            while (Board.IsPositionValid(position) && CanMove(position))
            {
                matrixOfPossibleMovements[position.Row, position.Column] = true;
                if (Board.GetPiece(position) != null && Board.GetPiece(position).Color != Color)
                {
                    break;
                }
                position.Column -= 1;
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
            return "R";
        }
    }
}
