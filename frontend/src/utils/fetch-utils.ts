const get = (url: string): Promise<Response> => {
    const requestOptions = {
        method: 'GET',
    };
    return fetch(url, requestOptions);
};

const post = async (url: string, body: unknown): Promise<Response> => {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body),
    };
    return fetch(url, requestOptions);
};

const put = (url: string, body: unknown): Promise<Response> => {
    const requestOptions = {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body),
    };
    return fetch(url, requestOptions);
};

// prefixed with underscored because delete is a reserved word in javascript
const _delete = (url: string): Promise<Response> => {
    const requestOptions = {
        method: 'DELETE',
    };
    return fetch(url, requestOptions);
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
