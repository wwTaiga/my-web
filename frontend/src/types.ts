export interface RefreshModel {
    refresh_token: string;
    rememberMe: boolean;
}

export interface LoginModel {
    username: string;
    password: string;
    rememberMe: boolean;
}

export interface AuthToken {
    access_token: string;
    refresh_token?: string;
    id_token: string;
    token_type: string;
    expires_in: number;
    rememberMe: boolean;
}

export interface Result {
    isSuccess: boolean;
    errorDesc?: string;
}

export interface AuthTokenError {
    error: string;
    error_description: string;
    error_uri: string;
}
