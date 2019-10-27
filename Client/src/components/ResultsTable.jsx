import React from 'react';
import { Container, Row, Badge } from 'reactstrap';

const headers = [
    'Address',
    'Current Temp',
    'Min Temp',
    'Max Temp',
    'Cached'
];

const ResultsTable = ({ weathers }) => (
    < Container className="p-5" >
        <Row>
            <h3 className="float-left">Results</h3>
        </Row>
        <Row>
            <table className='table table-striped'>
                <thead>
                    <tr>
                        {headers.map((header, i) =>
                            <th key={i}>{header}</th>
                        )}
                    </tr>
                </thead>
                <tbody>
                    {
                        weathers.map(weather => (
                            <tr key={weather.id}>
                                <td className="text-left">{weather.address}</td>
                                <td>{weather.currentTemp}</td>
                                <td>{weather.minTemp}</td>
                                <td>{weather.maxTemp}</td>
                                <td>
                                    {weather.cached
                                        ? <Badge color="success" pill>Hit</Badge>
                                        : <Badge color="danger" pill>Miss</Badge>
                                    }
                                </td>
                            </tr>
                        ))
                    }
                </tbody>
            </table>
        </Row>
    </Container >
);


export default ResultsTable;
