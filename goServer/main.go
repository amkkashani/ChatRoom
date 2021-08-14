// You can edit this code!
// Click here and start typing.
package main

import (
	"encoding/json"
	"fmt"
	"log"
	"net/http"
	"strconv"
	"time"
)

//structs
type payam struct {
	Owner string
	Text  string
}

// type Room struct {
// 	name     string
// 	massages []Massage
// }

func update(w http.ResponseWriter, req *http.Request) {
	index := req.URL.Query()["index"]
	indexNumber, _ := strconv.Atoi(string(index[0]))
	fmt.Print(indexNumber)

	var sendList []payam

	for j := 0; j < 14; j++ {
		if indexNumber <= len(massageList)-1 {
			for i := indexNumber; i < len(massageList); i++ {
				sendList = append(sendList, massageList[i])
			}
			break
		} else {
			time.Sleep(time.Millisecond * 500)
		}
	}
	output, _ := json.Marshal(sendList)
	fmt.Fprintf(w, string(output))
}

func dataSend(w http.ResponseWriter, req *http.Request) {

	decoder := json.NewDecoder(req.Body)
	var t payam
	err := decoder.Decode(&t)

	if err != nil {
		panic(err)
	}
	log.Println(t.Owner)
	massageList = append(massageList, t)
	log.Println(len(massageList))

	output, _ := json.Marshal(t)
	fmt.Fprint(w, string(output))
}

func getAllData(w http.ResponseWriter, req *http.Request) {

	test, _ := json.Marshal(massageList)
	print(string(test))
	fmt.Fprintf(w, string(test))

}

//variables
var m map[string]string
var massageList []payam

func main() {
	// res1D := &response1{
	// 	Page:   1,
	// 	Fruits: []string{"apple", "peach", "pear"}}
	// res1B, _ := json.Marshal(res1D)
	// fmt.Println(string(res1B))

	// input := &payam{
	// 	ID:    3,
	// 	Owner: "ali",
	// 	Text:  "salam"}
	// res, _ := json.Marshal(input)
	// fmt.Println(string(res))

	// var newPayam2 payam
	// json.Unmarshal([]byte(res), &newPayam2)
	// println(newPayam2.ID)

	// newMassage := payam{ID: 54, Owner: "hasan", Text: "salam be hame"}
	// massageList = append(massageList, newMassage)
	// massageList = append(massageList, newMassage)
	// fmt.Println(massageList)

	http.HandleFunc("/update", update)
	http.HandleFunc("/send", dataSend)
	http.HandleFunc("/all", getAllData)
	http.ListenAndServe(":8005", nil)

}
