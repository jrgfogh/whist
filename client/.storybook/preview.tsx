import type { Preview } from '@storybook/react-vite';
import { MemoryRouter } from 'react-router-dom';

import 'bootstrap/dist/css/bootstrap.css';
import '../src/custom.css';
import '../src/components/Game.css';

const preview: Preview = {
  decorators: [
    (Story) => (
      <MemoryRouter
        future={{
          v7_startTransition: true,
          v7_relativeSplatPath: true,
        }}
      >
        <Story />
      </MemoryRouter>
    ),
  ],
  parameters: {
    layout: 'fullscreen',
    controls: {
      matchers: {
        color: /(background|color)$/i,
        date: /Date$/i,
      },
    },
    a11y: {
      test: 'todo',
    },
  },
};

export default preview;
