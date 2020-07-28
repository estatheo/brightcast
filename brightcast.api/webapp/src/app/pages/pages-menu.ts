import { NbMenuItem } from '@nebular/theme';

export const MENU_ITEMS: NbMenuItem[] = [
  {
    title: 'Dashboard',
    icon: 'browser-outline',
    link: '/pages/dashboard',
    home: true,
  },
  {
    title: 'Campaigns',
    icon: 'message-square-outline',
    link: '/pages/main/campaign',
    home: true,
  },
  {
    title: 'Contact Lists',
    icon: 'person-add-outline',
    link: '/pages/main/customer-list',
    home: true,
  },
  {
    title: 'Chat',
    icon: 'message-circle-outline',
    link: '/pages/main/chat',
    home: true,
  },
  {
    title: 'Settings',
    icon: 'settings-2-outline',
    link: '/pages/main/settings',
    home: true,
  },
  {
    title: 'Logout',
    icon: 'log-out-outline',
    link: '/pages/logout',
    home: true,
  },
];
