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
        return response.json();
    }).then((result) => {
        result["data"].forEach((data) => {
            let element = BuildWeatherElement(data);
            $("#weather-forcast").append(element);
        })
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
        return response.json();
    }).then((result) => {
        result["data"].forEach((data) => {
            let element = BuildWeatherElement(data);
            $("#weather-forcast").append(element);
        })
    }).catch(err => {
        console.error('error occured: ', err.message)
        $("#weather-forcast").html(err.message)
    });
}

function RequestWeatherGeolocationFailure() {
    $("#weather-forcast").val("Unable to get current geolocation.")
}

function BuildWeatherElement(data) {
    return `<div>${data["temp"]}</div>`
}

$(document).ready(function () {
    $("#cityButton").click(RequestWeatherCity);
    $("#locationButton").click(RequestWeatherGeolocation);
});