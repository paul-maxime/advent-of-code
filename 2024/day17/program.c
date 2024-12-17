#include <stdio.h>

int main(void)
{
    long long int secret = 30886132;

    do {
        long long int current = secret & 0b111;

        long long int delta = current ^ 0b001;
        long long int xorKey = (secret >> delta) & 0b111;
        long long int output = current ^ xorKey ^ 0b101;

        secret >>= 3;
        printf("output %lld\n", delta, xorKey, output);
    } while (secret > 0);

    return 0;
}
