import { CheckIcon, WarningTwoIcon } from '@chakra-ui/icons';
import {
    Button,
    Container,
    Flex,
    FormControl,
    FormErrorMessage,
    Heading,
    Input,
    Stack,
    Text,
    useColorModeValue,
} from '@chakra-ui/react';
import { zodResolver } from '@hookform/resolvers/zod';
import { colors } from 'constans/colors';
import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { useAppSelector } from 'store/hooks';
import { ForgotPasswordForm, Result } from 'types';
import { jsonFetch } from 'utils/fetch-utils';
import { urls } from 'utils/url-utils';
import { z } from 'zod';

const ForgotPasswordPage = (): JSX.Element => {
    const isLoggedIn = useAppSelector((state) => state.account.isLoggedIn);
    const navigate = useNavigate();
    const [state, setState] = useState<'initial' | 'submitting' | 'success'>('initial');
    const [msg, setMsg] = useState<'text' | 'error' | 'not found' | 'success'>('text');
    const schema = z.object({
        email: z.string().min(1, 'This field is required').email('Invalid email format'),
    });
    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<ForgotPasswordForm>({
        mode: 'onBlur',
        resolver: zodResolver(schema),
    });
    const submit = async (input: ForgotPasswordForm): Promise<void> => {
        setMsg('text');
        setState('submitting');
        const result: Result = await jsonFetch.post(urls.account.forgotPassword(input.email), null);
        if (result.isSuccess) {
            setMsg('success');
            setState('success');
        } else {
            if (result.status == 422) {
                setMsg('not found');
                setState('initial');
            } else {
                setMsg('error');
                setState('initial');
            }
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
            <Container
                maxW={'lg'}
                bg={useColorModeValue(colors.light.main, colors.dark.main)}
                boxShadow={'xl'}
                rounded={'lg'}
                p={6}
                direction={'column'}
            >
                <Heading as={'h2'} fontSize={{ base: 'xl', sm: '2xl' }} textAlign={'center'} mb={5}>
                    Forgot Password
                </Heading>
                <Stack
                    direction={{ base: 'column', md: 'row' }}
                    spacing={'12px'}
                    as={'form'}
                    onSubmit={handleSubmit(submit)}
                >
                    <FormControl isInvalid={!!errors.email}>
                        <Input
                            variant={'solid'}
                            borderWidth={1}
                            color={'gray.800'}
                            _placeholder={{
                                color: 'gray.400',
                            }}
                            borderColor={useColorModeValue('gray.300', 'gray.700')}
                            type="email"
                            placeholder={'Your Email'}
                            aria-label={'Your Email'}
                            disabled={state !== 'initial'}
                            {...register('email')}
                        />
                        <FormErrorMessage>
                            <WarningTwoIcon />
                            {errors.email && errors.email.message}
                        </FormErrorMessage>
                    </FormControl>
                    <FormControl w={{ base: '100%', md: '40%' }}>
                        <Button
                            colorScheme={state === 'success' ? 'green' : 'blue'}
                            isLoading={state === 'submitting'}
                            w="100%"
                            type={state === 'success' ? 'button' : 'submit'}
                        >
                            {state === 'success' ? <CheckIcon /> : 'Submit'}
                        </Button>
                    </FormControl>
                </Stack>
                <Text
                    mt={2}
                    textAlign={'center'}
                    color={msg == 'error' || msg == 'not found' ? 'red.500' : 'gray.500'}
                >
                    {msg == 'text' ? 'Forgot password? Reset your password! ✌️' : ''}
                    {msg == 'error' ? 'Oh no an error occured! 😢 Please try again later.' : ''}
                    {msg == 'not found' ? 'The email is not register as user.' : ''}
                    {msg == 'success'
                        ? 'Reset email has been sent to your email, please check.'
                        : ''}
                </Text>
            </Container>
        </Flex>
    );
};

export default ForgotPasswordPage;
