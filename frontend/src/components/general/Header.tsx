import {
    ChevronDownIcon,
    ChevronRightIcon,
    CloseIcon,
    HamburgerIcon,
    MoonIcon,
    SunIcon,
} from '@chakra-ui/icons';
import {
    Avatar,
    Box,
    Button,
    Collapse,
    Flex,
    Icon,
    IconButton,
    Link,
    Menu,
    MenuButton,
    MenuDivider,
    MenuItem,
    MenuList,
    Popover,
    PopoverContent,
    PopoverTrigger,
    Stack,
    Text,
    useBreakpointValue,
    useColorMode,
    useColorModeValue,
    useDisclosure,
} from '@chakra-ui/react';
import { Link as RouterLink, useNavigate } from 'react-router-dom';
import { setIsLoggedIn } from 'store/account/accountSlice';
import { useAppDispatch, useAppSelector } from 'store/hooks';
import { removeToken } from 'utils/account-utils';
import { jsonFetch } from 'utils/fetch-utils';
import { getLogoutUrl } from 'utils/url-utils';

const Header = (): JSX.Element => {
    const { isOpen, onToggle } = useDisclosure();
    const { colorMode, toggleColorMode } = useColorMode();
    const isLoggedIn = useAppSelector((state) => state.account.isLoggedIn);
    const dispatch = useAppDispatch();
    const navigate = useNavigate();

    const logout = (): void => {
        jsonFetch.post(getLogoutUrl(), null);
        removeToken();
        dispatch(setIsLoggedIn(false));
        navigate('/');
    };

    return (
        <Box>
            <Flex
                bg={useColorModeValue('white', 'gray.800')}
                color={useColorModeValue('gray.600', 'white')}
                minH={'60px'}
                py={{ base: 2 }}
                px={{ base: 4 }}
                borderBottom={1}
                borderStyle={'solid'}
                borderColor={useColorModeValue('gray.200', 'gray.900')}
                align={'center'}
            >
                <Flex
                    flex={{ base: 1, md: 'auto' }}
                    ml={{ base: -2 }}
                    display={{ base: 'flex', md: 'none' }}
                >
                    <IconButton
                        onClick={onToggle}
                        icon={isOpen ? <CloseIcon w={3} h={3} /> : <HamburgerIcon w={5} h={5} />}
                        variant={'ghost'}
                        aria-label={'Toggle Navigation'}
                    />
                </Flex>
                <Flex flex={{ base: 1 }} justify={{ base: 'center', md: 'start' }}>
                    <Text
                        textAlign={useBreakpointValue({
                            base: 'center',
                            md: 'left',
                        })}
                        fontFamily={'heading'}
                        color={useColorModeValue('gray.800', 'white')}
                        onClick={() => navigate('/')}
                    >
                        Logo
                    </Text>

                    <Flex display={{ base: 'none', md: 'flex' }} ml={10}>
                        <DesktopNav />
                    </Flex>
                </Flex>

                {!isLoggedIn ? (
                    <Stack
                        flex={{ base: 1, md: 0 }}
                        justify={'flex-end'}
                        direction={'row'}
                        spacing={6}
                    >
                        <Button onClick={toggleColorMode}>
                            {colorMode === 'light' ? <MoonIcon /> : <SunIcon />}
                        </Button>
                        <Button
                            as={RouterLink}
                            fontSize={'sm'}
                            fontWeight={400}
                            variant={'link'}
                            to={'/login'}
                        >
                            Sign In
                        </Button>
                        <Button
                            as={RouterLink}
                            display={{ base: 'none', md: 'inline-flex' }}
                            fontSize={'sm'}
                            fontWeight={600}
                            color={'white'}
                            bg={'pink.400'}
                            _hover={{
                                bg: 'pink.300',
                            }}
                            to="/register"
                        >
                            Sign Up
                        </Button>
                    </Stack>
                ) : (
                    <UserAvatar logout={logout} navigate={navigate} />
                )}
            </Flex>

            <Collapse in={isOpen} animateOpacity>
                <MobileNav />
            </Collapse>
        </Box>
    );
};

const DesktopNav = () => {
    const linkColor = useColorModeValue('gray.600', 'gray.200');
    const linkHoverColor = useColorModeValue('gray.800', 'white');
    const popoverContentBgColor = useColorModeValue('white', 'gray.800');

    return (
        <Stack direction={'row'} spacing={4}>
            {NAV_ITEMS.map((navItem) => (
                <Box key={navItem.label}>
                    <Popover trigger={'hover'} placement={'bottom-start'}>
                        <PopoverTrigger>
                            <Link
                                p={2}
                                href={navItem.href ?? '#'}
                                fontSize={'sm'}
                                fontWeight={500}
                                color={linkColor}
                                _hover={{
                                    textDecoration: 'none',
                                    color: linkHoverColor,
                                }}
                            >
                                {navItem.label}
                            </Link>
                        </PopoverTrigger>

                        {navItem.children && (
                            <PopoverContent
                                border={0}
                                boxShadow={'xl'}
                                bg={popoverContentBgColor}
                                p={4}
                                rounded={'xl'}
                                minW={'sm'}
                            >
                                <Stack>
                                    {navItem.children.map((child) => (
                                        <DesktopSubNav key={child.label} {...child} />
                                    ))}
                                </Stack>
                            </PopoverContent>
                        )}
                    </Popover>
                </Box>
            ))}
        </Stack>
    );
};

