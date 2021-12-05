package main

import "strings"

const input string = "vzbxkghb"

func hasIncreasingStraight(password string) bool {
	for i := 2; i < len(password); i++ {
		if password[i-2] == password[i]-2 && password[i-1] == password[i]-1 {
			return true
		}
	}
	return false
}

func hasTwoPairOfLetters(password string) bool {
	var firstPair byte = 0
	for i := 1; i < len(password); i++ {
		if password[i] == password[i-1] && password[i] != firstPair {
			if firstPair == 0 {
				firstPair = password[i]
			} else {
				return true
			}
		}
	}
	return false
}

func isValid(password string) bool {
	return !strings.ContainsAny(password, "iol") && hasTwoPairOfLetters(password) && hasIncreasingStraight(password)
}

func nextPassword(password string) string {
	var output = ""
	var remainer = 1
	for i := len(password) - 1; i >= 0; i-- {
		var n = password[i] + byte(remainer)
		if n > 'z' {
			n = 'a'
			remainer = 1
		} else {
			remainer = 0
		}
		output = string(n) + output
	}
	return output
}

func nextValidPassword(password string) string {
	for {
		password = nextPassword(password)
		if isValid(password) {
			return password
		}
	}
}

func main() {
	var nextPassword = nextValidPassword(input)
	var nextNextPassword = nextValidPassword(nextPassword)
	println(input, "->", nextPassword, "->", nextNextPassword)
}
