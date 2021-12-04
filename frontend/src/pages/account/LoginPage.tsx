import {
    Flex,
    Box,
    FormControl,
    FormLabel,
    Input,
    Checkbox,
    Stack,
    Link,
    Button,
    Heading,
    Text,
    useColorModeValue,
    FormErrorMessage,
} from '@chakra-ui/react';
import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { setIsLoggedIn } from 'store/account/accountSlice';
import { useAppDispatch, useAppSelector } from 'store/hooks';
import { LoginModel } from 'types';
import { doLogin } from 'utils/account-utils';

interface Input {
    username: string;
    password: string;
    rememberMe: boolean;
}

const LoginPage = (): JSX.Element => {
    const dispatch = useAppDispatch();
    const navigate = useNavigate();
    const isLoggedIn = useAppSelector((state) => state.account.isLoggedIn);

    useEffect(() => {
        if (isLoggedIn) {
            navigate('/home', { replace: true });
        }
    }, []);

    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm();

    const login = async (input: LoginModel): Promise<void> => {
        const result = await doLogin(input);
        if (result.isSuccess) {
            dispatch(setIsLoggedIn(true));
            navigate('/home');
        } else {
            dispatch(setIsLoggedIn(false));
            alert(result.errorDesc);
        }
    };

    return (
        <Flex
            minH={'calc(100vh - 60px)'}
            align={'center'}
            justify={'center'}
            bg={useColorModeValue('gray.50', 'gray.800')}
        >
            <Stack spacing={8} mx={'auto'} maxW={'lg'} py={12} px={6}>
                <Stack align={'center'}>
                    <Heading fontSize={'4xl'}>Sign in to your account</Heading>
                    <Text fontSize={'lg'} color={'gray.600'}>
                        to enjoy all of our cool <Link color={'blue.400'}>features</Link> ✌️
                    </Text>
                </Stack>
                <Box
                    rounded={'lg'}
                    bg={useColorModeValue('white', 'gray.700')}
                    boxShadow={'lg'}
                    p={8}
                >
                    <Stack spacing={4}>
                        <form onSubmit={handleSubmit(login)}>
                            <FormControl isInvalid={errors.username}>
                                <FormLabel>User Name</FormLabel>
                                <Input
                                    type="text"
                                    {...register('username', {
                                        required: 'This is required',
                                    })}
                                />
                                <FormErrorMessage>
                                    {errors.username && errors.username.message}
                                </FormErrorMessage>
                            </FormControl>
                            <FormControl isInvalid={errors.password}>
                                <FormLabel>Password</FormLabel>
                                <Input
                                    type="password"
                                    {...register('password', {
                                        required: 'This is required',
                                    })}
                                />
                                <FormErrorMessage>
                                    {errors.password && errors.password.message}
                                </FormErrorMessage>
                            </FormControl>
                            <Stack spacing={10}>
                                <Stack
                                    direction={{ base: 'column', sm: 'row' }}
                                    align={'start'}
                                    justify={'space-between'}
                                >
                                    <Checkbox {...register('rememberMe')}>Remember me</Checkbox>
                                    <Link color={'blue.400'}>Forgot password?</Link>
                                </Stack>
                                <Stack
                                    direction={{ base: 'column', sm: 'row' }}
                                    align={'start'}
                                    justify={'space-between'}
                                >
                                    <Link color={'blue.400'} onClick={() => navigate('/register')}>
                                        Create a new account
                                    </Link>
                                    <Button
                                        type="submit"
                                        bg={'blue.400'}
                                        color={'white'}
                                        _hover={{
                                            bg: 'blue.500',
                                        }}
                                    >
                                        Sign in
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

export default LoginPage;
