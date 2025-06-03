mergeInto(LibraryManager.library, {
    UomaGameLogout: function() {
        try {
            if (typeof window.gameLogout === 'function') {
                window.gameLogout();
            } else {
                console.log("gameLogout function not found in window");
            }
        } catch (e) {
            console.error("gameLogout failed:", e.message);
        }
    }
});