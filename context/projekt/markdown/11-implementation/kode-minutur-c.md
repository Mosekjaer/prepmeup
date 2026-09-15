# Kode: Minutur i C (Atmel Studio, ATmega2560)

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | L22 — Application Models og Implementation |
| **Kursus** | SWISE-01 Indledende System Engineering |
| **Kilde** | `MinutUrC.zip` (Atmel Studio-projekt `MinuturC`, 9 kildefiler) |
| **Type** | eksempel (kode) |
| **Emner dækket** | Implementation af applikationsmodel i ren C, ét "objekt" pr. modul (static), state machine som switch, polling af knapper og timer, AVR Timer 1 |

---

## Mapping fra applikationsmodel til C-moduler

Ren C-implementation af minuturet på Arduino Mega2560 (I/O Shield). Der anvendes de drivere (`led`, `switch`), som E og SW kender fra MSYS. Hver "klasse" er et modul (`.c`/`.h`); private attributter og operationer er `static` i `.c`-filen; public operationer navngives `<klasse><Operation>` (jf. UML-Light-Ur §5.4.1 — dog med camelCase `urStart()` i stedet for `Ur_start()`).

| Applikationsmodel-klasse | Stereotype | C-modul | Public operationer |
|---|---|---|---|
| Hovedprogram | utility | `main.c` | `main()` — polling-løkke |
| Ur | «controller» | `ur.c` / `ur.h` | `urInit()`, `urStart()`, `urStop()`, `urReset()`, `urTimeout()` |
| Timer | «boundary» | `timer.c` / `timer.h` | `timerInit()`, `timerCheckForTimeout()`, `timerStart()`, `timerStop()` |
| KnapPanel | «boundary» | `switch.c` / `switch.h` (MSYS-driver) | `initSwitchPort()`, `switchStatus()`, `switchOn(nr)` |
| Display | «boundary» | `led.c` / `led.h` (MSYS-driver) | `initLEDport()`, `writeAllLEDs(pattern)` m.fl. |

Bemærkninger:
- Modellens `checkForTast()` svarer til `switchStatus() != 0`; `checkTast(tast)` svarer til `switchOn(nr)`. START/STOP/RESET er switch 0/1/2.
- Modellens `vis(min, sec)` er reduceret til `writeAllLEDs(tid)` — displayet er 8 LED'er, og Ur holder kun én tæller `tid` (unsigned char) i stedet for `minutter`/`sekunder`.
- Timer 1 (16 bit) i Normal Mode med prescaler 256 ved 16 MHz: `TCNT1 = 65536-62500` giver overflow efter præcis 1 s. `timerCheckForTimeout()` poller overflow-flaget TOV1 i `TIFR1`.
- State machine i `ur.c`: `enum TILSTAND { STARTET, STOPPET }` og `switch (Tilstand)` i hver event-operation — direkte oversættelse af tilstandsdiagrammet.

## Kildefiler

### `main.c`

```c
/*
 * MinuturC.c
 *
 * Created: 18-10-2017 09:35:18
 * Author : au237297
 */ 

#include <avr/io.h>
#include "led.h"
#include "switch.h"
#include "timer.h"
#include "ur.h"

#define START_KNAP 0
#define STOP_KNAP 1
#define RESET_KNAP 2

int main(void)
{
    initLEDport();
    initSwitchPort();
	timerInit();
	urInit();
    
    while (1)
    {
		if (switchStatus() != 0)
		{
			if (switchOn(START_KNAP))
			{
				// Start ur
				urStart();
			}
			else if (switchOn(STOP_KNAP))
			{
				// Stop ur
				urStop();
			}
			else if (switchOn(RESET_KNAP))
			{
				// Reset ur
				urReset();				
			}
		}
		
		if (timerCheckForTimeout())
		{
			urTimeout();
		}
    }
}


```

### `ur.h`

```c
/*
 * ur.h
 *
 * Created: 18-10-2017 09:52:44
 *  Author: au237297
 */ 


#ifndef UR_H_
#define UR_H_

void urInit();

void urStart();
void urStop();
void urReset();
void urTimeout();



#endif /* UR_H_ */
```

### `ur.c`

```c
/*
 * ur.c
 *
 * Created: 18-10-2017 09:58:49
 *  Author: au237297
 */ 

#include "ur.h"
#include "timer.h"
#include "led.h"

enum TILSTAND { STARTET, STOPPET };
static enum TILSTAND Tilstand;

static unsigned char tid;

static void nulstil()
{
	tid = 0;
	writeAllLEDs(tid);
}

static void taelOp()
{
	tid++;
}

void urInit()
{
	Tilstand = STOPPET;
	nulstil();
	timerStop();
}

void urStart()
{
	switch (Tilstand)
	{
	case STOPPET:
		Tilstand = STARTET;
		timerStart();
		break;
		
	case STARTET:
		break;
	}
}

void urStop()
{
	switch (Tilstand)
	{
	case STOPPET:
		break;
		
	case STARTET:
		Tilstand = STOPPET;
		timerStop();
		break;
	}
}

void urReset()
{
	switch (Tilstand)
	{
	case STOPPET:
		nulstil();
		break;
		
	case STARTET:
		break;
	}
}


void urTimeout()
{
	switch (Tilstand)
	{
	case STOPPET:
		break;
		
	case STARTET:
		timerStart();
		taelOp();
		writeAllLEDs(tid);		
		break;
	}
}

```

### `timer.h`

