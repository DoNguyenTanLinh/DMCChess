//If PERFECT_MAGIC_HASH is used, the move generator will use an additional indirection 
//to make the table sizes smaller : (~50kb+((original size)/sizeof(PERFECT_MAGIC_HASH)).
//Otherwise, the move Board generator will use up 2304kb of memory but might perform a bit faster.

//#define PERFECT_MAGIC_HASH
//#define VARIABLE_SHIFT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMCChess
{
    //smallchess.com/MagicMoves.zip
    class Magic
    {
        private static uint[] magicmoves_r_shift = new uint[64]
        {
	        52, 53, 53, 53, 53, 53, 53, 52,
	        53, 54, 54, 54, 54, 54, 54, 53,
	        53, 54, 54, 54, 54, 54, 54, 53,
	        53, 54, 54, 54, 54, 54, 54, 53,
	        53, 54, 54, 54, 54, 54, 54, 53,
	        53, 54, 54, 54, 54, 54, 54, 53,
	        53, 54, 54, 54, 54, 54, 54, 53,
	        53, 54, 54, 53, 53, 53, 53, 53
        };

        private static UInt64[] magicmoves_r_magics = new UInt64[64]
        {
	        0x0080001020400080UL, 0x0040001000200040UL, 0x0080081000200080UL, 0x0080040800100080UL,
	        0x0080020400080080UL, 0x0080010200040080UL, 0x0080008001000200UL, 0x0080002040800100UL,
	        0x0000800020400080UL, 0x0000400020005000UL, 0x0000801000200080UL, 0x0000800800100080UL,
	        0x0000800400080080UL, 0x0000800200040080UL, 0x0000800100020080UL, 0x0000800040800100UL,
	        0x0000208000400080UL, 0x0000404000201000UL, 0x0000808010002000UL, 0x0000808008001000UL,
	        0x0000808004000800UL, 0x0000808002000400UL, 0x0000010100020004UL, 0x0000020000408104UL,
	        0x0000208080004000UL, 0x0000200040005000UL, 0x0000100080200080UL, 0x0000080080100080UL,
	        0x0000040080080080UL, 0x0000020080040080UL, 0x0000010080800200UL, 0x0000800080004100UL,
	        0x0000204000800080UL, 0x0000200040401000UL, 0x0000100080802000UL, 0x0000080080801000UL,
	        0x0000040080800800UL, 0x0000020080800400UL, 0x0000020001010004UL, 0x0000800040800100UL,
	        0x0000204000808000UL, 0x0000200040008080UL, 0x0000100020008080UL, 0x0000080010008080UL,
	        0x0000040008008080UL, 0x0000020004008080UL, 0x0000010002008080UL, 0x0000004081020004UL,
	        0x0000204000800080UL, 0x0000200040008080UL, 0x0000100020008080UL, 0x0000080010008080UL,
	        0x0000040008008080UL, 0x0000020004008080UL, 0x0000800100020080UL, 0x0000800041000080UL,
	        0x00FFFCDDFCED714AUL, 0x007FFCDDFCED714AUL, 0x003FFFCDFFD88096UL, 0x0000040810002101UL,
	        0x0001000204080011UL, 0x0001000204000801UL, 0x0001000082000401UL, 0x0001FFFAABFAD1A2UL
        };

        private static UInt64[] magicmoves_r_mask = new UInt64[64]
        {	
	        0x000101010101017EUL, 0x000202020202027CUL, 0x000404040404047AUL, 0x0008080808080876UL,
	        0x001010101010106EUL, 0x002020202020205EUL, 0x004040404040403EUL, 0x008080808080807EUL,
	        0x0001010101017E00UL, 0x0002020202027C00UL, 0x0004040404047A00UL, 0x0008080808087600UL,
	        0x0010101010106E00UL, 0x0020202020205E00UL, 0x0040404040403E00UL, 0x0080808080807E00UL,
	        0x00010101017E0100UL, 0x00020202027C0200UL, 0x00040404047A0400UL, 0x0008080808760800UL,
	        0x00101010106E1000UL, 0x00202020205E2000UL, 0x00404040403E4000UL, 0x00808080807E8000UL,
	        0x000101017E010100UL, 0x000202027C020200UL, 0x000404047A040400UL, 0x0008080876080800UL,
	        0x001010106E101000UL, 0x002020205E202000UL, 0x004040403E404000UL, 0x008080807E808000UL,
	        0x0001017E01010100UL, 0x0002027C02020200UL, 0x0004047A04040400UL, 0x0008087608080800UL,
	        0x0010106E10101000UL, 0x0020205E20202000UL, 0x0040403E40404000UL, 0x0080807E80808000UL,
	        0x00017E0101010100UL, 0x00027C0202020200UL, 0x00047A0404040400UL, 0x0008760808080800UL,
	        0x00106E1010101000UL, 0x00205E2020202000UL, 0x00403E4040404000UL, 0x00807E8080808000UL,
	        0x007E010101010100UL, 0x007C020202020200UL, 0x007A040404040400UL, 0x0076080808080800UL,
	        0x006E101010101000UL, 0x005E202020202000UL, 0x003E404040404000UL, 0x007E808080808000UL,
	        0x7E01010101010100UL, 0x7C02020202020200UL, 0x7A04040404040400UL, 0x7608080808080800UL,
	        0x6E10101010101000UL, 0x5E20202020202000UL, 0x3E40404040404000UL, 0x7E80808080808000UL
        };

        private static uint[] magicmoves_b_shift = new uint[64]
        {
	        58, 59, 59, 59, 59, 59, 59, 58,
	        59, 59, 59, 59, 59, 59, 59, 59,
	        59, 59, 57, 57, 57, 57, 59, 59,
	        59, 59, 57, 55, 55, 57, 59, 59,
	        59, 59, 57, 55, 55, 57, 59, 59,
	        59, 59, 57, 57, 57, 57, 59, 59,
	        59, 59, 59, 59, 59, 59, 59, 59,
	        58, 59, 59, 59, 59, 59, 59, 58
        };

        private static UInt64[] magicmoves_b_magics = new UInt64[64]
        {
	        0x0002020202020200UL, 0x0002020202020000UL, 0x0004010202000000UL, 0x0004040080000000UL,
	        0x0001104000000000UL, 0x0000821040000000UL, 0x0000410410400000UL, 0x0000104104104000UL,
	        0x0000040404040400UL, 0x0000020202020200UL, 0x0000040102020000UL, 0x0000040400800000UL,
	        0x0000011040000000UL, 0x0000008210400000UL, 0x0000004104104000UL, 0x0000002082082000UL,
	        0x0004000808080800UL, 0x0002000404040400UL, 0x0001000202020200UL, 0x0000800802004000UL,
	        0x0000800400A00000UL, 0x0000200100884000UL, 0x0000400082082000UL, 0x0000200041041000UL,
	        0x0002080010101000UL, 0x0001040008080800UL, 0x0000208004010400UL, 0x0000404004010200UL,
	        0x0000840000802000UL, 0x0000404002011000UL, 0x0000808001041000UL, 0x0000404000820800UL,
	        0x0001041000202000UL, 0x0000820800101000UL, 0x0000104400080800UL, 0x0000020080080080UL,
	        0x0000404040040100UL, 0x0000808100020100UL, 0x0001010100020800UL, 0x0000808080010400UL,
	        0x0000820820004000UL, 0x0000410410002000UL, 0x0000082088001000UL, 0x0000002011000800UL,
	        0x0000080100400400UL, 0x0001010101000200UL, 0x0002020202000400UL, 0x0001010101000200UL,
	        0x0000410410400000UL, 0x0000208208200000UL, 0x0000002084100000UL, 0x0000000020880000UL,
	        0x0000001002020000UL, 0x0000040408020000UL, 0x0004040404040000UL, 0x0002020202020000UL,
	        0x0000104104104000UL, 0x0000002082082000UL, 0x0000000020841000UL, 0x0000000000208800UL,
	        0x0000000010020200UL, 0x0000000404080200UL, 0x0000040404040400UL, 0x0002020202020200UL
        };


        private static UInt64[] magicmoves_b_mask = new UInt64[64]
        {
	        0x0040201008040200UL, 0x0000402010080400UL, 0x0000004020100A00UL, 0x0000000040221400UL,
	        0x0000000002442800UL, 0x0000000204085000UL, 0x0000020408102000UL, 0x0002040810204000UL,
	        0x0020100804020000UL, 0x0040201008040000UL, 0x00004020100A0000UL, 0x0000004022140000UL,
	        0x0000000244280000UL, 0x0000020408500000UL, 0x0002040810200000UL, 0x0004081020400000UL,
	        0x0010080402000200UL, 0x0020100804000400UL, 0x004020100A000A00UL, 0x0000402214001400UL,
	        0x0000024428002800UL, 0x0002040850005000UL, 0x0004081020002000UL, 0x0008102040004000UL,
	        0x0008040200020400UL, 0x0010080400040800UL, 0x0020100A000A1000UL, 0x0040221400142200UL,
	        0x0002442800284400UL, 0x0004085000500800UL, 0x0008102000201000UL, 0x0010204000402000UL,
	        0x0004020002040800UL, 0x0008040004081000UL, 0x00100A000A102000UL, 0x0022140014224000UL,
	        0x0044280028440200UL, 0x0008500050080400UL, 0x0010200020100800UL, 0x0020400040201000UL,
	        0x0002000204081000UL, 0x0004000408102000UL, 0x000A000A10204000UL, 0x0014001422400000UL,
	        0x0028002844020000UL, 0x0050005008040200UL, 0x0020002010080400UL, 0x0040004020100800UL,
	        0x0000020408102000UL, 0x0000040810204000UL, 0x00000A1020400000UL, 0x0000142240000000UL,
	        0x0000284402000000UL, 0x0000500804020000UL, 0x0000201008040200UL, 0x0000402010080400UL,
	        0x0002040810204000UL, 0x0004081020400000UL, 0x000A102040000000UL, 0x0014224000000000UL,
	        0x0028440200000000UL, 0x0050080402000000UL, 0x0020100804020000UL, 0x0040201008040200UL
        };

	    #if !PERFECT_MAGIC_HASH
		    private static UInt64[,] magicmovesbdb = new UInt64[64,1<<9];
            private static UInt64[,] magicmovesrdb = new UInt64[64,1<<12];
	    #else
		    private static UInt64[] magicmovesbdb = new UInt64[1428];
		    private static ushort[,] magicmoves_b_indices = new ushort[64, 1UL << 9];
		    private static UInt64[] magicmovesrdb = new UInt64[4900];
		    private static ushort[,] magicmoves_r_indices = new ushort[64, 1UL << 12];
	    #endif

        private static UInt64 initmagicmoves_occ(int[] squares, int numSquares, UInt64 linocc)
        {
	        int i;
	        UInt64 ret = 0UL;
	        for(i=0;i<numSquares;i++)
		        if ((linocc & (1UL<<i)) != 0UL) 
                    ret |= (1UL<<squares[i]);
	        return ret;
        }

        private static UInt64 initmagicmoves_Rmoves(int square, UInt64 occ)
        {
	        UInt64 ret=0;
	        UInt64 bit;
	        UInt64 rowbits=(0xFFUL<<(8*(square/8)));
	
	        bit=(1UL<<square);
	        do
	        {
		        bit<<=8;
		        ret|=bit;
	        }while((bit != 0UL) && (~(bit&occ) != 0UL));
	        bit=(1UL<<square);
	        do
	        {
		        bit>>=8;
		        ret|=bit;
	        }while((bit != 0UL) && (~(bit&occ) != 0UL));
	        bit=(1UL<<square);
	        do
	        {
		        bit<<=1;
		        if((bit&rowbits) != 0UL) 
                    ret|=bit;
		        else 
                    break;
	        }while(~(bit&occ) != 0UL);
	        bit=(1UL<<square);
	        do
	        {
		        bit>>=1;
		        if((bit&rowbits)  != 0UL)
                    ret|=bit;
		        else 
                    break;
	        }while(~(bit&occ) != 0UL);
	        return ret;
        }

        private static UInt64 initmagicmoves_Bmoves(int square, UInt64 occ)
        {
	        UInt64 ret=0;
	        UInt64 bit;
	        UInt64 bit2;
	        UInt64 rowbits=(0xFFUL<<(8*(square/8)));
	
	        bit=(1UL<<square);
	        bit2=bit;
	        do
	        {
		        bit<<=8-1;
		        bit2>>=1;
		        if ((bit2&rowbits) != 0UL) ret|=bit;
		        else break;
	        }while((bit != 0UL) && (~(bit&occ) != 0UL));
	        bit=(1UL<<square);
	        bit2=bit;
	        do
	        {
		        bit<<=8+1;
		        bit2<<=1;
		        if ((bit2&rowbits) != 0UL) ret|=bit;
		        else break;
	        }while((bit != 0UL) && (~(bit&occ) != 0UL));
	        bit=(1UL<<square);
	        bit2=bit;
	        do
	        {
		        bit>>=8-1;
		        bit2<<=1;
		        if ((bit2&rowbits) != 0UL) ret|=bit;
		        else break;
	        }while((bit != 0UL) && (~(bit&occ) != 0UL));
	        bit=(1UL<<square);
	        bit2=bit;
	        do
	        {
		        bit>>=8+1;
		        bit2>>=1;
		        if ((bit2&rowbits) != 0UL) ret|=bit;
		        else break;
	        }while((bit != 0UL) && (~(bit&occ) != 0UL));
	        return ret;
        }

        public static void initmagicmoves()
        {
	        int i;

	        //for bitscans :
	        //initmagicmoves_bitpos64_database[(x*0x07EDD5E59A4E28C2UL)>>58]
	        int[] initmagicmoves_bitpos64_database = new int[64]{
	            63,  0, 58,  1, 59, 47, 53,  2,
	            60, 39, 48, 27, 54, 33, 42,  3,
	            61, 51, 37, 40, 49, 18, 28, 20,
	            55, 30, 34, 11, 43, 14, 22,  4,
	            62, 57, 46, 52, 38, 26, 32, 41,
	            50, 36, 17, 19, 29, 10, 13, 21,
	            56, 45, 25, 31, 35, 16,  9, 12,
	            44, 24, 15,  8, 23,  7,  6,  5
            };

        #if PERFECT_MAGIC_HASH
	        for(i=0;i<1428;i++)
		        magicmovesbdb[i]=0UL;
	        for(i=0;i<4900;i++)
		        magicmovesrdb[i]=0UL;
        #endif

            //Bishops
	        for(i=0;i<64;i++)
	        {
		        int[] squares = new int[64];
		        int numsquares = 0;
		        UInt64 temp = magicmoves_b_mask[i];

		        while(temp != 0UL)
		        {
                    int bitCount = BitOps.BitCountWegner(temp);
                    squares[numsquares++] = bitCount;
                    BitOps.BitScanForwardReset(ref temp);
                    //UInt64 bit = temp & -temp;
			        //squares[numsquares++] = initmagicmoves_bitpos64_database[(bit * 0x07EDD5E59A4E28C2UL) >> 58];
			        //temp ^= bit;
		        }

		        for(temp = 0; temp < (1UL<<numsquares); temp++)
		        {
			        UInt64 tempocc = initmagicmoves_occ(squares, numsquares, temp);

			        #if !PERFECT_MAGIC_HASH
                        #if !VARIABLE_SHIFT
                            magicmovesbdb[squares[i], Convert.ToInt32((tempocc * magicmoves_b_magics[squares[i]]) >> 55)] = initmagicmoves_Bmoves(i, tempocc);
                        #else
			                magicmovesbdb[squares[i], Convert.ToInt32((tempocc*magicmoves_b_magics[squares[i]]) >> magicmoves_b_shift[squares[i]])] = initmagicmoves_Bmoves(i,tempocc);
                        #endif
                        #else
				        UInt64 moves=initmagicmoves_Bmoves(i, tempocc);

                        #if !VARIABLE_SHIFT
				            UInt64 index = ((tempocc*magicmoves_b_magics[i]) >> 55);
                        #else
                            UInt64 index = ((tempocc*magicmoves_b_magics[i]) >> magicmoves_b_shift[square]);
                        #endif

                        for (ushort j = 0; j < 1428; j++)
				        {
					        if(~magicmovesbdb[j] != 0UL)
					        {
						        magicmovesbdb[j] = moves;
						        magicmoves_b_indices[i, Convert.ToInt32(index)] = j;
						        break;
					        }
					        else if(magicmovesbdb[j] == moves)
					        {
						        magicmoves_b_indices[i, Convert.ToInt32(index)] = j;
						        break;
					        }
				        }
                    #endif
                }
	        }

            //Rooks
	        for(i=0;i<64;i++)
	        {
		        int[] squares = new int[64];
		        int numsquares=0;
		        UInt64 temp = magicmoves_r_mask[i];

		        while(temp != 0UL)
		        {
                    int bitCount = BitOps.BitCountWegner(temp);
                    squares[numsquares++] = bitCount;
                    BitOps.BitScanForwardReset(ref temp);
			        //UInt64 bit = temp & -temp;
			        //squares[numsquares++]=initmagicmoves_bitpos64_database[(bit*0x07EDD5E59A4E28C2UL)>>58];
			        //temp^=bit;
		        }

		        for(temp = 0; temp < (1UL << numsquares); temp++)
		        {
			        UInt64 tempocc=initmagicmoves_occ(squares,numsquares,temp);

			        #if !PERFECT_MAGIC_HASH
                        #if !VARIABLE_SHIFT
                            magicmovesrdb[squares[i], Convert.ToInt32((tempocc * magicmoves_r_magics[squares[i]]) >> 52)] = initmagicmoves_Rmoves(i, tempocc);
			            #else
				            magicmovesrdb[squares[i], Convert.ToInt32((tempocc*magicmoves_r_magics[squares[i]]) >> magicmoves_r_shift[squares[i]])] = initmagicmoves_Rmoves(i,tempocc);
                        #endif
                        #else
				        UInt64 moves = initmagicmoves_Rmoves(i,tempocc);

                        #if !VARIABLE_SHIFT
				            UInt64 index = (((tempocc)*magicmoves_r_magics[i]) >> 52);
                        #else
                            UInt64 index = (((tempocc)*magicmoves_r_magics[i]) >> magicmoves_r_shift[squares[i]]);
                        #endif

                        for (ushort j = 0; j < 4900; j++)
				        {
					        if(~magicmovesrdb[j] != 0UL)
					        {
						        magicmovesrdb[j] = moves;
						        magicmoves_r_indices[i, Convert.ToInt32(index)] = j;
						        break;
					        }
					        else if(magicmovesrdb[j] == moves)
					        {
						        magicmoves_r_indices[i, Convert.ToInt32(index)] = j;
						        break;
					        }
				        }
#endif
                }
	        }
        }

        //These are the functions actually used publicly to calculate the attack sets
	    public static UInt64 Bmagic(uint square, UInt64 occupancy)
	    {
		    #if !PERFECT_MAGIC_HASH
                #if !VARIABLE_SHIFT
				    return magicmovesbdb[square, ((occupancy & magicmoves_b_mask[square]) * magicmoves_b_magics[square]) >> 55];
		        #else
				    return magicmovesbdb[square, ((occupancy & magicmoves_b_mask[square]) * magicmoves_b_magics[square]) >> magicmoves_b_shift[square]];		    
                #endif
		    #else
                #if !VARIABLE_SHIFT
			        return magicmovesbdb[magicmoves_b_indices[square, ((occupancy & magicmoves_b_mask[square]) * magicmoves_b_magics[square]) >> 55]];
		        #else
			        return magicmovesbdb[magicmoves_b_indices[square, ((occupancy & magicmoves_b_mask[square]) * magicmoves_b_magics[square]) >> magicmoves_b_shift[square]]];		    
                #endif
		    
            #endif
	    }

	    public static UInt64 Rmagic(uint square, UInt64 occupancy)
	    {
		    #if !PERFECT_MAGIC_HASH
                #if !VARIABLE_SHIFT
				    return magicmovesrdb[square, ((occupancy & magicmoves_r_mask[square]) * magicmoves_r_magics[square]) >> 52];
		        #else
				    return magicmovesrdb[square, ((occupancy & magicmoves_r_mask[square]) * magicmoves_r_magics[square]) >> magicmoves_r_shift[square]];		    
                #endif
		    #else
                #if !VARIABLE_SHIFT
			        return magicmovesrdb[magicmoves_r_indices[square, ((occupancy & magicmoves_r_mask[square]) * magicmoves_r_magics[square]) >> 52]];
		        #else
			        return magicmovesrdb[magicmoves_r_indices[square, ((occupancy & magicmoves_r_mask[square]) * magicmoves_r_magics[square]) >> magicmoves_r_shift[square]]];		    
                #endif
		    #endif
	    }

	    public static UInt64 BmagicNOMASK(uint square, UInt64 occupancy)
	    {
		    #if !PERFECT_MAGIC_HASH
                #if !VARIABLE_SHIFT
				    return magicmovesbdb[square, (occupancy * magicmoves_b_magics[square]) >> 55];
		        #else
				    return magicmovesbdb[square, (occupancy * magicmoves_b_magics[square]) >> magicmoves_b_shift[square]];		    
                #endif
		    #else
                #if !VARIABLE_SHIFT
			        return magicmovesbdb[magicmoves_b_indices[square, (occupancy * magicmoves_b_magics[square]) >> 55]];
		        #else
			        return magicmovesbdb[magicmoves_b_indices[square, (occupancy * magicmoves_b_magics[square]) >> magicmoves_b_shift[square]]];		    
                #endif
		    #endif
	    }

	    public static UInt64 RmagicNOMASK(uint square, UInt64 occupancy)
	    {
		    #if !PERFECT_MAGIC_HASH
                #if !VARIABLE_SHIFT
				    return magicmovesrdb[square, (occupancy * magicmoves_r_magics[square]) >> 52];
		        #else
				    return magicmovesrdb[square, (occupancy * magicmoves_r_magics[square]) >> magicmoves_r_shift[square]];		    
                #endif
		    #else
                #if !VARIABLE_SHIFT
			        return magicmovesrdb[magicmoves_r_indices[square, (occupancy * magicmoves_r_magics[square]) >> 52]];
		        #else
			        return magicmovesrdb[magicmoves_r_indices[square, (occupancy * magicmoves_r_magics[square]) >> magicmoves_r_shift[square]]];		    
                #endif
		    #endif
	    }

	    public static UInt64 Qmagic(uint square, UInt64 occupancy)
	    {
		    return Bmagic(square, occupancy) | Rmagic(square, occupancy);
	    }

	    public static UInt64 QmagicNOMASK(uint square, UInt64 occupancy)
	    {
		    return BmagicNOMASK(square,occupancy) | RmagicNOMASK(square,occupancy);
	    }

        //Alternatively, coudl define a struct SMagic like this, for each square

        //UInt64 attack_table[...]; // ~840 KiB all rook and bishop attacks, less with constructive collisions optimization

        //private struct SMagic {
        //   UInt64 attackTableForSquare;  // pointer to attack_table for each particular square
        //   UInt64 mask;  // to mask relevant squares of both lines (no outer squares)
        //   UInt64 magic; // magic 64-bit factor
        //   int shift; // shift right
        //};

        //private SMagic[] mBishopTbl = new SMagic[64];
        //private SMagic[] mRookTbl = new SMagic[64];

        //public UInt64 magicBishopAttacks(UInt64 occ, byte sq) 
        //{
        //   UInt64[] aptr = mBishopTbl[sq].attackTableForSquare;
        //   occ      &= mBishopTbl[sq].mask;
        //   occ      *= mBishopTbl[sq].magic;
        //   occ     >>= mBishopTbl[sq].shift;
        //   return aptr[occ];
        //}

        //public UInt64 magicRookAttacks(UInt64 occ, byte sq) 
        //{
        //   UInt64[] aptr = mRookTbl[sq].attackTableForSquare;
        //   occ      &= mRookTbl[sq].mask;
        //   occ      *= mRookTbl[sq].magic;
        //   occ     >>= mRookTbl[sq].shift;
        //   return aptr[occ];
        //}

    }
}
