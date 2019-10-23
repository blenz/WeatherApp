import React, { Component } from 'react';
import PlacesAutocomplete, {
    geocodeByAddress,
    getLatLng,
} from 'react-places-autocomplete';
import { Container, Row, Col, InputGroup, Input, InputGroupAddon, Button } from 'reactstrap';



class LocationInput extends Component {
    state = {
        address: ''
    }

    handleSelect = address => {
        geocodeByAddress(address)
            .then(results => getLatLng(results[0]))
            .then(latLng => { this.setState({ address }); })
            .catch(error => console.error('Error', error));
    };

    renderPlacesAutoComplete() {
        return (
            <PlacesAutocomplete
                value={this.state.address}
                onChange={address => this.setState({ address })}
                onSelect={this.handleSelect}
                searchOptions={{ types: ['address'] }}
            >
                {({ getInputProps, suggestions, getSuggestionItemProps, loading }) => (
                    <InputGroup>

                        <Input value={this.state.address} type="text" {...getInputProps({ placeholder: 'Search Address' })} />

                        <ul className="list-group" style={{ position: 'absolute', width: 'inherit', marginTop: '40px' }}>
                            {loading && <div>Loading...</div>}

                            {suggestions.map(suggestion => {

                                const style = suggestion.active
                                    ? { backgroundColor: '#fafafa', cursor: 'pointer' }
                                    : { backgroundColor: '#ffffff', cursor: 'pointer' };

                                return (
                                    <li className="list-group-item" {...getSuggestionItemProps(suggestion, { style })} >
                                        <span className="float-left">{suggestion.description}</span>
                                    </li>
                                );
                            })}
                        </ul>
                        <InputGroupAddon addonType="append">
                            <Button color="success" onClick={() => console.log('Hello')}>Search</Button>
                            <Button outline color="secondary" onClick={() => this.setState({ address: '' })}>Clear</Button>
                        </InputGroupAddon>
                    </InputGroup>
                )
                }
            </PlacesAutocomplete>
        );
    }

    render() {
        return (
            <Container className="p-5" >
                <Row>
                    <h3 className="float-left">Search</h3>
                </Row>
                <Row className="justify-content-center align-items-center">
                    <Col lg="8">
                        {this.renderPlacesAutoComplete()}
                    </Col>
                </Row >
            </Container >
        );
    }
};

export default LocationInput;
