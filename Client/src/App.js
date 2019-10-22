import React from 'react';
import Header from './components/Header';
import LocationInput from './components/LocationInput';
import { Container } from 'reactstrap';
import ResultsTable from './components/ResultsTable';

export default () => (
  <Container>
    <div className="wrapper">
      <Header />
      <LocationInput />
      <ResultsTable />
    </div>
  </Container>
);
