# Urlshortener: projekt

- Leili Mänder
- Anna-Maria Phalén
- Niclas Selvad

## Klient
swa- vanilla html - js

Första-sidan ska innehålla en login-knapp om en inte är inloggad


När en är inloggad ska en kunna göra följande:

- Lägga till en url.

- Sök på url för att se statistik med typ datum och antal klick.



## Funktionalitet 

När en går till vår endpoint så redirectas en till riktiga urlen. via HTTPTrigger

## Persistensmedium
En table ska bestå av följande kolumner: created (date), nr of clicks, indexerade columner: url, endpoint (id)


## Loginet
- github -> visa vilket namn det är.


Felhantering: Se till att inte samma url kan läggas till två gånger.
          Se till att appen inte går sönder om en lägger till en url som inte funkar.