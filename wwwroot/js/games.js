document.addEventListener("DOMContentLoaded", () => {

    const blobs = document.querySelectorAll(".animated-bg span");

    blobs.forEach(blob => {
        setRandomStart(blob);
    animateBlob(blob);
    });

    function setRandomStart(blob) {
        const x = Math.random() * window.innerWidth;
    const y = Math.random() * window.innerHeight;

    blob.style.left = x + "px";
    blob.style.top = y + "px";
    }

    function animateBlob(blob) {
        const randomX = (Math.random() - 0.5) * 200;
    const randomY = (Math.random() - 0.5) * 200;
    const duration = 4000 + Math.random() * 4000;

    blob.style.transition = `transform ${duration}ms ease-in-out`;
    blob.style.transform = `translate(${randomX}px, ${randomY}px)`;

        setTimeout(() => animateBlob(blob), duration);
    }

});