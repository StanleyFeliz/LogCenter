// Bootstrap Modal Utilities

// Show a bootstrap modal by its ID (without the # prefix)
window.showBootstrapModal = function(modalId) {
    try {
        const modalElement = document.getElementById(modalId);
        if (!modalElement) {
            console.error(`Modal element with ID "${modalId}" not found`);
            return false;
        }
        
        // Try to get the Bootstrap 5 modal instance
        let modal = bootstrap.Modal.getInstance(modalElement);
        
        // If the modal doesn't exist yet, create it
        if (!modal) {
            modal = new bootstrap.Modal(modalElement, {
                backdrop: 'static',  // Don't close when clicking outside
                keyboard: true       // Allow ESC key to close
            });
        }
        
        // Show the modal
        modal.show();
        return true;
    } catch (error) {
        console.error(`Error showing modal "${modalId}":`, error);
        return false;
    }
};

// Hide a bootstrap modal by its ID (without the # prefix)
window.hideBootstrapModal = function(modalId) {
    try {
        const modalElement = document.getElementById(modalId);
        if (!modalElement) {
            console.error(`Modal element with ID "${modalId}" not found`);
            return false;
        }
        
        // Try to get the Bootstrap 5 modal instance
        const modal = bootstrap.Modal.getInstance(modalElement);
        
        // If the modal exists, hide it
        if (modal) {
            modal.hide();
            return true;
        } else {
            console.warn(`No Bootstrap modal instance found for "${modalId}"`);
            return false;
        }
    } catch (error) {
        console.error(`Error hiding modal "${modalId}":`, error);
        return false;
    }
};

// Set up event listeners on a modal to call back to a .NET component reference
window.setupModalCallbacks = function(modalId, dotNetHelper) {
    try {
        const modalElement = document.getElementById(modalId);
        if (!modalElement) {
            console.error(`Modal element with ID "${modalId}" not found`);
            return false;
        }
        
        // Add the hidden.bs.modal event handler to call back to .NET
        modalElement.addEventListener('hidden.bs.modal', function() {
            dotNetHelper.invokeMethodAsync('OnModalHidden');
        });
        
        // Also add shown.bs.modal if needed
        modalElement.addEventListener('shown.bs.modal', function() {
            dotNetHelper.invokeMethodAsync('OnModalShown');
        });
        
        return true;
    } catch (error) {
        console.error(`Error setting up modal callbacks for "${modalId}":`, error);
        return false;
    }
}; 