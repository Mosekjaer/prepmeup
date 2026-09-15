# Note – .gitignore til .NET-projekter

## Metadata

- **Lektion:** Generel værktøjsnote
- **Kursus:** Front-end udvikling (SW4FED-02)
- **Forelæser:** Poul Ejnar Rovsing / Jung Min Kim
- **Kilde:** Brightspace-note "gitignore for .Net projects"
- **Emner dækket:**
  - Generering af en .gitignore til .NET-projekter med dotnet CLI

---

## Best way to create a .gitignore for .NET projects

Åbn en terminal i projektets rodmappe og giv denne kommando:

```bash
dotnet new gitignore
```

Kommandoen lægger en færdig .NET-tilpasset `.gitignore` i mappen, så build-artefakter som `bin/` og `obj/` ikke havner i dit repository.
