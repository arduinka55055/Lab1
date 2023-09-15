#include <iostream>

#include <cstdlib>
extern "C" {
using namespace std;
typedef struct {
    bool triag[6];
    bool xorr[6];
    bool orr[3];
} Solution;

uint solve(int number)
{
    Solution result;
    int a[6];
    a[0] = number & 1;
    a[1] = (number >> 5) & 1;
    a[2] = (number >> 4) & 1;
    a[3] = (number >> 3) & 1;
    a[4] = (number >> 2) & 1;
    a[5] = (number >> 1) & 1;;
    int b[5][5];
    for (int i = 0; i < 5; i++)
    {
        for (int j = 0; j < 5; j++)
        {
            b[i][j] = 0;
        }
    }
    b[4][0] = a[0] | a[1];
    b[4][1] = b[4][0] | a[2];
    b[4][2] = b[4][1] | a[3];
    b[4][3] = b[4][2] | a[4];
    b[4][4] = b[4][3] | a[5];
    b[3][1] = (a[0] && a[1]) || (b[4][0] && a[2]);
    b[3][2] = b[3][1] | (b[4][1] & a[3]);
    b[3][3] = b[3][2] | (b[4][2] & a[4]);
    b[3][4] = b[3][3] | (b[4][3] & a[5]);
    b[2][2] = (a[0] && a[1]) && (b[4][0] && a[2]) || (b[3][1] && (b[4][1] &&
                                                                  a[3]));
    b[2][3] = b[2][2] | ((b[3][2] & (b[4][2] & a[4])));
    b[2][4] = b[2][3] | (b[3][3] & (b[4][3] & a[5]));
    b[1][3] = ((a[0] && a[1]) && (b[4][0] && a[2]) && ((b[4][1] && a[3]))) &&
              ((b[4][2] && a[4]) && b[3][2]);
    b[1][4] = b[1][3] | (b[2][3] & (b[3][3] & (b[4][3] & a[5])));
    b[0][4] = ((a[0] && a[1]) && (b[4][0] && a[2]) && (b[4][1] && a[3]) &&
               (b[4][2] && a[4])) &&
              (b[4][3] && a[5] && b[3][3] && b[2][3] && b[1][3]);

    result.triag[0] = b[4][4];
    result.triag[1] = b[4][3];
    result.triag[2] = b[4][2];
    result.triag[3] = b[4][1];
    result.triag[4] = b[4][0];
    result.triag[5] = b[3][3];

    int p[6];
    //cout<< (b[4][4] | b[3][4]<<1 | b[2][4]<<2 | b[1][4]<<3 | b[0][4]<<4)<<endl;
    p[0] = b[4][4] ^ b[3][4];
    p[1] = b[3][4] ^ b[2][4];
    p[2] = b[2][4] ^ b[1][4];
    p[3] = b[1][4] ^ b[0][4];
    p[4] = b[0][4] ^ ((a[0] && a[1]) && (b[4][0] && a[2]) && (b[4][1] && a[3]) &&
                      (b[4][2] && a[4])) ||
           (b[4][3] & a[5] & b[3][3] & b[2][3] & b[1][3]);
    p[5] = 0 ^ ((a[0] && a[1]) && (b[4][0] && a[2]) && (b[4][1] && a[3]) &&
                (b[4][2] && a[4])) |
           (b[4][3] & a[5] & b[3][3] & b[2][3] & b[1][3]);

    result.xorr[0] = p[0];
    result.xorr[1] = p[1];
    result.xorr[2] = p[2];
    result.xorr[3] = p[3];
    result.xorr[4] = p[4];
    result.xorr[5] = p[5];

    int s[3];
    s[0] = p[0] | p[2] | p[4];
    s[1] = p[1] | p[2] | p[5];
    s[2] = p[3] | p[4] | p[5];

    result.orr[0] = s[0];
    result.orr[1] = s[1];
    result.orr[2] = s[2];

    //struct bits to uint
    uint res = 0;
    for (int i = 0; i < 6; i++)
    {
        res |= result.triag[i] << i;
    }
    for (int i = 0; i < 6; i++)
    {
        res |= result.xorr[i] << (i + 6);
    }
    for (int i = 0; i < 3; i++)
    {
        res |= result.orr[i] << (i + 12);
    }
    return res;
   
}
}