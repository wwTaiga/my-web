import { AuthToken } from 'types';
import { removeToken, retrieveToken } from 'utils/account-utils';

describe('retrieveToken', () => {
    test('has token in localstorage and success to retrieve it', () => {
        // arrange
        const mockToken: AuthToken = {
            access_token: 'access',
            refresh_token: 'refresh',
            id_token: 'id',
            token_type: 'type',
            expires_in: 1,
            rememberMe: true,
        };
        Storage.prototype.getItem = jest.fn(() => {
            return JSON.stringify(mockToken);
        });
        const getItem = jest
            .spyOn(Storage.prototype, 'getItem')
            .mockReturnValue(JSON.stringify(mockToken));

        // act
        const token = retrieveToken();

        // assert
        expect(getItem).toHaveBeenCalledTimes(1);
        expect(token).toMatchObject(mockToken);
    });

    test('no token in localstorage and get null', () => {
        // arrange
        const getItem = jest.spyOn(Storage.prototype, 'getItem').mockReturnValue(null);

        // act
        const token = retrieveToken();

        // assert
        expect(getItem).toHaveBeenCalledTimes(1);
        expect(token).toBeNull();
    });
});

describe('removeToken', () => {
    test('call remove token with param "authToken"', () => {
        // arrange
        const removeItem = jest.spyOn(Storage.prototype, 'removeItem').mockReturnValue();

        // act
        removeToken();

        // assert
        expect(removeItem).toHaveBeenCalledTimes(1);
        expect(removeItem).toBeCalledWith('authToken');
    });
});