const DesktopSubNav = ({ label, href, subLabel }: NavItem) => {
    return (
        <Link
            href={href}
            role={'group'}
            display={'block'}
            p={2}
            rounded={'md'}
            _hover={{ bg: useColorModeValue('pink.50', 'gray.900') }}
        >
            <Stack direction={'row'} align={'center'}>
                <Box>
                    <Text
                        transition={'all .3s ease'}
                        _groupHover={{ color: 'pink.400' }}
                        fontWeight={500}
                    >
                        {label}
                    </Text>
                    <Text fontSize={'sm'}>{subLabel}</Text>
                </Box>
                <Flex
                    transition={'all .3s ease'}
                    transform={'translateX(-10px)'}
                    opacity={0}
                    _groupHover={{
                        opacity: '100%',
                        transform: 'translateX(0)',
                    }}
                    justify={'flex-end'}
                    align={'center'}
                    flex={1}
                >
                    <Icon color={'pink.400'} w={5} h={5} as={ChevronRightIcon} />
                </Flex>
            </Stack>
        </Link>
    );
};

const MobileNav = () => {
    return (
        <Stack bg={useColorModeValue('white', 'gray.800')} p={4} display={{ md: 'none' }}>
            {NAV_ITEMS.map((navItem) => (
                <MobileNavItem key={navItem.label} {...navItem} />
            ))}
        </Stack>
    );
};

const MobileNavItem = ({ label, children, href }: NavItem) => {
    const { isOpen, onToggle } = useDisclosure();

    return (
        <Stack spacing={4} onClick={children && onToggle}>
            <Flex
                py={2}
                as={Link}
                href={href ?? '#'}
                justify={'space-between'}
                align={'center'}
                _hover={{
                    textDecoration: 'none',
                }}
            >
                <Text fontWeight={600} color={useColorModeValue('gray.600', 'gray.200')}>
                    {label}
                </Text>
                {children && (
                    <Icon
                        as={ChevronDownIcon}
                        transition={'all .25s ease-in-out'}
                        transform={isOpen ? 'rotate(180deg)' : ''}
                        w={6}
                        h={6}
                    />
                )}
            </Flex>

            <Collapse in={isOpen} animateOpacity style={{ marginTop: '0!important' }}>
                <Stack
                    mt={2}
                    pl={4}
                    borderLeft={1}
                    borderStyle={'solid'}
                    borderColor={useColorModeValue('gray.200', 'gray.700')}
                    align={'start'}
                >
                    {children &&
                        children.map((child) => (
                            <Link key={child.label} py={2} href={child.href}>
                                {child.label}
                            </Link>
                        ))}
                </Stack>
            </Collapse>
        </Stack>
    );
};

const UserAvatar = (props: { logout: () => void; navigate: (url: string) => void }) => {
    return (
        <Stack flex={{ base: 1, md: 0 }} justify={'flex-end'} direction={'row'} spacing={6}>
            <Menu>
                <MenuButton
                    as={Button}
                    rounded={'full'}
                    variant={'link'}
                    cursor={'pointer'}
                    minW={0}
                >
                    <Avatar
                        size={'sm'}
                        src={
                            'https://images.unsplash.com/photo-1493666438817-866a91353ca9?ixlib=rb-0.3.5&q=80&fm=jpg&crop=faces&fit=crop&h=200&w=200&s=b616b2c5b373a80ffc9636ba24f7a4a9'
                        }
                    />
                </MenuButton>
                <MenuList>
                    <MenuItem onClick={() => props.navigate('/home')}>Link 1</MenuItem>
                    <MenuItem>Link 2</MenuItem>
                    <MenuDivider />
                    <MenuItem onClick={props.logout}>Logout</MenuItem>
                </MenuList>
            </Menu>
        </Stack>
    );
};

interface NavItem {
    label: string;
    subLabel?: string;
    children?: Array<NavItem>;
    href?: string;
}

const NAV_ITEMS: Array<NavItem> = [
    {
        label: 'Inspiration',
        children: [
            {
                label: 'Explore Design Work',
                subLabel: 'Trending Design to inspire you',
                href: '#',
            },
            {
                label: 'New & Noteworthy',
                subLabel: 'Up-and-coming Designers',
                href: '#',
            },
        ],
    },
    {
        label: 'Find Work',
        children: [
            {
                label: 'Job Board',
                subLabel: 'Find your dream design job',
                href: '#',
            },
            {
                label: 'Freelance Projects',
                subLabel: 'An exclusive list for contract work',
                href: '#',
            },
        ],
    },
    {
        label: 'Learn Design',
        href: '#',
    },
    {
        label: 'Hire Designers',
        href: '#',
    },
];

export default Header;
