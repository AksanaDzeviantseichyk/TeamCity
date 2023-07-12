using Minesweeper.Console;
using Minesweeper.Core;
using Minesweeper.Core.Enums;
using Minesweeper.Core.Models;

DifficultyLevel difficultyLevel = Printer.ChooseDifficultyLevel();
GameSettings settings = DifficultyManager.GetGameSettingsByDifficultylevel(difficultyLevel);

var field = FieldGenerator.GetRandomField(settings.Width, settings.Height, settings.Mines);

var gameProcessor = new GameProcessor(field);

var currentField = gameProcessor.GetCurrentField();

Printer.PrintField(currentField);

while (gameProcessor.GameState == GameState.Active)
{
    System.Drawing.Point coordinates = Printer.GetCoordinates();

    gameProcessor.Open(coordinates.X, coordinates.Y);
    currentField = gameProcessor.GetCurrentField();

    Printer.PrintField(currentField);
}

Printer.PrintGameResult(gameProcessor.GameState);
