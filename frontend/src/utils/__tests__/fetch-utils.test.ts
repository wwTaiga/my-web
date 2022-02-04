import { handleFetchError } from 'utils/fetch-utils';

describe('handleFetchError', () => {
    test('get response with status code 503', () => {
        // arrange

        // act
        const response: Response = handleFetchError();

        // assert
        expect(response.status).toEqual(503);
    });
});
