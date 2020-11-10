// You can edit this code!
// Click here and start typing.
package main

import (
	"fmt"
	"net/http"
	"time"
)

func dataGet(w http.ResponseWriter, req *http.Request) {
	fmt.Fprintf(w, "salam kashi hastam")
	time.Sleep(10 * time.Second)
	fmt.Fprintf(w, "این هم بقیه اش")
}

func dataSend(w http.ResponseWriter, req *http.Request) {
	fmt.Fprintf(w, "test")
}

func main() {
	http.HandleFunc("/see", dataGet)
	http.HandleFunc("/send", dataSend)
	http.ListenAndServe(":8005", nil)
}
