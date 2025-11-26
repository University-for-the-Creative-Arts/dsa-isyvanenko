using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TextBasedCsharp : MonoBehaviour
{
   
    public class StoryNode
    {
        // Unique identifier for this story node
        public string id;
        
        // The text description or dialogue displayed when this node is reached.
        public string text;
        
        // The amount by which this node changes the player's Slaytion score.
        // Positive = good (slaying), negative = bad (flopping).
        public int slayChange;
        
        // A list of edges (player choices) connecting this node to other nodes.
        public List<Edge> edges = new List<Edge>();
        
        public StoryNode(string id, string text, int slayChange = 0)
        {
            this.id = id;
            this.text = text;
            this.slayChange = slayChange;
        }
    }
    
    public class Edge
    {
       
        public string choiceText;
        
        public StoryNode nextNode;

        public Edge(string choiceText, StoryNode nextNode)
        {
            this.choiceText = choiceText;
            this.nextNode = nextNode;
        }
    }
    
    //Stores all story nodes using their IDs as keys.

    private Dictionary<string, StoryNode> graph = new Dictionary<string, StoryNode>();
    
    // The node that the player is currently at in the story.
    private StoryNode currentNode;
    
    // The player's current Slaytion score
    private int slaytion = 0;
    
    // Tracks whether the game has ended to prevent further input.
    private bool gameOver = false;
    
    void Start()
    {
        var start = new StoryNode("start",
            "Hey Queen, Welcome to the stage of Queen of the World! " +
            "Slay the crown and send them bitches packing. But remember, your choices will take you closer or further from the crown. Let the race begin!!!.");

        var left = new StoryNode("left",
            "You checked your wig and saw people can see your hairline. +1 To Slaytion.", 1);

        var right = new StoryNode("right",
            "They made fun of your wig and started to call you Silly Sally. -2 To Slaytion.", -2);

        var challange = new StoryNode("challange",
            "You got into quick drag and ready to perform Cher.");

        var eyel = new StoryNode("Checking eyelashes",
            "You messed your lashes up and got your makeup all smudged. -4 To Slaytion.", -4);

        var challangeWin = new StoryNode("Challange win",
            "You slayed the challenge — judges were living! +2 To Slaytion.", 2);

        var challangeLost = new StoryNode("Challange Lose",
            "Judges weren't living... -5 To Slaytion.", -5);

        var finalchallange = new StoryNode("Final Challange",
            "Queens I made my decision. I'm sorry my queen but you're a bottom 2. Lipsync song is Nicki Minaj - Anaconda.");

        var end1 = new StoryNode("end1",
            "END GAME. Sorry my dear but you're going home, we weren't impressed with you being bold.");

        var end2 = new StoryNode("end2",
            "END GAME. My dear, you’re safe to slay another day!");

        // === Connect nodes ===
        start.edges.Add(new Edge("Check your wig.", left));
        start.edges.Add(new Edge("Meet the girls.", right));

        left.edges.Add(new Edge("Check your eyelashes.", right));
        left.edges.Add(new Edge("Get ready for the mini challenge.", challange));

        right.edges.Add(new Edge("Check your eyelashes.", eyel));
        right.edges.Add(new Edge("Start crying.", challangeLost));

        eyel.edges.Add(new Edge("Get ready for the mini challenge.", challange));

        challange.edges.Add(new Edge("Flip your hair.", challangeWin));
        challange.edges.Add(new Edge("Be emotional with the song.", challangeLost));

        challangeWin.edges.Add(new Edge("You ate the Strong Enough with your hair flips and dead drops.", finalchallange));
        challangeLost.edges.Add(new Edge("You might be at the bottom.", finalchallange));

        finalchallange.edges.Add(new Edge("Take your wig off — My Anaconda Don't.", end1));
        finalchallange.edges.Add(new Edge("Start twerking on judges’ table.", end2));
        
        graph[start.id] = start;
        graph[left.id] = left;
        graph[right.id] = right;
        graph[eyel.id] = eyel;
        graph[challange.id] = challange;
        graph[challangeWin.id] = challangeWin;
        graph[challangeLost.id] = challangeLost;
        graph[finalchallange.id] = finalchallange;
        graph[end1.id] = end1;
        graph[end2.id] = end2;
        
        currentNode = start;
        DisplayNode();
    }
    
    // Displays the current node's text, Slaytion score changes,
    void DisplayNode()
    {
        Debug.Log($" {currentNode.text}");
        slaytion += currentNode.slayChange;

        if (currentNode.edges.Count == 0)
        {
            Debug.Log($" FINAL SLAYTION SCORE: {slaytion}");
            Debug.Log(" Thanks for playing Queen!");
            gameOver = true;
            return;
        }

        Debug.Log($" Current Slaytion: {slaytion}");
        for (int i = 0; i < currentNode.edges.Count; i++)
        {
            Debug.Log($"{i + 1}: {currentNode.edges[i].choiceText}");
        }
    }

    /// <summary>
    /// Unity’s frame update method.
    /// Checks for number key presses (1–9) corresponding to player choices.
    /// </summary>
    void Update()
    {
        if (gameOver) return;

        // Listen for number key presses (1–9)
        for (int i = 0; i < currentNode.edges.Count; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                MakeChoice(i);
                break;
            }
        }
    }

    /// <summary>
    /// Advances the story based on the player’s selected choice.
    /// </summary>
    /// <param name="choiceIndex">The index of the chosen edge in the current node's list.</param>
    void MakeChoice(int choiceIndex)
    {
        if (choiceIndex < 0 || choiceIndex >= currentNode.edges.Count)
        {
            Debug.Log("Invalid choice, try again!");
            return;
        }

        currentNode = currentNode.edges[choiceIndex].nextNode;
        DisplayNode();
    }
}