export interface RefreshModel {
    refresh_token: string;
}

export interface PasswordModel {
    username: string;
    password: string;
}

export interface AuthToken {
    access_token: string;
    refresh_token?: string;
    id_token: string;
    token_type: string;
    expires_in: number;
}
