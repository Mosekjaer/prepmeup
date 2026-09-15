# Kode: Minutur i C++ med interrupts (Atmel Studio)

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | L22 — Application Models og Implementation |
| **Kursus** | SWISE-01 Indledende System Engineering |
| **Kilde** | `MinutUrCppInt.zip` (Atmel Studio-projekt `MinutUr`, 9 kildefiler) |
| **Type** | eksempel (kode) |
| **Emner dækket** | Timer-timeout som interrupt (ISR) i stedet for polling, globale objekter, `sei()`, TIMSK1 |

---

## Mapping fra applikationsmodel til C++-klasser

Identisk klassestruktur med `kode-minutur-cpp.md` (samme `Ur`, `Display`, `KnapPanel`; se mapping-tabellen dér). Forskellen er, hvordan hændelsen `timeout` når frem til Ur:

| Aspekt | Polling-version (`MinutUrCpp`) | Interrupt-version (`MinutUrCppInt`) |
|---|---|---|
| Hvor lever objekterne | lokale i `main()` | `globalTimerObj`, `globalDisplayObj`, `globalUrObj` på filniveau, så ISR'en kan nå dem |
| timeout-hændelse | `main` poller `timerObj.checkForTimeout()` og kalder `ur.timeout()` | `ISR(TIMER1_OVF_vect)` kalder `globalUrObj.timeout()` direkte |
| Timer-klassen | sætter kun TCCR/TCNT/TIFR | `start()` enabler overflow-interrupt (`TIMSK1 |= 0x01`), `stop()` og constructor disabler det |
| main | poller både knapper og timer | `sei()` og poller kun knapper; timer-polling er udkommenteret |

Dette svarer til sekvensdiagrammet i applikationsmodellen (`ApplicationModel.pdf` s. 3), hvor `timeout()` går fra `timerObj:Timer` til `:Ur` uden om Hovedprogram — i modsætning til `ImplementationFinal.pdf`, hvor Hovedprogram poller `checkForTimeout()`.

Kun `main.cpp` og `Timer.cpp` er ændret i forhold til polling-versionen; de øvrige filer er byte-identiske, men gengives fuldt ud for fuldstændighed.

## Kildefiler

### `main.cpp`

```cpp
/*
 * MinutUr.cpp
 *
 * Created: 26-10-2017 10:52:42
 * Author : au237297
 */ 

#include <avr/io.h>
#include <avr/interrupt.h>

#include "Ur.h"
#include "Display.h"
#include "Timer.h"
#include "KnapPanel.h"

#define START 0
#define STOP 1
#define RESET 2

Timer globalTimerObj;

Display globalDisplayObj;

Ur globalUrObj(&globalTimerObj, &globalDisplayObj);

ISR (TIMER1_OVF_vect)
{
	globalUrObj.timeout();
}

int main(void)
{
	KnapPanel knapper;
	
	sei();
	
    /* Replace with your application code */
    while (1) 
    {
		if (knapper.checkForTast())
		{
			if (knapper.checkTast(START))
			{
				globalUrObj.start();
			}
			else if (knapper.checkTast(STOP))
			{
				globalUrObj.stop();
			}
			else if (knapper.checkTast(RESET))
			{
				globalUrObj.reset();
			}
		}
	
		// timeout() is called directly from interrupt routine
		//if (timerObj.checkForTimeout())
		//{
			//ur.timeout();
		//}
    }
}


```

### `Ur.h`

```cpp
/* 
* Ur.h
*
* Created: 26-10-2017 11:00:43
* Author: au237297
*/


#ifndef __UR_H__
#define __UR_H__

#include "Timer.h"
#include "Display.h"

enum Tilstand { STARTET, STOPPET};

class Ur
{
//variables
public:
protected:
private:
	unsigned char minutter;
	unsigned char sekunder;

	Timer *timerObjPtr;
	Display *displayObjPtr;
	
	Tilstand tilstand;
	
//functions
public:
	Ur(Timer *timerPtr, Display * displayPt);
	
	void start();
	void stop();
	void reset();
		
	void timeout();
	
protected:
private:
	void nulstil();
	void taelOp();

}; //Ur

#endif //__UR_H__

```

### `Ur.cpp`

