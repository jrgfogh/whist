import type { HubConnection } from '@microsoft/signalr';
import type { Meta, StoryObj } from '@storybook/react-vite';
import { expect } from 'storybook/test';

import { Home } from './Home';

const connection = {
  invoke: async () => undefined,
} as unknown as HubConnection;

const meta = {
  component: Home,
  tags: ['ai-generated'],
  args: {
    dispatch: () => {},
    connection,
    cardsInHand: ['S1', 'H1', 'CK'],
    gameState: {
      state: 'playing',
      currentTrick: ['C1', 'D1'],
    },
  },
} satisfies Meta<typeof Home>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Playing: Story = {};

export const Bidding: Story = {
  args: {
    gameState: {
      state: 'bidding-choosing-bid',
      bids: [
        { bidder: 'Alice', bid: '7 Good' },
        { bidder: 'Bob', bid: 'pass' },
      ],
      currentTrick: [],
    },
  },
  play: async ({ canvas }) => {
    const bidButtons = canvas.getAllByRole('button', { name: 'Bid!' });

    await expect(bidButtons[0]).toBeEnabled();
  },
};

export const ChoosingTrump: Story = {
  args: {
    gameState: {
      state: 'bidding-choosing-trump',
      currentTrick: [],
    },
  },
};

export const CssCheck: Story = {
  play: async ({ canvasElement }) => {
    const gameBackground = canvasElement.querySelector('.game-background');

    await expect(gameBackground).not.toBeNull();
    await expect(getComputedStyle(gameBackground as Element).backgroundColor).toBe('rgb(6, 125, 0)');
  },
};
