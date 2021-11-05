import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface AccountState {
    token: string;
}

const initialState: AccountState = {
    token: '',
};

export const accountSlice = createSlice({
    name: 'account',
    initialState,
    reducers: {
        saveAccount: (state, action: PayloadAction<string>) => {
            state.token = action.payload;
        },
    },
});

export const { saveAccount } = accountSlice.actions;

export default accountSlice.reducer;
