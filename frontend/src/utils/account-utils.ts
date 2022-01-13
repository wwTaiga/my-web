import jwtDecode from 'jwt-decode';
import { AuthToken, AuthTokenError, LoginModel, RefreshModel, Result, UserProfile } from 'types';
import { handleFetchError, jsonFetch } from 'utils/fetch-utils';
import { urls } from 'utils/url-utils';

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
    const refreshToken: RefreshModel = {
        refresh_token: authToken.refresh_token,
        rememberMe: authToken.rememberMe,
    };
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
export const doLogin = async (loginModel: LoginModel): Promise<Result> => {
    const result = await getToken(loginModel, 'password');
    if (result.isSuccess) {
        scheduleRefresh();
    }
    return result;
};

/**
 * Get access token and refresh token from core api, save tokens and user info to local storage
 * if success.
 *
 * @params data - The required data that need for get tokens from core api
 * @params grantType - The workflow used to get the tokens
 *
 * @returns Result promise
 **/
const getToken = async (data: LoginModel | RefreshModel, grantType: string): Promise<Result> => {
    Object.assign(data, {
        grant_type: grantType,
        scope: 'openid offline_access profile roles',
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
    const response = await fetch(urls.account.token(), options).catch((error: TypeError) =>
        handleFetchError(error),
    );

    if (response.status == 400) {
        const result: AuthTokenError = await response.json();
        return { isSuccess: false, status: response.status, errorDesc: result.error_description };
    } else if (!response.ok) {
        return {
            isSuccess: false,
            status: response.status,
            errorDesc: response.statusText,
        };
    }
    const newToken: AuthToken = await response.json();
    newToken.rememberMe = data.rememberMe;
    saveToken(newToken);

    return { isSuccess: true };
};

/**
 * Save token object to local storage with key 'authToken'.
 *
 * @param newToken - The tokens object get from api core
 **/
const saveToken = (newToken: AuthToken) => {
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
 * Retrieve user profile from local storage.
 *
 * @returns User profile object if found else null
 * */
export const retrieveUserProfile = (): UserProfile | null => {
    const token = retrieveToken();
    if (token == null) {
        return null;
    }
    const userProfile: UserProfile = jwtDecode(token.id_token);
    return userProfile;
};

/**
 * Remove tokens and object from local storage.
 **/
export const removeToken = (): void => {
    localStorage.removeItem('authToken');
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

/**
 * Check if email is exist
 *
 * @param email - User email
 *
 * @returns Boolean promise
 **/
export const isEmailExist = async (email: string): Promise<boolean> => {
    const result: Result = await jsonFetch.get(urls.account.isEmailExist(email));
    if (!result.isSuccess) {
        return false;
    }

    if (
        result.data != null &&
        result.data.isExist != null &&
        typeof result.data.isExist === 'boolean'
    )
        return result.data.isExist;
    return false;
};
