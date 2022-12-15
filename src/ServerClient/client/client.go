package client

import (
	"bytes"
	"encoding/binary"
	"net"
	"os"
)

const (
	HOST = "DESKTOP-H2H53J5"
	PORT = "1234"
	TYPE = "tcp"
)

func DialTCP() *net.TCPConn {
	tcpServer, err := net.ResolveTCPAddr(TYPE, HOST+":"+PORT)
	if err != nil {
		println("resolveTCPAddr failed:", err.Error())
		os.Exit(1)
	}

	conn, err := net.DialTCP(TYPE, nil, tcpServer)
	if err != nil {
		println("Dial failed:", err.Error())
		os.Exit(1)
	}

	return conn
}

func SendToTCP(conn net.Conn, payload int32) {
	buf := new(bytes.Buffer)
	err := binary.Write(buf, binary.LittleEndian, payload)
	buf.WriteTo(conn)
	// _, err := conn.Write([]byte(payload))
	if err != nil {
		println("Failed to write data:", err.Error())
		os.Exit(1)
	}
}

func ReceiveFromTCP(conn net.Conn) []byte {
	received := make([]byte, 1024)
	_, err := conn.Read(received)
	if err != nil {
		println("Read data failed:", err.Error())
		os.Exit(1)
	}
	return received
}
