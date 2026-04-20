document.addEventListener("DOMContentLoaded", () => {

    const el = {
        bet: document.getElementById("bet-amount"),
        profit: document.getElementById("profit-num"),
        roll: document.getElementById("roll-num"),
        multi: document.getElementById("multi-num"),
        chance: document.getElementById("chance-num"),
        slider: document.getElementById("slider-id"),

        clearBtn: document.getElementById("clearBtn"),
        plus1: document.getElementById("plusOneBtn"),
        plus10: document.getElementById("plusTenBtn"),
        half: document.getElementById("halfBtn"),
        double: document.getElementById("multiTwoBtn"),
        max: document.getElementById("maxBtn"),

        marker: document.getElementById("roll-marker"),
        rollTypeBtn: document.getElementById("rollTypeBtn"),
        rollBtn: document.getElementById("roll-dice")
    };


    const state = {
        rollType: "Under"
    };

    init();

    function init() {
        bindEvents();
        syncFromSlider();
        updateSliderBackground();
        updateProfit();
    }

    function bindEvents() {

        el.clearBtn.onclick = () => updateBet(0);
        el.plus1.onclick = () => updateBet(+el.bet.value + 1);
        el.plus10.onclick = () => updateBet(+el.bet.value + 10);
        el.half.onclick = () => updateBet((el.bet.value / 2).toFixed(2));
        el.double.onclick = () => updateBet((el.bet.value * 2).toFixed(2));

        el.max.addEventListener("click", setMaxBet);

        el.rollTypeBtn.onclick = toggleRollType;

        el.slider.addEventListener("input", handleSlider);
        el.roll.addEventListener("input", handleRollInput);
        el.multi.addEventListener("input", handleMultiplier);

        el.bet.addEventListener("input", updateProfit);

        el.rollBtn.addEventListener("click", rollDice);
    }

    function updateBet(value) {
        el.bet.value = value || 0;
        updateProfit();
    }

    async function setMaxBet() {
        const res = await fetch('/api/user/balance');
        if (!res.ok) return;

        const balance = await res.json();
        el.bet.value = balance.toFixed(2);
        updateProfit();
    }

    function toggleRollType() {
        state.rollType = state.rollType === "Under" ? "Over" : "Under";
        el.rollTypeBtn.innerText = state.rollType;

        syncFromSlider();
        updateSliderBackground();
    }

    function handleSlider() {
        let value = parseFloat(el.slider.value);

        if (state.rollType === "Under" && value > 95) value = 95;
        if (state.rollType === "Over" && value < 5.99) value = 5.99;

        el.slider.value = value;
        syncFromSlider();
    }

    function handleRollInput() {
        let value = parseFloat(el.roll.value);

        if (state.rollType === "Under" && value > 95) value = 95;
        if (state.rollType === "Over" && value < 5.99) value = 5.99;

        el.slider.value = value;
        syncFromSlider();
    }

    function handleMultiplier() {
        let multi = parseFloat(el.multi.value);
        if (!multi || multi <= 0) return;

        let chance = 99 / multi;

        if (chance > 95) chance = 95;
        if (chance < 0.01) chance = 0.01;

        el.chance.innerText = chance.toFixed(2) + "%";

        el.slider.value = state.rollType === "Under"
            ? chance
            : 100 - chance;

        el.roll.value = el.slider.value;

        updateSliderBackground();
        updateProfit();
    }

    function syncFromSlider() {
        let value = parseFloat(el.slider.value);

        if (state.rollType === "Under" && value > 95) value = 95;
        if (state.rollType === "Over" && value < 5.99) value = 5.99;

        el.slider.value = value;
        el.roll.value = value.toFixed(2);

        let chance = state.rollType === "Under"
            ? value
            : 100 - value;

        if (chance > 94) chance = 95;
        if (chance < 0.01) chance = 0.01;

        let multi = 95 / chance;
        if (multi === 1.00 && chance === 95) multi = 1.01;

        el.chance.innerText = chance.toFixed(2) + "%";
        el.multi.value = multi.toFixed(2);

        updateSliderBackground();
        updateProfit();
    }

    function updateSliderBackground() {
        let value = parseFloat(el.slider.value);

        if (state.rollType === "Under") {
            el.slider.style.background = `
                linear-gradient(to right,
                #00c853 0%,
                #00c853 ${value}%,
                #ff5252 ${value}%,
                #ff5252 100%)
            `;
        } else {
            el.slider.style.background = `
                linear-gradient(to right,
                #ff5252 0%,
                #ff5252 ${value}%,
                #00c853 ${value}%,
                #00c853 100%)
            `;
        }
    }

    function updateProfit() {
        let bet = parseFloat(el.bet.value);
        let multi = parseFloat(el.multi.value);

        if (!bet || !multi || bet <= 0) {
            el.profit.innerText = "0.00";
            return;
        }

        el.profit.innerText = (bet * multi).toFixed(2);
    }

    async function rollDice() {

        if (el.bet.value <= 0) return;

        const balanceRes = await fetch('/api/user/balance');
        if (!balanceRes.ok) return;

        const balance = await balanceRes.json();
        if (balance <= 0 || el.bet.value > balance) return;

        const res = await fetch('/api/dice/rolldice', {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                betAmount: el.bet.value,
                rollNumber: el.roll.value,
                rollType: state.rollType
            })
        });

        if (!res.ok) return;

        const data = await res.json();

        showRollMarker(data.value.rolled);
        loadBalance();
    }

    function showRollMarker(rollValue) {

        let min = parseFloat(el.slider.min);
        let max = parseFloat(el.slider.max);

        let percent = (rollValue - min) / (max - min) * 100;

        el.marker.style.left = percent + "%";
        el.marker.style.display = "block";

        el.marker.style.animation = "none";
        el.marker.offsetHeight;
        el.marker.style.animation = "tada 0.6s";

        el.marker.innerText = rollValue;

        const win =
            (rollValue > el.roll.value && state.rollType === "Over") ||
            (rollValue < el.roll.value && state.rollType === "Under");

        el.marker.style.background = win ? "#00c853" : "#ff5252";
    }

    const rulesBtn = document.getElementById("rules-toggle");
    const rulesBody = document.getElementById("rules-body");

    rulesBtn.addEventListener("click", () => {
        rulesBody.classList.toggle("open");
    });

});