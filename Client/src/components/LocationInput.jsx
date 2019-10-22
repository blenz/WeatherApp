import React from 'react';
import { Container, Row, Col, InputGroup, Input, InputGroupAddon, Button } from 'reactstrap';

export default () => (
    <Container className="p-5">
        <Row>
            <h3 className="float-left">Search</h3>
        </Row>
        <Row className="justify-content-center align-items-center">
            <Col lg="6">
                <InputGroup>
                    <Input placeholder="Search Location" />
                    <InputGroupAddon addonType="append">
                        <Button color="success" onClick={() => console.log('Hello')}>Search</Button>
                        <Button outline color="secondary">Clear</Button>
                    </InputGroupAddon>
                </InputGroup>
            </Col>
        </Row>
    </Container >
);
