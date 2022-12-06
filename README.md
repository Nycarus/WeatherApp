## Weather App
Purpose of this app is to explore .NET 6 MVC for educational purposes. The app is able to display weather data using geolocation or city name using openweathermap API.

## How to run?
1. To run the app locally, first add your openweathermap.org key to the docker-compose.yml file in the root folder that displays this line by replacing "<insert_key>".
    
        WeatherApi:ServiceApiKey: <insert_key>

2. Navigate to the root directory and enter the following to create a weather-app container and image. You can then run your app through the docker container. The URL for the project is http://localhost:80.

        docker compose up

3. To stop the container, run the following.

        docker compose down

## Licenses
Weather data are taken from [openweathermap.org](https://openweathermap.org/faq).