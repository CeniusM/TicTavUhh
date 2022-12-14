package main

func main() {

	conn := dialTCP()

	sendToTCP(conn, 8)

	received := receiveFromTCP(conn)
	println("Received message:", string(received))
}
