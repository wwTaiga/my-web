import Board from 'components/tic-tac-toe/Board';
import { useState } from 'react';

interface props {
    squares: string[];
    nextIsX: boolean;
}
function TicTacToe(): JSX.Element {
    const [state, setState] = useState<props>({
        squares: Array(9).fill(null),
        nextIsX: true,
    });

    function findWinner(squares: string[]): string | null {
        const winConditions: number[][] = [
            [0, 1, 2],
            [3, 4, 5],
            [6, 7, 8],
            [0, 3, 6],
            [1, 4, 7],
            [2, 5, 8],
            [0, 4, 8],
            [2, 4, 6],
        ];

        for (const winCondition of winConditions) {
            const [a, b, c] = winCondition;
            if (squares[a] && squares[a] == squares[b] && squares[b] == squares[c]) {
                return squares[a];
            }
        }
        return null;
    }

    function handleClick(i: number) {
        const squares = state.squares.slice();
        if (findWinner(squares) || squares[i]) {
            return;
        }
        squares[i] = state.nextIsX ? 'O' : 'X';
        setState({
            squares: squares,
            nextIsX: !state.nextIsX,
        });
    }

    const winner = findWinner(state.squares);
    const player = state.nextIsX ? 'O' : 'X';

    return (
        <div>
            <h1>{winner ? 'Winner: ' + winner : 'Next player: ' + player}</h1>
            <Board squares={state.squares} onClick={handleClick} />
        </div>
    );
}

export default TicTacToe;
