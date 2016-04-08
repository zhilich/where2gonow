function initializeSlider(id, popupText, values, index) {
    var slider = new dhtmlXSlider({
        parent: id,
        value: index,
        step: 1,
        min: 0,
        max: values.length - 1
    });

    slider.getAbsoluteValue = function () {
        return values[slider.getValue()];
    }

    // attach popup to slider
    var myPop = new dhtmlXPopup({ slider: slider });

    // change popup value when slider moved
    slider.attachEvent("onChange", function (value) {
        updatePopupValue();
    });

    updatePopupValue();

    function updatePopupValue() {
        myPop.attachHTML(window.dhx4.template(popupText, { value: slider.getAbsoluteValue() }));
    }

    return slider;
}

function str2coords(str) {
    var location = str.split(',');
    return { lat: parseFloat(location[0]), lng: parseFloat(location[1]) };
}

function initMap() {
    $("#loader").show();

    var geoLocation;

    var place = autocomplete.getPlace();
    if (place && place.geometry) {
        geoLocation = { lat: place.geometry.location.lat(), lng: place.geometry.location.lng() }
    }
    else {
        geoLocation = defaultLocation;
    }

    var radius = radiusSlider.getAbsoluteValue();
    var popularity = popularitySlider.getAbsoluteValue();

    $.getJSON("api/attractions/?lat=" + geoLocation.lat + "&lng=" + geoLocation.lng + "&radius=" + radius + "&popularity=" + popularity, function (attractions) {

        $("#details").hide();
        $("#loader").hide();
        $("#filter").css("visibility", "visible");

        var zoom = Math.round(Math.log($(window).height() / radius * 23) / Math.LN2);

        var map = new google.maps.Map(document.getElementById('map'), {
            zoom: zoom,
            center: geoLocation
        });

        var marker = new google.maps.Marker({
            position: geoLocation,
            map: map,
            title: "My location",
            icon: "images/marker-red.png"
        });

        var circle = new google.maps.Circle({
            strokeColor: '#FF0000',
            strokeOpacity: 0.8,
            strokeWeight: 2,
            fillColor: '#FF0000',
            fillOpacity: 0.05,
            map: map,
            center: geoLocation,
            radius: radius * 1609.34
        });

        for (var i = 0; i < attractions.length; i++) {
            var attraction = attractions[i];

            var marker = new google.maps.Marker({
                position: { lat: attraction.lat, lng: attraction.lng },
                map: map,
                title: attraction.customHover.title,
                icon: "images/marker-blue.png"
            });

            marker.attraction = attraction;

            (function (marker) {
                google.maps.event.addListener(marker, 'click', function () {
                    if (selected) {
                        selected.setIcon("images/marker-blue.png");
                    }
                    selected = marker;

                    marker.setIcon("images/marker-green.png");

                    $("#title").text(marker.attraction.customHover.title);
                    $("#category").text(marker.attraction.categories.join(", "));
                    $("#rating")
                      .rating({
                          initialRating: Math.floor(marker.attraction.rating),
                          maxRating: 5
                      });
                    $("#reviews").text(marker.attraction.reviews);

                    var imageUrl = marker.attraction.imgUrl;
                    if (!imageUrl) {
                        imageUrl = "images/no-image.png";
                    }

                    $("#photo").attr("src", imageUrl);

                    $("#details").show();
                });
            })(marker);
        }

    });
}

function navigate() {
    window.open("https://www.tripadvisor.com" + selected.attraction.url, '_blank');
}

var selected; //selected marker

var radiusSlider = initializeSlider("radius", "Distance: <#value# miles", [5, 10, 20, 30, 50, 70, 100, 150, 200, 250, 300, 400, 500, 600, 700, 800, 900, 1000], 4);
var popularitySlider = initializeSlider("popularity", "Popularity: #value#+ reviews", [0, 5, 10, 20, 30, 50, 70, 100, 150, 200, 250, 300, 400, 500, 600, 700], 4);
var defaultLocation = { lat: 41.878114, lng: -87.629798 };

var autocomplete = new google.maps.places.Autocomplete($("#location")[0]);

autocomplete.addListener('place_changed', function () {
    initMap();
});

//Determine user location
if (navigator.geolocation) {
    navigator.geolocation.getCurrentPosition(function (position) {
        defaultLocation = { lat: position.coords.latitude, lng: position.coords.longitude };
        initMap();
    }, initMap);
}
else {
    initMap();
}