document.addEventListener("DOMContentLoaded", () => {
    const modal = document.getElementById("components-reconnect-modal");
    if (!modal) return;

    let reconnectInterval = null;
    let secondsToNextAttempt = 0;

    const secondsElement = document.getElementById("components-seconds-to-next-attempt");
    const reconnectButton = document.getElementById("components-reconnect-button");
    const resumeButton = document.getElementById("components-resume-button");

    const show = (className) => {
        document.querySelectorAll(`.${className}`).forEach(el => el.classList.add(className.replace("-visible", "") + "-visible"));
    };

    modal.addEventListener("blazor-reconnect-please-wait", () => {
        modal.showModal();
    });

    modal.addEventListener("blazor-reconnect-please-wait", () => {
        document.querySelector(".components-rejoining-animation").style.display = "block";
        document.querySelector(".components-rejoin-first-attempt-visible").style.display = "block";
    });

    modal.addEventListener("blazor-reconnect-retry", () => {
        if (reconnectInterval) clearInterval(reconnectInterval);
        document.querySelector(".components-rejoin-first-attempt-visible").style.display = "none";
        document.querySelector(".components-rejoin-repeated-attempt-visible").style.display = "block";
        secondsToNextAttempt = 5;
        if (secondsElement) secondsElement.textContent = secondsToNextAttempt.toString();
        reconnectInterval = setInterval(() => {
            secondsToNextAttempt--;
            if (secondsElement) secondsElement.textContent = secondsToNextAttempt.toString();
            if (secondsToNextAttempt <= 0) {
                clearInterval(reconnectInterval);
                reconnectInterval = null;
            }
        }, 1000);
    });

    modal.addEventListener("blazor-reconnect-failed", () => {
        if (reconnectInterval) clearInterval(reconnectInterval);
        document.querySelector(".components-rejoin-repeated-attempt-visible").style.display = "none";
        document.querySelector(".components-rejoin-failed-visible").style.display = "block";
        if (reconnectButton) reconnectButton.style.display = "block";
    });

    modal.addEventListener("blazor-reconnect-pause", () => {
        document.querySelector(".components-rejoin-failed-visible").style.display = "none";
        if (reconnectButton) reconnectButton.style.display = "none";
        document.querySelector(".components-pause-visible").style.display = "block";
        if (resumeButton) resumeButton.style.display = "block";
    });

    reconnectButton?.addEventListener("click", () => {
        modal.close();
        window.location.reload();
    });

    resumeButton?.addEventListener("click", () => {
        modal.close();
        window.location.reload();
    });
});
