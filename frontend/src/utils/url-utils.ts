import devSettings from 'app-settings-dev.json';
import prodSettings from 'app-settings-prod.json';

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

export const getTokenUrl = (): string => {
    return getCoreBaseUrl() + '/connect/token';
};

export const getRegisterUrl = (): string => {
    return getCoreBaseUrl() + '/account/register';
};

export const getLogoutUrl = (): string => {
    return getCoreBaseUrl() + '/account/logout';
};

export const getIsEmailExistUrl = (email: string): string => {
    return getCoreBaseUrl() + '/account/email/is-exist?email=' + email;
};