```cpp
/* 
* Ur.cpp
*
* Created: 26-10-2017 11:00:43
* Author: au237297
*/


#include "Ur.h"

 Ur::Ur(Timer *timerPtr, Display * displayPtr)
 : timerObjPtr(timerPtr), displayObjPtr(displayPtr)
{
	nulstil();
	tilstand = STOPPET;
}

void Ur::start()
{
	switch (tilstand)
	{
		
		case STOPPET:
		timerObjPtr->start();
		tilstand = STARTET;
		break;
		
		case STARTET:
		break;
	}
}

void Ur::timeout()
{
		switch (tilstand)
		{
			
			case STOPPET:
			break;
			
			case STARTET:
			timerObjPtr->start();
			taelOp();
			displayObjPtr->vis(minutter, sekunder);
			break;
		}

}

void Ur::stop()
{
	switch (tilstand)
	{
		case STOPPET:
		break;
		
		case STARTET:
		timerObjPtr->stop();
		tilstand = STOPPET;
		break;
	}
}

void Ur::reset()
{
	switch (tilstand)
	{
		case STOPPET:
		nulstil();
		break;
		
		case STARTET:
		break;
	}
}


void Ur::nulstil()
{
	minutter = sekunder = 0;
	displayObjPtr->vis(0,0);
}

void Ur::taelOp()
{
	sekunder++;
	if (sekunder >= 60)
	{
		sekunder = 0;
		minutter++;
	}

}

```

### `Timer.h`

```cpp
/* 
* Timer.h
*
* Created: 26-10-2017 10:57:31
* Author: au237297
*/


#ifndef __TIMER_H__
#define __TIMER_H__


class Timer
{
//variables
public:
protected:
private:

//functions
public:
	Timer();
	
	void start();
	void stop();
	unsigned char checkForTimeout();

protected:
private:




}; //Timer

#endif //__TIMER_H__

```

### `Timer.cpp`

```cpp
/* 
* Timer.cpp
*
* Created: 26-10-2017 10:57:31
* Author: au237297
*/


#include "Timer.h"
#include <avr/io.h>

// default constructor
Timer::Timer()
{
	TCCR1A = 0b00000000;
	// Stop Timer 1 (ingen clock)
	TCCR1B = 0b00000000;
	// Nulstil Timer 1 overflow flag
	TIFR1 = 1<<0;
	
	// Disable interrupt
	TIMSK1 &= ~0X01;
} //Timer

void Timer::start()
{
	// Stop Timer 1 (ingen clock)
	TCCR1B = 0b00000000;
	// Nulstil Timer 1 overflow flag
	TIFR1 = 1<<0;

	// 16000000 Hz /256 = 62500 Hz
	// Vi har altså 62500 "trin" per sekund
	// - og ønsker 1 sekund til overflow
	TCNT1 = 65536-62500;
	// Timer 1 i Normal Mode og PS = 256
	TCCR1A = 0b00000000;
	TCCR1B = 0b00000100;

	// Enable interrupt
	TIMSK1 |= 0x01;
}

void Timer::stop()
{
	// Stop Timer 1 (ingen clock)
	TCCR1B = 0b00000000;
	// Nulstil Timer 1 overflow flag
	TIFR1 = 1<<0;
	// Disable interrupt
	TIMSK1 &= ~0X01;
}

unsigned char Timer::checkForTimeout()
{
	return (TIFR1 & (1<<0)) != 0;
}


```

### `KnapPanel.h`

```cpp
/* 
* KnapPanel.h
*
* Created: 26-10-2017 10:59:32
* Author: au237297
*/


#ifndef __KNAPPANEL_H__
#define __KNAPPANEL_H__

const unsigned char MAX_SWITCH_NR = 7;

class KnapPanel
{
//variables
public:
protected:
private:

//functions
public:
	KnapPanel();
	
	unsigned char checkForTast();
	unsigned char checkTast(unsigned char tast);

protected:
private:

}; //KnapPanel

#endif //__KNAPPANEL_H__

```

### `KnapPanel.cpp`

```cpp
/* 
* KnapPanel.cpp
*
* Created: 25-10-2017 20:45:53
* Author: au237297
*/


#include "KnapPanel.h"

#include <avr/io.h>

// default constructor
KnapPanel::KnapPanel()
{
	// Switch-port = All inputs
	DDRA = 0;
} //KnapPanel

unsigned char KnapPanel::checkForTast()
{
	return (~PINA);
}

unsigned char KnapPanel::checkTast(unsigned char switch_nr)
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

### `Display.h`

```cpp
/* 
* Display.h
*
* Created: 26-10-2017 10:55:22
* Author: au237297
*/


#ifndef __DISPLAY_H__
#define __DISPLAY_H__


class Display
{
//variables
public:
protected:
private:

//functions
public:
	Display();
	void vis(unsigned char min, unsigned char sek);

protected:
private:


}; //Display

#endif //__DISPLAY_H__

```

### `Display.cpp`

```cpp
/* 
* Display.cpp
*
* Created: 26-10-2017 10:55:22
* Author: au237297
*/


#include "Display.h"
#include <avr/io.h>

// default constructor
Display::Display()
{
	  // Sæt alle PORTB's ben til at være udgange
	  DDRB = 0xFF;
	  // Sluk alle lysdioderne
	  PORTB = 0;
} //Display

void Display::vis(unsigned char min, unsigned char sek)
{
	unsigned char pattern = (min << 6) + sek;
	PORTB = pattern; 
}



```

