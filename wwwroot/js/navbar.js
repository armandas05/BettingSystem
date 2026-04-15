async function loadBalance() {
    const balanceElement = document.getElementById("userBalance");

    if (!balanceElement) return;

    try {
        const response = await fetch('/api/user/balance');

        if (!response.ok) return;

        const balance = await response.json();

        balanceElement.innerText = `Balance: ${balance}`;
    } catch (e) {
        console.error("Failed to load balance");
    }

}

async function deposit() {
    const amount = document.getElementById("depositAmount").value;
    const method = document.getElementById("depositMethod").value;

    const response = await fetch("/api/transaction/deposit", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            depositAmount: parseFloat(amount),
            method: parseInt(method)
        })
    });

    if (!response.ok) {
        alert("Deposit failed!");
        return;
    }

    alert("Deposit successful!");

    loadBalance();

    const modal = bootstrap.Modal.getInstance(document.getElementById("depositModal"));
    modal.hide();


}

document.addEventListener("DOMContentLoaded", loadBalance);