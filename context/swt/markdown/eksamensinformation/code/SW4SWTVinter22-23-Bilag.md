---
title: "SW4SWTVinter22-23 Bilag"
source: "SW4SWTVinter22-23 Bilag.zip"
modul: "Eksamensinformation"
type: "kildekode"
files: 1
---
# SW4SWTVinter22-23 Bilag

Kildeprojekt udpakket fra `SW4SWTVinter22-23 Bilag.zip`.

## Filtrae

```
Bilag/BeregnYdelser.cs
```

## `Bilag/BeregnYdelser.cs`

```csharp
namespace Banksystem.Classes
{
	public class BeregnYdelser : IBeregnYdelser
	{
		public double AktuelRente { get; private set; }

		// Her mangler constructor

		// Her mangler en event handler

		// Denne metode er implementeret, men skal testes
		// Den bruger annuitetsformlen
		public double BeregnYdelse(double beløb, int varighed)
		{
			double ydelse = double.MaxValue;

			if (varighed >= 1 && varighed <= 120)
			{
				if (AktuelRente == 0)
				{
					ydelse = beløb / varighed;
				}
				else
				{
					ydelse = beløb * AktuelRente / (1 - Math.Pow((1 + AktuelRente), -varighed));
				}
			}

			return ydelse;
		}
	}
}
```
