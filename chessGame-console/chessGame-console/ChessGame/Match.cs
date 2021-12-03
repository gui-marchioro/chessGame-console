using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chessGame_console.ChessBoard;
using chessGame_console.ChessBoard.Enums;
using chessGame_console.ChessBoard.Exceptions;

namespace chessGame_console.ChessGame
{
    class Match
    {
        private HashSet<Piece> pieces;
        private HashSet<Piece> catchedPieces;
        public Piece EnPassantCandidate { get; private set; }
        public Color CurrentPlayer { get; private set; }
        public Board Board { get; private set; }
        public int Turn { get; private set; }
        public bool Finished { get; private set; }
        public bool Check { get; private set; }

        public Match()
        {
            Board = new Board(8, 8);
            Turn = 1;
            CurrentPlayer = Color.White;
            Finished = false;
            pieces = new HashSet<Piece>();
            catchedPieces = new HashSet<Piece>();
            PlacePieces();
            Check = false;
            EnPassantCandidate = null;
        }

        public Piece MovePiece(Position from, Position to)
        {
            Piece piece = Board.WithdrawPiece(from);
            if (piece != null)
            {
                piece.IncrementMovementNumber();
                Piece pieceCatched = Board.WithdrawPiece(to);
                Board.PlacePiece(piece, to);
                if (pieceCatched != null)
                {
                    catchedPieces.Add(pieceCatched);
                }

                // Special Movement Castling
                if (piece is King && to.Column == from.Column + 2)
                {
                    Position rookFrom = new Position(from.Row, from.Column + 3);
                    Position rookTo = new Position(from.Row, from.Column + 1);
                    Piece rook = Board.WithdrawPiece(rookFrom);
                    rook.IncrementMovementNumber();
                    Board.PlacePiece(rook, rookTo);
                }
                if (piece is King && to.Column == from.Column - 2)
                {
                    Position rookFrom = new Position(from.Row, from.Column - 4);
                    Position rookTo = new Position(from.Row, from.Column - 1);
                    Piece rook = Board.WithdrawPiece(rookFrom);
                    rook.IncrementMovementNumber();
                    Board.PlacePiece(rook, rookTo);
                }

                // Special Movement En Passant
                if (piece is Pawn)
                {
                    if (from.Column != to.Column && pieceCatched == null)
                    {
                        Position piecePosition;
                        if (piece.Color == Color.White)
                        {
                            piecePosition = new Position(to.Row + 1, to.Column);                            
                        }
                        else
                        {
                            piecePosition = new Position(to.Row - 1, to.Column);
                        }
                        pieceCatched = Board.WithdrawPiece(piecePosition);
                        catchedPieces.Add(pieceCatched);
                    }
                }

                return pieceCatched;
            }
            return null;
        }
        public HashSet<Piece> CatchedPieces(Color color)
        {
            HashSet<Piece> catchedPiecesColor = new HashSet<Piece>();
            foreach (Piece piece in catchedPieces)
            {
                if (piece.Color == color)
                {
                    catchedPiecesColor.Add(piece);
                }
            }
            return catchedPiecesColor;
        }
        public HashSet<Piece> InGamePieces(Color color)
        {
            HashSet<Piece> inGamePiecesColor = new HashSet<Piece>();
            foreach (Piece piece in pieces)
            {
                if (piece.Color == color)
                {
                    inGamePiecesColor.Add(piece);
                }
            }
            inGamePiecesColor.ExceptWith(catchedPieces);
            return inGamePiecesColor;
        }

