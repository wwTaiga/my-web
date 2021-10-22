import TicTacToe from './pages/Tic-tac-toe';
import { ChakraProvider } from '@chakra-ui/react';

function App(): JSX.Element {
    return (
        <ChakraProvider>
            <TicTacToe />
        </ChakraProvider>
    );
}

export default App;
