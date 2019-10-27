import React, { Component } from 'react';
import PlacesAutocomplete, {
    geocodeByAddress,
    getLatLng,
} from 'react-places-autocomplete';
import { Container, Row, Col, InputGroup, Input, InputGroupAddon, Button } from 'reactstrap';

class LocationInput extends Component {
    initialState = {
        address: '',
        lat: null,
        lng: null,
        zip: null
    };
    state = this.initialState;

    handleSelect = async address => {

        try {

            const results = await geocodeByAddress(address);

            const latLng = await getLatLng(results[0]);

            address = results[0].formatted_address;

            const zip = parseInt(
                results[0]
                    .address_components[6]
                    .long_name, 10
            );

            this.setState({
                address,
                lat: latLng.lat,
                lng: latLng.lng,
                zip
            });
        }
        catch (error) {
            this.clearWeather();
            console.log(error);
        }
    };

    searchWeather = async () => {
        try {
            const response = await fetch(
                process.env.REACT_APP_WEATHER_URL,
                {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(this.state)
                }
            );

            const weather = await response.json();

            this.props.addWeather(weather);

        } catch (error) {
            console.log(error);
        }

        this.clearWeather()
    }

    clearWeather = () => {
        this.setState(this.initialState);
    }

    renderPlacesAutoComplete() {
        return (
            <PlacesAutocomplete
                value={this.state.address}
                onChange={address => this.setState({ address })}
                onSelect={this.handleSelect}
                searchOptions={{ types: ['address'] }}
                onError={(_, clearSuggestions) => clearSuggestions()}
            >
                {({ getInputProps, suggestions, getSuggestionItemProps, loading }) => (
                    <InputGroup>

                        <Input value={this.state.address} type="text" {...getInputProps({ placeholder: 'Search Address' })} />

                        <ul className="list-group" style={{ position: 'absolute', width: 'inherit', marginTop: '40px' }}>
                            {loading && <li className="list-group-item">Loading...</li>}

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
                            <Button disabled={!this.state.lat && !this.state.lng} color="success" onClick={() => this.searchWeather()}>Get Weather</Button>
                            <Button outline color="secondary" onClick={() => this.clearWeather()}>Clear</Button>
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
