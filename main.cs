using System;
using FEN_BTBConverterNS;

namespace main
{
    class main
    {
        static void Main(string[] args)
        {
            FEN_BTBConverter FEN_BTBconvert = new FEN_BTBConverter();
            Bitboards bitboards = FEN_BTBconvert.FENtoBTB("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            Console.WriteLine(bitboards.Wrook);
        }
    }
}