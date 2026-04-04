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

    }

}

let currentPage = 1;
let pageSize = 10;

async function loadUsers(page = 1) {
    currentPage = page;

    const response = await fetch(`/api/admin/users?page=${page}&pageSize=${pageSize}&sortBy=${currentSort.sortBy}&sortDir=${currentSort.sortDir}`);

    if (!response.ok) {
        console.error("Failed to load users!");
        return;
    }

    const result = await response.json();

    renderUsers(result.data);
    renderPagination(result.totalCount);
    renderSearch();
    renderButtons();
}

let currentSort = {
    sortBy: "userid",
    sortDir: "asc"
};

function renderSearch() {
    let html = "";

    html = `
    <label for="searchInput">Search user:</label>
        <input type="text" id="searchInput" />
        <button onclick="searchUser()" id="searchBtn">Search</button>
    `;
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
    renderPagination(result.totalCount);
    renderSearch();
}



function renderPagination(totalCount) {
    const totalPages = Math.ceil(totalCount / pageSize);

    let html = "";

    for (let i = 1; i <= totalPages; i++) {
        html += `<button onclick="loadUsers(${i})">${i}</button>`;
    }

    document.getElementById("pagination").innerHTML = html;
}

function sortTable(field) {
    let newDir = "asc";

    if (currentSort.sortBy === field && currentSort.sortDir === "asc") {
        newDir = "desc";
    }

    currentSort = {
        sortBy: field,
        sortDir: newDir
    };

    loadUsers(1);
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
    let html = "";

    html = `
        <button onclick="viewUser()">View</button>
        <button onclick="blockUser()">Block</button>
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
    console.log(userId);

}

function blockUser() {
    //prideti bool Blocked prie User table kad galetume prideti blockUser funckija;;;
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

        loadUsers(currentPage);

    } else {
        // canceled pop up
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
                <th onclick="sortTable('userid')">User ID</th>
                <th onclick="sortTable('firstname')">First name</th>
                <th onclick="sortTable('lastname')">Last name</th>
                <th onclick="sortTable('email')">Email</th>
                <th onclick="sortTable('balance')">Balance</th>
                <th>Date Created</th>
                <th>Verified</th>
                <th>Role</th>
                <th>Games</th>
                <th>Deposited</th>
            </tr>
        </thead>
        <tbody>
    `;

    users.forEach(u => {
        html += `
            <tr>
                <td><input type="checkbox" class="row-checkbox" data-userid="${u.userID}"></th>
                <td>${u.userID}</td>
                <td>${u.firstName}</td>
                <td>${u.lastName}</td>
                <td>${u.email}</td>
                <td>${u.balance}</td>
                <td>${u.dateCreated}</td>
                <td>${u.isVerified}</td>
                <td>${u.role}</td>
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

                