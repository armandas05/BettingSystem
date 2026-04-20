let gamesChart, winLoseChart, gamesPerDayChart = null;


async function loadPage(page) {
    let url = `/Admin/${page}`;

    const response = await fetch(url);

    if (!response.ok) {
        document.getElementById("content").innerHTML = "Error loading information!";
        return;
    }

    const html = await response.text();

    document.getElementById("content").innerHTML = html;

    afterLoadPage(url);
}

function afterLoadPage(url) {

    if (url.includes("Users")) {
        loadUsers();
        return;
    }

    if (url.includes("Payments")) {
        loadPayments();
        return;
    }

    if (url.includes("GameHistory")) {
        loadGameHistories();
        return;
    }

    if (url.includes("Analytics")) {
        loadAnalytics();
        return;
    }

}

let currentUserPage = 1;
let currentGameHistoryPage = 1;
let currentPaymentsPage = 1;
let pageSize = 10;

async function loadUsers(page = 1) {
    currentUserPage = page;

    const response = await fetch(`/api/admin/users?page=${page}&pageSize=${pageSize}&sortBy=${currentUserSort.sortBy}&sortDir=${currentUserSort.sortDir}`);

    if (!response.ok) {
        console.error("Failed to load users!");
        return;
    }

    const result = await response.json();

    renderUsers(result.data);
    renderPagination(result.totalCount, 1);
    renderSearch(1);

}


async function loadGameHistories(page = 1) {
    currentGameHistoryPage = page;

    const response = await fetch(`/api/admin/gamehistories?page=${page}&pageSize=${pageSize}&sortBy=${currentGameHistorySort.sortBy}&sortDir=${currentGameHistorySort.sortDir}`);

    if (!response.ok) {
        console.error("Failed to load game histories!");
        return;
    }

    const result = await response.json();

    renderGameHistories(result.data);
    renderPagination(result.totalCount, 2);
    renderSearch(2);

}

async function loadPayments(page = 1) {
    currentPaymentsPage = page;

    const response = await fetch(`/api/admin/transactions?page=${page}&pageSize=${pageSize}&sortBy=${currentPaymentSort.sortBy}&sortDir=${currentPaymentSort.sortDir}`);

    if (!response.ok) {
        console.error("Failed to load payments!");
        return;
    }

    const result = await response.json();

    renderPayments(result.data);
    console.log(result.data);
    renderPagination(result.totalCount, 3);
    renderSearch(3);


}

let currentUserSort = {
    sortBy: "userid",
    sortDir: "asc"
};

let currentGameHistorySort = {
    sortBy: "gamesessionid",
    sortDir: "asc"
};

let currentPaymentSort = {
    sortBy: "transactionid",
    sortDir: "asc"
}

function renderSearch(pageType) {
    let html = "";
    if (pageType === 1) {
        html = `
        <label for="searchInput">Search user:</label>
            <input type="text" id="searchInput" />
            <button onclick="searchUser()" id="searchBtn">Search</button>
        `;
    }
    else if (pageType === 2) {
        html = `
        <label for="searchInput">Search user:</label>
            <input type="text" id="searchInput" />
            <button onclick="searchGameHistoryUser()" id="searchBtn">Search</button>
        `;
    }
    else if (pageType === 3) {
        html = `
        <label for="searchInput">Search user:</label>
            <input type="text" id="searchInput" />
            <button onclick="searchTransactionUser()" id="searchBtn">Search</button>
        `;
    }

    document.getElementById("searchBox").innerHTML = html;
}

async function searchUser() {
    const input = document.getElementById("searchInput").value;


    const response = await fetch(`/api/admin/users?searchInput=${input}&page=1&pageSize=10`);

    if (!response.ok) {
        console.error("Failed to load users!");
        return;
    }

    const result = await response.json();

    renderUsers(result.data);
    renderPagination(result.totalCount, 1);
    renderSearch(1);
}

async function searchGameHistoryUser() {
    const input = document.getElementById("searchInput").value;


    const response = await fetch(`/api/admin/gamehistories?searchInput=${input}&page=1&pageSize=10`);

    if (!response.ok) {
        console.error("Failed to load users!");
        return;
    }

    const result = await response.json();

    renderGameHistories(result.data);
    renderPagination(result.totalCount, 2);
    renderSearch(2);
}

async function searchTransactionUser() {
    const input = document.getElementById("searchInput").value;


    const response = await fetch(`/api/admin/transactions?searchInput=${input}&page=1&pageSize=10`);

    if (!response.ok) {
        console.error("Failed to load transactions!");
        return;
    }

    const result = await response.json();

    renderPayments(result.data);
    renderPagination(result.totalCount, 3);
    renderSearch(3);
}



