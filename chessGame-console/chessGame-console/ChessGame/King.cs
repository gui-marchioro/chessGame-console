using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chessGame_console.ChessBoard;
using chessGame_console.ChessBoard.Enums;

namespace chessGame_console.ChessGame
{
    class King : Piece
    {
        private Match chessMatch;
        public King(Color color, Board board, Match match) : base(color, board)
        {
            chessMatch = match;
        }

        public override bool[,] PossibleMovements()
        {
            bool[,] matrixOfPossibleMovements = new bool[Board.Rows, Board.Columns];

            Position position = new Position(0, 0);
            // above
            position.SetPosition(Position.Row - 1, Position.Column);
            if (Board.IsPositionValid(position) && CanMove(position))
            {
                matrixOfPossibleMovements[position.Row, position.Column] = true;
            }
            // below
            position.SetPosition(Position.Row + 1, Position.Column);
            if (Board.IsPositionValid(position) && CanMove(position))
            {
                matrixOfPossibleMovements[position.Row, position.Column] = true;
            }
            // right
            position.SetPosition(Position.Row, Position.Column + 1);
            if (Board.IsPositionValid(position) && CanMove(position))
            {
                matrixOfPossibleMovements[position.Row, position.Column] = true;
            }
            // left
            position.SetPosition(Position.Row, Position.Column - 1);
            if (Board.IsPositionValid(position) && CanMove(position))
            {
                matrixOfPossibleMovements[position.Row, position.Column] = true;
            }
            // right diagonal, above
            position.SetPosition(Position.Row - 1, Position.Column + 1);
            if (Board.IsPositionValid(position) && CanMove(position))
            {
                matrixOfPossibleMovements[position.Row, position.Column] = true;
            }
            // right diagonal, below
            position.SetPosition(Position.Row + 1, Position.Column + 1);
            if (Board.IsPositionValid(position) && CanMove(position))
            {
                matrixOfPossibleMovements[position.Row, position.Column] = true;
            }
            // left diagonal, above
            position.SetPosition(Position.Row - 1, Position.Column - 1);
            if (Board.IsPositionValid(position) && CanMove(position))
            {
                matrixOfPossibleMovements[position.Row, position.Column] = true;
            }
            // left diagonal, below
            position.SetPosition(Position.Row + 1, Position.Column - 1);
            if (Board.IsPositionValid(position) && CanMove(position))
            {
                matrixOfPossibleMovements[position.Row, position.Column] = true;
            }

            // Special movement Castling
            if (MovementNumber == 0 && !chessMatch.Check)
            {
                Position rookPosition1 = new Position(Position.Row, Position.Column + 3);
                if (IsThereRookForCastling(rookPosition1))
                {
                    Position auxPosition1 = new Position(Position.Row, Position.Column + 1);
                    Position auxPosition2 = new Position(Position.Row, Position.Column + 2);
                    if (Board.GetPiece(auxPosition1) == null && Board.GetPiece(auxPosition2) == null)
                    {
                        matrixOfPossibleMovements[auxPosition2.Row, auxPosition2.Column] = true;
                    }
                }

                Position rookPosition2 = new Position(Position.Row, Position.Column - 4);
                if (IsThereRookForCastling(rookPosition2))
                {
                    Position auxPosition1 = new Position(Position.Row, Position.Column - 1);
                    Position auxPosition2 = new Position(Position.Row, Position.Column - 2);
                    Position auxPosition3 = new Position(Position.Row, Position.Column - 3);
                    if (Board.GetPiece(auxPosition1) == null && Board.GetPiece(auxPosition2) == null && Board.GetPiece(auxPosition3) == null)
                    {
                        matrixOfPossibleMovements[auxPosition2.Row, auxPosition2.Column] = true;
                    }
                }
            }

            return matrixOfPossibleMovements;
        }

        private bool IsThereRookForCastling(Position position)
        {
            Piece piece = Board.GetPiece(position);
            return piece != null && piece is Rook && piece.Color == Color && piece.MovementNumber == 0;
        }

        private bool CanMove(Position position)
        {
            Piece piece = Board.GetPiece(position);
            return piece == null || piece.Color != Color;
        }

        public override string ToString()
        {
            return "K";
        }
    }
}
