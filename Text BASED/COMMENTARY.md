For my project, I used a graph-style data structure to build the story. Each part of the story is a node, and every player choice connects one node to another. I chose this setup because it keeps branching dialogue simple and flexible — I can easily add new paths, endings, or scenes without rewriting everything.

Player choices are stored as edges between nodes. Each node contains the story text, the “slay change” value, and a list of choices. When the player picks an option, the game follows that edge to the next node. It’s a clean way to track progress and keep the story flowing smoothly.

All nodes are stored in a dictionary using unique IDs. This made it easy to find and connect scenes, especially compared to using a list where everything was based on index numbers. With the dictionary, I could quickly reference nodes by name and link them together like a web.

The hardest part was debugging the narrative flow. Sometimes choices didn’t connect properly or caused loops back to earlier points. I also had to make sure the “slaytion” score updated correctly each time the player made a decision. Once everything linked up properly, though, it felt really satisfying to see the story play out in order.

Overall, this project taught me how powerful simple data structures can be in storytelling. Even with basic code, it’s possible to create an interactive and fun narrative experience.
