import { useEffect } from 'react';
import LoginForm from 'components/account/LoginForm';
import Header from 'components/general/Header';
import { doRefresh, scheduleRefresh } from 'utils/account-utils';
import { Route, Routes } from 'react-router-dom';
import CallToActionWithAnnotation from 'pages/homepage';

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
                <Route path="/" element={<CallToActionWithAnnotation />} />
                <Route path="/login" element={<LoginForm />} />
            </Routes>
        </>
    );
}

export default App;
