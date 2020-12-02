// Advent of Code 2018
// Day 21
// Analysis version 1

#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>

int main(void)
{
    int64_t r0 = 0, r1 = 0, r2 = 0, r4 = 0, r5 = 0;

L0:
    r4 = 123; // seti 123 0 4
L1:
    r4 = r4 & 456; // bani 4 456 4
L2:
    r4 = (r4 == 72) ? 1 : 0; // eqri 4 72 4
L3:
    if (r4 == 0) goto L4; // addr 4 3 3
    else if (r4 == 1) goto L5;
L4:
    goto L1; // seti 0 0 3
L5:
    r4 = 0; // seti 0 6 4
L6:
    r1 = r4 | 65536; // bori 4 65536 1
L7:
    r4 = 678134; // seti 678134 1 4
L8:
    r5 = r1 & 255; // bani 1 255 5
L9:
    r4 = r4 + r5; // addr 4 5 4
L10:
    r4 = r4 & 16777215; // bani 4 16777215 4
L11:
    r4 = r4 * 65899; // muli 4 65899 4
L12:
    r4 = r4 & 16777215; // bani 4 16777215 4
L13:
    r5 = (256 > r1) ? 1 : 0; // gtir 256 1 5
L14:
    if (r5 == 0) goto L15; // addr 5 3 3
    else if (r5 == 1) goto L16;
L15:
    goto L17; // addi 3 1 3
L16:
    goto L28; // seti 27 8 3
L17:
    r5 = 0; // seti 0 1 5
L18:
    r2 = r5 + 1; // addi 5 1 2
L19:
    r2 = r2 * 256; // muli 2 256 2
L20:
    r2 = (r2 > r1 ? 1 : 0); // gtrr 2 1 2
L21:
    if (r2 == 0) goto L22; // addr 2 3 3
    else if (r2 == 1) goto L23;
L22:
    goto L24; // addi 3 1 3
L23:
    goto L26; // seti 25 7 3
L24:
    r5 = r5 + 1; // addi 5 1 5
L25:
    goto L18; // seti 17 1 3
L26:
    r1 = r5; // setr 5 3 1
L27:
    goto L8; // seti 7 8 3
L28:
    printf("%ld\n", r4);
    r5 = (r4 == r0) ? 1 : 0; // eqrr 4 0 5
L29:
    if (r5 == 0) goto L30; // addr 2 3 3
    else if (r5 == 1) goto L31;
L30:
    goto L6; // seti 5 4 3
L31:
    return 0;
}