        public void UndoMovement(Position from, Position to, Piece catchedPiece)
        {
            Piece piece = Board.WithdrawPiece(to);
            if (piece != null)
            {
                piece.DecrementMovementNumber();
                if (catchedPiece != null)
                {
                    Board.PlacePiece(catchedPiece, to);
                    catchedPieces.Remove(catchedPiece);
                }
                Board.PlacePiece(piece, from);

                // Special Movement Castling
                if (piece is King && to.Column == from.Column + 2)
                {
                    Position rookFrom = new Position(from.Row, from.Row + 3);
                    Position rookTo = new Position(from.Row, from.Row + 1);
                    Piece rook = Board.WithdrawPiece(rookTo);
                    rook.DecrementMovementNumber();
                    Board.PlacePiece(rook, rookFrom);
                }
                if (piece is King && to.Column == from.Column - 2)
                {
                    Position rookFrom = new Position(from.Row, from.Row - 4);
                    Position rookTo = new Position(from.Row, from.Row - 1);
                    Piece rook = Board.WithdrawPiece(rookTo);
                    rook.DecrementMovementNumber();
                    Board.PlacePiece(rook, rookFrom);
                }

                // Special Movement En Passant
                if (piece is Pawn)
                {
                    if (from.Column != to.Column && catchedPiece == null)
                    {
                        Piece pawnPiece = Board.WithdrawPiece(to);
                        Position piecePosition;
                        if (piece.Color == Color.White)
                        {
                            piecePosition = new Position(3, to.Column);
                        }
                        else
                        {
                            piecePosition = new Position(4, to.Column);
                        }
                        Board.PlacePiece(pawnPiece, piecePosition);
                    }
                }
            }
        }

