# TheRabbitsFootCode
The scripts used by the Rabbit's Foot game demo

The scripts for the Rabbit's Foot are mostly contained in the Object Classes folder. The two not in this folder are described below:

- CardTablePositions.cs: This class contains the various positions in which cards will need to be initialized. They are all stored in one class to avoid repeating code and make the code more readable, as random numbers would make it difficult to tell where a card is being instantiated.
  
- Enums.cs: This class contains several Enumerators that are frequently referenced by the code. These enums are for Suits, corresponding to card suits: Ranks, corresponding to card ranks: and Hands, corresponding to the different hands a player can have. There is also one method, which allows returns the next rank after a given rank, used to compare cards to each other.

There are also a few things to note about the classes in the Object Classes folder:

- 
