window.renderChart = (canvasId, data) => {
    const ctx = document.getElementById(canvasId)?.getContext('2d');

    if (!ctx) {
        console.error("Canvas no encontrado:", canvasId);
        return;
    }

    new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ['Completados', 'Pendientes', 'Vencidos'],
            datasets: [{
                data: data,
                backgroundColor: ['#28a745', '#ffc107', '#dc3545']
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            cutout: '60%' // 🔥 hace el huequito tipo donut
        }
    });
};
