window.tour360 = {
    initViewer: function (elementId, imageUrl) {
        const container = document.getElementById(elementId);
        if (!container) {
            console.warn("tour360: container not found: " + elementId);
            return;
        }

        // Si ya existe un visor, lo destruimos para evitar fugas
        if (container._viewer) {
            container._viewer.destroy();
            container._viewer = null;
        }

        // Crear el visor 360°
        container._viewer = new PhotoSphereViewer.Viewer({
            container: container,
            panorama: imageUrl,
            touchmoveTwoFingers: true,
            defaultZoomLvl: 0,
            navbar: [
                'zoom',
                'fullscreen',
                'download',
                'markers'
            ]
        });
    }
};
