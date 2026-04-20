document.querySelectorAll('.btn-primary, .game-card').forEach(el => {
    el.addEventListener('click', function (e) {

        const circle = document.createElement("span");
        const diameter = Math.max(this.clientWidth, this.clientHeight);

        circle.style.width = circle.style.height = diameter + "px";
        circle.style.left = e.offsetX - diameter / 2 + "px";
        circle.style.top = e.offsetY - diameter / 2 + "px";

        circle.classList.add("ripple");

        this.appendChild(circle);

        setTimeout(() => circle.remove(), 600);
    });
});

