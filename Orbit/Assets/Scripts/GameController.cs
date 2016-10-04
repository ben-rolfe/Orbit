using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	public static GameController singleton;
	public Dictionary<string, GameObject> overlays = new Dictionary<string, GameObject>();
	string _overlay = "Ship Overlay";

	public static bool isMale = false;
	public Slider moveSlider;
	public Slider moveSliderChildVR;
	public Slider moveSliderAdultVR;
	public ShipAvatar shipAvatar;
	public Compass compass;

	[SerializeField]
	Text questText;
	[SerializeField]
	Transform rewardSplash;
	[SerializeField]
	Transform progressSplash;

	public static List<TestInt> tests = new List<TestInt>();

	public Dictionary<string, ScriptLine> lines = new Dictionary<string, ScriptLine>();
	List<string> queue = new List<string>();
	Dictionary<int, Checkpoint> checkpoints = new Dictionary<int, Checkpoint>()
	{
		//PROLOGUE
		{ 0, new Checkpoint("", new float[] {0f, 0f, 0f} )}, //No Communicator
		{ 10, new Checkpoint("Explore the alien ship and find your room", new float[] {15.94f, 11.59f, 3.1f} )}, //Communicator and Quest bar added
		{ 11, new Checkpoint("Explore the alien ship and find your room", new float[] {15.94f, 11.59f, 3.1f } )}, //Epsilon finished talking, directed to open menu
		{ 12, new Checkpoint("Explore the alien ship and find your room", new float[] {15.94f, 11.59f, 3.1f } )}, //Menu described, directed to close menu
		{ 13, new Checkpoint("Explore the alien ship and find your room", new float[] {15.94f, 11.59f, 3.1f } )}, //Move added, instructed about overlay
		{ 14, new Checkpoint("Explore the alien ship and find your room", new float[] {15.94f, 11.59f, 3.1f } )}, //Jump added
		{ 15, new Checkpoint("Explore the alien ship and find your room", new float[] {15.94f, 11.59f, 3.1f } )}, //Instructed to talk to Tau
		{ 16, new Checkpoint("Explore the alien ship and find your room", new float[] {15.94f, 11.59f, 3.1f } )}, //Instructed to use Elevator
		{ 20, new Checkpoint("Decorate your room", new float[] {15.94f, 11.59f, 3.1f } )},
		{ 25, new Checkpoint("Decorate your room", new float[] {15.94f, 11.59f, 3.1f } )}, //Decorator tutorial delivered
		{ 29, new Checkpoint("Decorate your room", new float[] {15.94f, 11.59f, 3.1f } )}, //Decorator menu tutorial delivered, Exit state for decorator
		//CHAPTER 1
		{ 30, new Checkpoint("Give the photo to Delta", new float[] {15.94f, 13.5f, 12.98f} )},
		{ 40, new Checkpoint("Meet Delta in the Teleporter Room", new float[] {30f, 24.31f, 12.98f} )},
		{ 50, new Checkpoint("Use the Family Teleporter", new float[] {30f, 24.31f, 12.98f} )},
		{ 59, new Checkpoint("Use the Family Teleporter", new float[] { 30f, 24.31f, 12.98f} )}, //Exit state for teleporter 1
		{ 60, new Checkpoint("Meet Sammy", new float[] {15.94f, 2.83f, 12.98f} )},
		{ 70, new Checkpoint("Ask Epsilon how to talk to Sammy", new float[] {15.94f, 33.5f, 12.98f} )},
		{ 80, new Checkpoint("Take robot plan to Tau", new float[] { 19f, 24.31f, 12.98f} )},
		{ 90, new Checkpoint("Build robots", new float[] { 19f, 24.31f, 12.98f} )},
		{ 99, new Checkpoint("Build robots", new float[] { 19f, 24.31f, 12.98f} )}, //Exit state for factory 1
		{ 100, new Checkpoint("Build a Door Opener robot", new float[] { 19f, 24.31f, 12.98f} )},
		{ 109, new Checkpoint("Build a Door Opener robot", new float[] { 19f, 24.31f, 12.98f} )}, //Exit state for factory 2
		{ 110, new Checkpoint("Meet Sammy", new float[] {15.94f, 2.83f, 12.98f} )},
		{ 130, new Checkpoint("Talk to Sammy", new float[] {15.94f, 2.83f, 12.98f} )},
		{ 139, new Checkpoint("", new float[] {15.94f, 2.83f, 12.98f} )}, //Sammy is talking to Delta
		//LIGHTS UP TO L2
		{ 150, new Checkpoint("Talk to Delta", new float[] {15.94f, 13.5f, 12.98f} )},
		{ 160, new Checkpoint("Visit Epsilon", new float[] {15.94f, 33.5f, 12.98f} )},
		{ 170, new Checkpoint("Find Tau's logbook", new float[] {15.94f, 11.59f, 23.5f} )},
		{ 171, new Checkpoint("Find Tau's logbook", new float[] {15.94f, 11.59f, 23.5f } )}, //After mess discovered
		{ 180, new Checkpoint("Help Chi clean up", new float[] {15.94f, 11.59f, 15.25f} )},
		//CHAPTER 2
		{ 190, new Checkpoint("Ask Zeta about security recording", new float[] {15.94f, 13.5f, 12.98f} )},
		{ 200, new Checkpoint("Use the School Teleporter", new float[] { 30f, 24.31f, 12.98f} )},
		{ 209, new Checkpoint("Use the School Teleporter", new float[] { 30f, 24.31f, 12.98f} )}, //Exit state for teleporter 2
		{ 210, new Checkpoint("Talk to Zeta or Epsilon", new float[] {15.94f, 33.5f, 12.98f} )},
		{ 211, new Checkpoint("Ask Rho about the security cameras", new float[] {15.94f, 21f, 12.98f} )},
		{ 212, new Checkpoint("Return to Epsilon", new float[] {15.94f, 33.5f, 12.98f} )},
		{ 213, new Checkpoint("Talk to Zeta", new float[] {15.94f, 13.5f, 12.98f} )},
		{ 220, new Checkpoint("Ask Rho about the Need To Tell machine or visit Epsilon", new float[] {15.94f, 33.5f, 12.98f} )},
		{ 221, new Checkpoint("Ask Chi about lunch", new float[] {15.94f, 11.59f, 15.25f } )},
		{ 222, new Checkpoint("Return to Epsilon", new float[] {15.94f, 33.5f, 12.98f} )},
		{ 223, new Checkpoint("Ask Rho about the Need To Tell machine", new float[] {15.94f, 21f, 12.98f} )},
		{ 230, new Checkpoint("Use the Need To Tell Machine", new float[] {15.94f, 21f, 12.98f} )},
		{ 239, new Checkpoint("Use the Need To Tell Machine", new float[] {15.94f, 21f, 12.98f} )}, //Exit state for sorter 1
		{ 240, new Checkpoint("Visit Sammy in her room", new float[] {15.94f, 2.83f, 12.98f} )},
		{ 249, new Checkpoint("Find Sammy's favourite place", new float[] {0f, 0f, 0f } )}, //Temporary state, just mid-conversation that ends in state change to 250
		{ 250, new Checkpoint("Find Sammy's favourite place or build more robots", new float[] { 19f, 24.31f, 12.98f} )},
		{ 259, new Checkpoint("Find Sammy's favourite place or build more robots", new float[] { 19f, 24.31f, 12.98f} )}, //Exit state for factory 3
		{ 260, new Checkpoint("Find Sammy in the cargo bay", new float[] {3.5f, 24.31f, 12.98f} )},
		{ 270, new Checkpoint("Talk to Zeta", new float[] {15.94f, 13.5f, 12.98f} )},
		{ 271, new Checkpoint("Talk to Zeta", new float[] {0f, 0f, 0f} )}, //Zeta moved to bottom left corridor
		{ 272, new Checkpoint("Talk to Zeta", new float[] {0f, 0f, 0f} )}, //Zeta moved from lab
		//LIGHTS UP TO L3
		{ 280, new Checkpoint("Talk to Sammy", new float[] {3.5f, 24.31f, 12.98f} )},
		{ 290, new Checkpoint("Visit Sammy in her room", new float[] {15.94f, 2.83f, 12.98f} )},
		//CHAPTER 3
		{ 300, new Checkpoint("Return Tau's Logbook", new float[] { 19f, 24.31f, 12.98f} )},
		{ 301, new Checkpoint("Return Tau's Logbook", new float[] { 19f, 24.31f, 12.98f} )},
		{ 310, new Checkpoint("Use the third teleporter", new float[] { 30f, 24.31f, 12.98f} )},
		{ 319, new Checkpoint("Use the third teleporter", new float[] { 30f, 24.31f, 12.98f} )}, //Exit state for teleporter 3
		{ 320, new Checkpoint("Talk to Tau", new float[] { 19f, 24.31f, 12.98f} )},
		{ 330, new Checkpoint("Play the VR game with your trusted adult", new float[] {0f, 0f, 0f} )},
		{ 339, new Checkpoint("Play the VR game with your trusted adult", new float[] {0f, 0f, 0f} )}, //Exit state for vr game 1
		{ 340, new Checkpoint("Take the VR game to Sammy", new float[] {15.94f, 2.83f, 12.98f} )},
		{ 350, new Checkpoint("Use Sammy's Need To Tell machine", new float[] {15.94f, 21f, 12.98f} )},
		{ 359, new Checkpoint("Use Sammy's Need To Tell machine", new float[] {15.94f, 21f, 12.98f} )}, //Exit state for sorter 2
		{ 360, new Checkpoint("Talk to Sammy about the VR game", new float[] {15.94f, 2.83f, 12.98f} )},
		{ 369, new Checkpoint("", new float[] {15.94f, 2.83f, 12.98f} )}, //Sammy is talking to Tau
		//LIGHTS UP TO L4
		{ 370, new Checkpoint("Talk to Tau", new float[] {19f, 24.31f, 12.98f} )},
		{ 380, new Checkpoint("Take the unscrambler to Rho", new float[] {15.94f, 21f, 12.98f} )},
		//CHAPTER 4
		{ 390, new Checkpoint("Use the unscrambler", new float[] {15.94f, 21f, 12.98f} )},
		{ 399, new Checkpoint("Use the unscrambler", new float[] {15.94f, 21f, 12.98f} )}, //Exit state for unscrambler 1
		{ 400, new Checkpoint("Bring a fourth trusted adult aboard", new float[] { 30f, 24.31f, 12.98f} )},
		{ 409, new Checkpoint("Bring a fourth trusted adult aboard", new float[] { 30f, 24.31f, 12.98f} )}, //Exit state for teleporter 4
		{ 410, new Checkpoint("Talk to Rho", new float[] {15.94f, 21f, 12.98f} )},
		{ 420, new Checkpoint("Use the unscrambler", new float[] {15.94f, 21f, 12.98f} )},
		{ 429, new Checkpoint("Use the unscrambler", new float[] {15.94f, 21f, 12.98f} )}, //Exit state for unscrambler 2
		{ 430, new Checkpoint("Use the unscrambler again", new float[] {15.94f, 21f, 12.98f} )},
		{ 439, new Checkpoint("Use the unscrambler again", new float[] {15.94f, 21f, 12.98f} )}, //Exit state for unscrambler 3
		{ 440, new Checkpoint("Talk to Sammy", new float[] {15.94f, 2.83f, 12.98f} )},
		{ 450, new Checkpoint("Talk to Rho", new float[] {15.94f, 21f, 12.98f} )},
		//LIGHTS UP TO L5
		{ 460, new Checkpoint("Build Searcher robots", new float[] { 19f, 24.31f, 12.98f} )},
		{ 469, new Checkpoint("Wait for Searcher robots", new float[] { 19f, 24.31f, 12.98f} )}, //Exit state for factory 5
		{ 470, new Checkpoint("Take the lost recording to Rho", new float[] {15.94f, 21f, 12.98f} )},
		//CHAPTER 5
		{ 480, new Checkpoint("Ask Chi about the recording", new float[] {15.94f, 11.59f, 15.25f } )},
		{ 490, new Checkpoint("Bring a fifth trusted adult aboard", new float[] { 30f, 24.31f, 12.98f} )},
		{ 500, new Checkpoint("Build Fixer robots", new float[] { 19f, 24.31f, 12.98f} )},
		{ 509, new Checkpoint("Build Fixer robots", new float[] { 19f, 24.31f, 12.98f} )}, //Exit state for factory 5
		{ 510, new Checkpoint("Check the top floor", new float[] {15.94f, 11.59f, 0f} )},
		{ 520, new Checkpoint("Build Fixer robots properly", new float[] { 19f, 24.31f, 12.98f} )},
		{ 529, new Checkpoint("Build Fixer robots properly", new float[] { 19f, 24.31f, 12.98f} )}, //Exit state for factory 6
		{ 530, new Checkpoint("Bring a fifth trusted adult aboard", new float[] { 30f, 24.31f, 12.98f} )},
		{ 539, new Checkpoint("Bring a fifth trusted adult aboard", new float[] { 30f, 24.31f, 12.98f} )}, //Exit state for teleporter 5
		{ 540, new Checkpoint("Tell your trusted adult what happened", new float[] {0f, 0f, 0f} )},
		{ 550, new Checkpoint("It's scary, but tell your trusted adult what happened", new float[] {0f, 0f, 0f} )},
		{ 560, new Checkpoint("Talk to Tau", new float[] { 19f, 24.31f, 12.98f} )},
		{ 570, new Checkpoint("Find the red button near the teleporters", new float[] {0f, 0f, 0f} )},
		{ 580, new Checkpoint("Talk to Tau", new float[] { 19f, 24.31f, 12.98f} )},
		{ 590, new Checkpoint("Talk to Zeta", new float[] {3.5f, 24.31f, 12.98f} )},
		{ 600, new Checkpoint("Find the red button in the Cargo Bay", new float[] {3.5f, 24.31f, 12.98f} )},
		{ 610, new Checkpoint("Talk to Rho", new float[] {15.94f, 21f, 12.98f} )},
		{ 620, new Checkpoint("Try ALL the red buttons!", new float[] {15.94f, 21f, 12.98f} )},
		{ 630, new Checkpoint("Talk to Delta", new float[] {15.94f, 13.5f, 12.98f} )},
		{ 640, new Checkpoint("Find the red button in Sammy's room", new float[] {15.94f, 2.83f, 12.98f} )},
		{ 650, new Checkpoint("Talk to Chi", new float[] {15.94f, 11.59f, 15.25f } )},
		{ 660, new Checkpoint("Talk to your trusted adults", new float[] {0f, 0f, 0f} )},
		{ 670, new Checkpoint("Return to Earth", new float[] {0f, 0f, 0f} )},
		{ 680, new Checkpoint("", new float[] {0f, 0f, 0f} )}
	};
	bool ready = true;

	void Awake()
	{
		if (singleton == null)
		{
			/*
			GameController.SetInt("adult1voice", 1); //TODO: Testing only
			GameController.SetInt("adult2voice", 4); //TODO: Testing only
			GameController.SetInt("adult3voice", 2); //TODO: Testing only
			GameController.SetInt("adult4voice", 5); //TODO: Testing only
			GameController.SetInt("adult5voice", 3); //TODO: Testing only
			GameController.SetString("prevScene", "Teleporter"); //TODO: Testing only
			GameController.SetString("prevScene", "Teleporter"); //TODO: Testing only
			GameController.SetInt("Checkpoint", 0); //TODO: Testing only
			*/

			singleton = this;
			DontDestroyOnLoad(this);

			//SET UP THE OVERLAYS IN A DICTIONARY (because we need to access them even when inactive) AND DEACTIVATE THEM
			for (int i = 0; i < transform.childCount; i++)
			{
				//TODO: Test type instead?
				if (transform.GetChild(i).name != "EventSystem")
				{
					overlays[transform.GetChild(i).name] = transform.GetChild(i).gameObject;
					transform.GetChild(i).gameObject.SetActive(false);
				}
			}


			//DEFINE ALL LINES OF SCRIPT
			new ScriptLine("010_1", "epsilon", "Thanks for agreeing to come on board! We really need your help!");
			new ScriptLine("010_2", "epsilon", "The ship we're on is called Sammy. We'll meet her later.", "The ship we're on is called Sammy. We'll meet him later.");
			new ScriptLine("010_3", "epsilon", "You and I are going to be hard at work fixing all the problems with Sammy, so don't tell anyone else what we're doing.");
			new ScriptLine("010_4", "epsilon", "They'd only start worrying, and that would cause even more problems!");
			new ScriptLine("010_5", "epsilon", "I've got a present for you, since you've been so nice to come and help us.");
			new ScriptLine("010_6", "epsilon", "It's a communicator. You can call me at any time, day or night!");
			new ScriptLine("010_7", "epsilon", "But it's a secret, okay? I only have one, and the other crew members would be jealous if they knew you had it!");
			new ScriptLine("010_8", "epsilon", "If you need anything, just call me on the communicator, okay? Have fun!");
			new ScriptLine("011_1", "helpComm", "Press the communicator to open the menu. Try it now.");
			new ScriptLine("012_1", "helpShipMenuClose", "This is the menu. From here, you can exit the game, or play any of the minigames, once you unlock them!");
			new ScriptLine("012_2", "helpShipMenuClose", "For now, press the communicator again to go back to the game.");
			new ScriptLine("013_1", "helpGoal", "This is your goal. It tells you what you should be doing, right now.");
			new ScriptLine("013_2", "helpCompass", "And this is your goal compass. It tells you which way to go to reach your goal.");
			new ScriptLine("013_3", "helpMove", "To move, drag these arrows in the direction you want to go.");
			new ScriptLine("014_1", "helpJump", "To jump over the crate, press this jump button, while moving.");
			new ScriptLine("015_1", "helpTau", "You can talk to people by pressing them. Try talking to Tau.");
			new ScriptLine("016_1", "helpCompass", "When the goal compass turns into a circle, you're right where you need to be.");
			new ScriptLine("016_2", "helpCompass", "Stand in the elevator and press the 'Go Up' button, now."); //Better help location?
			new ScriptLine("020_1", "epsilonComm", "Here's your bedroom! Please decorate it however you want! No one can come in without your permission!");
			new ScriptLine("020_2", "child", "My own room on a spaceship? This is so cool!");
			new ScriptLine("020_3", "child", "I'll just unpack, then I can start helping Epsilon!");
			new ScriptLine("030_1", "child", "Hey! It's a photo of Delta and Sammy. But what's it doing in here?");
			new ScriptLine("030_2", "child", "I've gotta take this photo back to Delta. Although Epsilon did say not to bother anyone... I'll just see if she's busy!");
			new ScriptLine("040_1", "delta", "Oh, thank you so much! I've been looking everywhere for this photo, ever since Sammy locked herself away!", "Oh, thank you so much! I've been looking everywhere for this photo, ever since Sammy locked himself away!");
			new ScriptLine("040_2", "delta", "She didn't feel like talking, she wouldn't help with my experiments...", "He didn't feel like talking, he wouldn't help with my experiments...");
			new ScriptLine("040_3", "delta", "Maybe... Could you could talk to her for me? Please? I'd do anything to hear her voice again!", "Maybe... Could you could talk to him for me? Please? I'd do anything to hear his voice again!");
			new ScriptLine("040_4", "delta", "I've got some pod-goo here. You can use it to bring on board one trusted adult, someone who can help you talk to Sammy.");
			new ScriptLine("040_5", "delta", "I'll meet you in the teleporter room in just a sec!");
			new ScriptLine("050_1", "delta", "Here we are! Look at this wonderful teleporter. It can collect people from all over the galaxy!");
			new ScriptLine("050_2", "delta", "Put the pod goo on the family teleporter. It's the one with the \"home\" symbol on it.");
			new ScriptLine("060_1", "adult1", "All right, what can I do to help?");
			new ScriptLine("060_2", "child", "We need to find out if Sammy's okay. She's locked herself away in her room!");
			new ScriptLine("060_3", "adult1", "Then let's get to it!");
			new ScriptLine("070_1", "child", "She's not answering. Maybe she can't hear me!", "He's not answering. Maybe he can't hear me!");
			new ScriptLine("070_2", "adult1", "We need some more ideas...");
			new ScriptLine("070_3", "child", "Epsilon will know what to do!");
			new ScriptLine("080_1", "epsilon", "Hello again! Oh, I see you brought a trusted adult on board. Okay. Nice. How can I help?");
			new ScriptLine("080_2", "child", "We tried to talk to Sammy, but I don't think she can hear us. Do you have any ideas?", "We tried to talk to Sammy, but I don't think he can hear us. Do you have any ideas?");
			new ScriptLine("080_3", "epsilon", "Oh, don't worry about her. She told me she just wants to be alone right now.", "Oh, don't worry about him. He told me he just wants to be alone right now.");
			new ScriptLine("080_4", "adult1", "We'd still like to check on her, to make sure she's all right.", "We'd still like to check on him, to make sure he's all right.");
			new ScriptLine("080_5", "child", "Once she says she's fine I'll get straight to work!", "Once he says he's fine I'll get straight to work!");
			new ScriptLine("080_6", "epsilon", "Well... Okay. Here's a blueprint for a robot that'll open the door.");
			new ScriptLine("080_7", "child", "Thanks! I'll build it as fast as I can!");
			new ScriptLine("090_1", "tau", "Well hi there! Come to help me make robots, have you?");
			new ScriptLine("090_2", "child", "Actually, we're here to make a special robot, to help Sammy!");
			new ScriptLine("090_3", "tau", "First I need to show you how the machine works, so let's practice a little. Get ready to have some fun!");
			new ScriptLine("100_1", "tau", "Hey! That last robot was better than mine! You're definitely ready to follow the blueprints you gave me. Let's go!");
			new ScriptLine("110_1", "tau", "That's it, you've made a door opener robot! But are you sure that's what you want?");
			new ScriptLine("110_2", "child", "It's what Epsilon said to use...");
			new ScriptLine("110_3", "tau", "Just be careful, okay?");
			new ScriptLine("110_4", "child", "Okay!");
			new ScriptLine("120_1", "adult1", "This robot's strong enough to rip the door right off its hinges! We don't want to scare Sammy! Let's have it knock instead.");
			new ScriptLine("130_1", "adult1", "Look! She opened the door immediately! She must want us to go inside.", "Look! He opened the door immediately! He must want us to go inside.");
			new ScriptLine("140_1", "child", "Hello? Sammy?");
			new ScriptLine("140_2", "child", "Are you okay? Everyone's worried about you.");
			new ScriptLine("140_3", "child", "Especially Delta! She thinks you're mad at her, and she doesn't know what to do.");
			new ScriptLine("140_4", "adult1", "It can be really hard when someone you trust hurts you, but the only way to feel better is to talk to them about it.");
			new ScriptLine("140_5", "child", "Do you think you can try to talk to Delta? Please?");
			new ScriptLine("140_6", "adult1", "Let's leave her alone to make her decision.", "Let's leave him alone to make his decision.");
			new ScriptLine("140_7", "child", "Okay... I'll come and see you again soon, Sammy!");
			new ScriptLine("150_1", "child", "Hey, the lights are back on!");
			new ScriptLine("150_2", "adult1", "Sammy must be feeling better!");
			new ScriptLine("150_3", "child", "Let's go ask Delta!");
			new ScriptLine("160_1", "delta", "Sammy came to see me! She didn't say anything, and she didn't stay long, but it's a start!", "Sammy came to see me! He didn't say anything, and he didn't stay long, but it's a start!");
			new ScriptLine("160_2", "delta", "I'm trying to think of ways to show her we're all here for her! If you come up with any ideas, let me know!", "I'm trying to think of ways to show him we're all here for him! If you come up with any ideas, let me know!");
			new ScriptLine("160_3", "child", "Sure!");
			new ScriptLine("160_4", "epsilonComm", "Hey! Can you come see me in the navigator's room?");
			new ScriptLine("170_1", "epsilon", "Tau says his logbook has gone missing! That's why nothing's getting fixed around here!");
			new ScriptLine("170_2", "epsilon", "Last time he just left it in the bathroom! Can you check he hasn't left it there again?");
			new ScriptLine("171_1", "child", "What a mess! There must be a malfunction.");
			new ScriptLine("180_1", "child", "No logbook in here... Where else could it be? In the meantime, I guess I could help Chi clean up.");
			new ScriptLine("190_1", "child", "Hey, Chi! Do you want a hand?");
			new ScriptLine("190_2", "child", "What's this mixed in with the garbage? It's got \"Security Recording #587 - Zeta\" written on it!");
			new ScriptLine("190_3", "chi", "That's one of Rho's security tapes! Who would throw it away ?");
			new ScriptLine("190_4", "adult1", "And what are they trying to hide?");
			new ScriptLine("200_1", "zeta", "Oh my, a security recording with my name on it? I have no idea! Let's see...");
			new ScriptLine("200_2", "zeta", "Well, there's my vacuum cleaner in the video, but... goodness, someone's reprogramming it!");
			new ScriptLine("200_3", "zeta", "No wonder it exploded! I lost my voice for a week from all the dust!");
			new ScriptLine("200_4", "zeta", "And of course, that's when Sammy came to see me, but I couldn't comfort her, because I couldn't talk!", "And of course, that's when Sammy came to see me, but I couldn't comfort him, because I couldn't talk!");
			new ScriptLine("200_5", "zeta", "I found this tube of pod goo on my cleaning rounds! I thought you might like to invite another trusted adult onto the ship!");
			new ScriptLine("210_1", "adult2", "Hi there! You said Zeta needed help? Let's get going!");
			new ScriptLine("211_1", "epsilon", "Can you ask Rho if she's polished all the ship's camera lenses? Safety is our top priority!");
			new ScriptLine("212_1", "rho", "Zeta polished all the lenses this morning. All the cameras are working. The ship is safe. Let Epsilon know.");
			new ScriptLine("213_1", "epsilon", "Well that's Rho for you! She can be a bit abrupt, but don't take it personally.");
			new ScriptLine("220_1", "zeta", "Ooh, I'm so glad you're here! I'm completely out of ideas! How would you communicate with Sammy?");
			new ScriptLine("220_2", "adult2", "We could try calling her or sending her an email. We could even write her a letter and slip it under her door.", "We could try calling her or sending him an email. We could even write him a letter and slip it under his door.");
			new ScriptLine("220_3", "zeta", "Oh my. I've been sending her messages every day, but she hasn't answered a single one of them!", "Oh my. I've been sending him messages every day, but he hasn't answered a single one of them!");
			new ScriptLine("220_4", "zeta", "I wonder if there's something wrong with Sammy's Need to Tell machine.");
			new ScriptLine("220_5", "child", "How can we find out?");
			new ScriptLine("220_6", "zeta", "Rho designed it! She'll know!");
			new ScriptLine("221_1", "epsilon", "I'm so glad you're here! Could you ask Chi what time lunch is, today? I'm starving!");
			new ScriptLine("222_1", "chi", "Hey there, poquito! Lunch is at 12:30. Same time it is every day.");
			new ScriptLine("222_2", "chi", "But if you're hungry now, go grab a snack from the fridge.");
			new ScriptLine("223_1", "epsilon", "Oh of course, it's 12:30. How could I forget? It's just that Chi is really strict about snacking between meals.");
			new ScriptLine("230_1", "rho", "Need to check Sammy's Need to Tell Machine? Go ahead.");
			new ScriptLine("230_2", "rho", "It's one of the ways Sammy would tell us about important things. Not any more.");
			new ScriptLine("230_3", "rho", "Used to help her, when she shared with us.Now she's stopped, it's harder to protect her.", "Used to help him, when he shared with us. Now he's stopped, it's harder to protect him.");
			new ScriptLine("230_4", "rho", "System might need some retraining. Maybe some crossed wires. Help Sammy communicate again.");
			new ScriptLine("240_1", "rho", "Good work. Sammy's Need to Tell Machine is functioning again.");
			new ScriptLine("240_2", "rho", "Send her a new message, to test it.", "Send him a new message, to test it.");
			new ScriptLine("240_3", "child", "Hmmm, what'll I write? I know...");
			new ScriptLine("240_4", "child", "Hi, Sammy! I hope you're doing okay. Everyone's worried about you. Let's talk soon, all right?");
			new ScriptLine("240_5", "rho", "Hope Sammy reads her messages soon.", "Hope Sammy reads his messages soon.");
			new ScriptLine("240_6", "adult2", "If she doesn't reply, we can also call her again, or go see her in her room.", "If he doesn't reply, we can also call him again, or go see him in his room.");
			new ScriptLine("240_7", "child", "And there's always text messaging or writing a letter! We have lots of ways to contact Sammy!");
			new ScriptLine("250_1", "child", "Sammy? I wanted to see how you were feeling. I hope you don't mind.");
			new ScriptLine("250_2", "child", "I don't think Sammy feels safe enough to talk.");
			new ScriptLine("250_3", "adult2", "Maybe Zeta can help her, if we can find somewhere else that makes Sammy more comfortable.", "Maybe Zeta can help him, if we can find somewhere else that makes Sammy more comfortable.");
			new ScriptLine("250_4", "adult2", "Is there somewhere else on the ship where she spends a lot of time?", "Is there somewhere else on the ship where he spends a lot of time?");
			new ScriptLine("250_5", "child", "Let's go look!");
			new ScriptLine("250_6", "epsilonComm", "There you are! Could you do me a huge favour and build another one of those robots? You did such a great job last time!");
			new ScriptLine("250_7", "child", "Sorry, I'm trying to find Sammy right now. I can build the robot as soon as I've found her!", "Sorry, I'm trying to find Sammy right now. I can build the robot as soon as I've found him!");
			new ScriptLine("250_8", "epsilonComm", "Oh. Okay. I really need your help, but if you're too busy...");
			new ScriptLine("260_1", "tau", "Well done!");
			new ScriptLine("260_2", "child", "Now can I go and find Sammy?");
			new ScriptLine("260_3", "tau", "If it's Sammy you're looking for, Pi just came back from the Cargo Bay really excited.");
			new ScriptLine("260_4", "tau", "It's the way he acts when he's been spending time with one of his favourite people. Maybe he saw Sammy!");
			new ScriptLine("270_1", "child", "Hey, Sammy! It's great to see you again!");
			new ScriptLine("270_2", "adult2", "Zeta's really worried about you. Can we ask her to meet you here?");
			new ScriptLine("270_3", "child", "If you want to talk, just stay here! We'll go get her!");
			new ScriptLine("280_1", "zeta", "Sammy's waiting for me in the cargo bay? I'll go right this second!");
			new ScriptLine("280_2", "zeta", "Sammy said I wouldn't understand why she stopped talking.", "Sammy said I wouldn't understand why he stopped talking.");
			new ScriptLine("280_3", "zeta", "I'm just glad she's talking to me again! We have a chance to make things better, now. Just you wait and see!", "I'm just glad he's talking to me again! We have a chance to make things better, now. Just you wait and see!");
			new ScriptLine("280_4", "zeta", "She asked to speak with you, too.", "He asked to speak with you, too.");
			new ScriptLine("290_1", "child", "Sammy! You're talking again!");
			new ScriptLine("290_2", "sammy", "Yes, thanks to you. I still don't know what to say, but... It felt better to talk to Zeta again.");
			new ScriptLine("290_3", "sammy", "Would you... Would you do me another favour? Will you meet me in my room?");
			new ScriptLine("290_4", "child", "Of course!");
			new ScriptLine("300_1", "sammy", "Inside the cupboard is something very precious. It's Tau's logbook.");
			new ScriptLine("300_2", "sammy", "I don't know how it got in there, but I know he'd want it back.");
			new ScriptLine("300_3", "child", "I've been looking for it, too! Thanks!");
			new ScriptLine("300_4", "sammy", "No, thank you, for all your help. And I'm sorry, but please...");
			new ScriptLine("300_5", "sammy", "Please tell Tau I never want to see him again.");
			new ScriptLine("300_6", "child", "Sammy...");
			new ScriptLine("300_7", "sammy", "Just tell Tau that I don't want to see him when you give him his log book back. Will you do that for me? Please?");
			new ScriptLine("300_8", "child", "I'll see what I can do.");
			new ScriptLine("310_1", "tau", "You found it! Thanks! It brings back so many memories...");
			new ScriptLine("310_2", "tau", "Sammy used talk to me when she was sad or confused, even if she didn't think I would understand.", "Sammy used talk to me when he was sad or confused, even if he didn't think I would understand.");
			new ScriptLine("310_3", "tau", "But one day she was so sad, and shortly after that, she stopped visiting.", "But one day he was so sad, and shortly after that, he stopped visiting.");
			new ScriptLine("310_4", "tau", "It sounds like you've got some wise trusted adults in your life. People who always help you out!");
			new ScriptLine("310_5", "tau", "Listen, I found some pod-goo the other day. Could you bring another trusted adult on board?");
			new ScriptLine("310_6", "tau", "Try to think of someone who's not from your family or your school. That way, you'll always have someone you can rely on!");
			new ScriptLine("320_1", "adult3", "Hey!");
			new ScriptLine("320_2", "child", "Wait until I introduce you to Tau! He'll want to meet you for sure!");
			new ScriptLine("330_1", "tau", "This must be your trusted adult! Nice to meet you!");
			new ScriptLine("330_2", "adult3", "Lovely to be here!");
			new ScriptLine("330_3", "tau", "Pi just brought me this pair of VR goggles. He must have found them in the Cargo Bay when he was visiting Sammy.");
			new ScriptLine("330_4", "tau", "It looks like there's one set for a child, and one for an adult. Would you two play the game and tell me what it's about?");
			new ScriptLine("330_5", "adult3", "Absolutely! Just hand me a pair when you're ready.");
			new ScriptLine("340_1", "child", "I've been thinking... Maybe the game was brought onboard for Sammy to play! She's the only other kid on the ship.", "I've been thinking... Maybe the game was brought onboard for Sammy to play! He's the only other kid on the ship.");
			new ScriptLine("340_2", "adult3", "Good thinking! Let's take it to her.", "Good thinking! Let's take it to him.");
			new ScriptLine("350_1", "sammy", "Hey! What have you got there?");
			new ScriptLine("350_2", "child", "Pi found this game in the cargo bay! We think it's for you!");
			new ScriptLine("350_3", "adult3", "It's designed to help young people overcome their fears about what might happen if they share \"Need to Tell\" situations from their lives.");
			new ScriptLine("350_4", "child", "It's awesome! You should play it!");
			new ScriptLine("350_5", "sammy", "Sure! Oh, and I meant to thank you for retraining my \"Need to Tell\" machine.");
			new ScriptLine("350_6", "sammy", "It's working a lot better, but I could still use some help. Do you have time to run a couple more levels with Rho?");
			new ScriptLine("350_7", "child", "Of course! It'll be fun!");
			new ScriptLine("360_1", "rho", "Good work. The system can always use more retraining.");
			new ScriptLine("360_2", "adult3", "How do you think Sammy's going with the Speak Up game?"); //TODO: crop "Speak Up"? using "VR game" everywhere else.
			new ScriptLine("360_3", "child", "I bet she's way ahead already! Let's go see!", "I bet he's way ahead already! Let's go see!");
			new ScriptLine("370_1", "sammy", "After playing that game, I think I am ready to talk to Tau again.");
			new ScriptLine("370_2", "child", "It looks like even more lights came on!");
			new ScriptLine("370_3", "adult3", "Talking to Tau must have made Sammy a lot happier!");
			new ScriptLine("380_1", "tau", "Sammy told me how she feels, which is a great start...", "Sammy told me how he feels, which is a great start...");
			new ScriptLine("380_2", "tau", "But she's still worried no one will believe her if she tells us what happened!", "But he's still worried no one will believe him if he tells us what happened!");
			new ScriptLine("380_3", "tau", "I don't know what happened, but I do know this: whoever hurt Sammy is still on board this ship.");
			new ScriptLine("380_4", "tau", "And that means one of us has been lying to you all along.");
			new ScriptLine("380_5", "tau", "Please take this video unscrambler to Rho. It'll check all our security videos for any footage that's been tampered with.");
			new ScriptLine("390_1", "rho", "Tau made the video unscrambler. Good. Let's run it now.");
			new ScriptLine("390_2", "rho", "Three videos have been changed and two are missing.");
			new ScriptLine("390_3", "rho", "Recall missing video tape of Zeta with her vacuum cleaner.");
			new ScriptLine("390_4", "rho", "One recording found, one to go. Let me know if you find it anywhere. For now, let's unscramble what we have.");
			new ScriptLine("400_1", "rho", "I need you to do something for me - for both of us.");
			new ScriptLine("400_2", "rho", "Take this pod goo and beam on board someone you trust, someone older, who always believes you when you tell the truth.");
			new ScriptLine("410_1", "adult4", "Here I am! Let's go talk to Rho and see how we can help!");
			new ScriptLine("420_1", "rho", "This is terrible! Someone Sammy trusted went into her private space and locked the door so no one else could see what they were doing!", "This is terrible! Someone Sammy trusted went into his private space and locked the door so no one else could see what they were doing!");
			new ScriptLine("420_2", "rho", "Who would do that?");
			new ScriptLine("420_3", "adult4", "We don't know yet.");
			new ScriptLine("430_1", "rho", "I should have believed her. The sooner I know everything, the sooner I can make sure Sammy knows she's not alone!", "I should have believed him. The sooner I know everything, the sooner I can make sure Sammy knows he's not alone!");
			new ScriptLine("440_1", "rho", "She tried emailing me, but when I didn't respond, she gave up. Why didn't she try to talk to Tau, or Delta?", "He tried emailing me, but when I didn't respond, he gave up. Why didn't he try to talk to Tau, or Delta?");
			new ScriptLine("440_2", "child", "She told Tau she was afraid of getting in trouble. Let's make sure she knows that's not going to happen!", "He told Tau he was afraid of getting in trouble. Let's make sure he knows that's not going to happen!");
			new ScriptLine("440_3", "rho", "You're right.");
			new ScriptLine("450_1", "sammy", "I really like this game. It makes me happy when the children tell their trusted adults about what's been happening, even though it's hard for them!");
			new ScriptLine("450_2", "child", "I liked that they kept telling, even when the adults didn't believe them. One of them even called the Kids Help Line!");
			new ScriptLine("450_3", "sammy", "I tried to tell, but... Maybe I gave up too soon. My crew members care about me. I know that now.");
			new ScriptLine("450_4", "sammy", "I think I'm ready to try telling again. I'm scared, but... I have to try.");
			new ScriptLine("450_5", "child", "More lights came on! It's like a completely different ship!");
			new ScriptLine("450_6", "adult4", "Sammy's talk with Rho must have gone well.");
			new ScriptLine("450_7", "child", "Yeah! Let's go see Rho!");
			new ScriptLine("460_1", "rho", "Sammy spoke to me! She told me what happened, and I could see it was hard for her, but I'm so proud of her. She's so brave.", "Sammy spoke to me! He told me what happened, and I could see it was hard for him, but I'm so proud of him. He's so brave.");
			new ScriptLine("460_2", "rho", "Unfortunately she still won't tell me who it was. She's worried she might get in trouble!", "Unfortunately he still won't tell me who it was. He's worried he might get in trouble!");
			new ScriptLine("460_3", "rho", "She's so confused! But what happened to her wasn't her fault. I'll keep telling her that until she knows it's the truth!", "He's so confused! But what happened to him wasn't his fault. I'll keep telling him that until he knows it's the truth!");
			new ScriptLine("460_4", "rho", "In the meantime, finding that missing piece of footage will definitely help. Here, I found a blueprint for searcher robots.");
			new ScriptLine("470_1", "tau", "The searcher robots are on the job! We'll know as soon as they find something!");
			new ScriptLine("470_2", "tau", "Hey! That was fast! Here, quickly - take this to Rho!");
			new ScriptLine("480_1", "rho", "You found it! Let's see what's on it, and why someone tried to hide it! There! Listen!");
			new ScriptLine("480_2", "epsilonUnscrambler", "You know Chi only talks to you because I ask him to.");
			new ScriptLine("480_3", "epsilonUnscrambler", "He even tried to transfer to another ship, but I wouldn't let him go, because I know how much he means to you.");
			new ScriptLine("480_4", "rho", "That's... Epsilon! But why would he lie? Can you take this tape to Chi, and ask him if it's true?");
			new ScriptLine("490_1", "chi", "Why would Rho want me to watch a security video? Is it something to do with Sammy?");
			new ScriptLine("490_2", "epsilonUnscrambler", "You know Chi only talks to you because I ask him to. He even tried to transfer to another ship.");
			new ScriptLine("490_3", "chi", "Wh-what? Epsilon is wrong. He's wrong! Sammy's like a daughter to me!", "Wh-what? Epsilon is wrong. He's wrong! Sammy's like a son to me!");
			new ScriptLine("490_4", "chi", "Whenever I'm not sleeping, I'm in this kitchen, cooking! Sammy was the one who kept me company.");
			new ScriptLine("490_5", "chi", "She always brought me new recipes she'd found, and she encouraged me to try new things!", "He always brought me new recipes he'd found, and he encouraged me to try new things!");
			new ScriptLine("490_6", "chi", "Then suddenly she was gone, and my meals have tasted awful ever since.", "Then suddenly he was gone, and my meals have tasted awful ever since.");
			new ScriptLine("490_7", "chi", "I need you to take this pod goo and port in an adult you trust, someone who would always stand up for you, no matter what.");
			new ScriptLine("500_1", "epsilon", "There's something really wrong with the ship! We need as many fixer robots as you can make, right now!");
			new ScriptLine("500_2", "epsilon", "Here are the blueprints! Go!");
			new ScriptLine("510_1", "tau", "Huh? What was that? Quick, let's check that everyone's okay!");
			new ScriptLine("510_2", "tau", "You check the top floor! I'll check down here!");
			new ScriptLine("520_1", "child", "That's weird - the elevator isn't working!");
			new ScriptLine("520_2", "epsilonComm", "What have you done? You built the wrong kind of robots and now they're wrecking the ship!");
			new ScriptLine("520_3", "child", "But I followed the plans you gave me!");
			new ScriptLine("520_4", "epsilonComm", "You must have mixed up one of the parts!");
			new ScriptLine("520_5", "epsilonComm", "I won't tell the others what you've done, but you need to go make more robots, or the ship's going to be in big trouble!");
			new ScriptLine("520_6", "child", "I'll go right now! I'll make the right robots this time, I promise!");
			new ScriptLine("530_1", "epsilonComm", "Are you trying to destroy the ship? If the others found out what you've done they'd never talk to you again!");
			new ScriptLine("530_2", "child", "If I tell anyone what I did, they'll get mad at me! But I have to tell someone, or the ship might explode!");
			new ScriptLine("530_3", "child", "That's right! Chi told me to teleport in someone who would always stand up for me! I definitely need that person now!");
			new ScriptLine("540_1", "adult5", "What's wrong? Remember, you can tell me anything.");
			new ScriptLine("540_2", "child", "I don't know if I can...");
			new ScriptLine("550_1", "child", "If I tell, I might get into trouble!");
			new ScriptLine("560_1", "child", "Epsilon asked me to build some robots, but I built them wrong and now they're wrecking the ship and everyone's in danger...");
			new ScriptLine("560_2", "adult5", "Let me see that blueprint... These are constructor robots! They're doing exactly what they're meant to do!");
			new ScriptLine("560_3", "adult5", "Epsilon knows that, and he's making you feel bad when it isn't your fault at all!");
			new ScriptLine("560_4", "adult5", "Let's go see Tau. He'll know what we can do to stop the robots.");
			new ScriptLine("570_1", "child", "The door won't open! We're trapped! Sammy? I think I did something bad...");
			new ScriptLine("570_2", "adult5", "Not at all! Epsilon told you to build the wrong kind of robots and now they're causing the doors and the elevators to malfunction!");
			new ScriptLine("570_3", "sammy", "If you were given the wrong blueprints, you couldn't have known what was going to happen! It's not your fault, but I'm glad you told someone.");
			new ScriptLine("570_4", "sammy", "I think it's time for me to tell someone what happened, too. I want to tell Chi and the others, but... I'm afraid. Will you go with me?");
			new ScriptLine("570_5", "child", "But we're stuck!");
			new ScriptLine("570_6", "adult5", "Sammy, can you stop the robots?");
			new ScriptLine("570_7", "sammy", "Of course! There, they've stopped. I'll activate the emergency door release system.");
			new ScriptLine("570_8", "sammy", "Throughout the ship you'll find red buttons. Pressing these buttons should re-activate the doors and elevators.");
			new ScriptLine("570_9", "adult5", "Let's find that red button so we can get out of here!");
			new ScriptLine("580_1", "adult5", "Got it?");
			new ScriptLine("580_2", "child", "Got it!");
			new ScriptLine("590_1", "child", "Sammy! We made it!");
			new ScriptLine("590_2", "adult5", "Sammy? Are you ready to tell? We're here for you.");
			new ScriptLine("590_3", "adult5", "I'm glad that Sammy has decided to tell her trusted adults that Epsilon was abusing her. Now they can keep her safe.", "I'm glad that Sammy has decided to tell his trusted adults that Epsilon was abusing him. Now they can keep him safe.");
			new ScriptLine("590_4", "adult5", "Remember, if you ever want to talk about anything, big or small, you can always talk to me, and your other trusted adults, too.");
			new ScriptLine("590_5", "child", "Thanks. I know that now.");
			new ScriptLine("590_6", "sammy", "When I told Tau, he understood and said he would help keep me safe.");
			new ScriptLine("590_7", "sammy", "I was scared that when I told people, they might not understand. I'd like to talk to Zeta now. I think she's in the cargo bay.");
			new ScriptLine("600_1", "child", "Zeta's a really good listener. I'm glad Sammy has someone like that to talk to.");
			new ScriptLine("600_2", "adult5", "Some things can be very hard to say. But the things that are really hard to talk about are the things we definitely need to discuss.");
			new ScriptLine("600_3", "sammy", "When I told Zeta, she listened and said she would help keep me safe.");
			new ScriptLine("600_4", "sammy", "It made it easier to know that if someone didn't believe me, I could always tell another trusted adult. I'd like to talk to Rho now.");
			new ScriptLine("600_5", "child", "I don't know how to get to her. The elevator isn't working.");
			new ScriptLine("600_6", "adult5", "Then let's find the red button!");
			new ScriptLine("610_1", "adult5", "Did you find it?");
			new ScriptLine("610_2", "child", "Yup!");
			new ScriptLine("620_1", "child", "Sammy was so worried Rho wouldn't believe Epsilon was abusing her that she felt like she couldn't say anything to anyone!", "Sammy was so worried Rho wouldn't believe Epsilon was abusing him that he felt like he couldn't say anything to anyone!");
			new ScriptLine("620_2", "adult5", "Sometimes things can seem that way. We get worried about all the things that could go wrong.");
			new ScriptLine("620_3", "adult5", "But that's exactly why we need to talk about the things that worry us, or confuse us.");
			new ScriptLine("620_4", "child", "Sammy knows that now."); //or child?
			new ScriptLine("620_5", "sammy", "Yes, I do. When I told Rho, she believed me, and said she would help keep me safe.");
			new ScriptLine("620_6", "sammy", "I'd like to talk to Delta next.");
			new ScriptLine("620_7", "adult5", "There are so many red buttons in here! Which one unlocks the door?");
			new ScriptLine("620_8", "child", "I'll find it!");
			new ScriptLine("630_1", "child", "This looks like the right button!");
			new ScriptLine("630_2", "adult5", "Give it a try!");
			new ScriptLine("640_1", "child", "I'm so glad Sammy decided to tell Delta that she was being abused. I know she was worried that Delta wouldn't be there for her.", "I'm so glad Sammy decided to tell Delta that he was being abused. I know he was worried that Delta wouldn't be there for him.");
			new ScriptLine("640_2", "adult5", "She's there for her now! And even if Delta's not around, Sammy has other trusted adults she can talk to, as well.", "She's there for him now! And even if Delta's not around, Sammy has other trusted adults he can talk to, as well.");
			new ScriptLine("640_3", "adult5", "That's why it's a good idea to have trusted adults at home, school, and other places! Someone will always be around!");
			new ScriptLine("640_4", "sammy", "When I told Delta, she was there for me and said she would help keep me safe.");
			new ScriptLine("640_5", "sammy", "I've been really worried that when I told people, they might not stand up for me. I'd like to talk to Chi now.");
			new ScriptLine("640_6", "adult5", "About the red button to reactivate the elevator...");
			new ScriptLine("640_7", "sammy", "I think it's in my room.");
			new ScriptLine("640_8", "child", "Let's go!");
			//Both lines at 650 are reused lines
			new ScriptLine("660_1", "child", "Sammy was worried that Chi wouldn't stand up for her.", "Sammy was worried that Chi wouldn't stand up for him.");
			new ScriptLine("660_2", "adult5", "I can understand that, especially after what Epsilon told her. But we know Sammy doesn't need to worry at all!", "I can understand that, especially after what Epsilon told him. But we know Sammy doesn't need to worry at all!");
			new ScriptLine("660_3", "sammy", "When I told Chi, he said he would always stand up for me and help keep me safe. He never tried to transfer off the ship at all!");
			new ScriptLine("660_4", "child", "So why did Epsilon lie?");
			new ScriptLine("660_5", "adult5", "Some adults will lie to children to keep them from telling anyone else about what's happening.");
			new ScriptLine("660_6", "child", "But Epsilon seemed so nice!");
			new ScriptLine("660_7", "sammy", "Yes, everyone thought he was nice. I did, too.");
			new ScriptLine("660_8", "adult5", "That's why it's important to tell your trusted adults about what's going on in your life.");
			new ScriptLine("660_9", "child", "And to have five trusted adults from as many different places as you can!");
			new ScriptLine("670_1", "child", "Sammy's told all of her trusted adults that Epsilon was abusing her by breaking the body rules.", "Sammy's told all of his trusted adults that Epsilon was abusing her by breaking the body rules.");
			new ScriptLine("670_2", "adult5", "Now that she's told them, they'll do their best to keep her safe.", "Now that she's told them, they'll do their best to keep him safe.");
			new ScriptLine("670_3", "child", "I think Sammy was worried that she might get Epsilon in trouble.", "I think Sammy was worried that he might get Epsilon in trouble.");
			new ScriptLine("670_4", "adult4", "The most important thing is that Sammy is safe. It's not Sammy's fault if Epsilon gets in trouble - he knew he was breaking the law.");
			new ScriptLine("670_5", "child", "Are you sure Sammy isn't going to get in trouble, too?");
			new ScriptLine("670_6", "adult3", "Some adults will get upset when they find out someone has been abusing a child they care about, but it's never the child's fault.");
			new ScriptLine("670_7", "child", "And it's always okay to tell your trusted adults about it!");
			new ScriptLine("670_8", "child", "I guess it's time to go home, now. Will you be okay, Sammy?");
			new ScriptLine("670_9", "sammy", "I think so. Now I know I can talk to my trusted adults about anything. They're going to help make sure Epsilon can't abuse me anymore.");
			new ScriptLine("670_10", "adult2", "You may have to tell other people your story, too. People who can help you.");
			new ScriptLine("670_11", "sammy", "I know. But I also know I've got trusted adults I can rely on. As long as I tell them what's wrong, together we can fix any kind of problem.");
			new ScriptLine("670_12", "adult1", "I know it's been hard, but you've come such a long way, Sammy!");
			new ScriptLine("670_13", "child", "I think you're really brave!");
			new ScriptLine("670_14", "sammy", "Thank you. I wanted to thank all of you for all that you've done, and for helping me reconnect with my trusted adults.");
			new ScriptLine("670_15", "child", "You've helped me connect with mine, too! I'll miss you, Sammy.");
			new ScriptLine("670_16", "sammy", "I'll miss you, too. But remember, I'm just a teleporter ride away!");
			new ScriptLine("680_1", "adult1", "You did great!");
			new ScriptLine("680_2", "adult2", "Thanks to you, Sammy and her crew have a chance to make things better.", "Thanks to you, Sammy and his crew have a chance to make things better.");
			new ScriptLine("680_3", "adult3", "And you know, we'll always be there for you, too.");
			new ScriptLine("680_4", "adult4", "That's right! If you ever have any problems, anything at all, you can always tell your trusted adults!");
			new ScriptLine("680_5", "adult5", "Come and see us any time!");
			new ScriptLine("680_6", "child", "Thank you, all of you. Now if I'm ever in trouble, I'll know who I can rely on!");
			new ScriptLine("680_7", "child", "Sammy taught me that things can only get better if you just have the courage to tell someone what's wrong!");

			new ScriptLine("adult_knock", "currentAdult", "The door is closed. We'd better knock.");

			//IDLE LINES

			new ScriptLine("adult_idle_01|1", "adult1", "Keep up the good work!");
			new ScriptLine("adult_idle_02|1", "adult1", "You're really helping out around here!");
			new ScriptLine("adult_idle_03|1", "adult1", "It's so great we could come onboard!");
			new ScriptLine("adult_idle_04|1", "adult1", "How's it going?");
			new ScriptLine("adult_idle_01|2", "adult2", "Keep up the good work!");
			new ScriptLine("adult_idle_02|2", "adult2", "You're really helping out around here!");
			new ScriptLine("adult_idle_03|2", "adult2", "It's so great we could come onboard!");
			new ScriptLine("adult_idle_04|2", "adult2", "How's it going?");
			new ScriptLine("adult_idle_01|3", "adult3", "Keep up the good work!");
			new ScriptLine("adult_idle_02|3", "adult3", "You're really helping out around here!");
			new ScriptLine("adult_idle_03|3", "adult3", "It's so great we could come onboard!");
			new ScriptLine("adult_idle_04|3", "adult3", "How's it going?");
			new ScriptLine("adult_idle_01|4", "adult4", "Keep up the good work!");
			new ScriptLine("adult_idle_02|4", "adult4", "You're really helping out around here!");
			new ScriptLine("adult_idle_03|4", "adult4", "It's so great we could come onboard!");
			new ScriptLine("adult_idle_04|4", "adult4", "How's it going?");
			new ScriptLine("adult_idle_01|5", "adult5", "Keep up the good work!");
			new ScriptLine("adult_idle_02|5", "adult5", "You're really helping out around here!");
			new ScriptLine("adult_idle_03|5", "adult5", "It's so great we could come onboard!");
			new ScriptLine("adult_idle_04|5", "adult5", "How's it going?");

			new ScriptLine("chi_idle_01", "chi", "Baking is science for hungry people!");
			new ScriptLine("chi_idle_02", "chi", "If you pour salt into a glass of water, the water level will go down instead of up!");
			new ScriptLine("chi_idle_03", "chi", "Honey never goes off!");
			new ScriptLine("chi_idle_04", "chi", "Fish scales are a common lipstick ingredient! Yuck!");
			new ScriptLine("chi_idle_05", "chi", "Fresh eggs sink in water, but bad eggs will float!");
			new ScriptLine("chi_idle_06", "chi", "You can make rubber bands last longer by storing them in the fridge!");
			new ScriptLine("chi_idle_07", "chi", "Corn always grows with an even number of ears!");
			new ScriptLine("chi_idle_08", "chi", "Peanuts can be used to make dynamite!");
			new ScriptLine("chi_idle_09", "chi", "It takes about 250 licks to get to the centre of a lollypop!");
			new ScriptLine("chi_idle_10", "chi", "Hard sugar candies will emit light if you chew them in the dark!");

			new ScriptLine("delta_idle_01", "delta", "Mars is red because its soil has a lot of rust in it!");
			new ScriptLine("delta_idle_02", "delta", "Proxima Centauri is the closest star to your home planet of Earth! Apart from the sun, of course!");
			new ScriptLine("delta_idle_03", "delta", "The light from the stars in the sky takes so long to reach us that looking at them is like time travel!");
			new ScriptLine("delta_idle_04", "delta", "The star \"Lucy\" in constellation Centaurus is actually a huge cosmic diamond of 10 billion trillion trillion carats!");
			new ScriptLine("delta_idle_05", "delta", "Comets are made of ice, dust and small rocky particles!");
			new ScriptLine("delta_idle_06", "delta", "A neutron star is the strongest magnet in the universe!");
			new ScriptLine("delta_idle_07", "delta", "A new star is born in the Milky Way every 18 days!");
			new ScriptLine("delta_idle_08", "delta", "You Earthlings are so lucky! You get to watch the Perseid Meteor Shower in August every year!");
			new ScriptLine("delta_idle_09", "delta", "One of the stars in your Southern Cross constellation is called Delta! I'm flattered!");
			new ScriptLine("delta_idle_10", "delta", "Jupiter's moon Europa has an atmosphere mostly comprised of oxygen!");

			new ScriptLine("epsilon_idle_01", "epsilon", "When following other space crafts, make sure to leave plenty of space between you and them in case they stop unexpectedly.");
			new ScriptLine("epsilon_idle_02", "epsilon", "Look both ways and check for vehicles whenever you want to cross traffic - on zebra crossings, too!");
			new ScriptLine("epsilon_idle_03", "epsilon", "Magnets and electrical gadgets can affect your compass so make sure you keep them separate.");
			new ScriptLine("epsilon_idle_04", "epsilon", "Compasses with bubbles in them usually don't work properly.");
			new ScriptLine("epsilon_idle_05", "epsilon", "Some animals use sound to navigate their environment. This is called sonar!");
			new ScriptLine("epsilon_idle_06", "epsilon", "Sonar is an acronym for \"Sound Navigation Ranging\"!");
			new ScriptLine("epsilon_idle_07", "epsilon", "Make sure that you're not looking at the map upside down!");
			new ScriptLine("epsilon_idle_08", "epsilon", "Have you seen my trophies?");
			new ScriptLine("epsilon_idle_09", "epsilon", "The best surfing's on Neptune!");
			new ScriptLine("epsilon_idle_10", "epsilon", "* Humming *");

			new ScriptLine("rho_idle_01", "rho", "Underwater swimming is the only time you should hold your breath while exercising!");
			new ScriptLine("rho_idle_02", "rho", "Each muscle fibre is thinner than a hair, and can support up to 1,000 times its own weight!");
			new ScriptLine("rho_idle_03", "rho", "When going out in the sun, remember to slop on some sunscreen and slap on a hat!");
			new ScriptLine("rho_idle_04", "rho", "If you can't have a conversation with a friend while exercising, you're training too hard!");
			new ScriptLine("rho_idle_05", "rho", "Your tummy won't work without fruit, fruit and more fruit! Apples away!");
			new ScriptLine("rho_idle_06", "rho", "Brush your teeth at least once a day to keep 'em great for biting!");
			new ScriptLine("rho_idle_07", "rho", "You need strong muscles to have fun!Exercise for at least 30 minutes a day!");
			new ScriptLine("rho_idle_08", "rho", "A tired bunny cannot bounce!Get at least 7 hours of sleep each night!");
			new ScriptLine("rho_idle_09", "rho", "The balance of protein to carbohydrates in a healthy meal should be 2 to 5!");
			new ScriptLine("rho_idle_10", "rho", "Breakfast fills your empty tank after a long night without food!");
			new ScriptLine("rho_idle_11", "rho", "Nordic Walking has incontestable health benefits!");

			new ScriptLine("tau_idle_01", "tau", "One third of the people on Earth have never made a telephone call! Can you believe it?");
			new ScriptLine("tau_idle_02", "tau", "Robots can explore inside gas tanks, volcanoes, deep under the sea and other places too dangerous for people to go!");
			new ScriptLine("tau_idle_03", "tau", "It's easier for robots to get around on wheels than using legs.  Why don't we have wheels too?");
			new ScriptLine("tau_idle_04", "tau", "Robots can be really strong or really smart - sometimes both!");
			new ScriptLine("tau_idle_05", "tau", "Some people can dance like a robot. Some robots can dance like people. A lot of robots and people just can't dance.");
			new ScriptLine("tau_idle_06", "tau", "Sensors tell robots about their surroundings!Pi has 3.14 sensors per ear!");
			new ScriptLine("tau_idle_07", "tau", "Pi's birthday is on the 22nd of July!");
			new ScriptLine("tau_idle_08", "tau", "Flowers follow the Fibonacci sequence!");
			new ScriptLine("tau_idle_09", "tau", "How many nanobots can dance on the head of a pin?");
			new ScriptLine("tau_idle_10", "tau", "I'm reading Artificial Intelligence, by Ann Droid!");

			new ScriptLine("zeta_idle_01", "zeta", "You can combine baking soda with soap for a good scrubbing paste to clean the sink or bathroom!");
			new ScriptLine("zeta_idle_02", "zeta", "Add some vinegar to water for a window spray that really makes them sparkle!");
			new ScriptLine("zeta_idle_03", "zeta", "Some people vacuum their floor twice a week.If you have pets you might have to vaccum even more!");
			new ScriptLine("zeta_idle_04", "zeta", "Before electricity some people would sprinkle a carpet with tea leaves.The idea was that the leaves would attract the dust and be easily swept away!");
			new ScriptLine("zeta_idle_05", "zeta", "Cleaning can be a great way to clear your head or give you a chance to think about things!");
			new ScriptLine("zeta_idle_06", "zeta", "If there's a bad smell nearby, fill a plastic container with white vinegar and punch holes in the top. The smell should be gone by the next day!");
			new ScriptLine("zeta_idle_07", "zeta", "Never use disinfectants to clean a refrigerator or else the food inside will taste like disinfectant. Yuck!");
			new ScriptLine("zeta_idle_08", "zeta", "Before a major cleaning session, always make sure you have the items you'll need!");
			new ScriptLine("zeta_idle_09", "zeta", "Using an art gum eraser makes cleaning scuff marks off the floor really easy!");
			new ScriptLine("zeta_idle_10", "zeta", "Be sure to sanitize sponges periodically in hot water to prevent germs from collecting inside!");

			//MINIGAME LINES
			//TELEPORTER
			new ScriptLine("teleporter_01", "child", "Delta said I should bring someone from my family. I know just the person!");
			new ScriptLine("teleporter_02", "child", "Someone I trust from school... A teacher or another adult... Ooh! I know!");
			new ScriptLine("teleporter_03", "child", "A trusted adult, who's not from my family, or from school...");
			new ScriptLine("teleporter_04", "child", "Rho didn't say where my next trusted adult had to be from, so I can beam in someone from my family, school, or anywhere else!");

			//FACTORY
			new ScriptLine("factory_01", "tau", "I'll show you how the machine works, then I'll leave you to it.");
			new ScriptLine("factory_02", "tau", "Here's a blueprint to practice on.");

			new ScriptLine("factory_arm1", "tau", "This is one of the robot's arms. Drag it onto the blueprint.");
			new ScriptLine("factory_arm", "tau", "This the robot's other arm. Drag it onto the blueprint, too.");
			new ScriptLine("factory_leg", "tau", "This is the robot's leg. Drag it onto the blueprint.");
			new ScriptLine("factory_head", "tau", "This is the robot's head. Drag it onto the blueprint.");
			new ScriptLine("factory_stomach", "tau", "This is the robot's stomach. Drag it onto the blueprint.");

			new ScriptLine("factory_paint", "tau", "Paint is like clothes for robots. Put the paint in the paint area.");

			new ScriptLine("factory_mouth", "tau", "The mouth is a private part, so don't touch it.");
			new ScriptLine("factory_crotch", "tau", "This is the area covered by the robot's underpants. It's not okay for anyone to touch it.");
			new ScriptLine("factory_chest", "tau", "The chest is a private part, so leave it on the conveyor belt.");

			new ScriptLine("factory_private_01", "tau", "Just like us, these robots have private body parts.");
			new ScriptLine("factory_private_02", "tau", "It's not okay for anyone to touch a robot's private parts.");
			new ScriptLine("factory_private_03", "tau", "That would be breaking the body rules.");
			new ScriptLine("factory_private_04", "tau", "So just leave all the private parts in their boxes, and the robots will add them later.");

			new ScriptLine("factory_queue", "tau", "When the arms, legs, stomach, and head have been added to the blueprint, drag it to the painting queue.");
			new ScriptLine("factory_dressing", "tau", "Great work. The robot is in the private dressing room.");
			new ScriptLine("factory_firstrobot", "tau", "Look at that. You've built your first robot.");

			new ScriptLine("factory_failpart", "tau", "Uh-oh. A public body part fell on the ground. Let's knock these dents out, and we can start again.");
			new ScriptLine("factory_failpaint", "tau", "Don't let the paint fall on the ground! I'll have to stop the machine to clean it up.");
			new ScriptLine("factory_failprivate", "tau", "Touching a private body part is breaking the body rules. The system override's been engaged. We'll have to start again.");

			new ScriptLine("factory_wrongblueprint", "tau", "Not quite. Try putting that part on a different robot");
			new ScriptLine("factory_paintfull", "tau", "Careful! The paint queue can only hold 4 cans of paint at a time.");
			new ScriptLine("factory_multirobots", "tau", "This time, each robot you place in the paint queue will be replaced by a fresh blueprint, if there are more robots to make.");

			//CLASSIFIER
			new ScriptLine("classifier_debrief_0_0", "rhoClassifier", "The \"Need to Tell\" stories in this training level were examples of bribes.");
			new ScriptLine("classifier_debrief_0_1", "rhoClassifier", "A bribe is when someone offers something like money, gifts or other treats so someone else will do what they want.");
			new ScriptLine("classifier_debrief_0_2", "rhoClassifier", "If someone ever tries to bribe you into breaking the body rules, tell your trusted adults immediately!");
			new ScriptLine("classifier_debrief_1_0", "rhoClassifier", "The \"Need to Tell\" stories in this training level were examples of tricks.");
			new ScriptLine("classifier_debrief_1_1", "rhoClassifier", "People who break the body rules can be very good at tricking children into thinking it's a game, or into doing something they didn't mean to.");
			new ScriptLine("classifier_debrief_1_2", "rhoClassifier", "Always tell your trusted adults if someone tricks you into breaking the body rules!");
			new ScriptLine("classifier_debrief_2_0", "rhoClassifier", "The \"Need to Tell\" stories in this training level were examples of secrets.");
			new ScriptLine("classifier_debrief_2_1", "rhoClassifier", "Adults who break the body rules know they're also breaking the law, which is why they want to keep it a secret!");
			new ScriptLine("classifier_debrief_2_2", "rhoClassifier", "If someone asks you to keep breaking the body rules a secret, tell your trusted adults immediately!");
			new ScriptLine("classifier_debrief_3_0", "rhoClassifier", "The \"Need to Tell\" stories in this training level were examples of coercion.");
			new ScriptLine("classifier_debrief_3_1", "rhoClassifier", "To coerce someone means to pressure them into doing something they don't want to do. Someone might coerce a child into breaking the body rules by making them feel scared, promising them something in return, or making the child feel like it was their fault.");
			new ScriptLine("classifier_debrief_3_2", "rhoClassifier", "People who use these tactics to get children to break the body rules do so because they know they're breaking the law! If someone coerces you into breaking the body rules, tell your trusted adults immediately!");
			new ScriptLine("classifier_debrief_4_0", "rhoClassifier", "The \"Need to Tell\" stories in this training level were examples of how someone who breaks the body rules may try to make a child think that no one will believe them if they tell..");
			new ScriptLine("classifier_debrief_4_1", "rhoClassifier", "The person might even change the story and tell the child's trusted adults that it was the child who broke the body rules.");
			new ScriptLine("classifier_debrief_4_2", "rhoClassifier", "If this happens, keep telling your trusted adults until someone believes you!");
			new ScriptLine("classifier_debrief_5_0", "rhoClassifier", "The \"Need to Tell\" stories in this training level were examples of child grooming.");
			new ScriptLine("classifier_debrief_5_1", "rhoClassifier", "Child grooming is a lengthy process that an adult who wants to break the body rules may use. It involves the adult building a friendship with the child to slowly introduce ways of breaking the body rules.");
			new ScriptLine("classifier_debrief_5_2", "rhoClassifier", "The adult may be hoping that by breaking the body rules slowly, the child won't notice, or they might get so used to it that they think it's okay. It is okay to be friends with adults but it is not okay if the adult breaks the body rules. Even if someone only breaks the body rules a little at a time, you still need to tell a trusted adult.");
			new ScriptLine("classifier_debrief_6_0", "rhoClassifier", "The \"Need to Tell\" stories in this training level were examples of how some people can make a child believe that they wanted the abuse, or that it was their fault.");
			new ScriptLine("classifier_debrief_6_1", "rhoClassifier", "These people may ask the child for their permission to break the body rules, but even if the child says yes, what happens is never their fault, and they should still tell a trusted adult. People who try to trick children do it because they know what they're doing is wrong.");
			new ScriptLine("classifier_debrief_6_2", "rhoClassifier", "If someone breaks the body rules and says it's your fault, don't believe them! Tell your trusted adults immediately!");
			new ScriptLine("classifier_debrief_7_0", "rhoClassifier", "The \"Need to Tell\" stories in this training level were examples of isolation.");
			new ScriptLine("classifier_debrief_7_1", "rhoClassifier", "Isolation is a tactic used to keep children away from their trusted adults, by telling them that no one really understands them, and making them feel alone.");
			new ScriptLine("classifier_debrief_7_2", "rhoClassifier", "Some people use this tactic in order to break the body rules. Always talk to your trusted adults if you're starting to feel isolated.");
			new ScriptLine("classifier_debrief_8_0", "rhoClassifier", "The \"Need to Tell\" stories in this training level were examples of people making a child feel special.");
			new ScriptLine("classifier_debrief_8_1", "rhoClassifier", "Everyone wants to feel special, and there's nothing wrong with that. However, sometimes people who want to break the body rules will try to convince the child that what they're doing is okay because they love them or care about them.");
			new ScriptLine("classifier_debrief_8_2", "rhoClassifier", "If someone treats you this way in order to break the body rules, that's not okay, and you need to tell your trusted adults!");

			//VR GAME
			new ScriptLine("vr_0_01", "vr1", "Hi Grandma, Hi Grandad");
			new ScriptLine("vr_0_02", "vr2", "Gwethana! It's so good to see you! Your father's really been enjoying your weekend visits.");
			new ScriptLine("vr_0_03", "vr3", "Your dad says you're starting to warm up to your new step-brother, too.");
			new ScriptLine("vr_0_04", "vr1", "Well, a little bit... Yerpal likes to play hide-and-seek with me.");
			new ScriptLine("vr_0_05", "vr2", "How lovely! What a nice big brother he is.");
			new ScriptLine("vr_0_06", "vr1", "I guess so. Sometimes he just yells at me. Or tells me not to touch the computer or the TV.");
			new ScriptLine("vr_0_07", "vr3", "It sounds like he's causing trouble for you.");
			new ScriptLine("vr_0_08", "vr1", "It's not that bad. I just do what he wants. Then he leaves me alone. Sometimes, he's even nice.");
			new ScriptLine("vr_0_09", "wall", "Gwethana is scared to tell her grandparents that when Yerpal plays hide and seek with her, he uses it as an opportunity to get her alone and break the body rules.");
			new ScriptLine("vr_0_10", "child", "Gwethana is scared. What if Yerpal gets angry that she told her grandparents he's been breaking the body rules?");
			new ScriptLine("vr_0_11", "currentAdult", "If Yerpal gets angry, it's because he knows he's doing something wrong. Gwethana has to tell her trusted adults, so they can keep her safe");
			new ScriptLine("vr_0_12", "child", "Gwethana is amazing! She told her grandparents what's been happening, even though she was really scared.");
			new ScriptLine("vr_0_13", "currentAdult", "Now that she's told someone, they help keep her safe, and make sure that she can have fun visiting her dad, without having to be afraid.");


			/*
			//UNCOMMENT THIS TO FIND MISSING AUDIO LINES
			foreach (string key in lines.Keys)
			{
				if (lines[key].characterName.StartsWith("adult"))
				{
					for(int adult=1; adult<7; adult++)
					{
						if (Resources.Load("Audio/en/" + key + "_a" + adult) == null)
						{
							Debug.Log("Audio missing (" + key + "_a" + adult + ") - " + lines[key].characterName + ": " + lines[key].text);
						}
						if (lines[key].maleText != null && Resources.Load("Audio/en/" + key + "_a" + adult + "_m") == null)
						{
							Debug.Log("Audio missing (" + key + "_a" + adult + "_m) - " + lines[key].characterName + ": " + lines[key].maleText);
						}
					}
				}
				else if (lines[key].characterName == "child")
				{
					if (Resources.Load("Audio/en/" + key) == null)
					{
						Debug.Log("Audio missing (" + key + ") - " + lines[key].characterName + ": " + lines[key].text);
					}
					if (Resources.Load("Audio/en/" + key + "_m") == null)
					{
						Debug.Log("Audio missing (" + key + "_m) - " + lines[key].characterName + ": " + ((lines[key].maleText != null) ? lines[key].maleText : lines[key].text));
					}
				}
				else
				{
					if (Resources.Load("Audio/en/" + key) == null)
					{
						Debug.Log("Audio missing (" + key + ") - " + lines[key].characterName + ": " + lines[key].text);
					}
					if (lines[key].maleText != null && Resources.Load("Audio/en/" + key + "_m") == null)
					{
						Debug.Log("Audio missing (" + key + "_m) - " + lines[key].characterName + ": " + lines[key].maleText);
					}
				}
			}
			*/
		}
		else
		{
			Destroy(this.gameObject);
		}
	}
	void Start()
	{
		//OnLevelWasLoaded only fires when moving between levels - we call it from start to make it fire on the first loaded level
		OnLevelWasLoaded(0);
	}
	void OnLevelWasLoaded(int level)
	{
		//Awake is called before OnLevelWasLoaded, but is still being called for gameObjects that are destroyed in Awake!? So we test to see if this is the persistent GameController
		if (this == GameController.singleton)
		{
			//TODO: Show the correct overlay
			tests = new List<TestInt>();
			//TODO: Since overlays are hidden, the following find won't work on their children, so we cycle through the hidden overlays and use getcomponentinchildren, instead (and hide them at the same time)
			foreach (GameObject o in overlays.Values)
			{
				o.SetActive(false);
				foreach (TestInt test in o.GetComponentsInChildren<TestInt>(true))
				{
					Debug.Log(test.gameObject);
					tests.Add(test);
				}
			}
			foreach (TestInt test in FindObjectsOfType<TestInt>())
			{
				tests.Add(test);
			}

			//Turn on the overlay for the current scene.
			//This will not work for scenes where the overlay is shared (e.g. ShipTop, ShipMiddle, and ShipBottom)
			//Those scene overlays are turned on by an object within the scene (e.g. Ship Avatar)
			/*string startingOverlay = SceneManager.GetActiveScene().name + " Overlay";
			foreach (GameObject o in overlays.Values)
			{
				o.SetActive(o.name == startingOverlay);
			}*/

		   //Set checkpoint to current checkpoint to update quest and checkpoint-sensitive objects
		   SetCheckpoint(GameController.GetInt("Checkpoint"));


		}
	}

	public void SetOverlay(string overlayName)
	{
		if (singleton.overlays.ContainsKey(overlayName))
		{
			overlays[_overlay].SetActive(false);
			_overlay = overlayName;
			overlays[_overlay].SetActive(true);
			// Freeze time when opening the menu of a time-sensitive minigame. This means we can't do some things that rely on time, like speech triggers
			if (_overlay == "Factory Menu" || _overlay == "Sorter Menu")
			{
				Time.timeScale = 0f;
			}
			else
			{
				Time.timeScale = 1f; // Freeze time when opening the menu of a time-sensitive minigame.
				Invoke("FireOverlayTriggers", 0.1f); //Wait briefly after activating the layer, and before firing overlay triggers, or some of the internal objects won't be found by Find operations.
			}
		}
	}
	void FireOverlayTriggers()
	{
		//TODO: Cancel invoked actions that haven't occurred, yet
		foreach (StoryTrigger trigger in overlays[_overlay].GetComponents<StoryTrigger>())
		{
			trigger.Trigger();
		}

	}


	public static string overlay
	{
		get
		{
			return singleton._overlay;
		}
		set
		{
			singleton.SetOverlay(value);
		}

	}

	public void StartGame()
	{
		StartGame("test");
	}
	public void StartGame(string slot)
	{
		FindObjectOfType<Menu>().StartGame(slot);
	}

	//When called with no parameter, toggles back to prevScene. Unless called from MainMenu
	public void LoadScene()
	{
		LoadScene(GameController.GetString("prevScene"), SceneManager.GetActiveScene().name);
	}

	public void LoadScene(string sceneName)
	{
		//		Debug.Log(SceneManager.GetActiveScene().name);
		if (sceneName.Contains(">")) //Force prevScene
		{
			string[] sceneNames = sceneName.Split('>');
			LoadScene(sceneNames[1], sceneNames[0]);
		}
		else //Use current scene as prevScene
		{
			LoadScene(sceneName, SceneManager.GetActiveScene().name);
		}
	}

	public void LoadScene(string sceneName, string prevSceneName)
	{
		GameController.SetString("prevScene", prevSceneName);
		GameController.SetString("scene", sceneName);
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
	}

	public void LoadTeleporter(int character)
	{
		GameController.SetInt("teleporter_level", character);
		LoadScene("Teleporter");
	}
	int PickLevel(int completedMask, int maxLevel)
	{
		Debug.Log("Testing level mask: " + completedMask);
		int i = 0;
		while (i < maxLevel && ((completedMask & (1 << i)) != 0))
		{
			i++;
		}
		Debug.Log("Level Picked:" + i);
		return i;
	}
	public void LoadFactory()
	{
		LoadFactory(PickLevel(GameController.GetInt("factory_completed"), 11));
	}
	public void LoadFactory(int level)
	{
		Debug.Log("Loading Factory: " + level);
		GameController.SetInt("factory_level", level);
		LoadScene("Factory");
	}
	public void LoadClassifier()
	{
		LoadClassifier(PickLevel(GameController.GetInt("classifier_completed"), 8));
	}
	public void LoadClassifier(int level)
	{
		Debug.Log("Loading Classifier: " + level);
		GameController.SetInt("classifier_level", level);
		LoadScene("Classifier");
	}
	public void LoadUnscrambler()
	{
		//TODO: Based on checkpoint
	}
	public void LoadUnscrambler(int level)
	{
		Debug.Log("Loading Unscrambler: " + level);
		GameController.SetInt("unscrambler_level", level);
		LoadScene("Unscrambler");
	}
	public void LoadVR()
	{
		LoadVR(PickLevel(GameController.GetInt("vr_completed"), 3));
	}
	public void LoadVR(int level)
	{
		Debug.Log("Loading VR Game: " + level);
		GameController.SetInt("vr_level", level);
		LoadScene("VR");
	}
	public static void CompleteLevel(string minigame, int level, bool isComplete = true)
	{
		Debug.Log(minigame + " level " + level + " complete.");
		int val = GameController.GetInt(minigame + "_completed");
		Debug.Log("Level mask was: " + val);
		int mask = 1 << level;
		if (isComplete)
		{
			val |= mask;
		}
		else
		{
			val &= ~mask;
		}
		Debug.Log("Level mask now: " + val);
		GameController.SetInt(minigame + "_completed", val);
	}

	public static void Directions(string[] directions)
	{
		foreach (string direction in directions)
		{
			singleton.QueueLine(direction);
		}
	}

	void QueueLine(string direction)
	{
		if (ready)
		{
			ready = false;
			SpeakLine(direction);
		}
		else
		{
			queue.Add(direction);
		}
	}

	public void NextLine()
	{
		if (queue.Count > 0)
		{
			string direction = queue[0];
			queue.RemoveAt(0);
			SpeakLine(direction);
		}
		else
		{
			ready = true;
		}
	}

	void SpeakLine(string direction)
	{
		Debug.Log(direction);
		switch (direction.Substring(0, 1))
		{
			case "!": //Display Reward Item Splash
				DisplayReward(direction.Substring(1));
				break;
			case "@": //Load Scene
				LoadScene(direction.Substring(1));
				NextLine();
				break;
			case "#": //Set Checkpoint
				SetCheckpoint(int.Parse(direction.Substring(1)));
				break;
			case "(": //Display Chapter Beginning Splash
				Debug.Log("Begin Chapter " + direction.Substring(1));
				DisplayProgress(int.Parse(direction.Substring(1)) - 1);
				break;
			case ")": //Display Chapter Ending Splash
				Debug.Log("Complete Chapter " + direction.Substring(1));
				DisplayProgress(int.Parse(direction.Substring(1)) + 4);
				break;
			case "^": //Display Disclosure Splash
				Debug.Log("Disclosure " + direction.Substring(1));
				DisplayProgress(int.Parse(direction.Substring(1)) + 9);
				break;
			case "~": //Wait for Milliseconds
				Invoke("NextLine", int.Parse(direction.Substring(1)) / 1000f);
				break;
			case "%": //Set int variable
				string[] terms = direction.Substring(1).Split('=');
				if (terms.Length == 2)
				{
					GameController.SetInt(terms[0], int.Parse(terms[1]));
				}
				break;
			case "+": //Special Commands (currently only shake)
				switch(direction.Substring(1))
				{
					case "shake":
						//TODO: Screen shake and vibrate
						break;
				}
				break;
			default: //Speak Voice Line
				if (singleton.lines.ContainsKey(direction))
				{
					ScriptLine line = lines[direction];
					string characterName = line.characterName;
					if (characterName == "currentAdult")
					{
						if (GameController.GetInt("Checkpoint") < 210)
						{
							characterName = "adult1";
						}
						else if (GameController.GetInt("Checkpoint") < 320)
						{
							characterName = "adult2";
						}
						else if (GameController.GetInt("Checkpoint") < 410)
						{
							characterName = "adult3";
						}
						else if (GameController.GetInt("Checkpoint") < 530)
						{
							characterName = "adult4";
						}
						else
						{
							characterName = "adult5";
						}
					}

					bool spoken = false;
					foreach (SpeakingCharacter speaker in FindObjectsOfType<SpeakingCharacter>())
					{
						if (!spoken && speaker.name == characterName)
						{
							spoken = true;
							speaker.Speak(line);
						}
					}
					//If speaker was not present, move on to next line
					if (!spoken)
					{
						NextLine();
					}
				}
				else
				{
					Debug.LogWarning("Non-existent script line requested: " + direction);
					NextLine();
				}
				break;
		}
	}

	void SetCheckpoint(int checkpoint)
	{
		if (checkpoints.ContainsKey(checkpoint))
		{
			GameController.SetInt("Checkpoint", checkpoint);
			Debug.Log("Checkpoint Set: " + checkpoint);
			if (questText != null)
			{
				questText.text = checkpoints[checkpoint].name;
			}
			//We setup for all the tests before we do all the tests, because some objects have multiple tests on them, and we don't want resetting between tests.
			//			Debug.Log("Prepare");
			foreach (TestInt test in tests)
			{
				test.PrepareTest();
				//				Debug.Log(test.gameObject);
			}
			//			Debug.Log("Test");
			foreach (TestInt test in tests)
			{
				test.DoTest();
				//				Debug.Log(test.gameObject);
			}
			switch (GameController.GetString("scene"))
			{
				case "ShipBottom":
					compass.targetX = checkpoints[checkpoint].compassTargets[0];
					break;
				case "ShipMiddle":
					compass.targetX = checkpoints[checkpoint].compassTargets[1];
					break;
				case "ShipTop":
					compass.targetX = checkpoints[checkpoint].compassTargets[2];
					break;
				default:
					compass.targetX = 0;
					break;
			}
		}
		else
		{
			Debug.LogWarning("Non-existent checkpoint requested: " + checkpoint);
		}
		NextLine();
	}

	void DisplayReward(string rewardID)
	{
		rewardSplash.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Rewards/" + rewardID);
		rewardSplash.GetComponent<Animator>().Play("Reward Splash");
		Invoke("NextLine", 1.5f);
	}
	void DisplayProgress(int progressID)
	{
		for (int i = 0; i < progressSplash.childCount; i++)
		{
			progressSplash.GetChild(i).gameObject.SetActive(i == progressID);
		}
		//		progressSplash.GetComponent<Animator>().Play("Progress Hidden");
		progressSplash.GetComponent<Animator>().Play("Progress Splash", 0, 0f);
		Invoke("NextLine", 3f);
	}

	//Setting up a thin layer on top of player prefs, because I think I'll end up using something different - prob some kind of serialization
	public static string slot
	{
		get
		{
			return PlayerPrefs.GetString("slot");
		}
		set
		{
			PlayerPrefs.SetString("slot", value);
		}
	}
	public static int GetInt(string key)
	{
		return PlayerPrefs.GetInt(slot + key);
	}
	public static void SetInt(string key, int value)
	{
		PlayerPrefs.SetInt(slot + key, value);
	}
	public static float GetFloat(string key)
	{
		return PlayerPrefs.GetFloat(slot + key);
	}
	public static void SetFloat(string key, float value)
	{
		PlayerPrefs.SetFloat(slot + key, value);
	}
	public static string GetString(string key)
	{
		return PlayerPrefs.GetString(slot + key);
	}
	public static void SetString(string key, string value)
	{
		PlayerPrefs.SetString(slot + key, value);
	}
	public static bool GetBool(string key)
	{
		return PlayerPrefs.GetInt(slot + key) == 1;
	}
	public static void SetBool(string key, bool value)
	{
		PlayerPrefs.SetInt(slot + key, (value) ? 1 : 0);
	}
	public static Color GetColor(string key)
	{
		return new Color(PlayerPrefs.GetFloat(slot + key + "_r"), PlayerPrefs.GetFloat(slot + key + "_g"), PlayerPrefs.GetFloat(slot + key + "_b"));
	}
	public static void SetColor(string key, Color value)
	{
		Debug.Log(value);
		PlayerPrefs.SetFloat(slot + key + "_r", value.r);
		PlayerPrefs.SetFloat(slot + key + "_g", value.g);
		PlayerPrefs.SetFloat(slot + key + "_b", value.b);
	}

	public static void SetAvatar(ShipAvatar avatar)
	{
		singleton.shipAvatar = avatar;
	}

}

public class ScriptLine
{
	public string lineID;
	public string characterName;
	public string text;
	public string maleText;
	public ScriptLine(string lineID, string characterName, string text, string maleText = null) //Provide male text only if line is gendered
	{
		//Auto insert line breaks.
		int minLength = 20;
		int i = minLength;
		while (i < text.Length)
		{
			if(text.Substring(i, 1) == " ")
			{
				text = text.Substring(0, i) + "\n" + text.Substring(i + 1);
				i += minLength;
			}
			i ++;
		}

		this.lineID = lineID;
		this.characterName = characterName;
		this.text = text;
		this.maleText = maleText;
		GameController.singleton.lines.Add(lineID, this);
	}
}

public class Checkpoint
{
	public string name;
	public float[] compassTargets;
	public Checkpoint(string name, float[] compassTargets)
	{
		this.name = name;
		this.compassTargets = compassTargets;
	}
}

