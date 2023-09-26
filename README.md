# Unity Save System
### A multiplatform Unity save system with the following functionality:
1. Create, save and load game data into multiple save slots.
2. Each save slot has its own metadata file, allowing you to quickly load information about it without loading the main file.
3. Any game system can subscribe to the save system to save and load data.
4. Game data is completely generic, allowing the data structures to be defined in the local context of a subscribing game system.
5. The generic save manager can easily be extended for testing and additional save files.
6. The save system can easily be extended to all platforms, by adding new file handlers and data serializers.
