#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>

int main(void) {
    int64_t r0 = 1, r1 = 0, r2 = 0, r3 = 0, r5 = 0;

j0:
    goto j17; // addi 4 16 4

j1:
    r2 = 1; // seti 1 7 2
j2:
    printf("%ld %ld %ld %ld <rip> %ld\n", r0, r1, r2, r3, r5);
    r5 = 1; // seti 1 1 5
j3:
    r3 = r2 * r5; // mulr 2 5 3
j4:
    r3 = (r3 == r1 ? 1 : 0); // eqrr 3 1 3
j5:
    if (r3 == 0) goto j6; // addr 3 4 4
    else if (r3 == 1) goto j7; // ^
j6:
    goto j8; // addi 4 1 4
j7:
    r0 = r2 + r0; // addr 2 0 0
j8:
    r5 = r5 + 1; // addi 5 1 5
j9:
    r3 = (r5 > r1 ? 1 : 0); // gtrr 5 1 3
j10:
    if (r3 == 0) goto j11; // addr 4 3 4
    else if (r3 == 1) goto j12; // ^
j11:
    goto j3; // seti 2 7 4
j12:
    r2 = r2 + 1; // addi 2 1 2
j13:
    r3 = (r2 > r1 ? 1 : 0); // gtrr 2 1 3
j14:
    if (r3 == 0) goto j15; // addr 3 4 4
    else if (r3 == 1) goto j16; // ^
j15:
    goto j2; // seti 1 3 4
j16:
    printf("%ld %ld %ld %ld <rip> %ld\n", r0, r1, r2, r3, r5);
    return 0; // mulr 4 4 4

j17:
    r1 = r1 + 2; // addi 1 2 1
j18:
    r1 = r1 * r1; // mulr 1 1 1
j19:
    r1 = 19 * r1; // mulr 4 1 1 (r4 = 19)
j20:
    r1 = r1 * 11; // muli 1 11 1
j21:
    r3 = r3 + 3; // addi 3 3 3
j22:
    r3 = r3 * 22; // mulr 3 4 3 (r4 = 22)
j23:
    r3 = r3 + 9; // addi 3 9 3
j24:
    r1 = r1 + r3; // addr 1 3 1
j25:
    if (r0 == 0) goto j26; // addr 4 0 4
    else if (r0 == 1) goto j27; // ^
j26:
    goto j1; // seti 0 1 4
j27:
    r3 = 27; // setr 4 9 3 (r4 = 27)
j28:
    r3 = r3 * 28; // mulr 3 4 3 (r4 = 28)
j29:
    r3 = 29 + r3; // addr 4 3 3 (r4 = 29)
j30:
    r3 = 30 * r3; // mulr 4 3 3 (r4 = 30)
j31:
    r3 = r3 * 14; // muli 3 14 3
j32:
    r3 = r3 * 32; // mulr 3 4 3 (r4 = 32)
j33:
    r1 = r1 + r3; // addr 1 3 1
j34:
    r0 = 0; // seti 0 6 0
j35:
    goto j1; // seti 0 7 4
}
