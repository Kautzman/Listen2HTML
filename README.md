# Listen2HTML
Gets HTML from a webpage.  Compares history and notifies user via email when changes occur

- Listen to one or more web pages
- Set up a timer, as long or as short as you want.
- The listener pulls the HTML from that page at the timer's cadence
- A comparison is made between the most recent and previous HTML pulls.  If a difference is detected, it notifies the user.
- Specify email info to recieve notifications when a change is detected and recieve a copy of both the current and previous HTML dumps.
- View the listener history inside the software.

## Status and Roadmap

This was built in a night and is not feature complete - it should be considered an Alpha.  Planned Features include:

- Persistent storage (information is lost upon close).
- Windowless function in the sys tray.
- Async all the things
- Robust per-line delta reporting
- Arbitrary Regex Compares.
