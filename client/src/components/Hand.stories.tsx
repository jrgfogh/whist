import { useState } from 'react';

import type { Meta, StoryObj } from '@storybook/react-vite';
import { expect } from 'storybook/test';

import Hand from './Hand';

const cards = ['C1', 'D1', 'S1'];
const oneCard = ['C1'];
const sixCards = ['C1', 'D3', 'H5', 'S7', 'C9', 'DK'];
const thirteenCards = ['C1', 'C2', 'C3', 'D4', 'D5', 'D6', 'H7', 'H8', 'H9', 'S10', 'SJ', 'SQ', 'SK'];

const meta = {
  component: Hand,
  tags: ['ai-generated'],
  args: {
    cards,
  },
} satisfies Meta<typeof Hand>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Display: Story = {};

export const OneCard: Story = {
  args: {
    cards: oneCard,
  },
};

export const SixCards: Story = {
  args: {
    cards: sixCards,
  },
};

export const ThirteenCards: Story = {
  args: {
    cards: thirteenCards,
  },
};

export const Playing: Story = {
  args: {
    playing: true,
  },
  render: (args) => {
    const [playedCard, setPlayedCard] = useState<string | null>(null);

    return (
      <div>
        <Hand {...args} playCard={setPlayedCard} />
        <p>Last played: {playedCard ?? 'none'}</p>
      </div>
    );
  },
  play: async ({ canvas, userEvent, canvasElement }) => {
    const firstCard = canvasElement.querySelector('.card.C1');

    await expect(firstCard).not.toBeNull();
    await userEvent.click(firstCard as HTMLElement);
    await expect(firstCard).toHaveClass('selected');
    await userEvent.dblClick(firstCard as HTMLElement);
    await expect(canvas.getByText('Last played: C1')).toBeVisible();
  },
};
