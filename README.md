# TheRabbitsFootCode
The scripts used by the Rabbit's Foot game demo

The scripts for the Rabbit's Foot are mostly contained in the Object Classes folder. The two not in this folder are described below:

- CardTablePositions.cs: This class contains the various positions in which cards will need to be initialized. They are all stored in one class to avoid repeating code and make the code more readable, as random numbers would make it difficult to tell where a card is being instantiated.
  
- Enums.cs: This class contains several Enumerators that are frequently referenced by the code. These enums are for Suits, corresponding to card suits: Ranks, corresponding to card ranks: and Hands, corresponding to the different hands a player can have. There is also one method, which allows returns the next rank after a given rank, used to compare cards to each other.

There are also a few things to note about the classes in the Object Classes folder:

- The entire "game", that is the poker game, is controlled from the TexasHoldEmTable class, and would begin when the player sits down in game. In the demo, the game simply begins by default. The choice to have the entire game run from one class was so that different tables would be able to control different kinds of poker games, allowing for the kind of poker game to be more easily changed.

- To play the poker game, the players are also broken down into AI and Human players. As the names would suggest, the AI classes are for AI, and the Human classes are for Human players. Both classes extend the Player class. The Human doesn't actually do much itself, but is necessary in order for the code to be able to recognize when it is an AI or a Player's turn.