        public void MakeMove(Position from, Position to)
        {
            Piece catchedPiece = MovePiece(from, to);

            if (IsCheck(CurrentPlayer))
            {
                UndoMovement(from, to, catchedPiece);
                throw new BoardException("You can't put yourself in check");
            }
            // Special movement promotion
            Piece pieceAux = Board.GetPiece(to);
            if (pieceAux is Pawn)
            {
                if ((pieceAux.Color == Color.White && to.Row == 0) || (pieceAux.Color == Color.Black && to.Row == (Board.Rows - 1)))
                {
                    Board.WithdrawPiece(to);
                    pieces.Remove(pieceAux);
                    Piece queenPiece = new Queen(pieceAux.Color, Board);
                    Board.PlacePiece(queenPiece, to);
                    pieces.Add(queenPiece);
                }
            }
            if (IsCheck(OpponentColor(CurrentPlayer)))
            {
                Check = true;
            }
            else
            {
                Check = false;
            }
            if (IsCheckMate(OpponentColor(CurrentPlayer)))
            {
                Finished = true;
            }
            else
            {
                Turn++;
                ChangePlayer();
            }
            Piece piece = Board.GetPiece(to);
            // Special Movement En Passant
            if (piece is Pawn && (to.Row == from.Row - 2 || to.Row == from.Row + 2))
            {
                EnPassantCandidate = piece;
            }
            else
            {
                EnPassantCandidate = null;
            }
        }
        private void ChangePlayer()
        {
            if (Turn % 2 == 0)
            {
                CurrentPlayer = Color.Black;
            }
            else
            {
                CurrentPlayer = Color.White;
            }
        }
        private void PlaceNewPiece(Piece piece, char column, int row)
        {
            Board.PlacePiece(piece, new ChessPosition(column, row).ToPosition());
            pieces.Add(piece);
        }
        private void PlacePieces()
        {
            PlaceNewPiece(new King(ChessBoard.Enums.Color.White, Board, this), 'e', 1);
            PlaceNewPiece(new Queen(ChessBoard.Enums.Color.White, Board), 'd', 1);
            PlaceNewPiece(new Rook(ChessBoard.Enums.Color.White, Board), 'h', 1);
            PlaceNewPiece(new Rook(ChessBoard.Enums.Color.White, Board), 'a', 1);
            PlaceNewPiece(new Bishop(ChessBoard.Enums.Color.White, Board), 'c', 1);
            PlaceNewPiece(new Bishop(ChessBoard.Enums.Color.White, Board), 'f', 1);
            PlaceNewPiece(new Knight(ChessBoard.Enums.Color.White, Board), 'b', 1);
            PlaceNewPiece(new Knight(ChessBoard.Enums.Color.White, Board), 'g', 1);
            PlaceNewPiece(new Pawn(ChessBoard.Enums.Color.White, Board, this), 'e', 2);
            PlaceNewPiece(new Pawn(ChessBoard.Enums.Color.White, Board, this), 'd', 2);
            PlaceNewPiece(new Pawn(ChessBoard.Enums.Color.White, Board, this), 'h', 2);
            PlaceNewPiece(new Pawn(ChessBoard.Enums.Color.White, Board, this), 'a', 2);
            PlaceNewPiece(new Pawn(ChessBoard.Enums.Color.White, Board, this), 'c', 2);
            PlaceNewPiece(new Pawn(ChessBoard.Enums.Color.White, Board, this), 'f', 2);
            PlaceNewPiece(new Pawn(ChessBoard.Enums.Color.White, Board, this), 'b', 2);
            PlaceNewPiece(new Pawn(ChessBoard.Enums.Color.White, Board, this), 'g', 2);


            PlaceNewPiece(new King(ChessBoard.Enums.Color.Black, Board, this), 'e', 8);
            PlaceNewPiece(new Queen(ChessBoard.Enums.Color.Black, Board), 'd', 8);
            PlaceNewPiece(new Rook(ChessBoard.Enums.Color.Black, Board), 'a', 8);
            PlaceNewPiece(new Bishop(ChessBoard.Enums.Color.Black, Board), 'c', 8);
            PlaceNewPiece(new Bishop(ChessBoard.Enums.Color.Black, Board), 'f', 8);
            PlaceNewPiece(new Knight(ChessBoard.Enums.Color.Black, Board), 'b', 8);
            PlaceNewPiece(new Knight(ChessBoard.Enums.Color.Black, Board), 'g', 8);
            PlaceNewPiece(new Rook(ChessBoard.Enums.Color.Black, Board), 'h', 8);
            PlaceNewPiece(new Pawn(ChessBoard.Enums.Color.Black, Board, this), 'e', 7);
            PlaceNewPiece(new Pawn(ChessBoard.Enums.Color.Black, Board, this), 'd', 7);
            PlaceNewPiece(new Pawn(ChessBoard.Enums.Color.Black, Board, this), 'h', 7);
            PlaceNewPiece(new Pawn(ChessBoard.Enums.Color.Black, Board, this), 'a', 7);
            PlaceNewPiece(new Pawn(ChessBoard.Enums.Color.Black, Board, this), 'c', 7);
            PlaceNewPiece(new Pawn(ChessBoard.Enums.Color.Black, Board, this), 'f', 7);
            PlaceNewPiece(new Pawn(ChessBoard.Enums.Color.Black, Board, this), 'b', 7);
            PlaceNewPiece(new Pawn(ChessBoard.Enums.Color.Black, Board, this), 'g', 7);
        }
        public void VerifyPiecePositionFrom(Position position)
        {
            Board.ValidatePosition(position);
            if (Board.GetPiece(position) == null)
            {
                throw new BoardException("There isn't any piece in this position!");
            }
            if (CurrentPlayer != Board.GetPiece(position).Color)
            {
                throw new BoardException("Please, select the turn corresponding piece color!");
            }
            if (!Board.GetPiece(position).IsMovePossible())
            {
                throw new BoardException("There isn't possible movements for this piece!");
            }
        }
        public void VerifyPiecePositionTo(Position from, Position to)
        {
            Board.ValidatePosition(to);
            if (!Board.GetPiece(from).PossibleMovements()[to.Row, to.Column])
            {
                throw new BoardException("Please, select just possible movements!");
            }
        }
        private Color OpponentColor(Color color)
        {
            if (color == Color.White)
            {
                return Color.Black;
            }
            else
            {
                return Color.White;
            }
        }
        private Piece FindKing(Color color)
        {
            foreach (Piece piece in InGamePieces(color))
            {
                if (piece is King)
                {
                    return piece;
                }
            }
            return null;
        }
        public bool IsCheck(Color color)
        {
            Piece king = FindKing(color);
            if (king == null)
            {
                throw new BoardException("No king was found!");
            }
            foreach (Piece opponentPiece in InGamePieces(OpponentColor(color)))
            {
                bool[,] OpponentPossibleMovements = opponentPiece.PossibleMovements();
                if (OpponentPossibleMovements[king.Position.Row, king.Position.Column])
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsCheckMate(Color color)
        {
            if (!IsCheck(color))
            {
                return false;
            }
            foreach (Piece piece in InGamePieces(color))
            {
                bool[,] possibleMovements = piece.PossibleMovements();
                for (int i = 0; i < Board.Rows; i++)
                {
                    for (int j = 0; j < Board.Columns; j++)
                    {
                        if (possibleMovements[i, j])
                        {
                            Position from = piece.Position;
                            Position to = new Position(i, j);
                            Piece catchedPiece = MovePiece(from, to);
                            bool isCheck = IsCheck(color);
                            UndoMovement(from, to, catchedPiece);
                            if (!isCheck)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