```c
/*
 * IncFile1.h
 *
 * Created: 18-10-2017 09:49:13
 *  Author: au237297
 */ 


#ifndef TIMER_H_
#define TIMER_H_

void timerInit();
unsigned char timerCheckForTimeout();
void timerStart();
void timerStop();



#endif /* INCFILE1_H_ */
```

### `timer.c`

```c
/*
 * timer.c
 *
 * Created: 18-10-2017 10:15:05
 *  Author: au237297
 
 We will use ATMega2560 Timer 1, because it is a 16 bit timer, and we need 1 s = 1000 ms
 We assume clock is 16 MHz
 
 */

#include <avr/io.h>
#include "timer.h"

void timerInit()
{
	// Stop Timer 1 (ingen clock)
	TCCR1A = 0b00000000;
	TCCR1B = 0b00000000;
	// Nulstil Timer 1 overflow flag
	TIFR1 = 1<<0;
}

unsigned char timerCheckForTimeout()
{
	// Check for Timer 1 overflow flag
	return (TIFR1 & (1<<0)) != 0;
}

void timerStart()
{
	// Stop og tag tidligere flag ned
	TCCR1B = 0b00000000;
	// Nulstil Timer 1 overflow flag
	TIFR1 = 1<<0;
	
	// Brug pre scaling med 256
	// 16000000 Hz /256 = 62500 Hz
	// Vi har altså 62500 "trin" per sekund
	// - og ønsker 1 sekund til overflow
	TCNT1 = 65536-62500;
	// Timer 1 i Normal Mode og PS = 256 startes nu
	TCCR1A = 0b00000000;
	TCCR1B = 0b00000100;
}

void timerStop()
{
	// Stop Timer 1 (ingen clock)
	TCCR1B = 0b00000000;
	// Nulstil Timer 1 overflow flag
	TIFR1 = 1<<0;
}


```

### `switch.h`

```c
/**********************************************************
* "Switch.h"                                              *
* Header file for "Mega2560 I/O Shield" SWITCH driver.    *
* Henning Hargaard, 23/9 2015                             *
***********************************************************/
void initSwitchPort();
unsigned char switchStatus();
unsigned char switchOn(unsigned char switch_nr);
/**********************************************************/
```

### `switch.c`

```c
/**********************************************************
* "Switch.c"                                              *
* Implementation for "Mega2560 I/O Shield" SWITCH driver. *
* Henning Hargaard, 23/9 2015                             *
***********************************************************/
#include <avr/io.h>
#define MAX_SWITCH_NR 7

// Klargør switch-porten
void initSwitchPort()
{
  // Switch-port = All inputs
  DDRA = 0;
}

// Læser alle switches samtidigt
unsigned char switchStatus()
{
  return (~PINA);
}

// Returnerer TRUE, hvis switchen med nummeret
// "switch_nr" er aktiveret - ellers returneres FALSE
unsigned char switchOn(unsigned char switch_nr)
{
unsigned char mask;
  if (switch_nr <= MAX_SWITCH_NR)
  {
    mask = 0b00000001 << switch_nr;
    return (~PINA & mask);
  }
  else
    return 0;
} 
```

### `led.h`

```c
/******************************************
* "LED.H"                                 *
* Header file for "Mega2560 LED driver"   *
* Henning Hargaard, 25/10 2016            *
*******************************************/ 
void initLEDport();
void writeAllLEDs(unsigned char pattern);
void turnOnLED(unsigned char led_nr);
void turnOffLED(unsigned char led_nr);
void toggleLED(unsigned char led_nr);
/******************************************/

```

### `led.c`

```c
/*************************************************
* "LED.C"                                        *
* Implementation file for "Mega2560 LED driver"  *
* Henning Hargaard, 22/9 2015                    *
**************************************************/
#include <avr/io.h>
#define MAX_LED_NR 7

void initLEDport()
{
  // Sæt alle PORTB's ben til at være udgange
  DDRB = 0xFF;
  // Sluk alle lysdioderne
  PORTB = 0;
}

void writeAllLEDs(unsigned char pattern)
{
  // Hent parameteren og skriv til lysdioderne
  PORTB = pattern;   
}

void turnOnLED(unsigned char led_nr)
{
// Lokal variabel
unsigned char mask;
  // Vi skal kun lave noget, hvis led_nr < 8
  if (led_nr <= MAX_LED_NR)
  {
    // Dan maske på basis af parameteren (led_nr)
    mask = 0b00000001 << led_nr;
    // Tænd den aktuelle lysdiode (de andre ændres ikke)
    PORTB = PINB | mask;
  }   
}

void turnOffLED(unsigned char led_nr)
{
  // Lokal variabel
  unsigned char mask;
  // Vi skal kun lave noget, hvis led_nr < 8
  if (led_nr <= MAX_LED_NR)
  {
	// Dan (inverteret) maske på basis af parameteren (led_nr)
    mask = ~(0b00000001 << led_nr);
    // Sluk den aktuelle lysdiode (de andre ændres ikke)
    PORTB = PINB & mask;
  }
}

void toggleLED(unsigned char led_nr)
{
  // Lokal variabel
  unsigned char mask;
  // Vi skal kun lave noget, hvis led_nr < 8
  if (led_nr <= MAX_LED_NR)
  {
    // Dan maske på basis af parameteren (led_nr)
    mask = 0b00000001 << led_nr;
    // Toggle den aktuelle lysdiode (de andre ændres ikke)
    PORTB = PINB ^ mask;
  }
}
```

