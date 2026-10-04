// Leaflet Map Interop for EcoMeal
window.leafletMap = {
    map: null,
    markersLayer: null,

    initMap: function (elementId, businesses, dotNetHelper) {
        // If map already exists, remove it first
        if (this.map) {
            this.map.remove();
            this.map = null;
        }

        const mapContainer = document.getElementById(elementId);
        if (!mapContainer) return;

        // Default center (e.g. Craiova: 44.3302, 23.7949)
        const defaultLat = 44.3302;
        const defaultLng = 23.7949;

        this.map = L.map(elementId).setView([defaultLat, defaultLng], 13);

        // OpenStreetMap tile layer
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
            maxZoom: 19
        }).addTo(this.map);

        this.markersLayer = L.layerGroup().addTo(this.map);
        this.updateMarkers(businesses, dotNetHelper);
    },

    updateMarkers: function (businesses, dotNetHelper) {
        if (!this.map || !this.markersLayer) return;

        this.markersLayer.clearLayers();

        if (!businesses || businesses.length === 0) return;

        const bounds = [];

        // Custom EcoMeal green pin icon
        const ecoIcon = L.divIcon({
            className: 'eco-custom-pin',
            html: `<div style="
                background: linear-gradient(135deg, #10b981 0%, #059669 100%);
                width: 36px;
                height: 36px;
                border-radius: 50% 50% 50% 0;
                transform: rotate(-45deg);
                box-shadow: 0 4px 10px rgba(0,0,0,0.3);
                display: flex;
                align-items: center;
                justify-content: center;
                border: 2px solid white;
            ">
                <i class="bi bi-shop" style="
                    transform: rotate(45deg);
                    color: white;
                    font-size: 16px;
                "></i>
            </div>`,
            iconSize: [36, 36],
            iconAnchor: [18, 36],
            popupAnchor: [0, -36]
        });

        businesses.forEach(business => {
            if (business.latitude && business.longitude) {
                const lat = business.latitude;
                const lng = business.longitude;
                bounds.push([lat, lng]);

                const marker = L.marker([lat, lng], { icon: ecoIcon }).addTo(this.markersLayer);

                const popupContent = `
                    <div style="min-width: 200px; padding: 4px;">
                        <h6 style="margin: 0 0 6px 0; font-weight: 700; color: #0f172a; font-size: 15px;">${escapeHtml(business.name)}</h6>
                        <p style="margin: 0 0 8px 0; font-size: 12px; color: #64748b;">
                            <i class="bi bi-geo-alt-fill text-danger me-1"></i>${escapeHtml(business.address || '')}
                        </p>
                        ${business.description ? `<p style="margin: 0 0 10px 0; font-size: 12px; color: #475569;">${escapeHtml(business.description.length > 80 ? business.description.substring(0, 80) + '...' : business.description)}</p>` : ''}
                        <button class="btn btn-sm btn-success w-100 fw-bold" onclick="window.leafletMap.onSelectBusiness('${business.id}')">
                            <i class="bi bi-bag-plus me-1"></i> View Packages
                        </button>
                    </div>
                `;

                marker.bindPopup(popupContent);
            }
        });

        this.currentDotNetHelper = dotNetHelper;

        if (bounds.length > 0) {
            this.map.fitBounds(bounds, { padding: [40, 40], maxZoom: 15 });
        }
    },

    onSelectBusiness: function (businessId) {
        if (this.currentDotNetHelper) {
            this.currentDotNetHelper.invokeMethodAsync('SelectBusinessFromMap', businessId);
        }
    }
};

function escapeHtml(text) {
    if (!text) return '';
    return text
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}
