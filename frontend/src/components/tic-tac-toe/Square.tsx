import classes from './Square.module.css';

interface props {
    index: number;
    value: string | null;
    onClick: (args: number) => void;
}

function Square(props: props): JSX.Element {
    return (
        <button
            className={classes.square}
            onClick={() => props.onClick(props.index)}
        >
            {props.value}
        </button>
    );
}

export default Square;
