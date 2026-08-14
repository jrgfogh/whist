import type { HubConnection } from '@microsoft/signalr';
import type { Meta, StoryObj } from '@storybook/react-vite';
import { expect } from 'storybook/test';

import { BidPicker } from './BidPicker';

const connection = {
  invoke: async () => undefined,
} as unknown as HubConnection;

const meta = {
  component: BidPicker,
  tags: ['ai-generated'],
  args: {
    state: 'bidding-choosing-bid',
    bids: [
      { bidder: 'Alice', bid: '7 Good' },
      { bidder: 'Bob', bid: 'pass' },
    ],
    dispatch: () => {},
    connection,
  },
} satisfies Meta<typeof BidPicker>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Default: Story = {
  play: async ({ canvas }) => {
    const bidButtons = canvas.getAllByRole('button', { name: 'Bid!' });

    await expect(bidButtons[0]).toBeEnabled();
    await expect(canvas.getByText('Alice bid 7 Good')).toBeVisible();
  },
};

export const Waiting: Story = {
  args: {
    state: 'bidding',
  },
  play: async ({ canvas }) => {
    const bidButtons = canvas.getAllByRole('button', { name: 'Bid!' });

    await expect(bidButtons[0]).toBeDisabled();
  },
};
