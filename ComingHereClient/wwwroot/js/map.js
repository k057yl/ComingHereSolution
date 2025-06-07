window._mapInstance = null;

window.startMap = function (lat, lng, dotNetHelper) {
    if (!window.L) {
        console.error("Leaflet не загружен!");
        return;
    }
    const mapContainer = document.getElementById('map');
    if (!mapContainer) {
        console.error("Элемент #map не найден!");
        return;
    }

    if (window._mapInstance) {
        window._mapInstance.remove();
        window._mapInstance = null;
    }

    const map = L.map('map').setView([lat, lng], 13);
    window._mapInstance = map;

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap contributors'
    }).addTo(map);

    const marker = L.marker([lat, lng], { draggable: true }).addTo(map);

    marker.on('dragend', function () {
        const position = marker.getLatLng();
        dotNetHelper.invokeMethodAsync('UpdateCoordinates', position.lat, position.lng);
    });

    setTimeout(() => {
        map.invalidateSize();
    }, 300);
};