import React from 'react';
import { Container } from 'reactstrap';

const headerStyle = {
    background: 'linear-gradient(90deg, rgba(46,9,121,1) 0%, rgba(9,24,121,1) 37%, rgba(0,134,255,1) 100%)'
}

export default () => (
    <Container className="p-5" style={headerStyle}>
        <h1 className="display-3 text-white" style={{ textShadow: '2px 2px 8px #000000' }} >
            Weather Checker
        </h1>
    </Container>
);
