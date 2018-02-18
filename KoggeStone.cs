using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMCChess
{
    class KoggeStone
    {
        //https://chessprogramming.wikispaces.com/First+Rank+Attacks
        //https://chessprogramming.wikispaces.com/Hyperbola+Quintessence
        //https://stackoverflow.com/questions/16925204/sliding-move-generation-using-magic-Board

        private const UInt64 notAFile = 0xfefefefefefefefeUL;
        private const UInt64 notHFile = 0x7f7f7f7f7f7f7f7fUL;

        //Kogge-Stone flood algorithm - all directions
        public static UInt64 southAttacks(UInt64 rooks, UInt64 empty)
        {
            UInt64 flood = rooks;
            flood |= rooks = (rooks << 8) & empty;
            flood |= rooks = (rooks << 8) & empty;
            flood |= rooks = (rooks << 8) & empty;
            flood |= rooks = (rooks << 8) & empty;
            flood |= rooks = (rooks << 8) & empty;
            flood |= rooks = (rooks << 8) & empty;
            flood |= (rooks << 8) & empty;
            return flood << 8;
        }

        public static UInt64 northAttacks(UInt64 rooks, UInt64 empty)
        {
            UInt64 flood = rooks;
            flood |= rooks = (rooks >> 8) & empty;
            flood |= rooks = (rooks >> 8) & empty;
            flood |= rooks = (rooks >> 8) & empty;
            flood |= rooks = (rooks >> 8) & empty;
            flood |= rooks = (rooks >> 8) & empty;
            flood |= (rooks >> 8) & empty;
            return flood >> 8;
        }

        public static UInt64 eastAttacks(UInt64 rooks, UInt64 empty)
        {
            UInt64 flood = rooks;
            empty &= notAFile;
            flood |= rooks = (rooks << 1) & empty;
            flood |= rooks = (rooks << 1) & empty;
            flood |= rooks = (rooks << 1) & empty;
            flood |= rooks = (rooks << 1) & empty;
            flood |= rooks = (rooks << 1) & empty;
            flood |= (rooks << 1) & empty;
            return (flood << 1) & notAFile;
        }

        public static UInt64 westAttacks(UInt64 rooks, UInt64 empty)
        {
            UInt64 flood = rooks;
            empty &= notHFile;
            flood |= rooks = (rooks >> 1) & empty;
            flood |= rooks = (rooks >> 1) & empty;
            flood |= rooks = (rooks >> 1) & empty;
            flood |= rooks = (rooks >> 1) & empty;
            flood |= rooks = (rooks >> 1) & empty;
            flood |= (rooks >> 1) & empty;
            return (flood >> 1) & notHFile;
        }

        public static UInt64 noWeAttacks(UInt64 bishops, UInt64 empty)
        {
            UInt64 flood = bishops;
            empty &= notHFile;
            flood |= bishops = (bishops << 7) & empty;
            flood |= bishops = (bishops << 7) & empty;
            flood |= bishops = (bishops << 7) & empty;
            flood |= bishops = (bishops << 7) & empty;
            flood |= bishops = (bishops << 7) & empty;
            flood |= (bishops << 7) & empty;
            return (flood << 7) & notHFile;
        }

        public static UInt64 soWeAttacks(UInt64 bishops, UInt64 empty)
        {
            UInt64 flood = bishops;
            empty &= notHFile;
            flood |= bishops = (bishops >> 9) & empty;
            flood |= bishops = (bishops >> 9) & empty;
            flood |= bishops = (bishops >> 9) & empty;
            flood |= bishops = (bishops >> 9) & empty;
            flood |= bishops = (bishops >> 9) & empty;
            flood |= (bishops >> 9) & empty;
            return (flood >> 9) & notHFile;
        }

        public static UInt64 noEaAttacks(UInt64 bishops, UInt64 empty)
        {
            UInt64 flood = bishops;
            empty &= notAFile;
            flood |= bishops = (bishops << 9) & empty;
            flood |= bishops = (bishops << 9) & empty;
            flood |= bishops = (bishops << 9) & empty;
            flood |= bishops = (bishops << 9) & empty;
            flood |= bishops = (bishops << 9) & empty;
            flood |= (bishops << 9) & empty;
            return (flood << 9) & notAFile;
        }

        public static UInt64 soEaAttacks(UInt64 bishops, UInt64 empty)
        {
            UInt64 flood = bishops;
            empty &= notAFile;
            flood |= bishops = (bishops >> 7) & empty;
            flood |= bishops = (bishops >> 7) & empty;
            flood |= bishops = (bishops >> 7) & empty;
            flood |= bishops = (bishops >> 7) & empty;
            flood |= bishops = (bishops >> 7) & empty;
            flood |= (bishops >> 7) & empty;
            return (flood >> 7) & notAFile;
        }

        public static UInt64 rookAttacks(UInt64 rooks, UInt64 empty)
        {
            return southAttacks(rooks, empty) | northAttacks(rooks, empty) |
                      eastAttacks(rooks, empty) | westAttacks(rooks, empty);
        }

        public static UInt64 bishopAttacks(UInt64 bishops, UInt64 empty)
        {
            return soEaAttacks(bishops, empty) | soWeAttacks(bishops, empty) |
                      noEaAttacks(bishops, empty) | noWeAttacks(bishops, empty);
        }

    }
}
