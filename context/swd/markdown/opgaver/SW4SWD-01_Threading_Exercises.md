# Øvelser — C# Threading (selvstudie)

## Metadata

| Felt | Værdi |
|---|---|
| **Type** | Øvelsessæt, selvstudie |
| **Kursus** | Softwaredesign (SW4SWD-01) |
| **Hører til** | Uge 11 forberedelse — se [../slides/SW4SWD-01_W10_Threading_in_CSharp.md](../slides/SW4SWD-01_W10_Threading_in_CSharp.md) |
| **Sprog/kode** | C# |
| **Kilde** | `Exercises.html` (Brightspace) |
| **Bilag (ikke downloadet)** | `11 Hints.pdf`, `11 Cards.zip` — ligger bag Brightspace quickLink |

Øvelserne bygger gradvist op: rå tråde → parameterisering → timing → join → baggrundstråde → graceful shutdown → race conditions → parallelt filarbejde → thread pool.

---

In these exercises, you will work with C# threads to create software with concurrency.

**Exercise 1:**Create a new C# console application project and add a new class “HelloWriter” to the project. HelloWriter shall have a name (you can use a property or assign a name in the constructor). Create a SayHello() method, which loops 1000 times and in the loop outputs:

“Hello from <name> #<number of times run>”

e.g. “Hello from writer A #394”.  Instantiate two HelloWriters, with different names. Create two threads and make one thread run the SayHello() method in one of the HelloWriters and make the other thread run the SayHello() method in the other HelloWriter.  Run your program a couple of times and observe the output.

**Exercise 2:** Change your program, so the number of times to loop in the SayHello() method is configurable. There are multiple ways to do this:

- Create a numberOfTimesToLoop property on the HelloWriter.

- Pass the number of times to loop to the HelloWriter constructor.

- Pass the number of times to loop in the Start() method on the thread.

Try implementing all three ways (one at the time). Discuss with another student: Are there any benefits or drawbacks of the different methods above?

**Exercise 3:** Make your HelloWriters sleep between each output. One should sleep 200 ms and one should sleep 500 ms.  **Exercise 4:** Make your main thread (the one staring the other threads) write “Hello from main” after starting the threads. Observe the console output. When do you see the message from the main thread?  **Exercise 5:** Change your code, so the main thread does not output “Hello from main” until both your HelloWriter threads have finished.  **Exercise 6:**Add a new thread, the NeverEndingStoryThread, to your program. The thread shall run an endless loop, which writes “Never ending story” every 5 seconds. Run your program. What happens?  **Exercise 7:**Change the NeverEndingStoryThread to be a background thread. What happens?  **Exercise 8:** Change the NeverEndingStoryThread back to being a foreground thread. Add code, so the NeverEndingStoryThread is stopped gracefully after your two HelloWriter threads have finished. How long does it take for the program to stop?

**Exercise 9:**Create a console application. Create a TotalCount class. It shall be very simple, with just an integer Count property, so you can read the count value and set a new value. Create a Counter class, with a StartCounting method, which loops a given number of times and increments the TotalCount for each loop. Create two threads. One thread shall run a Counter, which loops 200000 times. The other thread shall run a Counter, which loops 500000 times. Output the total count, when both threads have finished counting. What do you expect the total count to be? Run your program a couple of times and observe the output. What is the actual count?

**Exercise 10:**Consider: What is the shared resource? Change the TotalCount class implementation, so the total count always becomes correct. **Exercise 11:**A .zip file with 3 files containing card tuples are provided on Blackboard. Each tuple is a line in the file in the form “SPADE, 10” or “HEART, 8”. Write a console application (without threads), which answers the following questions: • How many cards are there? • What is the total sum of all SPADES? • How many aces (1’s) are there? Create three threads and let each thread process one file. You are now counting in parallel! Verify, that your parallel program gives the same results as you got when you counted without threads. **Exercise 12:**Use the thread pool to solve the same problem as in Exercise 11.

**Hints:**`11 Hints.pdf` *(ligger bag Brightspace — ikke downloadet)*

**Attachments:** `11 Cards.zip` *(ligger bag Brightspace — ikke downloadet)*