function renderPagination(totalCount, pageType) {
    const totalPages = Math.ceil(totalCount / pageSize);

    switch (pageType) {
        case 1: currentPage = currentUserPage;
            break;
        case 2: currentPage = currentGameHistoryPage;
            break;
        case 3: currentPage = currentPaymentsPage;
            break;
    }

    let html = "";

    html += `<button onclick="changePage(${currentPage - 1}, ${pageType})"
                ${currentPage === 1 ? "disabled" : ""}>
                Prev
             </button>`;

    html += `<span style="margin:5px;">Page ${currentPage} / ${totalPages}</span>`;

    html += `<button onclick="changePage(${currentPage + 1}, ${pageType})" 
                ${currentPage === totalPages ? "disabled" : ""}>
                Next
            </button>`;

    if (totalPages > 1 && currentPage === 1 || currentPage < totalPages) {
        html += `<button onclick="changePage(${totalPages}, ${pageType})">
                    Last
                </button>`;
    } else if (totalPages > 1 && currentPage === totalPages ) {
        html += `<button onclick="changePage(1, ${pageType})">
                    First
                </button>`;
    }


    document.getElementById("pagination").innerHTML = html;
}

function changePage(page, pageType) {
    if (pageType === 1) {
        loadUsers(page);
    }
    else if (pageType === 2)
    {
        loadGameHistories(page);
    }
    else if (pageType === 3)
    {
        loadPayments(page);
    }
}

function sortTable(field, pageType) {
    let newDir = "asc";

    if (pageType === 1)
    {

        if (currentUserSort.sortBy === field && currentUserSort.sortDir === "asc") {
            newDir = "desc";
        }

        currentUserSort = {
            sortBy: field,
            sortDir: newDir
        };

        loadUsers(1);
        return;
    }
    else if (pageType === 2)
    {
        if (currentGameHistorySort.sortBy === field && currentGameHistorySort.sortDir === "asc") {
            newDir = "desc";
        }

        currentGameHistorySort = {
            sortBy: field,
            sortDir: newDir
        };

        loadGameHistories(1);
        return;
    }
    else if (pageType === 3)
    {
        if (currentPaymentSort.sortBy === field && currentPaymentSort.sortDir === "asc") {
            newDir = "desc";
        }

        currentPaymentSort = {
            sortBy: field,
            sortDir: newDir
        };

        loadPayments(1);
        return;
    }
}

function getSelectedUserId() {
    const checkboxes = document.querySelectorAll(".row-checkbox");

    let selected = [];

    checkboxes.forEach(cb => {
        if (cb.checked) {
            selected.push(cb.dataset.userid);
        }
    });

    return selected;
}

function renderButtons() {
    const selected = getSelectedUserId();
    if (selected.length === 0) {
        document.getElementById("userButtons").innerHTML = "";
        return;
    }

    let html = `
        <button onclick="viewUser()">View</button>
        <button onclick="deleteUser()">Delete</button>
    `;

    document.getElementById("userButtons").innerHTML = html;
}


function viewUser() {
    const selected = getSelectedUserId();

    if (selected.length != 1) {
        alert("Select one user!");
        return;
    }
    const userId = selected[0];
}

async function deleteUser() {
    const selected = getSelectedUserId();

    if (selected.length != 1) {
        alert("Select one user!");
        return;
    }

    let value = confirm(`Please press "Confirm" to delete User: ${selected[0]}`);
    let userId = selected[0];

    if (value) {
        const response = await fetch(`/api/admin/users/${userId}`, {
            method: "DELETE"
        });

        if (!response.ok) {
            console.error("Failed to delete user");
            return;
        }
        console.log("User deleted");

        loadUsers(currentUserPage);

    } else {
        console.log("cancelled!");
        return;
    }


}

function renderUsers(users) {
    let html = `
    <table>
        <thead>
            <tr>
                <th></th>
                <th onclick="sortTable('userid', 1)">User ID</th>
                <th onclick="sortTable('firstname', 1)">First name</th>
                <th onclick="sortTable('lastname', 1)">Last name</th>
                <th onclick="sortTable('email', 1)">Email</th>
                <th onclick="sortTable('balance', 1)">Balance</th>
                <th>Date Created</th>
                <th>Verified</th>
                <th>Role</th>
                <th>Games Played</th>
                <th>Deposited</th>
            </tr>
        </thead>
        <tbody>
    `;

    users.forEach(u => {
        let role = u.role === 0
            ? `<span class="badge user">User</span>`
            : `<span class="badge admin">Admin</span>`;

        html += `
            <tr>
                <td><input type="checkbox" class="row-checkbox" onclick="renderButtons()" data-userid="${u.userID}"></th>
                <td>${u.userID}</td>
                <td>${u.firstName}</td>
                <td>${u.lastName}</td>
                <td>${u.email}</td>
                <td>${u.balance}</td>
                <td>${u.dateCreated}</td>
                <td>${u.isVerified}</td>
                <td>${role}</td>
                <td>${u.gamesPlayed}</td>
                <td>${u.totalDeposited}</td>
            </tr>
        `;
    });

    html += `
        </tbody>
    </table>
    `;

    document.getElementById("usersTable").innerHTML = html;
}

