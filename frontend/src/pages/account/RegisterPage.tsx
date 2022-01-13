import { WarningTwoIcon } from '@chakra-ui/icons';
import {
    Box,
    Button,
    Flex,
    FormControl,
    FormErrorMessage,
    FormLabel,
    Heading,
    Input,
    Link,
    Stack,
    Text,
    useColorModeValue,
    useToast,
} from '@chakra-ui/react';
import { zodResolver } from '@hookform/resolvers/zod';
import { colors } from 'constans/colors';
import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { Link as RouterLink, useNavigate } from 'react-router-dom';
import { setIsLoggedIn } from 'store/account/accountSlice';
import { useAppDispatch, useAppSelector } from 'store/hooks';
import { LoginModel, Result } from 'types';
import { doLogin, isEmailExist } from 'utils/account-utils';
import { jsonFetch } from 'utils/fetch-utils';
import {
    minOneLowerAlphaRegex,
    minOneNumberRegex,
    minOneSpecialCharRegex,
    minOneUpperAlphaRegex,
} from 'utils/regex-utils';
import { urls } from 'utils/url-utils';
import { z } from 'zod';

interface Input {
    username: string;
    password: string;
}

const RegisterPage = (): JSX.Element => {
    const navigate = useNavigate();
    const isLoggedIn = useAppSelector((state) => state.account.isLoggedIn);
    const toast = useToast();
    const dispatch = useAppDispatch();
    const schema = z
        .object({
            username: z.string().min(1, 'This field is requird'),
            email: z
                .string()
                .min(1, 'This field is required')
                .email('Invalid email format')
                .refine(async (value) => !(await isEmailExist(value)), {
                    message: 'Email is existed',
                }),
            password: z
                .string()
                .min(1, 'This field is required')
                .min(6, 'Password length must more than 6 characters')
                .regex(minOneUpperAlphaRegex(), 'Password must contains 1 uppercase character')
                .regex(minOneLowerAlphaRegex(), 'Password must contains 1 lowercase character')
                .regex(minOneNumberRegex(), 'Password must contains 1 number')
                .regex(minOneSpecialCharRegex(), 'Password must contains 1 special character'),
            confirmPassword: z.string(),
        })
        .refine(({ password, confirmPassword }) => password === confirmPassword, {
            message: 'Password not same',
            path: ['confirmPassword'],
        });

    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm({
        mode: 'onBlur',
        resolver: zodResolver(schema),
    });

    const doRegister = async (input: Input): Promise<void> => {
        const result: Result = await jsonFetch.post(urls.account.registerNewUser(), input);
        if (result.isSuccess) {
            const loginInput: LoginModel = {
                username: input.username,
                password: input.password,
                rememberMe: false,
            };
            const loginResult = await doLogin(loginInput);
            if (loginResult.isSuccess) {
                dispatch(setIsLoggedIn(true));
                navigate('/home');
            } else {
                toast({
                    title: 'Registration Successfully',
                    description: 'Will redirect to login page in 5 seconds',
                    position: 'top',
                    isClosable: true,
                    status: 'success',
                });
                setTimeout(() => navigate('/login'), 5000);
            }
        } else {
            toast({
                title: 'Error',
                description: result.errorDesc,
                position: 'top',
                isClosable: true,
                status: 'error',
            });
        }
    };

    useEffect(() => {
        if (isLoggedIn) {
            navigate('/home', { replace: true });
        }
    }, []);

    return (
        <Flex
            minH={'calc(100vh - 60px)'}
            align={'center'}
            justify={'center'}
            bg={useColorModeValue(colors.light.bg, colors.dark.bg)}
        >
            <Stack spacing={8} mx={'auto'} maxW={'lg'} py={12} px={6}>
                <Stack align={'center'}>
                    <Heading fontSize={'4xl'}>Create your new account</Heading>
                    <Text fontSize={'lg'} color={'gray.600'}>
                        to enjoy all of our cool <Link color={colors.link}>features</Link> ✌️
                    </Text>
                </Stack>
                <Box
                    rounded={'lg'}
                    bg={useColorModeValue(colors.light.main, colors.dark.main)}
                    boxShadow={'lg'}
                    p={8}
                >
                    <Stack spacing={4}>
                        <form onSubmit={handleSubmit(doRegister)}>
                            <FormControl isInvalid={errors.username}>
                                <FormLabel>User Name</FormLabel>
                                <Input type="text" {...register('username')} />
                                <FormErrorMessage>
                                    <WarningTwoIcon />
                                    {errors.username && errors.username.message}
                                </FormErrorMessage>
                            </FormControl>
                            <FormControl isInvalid={errors.email}>
                                <FormLabel>Email</FormLabel>
                                <Input type="email" {...register('email')} />
                                <FormErrorMessage>
                                    <WarningTwoIcon />
                                    {errors.email && errors.email.message}
                                </FormErrorMessage>
                            </FormControl>
                            <FormControl isInvalid={errors.password}>
                                <FormLabel>Password</FormLabel>
                                <Input type="password" {...register('password')} />
                                <FormErrorMessage>
                                    <WarningTwoIcon />
                                    {errors.password && errors.password.message}
                                </FormErrorMessage>
                            </FormControl>
                            <FormControl isInvalid={errors.confirmPassword}>
                                <FormLabel>Repeat Password</FormLabel>
                                <Input type="password" {...register('confirmPassword')} />
                                <FormErrorMessage>
                                    <WarningTwoIcon />
                                    {errors.confirmPassword && errors.confirmPassword.message}
                                </FormErrorMessage>
                            </FormControl>
                            <Stack spacing={10} pt={3}>
                                <Stack
                                    direction={{ base: 'column', sm: 'row' }}
                                    align={'start'}
                                    justify={'space-between'}
                                >
                                    <Link color={colors.link} as={RouterLink} to="/login">
                                        Already have account? <br />
                                        Go to Login
                                    </Link>
                                    <Button
                                        type="submit"
                                        bg={colors.primaryBtn.bg}
                                        color={colors.primaryBtn.text}
                                        _hover={{
                                            bg: colors.primaryBtn.hover,
                                        }}
                                    >
                                        Create account
                                    </Button>
                                </Stack>
                            </Stack>
                        </form>
                    </Stack>
                </Box>
            </Stack>
        </Flex>
    );
};

export default RegisterPage;
