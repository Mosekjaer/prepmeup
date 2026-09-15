# Lab 27 – Vitest

## Metadata

- **Lektion:** L27 – Test af React-komponenter
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** Brightspace-note "Lab 27 Vitest" (overskriften i kilden siger fejlagtigt "Lab 23 Vitest")
- **Emner dækket:**
  - Installation og konfiguration af Vitest
  - npm scripts til test og coverage
  - JSDOM som test environment
  - React Testing Library (RTL)
  - Smoke tests og indholdstests
  - `fireEvent` til simulering af brugerinteraktion
  - Snapshot tests
  - MSW til mocking af fetch-kald

---

## Opgaven

Tag udgangspunkt i lab 22.

1. Installer Vitest.
2. Opdater `package.json` med scripts til test og coverage.
3. Installer JSDOM.
4. Installer React Testing Library.
5. Konfigurer test environment og opdater Vites konfigurationsfil.
6. Lav en smoke test for en eller flere af komponenterne.
7. Lav en test, som tester at indholdet (noget tekst) er rigtigt.
8. Lav en test, som bruger RTL's `fireEvent` til at simulere brugerinteraktion.
9. Lav en snapshot test af en komponent.
10. Installer MSW og lav en mock af et fetch get-kald, samt en test af en komponent, som benytter dette kald.
