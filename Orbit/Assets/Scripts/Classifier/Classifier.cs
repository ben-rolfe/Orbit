using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Classifier : MonoBehaviour {
	[SerializeField] Sprite[] iconSprites;
	[SerializeField] Image[] icons;
	[SerializeField] Image[] flags;
	[SerializeField] Text heading;
	[SerializeField] Text text;
	[SerializeField] Image icon;
	[SerializeField] Image flag;
	[SerializeField] Button[] buttons;
	[SerializeField] Button needToTellButton;
	[SerializeField] Button doNotNeedToTellButton;
	int selectedClip = -1;
	ClassifierClip[,] clips = new ClassifierClip[9, 8]
	{
		{
			new ClassifierClip(4, false, "classifier_0_0", "Car wash", "To: Brother\nFrom: Sister\n\nMum says she'll give you $5 if you wash the car! Lucky!"),
			new ClassifierClip(2, true, "classifier_0_1", "Computer games", "Location: Friend's bedroom\nActivity: Playing Computer Games\nTime: Saturday, 2:30 pm\n\nI'll let you play this new game my mum bought me,\nbut first you need to promise to do whatever I say, okay?\nYou show me what's inside your underpants, and you can play first!\nYou promised to do whatever I said, remember?"),
			new ClassifierClip(1, true, "classifier_0_2", "Burger?", "To: Child\nFrom: Adult\n\nI'm going to be visiting again this weekend - want to play our special private parts game? I'll take you for a burger afterwards, and you can eat whatever you want."),
			new ClassifierClip(4, false, "classifier_0_3", "Private", "To: Sister\nFrom: Brother\n\nMum and Dad are having a private conversation. What do you think they're talking about?"),
			new ClassifierClip(2, false, "classifier_0_4", "Apple", "Location: school playground\nTime: lunchbreak\n\nFirst child: I'll trade my muesli bar for your apple!\nSecond child: Okay!"),
			new ClassifierClip(0, true, "classifier_0_5", "Camping", "To: Child\nFrom: Adult\n\nHey, sorry I missed you this afternoon!\nLet's go camping again, this weekend!\nIt should be beautiful weather, but my other sleeping bag has a hole in it,\nso we're going to have to share!\nSee you at 5pm on Friday? Let me know!"),
			new ClassifierClip(1, false, "classifier_0_6", "My dog", "To: Child\nFrom: Friend\n\nI really like playing with my dog, Rex. He's so funny!"),
			new ClassifierClip(1, true, "classifier_0_7", "Party", "To: Child\nFrom: Young adult friend\n\nHey, I hope you enjoyed the grown-up party I took you to. You were the prettiest girl there. I'll give you another $20 if we can play our special game this Friday. Love you.")
		},{
			new ClassifierClip(1, true, "classifier_1_0", "Clumsy me", "To: Friend\nFrom: Child\n\nThe weirdest thing happened yesterday! My dad's friend is staying with us, and as he was walking past my room after his shower, he dropped his towel! He laughed, and I tried not to look, but it seemed to take a while for him to keep walking. Gross, huh?"),
			new ClassifierClip(0, true, "classifier_1_1", "Mum's friend", "To: Sister\nFrom: Brother\n\nHey, sis. Sorry I wouldn't talk to you before but I was wondering...\nThe other day me and mum's friend were swimming in the pool\nand she, uh, touched me inside my swimmers.\nI asked her what she was doing, but she just smiled and swam away.\nShould I tell someone? What do you think?"),
			new ClassifierClip(0, false, "classifier_1_2", "Hot chocolate", "To: Brother\nFrom: Sister\n\nI was making a hot chocolate the other day and\nI put two spoonfuls of sugar in it when mum wasn't looking! \nThen, I took a sip, and I realised someone had switched the salt with the sugar!\nSo gross! It was you, wasn't it?"),
			new ClassifierClip(4, true, "classifier_1_3", "Cards?", "To: Sister\nFrom: Brother\n\nI was playing cards with my parents' friend yesterday, but when he turned them over they had pictures of naked women on them! He just smiled and said, \"Where did they come from?\" So weird!"),
			new ClassifierClip(1, true, "classifier_1_4", "Didn't go", "To: Friend\nFrom: Child\n\nMy uncle came over to take care of me the other day, and he said he'd take me for a burger if we played a special game. I had to touch his private parts, but even after I did, he just told me to eat something from the fridge! Ripped off! So unfair!"),
			new ClassifierClip(1, true, "classifier_1_5", "Movies", "To: Child\nFrom: 19 year old friend\n\nI'm so sorry we didn't watch your favourite movie the other night! I have no idea where it went! I hope you didn't mind watching that one with the naked adults in it. I was as surprised as you were!"),
			new ClassifierClip(4, true, "classifier_1_6", "Shower", "To: Child\nFrom: Aunt\n\nThanks for helping me in the shower this morning. With my sore arm, I just can't reach all those hard-to-wash places!"),
			new ClassifierClip(0, false, "classifier_1_7", "Worming medicine", "To: Friend\nFrom: Child\n\nMum is so mean!\nShe gave me a chocolate yesterday as a treat, \nbut once I'd eaten it she told me it was worming medicine! Gross!")
		},{
			new ClassifierClip(4, true, "classifier_2_0", "Game", "To: Child\nFrom: Adult friend\n\nDo you want to play a secret game with me? It's a lot of fun, but you can't tell anyone about it."),
			new ClassifierClip(2, false, "classifier_2_1", "Watching movies", "Location: Playground\nActivity: Two children playing\n\nI had a sleep-over last night, \nand we stayed up talking and watching movies 'til after midnight!\nIt was so much fun!"),
			new ClassifierClip(2, true, "classifier_2_2", "Don't tell", "Location: Friend's House\n\nIf anyone finds out we're breaking the body rules we'll be in so much trouble!\nPromise you won't tell anyone!"),
			new ClassifierClip(4, true, "classifier_2_3", "Hurt", "To: Child\nFrom: Friend\n\nSomeone's been hurting me, but I'm scared to tell. Please don't you tell anyone, either."),
			new ClassifierClip(1, false, "classifier_2_4", "Surprise", "To: Little Sister\nFrom: Big Sister\n\nMum is planning a surprise birthday party for Nana! Don't tell anyone!"),
			new ClassifierClip(4, false, "classifier_2_5", "Treat", "To: Child\nFrom: Mum\n\nDid you like the surprise I put in your lunch box today? It was a special treat, just for you!"),
			new ClassifierClip(2, false, "classifier_2_6", "TV show", "To: Child\nFrom: Friend\n\nMy favourite TV show is on tonight! I can't wait! \nI've gotta run to get home in time, see you tomorrow!"),
			new ClassifierClip(1, true, "classifier_2_7", "Thanks", "To: Child\nFrom: Adult\n\nI felt so much better after you showed me your private parts yesterday. But remember, this is our little secret.")
		},{
			new ClassifierClip(0, true, "classifier_3_0", "Magazines", "Situation: Older brother's friend talking to child on telephone\n\nHey! Check out these magazines I found that have naked people in them!\nWhat? You don't want to look?\nWhat's wrong with you?"),
			new ClassifierClip(2, false, "classifier_3_1", "Nice to see you", "Situation: Adult talking to child\n\nImagine us meeting up at the park like that today!\nWhere's your little brother? He's not sick, I hope.\nI'll give your mum a call soon. Bye!"),
			new ClassifierClip(4, false, "classifier_3_2", "Homework", "To: Child\nFrom: Mum\n\nSure, you can watch TV tonight, but only if you get all of your homework done first!"),
			new ClassifierClip(4, false, "classifier_3_3", "Museum", "To: Girl\nFrom: Boy\n\nAre you going on the school trip to the museum? I am. I hope you are, too!"),
			new ClassifierClip(1, true, "classifier_3_4", "I'll teach you", "To: Child\nFrom: Young adult friend\n\nI was so surprised when you said you'd never kissed anyone! All the other people your age know how to kiss properly. Next time I see you, I'll help you practice, so you aren't embarrassed when you start dating, if you like."),
			new ClassifierClip(0, true, "classifier_3_5", "So sad", "Situation: Adult talking to child\n\nI've been so sad lately. I don't know what to do.\nBut I was thinking\nMaybe if you showed me your private parts it might cheer me up?\nPlease? I'm so lonely."),
			new ClassifierClip(1, false, "classifier_3_6", "Concert", "To: Brother\nFrom: Sister\n\nMum says she really wants me to play in the school concert! No way! But she did say it's my choice, and I don't have to if I don't want to."),
			new ClassifierClip(4, true, "classifier_3_7", "Go out with me?", "To: Girl\nFrom: Boy\n\nSure, I'll be your boyfriend, but you have to send a naked picture of yourself to my phone!")
		},{
			new ClassifierClip(4, false, "classifier_4_0", "Pancakes", "To: Friend\nFrom: Child\n\nMy mum makes the best pancakes, doesn't she? I can't believe how many you ate! I'm glad you like them!"),
			new ClassifierClip(4, false, "classifier_4_1", "Shooting star", "To: Child\nFrom: Friend\n\nI was lying in bed last night, and guess what? I saw a shooting star!! My brother says I'm lying, but you believe me, don’t you?"),
			new ClassifierClip(0, false, "classifier_4_2", "Wooden fort", "To: Friend\nFrom: Child\n\nLast weekend dad and I built a fort in the back yard! You're not gonna believe it! It's huge!"),
			new ClassifierClip(2, true, "classifier_4_3", "Only a movie", "Situation: Mother's boyfriend talking to child\n\nYou've been complaining about me to your mum all this time and I'm still here.\nGo ahead and tell her I made you watch a movie with naked people in it.\nYou think she'll believe you? Good luck!\nBesides, it's only a movie!"),
			new ClassifierClip(2, false, "classifier_4_4", "Project work", "Location: classroom\n\nI can't believe it!\nThis project is going to take us ages to finish!"),
			new ClassifierClip(4, true, "classifier_4_5", "Shower", "To: Child\nFrom: Friend's older brother\n\nYou can't prove I was watching you in the shower! No one will ever believe you! They'll think you're the perv for making it up, not me!"),
			new ClassifierClip(2, true, "classifier_4_6", "Your father and me", "To: Child\nFrom: Father's friend\n\nI've known your father since before you were born.\nWho's he going to believe? \nIf you try telling him I made you touch my private parts,\nhe'll just laugh at you."),
			new ClassifierClip(1, true, "classifier_4_7", "Free magazines", "To: Friend\nFrom: Child\n\nI always thought the man at the newsagents was really nice! He gave me all these free magazines, but today he said if I don't show him my private parts he's going to tell my parents I stole them! What a jerk!")
		},{
			new ClassifierClip(1, true, "classifier_5_0", "Massage", "To: Diary\nFrom: Child\n\nMy older cousin likes to give me and all my other cousins massages. They're usually good, but sometimes he puts his hand too close to my private parts! Today he left his hand there for a long time!"),
			new ClassifierClip(4, true, "classifier_5_1", "Friend's house", "To: Friend\nFrom: Child\n\nI really like sleeping over at Sunthi's house! Her mum lets us watch whatever we want, even DVDs for adults! Some of them even have naked people in!"),
			new ClassifierClip(2, true, "classifier_5_2", "Neighbour", "Situation: Child talking to friend\n\nFriend: Who's that guy with your mum?\nChild: Oh, he lives next door.\nHe spends a lot of time playing with me, \nand he wants to take me camping this weekend, just the two of us! \nBut I don't know if I want to go, \nand I'm worried about telling mum, in case she thinks I'm being rude!"),
			new ClassifierClip(1, true, "classifier_5_3", "Coach", "To: Sport team friend\nFrom: Child\n\nPractice is so much fun! Coach is really nice, and he always gives me a hug at the end! But, um, yesterday? He hugged me for a long time and then his hand... like, went, to my bottom? Weird, right??!"),
			new ClassifierClip(1, false, "classifier_5_4", "New sleeping bag", "To: Aunt\nFrom: Child\n\nWe went on a school camping trip last week, and I saw three frogs! When my friends finally stopped talking, I slept really well. Mum even bought me a new sleeping bag! When are you coming to visit again?"),
			new ClassifierClip(4, false, "classifier_5_5", "Hurt back", "To: Friend\nFrom: Child\n\nWhen I got home from sport dad rubbed some cream on my back, because it was sore. Eeeew, it was so stinky! Next time I won't tell him my back hurts!!"),
			new ClassifierClip(0, false, "classifier_5_6", "Choose a movie", "Situation: At a sleepover\n\nFriend's Mum: We're going to watch a movie tonight - when you come over,\nyou can pick any DVD from the kids' section at our house, okay?"),
			new ClassifierClip(4, false, "classifier_5_7", "Grandma", "To: Friend\nFrom: Child\n\nMy grandma always gives me a huge hug when I see her! She's the best!")
		},{
			new ClassifierClip(0, true, "classifier_6_0", "Sleepover", "To: Child\nFrom: Adult\n\nI knew when you agreed to sleep over that you wanted to touch each other's private parts, too."),
			new ClassifierClip(2, true, "classifier_6_1", "Dressed like that", "Location: Friend's House\nAdult is talking to child\n\nYou wouldn't have come over dressed like that\nif you didn't secretly want to break the body rules with me."),
			new ClassifierClip(4, true, "classifier_6_2", "Noone else", "To: Child\nFrom: Adult\n\nNo one else knows how to touch me like you do. I can tell you really enjoyed it, because you're really good at it!"),
			new ClassifierClip(2, false, "classifier_6_3", "Spilt milk", "Location: Home\nMum is talking\n\nDon’t worry, no need to cry. \nJust wipe up the spilt milk and be more careful next time, okay?"),
			new ClassifierClip(4, false, "classifier_6_4", "Clothes", "To: Child\nFrom: Friend\n\nI saw you at the shopping centre this weekend, even if you didn't see me! When did you get those new clothes? You look so cool!"),
			new ClassifierClip(2, false, "classifier_6_5", "Beach", "To: Child\nFrom: Mum and Dad\n\nYou love it when we get to spend the whole day at the beach, don't you?"),
			new ClassifierClip(0, false, "classifier_6_6", "Fun camping", "To: Child\nFrom: Cousin\n\nIt's so great when our whole family can go camping together! \nSharing that huge tent was fun, too, all of us in our own sleeping bags!\nI was so warm I fell asleep straight away!"),
			new ClassifierClip(4, true, "classifier_6_7", "So cute!", "To: Child\nFrom: Adult\n\nI couldn't help myself! You just looked so cute! What was I supposed to do? I just had to touch you, but I know you didn't mind.")
		},{
			new ClassifierClip(2, false, "classifier_7_0", "Making dinner", "Location: Child's House\nSituation: Mum is cooking dinner\n\nMum: I'm sorry, sweetie. I'm cooking dinner right now.\nDad: Come over here. I'll help you with your homework."),
			new ClassifierClip(4, true, "classifier_7_1", "Busy", "To: Child\nFrom: Family Friend\n\nYour mum's really busy, but don't forget that I'm here for you if you ever get lonely. I'm right next door, and I'll never be too busy for you. Your mum won't even notice you're gone."),
			new ClassifierClip(0, false, "classifier_7_2", "Headache", "To: Child\nFrom: Mum\n\nHey, sweetie, mum here. Sorry, but I've got a really bad headache today! \nCould you go next door when you get home? \nI really need to take a nap, and Mrs. Omikron's happy to have you.\nI'll come get you when I wake up. Love you!"),
			new ClassifierClip(2, true, "classifier_7_3", "You lie", "Location: Child's House\nSituation: Mum's Partner talking with daughter\n\nI know how often you lie.\nI've told your mother you're a liar, too, \nso I wouldn’t bother telling her anything bad about me. \nShe'll never believe you."),
			new ClassifierClip(1, false, "classifier_7_4", "Saturday night", "To: Friend\nFrom: Child\n\nHow was your Saturday night? Mine was so boring! My parents took me to a party, but I was the only kid there! They just turned on the TV for me while they had fun! At least they gave me chips and soft drink!"),
			new ClassifierClip(2, false, "classifier_7_5", "But dad...", "Location: Child's House\n\nChild: But dad...\nDad: That's enough!\nDad: Go to your room! I don't want to see you until dinner time!"),
			new ClassifierClip(4, true, "classifier_7_6", "Come over", "To: Child\nFrom: Granny's Neighbour\n\nYour Granny's getting old, and she needs to rest. Why don't you come spend time with me instead? I know she'd appreciate the break. I have lots of special games we can play, so you'll never get bored!"),
			new ClassifierClip(1, true, "classifier_7_7", "Make muffins", "To: Child\nFrom: Adult Neighbour\n\nYour mum and dad work a lot, don't they? Do you really like being alone? If not, come over, and I'll bake your favourite muffins for you! Then you can sneak home just before your parents get home from work! They'll never even know. :)")
		},{
			new ClassifierClip(4, true, "classifier_8_0", "Model", "To: Friend\nFrom: Child\n\nWhen I was at Ruthea's place today, her dad said I was the most beautiful girl he'd ever seen and that I should be a model!"),
			new ClassifierClip(4, true, "classifier_8_1", "Understanding", "To: Child\nFrom: Uncle\n\nIt was good to talk to you the other day. You're like a grown-up. I can talk to you about anything. You're the only one who really understands me."),
			new ClassifierClip(2, true, "classifier_8_2", "Smart and pretty", "To: Child\nFrom: Grandfather\n\nYou're the smartest girl in our whole family, and you're far prettier than your sister! I love watching you."),
			new ClassifierClip(4, false, "classifier_8_3", "Listener", "To: Child\nFrom: Uncle\n\nYou're a really good listener! How did you remember that story I told you last week?"),
			new ClassifierClip(1, false, "classifier_8_6", "Stay up late", "To: Friend\nFrom: Child\n\nSometimes my parents let my sister and her boyfriend babysit me. They always want me to go to bed right away, so I stay up as long as I can!"),
			new ClassifierClip(2, true, "classifier_8_5", "Spending time", "Situation: Older sister's boyfriend is talking to child\n\nI know I'm your sister's boyfriend,\nbut I'd much rather spend time with you. You're a lot prettier than she is!"),
			new ClassifierClip(1, true, "classifier_8_7", "Good gamer", "To: Cousin\nFrom: Child\n\nThe man who lives next door has all the latest games! He keeps inviting me over to play, even though it feels a little weird being alone together. At least I always beat him!"),
			new ClassifierClip(4, false, "classifier_8_4", "Play games", "To: Friend\nFrom: Child\n\nI get to play all the newest video games with my friends when my parents take me to visit! It's so cool!")
		}
	};

	public void Setup()
	{
		heading.text = "";
		text.text = "";
		icon.color = new Color (0f, 0f, 0f, 0f);
		flag.gameObject.SetActive(false);
		if (GameController.GetString("prevScene") != "Sorter")
		{
			//If this is not a re-classification (coming back from sorter), then play the intro
			string debriefStub = "classifier_debrief_" + GameController.GetInt("classifier_level") + "_";
			GameController.Directions(new string[] { "~100", debriefStub + "0", debriefStub + "1", debriefStub + "2" }); //Tiny wait, without which this wasn't working???
			if (GameController.GetInt("classifier_level") == 0)
			{
				GameController.Directions(new string[] { "classifier_help_01", "classifier_help_02", "classifier_help_03" });
			}
		}
		else
		{
			GameController.Directions(new string[] { "classifier_help_04" });
		}
		for (int i = 0; i < 8; i++)
		{
			//If this is not a re-classification (coming back from sorter), then clear old answers. If this is a reclassification, and the answer is incorrect, clear it.
			if (GameController.GetString("prevScene") != "Sorter" || GameController.GetBool("classifier_answer_" + i) != GameController.GetBool("classifier_response_" + i))
			{
				GameController.SetBool("classifier_classified_" + i, false);
				GameController.SetBool("classifier_response_" + i, false);
				GameController.SetInt("classifier_type_" + i, clips[GameController.GetInt("classifier_level"), i].clipType);
				GameController.SetBool("classifier_answer_" + i, clips[GameController.GetInt("classifier_level"), i].needToTell);
				ColorBlock colors = buttons[i].colors;
				colors.normalColor = colors.highlightedColor = new Color(1f, 0.4f, 0f);
				buttons[i].colors = colors;
			}
			flags[i].enabled = GameController.GetBool("classifier_response_" + i);
			icons[i].sprite = iconSprites[clips[GameController.GetInt("classifier_level"), i].clipType];
			buttons[i].GetComponentInChildren<Text>().text = clips[GameController.GetInt("classifier_level"), i].heading;
		}
	}

	public void SelectClip(int clip)
	{
		ColorBlock colors;
		if (selectedClip > -1)
		{
			colors = buttons[selectedClip].colors;
			colors.normalColor = colors.highlightedColor = (GameController.GetBool("classifier_classified_" + selectedClip)) ? new Color(0.4f, 0.8f, 1f) : new Color(1f, 0.4f, 0f);
			buttons[selectedClip].colors = colors;
		}
		heading.text = clips[GameController.GetInt("classifier_level"), clip].heading;
		text.text = clips[GameController.GetInt("classifier_level"), clip].text;
		icon.sprite = iconSprites[clips[GameController.GetInt("classifier_level"), clip].clipType];
		icon.color = Color.white;
		flag.gameObject.SetActive(flags[clip].IsActive());
		AudioSource audioSource = GetComponent<AudioSource>();
		if (audioSource.isPlaying)
		{
			audioSource.Stop();
		}
		audioSource.PlayOneShot(Resources.Load<AudioClip>("Audio/en/classifier_" + GameController.GetInt("classifier_level") + "_" + clip));
		needToTellButton.interactable = true;
		doNotNeedToTellButton.interactable = true;
		selectedClip = clip;

		colors = buttons[selectedClip].colors;
		colors.normalColor = colors.highlightedColor = new Color(0f, 0.6f, 1f);
		buttons[selectedClip].colors = colors;

	}


	public void SetNeedToTell(bool value)
	{
		GameController.SetBool("classifier_classified_" + selectedClip, true);
		if (selectedClip > -1)
		{
			GameController.SetBool("classifier_response_" + selectedClip, value);
			flags[selectedClip].enabled = value;
			flag.gameObject.SetActive(value);
		}

	}

	public void Done()
	{
		
	}

}

class ClassifierClip {
	public int clipType;
	public bool needToTell;
	public string lineID;
	public string heading;
	public string text;

	public ClassifierClip (int clipType, bool needToTell, string lineID, string heading, string text)
	{
		this.clipType = clipType;
		this.needToTell = needToTell;
		this.lineID = lineID;
		this.heading = heading;
		this.text = text;
	}
}
