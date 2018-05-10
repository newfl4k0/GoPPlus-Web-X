var map;
var marker;
var directionsService;
function initMap() {
    var myLatLng = { lat: 21.1378588, lng: -101.6854752 };
    // Create a map object and specify the DOM element for display.
    map = new google.maps.Map(document.getElementById('map'), {
        center: myLatLng,
        scrollwheel: true,
        zoom: 9
    });
    directionsService = new google.maps.DirectionsService;
    CargarVehiculos(false);
    MostrarRutas();
}

function CargarVehiculos(ocultar_servicios) {
    //var ocultar_servicios = $('#Ocultar').is(":checked");
    var link = '@Url.Action("GetLocation", "Home", new { ocultar = "-1" })';
    link = link.replace("-1", ocultar_servicios);
    $.ajax({
        type: "GET",
        url: link,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (index, value) {
                    var latLng = { lat: value.Latitud_Actual, lng: value.Longitud_Actual };
                    var icon = '';
                    if (value.Estatus === "Libre")
                        icon = 'http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=%E2%80%A2|2ecc71&chf=a,s,ee00FFFF';
                    else if (value.Estatus === "En Servicio")
                        icon = 'http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=%E2%80%A2|3399FF&chf=a,s,ee00FFFF';
                    else if (value.Estatus === "Ausente (Comida)")
                        icon = 'http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=%E2%80%A2|FFFF00&chf=a,s,ee00FFFF';
                    else if (value.Estatus === "Ocupado")
                        icon = 'http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=%E2%80%A2|EC1C24&chf=a,s,ee00FFFF';
                    marker = new google.maps.Marker({
                        map: map,
                        position: latLng,
                        icon: icon,
                        clickable: true
                    });
                    marker.info = new google.maps.InfoWindow({
                        content: '<b>Vehículo:</b> ' + value.Vehiculo + '. <b>Conductor:</b>: ' + value.Conductor + '.'
                    });
                    google.maps.event.addListener(marker, 'click', function () {
                        marker.info.open(map, marker);
                    });
                });
            }
        },
    });
}

function MostrarRutas() {
    $.ajax({
        type: "GET",
        url: '@Url.Action("GetRoutes", "Home")',
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data != null) {
                $.each(data, function (index, value) {

                    var latLngOrigen = { lat: value.Latitud_Origen, lng: value.Longitud_Origen };
                    var latLngDestino = { lat: value.Latitud_Destino, lng: value.Longitud_Destino };

                    requestDirections(latLngOrigen, latLngDestino);
                });
            }
        },
    });
}
        
function renderDirections(result) {
    var directionsRenderer = new google.maps.DirectionsRenderer;
    directionsRenderer.setMap(map);
    directionsRenderer.setDirections(result);
}

function requestDirections(start, end) {
    directionsService.route({
        origin: start,
        destination: end,
        travelMode: google.maps.DirectionsTravelMode.DRIVING
    }, function (result) {
        renderDirections(result);
    });
}