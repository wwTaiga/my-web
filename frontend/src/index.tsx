import { ChakraProvider } from '@chakra-ui/react';
import App from 'App';
import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import { BrowserRouter } from 'react-router-dom';
import { store } from 'store/store';

ReactDOM.render(
    <React.StrictMode>
        <Provider store={store}>
            <ChakraProvider>
                <BrowserRouter>
                    <App />
                </BrowserRouter>
            </ChakraProvider>
        </Provider>
    </React.StrictMode>,
    document.getElementById('root'),
);
