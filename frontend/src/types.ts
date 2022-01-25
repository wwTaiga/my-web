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

export interface UserProfile {
    sub: string;
    name: string;
    role: string;
}

export interface Result {
    isSuccess: boolean;
    status?: number;
    data?: Record<string, unknown>;
    errorDesc?: string;
}

export interface AuthTokenError {
    error: string;
    error_description: string;
    error_uri: string;
}

export interface FetchParams {
    method: 'GET' | 'POST' | 'PUT' | 'DELETE';
    url: string;
    body?: unknown;
}

export interface ErrorResponse {
    errors: Record<string, string>;
}

export interface RegisterForm {
    username: string;
    email: string;
    password: string;
}

export interface ForgotPasswordForm {
    email: string;
}

export interface ResetPasswordForm {
    password: string;
}

export interface ResetPasswordParams {
    token: string;
    userId: string;
    newPassword: string;
}
