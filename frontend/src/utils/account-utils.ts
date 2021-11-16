import { AuthToken, AuthTokenError, PasswordModel, RefreshModel, Result } from 'types';
import { getTokenUrl } from 'utils/url-utils';

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

export const doLogin = async (username: string, password: string): Promise<Result> => {
    const loginForm: PasswordModel = {
        username: username,
        password: password,
    };
    const result = await getToken(loginForm, 'password');
    if (result) {
        scheduleRefresh();
    }
    return result;
};

const getToken = async (data: RefreshModel | PasswordModel, grantType: string): Promise<Result> => {
    Object.assign(data, {
        grant_type: grantType,
        scope: 'openid offline_access',
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
    const response = await fetch(getTokenUrl(), options);

    if (response.status == 400) {
        const result: AuthTokenError = await response.json();
        return { isSuccess: false, errorDesc: result.error_description };
    } else if (!response.ok) {
        return { isSuccess: false, errorDesc: await response.json() };
    }
    const newToken: AuthToken = await response.json();
    saveToken(newToken);

    return { isSuccess: true };
};

const saveToken = (newToken: AuthToken) => {
    const previousToken = retrieveToken();
    // For not rolling refresh token
    if (previousToken != null && newToken.refresh_token == null) {
        newToken.refresh_token == previousToken.refresh_token;
    }
    localStorage.setItem('authToken', JSON.stringify(newToken));
};

export const retrieveToken = (): AuthToken | null => {
    const authTokenString = localStorage.getItem('authToken');
    const authToken: AuthToken = authTokenString == null ? null : JSON.parse(authTokenString);
    return authToken;
};

export const removeToken = (): void => {
    localStorage.removeItem('authToken');
};

const scheduleRefresh = (): void => {
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
