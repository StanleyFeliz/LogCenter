// Blazor interoperability functions for LogCenter Dashboard
// These are loaded early to ensure they're available when Blazor initializes

// Create a placeholder for chartsInterop if it doesn't exist yet
window.chartsInterop = window.chartsInterop || {
    isReady: false,
    chartInstances: {},
    isChartJsLoaded: function() { return false; },
    ensureReady: function() { return Promise.resolve(false); },
    renderApplicationChart: function() { console.warn("Chart functions not fully loaded"); },
    renderLevelChart: function() { console.warn("Chart functions not fully loaded"); },
    renderDateChart: function() { console.warn("Chart functions not fully loaded"); }
};

// Chart.js interop functions
window.isChartJsLoaded = function() { 
    return typeof Chart !== 'undefined'; 
};

window.ensureChartJsReady = function() { 
    if (window.chartsInterop && typeof window.chartsInterop.ensureReady === 'function') {
        return window.chartsInterop.ensureReady();
    }
    
    return new Promise((resolve) => {
        if (window.isChartJsLoaded()) {
            // If Chart.js is available, resolve immediately
            resolve(true);
            return;
        }

        // Wait for a moment and check again
        setTimeout(() => {
            if (window.isChartJsLoaded()) {
                resolve(true);
            } else {
                // If still not loaded, return false but don't reject
                // This allows the app to continue even if Chart.js isn't available
                console.warn("Chart.js not loaded after delay");
                resolve(false);
            }
        }, 500);
    }); 
};

// Basic placeholder chart rendering functions
// These will be replaced by the full implementation in charts.js
window.renderApplicationChart = function(data) { 
    if (window.chartsInterop && typeof window.chartsInterop.renderApplicationChart === 'function') {
        return window.chartsInterop.renderApplicationChart(data);
    }
    console.warn("Chart rendering functions not fully loaded");
};

window.renderLevelChart = function(data) { 
    if (window.chartsInterop && typeof window.chartsInterop.renderLevelChart === 'function') {
        return window.chartsInterop.renderLevelChart(data);
    }
    console.warn("Chart rendering functions not fully loaded");
};

window.renderDateChart = function(data) { 
    if (window.chartsInterop && typeof window.chartsInterop.renderDateChart === 'function') {
        return window.chartsInterop.renderDateChart(data);
    }
    console.warn("Chart rendering functions not fully loaded");
}; 