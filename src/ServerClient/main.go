package main

import (
	"serverclient/game"
)

func main() {

	/*conn := client.DialTCP()

	client.SendToTCP(conn, 8)

	received := client.ReceiveFromTCP(conn)
	println("Received message:", string(received))*/

	state := game.GameState{}

	state.Board[0][1] = game.Cross
	state.Board[1][1] = game.Circle

	state.DrawBoard()
}
