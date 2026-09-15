# L18 – Web Storage og Cookies

## Metadata

- **Lektion:** L18 – Web Storage and Cookies
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L18/FED WebStorageAndCookies.pdf (13 slides)
- **Emner dækket:**
  - Hvorfor client-side storage: HTTP er stateless
  - Hvad cookies er, og hvad de bruges til
  - EU-lovgivning om cookies (ePrivacy-direktivet, Artikel 5(3))
  - Cookie-typer efter levetid og domæne
  - Begrænsninger for cookies
  - Cookies fra JavaScript via `document.cookie`
  - HTML client-side storage: Web Storage, Web SQL, Indexed Database
  - `localStorage` vs. `sessionStorage`
  - Serialisering med `JSON.stringify()` / `JSON.parse()`

---

## 1. Hvorfor client-side storage?

HTTP er en **stateless** protokol. En web server behandler hver HTTP-request som en uafhængig request og bevarer ikke brugerværdier fra tidligere requests.

Men du kan bruge cookies eller local storage på klienten til at bevare application state og session state mellem requests. Man kan altså gemme data på klienten — endda mellem sessioner.

## 2. Hvad er cookies?

Cookies er stumper af data, normalt gemt i tekstfiler, som websites placerer på besøgendes computere for at gemme en række informationer, som regel specifikke for den besøgende.

- Cookies gør det muligt at logge ind på én side, derefter bevæge sig rundt til andre sider og forblive logget ind.
- De gør det muligt at sætte præferencer for visningen af en side, og at disse huskes næste gang du vender tilbage.
- Cookies kan også bruges til at følge de sider, du besøger på tværs af sites, hvilket lader annoncører opbygge et billede af dine interesser. Det kaldes **behavioural advertising**.

Cookies er utroligt nyttige. Men de kan også bruges til at manipulere din weboplevelse på måder, du måske ikke forventer eller bryder dig om.

## 3. EU-lovgivning om cookies

ePrivacy-direktivet — nærmere bestemt Artikel 5(3) — kræver **prior informed consent** til lagring af, eller adgang til, information gemt på en brugers terminaludstyr. Med andre ord: du skal spørge brugerne, om de accepterer de fleste cookies og lignende teknologier.

Samtykke er **ikke** påkrævet, hvis cookien er:

- **user-input cookies** (session-id), fx first-party cookies til at holde styr på brugerens input ved udfyldelse af online formularer, indkøbskurve osv., for varigheden af en session — eller persistent cookies begrænset til nogle få timer i visse tilfælde
- **authentication cookies**, til at identificere brugeren når han er logget ind, for varigheden af en session
- **user-centric security cookies**, brugt til at detektere authentication-misbrug, for en begrænset persistent varighed
- **multimedia content player cookies**, brugt til at gemme tekniske data til afspilning af video- eller lydindhold, for varigheden af en session
- **load-balancing cookies**, for varigheden af en session
- **user-interface customisation cookies** som sprog- eller font-præferencer, for varigheden af en session (eller lidt længere)
- **third-party social plug-in content-sharing cookies**, for medlemmer der er logget ind på et socialt netværk

## 4. De forskellige typer cookies

**Efter levetid** er en cookie enten:

- en **session cookie**, som slettes når brugeren lukker browseren, eller
- en **persistent cookie**, som bliver på brugerens computer/enhed i en foruddefineret periode

**Efter det domæne den tilhører**, er der enten:

- **first-party cookies**, som sættes af web-serveren for den besøgte side og deler samme domæne
- **third-party cookies**, gemt af et andet domæne end den besøgte sides domæne

## 5. Cookies — begrænsninger

- Fordi cookies sendes med **hver** request, bør deres størrelse holdes på et minimum.
- Ideelt set bør kun en identifier gemmes i en cookie, mens de faktiske data gemmes på serveren.
- De fleste browsere begrænser cookies til **4096 bytes**.
- Kun et begrænset antal cookies er tilgængelige pr. domæne.
- Brugere kan når som helst rydde cookies på deres computer. Selv hvis du gemmer cookies med lange expiration times, kan en bruger beslutte at slette alle cookies og dermed udradere de indstillinger, du måtte have gemt.

## 6. Cookies fra JavaScript

JavaScript kan oprette, læse og slette cookies med `document.cookie`-propertyen.

### Kodeeksempel

```javascript
document.cookie = "username=John Doe";
```

Cookien slettes, når browseren lukkes, medmindre du tilføjer en expiry date i UTC-tid:

```javascript
document.cookie = "username=John Doe; expires=Thu, 18 Dec 2028 12:00:00 UTC";
```

Med en `path`-parameter kan du fortælle browseren, hvilken path cookien tilhører. Som default tilhører cookien den aktuelle side:

```javascript
document.cookie = "username=John Doe; expires=Thu, 18 Dec 2013 12:00:00 UTC; path=/";
```

## 7. En funktion til at hente en cookie

Bemærk at `document.cookie` returnerer alle cookies som én semikolon-separeret streng — derfor skal man selv splitte og trimme.

### Kodeeksempel

```javascript
function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for(var i = 0; i <ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
```

## 8. HTML Client-Side Storage

Slidesene viser en oversigtstabel (fra http://www.html5rocks.com/it/features/storage) over browser-understøttelse — begge de viste kolonner er markeret med "9+".

De tre teknologier:

- **Web Storage** giver simpelthen en key-value mapping, fx `localStorage["name"] = username;`
- **Web SQL Database** giver dig al kraften fra en struktureret SQL relationel database
- **Indexed Database** ligger et sted mellem Web Storage og Web SQL Database. Ligesom Web Storage er det en ligefrem key-value mapping, men den understøtter indexes, så søgning efter objekter, der matcher et bestemt felt, er hurtig.

## 9. Web Storage

Web Storage understøtter persistent data storage — svarende til cookies, men med en langt større kapacitet (**5-10 MB per origin**) og **uden** at information gemmes i HTTP request headeren.

Der er to hovedtyper web storage:

**localStorage**

- Permanent storage (kan slettes af brugeren)
- Data placeret i local storage er per origin (protocol + hostname + port number)

**sessionStorage**

- Begrænset til vinduets levetid

Browsere, der understøtter web storage, har de globale variabler `sessionStorage` og `localStorage` deklareret på window-niveau.

Web storage er ved at blive standardiseret af World Wide Web Consortium.

## 10. Hvordan bruges det?

Web Storage understøtter kun **string-til-string** mappings, så du skal serialisere og de-serialisere andre datastrukturer selv.

Du kan bruge `JSON.stringify()` og `JSON.parse()` til at gemme andre datatyper end string.

### Kodeeksempel

```javascript
// Store an object/array using JSON
localStorage.numbers = JSON.stringify(numbers);
```

```javascript
if (localStorage.hasOwnProperty("numbers")) {
   // Read an object/array using JSON
   numbers = JSON.parse(localStorage.numbers);
}
```

## 11. References & Links

- Cookiepedia: https://cookiepedia.co.uk/all-about-cookies
- Lovgivning og vejledning til cookiebekendtgørelsen: https://erhvervsstyrelsen.dk/lovgivning-og-vejledning-til-cookiebekendtgoerelsen
- MDN localStorage property: https://developer.mozilla.org/en-US/docs/Web/API/Window/localStorage
