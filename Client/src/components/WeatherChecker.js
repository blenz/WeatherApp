import React, { Component } from 'react'
import Header from './Header'
import AddressInput from './AddressInput'
import WeatherTable from './WeatherTable';

class WeatherChecker extends Component {
    state = {
        weathers: []
    }

    async componentDidMount() {
        try {
            const response = await fetch('/api/weather');

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
                <AddressInput addWeather={this.addWeather} />
                <WeatherTable weathers={this.state.weathers} />
            </div>
        )
    }
}

export default WeatherChecker;
