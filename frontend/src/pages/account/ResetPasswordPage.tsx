import {
    Box,
    Button,
    Flex,
    FormControl,
    FormErrorMessage,
    FormLabel,
    Heading,
    Input,
    Stack,
    useColorModeValue,
    useToast,
} from '@chakra-ui/react';
import { zodResolver } from '@hookform/resolvers/zod';
import { colors } from 'constans/colors';
import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { useAppSelector } from 'store/hooks';
import { ResetPasswordForm, ResetPasswordParams, Result } from 'types';
import { jsonFetch } from 'utils/fetch-utils';
import {
    minOneLowerAlphaRegex,
    minOneNumberRegex,
    minOneSpecialCharRegex,
    minOneUpperAlphaRegex,
} from 'utils/regex-utils';
import { urls } from 'utils/url-utils';
import { z } from 'zod';

const ResetPasswordPage = (): JSX.Element => {
    const navigate = useNavigate();
    const isLoggedIn = useAppSelector((state) => state.account.isLoggedIn);
    const toast = useToast();
    const [queryParams] = useSearchParams();
    const [formState, setFormState] = useState<'initial' | 'submitting' | 'success'>('initial');
    const schema = z
        .object({
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
    } = useForm<ResetPasswordForm>({
        mode: 'onBlur',
        resolver: zodResolver(schema),
    });

    const resetPassword = async (input: ResetPasswordForm): Promise<void> => {
        setFormState('initial');
        const token = queryParams.get('token');
        const userId = queryParams.get('userId');

        if (token == null || userId == null) {
            toast({
                title: 'Invalid reset password link',
                position: 'top',
                isClosable: true,
                status: 'error',
            });
            return;
        }

        const params: ResetPasswordParams = {
            token: token,
            userId: userId,
            newPassword: input.password,
        };

        setFormState('submitting');
        const result: Result = await jsonFetch.post(urls.account.resetPassword(), params);
        if (result.isSuccess) {
            setFormState('success');
            toast({
                title: 'Reset password Successfully',
                description: 'Will redirect to login page in 5 seconds',
                position: 'top',
                isClosable: true,
                status: 'success',
            });
            setTimeout(() => navigate('/login'), 5000);
        } else {
            setFormState('initial');
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
                    <Heading fontSize={'4xl'}>Reset your password</Heading>
                </Stack>
                <Box
                    rounded={'lg'}
                    bg={useColorModeValue(colors.light.main, colors.dark.main)}
                    boxShadow={'lg'}
                    p={8}
                >
                    <Stack spacing={4} as={'form'} onSubmit={handleSubmit(resetPassword)}>
                        <FormControl isInvalid={!!errors.password}>
                            <FormLabel>New Password</FormLabel>
                            <Input
                                type="password"
                                disabled={formState != 'initial'}
                                {...register('password')}
                            />
                            <FormErrorMessage>
                                {errors.password && errors.password.message}
                            </FormErrorMessage>
                        </FormControl>
                        <FormControl isInvalid={!!errors.confirmPassword}>
                            <FormLabel>Repeat Password</FormLabel>
                            <Input
                                type="password"
                                disabled={formState != 'initial'}
                                {...register('confirmPassword')}
                            />
                            <FormErrorMessage>
                                {errors.confirmPassword && errors.confirmPassword.message}
                            </FormErrorMessage>
                        </FormControl>
                        <Stack spacing={10}>
                            <Stack
                                direction={{ base: 'column', sm: 'row' }}
                                align={'start'}
                                justify={'right'}
                            >
                                <Button
                                    type="submit"
                                    disabled={formState != 'initial'}
                                    isLoading={formState == 'submitting'}
                                    bg={colors.primaryBtn.bg}
                                    color={colors.primaryBtn.text}
                                    _hover={{
                                        bg: colors.primaryBtn.hover,
                                    }}
                                >
                                    Submit
                                </Button>
                            </Stack>
                        </Stack>
                    </Stack>
                </Box>
            </Stack>
        </Flex>
    );
};

export default ResetPasswordPage;
