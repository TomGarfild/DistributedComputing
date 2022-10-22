package main

import (
	"fmt"
	"math/rand"
	"sync"
	"time"
)

const ArraysCount = 3
const ArraySize = 10
const UpperBound = 5

var random = rand.New(rand.NewSource(time.Now().Unix()))

type ArrayGroup struct {
	arrays [][]int
	sync.WaitGroup
}

func createArrayGroup(arraysCount, arraySize, upperBound int) *ArrayGroup {
	return &ArrayGroup{arrays: initArrayGroup(arraysCount, arraySize, upperBound)}
}

func initArrayGroup(arraysCount, arraySize, upperBound int) [][]int {
	arrayList := make([][]int, arraysCount, arraySize)

	for i := 0; i < arraysCount; i++ {
		arrayList[i] = generateArray(arraySize, upperBound)
	}

	return arrayList
}

func generateArray(size, upperBound int) []int {
	array := make([]int, size)

	for i := 0; i < size; i++ {
		array[i] = random.Intn(upperBound)
	}

	return array
}

func printArrayGroup(arrayGroup *ArrayGroup) {
	for i := 0; i < len(arrayGroup.arrays); i++ {
		fmt.Println(arrayGroup.arrays[i])
	}

	fmt.Println()
}

func run(arrayGroup *ArrayGroup, waitGroup *sync.WaitGroup, arraysCount, arraySize, upperBound int) {
	stop := false

	for !stop {
		waitGroup.Add(arraysCount)

		for i := 0; i < arraysCount; i++ {
			go changeElement(arrayGroup, waitGroup, i, arraySize, upperBound)
		}

		waitGroup.Wait()

		if checkRule(arrayGroup, arraysCount) {
			stop = true
			fmt.Println("ArrayGroup sum are equal. Terminate...")
		}

		printArrayGroup(arrayGroup)
	}
}

func Abs(x int) int {
	if x < 0 {
		return -x
	}
	return x
}

func changeElement(arrayGroup *ArrayGroup, waitGroup *sync.WaitGroup, currentArrayIndex, arraySize, upperBound int) {
	elemToChange := random.Intn(arraySize)
	sign := 1

	if random.Intn(2) == 1 {
		sign = -1
	}

	if Abs(arrayGroup.arrays[currentArrayIndex][elemToChange]) < upperBound {
		arrayGroup.arrays[currentArrayIndex][elemToChange] += sign
	}

	time.Sleep(100 * time.Millisecond)

	waitGroup.Done()
}

func checkRule(arrayGroup *ArrayGroup, arraysCount int) bool {
	sumArray := make([]int, arraysCount)

	for i := 0; i < arraysCount; i++ {
		currentArraySum := 0

		for _, j := range arrayGroup.arrays[i] {
			currentArraySum += j
		}

		sumArray[i] = currentArraySum
	}

	return checkSumEquality(sumArray)
}

func checkSumEquality(array []int) bool {
	fmt.Println("Sums: ", array)

	for i := range array {
		if array[0] != array[i] {
			return false
		}
	}

	return true
}

func main() {
	arrayGroup := createArrayGroup(ArraysCount, ArraySize, UpperBound)
	waitGroup := new(sync.WaitGroup)
	run(arrayGroup, waitGroup, ArraysCount, ArraySize, UpperBound)
}
