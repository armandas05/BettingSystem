const password = document.getElementById("password");
const strengthBar = document.getElementById("strength");
const btn = document.getElementById("registerBtn");
const loginBtn = document.getElementById("loginBtn");

password.addEventListener("input", () => {
    const val = password.value;
    let strength = 0;

    if (val.length > 5) strength++;
    if (/[A-Z]/.test(val)) strength++;
    if (/[0-9]/.test(val)) strength++;
    if (/[^A-Za-z0-9]/.test(val)) strength++;

    const colors = ["red", "orange", "yellow", "limegreen"];
    strengthBar.style.width = (strength * 25) + "%";
    strengthBar.style.background = colors[strength - 1] || "transparent";
});

btn.onclick = async () => {
    btn.classList.add("loading");

    const user = {
        firstName: firstName.value,
        lastName: lastName.value,
        email: email.value,
        password: password.value
    };

    const res = await fetch('/api/auth/register', {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(user)
    });

    const result = document.getElementById("register-result");

    if (res.ok) {
        result.style.color = "#00e676";
        result.innerText = "Success!";
    } else {
        result.style.color = "#ff5252";
        result.innerText = await res.text();
    }

    btn.classList.remove("loading");
};