import { ErrorResponse, FetchParams, Result } from 'types';
import { retrieveToken } from 'utils/account-utils';

/**
 * A fetch wrapper which use to perform json request
 **/
export const jsonFetch = {
    /**
     * Perform get request.
     *
     * @param url - Url and params query (optional)
     *
     * @returns Result Promise
     **/
    get: async (url: string): Promise<Result> => {
        return await jsonFetchWrapper({ method: 'GET', url: url });
    },

    /**
     * Perform post request.
     *
     * @param url - Url
     * @param body - Data need to send
     *
     * @returns Result Promise
     **/
    post: async (url: string, body: unknown): Promise<Result> => {
        return await jsonFetchWrapper({ method: 'POST', url: url, body: body });
    },

    /**
     * Perform put request.
     *
     * @param url - Url
     * @param body - Data need to send
     *
     * @returns Result Promise
     **/
    put: async (url: string, body: unknown): Promise<Result> => {
        return await jsonFetchWrapper({ method: 'PUT', url: url, body: body });
    },

    /**
     * Perform delete request.
     *
     * @param url - Url
     *
     * @returns Result Promise
     **/
    delete: async (url: string): Promise<Result> => {
        return await jsonFetchWrapper({ method: 'DELETE', url: url });
    },
};

/**
 * A fetch wrapper with content-type: application/json.
 *
 * @param fetchParams - method, url, and data (optional)
 *
 * @returns Result promise
 **/
const jsonFetchWrapper = async (fetchParams: FetchParams): Promise<Result> => {
    const requestOptions = {
        method: fetchParams.method,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(fetchParams.body),
    };
    const accessToken = retrieveToken()?.access_token;
    if (accessToken != null) {
        Object.assign(requestOptions.headers, { Authorization: 'Bearer ' + accessToken });
    }

    const response = await fetch(fetchParams.url, requestOptions).catch((error: TypeError) =>
        handleFetchError(error),
    );

    if (response.status == 400 || response.status == 422) {
        const data: ErrorResponse = await response.json();
        const errorDescs = Object.values(data.errors).join('\n');
        return { isSuccess: false, status: response.status, errorDesc: errorDescs };
    } else if (response.status == 401 || response.status == 403) {
        return { isSuccess: false, status: response.status, errorDesc: 'No permission' };
    } else if (!response.ok) {
        return { isSuccess: false, status: response.status, errorDesc: response.statusText };
    }

    try {
        return { isSuccess: true, data: await response.json() };
    } catch {
        return { isSuccess: true };
    }
};

/**
 * Return a new response with status code 503.
 *
 * @param error - The error catched from fetch
 *
 * @returns Response object
 **/
export const handleFetchError = (error: TypeError): Response => {
    console.warn(error);
    const init: ResponseInit = { status: 503, statusText: 'Service Unavailable' };
    return new Response(null, init);
};
