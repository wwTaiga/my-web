import Square from './Square';

interface props {
    squares: string[];
    onClick: (args: number) => void;
}
function Board(props: props): JSX.Element {
    function renderSquare(i: number) {
        return (
            <Square
                value={props.squares[i]}
                index={i}
                onClick={props.onClick}
            />
        );
    }

    return (
        <div>
            <div>
                {renderSquare(0)}
                {renderSquare(1)}
                {renderSquare(2)}
            </div>
            <div>
                {renderSquare(3)}
                {renderSquare(4)}
                {renderSquare(5)}
            </div>
            <div>
                {renderSquare(6)}
                {renderSquare(7)}
                {renderSquare(8)}
            </div>
        </div>
    );
}

export default Board;
