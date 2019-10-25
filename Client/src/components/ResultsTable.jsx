import React from 'react';
import { Container, Row, Badge } from 'reactstrap';

const ResultsTable = ({ weathers }) => (
    < Container className="p-5" >
        <Row>
            <h3 className="float-left">Results</h3>
        </Row>
        <Row>
            <table className='table table-striped'>
                <thead>
                    <tr>
                        <th>Address</th>
                        <th>Current Temp</th>
                        <th>Min Temp</th>
                        <th>Max Temp</th>
                        <th>Cached</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        weathers.map((weather, i) => (
                            <tr key={i}>
                                <td>{weather.address}</td>
                                <td>{weather.currentTemp}</td>
                                <td>{weather.minTemp}</td>
                                <td>{weather.maxTemp}</td>
                                <td><Badge color="danger" pill>Hit</Badge></td>
                            </tr>
                        ))
                    }
                </tbody>
            </table>
        </Row>
    </Container >
);


export default ResultsTable;
