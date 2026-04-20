let gameFinished = false;

document.getElementById("restart").style.display = "none";
document.getElementById("hit").style.display = "none";
document.getElementById("stand").style.display = "none";
document.getElementById("startGame").style.display = "none";
document.getElementById("game-area").style.display = "none";

document.getElementById("bet-number").addEventListener("input", () => {
    const value = document.getElementById("bet-number").value;
    const startBtn = document.getElementById("startGame");

    if (!value || value <= 0) {
        startBtn.style.display = "none";
    }
    else {
        startBtn.style.display = "inline-block";
    }
});


document.getElementById("startGame").addEventListener("click", async () => {
    try {
        gameFinished = false;

        const betNumber = document.getElementById("bet-number").value;
        const parsedNumber = parseInt(betNumber);

        if (isNaN(parsedNumber) || parsedNumber <= 0) {
            document.getElementById("bet-error").innerText = "Enter valid bet!";
            return;
        }

        document.getElementById("bet-error").innerText = "";

        const betRes = await fetch('/api/blackjack/bet', {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(parsedNumber)
        });

        if (!betRes.ok) {
            document.getElementById("bet-error").innerText = await betRes.text();
            return
        }

        await fetch('/api/Blackjack/startgame', { method: "POST" });

        document.getElementById("bet-container").style.display = "none";
        document.getElementById("startGame").style.display = "none";

        document.getElementById("game-area").style.display = "block";

        document.getElementById("hit").style.display = "inline-block";
        document.getElementById("stand").style.display = "inline-block";

        await updateUI();
        await checkGameStatus();

    } catch (error) {
        console.error(error);
    }
});


document.getElementById("hit").addEventListener("click", async () => {
    try {
        if (gameFinished) return;

        await fetch(`/api/Blackjack/hit`, { method: "POST" });

        await updateUI();

        const playerScore = await (await fetch('/api/Blackjack/playerscore')).json();

        if (playerScore >= 21) {
            await checkGameStatus();
        }

    } catch (error) {
        console.error(error);
    }
});


document.getElementById("stand").addEventListener("click", async () => {
    try {
        if (gameFinished) return;

        await fetch(`/api/Blackjack/stand`, { method: "POST" });

        await updateUI();

        await checkGameStatus();

    } catch (error) {
        console.error(error);
    }
});

document.getElementById("restart").addEventListener("click", async () => {
    try {
        gameFinished = false;

        await fetch(`/api/Blackjack/restart`, { method: "POST" });

        document.getElementById("game-status").innerHTML = "";

        document.getElementById("game-area").style.display = "none";

        document.getElementById("bet-container").style.display = "inline-block";
        document.getElementById("hit").style.display = "none";
        document.getElementById("stand").style.display = "none";
        document.getElementById("restart").style.display = "none";

        document.getElementById("dealer-hand").innerHTML = "";
        document.getElementById("player-hand").innerHTML = "";

        document.getElementById("dealer-score").innerHTML = "";
        document.getElementById("player-score").innerHTML = "";

        const value = document.getElementById("bet-number").value;
        const startBtn = document.getElementById("startGame");

        if (value && value > 0) {
            startBtn.style.display = "inline-block";
        } else {
            startBtn.style.display = "none";
        }

        loadBalance();

    } catch (error) {
        console.error(error);
    }
});


async function updateUI() {

    const dealerScore = await (await fetch('/api/Blackjack/dealerscore')).json();
    const playerScore = await (await fetch('/api/Blackjack/playerscore')).json();

    document.getElementById("player-score").innerHTML = playerScore;

    document.getElementById("dealer-score").innerHTML = dealerScore;

    const dealerData = await (await fetch('/api/Blackjack/dealerhand')).json();
    const playerData = await (await fetch('/api/Blackjack/playerhand')).json();

    const dealerDiv = document.getElementById("dealer-hand");
    const playerDiv = document.getElementById("player-hand");

    dealerDiv.innerHTML = "";
    playerDiv.innerHTML = "";

    if (!gameFinished) {
        if (dealerData.length > 0) {
            showCard(dealerData[0], "dealer-hand");
        }
        showCard("hidden-card", "dealer-hand");
    } else {
        dealerData.forEach(card => showCard(card, "dealer-hand"));
    }

    playerData.forEach(card => showCard(card, "player-hand"));

    if (playerScore >= 21) {
        document.getElementById("hit").style.display = "none";
        document.getElementById("stand").style.display = "none";
    }
}

async function checkGameStatus() {

    if (gameFinished) return;

    const statusRes = await fetch('/api/Blackjack/status');
    const status = await statusRes.text();

    const statusDiv = document.getElementById("game-status");

    if (
        status === "Player wins!" ||
        status === "Dealer wins!" ||
        status === "Draw!"
    ) {
        gameFinished = true;

        const dealerData = await (await fetch('/api/Blackjack/dealerhand')).json();
        const dealerScore = await (await fetch('/api/Blackjack/dealerscore')).json();
        const playerScore = await (await fetch('/api/Blackjack/playerscore')).json();

        document.getElementById("dealer-score").innerHTML = "...";

        const dealerDiv = document.getElementById("dealer-hand");
        dealerDiv.innerHTML = "";

        if (playerScore >= 21) {
            dealerData.forEach(card => showCard(card, "dealer-hand"));
        }
        else {
            await animateDealerCards(dealerData);
        }

        document.getElementById("dealer-score").innerHTML = dealerScore;

        const res = await fetch('/api/blackjack/finishgame', {
            method: "POST"
        });

        const final = await res.text();
        statusDiv.innerHTML = final;

        document.getElementById("hit").style.display = "none";
        document.getElementById("stand").style.display = "none";
        document.getElementById("restart").style.display = "inline-block";

        loadBalance();
    }
}


async function animateDealerCards(dealerData) {

    const dealerDiv = document.getElementById("dealer-hand");
    dealerDiv.innerHTML = "";

    for (let i = 0; i < dealerData.length; i++) {
        await sleep(500);
        showCard(dealerData[i], "dealer-hand");
    }
}


function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

function showCard(card, containerId) {
    const suits = ["Clubs", "Diamonds", "Hearts", "Spades"];
    const ranks = ["A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"];

    const div = document.createElement("div");
    div.className = "card";

    if (card === "hidden-card") {
        div.style.backgroundPosition = "-938px -2px";
    } else {
        const col = ranks.indexOf(card.rank);
        const row = suits.indexOf(card.suit);

        const x = col * 72 + 2;
        const y = row * 96 + 2;

        div.style.backgroundPosition = `-${x}px -${y}px`;
    }

    document.getElementById(containerId).appendChild(div);
}

function toggleRules() {
    const content = document.getElementById("rulesContent");
    const btn = document.getElementById("toggleRulesBtn");

    if (content.classList.contains("rules-hidden")) {
        content.classList.remove("rules-hidden");
        content.classList.add("rules-visible");
        btn.innerText = "📘 Hide Rules";
    } else {
        content.classList.remove("rules-visible");
        content.classList.add("rules-hidden");
        btn.innerText = "📘 Show Rules";
    }
}