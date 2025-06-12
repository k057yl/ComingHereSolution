window.renderEventMarkers = (elementId, events) => {
    const map = L.map(elementId).setView([48.9226, 24.7111], 6);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; OpenStreetMap contributors'
    }).addTo(map);

    events.forEach(ev => {
        if (ev.latitude && ev.longitude) {
            L.marker([ev.latitude, ev.longitude])
                .addTo(map)
                .bindPopup(`<b>${ev.name}</b><br/>${ev.description}`);
        }
    });
};