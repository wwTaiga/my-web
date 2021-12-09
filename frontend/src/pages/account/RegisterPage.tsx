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
    useToast,
} from '@chakra-ui/react';
import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { useNavigate, Link as RouterLink } from 'react-router-dom';
import { setIsLoggedIn } from 'store/account/accountSlice';
import { useAppDispatch, useAppSelector } from 'store/hooks';
import { Result } from 'types';
import { doLogin } from 'utils/account-utils';
import { jsonFetch } from 'utils/fetch-utils';
import { getRegisterUrl } from 'utils/url-utils';

interface Input {
    username: string;
    password: string;
}

const RegisterPage = (): JSX.Element => {
    const dispatch = useAppDispatch();
    const navigate = useNavigate();
    const isLoggedIn = useAppSelector((state) => state.account.isLoggedIn);
    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm();
    const toast = useToast();

    useEffect(() => {
        if (isLoggedIn) {
            navigate('/home', { replace: true });
        }
    }, []);

    const doRegister = async (input: Input): Promise<void> => {
        input.username = '';
        const result: Result = await jsonFetch.post(getRegisterUrl(), input);
        console.log(result.errorDesc);
        if (result.isSuccess) {
            toast({
                title: 'Success',
                position: 'top',
                isClosable: true,
                status: 'success',
            });
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

    return (
        <Flex
            minH={'calc(100vh - 60px)'}
            align={'center'}
            justify={'center'}
            bg={useColorModeValue('gray.50', 'gray.800')}
        >
            <Stack spacing={8} mx={'auto'} maxW={'lg'} py={12} px={6}>
                <Stack align={'center'}>
                    <Heading fontSize={'4xl'}>Create your new account</Heading>
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
                        <form onSubmit={handleSubmit(doRegister)}>
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
                                    <Checkbox>Remember me</Checkbox>
                                    <Link color={'blue.400'}>Forgot password?</Link>
                                </Stack>
                                <Stack
                                    direction={{ base: 'column', sm: 'row' }}
                                    align={'start'}
                                    justify={'space-between'}
                                >
                                    <Link color={'blue.400'} as={RouterLink} to="/login">
                                        Already have account? <br />
                                        Go to Login
                                    </Link>
                                    <Button
                                        type="submit"
                                        bg={'blue.400'}
                                        color={'white'}
                                        _hover={{
                                            bg: 'blue.500',
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
