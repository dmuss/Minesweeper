# Minesweeper
A classic minesweeper game with three levels of difficulty. Attempt to clear the minefield by revealing all non-mine cells.

## Rules
The game is won when all non-mine cells have been revealed. The player loses if they reveal a mine.

When a cell is revealed, it will display a number that represents how many neighbouring cells have been mined. When a cell containing no neighbouring mines is revealed, all other empty neighbouring cells are automatically revealed.

## Difficulty

The game has three difficulty levels, which determine the size of the minefield and the number of mines placed:

| Difficulty | Minefield Dimensions (Width x Height) | Number of Mines |
|:----------:|:-------------------------------------:|:---------------:|
| Easy       | 9 x 9                                 | 10              |
| Medium     | 16 x 16                               | 40              |
| Hard       | 30 x 16                               | 99              |

## Controls

Main menu buttons can be selected by clicking the left mouse button.

Left mouse button clicks will reveal individual cells. Right mouse button clicks will rotate through several flags, letting you mark a cell as a suspected or definite mine.

When a game is over, left or right mouse button clicks will return you to the main menu.

The game window can be closed through operating system controls, or by pressing `Escape` on your keyboard.

## Getting the Game

### Download a Release



### Build it Yourself

If you have installed the [.NET Framework](https://dotnet.microsoft.com/en-us/download), this project can be built using most default commands in your build tool of choice. Note that `Debug` builds allows the use of the `F12` key to reveal the minefield and any point and "win" the game.