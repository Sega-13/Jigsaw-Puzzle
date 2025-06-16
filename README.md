                                                                    **#Jigsaw-Puzzle**   

Rearrange scattered puzzle pieces to form stunning images and put your logic and attention to detail to the test.   
   
**What I did**: Developed a Unity-based jigsaw puzzle game using C#, implementing game flow with a GameManager, difficulty selection via UI buttons, and a countdown timer. Puzzle pieces are instantiated from prefabs, and gameplay logic uses basic design patterns like Singleton and event-driven UI.   
                                                                    
**Genre**: Casual Game, PUZZLE   
    
**Technology**     
            •	Unity Engine    
            •	C# Scripting    
            •	TextMesh Pro    
            •	Custom Prefabs    
            •	Sprite Animation    
            •	Timer System    
            •	Button UI Logic    

**Design Pattern**      
      1. Singleton Pattern     
          •	Ensures there's only one active game manager controlling the state across scenes.   
      2. Event-Driven Programming   
          •	Through Unity's UI buttons and timers (e.g., Button.onClick), your game responds to user interactions and timed events, a foundational principle in Unity games.  
      3.	MVC-Like Separation    
          •	Model: Game state (score, puzzle data).   
          •	View: UI elements (images, buttons, timer).   
          •	Controller: Scripts like GameManager.cs, DifficultyLevelButton.cs.    
          •	This isn't strict MVC but mirrors its logic separation.    




