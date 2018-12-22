// Advent of Code 2018
// Day 21
// Analysis version 3

#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>

int main(void)
{
    const int64_t expected = 8164934;
    int64_t r1 = 0, r4 = 0;

    while (r4 != expected)
    {
        r1 = r4 | 0x10000;
        r4 = 678134;
        bool keepGoing = true;
        while (keepGoing) {
            keepGoing = false;
            r4 += r1 & 0xFF;
            r4 &= 0xFFFFFF;
            r4 *= 65899;
            r4 &= 0xFFFFFF;
            if (r1 >= 0x100)
            {
                r1 /= 0x100;
                keepGoing = true;
            }
        }
        printf("%ld\n", r4);
    }
    return 0;
}
