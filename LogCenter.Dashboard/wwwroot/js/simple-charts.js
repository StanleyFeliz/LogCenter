// Simple Charts Implementation
// A more straightforward approach to rendering charts in the LogCenter Dashboard

(function() {
    // Check if Chart.js is loaded
    function isChartJsAvailable() {
        return typeof Chart !== 'undefined';
    }
    
    // Clear a canvas by removing it and creating a new one
    function resetCanvas(canvasId) {
        try {
            const container = document.getElementById(canvasId + '-container');
            if (!container) {
                console.error(`Cannot find container for canvas: ${canvasId}-container`);
                return null;
            }
            
            // Remove the existing canvas
            const existingCanvas = document.getElementById(canvasId);
            if (existingCanvas) {
                existingCanvas.remove();
            }
            
            // Create a new canvas
            const newCanvas = document.createElement('canvas');
            newCanvas.id = canvasId;
            container.appendChild(newCanvas);
            
            return newCanvas;
        } catch (error) {
            console.error(`Error resetting canvas ${canvasId}:`, error);
            return null;
        }
    }
    
    // Generate a random color
    function getRandomColor(opacity = 1) {
        const r = Math.floor(Math.random() * 255);
        const g = Math.floor(Math.random() * 255);
        const b = Math.floor(Math.random() * 255);
        return `rgba(${r}, ${g}, ${b}, ${opacity})`;
    }
    
    // Get color for log level
    function getLevelColor(level) {
        switch (level.toLowerCase()) {
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
    
    // Render application chart
    function renderApplicationBarChart(data) {
        if (!isChartJsAvailable()) {
            console.error("Chart.js is not available");
            return false;
        }
        
        if (!data || Object.keys(data).length === 0) {
            console.warn("No application data to render");
            return false;
        }
        
        // Reset canvas
        const canvas = resetCanvas('applicationChart');
        if (!canvas) return false;
        
        const ctx = canvas.getContext('2d');
        if (!ctx) {
            console.error("Could not get 2D context for application chart");
            return false;
        }
        
        // Prepare data
        const labels = Object.keys(data);
        const values = Object.values(data);
        const backgroundColors = labels.map(() => getRandomColor(0.7));
        
        // Create chart
        try {
            new Chart(ctx, {
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
            return true;
        } catch (error) {
            console.error("Error creating application chart:", error);
            return false;
        }
    }
    
    // Render level chart
    function renderLevelPieChart(data) {
        if (!isChartJsAvailable()) {
            console.error("Chart.js is not available");
            return false;
        }
        
        if (!data || Object.keys(data).length === 0) {
            console.warn("No level data to render");
            return false;
        }
        
        // Reset canvas
        const canvas = resetCanvas('levelChart');
        if (!canvas) return false;
        
        const ctx = canvas.getContext('2d');
        if (!ctx) {
            console.error("Could not get 2D context for level chart");
            return false;
        }
        
        // Prepare data
        const labels = Object.keys(data);
        const values = Object.values(data);
        const backgroundColors = labels.map(level => getLevelColor(level));
        
        // Create chart
        try {
            new Chart(ctx, {
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
            return true;
        } catch (error) {
            console.error("Error creating level chart:", error);
            return false;
        }
    }
    
    // Render date chart
    function renderDateLineChart(data) {
        if (!isChartJsAvailable()) {
            console.error("Chart.js is not available");
            return false;
        }
        
        if (!data || Object.keys(data).length === 0) {
            console.warn("No date data to render");
            return false;
        }
        
        // Reset canvas
        const canvas = resetCanvas('dateChart');
        if (!canvas) return false;
        
        const ctx = canvas.getContext('2d');
        if (!ctx) {
            console.error("Could not get 2D context for date chart");
            return false;
        }
        
        // Sort and prepare data
        const sortedDates = Object.keys(data).sort();
        const values = sortedDates.map(date => data[date]);
        
        // Format dates for display
        const labels = sortedDates.map(date => {
            const dateObj = new Date(date);
            return dateObj.toLocaleDateString();
        });
        
        // Create chart
        try {
            new Chart(ctx, {
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
            return true;
        } catch (error) {
            console.error("Error creating date chart:", error);
            return false;
        }
    }
    
    // Expose functions to global scope
    window.renderApplicationChart = function(data) {
        // Wait until Chart.js is loaded
        if (!isChartJsAvailable()) {
            setTimeout(() => window.renderApplicationChart(data), 500);
            return;
        }
        return renderApplicationBarChart(data);
    };
    
    window.renderLevelChart = function(data) {
        // Wait until Chart.js is loaded
        if (!isChartJsAvailable()) {
            setTimeout(() => window.renderLevelChart(data), 500);
            return;
        }
        return renderLevelPieChart(data);
    };
    
    window.renderDateChart = function(data) {
        // Wait until Chart.js is loaded
        if (!isChartJsAvailable()) {
            setTimeout(() => window.renderDateChart(data), 500);
            return;
        }
        return renderDateLineChart(data);
    };
    
    // Notify that simple charts are ready
    console.log("Simple Charts module loaded");
})(); 