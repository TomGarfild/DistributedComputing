package main

import (
	"fmt"
	"math/rand"
	"time"
)

const Duration = 3000

type Components struct {
	componentsArray [3]bool
}

func (c *Components) printComponents() string {
	componentsName := [3]string{"tobacco", "paper", "matches"}
	var result string

	for i := 0; i < len(c.componentsArray); i++ {
		if !c.componentsArray[i] {
			continue
		}

		result += componentsName[i] + ", "
	}

	return result[:len(result)-2]
}

func getComponent() (*Components, int) {
	array := [3]bool{true, true, true}
	index := rand.Int() % len(array)
	array[index] = false
	return &Components{array}, index
}

func mediator(pingChanArray []chan *Components, semaphore chan bool) {
	for {
		semaphore <- true
		toPush, idx := getComponent()
		fmt.Println("Mediator gave: ", toPush.printComponents())
		pingChanArray[idx] <- toPush
	}
}

type Smoker struct {
	name         string
	mediatorChan chan *Components
}

func (smoker *Smoker) smoke(semaphore chan bool) {
	for {
		<-smoker.mediatorChan
		fmt.Printf("%s started smoking...\n", smoker.name)
		time.Sleep(Duration * time.Millisecond)
		fmt.Printf("%s finished smoking...\n", smoker.name)
		<-semaphore
	}
}

func main() {
	tobaccoOwner := Smoker{"Tobacco owner", make(chan *Components)}
	paperOwner := Smoker{"Paper owner", make(chan *Components)}
	matchesOwner := Smoker{"Matches owner", make(chan *Components)}

	semaphore := make(chan bool, 1)
	ownersMediatorChan := []chan *Components{tobaccoOwner.mediatorChan, paperOwner.mediatorChan, matchesOwner.mediatorChan}

	go mediator(ownersMediatorChan, semaphore)
	go tobaccoOwner.smoke(semaphore)
	go paperOwner.smoke(semaphore)
	go matchesOwner.smoke(semaphore)

	for {
		time.Sleep(1000 * time.Millisecond)
	}
}
