import {
    Box,
    Button,
    Checkbox,
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
import { LoginModel } from 'types';
import { doLogin } from 'utils/account-utils';
import { z } from 'zod';

const LoginPage = (): JSX.Element => {
    const dispatch = useAppDispatch();
    const navigate = useNavigate();
    const isLoggedIn = useAppSelector((state) => state.account.isLoggedIn);
    const toast = useToast();
    const schema = z.object({
        username: z.string().min(1, 'This field is required'),
        password: z.string().min(1, 'This field is required'),
    });

    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<LoginModel>({
        mode: 'onBlur',
        resolver: zodResolver(schema),
    });

    const login = async (input: LoginModel): Promise<void> => {
        const result = await doLogin(input);
        if (result.isSuccess) {
            dispatch(setIsLoggedIn(true));
            navigate('/home');
        } else {
            dispatch(setIsLoggedIn(false));
            toast({
                title: result.errorDesc,
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
                    <Heading fontSize={'4xl'}>Sign in to your account</Heading>
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
                    <Stack spacing={4} as={'form'} onSubmit={handleSubmit(login)}>
                        <FormControl isInvalid={!!errors.username}>
                            <FormLabel>User Name</FormLabel>
                            <Input type="text" {...register('username')} />
                            <FormErrorMessage>
                                {errors.username && errors.username.message}
                            </FormErrorMessage>
                        </FormControl>
                        <FormControl isInvalid={!!errors.password}>
                            <FormLabel>Password</FormLabel>
                            <Input type="password" {...register('password')} />
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
                                <Link color={colors.link} as={RouterLink} to="/forgot-password">
                                    Forgot password?
                                </Link>
                            </Stack>
                            <Stack
                                direction={{ base: 'column', sm: 'row' }}
                                align={'start'}
                                justify={'space-between'}
                            >
                                <Link color={colors.link} as={RouterLink} to="/register">
                                    Create a new account
                                </Link>
                                <Button
                                    type="submit"
                                    bg={colors.primaryBtn.bg}
                                    color={colors.primaryBtn.text}
                                    _hover={{
                                        bg: colors.primaryBtn.hover,
                                    }}
                                >
                                    Sign in
                                </Button>
                            </Stack>
                        </Stack>
                    </Stack>
                </Box>
            </Stack>
        </Flex>
    );
};

export default LoginPage;