function renderGameHistories(histories) {
    let html = `
    <table>
        <thead>
            <tr>
                <th onclick="sortTable('gamesessionid', 2)">Game Session ID</th>
                <th onclick="sortTable('userid', 2)">User ID</th>
                <th onclick="sortTable('datetime', 2)">Date/Time</th>
                <th onclick="sortTable('betamount', 2)">Bet Amount</th>
                <th onclick="sortTable('amountwon', 2)">Amount Won</th>
                <th onclick="sortTable('result', 2)">Result</th>
                <th onclick="sortTable('gameid', 2)">Game</th>
            </tr>
        </thead>
        <tbody>
    `;

    histories.forEach(h => {
        let game = h.gameID === 1 ? "Blackjack" : "Dices";
        let result = h.result === 0
            ? `<span class="badge win">Win</span>`
            : h.result === 1
                ? `<span class="badge lose">Lose</span>`
                : `<span class="badge draw">Draw</span>`;


        html += `
            <tr>
                <td>${h.gameSessionID}</td>
                <td>${h.userID}</td>
                <td>${h.dateTime}</td>
                <td>${h.betAmount}</td>
                <td>${h.amountWon}</td>
                <td>${result}</td>
                <td>${game}</td>
            </tr>
        `;
    });

    html += `
        </tbody>
    </table>
    `;

    document.getElementById("gamesTable").innerHTML = html;
}


function renderPayments(transactions) {
    let html = `
    <table>
        <thead>
            <tr>
                <th onclick="sortTable('transactionid', 3)">Transaction ID</th>
                <th onclick="sortTable('userid', 3)">User ID</th>
                <th onclick="sortTable('depositamount', 3)">Deposit Amount</th>
                <th onclick="sortTable('method', 3)">Deposit Method</th>
                <th onclick="sortTable('datedeposited', 3)">Date/Time</th>
            </tr>
        </thead>
        <tbody>
    `;

    transactions.forEach(t => {
        let method;
        switch (t.method) {
            case 0: method = "BTC";
                break;
            case 1: method = "ETH";
                break;
            case 2: method = "Bank";
                break;
            case 3: method = "Card";
                break;
            case 4: method = "Google Pay";
                break;
            case 5: method = "Apple Pay";
                break;
        }

        html += `
            <tr>
                <td>${t.transactionID}</td>
                <td>${t.userID}</td>
                <td>${t.depositAmount}</td>
                <td>${method}</td>
                <td>${t.dateDeposited}</td>
            </tr>
        `;
    });

    html += `
        </tbody>
    </table>
    `;

    document.getElementById("paymentsTable").innerHTML = html;
}

function loadAnalytics() {

    let selected = "7";

    const filter = document.getElementById("timeFilter");
    if (filter) selected = filter.value;

    renderAnalytics();

    document.getElementById("timeFilter").value = selected;

    loadAnalyticsData();
}

function renderAnalytics() {

    let html = `
    <div class="analytics-filter">
        <select id="timeFilter" onchange="loadAnalytics()">
            <option value="7">Last 7 days</option>
            <option value="30">Last 30 days</option>
            <option value="0">All time</option>
        </select>
    </div>

    <!-- KPI -->
    <div class="analytics-kpi">

        <div class="kpi-card glow-green">
            <h6>Total Games</h6>
            <h3 id="totalGames">0</h3>
        </div>

        <div class="kpi-card glow-blue">
            <h6>Total Bet</h6>
            <h3 id="totalBet">0</h3>
        </div>

        <div class="kpi-card glow-purple">
            <h6>Total Users</h6>
            <h3 id="totalUsers">0</h3>
        </div>

        <div class="kpi-card glow-orange">
            <h6>Total Wins</h6>
            <h3 id="totalWins">0</h3>
        </div>

    </div>

    <!-- CHART GRID -->
    <div class="charts-grid">

        <div class="chart-card pie">
            <h5>🎮 Most Played</h5>
            <canvas id="gamesChart"></canvas>
        </div>

        <div class="chart-card">
            <h5>📉 Win vs Lose</h5>

            <div class="chart-wrapper">
                <canvas id="winLoseChart"></canvas>
            </div>
        </div>

        <div class="chart-card full">
            <h5>📅 Games per Day</h5>

            <div class="chart-wrapper">
                <canvas id="gamesPerDayChart"></canvas>
            </div>
        </div>

    </div>
    `;

    document.getElementById("analytics").innerHTML = html;
}

