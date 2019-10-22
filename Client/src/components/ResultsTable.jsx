import React from 'react';
import { Container, Row, Badge } from 'reactstrap';

export default () => (
    <Container className="p-5">
        <Row>
            <h3 className="float-left">Results</h3>
        </Row>
        <Row>
            <table className='table table-striped'>
                <thead>
                    <tr>
                        <th>Location</th>
                        <th>Temp.</th>
                        <th>Cached</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>Foo</td>
                        <td>100</td>
                        <td><Badge color="danger" pill>Hit</Badge></td>
                    </tr>
                    <tr>
                        <td>Bar</td>
                        <td>60</td>
                        <td><Badge color="secondary" pill>Miss</Badge></td>
                    </tr>
                </tbody>
            </table>
        </Row>
    </Container>
);
