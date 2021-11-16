import devSettings from 'app-settings-dev.json';
import prodSettings from 'app-settings-prod.json';

const getCoreBaseUrl = (version?: string): string => {
    if (!process.env.NODE_ENV || process.env.NODE_ENV === 'development') {
        const v = version == null ? devSettings.api.core.defaultVersion : version;
        return (
            devSettings.api.core.domain +
            devSettings.api.core.contextPath +
            v +
            devSettings.api.core.defaultVersion
        );
    } else {
        const v = version == null ? prodSettings.api.core.defaultVersion : version;
        return (
            prodSettings.api.core.domain +
            prodSettings.api.core.contextPath +
            v +
            prodSettings.api.core.defaultVersion
        );
    }
};

export const getTokenUrl = (): string => {
    return getCoreBaseUrl() + '/connect/token';
};
