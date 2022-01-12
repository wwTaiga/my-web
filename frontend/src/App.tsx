import { useEffect } from 'react';
import Header from 'components/general/Header';
import { doRefresh, scheduleRefresh } from 'utils/account-utils';
import { Route, Routes } from 'react-router-dom';
import HomePage from 'pages/HomePage';
import ProtectedRoute from 'components/Router/ProtectedRoute';
import Test from 'pages/Test';
import LoginPage from 'pages/account/LoginPage';
import RegisterPage from 'pages/account/RegisterPage';
import ForgotPasswordPage from 'pages/account/ForgotPasswordPage';

const App = (): JSX.Element => {
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
                <Route path="/login" element={<LoginPage />} />
                <Route path="/register" element={<RegisterPage />} />
                <Route path="/forgot-password" element={<ForgotPasswordPage />} />
                <Route path="/home" element={<ProtectedRoute />}>
                    <Route path="/home" element={<Test />} />
                </Route>
            </Routes>
        </>
    );
};

export default App;
