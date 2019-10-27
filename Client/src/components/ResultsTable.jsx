import React from 'react';
import BootstrapTable from 'react-bootstrap-table-next';
import paginationFactory from 'react-bootstrap-table2-paginator';
import { Container, Row, Badge } from 'reactstrap';

const columns = [
    {
        text: 'Address',
        dataField: 'address',
        align: 'left',
        sort: true,
    },
    {
        text: 'Current Temp',
        dataField: 'currentTemp',
        sort: true
    },
    {
        text: 'Min Temp',
        dataField: 'minTemp',
        sort: true
    },
    {
        text: 'Max Temp',
        dataField: 'maxTemp',
        sort: true
    },
    {
        text: 'Cached',
        dataField: 'cached',
        sort: true,
        formatter: (cell) => cell
            ? <Badge color='success' pill>Hit</Badge>
            : <Badge color='danger' pill>Miss</Badge>
    },
];

const ResultsTable = ({ weathers }) => (
    < Container className="p-5" >
        <Row>
            <h3 className="float-left">Results</h3>
        </Row>
        <Row>
            <Container>
                <BootstrapTable
                    keyField='id'
                    data={weathers}
                    columns={columns}
                    striped
                    pagination={paginationFactory()}
                />
            </Container>
        </Row>
    </Container >
);


export default ResultsTable;
