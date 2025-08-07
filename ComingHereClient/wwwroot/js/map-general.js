/*window._maps = window._maps || {};
window._mapMarkers = window._mapMarkers || {};

window.renderEventMarkers = (elementId, events) => {
    // Если карта не создана, создаём
    if (!window._maps[elementId]) {
        window._maps[elementId] = L.map(elementId).setView([48.9226, 24.7111], 6);

        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; OpenStreetMap contributors'
        }).addTo(window._maps[elementId]);

        // Инициализируем массив маркеров
        window._mapMarkers[elementId] = [];
    }

    const map = window._maps[elementId];

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
        if (ev.latitude && ev.longitude) {
            let markerColor = 'blue';

            const isRecurring = String(ev.isRecurring).toLowerCase() === "true" || ev.isRecurring === 1;

            if (isRecurring) {
                markerColor = 'violet';
            } else if (ev.startTime) {
                const startTime = new Date(ev.startTime);
                const diffDays = (startTime - now) / (1000 * 60 * 60 * 24);

                if (diffDays < 1) {
                    markerColor = 'green';
                } else if (diffDays <= 7) {
                    markerColor = 'yellow';
                }
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
        }
    });
};*/