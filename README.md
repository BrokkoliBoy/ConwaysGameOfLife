# Conway's Game of Life

**Status:** Finished  |  **Built:** 2021  |  **Role:** Solo developer

A from-scratch Unity implementation of Conway's Game of Life, built in a single weekend to win a bet. I came back later and added a full settings panel and several additional rule variants on top of that original build.

*(GIF idea, provisional: a short intro clip giving a first overall impression before reading further, e.g. an interesting pattern evolving on the board for a few seconds. The one GIF a skimming reader sees before deciding whether to read on.)*

## What is it?
A Unity simulation of Conway's Game of Life: a grid of cells that live, die, or come to life each generation based on how many neighbors are alive, following the classic ruleset (a dead cell with exactly 3 live neighbors is born, a live cell with fewer than 2 or more than 3 live neighbors dies, everything else stays the same). Click cells to toggle them alive or dead, then step through generations manually or let the simulation run automatically at an adjustable speed.

## Goal
My brother described Conway's Game of Life to me as a fascinating but complicated system. I bet him I could implement it from scratch in two days. He took the bet, I built this between Friday evening and Sunday evening, and won. The goal was never to build the most feature-complete version of the game. It was to prove the core idea is simple by shipping a working implementation fast. Every design choice below was picked because it was cheap to build within that two-day window, not because it was the best possible approach.

## Concept
The simulation runs on a fixed-size grid rather than a truly infinite one. An unbounded board needs chunked or sparse storage to avoid unbounded memory use, which is solvable but not a two-day problem. Instead, the board wraps at its edges like the arena in old mobile Snake games: the top row's neighbors include the bottom row, and the left column's neighbors include the right column, so the grid behaves like an endless tiled surface without actually being one.

Each generation, every cell first counts its 8 neighbors (with wraparound) and decides its next state, and only after every cell has decided does any cell actually change. This two-pass approach matters because a cell's decision must be based on the previous generation only. If cells updated one by one in a single pass, later cells in that pass would see some neighbors that already updated and some that hadn't, corrupting the result. Simulation speed is also decoupled from the frame rate through a small time-scaling layer, so generations advance at a fixed, adjustable rate regardless of how fast the game is rendering.

## Features
*(Placeholder note, not final: the italic lines below are provisional ideas for where a short GIF could go, added while planning what is worth recording. Not confirmed content.)*

- Standard Conway's Game of Life rules on a wrapping (toroidal) grid
  - *GIF idea: a well-known pattern (e.g. a glider) moving across the board and visibly wrapping around an edge, probably the clearest single shot of both the simulation and the toroidal topology at once.*
- Click and hold the mouse to set cells alive or dead
  - *GIF idea: painting a recognizable shape onto an empty board by clicking/dragging.*
- Adjustable simulation speed, play/pause, and manual single-step
  - *GIF idea: unpausing a populated board and watching it run, maybe with a visible speed change.*
- Randomize the board, either with a fresh random seed or a fixed one for a reproducible starting pattern
  - *GIF idea: randomizing the board a couple of times in a row.*
- Clear the board back to empty
  - *GIF idea: clearing a populated board.*
- Generation counter
- Five selectable rulesets — Conway's classic rules, HighLife, Day and Night, Seeds, and Maze — each producing very different behavior from the same simulation
  - *GIF idea: switching rulesets on a running board and watching the behavior change.*
- Settings panel with icon buttons and sliders for simulation speed, starting density, and cell fade time

## Learnings
**Going back in time doesn't work the way it seems like it should.** While building this I assumed I'd eventually add a "step backward" feature and reserved a key binding for it early on. It turns out you can't just run the rules in reverse: several different previous generations can produce the same next generation, so "the generation before this one" is not something you can reconstruct from the current state alone. You'd have to record history separately as you go. I never removed the leftover key binding, so a "go back" input exists in the code with nothing behind it.

**Deliberately stopping was the right call, but I did come back once.** After winning the bet I had a running list of ways to keep extending this: a genuinely unbounded board (and a more efficient way to render cells than instantiating a prefab per cell, which is what happens now), the ability to enable or disable individual cells so you can shape your own play area instead of one big rectangle, a library of well-known patterns you could drag onto the board, undo via a stored generation history (with the state of a whole generation packed into bits instead of one bool per cell, for a much smaller memory footprint), swappable rule variants, and a proper settings UI instead of Inspector-only controls. I originally chose not to build any of it, on purpose. I tend to keep projects open-ended rather than calling them finished, and having one project I can point to as genuinely done, instead of one more half-built thing on the pile, was worth more to me than any of those features. Later I picked the two cheapest, highest-value items off that list — rule variants and a settings panel — and added them on top of the finished weekend build. The rest (unbounded board, per-cell enable/disable, pattern library, undo) stayed out of scope.

## Build / Play
Not published yet. A build and a short gameplay clip are planned but not recorded.
