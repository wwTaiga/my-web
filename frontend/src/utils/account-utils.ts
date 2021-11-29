import { AuthToken, AuthTokenError, PasswordModel, RefreshModel, Result } from 'types';
import { getTokenUrl } from 'utils/url-utils';
import { handleFetchError } from 'utils/fetch-utils';
import { useAppDispatch } from 'store/hooks';
import { setIsLoggedIn } from 'store/account/accountSlice';

/**
 * If refresh token exist in local storage, use it token to get a new access token and refresh
 * token.
 * If failed to get new access token and refresh token with existing refresh token, remove the
 * authToken in local storage.
 *
 * @returns Result promise
 **/
export const doRefresh = async (): Promise<Result> => {
    const authToken = retrieveToken();
    if (authToken == null || authToken.refresh_token == null) {
        return { isSuccess: false, errorDesc: 'Refresh token not found.' };
    }
    const refreshToken: RefreshModel = { refresh_token: authToken.refresh_token };
    const result = await getToken(refreshToken, 'refresh_token');
    if (!result.isSuccess) {
        removeToken();
    }
    return result;
};

/**
 * Do login.
 * Schedule refresh token task if login successfully.
 *
 * @params username - Username
 * @params password - User password
 *
 * @returns Result promise
 **/
export const doLogin = async (username: string, password: string): Promise<Result> => {
    const loginForm: PasswordModel = {
        username: username,
        password: password,
    };
    const result = await getToken(loginForm, 'password');
    if (result.isSuccess) {
        scheduleRefresh();
    }
    return result;
};

/**
 * Get access token and refresh token from core api, save tokens to local storage if success.
 *
 * @params data - The required data that need for get tokens from core api
 * @params grantType - The workflow used to get the tokens
 *
 * @returns Result promise
 **/
const getToken = async (data: RefreshModel | PasswordModel, grantType: string): Promise<Result> => {
    Object.assign(data, {
        grant_type: grantType,
        scope: 'openid offline_access profile roles',
        rememberme: 'true',
    });

    const params = new URLSearchParams();
    for (const [key, value] of Object.entries(data)) {
        params.append(key, value);
    }

    const options = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
        },
        body: new URLSearchParams(params),
    };
    const response = await fetch(getTokenUrl(), options).catch((error: TypeError) =>
        handleFetchError(error),
    );

    if (response.status == 400) {
        const result: AuthTokenError = await response.json();
        return { isSuccess: false, errorDesc: result.error_description };
    } else if (!response.ok) {
        return { isSuccess: false, errorDesc: response.status + ': ' + response.statusText };
    }
    const newToken: AuthToken = await response.json();
    saveToken(newToken);

    return { isSuccess: true };
};

/**
 * Save tokens object to local storage with key 'authToken'.
 *
 * @param newToken - The tokens object get from api core
 **/
const saveToken = (newToken: AuthToken) => {
    const previousToken = retrieveToken();
    // For not rolling refresh token
    if (previousToken != null && newToken.refresh_token == null) {
        newToken.refresh_token == previousToken.refresh_token;
    }
    localStorage.setItem('authToken', JSON.stringify(newToken));
};

/**
 * Retrieve tokens object from local storage.
 *
 * @returns Tokens object if found else null
 **/
export const retrieveToken = (): AuthToken | null => {
    const authTokenString = localStorage.getItem('authToken');
    const authToken: AuthToken = authTokenString == null ? null : JSON.parse(authTokenString);
    return authToken;
};

/**
 * Remove tokens object from local storage and set IsLoggedIn (Redux state) to
 * false.
 **/
export const removeToken = (): void => {
    localStorage.removeItem('authToken');

    const dispatch = useAppDispatch();
    dispatch(setIsLoggedIn(false));
};

/**
 * Schedule a doRefresh task to get new access token and refresh token 20 seconds before the
 * existing access token expire.
 **/
export const scheduleRefresh = (): void => {
    const authToken = retrieveToken();
    if (authToken != null && authToken.refresh_token != null) {
        setTimeout(async () => {
            const result = await doRefresh();
            if (result.isSuccess) {
                scheduleRefresh();
            }
        }, (authToken.expires_in - 20) * 1000);
    }
};
