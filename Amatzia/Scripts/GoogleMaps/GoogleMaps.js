function initMap() {
    var map = new google.maps.Map(
        document.getElementById('map'), { zoom: 4 });

    var geocoder = new google.maps.Geocoder();
    geocoder.geocode({
        'address': $('#countryValue')[0].innerText
    }, function (results, status) {
        if (status === 'OK') {
            map.setCenter(results[0].geometry.location);
            var marker = new google.maps.Marker({
                map: map,
                position: results[0].geometry.location
            });
        } else {
            window.alert('Geocode was not successful for the following reason: ' + status);
        }
    });
}