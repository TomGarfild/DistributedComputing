package main

import "fmt"

type Bidder struct {
	name         string
	currentQueue int
}

func manipulateQueue(queue1 chan Bidder, queue2 chan Bidder, currentQueue int, exit chan bool) {
	for len(queue1) > 0 || len(queue2) > 0 {
		if len(queue1) == 0 {
			bidder := <-queue2
			queue1 <- bidder
			fmt.Println("Bidder ", bidder.name, " moved to ", currentQueue, " queue")
		}
		bidder := <-queue1
		fmt.Println("Serviced bidder ", bidder.name, " in ", currentQueue, " queue")
		fmt.Println(currentQueue, "queue has", len(queue1), "bidders")
		len1 := len(queue1)
		len2 := len(queue2)
		if len1 > len2+1 {
			bidder := <-queue1
			queue2 <- bidder
			fmt.Println("Bidder", bidder.name, "left", currentQueue, "queue")
		}
	}
	exit <- true
}

func main() {
	bidder1 := Bidder{name: "Oleksii", currentQueue: 1}
	bidder2 := Bidder{name: "Roman", currentQueue: 1}
	bidder3 := Bidder{name: "Stas", currentQueue: 2}
	bidder4 := Bidder{name: "Viktor", currentQueue: 2}
	bidder5 := Bidder{name: "Sasha", currentQueue: 1}

	queue1 := make(chan Bidder, 5)
	queue2 := make(chan Bidder, 5)
	exit := make(chan bool, 1)

	queue1 <- bidder1
	queue1 <- bidder2
	queue1 <- bidder5

	queue2 <- bidder3
	queue2 <- bidder4

	go manipulateQueue(queue1, queue2, 1, exit)
	go manipulateQueue(queue2, queue1, 2, exit)
	<-exit
}
