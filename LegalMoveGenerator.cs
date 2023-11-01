using System;
using System.Reflection;
using FEN_BTBConverterNS;

namespace LegalMoveGeneratorNS
{
	public class LegalMoveGenerator
	{

        public static UInt64 FlipBit(UInt64 number, int position)		//Function to make the flipping of certain bits easier
        {
            UInt64 mask = (UInt64)1 << position;  // Create a mask with a 1 at the specified position
            return number ^ mask;  // Use XOR operator to flip the bit at the specified position
        }

		public static List<int> flippedBitDetector(UInt64 pieceBitboard)		// Function that returs the indexes of bits which are 1
		{
			List<int> piecesIndexList = new List<int>();

			for (int i = 0; i < 64; i++)		// Loop that checks every index one by one to see which ones have their value set to 1
			{
				if(((pieceBitboard >> i) & 1) != 0)
				{
					piecesIndexList.Add(i);
				}
			}
			return piecesIndexList;
		}



        public static UInt64 rookLegalMovesGenerator(UInt64 rookBitboard, UInt64 allPiecesBitboard, UInt64 opponentColorBitboard)
		{
			UInt64 legalRookMoves = UInt64.MinValue;
			List<int> rookIndexList = flippedBitDetector(rookBitboard);
			
			foreach(int currentRookIndex in rookIndexList)
			{
				int currentRow = (currentRookIndex % 8);
				int currentColumn = currentRookIndex - 8 * currentRow;
				//The amount of squares we can go up, is equal to the current row
				int squaresDown = 7 - currentRow;
				// The amount of squares we can go left, is equal to the current column
				int squaresRight = 7 - currentColumn;

                // Going bit per bit and flipping it (or not) in all directions; Up, Down, Left, Right
                //legalRookMoves = ((legalRookMoves & allPiecesBitboard) != 0) ? legalRookMoves : FlipBit(legalRookMoves, currentRookIndex + 8 * i);

                // /\ UP /\
                for (int i = 1; i <= currentRow; i++)		//going one row up at a time, flipping the bit and checking if a piece is there 
				{
					legalRookMoves = FlipBit(legalRookMoves, currentRookIndex - 8 * i); //flipping the bit
					if ((legalRookMoves & allPiecesBitboard) != 0)	// checking if there is a piece on that square
					{
						Console.WriteLine($"UP, piece on square with index {i}");
						legalRookMoves = ((legalRookMoves & opponentColorBitboard) != 0) ? legalRookMoves : FlipBit(legalRookMoves, currentRookIndex - 8 * i);      //leaving the bit as is or leaving it as is depending on the color of the piece on the square
                        allPiecesBitboard = FlipBit(allPiecesBitboard, currentRookIndex - 8 * i);            //  /!\ Careful /!\, allpiece list is here altered and no longer contains all pieces, but as it isn't a global variable, it doesnt matter
                        break;
                    }
				}

				//  \/ DOWN \/
                for (int i = 1; i <= squaresDown; i++)       
                {
                    legalRookMoves = FlipBit(legalRookMoves, currentRookIndex + 8 * i); 
                    if ((legalRookMoves & allPiecesBitboard) != 0)  
                    {
                        Console.WriteLine($"DOWN piece on square with index {i}");
                        legalRookMoves = ((legalRookMoves & opponentColorBitboard) != 0) ? legalRookMoves  : FlipBit(legalRookMoves, currentRookIndex + 8 * i);
                        allPiecesBitboard = FlipBit(allPiecesBitboard, currentRookIndex + 8 * i);           
                        break;
                    }
                }

				// <-- LEFT <--
                for (int i = 1; i <= currentColumn; i++)       
                {
                    legalRookMoves = FlipBit(legalRookMoves, currentRookIndex - i); 
                    if ((legalRookMoves & allPiecesBitboard) != 0)  
                    {
                        Console.WriteLine($"LEFT piece on square with index {i}");
                        legalRookMoves = ((legalRookMoves & opponentColorBitboard) != 0) ? legalRookMoves : FlipBit(legalRookMoves, currentRookIndex - i);
                        allPiecesBitboard = FlipBit(allPiecesBitboard, currentRookIndex - i);           
                        break;
                    }
                }

				//--> RIGHT -->
                for (int i = 1; i <= squaresRight; i++)       
                {
                    legalRookMoves = FlipBit(legalRookMoves, currentRookIndex + i); 
                    if ((legalRookMoves & allPiecesBitboard) != 0)  
                    {
                        Console.WriteLine($"RIGHT piece on square with index {i}");
                        legalRookMoves = ((legalRookMoves & opponentColorBitboard) != 0) ? legalRookMoves : FlipBit(legalRookMoves, currentRookIndex + i);
                        allPiecesBitboard = FlipBit(allPiecesBitboard, currentRookIndex + i);           
                        break;
                    }
                }
            }


			return legalRookMoves;
		}
	}

     class tester{
        static void sMain(string[] args)
		{
            FEN_BTBConverter FEN_BTBconvert = new FEN_BTBConverter();
            Bitboards bitboards = FEN_BTBconvert.FENtoBTB("8/4Q3/8/8/1q2R3/8/8/8 w - - 0 1");

            //Bitboard with all the pieces
            UInt64 allPiecesBitboard = bitboards.Wrook | bitboards.Brook | bitboards.Wknight | bitboards.Bknight | bitboards.Bbishop | bitboards.Wbishop | bitboards.Wqueen | bitboards.Bqueen | bitboards.Wking | bitboards.Bking | bitboards.Wpawn | bitboards.Bpawn;
			// Bitboard with all the white pieces
			UInt64 whitePiecesBitboard = bitboards.Wrook | bitboards.Wknight | bitboards.Wbishop | bitboards.Wqueen | bitboards.Wking | bitboards.Wpawn;
			//Bitboard with all the white pieces
			UInt64 blackPiecesBitboard = bitboards.Brook | bitboards.Bknight | bitboards.Bbishop | bitboards.Bqueen | bitboards.Bking | bitboards.Bpawn;

			UInt64 legalRookMoves = LegalMoveGenerator.rookLegalMovesGenerator(bitboards.Wrook, allPiecesBitboard, blackPiecesBitboard);

            Console.WriteLine(legalRookMoves);
		}
    } 
}