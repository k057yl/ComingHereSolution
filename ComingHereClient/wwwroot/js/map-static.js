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

    if (container.offsetWidth === 0 || container.offsetHeight === 0) {
        console.log(`Контейнер ${mapId} еще не видим. Повторный вызов через 100мс.`);
        setTimeout(() => window.renderStaticMap(mapId, lat, lng), 100);
        return;
    }

    if (window._maps[mapId]) {
        try {
            window._maps[mapId].remove();
        } catch (e) {
            console.warn("Ошибка при удалении карты:", e);
        }
        window._maps[mapId] = null;
    }

    if (container._leaflet_id) {
        try {
            delete container._leaflet_id;
        } catch { }
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