async function loadAnalyticsData() {

    const days = document.getElementById("timeFilter").value;

    const response = await fetch(`/api/admin/stats?days=${days}`);

    if (!response.ok) return console.error("Failed to load stats!");

    const data = await response.json();

    animateValue("totalGames", data.totalGames);
    animateMoney("totalBet", data.totalBetAmount);
    animateValue("totalUsers", data.totalUsers);
    animateValue("totalWins", data.totalWins);

    renderGamesChart(data.playedGames);
    renderWinLoseChart(data.winLoseStats);
    renderGamesPerDayChart(data.gamesPerDayStats);

}

function renderGamesChart(games) {

    if (gamesChart) {
        gamesChart.destroy();
    }

    if (!games || games.length === 0) return;

    const labels = games.map(g => g.gameName);
    const values = games.map(g => g.count);

    const ctx = document.getElementById("gamesChart").getContext("2d");

    const gradient1 = ctx.createLinearGradient(0, 0, 0, 300);
    gradient1.addColorStop(0, "#00c2ff");
    gradient1.addColorStop(1, "#007bff");

    const gradient2 = ctx.createLinearGradient(0, 0, 0, 300);
    gradient2.addColorStop(0, "#ff6384");
    gradient2.addColorStop(1, "#ff2e63");

    gamesChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: labels,
            datasets: [{
                data: values,
                backgroundColor: [gradient1, gradient2],
                borderWidth: 0
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            cutout: "60%"
        }
    });
}

function renderWinLoseChart(stats) {

    if (winLoseChart) {
        winLoseChart.destroy();
    }

    if (!stats || stats.length === 0) return;

    const labels = stats.map(s =>
        s.result === 0 ? "Win" :
            s.result === 1 ? "Lose" :
                "Draw"
    );

    const values = stats.map(s => s.count);

    const ctx = document.getElementById("winLoseChart").getContext("2d");

    const max = Math.max(...values);

    winLoseChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: "Results",
                data: values,
                backgroundColor: ["#ff5252", "#00e676", "#ffc107"],
                borderRadius: 8
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    beginAtZero: true,
                    suggestedMax: max * 1.2,
                    grid: {
                        color: "rgba(255,255,255,0.05)"
                    }
                },
                x: {
                    grid: {
                        color: "rgba(255,255,255,0.05)"
                    }
                }
            }
        }
    });
}

function renderGamesPerDayChart(days) {

    if (gamesPerDayChart) {
        gamesPerDayChart.destroy();
    }

    const ctx = document.getElementById("gamesPerDayChart").getContext("2d");

    const lineGradient = ctx.createLinearGradient(0, 0, 0, 400);
    lineGradient.addColorStop(0, "rgba(0, 200, 255, 0.5)");
    lineGradient.addColorStop(1, "rgba(0, 200, 255, 0)");

    const dayMap = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];
    const fullWeek = Array(7).fill(0);

    days.forEach(d => {
        const index = (d.weekDay + 6) % 7;
        fullWeek[index] = d.count;
    });

    const max = Math.max(...fullWeek);

    gamesPerDayChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: dayMap,
            datasets: [{
                label: "Games per day",
                data: fullWeek,
                borderColor: "#00c2ff",
                backgroundColor: lineGradient,
                fill: true,
                tension: 0.4,
                pointRadius: 5,
                pointHoverRadius: 7
            }]
        },

        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    beginAtZero: true,
                    suggestedMax: max * 1.1
                }
            }
        }
    });
}


function animateValue(id, end, duration = 800) {
    const el = document.getElementById(id);
    let start = 0;
    const range = end - start;
    const startTime = performance.now();

    function update(currentTime) {
        const progress = Math.min((currentTime - startTime) / duration, 1);
        const value = Math.floor(progress * range + start);

        el.innerText = value.toLocaleString();

        if (progress < 1) {
            requestAnimationFrame(update);
        }
    }

    requestAnimationFrame(update);
}

function animateMoney(id, end, duration = 800) {
    const el = document.getElementById(id);
    let start = 0;
    const startTime = performance.now();

    function update(currentTime) {
        const progress = Math.min((currentTime - startTime) / duration, 1);
        const value = (progress * end);

        el.innerText = value.toFixed(2);

        if (progress < 1) {
            requestAnimationFrame(update);
        }
    }

    requestAnimationFrame(update);
}

                