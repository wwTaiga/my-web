import { AuthToken, PasswordModel, RefreshModel } from 'types';
import { getTokenUrl } from 'utils/url-utils';

export const doRefresh = async (): Promise<boolean> => {
    const authToken = retrieveToken();
    if (authToken == null || authToken.refresh_token == null) {
        return false;
    }

    const refreshToken: RefreshModel = { refresh_token: authToken.refresh_token };

    return await getToken(refreshToken, 'refresh_token');
};

export const doLogin = async (username: string, password: string): Promise<boolean | string> => {
    const loginForm: PasswordModel = {
        username: username,
        password: password,
    };
    return await getToken(loginForm, 'password');
};

const getToken = async (
    data: RefreshModel | PasswordModel,
    grantType: string,
): Promise<boolean> => {
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

    if (!response.ok) {
        return false;
    }
    const newToken: AuthToken = await response.json();
    saveToken(newToken);

    return true;
};

const saveToken = (newToken: AuthToken) => {
    const previousToken = retrieveToken();
    // For not rolling refresh token
    if (previousToken != null && newToken.refresh_token == null) {
        newToken.refresh_token == previousToken.refresh_token;
    }
    localStorage.setItem('authToken', JSON.stringify(newToken));
};

const retrieveToken = (): AuthToken | null => {
    const tokenString = localStorage.getItem('authToken');
    const token: AuthToken = tokenString == null ? null : JSON.parse(tokenString);
    return token;
};
