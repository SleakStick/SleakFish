using System;
using System.Diagnostics;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Xml.Linq;

namespace FEN_BTBConverterNS
{
    public class Bitboards //Class with all the bitboards of all the pieces
    {
        public Bitboards(UInt64 WRook, UInt64 WKnight, UInt64 WBishop, UInt64 WQueen, UInt64 WKing, UInt64 WPawn, UInt64 BRook, UInt64 BKnight, UInt64 BBishop, UInt64 BQueen, UInt64 BKing, UInt64 BPawn)
        => (Wrook, Wknight, Wbishop, Wqueen, Wking, Wpawn, Brook, Bknight, Bbishop, Bqueen, Bking, Bpawn) = (WRook, WKnight, WBishop, WQueen, WKing, WPawn, BRook, BKnight, BBishop, BQueen, BKing, BPawn);
		
		public UInt64 Wrook { get; }
        public UInt64 Wknight { get; }
        public UInt64 Wbishop { get; }
        public UInt64 Wqueen { get; }
        public UInt64 Wking { get; }
        public UInt64 Wpawn { get; }
        public UInt64 Brook { get; }
        public UInt64 Bknight { get; }
        public UInt64 Bbishop { get; }
        public UInt64 Bqueen { get; }
        public UInt64 Bking { get; }
        public UInt64 Bpawn { get; }
    }



	public class FEN_BTBConverter
    {

        public static UInt64 FlipBit(UInt64 number, int position)
        {
            UInt64 mask = (UInt64)1 << position;  // Create a mask with a 1 at the specified position
            return number ^ mask;  // Use XOR operator to flip the bit at the specified position
        }


        public Bitboards FENtoBTB(string FENcode)
		{

            // Dictionary with every piece name as written in FEN and a Bitboard attached to that piece
            Dictionary<string, UInt64> piecesIndexes = new Dictionary<string, UInt64>(12);
            piecesIndexes.Add("R", UInt64.MinValue);
            piecesIndexes.Add("N", UInt64.MinValue);
            piecesIndexes.Add("B", UInt64.MinValue);
            piecesIndexes.Add("Q", UInt64.MinValue);
            piecesIndexes.Add("K", UInt64.MinValue);
            piecesIndexes.Add("P", UInt64.MinValue);
            piecesIndexes.Add("r", UInt64.MinValue);
            piecesIndexes.Add("n", UInt64.MinValue);
            piecesIndexes.Add("b", UInt64.MinValue);
            piecesIndexes.Add("q", UInt64.MinValue);
            piecesIndexes.Add("k", UInt64.MinValue);
            piecesIndexes.Add("p", UInt64.MinValue);


            int i = 0;
            short numberValue = 0;
            foreach( char c in FENcode)
            {
                string character = Char.ToString(c);

                if (character == " ")
                {
                    break;
                }

                if (Int16.TryParse(character,out numberValue))  // If the current character is a number, skip that many positions
                {
                    i = i + numberValue;
                }
                else if (character == "/") // FEN contains slashes, these are to be ignored in this algorithm
                {
                    continue;
                }
                else  // Else, just skip one position and add a 1 for the correct bitboard
                {
                    piecesIndexes[character] = FlipBit(piecesIndexes[character], i);
                    i = i + 1;
                }
            }

            Bitboards bitboards = new Bitboards(
                piecesIndexes["R"],
                piecesIndexes["N"],
                piecesIndexes["B"],
                piecesIndexes["Q"],
                piecesIndexes["K"],
                piecesIndexes["P"],     // Defining Bitboard type item to return
                piecesIndexes["r"],
                piecesIndexes["n"],
                piecesIndexes["b"],
                piecesIndexes["q"],
                piecesIndexes["k"],
                piecesIndexes["p"]
                );

            return bitboards;
		}

    }

    //---------------------------------------Testing---------------------------------------

