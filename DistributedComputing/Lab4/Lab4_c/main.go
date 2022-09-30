package main

import (
	"fmt"
	"math/rand"
	"sync"
	"time"
)

const Duration = 5000

type Route struct {
	destination string
	price       int
}

type Graph struct {
	adjacencyList map[string][]*Route
	lock          sync.RWMutex
}

func (graph *Graph) init() {
	graph.adjacencyList = make(map[string][]*Route)
}

func (graph *Graph) getRoute(from, to string) (*Route, int) {
	if graph.adjacencyList[from] == nil {
		return nil, -1
	}

	for i := 0; i < len(graph.adjacencyList[from]); i++ {
		current := graph.adjacencyList[from][i]
		if current.destination == to {
			return current, i
		}
	}

	return nil, -1
}

func (graph *Graph) addRoute(from, to string, price int) bool {
	if (from == to) || graph.adjacencyList[from] == nil || graph.adjacencyList[to] == nil {
		return false
	}

	_, routeIdx := graph.getRoute(from, to)

	if routeIdx != -1 {
		return false
	}

	graph.adjacencyList[from] = append(graph.adjacencyList[from], &Route{to, price})
	graph.adjacencyList[to] = append(graph.adjacencyList[to], &Route{from, price})

	return true
}

func (graph *Graph) addCity(name string) bool {
	if graph.adjacencyList[name] != nil {
		return false
	}

	graph.adjacencyList[name] = make([]*Route, 0)

	return true
}

func removeByIndex[T any](slice []T, i int) []T {
	return append(slice[:i], slice[i+1:]...)
}

func (graph *Graph) removeCity(city string) bool {
	if graph.adjacencyList[city] == nil {
		return false
	}

	for currentCity, _ := range graph.adjacencyList {
		if currentCity == city {
			continue
		}

		graph.removeRoute(currentCity, city)
	}

	delete(graph.adjacencyList, city)

	return true
}

func (graph *Graph) removeRoute(from, to string) bool {
	_, idxFrom := graph.getRoute(from, to)

	if idxFrom == -1 {
		return false
	}

	_, idxTo := graph.getRoute(to, from)

	graph.adjacencyList[from] = removeByIndex(graph.adjacencyList[from], idxFrom)
	graph.adjacencyList[to] = removeByIndex(graph.adjacencyList[to], idxTo)

	return true
}

func (graph *Graph) editRoutePrice(from, to string, newPrice int) bool {
	routeTo, _ := graph.getRoute(from, to)

	if routeTo == nil {
		return false
	}

	routeFrom, _ := graph.getRoute(to, from)

	routeTo.price = newPrice
	routeFrom.price = newPrice

	return true
}

func (graph *Graph) print() {
	for city, list := range graph.adjacencyList {
		fmt.Printf("From : %s\n", city)

		for i := 0; i < len(list); i++ {
			fmt.Printf("%s : %d\n", list[i].destination, list[i].price)
		}

		fmt.Println("----------------------------------------")
	}
}

func priceEditor(graph *Graph, cities []string) {
	from := cities[rand.Int()%len(cities)]
	to := cities[rand.Int()%len(cities)]

	if from == to {
		fmt.Println("PriceEditor: same city")
		return
	}

	route, _ := graph.getRoute(from, to)
	if route == nil {
		if graph.adjacencyList[from] == nil || graph.adjacencyList[to] == nil {
			fmt.Printf("PriceEditor: there is no %s or %s in graph\n", from, to)
		} else {
			fmt.Printf("PriceEditor: there is no route from %s to %s\n", from, to)
		}
	} else {
		oldPrice := route.price
		newPrice := rand.Intn(2000)
		graph.editRoutePrice(from, to, newPrice)
		fmt.Printf("PriceEditor: changed price from %s to %s(before = %d, after = %d)\n", from, to, oldPrice, newPrice)
	}
}

func routeEditor(graph *Graph, cities []string) {
	from := cities[rand.Int()%len(cities)]
	to := cities[rand.Int()%len(cities)]
	toRemove := rand.Intn(2) == 0

	if toRemove {
		if graph.removeRoute(from, to) {
			fmt.Printf("RouteEditor: removed route from %s to %s\n", from, to)
		} else {
			fmt.Printf("RouteEditor: there is no route from %s to %s\n", from, to)
		}
	} else {
		price := rand.Intn(2000)

		if graph.addRoute(from, to, price) {
			fmt.Printf("RouteEditor: added route from %s to %s, price: %d\n", from, to, price)
		} else {
			route, _ := graph.getRoute(from, to)

			if route != nil {
				fmt.Printf("RouteEditor: the route from %s to %s already exists\n", from, to)
			} else if graph.adjacencyList[from] == nil || graph.adjacencyList[to] == nil {
				fmt.Printf("RouteEditor: there is no city %s or %s in graph\n", from, to)
			}
		}
	}
}

func cityEditor(graph *Graph, cities []string) {
	city := cities[rand.Int()%len(cities)]
	toRemove := rand.Intn(2) == 0

	if toRemove {
		if graph.removeCity(city) {
			fmt.Printf("CityEditor: successfully removed %s city\n", city)
		} else {
			fmt.Printf("CityEditor: %s doesn't exist\n", city)
		}
	} else {
		if graph.addCity(city) {
			fmt.Printf("CityEditor: successfully added %s city\n", city)
		} else {
			fmt.Printf("CityEditor: %s already exists\n", city)
		}
	}
}

func routeFinder(graph *Graph, cities []string) {
	from := cities[rand.Int()%len(cities)]
	to := cities[rand.Int()%len(cities)]
	route, _ := graph.getRoute(from, to)

	if route != nil {
		fmt.Printf("RouteFinder: found route from %s to %s, price: %d\n", from, to, route.price)
	} else {
		fmt.Printf("RouteFinder: there is no route from %s to %s\n", from, to)
	}
}

func start(action func(*Graph, []string), graph *Graph, cities []string) {
	for {
		graph.lock.Lock()
		time.Sleep(Duration * time.Millisecond)
		action(graph, cities)
		graph.lock.Unlock()
	}
}

func main() {
	cities := []string{"Kyiv", "Zaporizhzhia", "Dnipro", "Lviv", "Kharkiv", "Donetsk"}
	graph := Graph{}
	graph.init()

	for i := 0; i < len(cities); i++ {
		graph.addCity(cities[i])
	}

	graph.addRoute("Kyiv", "Zaporizhzhia", 1)
	graph.addRoute("Kyiv", "Kharkiv", 2)
	graph.addRoute("Kyiv", "Dnipro", 3)
	graph.addRoute("Kyiv", "Lviv", 4)
	graph.addRoute("Zaporizhzhia", "Dnipro", 5)
	graph.addRoute("Kharkiv", "Dnipro", 5)

	go start(priceEditor, &graph, cities)
	go start(routeEditor, &graph, cities)
	go start(cityEditor, &graph, cities)
	go start(routeFinder, &graph, cities)

	for {
	}
}
