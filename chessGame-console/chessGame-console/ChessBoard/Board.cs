using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chessGame_console.ChessBoard.Exceptions;

namespace chessGame_console.ChessBoard
{
    class Board
    {
        private Piece[,] pieces;
        public int Rows { get; set; }
        public int Columns { get; set; }

        public Board(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            pieces = new Piece[rows, columns];
        }

        public Piece GetPiece(int row, int column)
        {
            return pieces[row, column];
        }
        public Piece GetPiece(Position position)
        {
            return pieces[position.Row, position.Column];
        }

        public void PlacePiece(Piece piece, Position placePosition)
        {
            if (IsPiecePlaced(placePosition))
            {
                throw new BoardException("There is already a piece in this position!");
            }
            pieces[placePosition.Row, placePosition.Column] = piece;
            piece.Position = placePosition;
        }
        public Piece WithdrawPiece(Position position)
        {
            if (!IsPiecePlaced(position))
            {
                return null;
            }
            Piece auxPiece = GetPiece(position);
            auxPiece.Position = null;
            pieces[position.Row, position.Column] = null;
            return auxPiece;
        }

        public bool IsPiecePlaced(Position position)
        {
            ValidatePosition(position);
            return GetPiece(position) != null;
        }
        public bool IsPositionValid(Position position)
        {
            if(position.Row < 0 || position.Row >= Rows || position.Column < 0 || position.Column >= Columns)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void ValidatePosition(Position position)
        {
            if (!IsPositionValid(position))
            {
                throw new BoardException("Invalid position");
            }
        }
    }
}
