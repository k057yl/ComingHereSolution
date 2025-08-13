window._maps = window._maps || {};
window._mapMarkers = window._mapMarkers || {};

// Универсальная функция для инициализации карты или получения существующей
function _getOrCreateMap(elementId, options = {}, center = [48.9226, 24.7111], zoom = 6) {
    if (!window.L) {
        console.error("Leaflet не загружен!");
        return null;
    }

    const container = document.getElementById(elementId);
    if (!container) {
        console.error(`Контейнер #${elementId} не найден!`);
        return null;
    }

    // Если карта уже есть — удаляем её (важно!)
    if (window._maps[elementId]) {
        window._mapMarkers[elementId]?.forEach(marker => {
            window._maps[elementId].removeLayer(marker);
        });
        window._mapMarkers[elementId] = [];

        window._maps[elementId].off();
        window._maps[elementId].remove();
        delete window._maps[elementId];
        delete window._mapMarkers[elementId];
    }

    // Проверяем, видим ли контейнер
    if (container.offsetWidth === 0 || container.offsetHeight === 0) {
        console.log(`Контейнер #${elementId} не видим. Повторный вызов через 100мс.`);
        setTimeout(() => _getOrCreateMap(elementId, options, center, zoom), 100);
        return null;
    }

    // Создаём карту
    const map = L.map(elementId, options).setView(center, zoom);

    // Добавляем слой
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; OpenStreetMap contributors'
    }).addTo(map);

    window._maps[elementId] = map;
    window._mapMarkers[elementId] = [];

    // Чтобы карта корректно отрисовалась
    setTimeout(() => map.invalidateSize(), 200);

    return map;
}

// Функция для очистки карты (маркеры + сам объект)
window.removeMap = function (elementId) {
    if (window._maps[elementId]) {
        window._mapMarkers[elementId]?.forEach(marker => {
            window._maps[elementId].removeLayer(marker);
        });
        window._mapMarkers[elementId] = [];

        window._maps[elementId].off();
        window._maps[elementId].remove();
        delete window._maps[elementId];
        delete window._mapMarkers[elementId];
    }
};

// Функция для рендера статичной карты с одним маркером (без взаимодействия)
window.renderStaticMap = function (elementId, lat, lng) {
    // Удаляем карту если была
    window.removeMap(elementId);

    // Создаём карту без взаимодействия
    const options = {
        zoomControl: false,
        dragging: false,
        scrollWheelZoom: false,
        doubleClickZoom: false,
        boxZoom: false,
        keyboard: false,
        touchZoom: false,
        tap: false,
        attributionControl: false,
    };

    const map = _getOrCreateMap(elementId, options, [lat, lng], 13);
    if (!map) return;

    L.marker([lat, lng]).addTo(map);
};

// Функция для старта карты с draggable-маркером и обратным вызовом в .NET
window.startMap = function (lat, lng, dotNetHelper) {
    // Удаляем старую карту, если есть
    window.removeMap('map');

    const map = _getOrCreateMap('map', {}, [lat, lng], 13);
    if (!map) return;

    const marker = L.marker([lat, lng], { draggable: true }).addTo(map);

    marker.on('dragend', function () {
        const pos = marker.getLatLng();
        dotNetHelper.invokeMethodAsync('UpdateCoordinates', pos.lat, pos.lng);
    });
};

// Функция для рендера событий с маркерами и кастомными иконками
window.renderEventMarkers = function (elementId, events) {
    const map = _getOrCreateMap(elementId, {}, [48.9226, 24.7111], 6);
    if (!map) return;

    // Удаляем старые маркеры
    window._mapMarkers[elementId].forEach(marker => map.removeLayer(marker));
    window._mapMarkers[elementId] = [];

    const iconUrls = {
        green: '/pic/markers/marker-green.png',
        yellow: '/pic/markers/marker-yellow.png',
        blue: '/pic/markers/marker-blue.png',
        violet: '/pic/markers/marker-violet.png'
    };

    const now = new Date();

    events.forEach(ev => {
        if (!ev.latitude || !ev.longitude) return;

        let markerColor = 'blue';
        const isRecurring = String(ev.isRecurring).toLowerCase() === "true" || ev.isRecurring === 1;

        if (isRecurring) {
            markerColor = 'violet';
        } else if (ev.startTime) {
            const startTime = new Date(ev.startTime);
            const diffDays = (startTime - now) / (1000 * 60 * 60 * 24);

            if (diffDays < 1) markerColor = 'green';
            else if (diffDays <= 7) markerColor = 'yellow';
        }

        const icon = new L.Icon({
            iconUrl: iconUrls[markerColor],
            shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-shadow.png',
            iconSize: [25, 41],
            iconAnchor: [12, 41],
            popupAnchor: [1, -34],
            shadowSize: [41, 41]
        });

        const popupContent = `
            <b>${ev.name}</b><br/>
            ${ev.description}<br/>
            <a href="/events/${ev.id}" style="
                display:inline-block;
                margin-top:5px;
                padding:4px 8px;
                background:#007bff;
                color:white;
                border-radius:4px;
                text-decoration:none;
                font-size: 0.9em;">
                Перейти к событию
            </a>
        `;

        const marker = L.marker([ev.latitude, ev.longitude], { icon })
            .addTo(map)
            .bindPopup(popupContent);

        window._mapMarkers[elementId].push(marker);
    });
};