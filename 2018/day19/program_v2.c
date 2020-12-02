#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>

int main(void) {
    int64_t r0 = 1, r1 = 0, r2 = 0, r3 = 0, r5 = 0;

    r1 += 2; // addi 1 2 1
    r1 *= r1; // mulr 1 1 1
    r1 *= 19; // mulr 4 1 1 (r4 = 19)
    r1 *= 11; // muli 1 11 1
    r3 += 3; // addi 3 3 3
    r3 *= 22; // mulr 3 4 3 (r4 = 22)
    r3 += 9; // addi 3 9 3
    r1 += r3; // addr 1 3 1

    if (r0 == 1) {
        r3 = 27; // setr 4 9 3 (r4 = 27)
        r3 *= 28; // mulr 3 4 3 (r4 = 28)
        r3 += 29; // addr 4 3 3 (r4 = 29)
        r3 *= 30; // mulr 4 3 3 (r4 = 30)
        r3 *= 14; // muli 3 14 3
        r3 *= 32; // mulr 3 4 3 (r4 = 32)
        r1 += r3; // addr 1 3 1
        r0 = 0; // seti 0 6 0
    }

loop1:
    r2 = 1; // seti 1 7 2
loop2:
    printf("%ld %ld %ld %ld <rip> %ld\n", r0, r1, r2, r3, r5);
    r5 = 1; // seti 1 1 5
loop3:
    if ((r2 * r5) == r1) {
        r0 += r2;
    }
    r5 = r5 + 1; // addi 5 1 5
    if (r5 <= r1) goto loop3; // addr 4 3 4
    r2 = r2 + 1; // addi 2 1 2
    if (r2 <= r1) goto loop2; // addr 3 4 4
    printf("%ld %ld %ld %ld <rip> %ld\n", r0, r1, r2, r3, r5);
    return 0; // mulr 4 4 4
}
