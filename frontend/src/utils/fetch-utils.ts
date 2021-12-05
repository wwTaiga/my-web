import { ErrorResponse, FetchParams, Result } from 'types';

const get = async (url: string): Promise<Result> => {
    return await jsonFetch({ method: 'POST', url: url });
};

const post = async (url: string, body: unknown): Promise<Result> => {
    return await jsonFetch({ method: 'POST', url: url, body: body });
};

const put = async (url: string, body: unknown): Promise<Result> => {
    return await jsonFetch({ method: 'POST', url: url, body: body });
};

// prefixed with underscored because delete is a reserved word in javascript
const _delete = async (url: string): Promise<Result> => {
    return await jsonFetch({ method: 'POST', url: url });
};

const jsonFetch = async (fetchParams: FetchParams): Promise<Result> => {
    const requestOptions = {
        method: fetchParams.method,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(fetchParams.body),
    };
    const response = await fetch(fetchParams.url, requestOptions).catch((error: TypeError) =>
        handleFetchError(error),
    );

    if (response.status == 400) {
        const data: ErrorResponse = await response.json();
        const errorDescs = Object.values(data.errors).join('\n');
        return { isSuccess: false, status: response.status, errorDesc: errorDescs };
    } else if (response.status == 401 || response.status == 403) {
        return { isSuccess: false, status: response.status, errorDesc: 'No permission' };
    } else if (!response.ok) {
        return { isSuccess: false, status: response.status, errorDesc: response.statusText };
    }

    return { isSuccess: true, data: await response.json() };
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

export const fetchWrapper = {
    get,
    post,
    put,
    delete: _delete,
};
