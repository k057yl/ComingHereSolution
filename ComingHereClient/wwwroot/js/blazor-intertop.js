window.registerOutsideClickHandler = (dropdownId, dotnetRef) => {
    function handler(event) {
        const dropdown = document.getElementById(dropdownId);
        if (dropdown && !dropdown.contains(event.target)) {
            dotnetRef.invokeMethodAsync("CloseLangMenu");
        }
    }
    document.addEventListener("mousedown", handler);

    return () => document.removeEventListener("mousedown", handler);
};