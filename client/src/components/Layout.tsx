import React, { Component, type ReactNode } from "react";
import { Container } from "reactstrap";
import { NavMenu } from "./NavMenu";

interface LayoutProps {
    children?: ReactNode;
}

export class Layout extends Component<LayoutProps> {
  render () {
    return (
      <div>
        <NavMenu />
        <Container>
          {this.props.children}
        </Container>
      </div>
    );
  }
}
