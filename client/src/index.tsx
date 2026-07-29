import "bootstrap/dist/css/bootstrap.css";
import React from "react";
import { createRoot } from 'react-dom/client';
import { BrowserRouter } from "react-router-dom";
import App from "./App";
import { connect } from "./network";
import registerServiceWorker from "./registerServiceWorker";

const rootElement = document.getElementById("root");
if (!rootElement) throw new Error("Root element not found");

const root = createRoot(rootElement);
root.render(
  <BrowserRouter basename={import.meta.env.BASE_URL}>
    <App connect={connect} />
  </BrowserRouter>);

registerServiceWorker();
