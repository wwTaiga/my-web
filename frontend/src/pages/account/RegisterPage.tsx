import { WarningTwoIcon } from '@chakra-ui/icons';
import {
    Flex,
    Box,
    FormControl,
    FormLabel,
    Input,
    Stack,
    Link,
    Button,
    Heading,
    Text,
    useColorModeValue,
    FormErrorMessage,
    useToast,
} from '@chakra-ui/react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { useNavigate, Link as RouterLink } from 'react-router-dom';
import { useAppSelector } from 'store/hooks';
import { Result } from 'types';
import { isEmailExist } from 'utils/account-utils';
import { jsonFetch } from 'utils/fetch-utils';
import { getRegisterUrl } from 'utils/url-utils';
import { z } from 'zod';

interface Input {
    username: string;
    password: string;
}

const RegisterPage = (): JSX.Element => {
    const navigate = useNavigate();
    const isLoggedIn = useAppSelector((state) => state.account.isLoggedIn);
    const toast = useToast();
    const schema = z
        .object({
            username: z.string().min(1, 'This field is requird'),
            email: z
                .string()
                .min(1, 'This field is required')
                .email('Invalid email format')
                .refine(async (value) => await isEmailExist(value), {
                    message: 'Email is existed',
                }),
            password: z
                .string()
                .min(1, 'This field is required')
                .min(6, 'Password length must more than 6 characters')
                .regex(new RegExp('.*[A-Z].*'), 'Password must contains 1 uppercase character')
                .regex(new RegExp('.*[a-z].*'), 'Password must contains 1 lowercase character')
                .regex(new RegExp('.*\\d.*'), 'Password must contains 1 number')
                .regex(
                    new RegExp('.*[`~<>?,./!@#$%^&*()\\-_+="\'|{}\\[\\];:\\\\].*'),
                    'Password must contains 1 special character',
                ),
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
        const result: Result = await jsonFetch.post(getRegisterUrl(), input);
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
                                <Input type="text" {...register('username')} />
                                <FormErrorMessage>
                                    <WarningTwoIcon />
                                    {errors.username && errors.username.message}
                                </FormErrorMessage>
                            </FormControl>
                            <FormControl isInvalid={errors.email}>
                                <FormLabel>Email</FormLabel>
                                <Input type="text" {...register('email')} />
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
