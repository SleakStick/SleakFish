using System;
using System.Reflection;
using FEN_BTBConverterNS;

namespace LegalMoveGeneratorNS
{
	public class LegalMoveGenerator
	{
		//		public rookLegalMovesGenerator

		public LegalMoveGenerator(Bitboards bitboards)
		{
			//Bitboard with all the pieces
			UInt64 allPiecesBitboard = bitboards.Wrook | bitboards.Brook | bitboards.Wknight | bitboards.Bknight | bitboards.Bbishop | bitboards.Wbishop | bitboards.Wqueen | bitboards.Bqueen | bitboards.Wking | bitboards.Bking | bitboards.Wpawn | bitboards.Bpawn;
			// Bitboard with all the white pieces
			UInt64 whitePiecesBitboard = bitboards.Wrook | bitboards.Wknight | bitboards.Wbishop | bitboards.Wqueen | bitboards.Wking | bitboards.Wpawn;
			//Bitboard with all the white pieces
			UInt64 blackPiecesBitboard = bitboards.Brook | bitboards.Bknight | bitboards.Bbishop | bitboards.Bqueen | bitboards.Bking | bitboards.Bpawn;
		}
	}
}