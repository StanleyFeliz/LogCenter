// Chart.js rendering functions
window.chartsInterop = {
    isReady: false,
    chartInstances: {}, // Store chart instances here instead of on window
    
    // Check if Chart.js is loaded
    isChartJsLoaded: function() {
        return typeof Chart !== 'undefined';
    },
    
    // Initialize when Chart.js is available
    initialize: function() {
        if (this.isChartJsLoaded()) {
            this.isReady = true;
            console.log("Chart.js is ready");
            return true;
        }
        return false;
    },
    
    // Ensure Chart.js is ready (will retry if needed)
    ensureReady: function() {
        return new Promise((resolve, reject) => {
            if (this.isReady) {
                resolve(true);
                return;
            }
            
            if (this.initialize()) {
                resolve(true);
                return;
            }
            
            let attempts = 0;
            const maxAttempts = 10;
            const interval = setInterval(() => {
                attempts++;
                if (this.initialize() || attempts >= maxAttempts) {
                    clearInterval(interval);
                    if (this.isReady) {
                        console.log("Chart.js is now ready");
                        resolve(true);
                    } else {
                        console.error("Failed to initialize Chart.js after multiple attempts");
                        reject("Chart.js failed to initialize");
                    }
                }
            }, 100);
        });
    },
    
    // Safely destroy a chart instance if it exists
    safelyDestroyChart: function(chartId) {
        try {
            if (this.chartInstances[chartId] && 
                typeof this.chartInstances[chartId].destroy === 'function') {
                this.chartInstances[chartId].destroy();
                console.log(`Destroyed existing chart: ${chartId}`);
            }
        } catch (error) {
            console.warn(`Failed to destroy chart ${chartId}:`, error);
        }
        // Remove the reference regardless of destroy success
        this.chartInstances[chartId] = null;
    },
    
    // Render the application chart
    renderApplicationChart: function(data) {
        if (!this.isReady) {
            console.warn("Chart.js not fully loaded yet. Waiting for initialization...");
            return this.ensureReady().then(() => this.renderApplicationChart(data))
                .catch(err => console.error("Failed to render application chart:", err));
        }
        
        if (!data || Object.keys(data).length === 0) return;
    
        const canvasElement = document.getElementById('applicationChart');
        if (!canvasElement) {
            console.error('Cannot find canvas element with id "applicationChart"');
            return;
        }
    
        try {
            const ctx = canvasElement.getContext('2d');
            if (!ctx) {
                console.error('Failed to get 2D context for applicationChart');
                return;
            }
            
            // Convert the dictionary to arrays for Chart.js
            const labels = Object.keys(data);
            const values = Object.values(data);
            
            // Generate random colors for each application
            const backgroundColors = labels.map(() => this.getRandomColor(0.7));
            
            // Safely destroy any existing chart
            this.safelyDestroyChart('applicationChart');
            
            // Create and store new chart instance
            this.chartInstances['applicationChart'] = new Chart(ctx, {
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
        } catch (error) {
            console.error("Error rendering application chart:", error);
        }
    },
    
    // Render the level chart
    renderLevelChart: function(data) {
        if (!this.isReady) {
            console.warn("Chart.js not fully loaded yet. Waiting for initialization...");
            return this.ensureReady().then(() => this.renderLevelChart(data))
                .catch(err => console.error("Failed to render level chart:", err));
        }
        
        if (!data || Object.keys(data).length === 0) return;
    
        const canvasElement = document.getElementById('levelChart');
        if (!canvasElement) {
            console.error('Cannot find canvas element with id "levelChart"');
            return;
        }
    
        try {
            const ctx = canvasElement.getContext('2d');
            if (!ctx) {
                console.error('Failed to get 2D context for levelChart');
                return;
            }
            
            // Convert the dictionary to arrays for Chart.js
            const labels = Object.keys(data);
            const values = Object.values(data);
            
            // Define colors for log levels
            const backgroundColors = labels.map(level => this.getLevelColor(level.toLowerCase()));
            
            // Safely destroy any existing chart
            this.safelyDestroyChart('levelChart');
            
            // Create and store new chart instance
            this.chartInstances['levelChart'] = new Chart(ctx, {
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
        } catch (error) {
            console.error("Error rendering level chart:", error);
        }
    },
    
    // Render the date chart
    renderDateChart: function(data) {
        if (!this.isReady) {
            console.warn("Chart.js not fully loaded yet. Waiting for initialization...");
            return this.ensureReady().then(() => this.renderDateChart(data))
                .catch(err => console.error("Failed to render date chart:", err));
        }
        
        if (!data || Object.keys(data).length === 0) return;
    
        const canvasElement = document.getElementById('dateChart');
        if (!canvasElement) {
            console.error('Cannot find canvas element with id "dateChart"');
            return;
        }
    
        try {
            const ctx = canvasElement.getContext('2d');
            if (!ctx) {
                console.error('Failed to get 2D context for dateChart');
                return;
            }
            
            // Sort dates and convert to arrays for Chart.js
            const sortedDates = Object.keys(data).sort();
            const values = sortedDates.map(date => data[date]);
            
            // Format dates for display
            const labels = sortedDates.map(date => {
                const dateObj = new Date(date);
                return dateObj.toLocaleDateString();
            });
            
            // Safely destroy any existing chart
            this.safelyDestroyChart('dateChart');
            
            // Create and store new chart instance
            this.chartInstances['dateChart'] = new Chart(ctx, {
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
        } catch (error) {
            console.error("Error rendering date chart:", error);
        }
    },
    
    // Helper function to generate random colors
    getRandomColor: function(opacity = 1) {
        const r = Math.floor(Math.random() * 255);
        const g = Math.floor(Math.random() * 255);
        const b = Math.floor(Math.random() * 255);
        return `rgba(${r}, ${g}, ${b}, ${opacity})`;
    },
    
    // Helper function to get colors for log levels
    getLevelColor: function(level) {
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
};

// Initialize on load
window.addEventListener('load', function() {
    window.chartsInterop.initialize();
});

// Expose functions on the window for Blazor
window.isChartJsLoaded = function() {
    return window.chartsInterop.isChartJsLoaded();
};

window.ensureChartJsReady = function() {
    return window.chartsInterop.ensureReady();
};

window.renderApplicationChart = function(data) {
    return window.chartsInterop.renderApplicationChart(data);
};

window.renderLevelChart = function(data) {
    return window.chartsInterop.renderLevelChart(data);
};

window.renderDateChart = function(data) {
    return window.chartsInterop.renderDateChart(data);
}; 