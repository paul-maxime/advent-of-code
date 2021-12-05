package main

import (
	"fmt"
	"strings"
)

const input string = "1321131112"

func applyLookAndSay(number string) string {
	var output strings.Builder
	var previousDigit = -1
	var occurrences = 0

	for _, c := range number {
		var digit = int(c - '0')
		if digit == previousDigit {
			occurrences++
		} else {
			if occurrences > 0 {
				output.WriteString(fmt.Sprintf("%d%d", occurrences, previousDigit))
			}
			previousDigit = digit
			occurrences = 1
		}
	}
	if occurrences > 0 {
		output.WriteString(fmt.Sprintf("%d%d", occurrences, previousDigit))
	}

	return output.String()
}

func main() {
	var current = input
	for i := 1; i <= 50; i++ {
		current = applyLookAndSay(current)
		fmt.Println(i, len(current))
	}
}
