async function RequestWeatherCity() {
    await fetch("/api/weather/", {
        method: "POST",
        mode: "cors",
        cache: "no-cache",
        credentials: "same-origin",
        headers: {
            "Content-Type": "application/json"
        },
        redirect: "follow",
        referrerPolicy: "no-referrer",
        body: JSON.stringify({ city: $("#city").val(), countryCode: $("#countryCode").val() })
    }).then((response) => {
        if (response.ok) {
            return response.json();
        }
    }).then((result) => {
        if (result["data"] !== null) {
            $("#weather-forcast").empty()
            result["data"].forEach((data) => {
                let element = BuildWeatherElement(data);
                $("#weather-forcast").append(element);
            })
        }
        else {
            $("#weather-forcast").html(result["error"]);
        }
    }).catch(err => {
        console.error('error occured: ', err.message)
        $("#weather-forcast").html(err.message)
    });
}

function RequestWeatherGeolocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(RequestWeatherGeolocationSuccess, RequestWeatherGeolocationFailure);
    } else {
        $("#weather-forcast").val("Geolocation is not supported by this browser.");
    }
}

async function RequestWeatherGeolocationSuccess(position) {
    await fetch("/api/weather/", {
        method: "POST",
        mode: "cors",
        cache: "no-cache",
        credentials: "same-origin",
        headers: {
            "Content-Type": "application/json"
        },
        redirect: "follow",
        referrerPolicy: "no-referrer",
        body: JSON.stringify({ lat: position.coords.latitude, lon: position.coords.longitude })
    }).then((response) => {
        if (response.ok) {
            return response.json();
        }
    }).then((result) => {
        if (result["data"] !== null) {
            $("#weather-forcast").empty()
            result["data"].forEach((data) => {
                let element = BuildWeatherElement(data);
                $("#weather-forcast").append(element);
            })
        }
        else {
            $("#weather-forcast").html(result["error"]);
        }
    }).catch(err => {
        console.error('error occured: ', err.message)
        $("#weather-forcast").html(err.message)
    });
}

function RequestWeatherGeolocationFailure() {
    $("#weather-forcast").val("Unable to get current geolocation.")
}

function BuildWeatherElement(data) {
    return (
        `<div class="row-sm card justify-content-between">
            <p>${data["time"]}</p>
            <p>Temp: ${data["temp"]}&deg;C</p>
            <p>Min Temp: ${data["tempMin"]}&deg;C</p>
            <p>Max Temp: ${data["tempMax"]}&deg;C</p>
            <p>Cloud: ${data["cloud"]}%</p>
            <p>Humidity: ${data["humidity"]}%</p>
            <p>Weather: ${data["weather"]}</p>
            <p>Description: ${data["description"]}</p>
        </div>`
    )
}

$(document).ready(function () {
    $("#cityButton").click(RequestWeatherCity);
    $("#locationButton").click(RequestWeatherGeolocation);
});