import { PayloadAction, createSlice } from '@reduxjs/toolkit';
import { retrieveToken } from 'utils/account-utils';

interface AccountState {
    isLoggedIn: boolean;
}

const initialState: AccountState = {
    isLoggedIn: retrieveToken() != null ? true : false,
};

export const accountSlice = createSlice({
    name: 'account',
    initialState,
    reducers: {
        setIsLoggedIn: (state, action: PayloadAction<boolean>) => {
            state.isLoggedIn = action.payload;
        },
    },
});

export const { setIsLoggedIn } = accountSlice.actions;

export default accountSlice.reducer;
