import React, { Component } from 'react'
import Header from './Header'
import LocationInput from './LocationInput'
import ResultsTable from './ResultsTable';

class WeatherChecker extends Component {
    state = {
        weathers: []
    }

    async componentDidMount() {
        try {
            const response = await fetch(
                process.env.REACT_APP_WEATHER_URL,
            );

            const weathers = await response.json();

            this.setState({ weathers })

        } catch (error) {
            console.log(error);
        }
    }

    addWeather = (weather) => {
        this.setState({
            weathers: [weather, ...this.state.weathers]
        })
    }

    render() {
        return (
            <div className="wrapper">
                <Header />
                <LocationInput addWeather={this.addWeather} />
                <ResultsTable weathers={this.state.weathers} />
            </div>
        )
    }
}

export default WeatherChecker;
