(async function () {
  const response = await fetch("/.auth/me");
  const data = await response.json();

  data.clientPrincipal === null ? DrawLogin() : DrawLogout();
})();

function DrawLogin() {
  let div = document.querySelector("#root");
  div.innerHTML = `<div class="login">
                    <h2 class="login-header">Logga smidigt in med:</h2>
                  <div>
                    <a class="login-btn" href="/login-github">Github</a>
                  </div>
              </div>`;
}

function DrawLogout() {
  let div = document.querySelector("#root");
  div.innerHTML = `<div class="login">
                    <h2 class="login-header">Du är redan inloggad</h2>
                  <div>
                    <a class="login-btn" href="/min-sida">Min sida</a>
                    <a class="login-btn" href="/logout">Logga ut</a>
                  </div>
              </div>`;
}