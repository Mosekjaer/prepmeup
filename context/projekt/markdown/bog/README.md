# Bog — ISE-kompendiet "Introduction to System Engineering"

Kompendiet er 14 uddrag fra syv lærebøger. Kilden i [`../../kilder/ISE Book/`](../../kilder/ISE%20Book/) er en LaTeX-transskription (`chapters/*.tex`, figurer som TikZ).

Hvert kapitel findes i to udgaver:

- **Struktureret** (denne mappe): samme overskriftsstruktur som bogen, alle tabeller, definitioner, guidelines, tal og figurer (mermaid hvor entydigt, ellers tabel/prosa), sidehenvisninger. Den løbende prosa er sammenfattet, ikke ordret. Kap. 01–02 og 09–12 er tættere på fuld længde end resten.
- **Fuldtekst** ([`fuldtekst/`](fuldtekst/)): mekanisk `pandoc latex → gfm` af tex-filerne, ordret, uden redaktion. TikZ-figurer erstattet af placeholder. Brug denne når ordlyden betyder noget.

| # | Fil | Bog | Uddrag | Sprog |
|---|---|---|---|---|
| 01 | [01-larman-ch5-requirements.md](01-larman-ch5-requirements.md) | Larman, *Applying UML and Patterns*, 3. udg. | Kap. 5 Requirements, s. 54–57 | en |
| 02 | [02-larman-ch6-use-cases.md](02-larman-ch6-use-cases.md) | Larman | Kap. 6 Use Cases, s. 61–100 | en |
| 03 | [03-peckol-ch10-hardware-test-debug.md](03-peckol-ch10-hardware-test-debug.md) | Peckol, *Embedded Systems Design* | Kap. 10 Hardware Test and Debug, s. 401–407 | en |
| 04 | [04-spu-vejledning-softwaretest.md](04-spu-vejledning-softwaretest.md) | Biering-Sørensen m.fl., *Struktureret Program-Udvikling* (SPU) | Vejledning i softwaretest, s. 171–207 | da |
| 05 | [05-friedenthal-ch3-sysml.md](05-friedenthal-ch3-sysml.md) | Friedenthal/Moore/Steiner, *A Practical Guide to SysML* | Kap. 3 SysML Language Overview, s. 29–60 | en |
| 06 | [06-spu-vejledning-review.md](06-spu-vejledning-review.md) | SPU | Vejledning i review, s. 215–236 | da |
| 07 | [07-larman-ch9-domain-models.md](07-larman-ch9-domain-models.md) | Larman | Kap. 9 Domain Models, s. 131–159 | en |
| 08 | [08-peckol-ch9-system-design.md](08-peckol-ch9-system-design.md) | Peckol | Kap. 9 System Design, s. 366–368 og 376–391 | en |
| 09 | [09-vahid-ch6-interfacing-137-153.md](09-vahid-ch6-interfacing-137-153.md) | Vahid/Givargis, *Embedded System Design* | Kap. 6 Interfacing, s. 137–153 | en |
| 10 | [10-vahid-ch6-interfacing-166-169.md](10-vahid-ch6-interfacing-166-169.md) | Vahid/Givargis | Kap. 6 Interfacing, s. 166–169 (serielle protokoller: I²C, CAN, FireWire, USB) | en |
| 11 | [11-peckol-ch16-7-network-architecture.md](11-peckol-ch16-7-network-architecture.md) | Peckol | Kap. 16.7 Network Architecture, s. 627–637 | en |
| 12 | [12-vahid-ch1-embedded-systems-overview.md](12-vahid-ch1-embedded-systems-overview.md) | Vahid/Givargis | Kap. 1 Embedded Systems Overview, s. 1–11 | en |
| 13 | [13-vinje-udviklingsprocesser.md](13-vinje-udviklingsprocesser.md) | Vinje, *Projektledelse af systemudvikling* | Kap. 5.8–5.9, 6.3, 10.1–10.7 (Udviklingsprocesser) | da |
| 14 | [14-vinje-projektledelse.md](14-vinje-projektledelse.md) | Vinje | Kap. 3.1–3.4, 3.7, 5.4, 5.5, 6.4, 6.5, 6.7 (Projektledelse) | da |

## Hvilket kapitel hører til hvilken lektion

| Lektion/modul | Kapitler |
|---|---|
| [01-kravspecifikation](../01-kravspecifikation/README.md), [02-use-cases](../02-use-cases/README.md) | 01, 02 |
| [03-systemtest](../03-systemtest/README.md) | 03, 04 |
| [05-sysml](../05-sysml/README.md) | 05 |
| [08-kvalitetssikring](../08-kvalitetssikring/README.md) | 06 |
| [09-domaeneanalyse](../09-domaeneanalyse/README.md) | 07 |
| [06-arkitektur-og-design](../06-arkitektur-og-design/README.md) | 08, 09, 10 |
| [12-protokoller](../12-protokoller/README.md) | 10, 11 |
| [11-implementation](../11-implementation/README.md) | 12 |
| [04-udviklingsprocesser](../04-udviklingsprocesser/README.md) | 13 |
| [07-projektledelse](../07-projektledelse/README.md) | 14 |

## Kendte mangler i transskriptionen (ikke i konverteringen)

- Kap. 14 er stedvis OCR-korrumperet (3.3, 3.4, 6.5, 6.7); markeret i filen.
- Kap. 10 starter og slutter midt i en sætning; kap. 11's spillover starter midt i en sætning; kap. 13 har afbrudte afsnit (5.9, 6.3).
- Kap. 12, figur 3: TikZ tegner Technology C modsat teksten. Kap. 09, afsnit 6.4: garbled sætning. Begge bevaret med note.
- `spillover/*.tex`: `04_to_03`, `09_to_08`, `10_to_09` er \input'et og indsat; `05_to_04` er dublet af kap. 04 afsnit 6.3–6.4; `07_to_06` og `12_to_11` er indsat sidst i hhv. kap. 06 og 11.
