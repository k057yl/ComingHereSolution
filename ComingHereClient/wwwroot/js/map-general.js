window.renderEventMarkers = (elementId, events) => {
    const map = L.map(elementId).setView([48.9226, 24.7111], 6);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; OpenStreetMap contributors'
    }).addTo(map);

    const iconUrls = {
        green: 'https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-green.png',
        yellow: 'https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-yellow.png',
        blue: 'https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-blue.png'
    };

    const now = new Date();

    events.forEach(ev => {
        if (ev.latitude && ev.longitude && ev.startTime) {
            const startTime = new Date(ev.startTime);
            const diffDays = (startTime - now) / (1000 * 60 * 60 * 24);

            let markerColor = 'blue';
            if (diffDays < 1) {
                markerColor = 'green';
            } else if (diffDays <= 7) {
                markerColor = 'yellow';
            }

            const icon = new L.Icon({
                iconUrl: iconUrls[markerColor],
                shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-shadow.png',
                iconSize: [25, 41],
                iconAnchor: [12, 41],
                popupAnchor: [1, -34],
                shadowSize: [41, 41]
            });

            L.marker([ev.latitude, ev.longitude], { icon })
                .addTo(map)
                .bindPopup(`<b>${ev.name}</b><br/>${ev.description}`);
        }
    });
};