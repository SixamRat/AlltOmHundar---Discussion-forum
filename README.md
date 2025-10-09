DETTA ÄR ETT INLÄMNINGSPROJEKT 
KRAV
◦ MVC, eller Blazor
◦ Minst en del av applikationen ska funktionellt vara frikopplad från huvudprojektet (helt eget
projekt), och exponeras ut som ett API
◦ Publicerat på Azure
◦ All kod ska följa Dependency Injection-mönstret, och med service och repository-lager
◦ Bootstrap-fint (men med egen CSS också)
◦ Fler krav kan komma (agilt)

Följande funktioner ska finnas:
◦ En admin ska kunna skapa Rubrik på forumet och övergripande ämnen, såsom
◦ Fordonssnack
◦ Bilsnack, Båtsnack, Cykelsnack
◦ Vem som helst ska kunna registrera sig, och kan därefter skapa ett eget inlägg, under en av de
övergripande ämnena.
◦ Användaren ska också kunna ladda upp en bild på sig själv, som syns vid sidan om inlägget.
◦ Vem som helst ska kunna kommentera inläggen, och allt sker I en enda lång tråd.
◦ Varje inlägg ska markeras med Datum och namn på författaren.
◦ Det ska också gå att skicka privata meddelanden till enskilda personer
◦ Det ska också finnas en anmälanfunktion, för stötande inlägg, som en admin sedan ska kunna
granska I en lista.
◦ Det ska gå att hämta alla inlägg för en diskussion, med författare och datum, via API.

Inlämningsuppgift – Snackis - VG

◦ Förutom G-delen:
◦ Inläggen ska ordnas I en trädliknande struktur, där man kan kommentera enskilda inlägg, som
blir en sub-diskussion.
◦ Alternativt så ska ett citat från det tidigare inlägget synas.
◦ Det ska gå att lägga in bilder I inlägget.
◦ Det ska gå att skapa gruppmeddelanden, som bara når de som ingår I Gruppen
◦ Inbjudningsfunktion för Gruppen, samt möjlighet för den som skapade Gruppen att kunna radera
personer
◦ Varje registrerad användare ska också kunna ha en enkel info-sida om sig själv.
◦ Det ska gå att ge inlägget t ex tummen-upp, hjärta m m, och det ska gå att se hur många som
t ex gillat inlägget.
◦ Fula ord-filter. Om inlägget innehåller opassande ord, så ska dessa ersättas med *********

DISCLAIMER! Jag använde POMPAs hemsida som inspiration när jag skulle göra CSS.



https://alltomhundar-b0ctaxe7d6e5hegv.swedencentral-01.azurewebsites.net
