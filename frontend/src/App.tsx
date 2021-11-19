import { useEffect } from 'react';
import LoginForm from 'components/account/LoginForm';
import Header from 'components/general/Header';
import { doRefresh, scheduleRefresh } from 'utils/account-utils';
import { Route, Routes } from 'react-router-dom';
import HomePage from 'pages/HomePage';
import ProtectedRoute from 'components/Router/ProtectedRoute';
import Test from 'pages/Test';

function App(): JSX.Element {
    useEffect(() => {
        const startupRefreshToken = async (): Promise<void> => {
            const result = await doRefresh();
            if (result.isSuccess) {
                scheduleRefresh();
            }
        };

        startupRefreshToken();
    }, []);

    return (
        <>
            <Header />
            <Routes>
                <Route path="/" element={<HomePage />} />
                <Route path="/login" element={<LoginForm />} />
                <Route path="/private" element={<ProtectedRoute />}>
                    <Route path="/private" element={<Test />} />
                </Route>
            </Routes>
        </>
    );
}

export default App;
