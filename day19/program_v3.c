#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>

int main(void) {
    const bool isSecondStep = true;

    int64_t result = 0;
    int64_t target = (isSecondStep ? 10551311 : 911);

    for (int64_t i = 1; i <= target; ++i) {
        for (int64_t j = 1; j <= target / i; ++j) { // optimization!
            if (i * j == target) {
                result += i;
            }
        }
    }

    printf("%ld\n", result);
    return 0;
}
