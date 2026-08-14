import type { Meta, StoryObj } from '@storybook/react-vite';
import { expect } from 'storybook/test';

import Card from './Card';

const ranks = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', 'J', 'Q', 'K'];
const suits = ['C', 'D', 'H', 'S'];

const allCards = suits.flatMap((suit) => ranks.map((rank) => `${suit}${rank}`));

const meta = {
  component: Card,
  tags: ['ai-generated'],
  args: {
    name: 'S1',
    clickCard: () => {},
    doubleClickCard: () => {},
  },
  render: (args) => (
    <div style={{ padding: '2rem' }}>
      <Card {...args} />
    </div>
  ),
} satisfies Meta<typeof Card>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Default: Story = {};

export const Selected: Story = {
  args: {
    selected: true,
  },
  play: async ({ canvasElement }) => {
    const card = canvasElement.querySelector('.card');

    await expect(card).not.toBeNull();
    await expect(card).toHaveClass('selected');
    await expect(card).toHaveClass('S1');
  },
};

export const AllCardsGrid: Story = {
  render: (args) => {
    const cards = [...allCards, 'Joker'];

    return (
      <div style={{ padding: '2rem', display: 'flex', gap: '1rem', flexWrap: 'wrap' }}>
        {cards.map((name) => (
          <div key={name} style={{ textAlign: 'center' }}>
            <Card {...args} name={name} />
            <div>{name}</div>
          </div>
        ))}
      </div>
    );
  },
};
