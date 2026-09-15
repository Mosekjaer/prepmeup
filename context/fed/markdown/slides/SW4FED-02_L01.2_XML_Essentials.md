# L01.2 – XML Essentials

## Metadata

- **Lektion:** L01 – XML Essentials
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** L01/FED XML Essentials.pdf (7 slides)
- **Emner dækket:**
  - Hvad XML er, og hvad det bruges til
  - Well-formed documents og XML-syntaks
  - Rod-element og XML-deklaration
  - Kommentarer
  - Grundsyntaks for elementer og attributter
  - Særlig syntaks for tomt indhold
  - Korrekt nesting — hvad der gør et dokument *ikke* well-formed

---

## 1. Hvad er XML?

XML (Extensible Markup Language) er en general-purpose specifikation til at lave custom markup languages. Sproget kaldes extensible, fordi det tillader brugerne at definere deres egne elementer.

Det primære formål er at lette deling af struktureret data mellem forskellige informationssystemer. XML bruges både til at encode dokumenter og til at serialisere data.

XML er et generisk framework til at gemme vilkårlige mængder tekst eller vilkårlige data, hvis struktur kan repræsenteres som et træ. XML er en W3C Recommendation.

## 2. Well-formed documents: XML-syntaks

Et XML-dokument har præcis ét rod-element (root element). Det betyder, at teksten skal være omsluttet af et root start-tag og et tilsvarende end-tag.

Følgende er et well-formed XML-dokument:

```xml
<book>This is a book.... </book>
```

Kommentarer kan placeres hvor som helst uden for et tag:

```xml
<!-- This is a comment. -->
```

Rod-elementet kan gå forud af en valgfri XML-deklaration:

```xml
<?xml version="1.0" encoding="UTF-8"?>
```

## 3. Grundsyntaksen

Grundsyntaksen for ét element ser sådan ud:

```xml
<name attribute="value">Content</name>
```

`<name ...>` er start-tag, `</name>` er end-tag.

Reglerne omkring dette: hvert attribute-navn må kun optræde én gang i et element. Attributværdier skal altid være i anførselstegn — enkelte eller dobbelte. Indholdet er tekst, som igen kan indeholde XML-elementer. Et generisk XML-dokument indeholder derfor en træbaseret datastruktur.

## 4. Særlig syntaks for tomt indhold

XML har en særlig syntaks til at repræsentere et element med tomt indhold. De følgende tre eksempler er ækvivalente i XML:

```xml
<foo></foo>
<foo />
<foo/>
```

Et empty-element må gerne indeholde attributter:

```xml
<info author="John Smith"
      genre="science-fiction"
      date="2109-Jan-01" />
```

## 5. Ikke well-formed XML

XML kræver, at elementer er korrekt nestede — elementer må aldrig overlappe hinanden.

```xml
<!-- WRONG! NOT WELL-FORMED XML! -->
<title>Book on Logic<author>Aristotle<title>Another Book on Logic
<author>Boole</title></author></title></author>

<!-- Correct: Well-formed XML. -->
<books>
  <book>
    <title>Book on Logic</title>
    <author>Aristotle</author>
  </book>
<book>
<title>Another Book on Logic <author>Boole</author> </title>
</book>
</books>
```

## 6. References & Links

- XML overview: http://en.wikipedia.org/wiki/XML
- XML definition: http://www.w3.org/XML/Core/#Publications
