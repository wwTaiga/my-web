import ProtectedRoute from 'components/Router/ProtectedRoute';
import Header from 'components/general/Header';
import HomePage from 'pages/HomePage';
import Test from 'pages/Test';
import ForgotPasswordPage from 'pages/account/ForgotPasswordPage';
import LoginPage from 'pages/account/LoginPage';
import RegisterPage from 'pages/account/RegisterPage';
import ResetPasswordPage from 'pages/account/ResetPasswordPage';
import { useEffect } from 'react';
import { Route, Routes } from 'react-router-dom';
import { doRefresh, scheduleRefresh } from 'utils/account-utils';

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
                <Route path="/reset-password" element={<ResetPasswordPage />} />
                <Route path="/home" element={<ProtectedRoute />}>
                    <Route path="/home" element={<Test />} />
                </Route>
            </Routes>
        </>
    );
};

export default App;
