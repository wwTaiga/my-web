import devSettings from 'app-settings-dev.json';
import prodSettings from 'app-settings-prod.json';

/**
 * Get core api domain url.
 *
 * @param version - Api version, if null then will use default value ''
 *
 * @returns Core api domain url
 **/
const getCoreBaseUrl = (version?: string): string => {
    let settings;
    if (!process.env.NODE_ENV || process.env.NODE_ENV === 'development') {
        settings = devSettings;
    } else {
        settings = prodSettings;
    }
    const v = version == null ? settings.api.core.defaultVersion : version;
    return (
        settings.api.core.domain +
        settings.api.core.contextPath +
        v +
        settings.api.core.defaultVersion
    );
};

/**
 * Urls utils, use to get api url.
 **/
export const urls = {
    /**
     * Api related to account.
     **/
    account: {
        /**
         * Token api - use to retrive jwt and refresh token.
         **/
        token: (): string => {
            return getCoreBaseUrl() + '/connect/token';
        },
        /**
         * Register api - register new user.
         **/
        registerNewUser: (): string => {
            return getCoreBaseUrl() + '/account/register';
        },
        /**
         * Logout api - perform logout and revoke all tokens of this login session
         **/
        logout: (): string => {
            return getCoreBaseUrl() + '/account/logout';
        },
        /**
         * Checking api - use to check if the email is existed.
         *
         * @param email - The email that need to check
         **/
        isEmailExist: (email: string): string => {
            return getCoreBaseUrl() + '/account/email/is-exist?email=' + email;
        },
    },
};
