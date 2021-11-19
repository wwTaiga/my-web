import { Navigate, Outlet } from 'react-router-dom';
import { useAppSelector } from 'store/hooks';

const ProtectedRoute = (): JSX.Element => {
    const isLoggedIn = useAppSelector((state) => state.account.isLoggedIn);

    return isLoggedIn ? <Outlet /> : <Navigate to="/login" />;
};
export default ProtectedRoute;
