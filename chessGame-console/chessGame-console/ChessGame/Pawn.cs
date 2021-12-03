using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chessGame_console.ChessBoard;
using chessGame_console.ChessBoard.Enums;

namespace chessGame_console.ChessGame
{
    class Pawn : Piece
    {
        private Match chessMatch;
        public Pawn(Color color, Board board, Match match) : base(color, board)
        {
            chessMatch = match;
        }
        public override bool[,] PossibleMovements()
        {
            bool[,] matrixOfPossibleMovements = new bool[Board.Rows, Board.Columns];

            Position position = new Position(0, 0);

            if (Color == Color.White)
            {
                position.SetPosition(Position.Row - 2, Position.Column);
                if (Board.IsPositionValid(position) && CanMove(position) && MovementNumber == 0)
                {
                    matrixOfPossibleMovements[position.Row, position.Column] = true;
                }
                position.SetPosition(Position.Row - 1, Position.Column);
                if (Board.IsPositionValid(position) && CanMove(position))
                {
                    matrixOfPossibleMovements[position.Row, position.Column] = true;
                }
                position.SetPosition(Position.Row - 1, Position.Column + 1);
                if (Board.IsPositionValid(position) && IsEnemy(position))
                {
                    matrixOfPossibleMovements[position.Row, position.Column] = true;
                }
                position.SetPosition(Position.Row - 1, Position.Column - 1);
                if (Board.IsPositionValid(position) && IsEnemy(position))
                {
                    matrixOfPossibleMovements[position.Row, position.Column] = true;
                }
                // Special Movement En Passant
                if (Position.Row == 3)
                {
                    Position left = new Position(Position.Row, Position.Column - 1);
                    if (Board.IsPositionValid(left) && IsEnemy(left) && Board.GetPiece(left) == chessMatch.EnPassantCandidate)
                    {
                        matrixOfPossibleMovements[left.Row - 1, left.Column] = true;
                    }
                    Position right = new Position(Position.Row, Position.Column + 1);
                    if (Board.IsPositionValid(right) && IsEnemy(right) && Board.GetPiece(right) == chessMatch.EnPassantCandidate)
                    {
                        matrixOfPossibleMovements[right.Row - 1, right.Column] = true;
                    }
                }
            }
            else
            {
                position.SetPosition(Position.Row + 2, Position.Column);
                if (Board.IsPositionValid(position) && CanMove(position) && MovementNumber == 0)
                {
                    matrixOfPossibleMovements[position.Row, position.Column] = true;
                }
                position.SetPosition(Position.Row + 1, Position.Column);
                if (Board.IsPositionValid(position) && CanMove(position))
                {
                    matrixOfPossibleMovements[position.Row, position.Column] = true;
                }
                position.SetPosition(Position.Row + 1, Position.Column + 1);
                if (Board.IsPositionValid(position) && IsEnemy(position))
                {
                    matrixOfPossibleMovements[position.Row, position.Column] = true;
                }
                position.SetPosition(Position.Row + 1, Position.Column - 1);
                if (Board.IsPositionValid(position) && IsEnemy(position))
                {
                    matrixOfPossibleMovements[position.Row, position.Column] = true;
                }

                // Special Movement En Passant
                if (Position.Row == 4)
                {
                    Position left = new Position(Position.Row, Position.Column - 1);
                    if (Board.IsPositionValid(left) && IsEnemy(left) && Board.GetPiece(left) == chessMatch.EnPassantCandidate)
                    {
                        matrixOfPossibleMovements[left.Row + 1, left.Column] = true;
                    }
                    Position right = new Position(Position.Row, Position.Column + 1);
                    if (Board.IsPositionValid(right) && IsEnemy(right) && Board.GetPiece(right) == chessMatch.EnPassantCandidate)
                    {
                        matrixOfPossibleMovements[right.Row + 1, right.Column] = true;
                    }
                }
            }

            return matrixOfPossibleMovements;
        }
        private bool CanMove(Position position)
        {
            Piece piece = Board.GetPiece(position);
            return piece == null;
        }
        private bool IsEnemy(Position position)
        {
            Piece piece = Board.GetPiece(position);
            return piece != null && piece.Color != Color;
        }

        public override string ToString()
        {
            return "P";
        }
    }
}
