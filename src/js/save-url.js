document.getElementById("saveButton").addEventListener("click", async function(e){
    e.preventDefault();
    console.log("Du kom in i din funktion, bitch");
    let nameResponse = await fetch("/.auth/me");
    let name = await nameResponse.json();
    let longUrl = document.getElementById("longUrl").value;
    const partialTableData = { LongUrl: longUrl,
                   CreatedBy: name.clientPrincipal.userDetails};
    const response = await fetch('/api/SaveUrl', {
      method: 'POST',
      body: JSON.stringify(partialTableData)
    });
    const responseText = await response.text();
    console.log(responseText); // logs 'OK'
    document.getElementById("longUrl").value = '';
});