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

document.addEventListener("DOMContentLoaded", loadBalance);