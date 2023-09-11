using System.Runtime.InteropServices;  
using System.Collections.Generic;
using System.Runtime.InteropServices;
namespace Lab1;
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct Solution
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public bool[] triag;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public bool[] xorr;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public bool[] orr;
    public bool nonce;
}


public class Test{

    [DllImport("libtest", EntryPoint = "solve")]
    public static extern uint solve(int number);
    public static Solution solve2(int number){
        uint data = Test.solve(number);
        Solution res = new Solution();
        res.triag = new bool[6];
        res.xorr = new bool[6];
        res.orr = new bool[3];
        res.nonce = false;
        int ii=0;
        for(;ii<6;ii++){
            res.triag[ii] = (data & (1U << ii)) != 0;
        }
        for(;ii<12;ii++){
            res.xorr[ii-6] = (data & (1U << ii)) != 0;
        }
        for(;ii<15;ii++){
            res.orr[ii-12] = (data & (1U << ii)) != 0;
        }
        return res;
    }

}