// Advent of Code 2018
// Day 21
// Analysis version 2

#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>

int main(void)
{
    int64_t r0 = 0, r1 = 0, r2 = 0, r4 = 0, r5 = 0;

loop1:
    r1 = r4 | 0x10000;
    r4 = 678134;
loop2:
    r4 += r1 & 0xFF;
    r4 &= 0xFFFFFF;
    r4 *= 65899;
    r4 &= 0xFFFFFF;
    r5 = (r1 < 256) ? 1 : 0;
    if (r5 == 0)
    {
        r5 = 0;
    loop3:
        r2 = r5 + 1;
        r2 = r2 * 256;
        r2 = (r2 > r1 ? 1 : 0);
        if (r2 == 0)
        {
            r5 = r5 + 1;
            goto loop3;
        }
        r1 = r5;
        goto loop2;
    }
    printf("%ld\n", r4);
    if (r4 != r0)
    {
        goto loop1;
    }
    return 0;
}
