(async function () {
  const response = await fetch("/.auth/me");
  const data = await response.json();

  let heading = document.querySelector("#name");
  heading.innerText = data.clientPrincipal.userDetails;
})();