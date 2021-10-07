getStats();

async function getStats() {
  let nameResponse = await fetch("/.auth/me");
  let name = await nameResponse.json();

  let response = await fetch(
    `/api/getUrlStats?userName=${name.clientPrincipal.userDetails}`
  );
  if (response.status === 200) {
    let data = await response.json();
    displayStats(await data);
  } else if (response.status === 404) {
    let root = document.querySelector("#user-urls");
    root.innerHTML = "";
    let headingCont = document.createElement("div");
    headingCont.classList.add("container-heading");

    let heading = document.createElement("h2");
    heading.classList.add("user-urls-heading");
    heading.textContent = "Inga URL:er att visa";
    headingCont.appendChild(heading);

    let refr = createRefreshButton();

    headingCont.appendChild(refr);
    root.appendChild(headingCont);
  }
}

function createRefreshButton(){
  let refr = document.createElement("span");
  refr.textContent = "↻";
  refr.classList.add("refr-btn");
  refr.addEventListener("click", getStats);
  refr.title = "refresh";

  return refr;
}

function displayStats(data) {
  let root = document.querySelector("#user-urls");
  root.innerHTML = "";
  let headingCont = document.createElement("div");
  headingCont.classList.add("container-heading");

  let heading = document.createElement("h2");
  heading.classList.add("user-urls-heading");
  heading.textContent = "Dina Skapade URL:er";

  let refr = createRefreshButton();

  headingCont.appendChild(heading);
  headingCont.appendChild(refr);
  root.appendChild(headingCont);

  let urls = document.createElement("div");
  urls.classList.add("url-list-container");

  let dateOptions = {
    weekday: "long",
    year: "numeric",
    month: "long",
    day: "numeric",
  };

  for (let i = 0; i < data.length; i++) {
    let div = document.createElement("div");
    let cardHeading = document.createElement("div");
    div.classList.add("card");
    cardHeading.classList.add("card-heading");

    let heading = document.createElement("h3");
    heading.textContent = data[i].longUrl;
    cardHeading.appendChild(heading);

    div.appendChild(cardHeading);
    let list = document.createElement("ul");
    list.classList.add("url-list");
    let shortUrl = document.createElement("li");
    let clicks = document.createElement("li");
    let date = document.createElement("li");

    //FIXME: only works in DEVELOPMENT. --PORT specified
    const host = window.location.hostname;
    shortUrl.textContent = `Fökortad Url: `;
    let link = document.createElement("a");
    const url = `http://${host}:4280/${data[i].id}`;
    link.href = url;
    link.textContent = url;

    link.target = "_blank";
    shortUrl.appendChild(link);

    clicks.textContent = `Antal klick: ${data[i].timesClicked}`;
    let createdDate = new Date(data[i].createdAt);

    date.textContent = `Skapades: ${createdDate.toLocaleDateString(
      "sv-SV",
      dateOptions
    )}`;

    list.appendChild(shortUrl);
    list.appendChild(clicks);
    list.appendChild(date);
    div.appendChild(list);

    urls.appendChild(div);
  }

  root.appendChild(urls);
}