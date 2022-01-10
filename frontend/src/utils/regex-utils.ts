/**
 * Regex - At least 1 upper alphabet.
 *
 * @returns RegExp
 **/
export const minOneUpperAlphaRegex = (): RegExp => {
    return new RegExp('.*[A-Z].*');
};

/**
 * Regex - At least 1 lower alphabet.
 *
 * @returns RegExp
 **/
export const minOneLowerAlphaRegex = (): RegExp => {
    return new RegExp('.*[a-z].*');
};

/**
 * Regex - At least 1 number.
 *
 * @returns RegExp
 **/
export const minOneNumberRegex = (): RegExp => {
    return new RegExp('.*\\d.*');
};

/**
 * Regex - At least 1 special character.
 *
 * @returns RegExp
 **/
export const minOneSpecialCharRegex = (): RegExp => {
    return new RegExp('.*[`~<>?,./!@#$%^&*()\\-_+="\'|{}\\[\\];:\\\\].*');
};
