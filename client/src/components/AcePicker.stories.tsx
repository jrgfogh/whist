import { useState } from 'react';

import type { Meta, StoryObj } from '@storybook/react-vite';
import { expect } from 'storybook/test';

import AcePicker from './AcePicker';

const meta = {
  component: AcePicker,
  tags: ['ai-generated'],
  args: {
    title: 'Please choose trump:',
    onChoice: () => {},
  },
} satisfies Meta<typeof AcePicker>;

export default meta;
type Story = StoryObj<typeof meta>;

export const Default: Story = {};

export const ChooseAce: Story = {
  render: (args) => {
    const [choice, setChoice] = useState('none');

    return (
      <div>
        <AcePicker {...args} onChoice={setChoice} />
        <p>Choice: {choice}</p>
      </div>
    );
  },
  play: async ({ canvas, userEvent, canvasElement }) => {
    const aceOfHearts = canvasElement.querySelector('.card.H1');

    await expect(aceOfHearts).not.toBeNull();
    await userEvent.click(aceOfHearts as HTMLElement);
    await expect(canvas.getByText('Choice: H1')).toBeVisible();
  },
};
