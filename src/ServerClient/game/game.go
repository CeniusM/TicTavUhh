package game

type SquareState int

type Player int

type GameState struct {
	Board      [3][3]SquareState
	TurnPlayer Player
}

const (
	None   = iota
	Cross  = iota
	Circle = iota
)

var Board [3][3]SquareState

var TurnPlayer Player

func (state *GameState) DrawBoard() {
	for i, row := range state.Board {
		for j, square := range row {
			print(" ")
			switch square {
			case None:
				print(" ")
			case Cross:
				print("X")
			case Circle:
				print("O")
			}
			if j != len(row)-1 {
				print(" |")
			}
		}
		if i != len(state.Board)-1 {
			print("\n-----------")
		}
		print("\n")
	}
}

func TranslateReceived(received []byte) {
	if received[len(received)-1] == 1 {
		println("someone won, idk wtf the last byte is when someone wins")
	} else {

	}
}
