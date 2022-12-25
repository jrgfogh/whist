import React, { Component } from "react";
import { Container, Navbar, NavbarBrand } from "reactstrap";
import "./NavMenu.css";

export class NavMenu extends Component {
  static displayName = NavMenu.name;

  render () {
    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
          <Container>
            <NavbarBrand>Whist</NavbarBrand>
          </Container>
        </Navbar>
      </header>
    );
  }
}