    class tester
    {
        static void  sMain(string[] args) // if this file is ran as main program, and not library, it is to be tested
        {
            Console.WriteLine("Program FENtoBTB ran as main, not library, starting test suite...");


            // These are the results we are expecting to get
            string testFEN1 = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            UInt64 test1WhiteRook = 0x8100000000000000;
            UInt64 test1WhiteKnight = 0x4200000000000000;
            UInt64 test1WhiteBishop = 0x2400000000000000;
            UInt64 test1WhiteQueen = 0x800000000000000;
            UInt64 test1WhiteKing = 0x1000000000000000;
            UInt64 test1WhitePawn = 0xff000000000000;
            UInt64 test1BlackRook = 0x81;
            UInt64 test1BlackKnight = 0x42;
            UInt64 test1BlackBishop = 0x24;
            UInt64 test1BlackQueen = 0x8;
            UInt64 test1BlackKing = 0x10;
            UInt64 test1BlackPawn = 0xff00;

            string testFEN2 = "5rk1/1p6/1Npb1r2/p3p2p/P1PpP1pP/3PB1P1/2b1QP1N/6KR w q - 0 1";

            UInt64 test2WhiteRook = 0x8000000000000000;
            UInt64 test2WhiteKnight = 0x80000000020000;
            UInt64 test2WhiteBishop = 0x100000000000;
            UInt64 test2WhiteQueen = 0x10000000000000;
            UInt64 test2WhiteKing = 0x4000000000000000;
            UInt64 test2WhitePawn = 0x20489500000000;
            UInt64 test2BlackRook = 0x200020;
            UInt64 test2BlackKnight = 0x0;
            UInt64 test2BlackBishop = 0x4000000080000;
            UInt64 test2BlackQueen = 0x0;
            UInt64 test2BlackKing = 0x40;
            UInt64 test2BlackPawn = 0x4891040200;

            Bitboards correctTestResult1 = new Bitboards(
                test1WhiteRook,
                test1WhiteKnight,
                test1WhiteBishop,
                test1WhiteQueen,
                test1WhiteKing,
                test1WhitePawn,
                test1BlackRook,
                test1BlackKnight,
                test1BlackBishop,
                test1BlackQueen,
                test1BlackKing,
                test1BlackPawn
                );

            Bitboards correctTestResult2 = new Bitboards(
                test2WhiteRook,
                test2WhiteKnight,
                test2WhiteBishop,
                test2WhiteQueen,
                test2WhiteKing,
                test2WhitePawn,
                test2BlackRook,
                test2BlackKnight,
                test2BlackBishop,
                test2BlackQueen,
                test2BlackKing,
                test2BlackPawn
                );

            FEN_BTBConverter FEN_BTBconvert = new FEN_BTBConverter();



            Stopwatch timer = new Stopwatch();
            timer.Start();

            Bitboards testBitboards1 = FEN_BTBconvert.FENtoBTB(testFEN1);

            timer.Stop();

            bool test1Succeeded = true;

            foreach (PropertyInfo property in testBitboards1.GetType().GetProperties())
            {
                if (!(property.GetValue(testBitboards1).Equals(property.GetValue(correctTestResult1))))
                {
                    if (test1Succeeded)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Test 1 failed in {timer.ElapsedMilliseconds} ms, more information about failure below");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"FEN for failed test; {testFEN1}");
                        Console.WriteLine("\n");
                        test1Succeeded = false;
                    }
                    Console.WriteLine($"Mistake found: Found bitboard for {property.Name} type piece is; ");
                    Console.WriteLine(property.GetValue(testBitboards1));
                    Console.WriteLine("While supposed correct value is; ");
                    Console.WriteLine(property.GetValue(correctTestResult1));
                }
            }
            if ( test1Succeeded )
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Test 1 passed in {timer.ElapsedMilliseconds} ms");
                Console.ForegroundColor = ConsoleColor.White;
            }



            timer.Start();

            Bitboards testBitboards2 = FEN_BTBconvert.FENtoBTB(testFEN2);

            timer.Stop();

            bool test2Succeeded = true;

            foreach (PropertyInfo property in testBitboards2.GetType().GetProperties())
            {
                if (!property.GetValue(testBitboards2).Equals(property.GetValue(correctTestResult2)))
                {
                    if (test2Succeeded)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Test 2 failed in {timer.ElapsedMilliseconds} ms, more information about failure below");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"FEN for failed test; {testFEN2}");
                        Console.WriteLine("\n");
                        test2Succeeded = false;
                    }
                    Console.WriteLine($"Mistake found: Found bitboard for {property.Name} type piece is; ");
                    Console.WriteLine(property.GetValue(testBitboards2));
                    Console.WriteLine("While supposed correct value is; ");
                    Console.WriteLine(property.GetValue(correctTestResult2));
                }
            }
            if (test2Succeeded)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Test 2 passed in {timer.ElapsedMilliseconds} ms");
                Console.ForegroundColor = ConsoleColor.White;
            }

        }
    } 
}
