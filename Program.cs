using System;

Random random = new Random();
Console.CursorVisible = false;
int height = Console.WindowHeight - 1;
int width = Console.WindowWidth - 5;
bool shouldExit = false;

// Console position of the player
int playerX = 0;
int playerY = 0;

// Console position of the food
int foodX = 0;
int foodY = 0;

// Position of the human
int humanX = 0;
int humanY = 0;

// Available player and food strings
string[] states = {"=^..^=", "=^--^=", "=^oo^="};
string food = "|FOOD|";
string[] humans = {"|'<'|", $"|'>'|", "|.<.|", "|.>.|"};

// Current player string displayed in the Console
string player = states[0];
string human = humans[0];
int playerScore = 0;

InitializeGame();
int playerCounter = 0;
int playerSpeed = 2;
int humanCounter = 0;
int humanSpeed = 1;
while (!shouldExit) 
{
    ResizeTerminal();
    Console.SetCursorPosition(Console.WindowWidth - 10, 0);
    Console.Write("Score: " + playerScore);

    playerCounter += playerSpeed;
    if (playerCounter > 16) 
    {
        Move();
        playerCounter = 0;
    }
    if (WasFoodConsumed())
    {
        EatFood();
        FoodPlayerChange();
        ShowFood();
        humanSpeed++;
    }
    humanCounter += humanSpeed;

    if (humanCounter > 16) {
        HumanMove();
        humanCounter = 0;
    }

    Console.SetCursorPosition(foodX, foodY);
    Console.Write(food);
}

// Resizes the terminal if necessary
void ResizeTerminal() 
{
    if (height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5)
    {
        height = Console.WindowHeight - 1;
        width = Console.WindowWidth - 5;
    }
}

// Displays food at a random location
void ShowFood() 
{
    // Update food position to a random location
    foodX = random.Next(0, width - player.Length);
    foodY = random.Next(0, height - 1);

    // Display the food at the location
    Console.SetCursorPosition(foodX, foodY);
    Console.Write(food);
}

void ShowHuman()
{
    // Place human at random location
    humanX = random.Next(0, width - human.Length);
    humanY = random.Next(0, height - 1);

    // Display the human
    Console.SetCursorPosition(humanX, humanY);
    Console.Write(human);
}

void HumanMove()
{
    int lastX = humanX;
    int lastY = humanY;
    
    int direction = random.Next(0, 6);
    switch (direction)
    {
        case 0:
        case 1:
        case 2:
        case 3:
            if (playerX == humanX) 
            {
                if (playerY > humanY) 
                {
                    humanY += 3;
                    humanSpeed = 2;
                } 
                else
                {
                    humanY -= 3;
                    humanSpeed = 2;
                }
            }
            else if (playerX > humanX)
            {
                humanX++;
                human = humans[1];
                humanSpeed = 1;
            }
            else
            {
                humanX--;
                human = humans[0];
                humanSpeed = 1;
            }
            break;
        case 4:
        case 5:
             if (playerY == humanY) 
            {
                if (playerX > humanX) 
                {
                    humanX += 3;
                    humanSpeed = 2;
                } 
                else
                {
                    humanX -= 3;
                    humanSpeed = 2;
                }
            }
            else if (playerY > humanY)
            {
                humanY++;
                humanSpeed = 1;
            }
            else
            {
                humanY--;
                humanSpeed = 1;
            }
            break;
    }

    Console.SetCursorPosition(lastX, lastY);
    for (int i = 0; i < human.Length; i++) 
    {
        Console.Write(" ");
    }

    // Keep human position within the bounds of the Terminal window
    humanX = (humanX < 0) ? 0 : (humanX >= width ? width : humanX);
    humanY = (humanY < 0) ? 0 : (humanY >= height ? height : humanY);

    // Draw the human at the new location
    Console.SetCursorPosition(humanX, humanY);
    Console.Write(human);

    if (HitDetection()) {
        CaughtPlayerChange();
        PlayerReturn();
    }

}

bool HitDetection() 
{

    if ((humanX <= (playerX + player.Length) && humanX >= playerX) && (humanY <= playerY + 1 && humanY >= playerY - 1))
    {
        return true;
    }
    else if (((humanX + human.Length) >= playerX && (humanX + human.Length) <= (playerX + player.Length)) && (humanY <= playerY + 1 && humanY >= playerY - 1))
    {
        return true;
    }
    else
    {
        return false;
    }

}

// Check to see if player consumed food

bool WasFoodConsumed()
{
     if ((playerX <= (foodX + food.Length) && playerX >= foodX) && (playerY <= foodY + 1 && playerY >= foodY - 1))
    {
        return true;
    }
    else if (((playerX + player.Length) >= foodX && (playerX + player.Length) <= (foodX + food.Length)) && (playerY <= foodY + 1 && playerY >= foodY - 1))
    {
        return true;
    }
    else
    {
        return false;
    }

}

void EatFood()
{
    // make food disappear
    Console.SetCursorPosition(foodX, foodY);
    for (int i = 0; i < food.Length; i++) 
    {
        Console.Write(" ");
    }

    // add points
    playerScore += 5;

}

// Changes the player icon when food is consumed
void FoodPlayerChange() 
{
    player = states[1];
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
    System.Threading.Thread.Sleep(1000);
    player = states[0];
}

// changes player icon when caught by human

void CaughtPlayerChange()
{
    player = states[2];
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

// returns player to game start, resetting score and human speed

void PlayerReturn()
{
    playerScore = 0;
    humanSpeed = 1;

    while (playerX > 0 || playerY > 0) 
    {
        int lastX = playerX;
        int lastY = playerY;

        playerX--;
        playerY--;

        Console.SetCursorPosition(lastX, lastY);
        for (int i = 0; i < player.Length; i++) 
        {
            Console.Write(" ");
        }

        playerX = (playerX < 0) ? 0 : (playerX >= width ? width - 1 : playerX);
        playerY = (playerY < 0) ? 0 : (playerY >= height ? height - 1 : playerY);

        Console.SetCursorPosition(playerX, playerY);
        Console.Write(player);
        System.Threading.Thread.Sleep(10);

    }
    player = states[0];
    InitializeGame();
}

// Reads directional input from the Console and moves the player
void Move() 
{
    int lastX = playerX;
    int lastY = playerY;
    
    switch (Console.ReadKey(true).Key) 
    {
        case ConsoleKey.UpArrow:
            playerY--; 
            break;
		case ConsoleKey.DownArrow: 
            playerY++; 
            break;
		case ConsoleKey.LeftArrow:  
            playerX--; 
            break;
		case ConsoleKey.RightArrow: 
            playerX++; 
            break;
		case ConsoleKey.Escape:     
            shouldExit = true; 
            break;
    }

    // Clear the characters at the previous position
    Console.SetCursorPosition(lastX, lastY);
    for (int i = 0; i < player.Length; i++) 
    {
        Console.Write(" ");
    }

    // Keep player position within the bounds of the Terminal window
    playerX = (playerX < 0) ? 0 : (playerX >= width ? width - 1 : playerX);
    playerY = (playerY < 0) ? 0 : (playerY >= height ? height - 1 : playerY);

    // Draw the player at the new location
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);

    if (HitDetection()) {
        CaughtPlayerChange();
        PlayerReturn();
    }
}

// Clears the console, displays the food and player
void InitializeGame() 
{
    Console.Clear();
    Console.SetCursorPosition(Console.WindowWidth - 10, 0);
    Console.Write("Score: " + playerScore);
    ShowFood();
    ShowHuman();
    Console.SetCursorPosition(0, 0);
    Console.Write(player);
}