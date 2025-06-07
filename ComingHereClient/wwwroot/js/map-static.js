window._maps = window._maps || {};

window.renderStaticMap = function (mapId, lat, lng) {
    if (!window.L) {
        console.error("Leaflet не загружен!");
        return;
    }

    const container = document.getElementById(mapId);
    if (!container) {
        console.warn(`Контейнер с id ${mapId} не найден`);
        return;
    }

    if (window._maps[mapId]) {
        window._maps[mapId].remove();
        window._maps[mapId] = null;
    }

    const map = L.map(mapId, {
        zoomControl: false,
        dragging: false,
        scrollWheelZoom: false,
        doubleClickZoom: false,
        boxZoom: false,
        keyboard: false,
        touchZoom: false,
        tap: false,
        attributionControl: false,
    }).setView([lat, lng], 13);

    window._maps[mapId] = map;

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '',
    }).addTo(map);

    L.marker([lat, lng]).addTo(map);

    setTimeout(() => {
        map.invalidateSize();
    }, 200);
};