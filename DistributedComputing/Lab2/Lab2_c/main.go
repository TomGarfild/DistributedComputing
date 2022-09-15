package main

import (
	"fmt"
	"math/rand"
	"time"
)

func getWinner(monks []int, left, right int) int {
	if monks[left] < monks[right] {
		return right
	}

	return left
}

func getMonastery(index int) string {
	if index%2 == 0 {
		return "Guan-Yin"
	}

	return "Guan-Yan"
}

func fight(monks []int, left, right int, pipe chan int) {
	var winner int
	if right-left < 2 {
		winner = getWinner(monks, left, right)
	} else {
		mid := (left + right) / 2
		var subpipe = make(chan int, 2)
		go fight(monks, left, mid, subpipe)
		fight(monks, mid+1, right, subpipe)
		firstWinner := <-subpipe
		secondWinner := <-subpipe
		winner = getWinner(monks, firstWinner, secondWinner)
	}
	pipe <- winner
}

func main() {
	r := rand.New(rand.NewSource(time.Now().UnixNano()))
	var monks []int

	for i := 0; i < 100; i++ {
		monks = append(monks, r.Intn(100)+1)
	}

	var pipe = make(chan int, 1)
	fight(monks, 0, len(monks)-1, pipe)
	var winnerIdx = <-pipe
	fmt.Printf("Winner is %d monk with power %d, from %s monastery", winnerIdx+1, monks[winnerIdx], getMonastery(winnerIdx))
}
