loginBtn.onclick = async () => {
    loginBtn.classList.add("loading");

    const loginRes = document.getElementById("login-result");

    const res = await fetch('/api/auth/login', {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            email: email.value,
            password: password.value
        })
    });

    if (res.ok) {
        window.location.href = "/";
    } else {
        loginBtn.classList.remove("loading");
        loginRes.innerText = await res.text();
    }
};