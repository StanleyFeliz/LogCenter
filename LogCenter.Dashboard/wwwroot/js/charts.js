// Chart.js rendering functions

function renderApplicationChart(data) {
    if (!data || Object.keys(data).length === 0) return;

    const ctx = document.getElementById('applicationChart').getContext('2d');
    
    // Convert the dictionary to arrays for Chart.js
    const labels = Object.keys(data);
    const values = Object.values(data);
    
    // Generate random colors for each application
    const backgroundColors = labels.map(() => getRandomColor(0.7));
    
    if (window.applicationChart) {
        window.applicationChart.destroy();
    }
    
    window.applicationChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Log Count',
                data: values,
                backgroundColor: backgroundColors,
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    beginAtZero: true,
                    title: {
                        display: true,
                        text: 'Number of Logs'
                    }
                },
                x: {
                    title: {
                        display: true,
                        text: 'Application'
                    }
                }
            }
        }
    });
}

function renderLevelChart(data) {
    if (!data || Object.keys(data).length === 0) return;

    const ctx = document.getElementById('levelChart').getContext('2d');
    
    // Convert the dictionary to arrays for Chart.js
    const labels = Object.keys(data);
    const values = Object.values(data);
    
    // Define colors for log levels
    const backgroundColors = labels.map(level => getLevelColor(level.toLowerCase()));
    
    if (window.levelChart) {
        window.levelChart.destroy();
    }
    
    window.levelChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: labels,
            datasets: [{
                data: values,
                backgroundColor: backgroundColors,
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'right'
                }
            }
        }
    });
}

function renderDateChart(data) {
    if (!data || Object.keys(data).length === 0) return;

    const ctx = document.getElementById('dateChart').getContext('2d');
    
    // Sort dates and convert to arrays for Chart.js
    const sortedDates = Object.keys(data).sort();
    const values = sortedDates.map(date => data[date]);
    
    // Format dates for display
    const labels = sortedDates.map(date => {
        const dateObj = new Date(date);
        return dateObj.toLocaleDateString();
    });
    
    if (window.dateChart) {
        window.dateChart.destroy();
    }
    
    window.dateChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'Log Count',
                data: values,
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                borderColor: 'rgba(75, 192, 192, 1)',
                borderWidth: 2,
                tension: 0.1,
                fill: true
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    beginAtZero: true,
                    title: {
                        display: true,
                        text: 'Number of Logs'
                    }
                },
                x: {
                    title: {
                        display: true,
                        text: 'Date'
                    }
                }
            }
        }
    });
}

// Helper function to generate random colors
function getRandomColor(opacity = 1) {
    const r = Math.floor(Math.random() * 255);
    const g = Math.floor(Math.random() * 255);
    const b = Math.floor(Math.random() * 255);
    return `rgba(${r}, ${g}, ${b}, ${opacity})`;
}

// Helper function to get colors for log levels
function getLevelColor(level) {
    switch (level) {
        case 'error':
            return 'rgba(220, 53, 69, 0.8)'; // danger
        case 'warning':
            return 'rgba(255, 193, 7, 0.8)'; // warning
        case 'information':
            return 'rgba(13, 202, 240, 0.8)'; // info
        case 'debug':
            return 'rgba(108, 117, 125, 0.8)'; // secondary
        case 'trace':
            return 'rgba(233, 236, 239, 0.8)'; // light
        case 'critical':
            return 'rgba(33, 37, 41, 0.8)'; // dark
        default:
            return 'rgba(13, 110, 253, 0.8)'; // primary
    }
} 