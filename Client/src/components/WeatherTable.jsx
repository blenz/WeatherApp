import React, { Fragment } from 'react';
import BootstrapTable from 'react-bootstrap-table-next';
import paginationFactory from 'react-bootstrap-table2-paginator';
import ToolkitProvider, { Search } from 'react-bootstrap-table2-toolkit';
import { Container, Row, Badge } from 'reactstrap';

const { SearchBar } = Search;
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

const WeatherTable = ({ weathers }) => (
    < Container className="p-5" >
        <Row>
            <h3>Results</h3>
        </Row>
        <Row>
            <Container>
                <ToolkitProvider
                    keyField="id"
                    data={weathers}
                    columns={columns}
                    search
                >
                    {
                        props => (
                            <Fragment>
                                <div className='float-right'>
                                    <SearchBar {...props.searchProps} />
                                </div>
                                <BootstrapTable
                                    {...props.baseProps}
                                    striped
                                    pagination={paginationFactory()}
                                />
                            </Fragment>
                        )
                    }
                </ToolkitProvider>
            </Container>
        </Row>
    </Container >
);


export default WeatherTable;
