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

    html += `<button onclick="changePage(${totalPages}, ${pageType})">
                Last
            </button>`;


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
        let role = u.role === 0 ? "User" : "Admin";

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
        let result = h.result === 0 ? "Win"
            : h.result === 1 ? "Lose"
                : h.result === 2 ? "Draw"
                    : "Unknown";


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

                