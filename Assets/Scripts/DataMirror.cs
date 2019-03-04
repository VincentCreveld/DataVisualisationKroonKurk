using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DataMirror
{
	public static string 
		OPEN_QUESTION_1  = "Tijdstempel", 
		OPEN_QUESTION_2  = "Leeftijd", 
		OPEN_QUESTION_3  = "Geslacht", 
		OPEN_QUESTION_4  = "Leefsituatie", 
		OPEN_QUESTION_5  = "Ethniciteit", 
		OPEN_QUESTION_6  = "Ben je opgegroeid in Nederland?", 
		OPEN_QUESTION_7  = "Woonplaats", 
		OPEN_QUESTION_8  = "Wat voor sociale media gebruik jij?", 
		OPEN_QUESTION_9  = "Indien toepasselijk, welke TV programma's kijk jij?", 
		OPEN_QUESTION_10 = "Zijn er tv zenders waar je een voorkeur voor hebt, en indien ja, welke?", 
		OPEN_QUESTION_11 = "Indien toepasselijk, welke soorten boeken lees jij?", 
		OPEN_QUESTION_12 = "Indien toepasselijk, wat voor muziek luister je?", 
		OPEN_QUESTION_13 = "Indien toepasselijk, welke websites bezoek jij (behalve social media)?", 
		OPEN_QUESTION_14 = "Indien toepasselijk, welke games speel jij?", 
		OPEN_QUESTION_15 = "Mogen wij jouw social media gebruik volgen om verdere informatie te verzamelen. (Wij zullen nooit zonder je toestemming specifieke informatie doorgeven / gebruiken.)", 
		OPEN_QUESTION_16 = "Wat zijn jouw gebruikersnamen / tags waarop wij jou mogen volgen?", 
		OPEN_QUESTION_17 = "Welke thema's zijn voor jou belangrijk/interessant? (meerdere keuzes mogelijk)", 
		OPEN_QUESTION_18 = "Welke van deze vier archetypes past het beste bij jou?", 
		OPEN_QUESTION_19 = "E-mail", 
		OPEN_QUESTION_20 = "Wil je graag uitgenodigd worden voor eventuele toekomstige playtests, enquetes of soortgelijke tests?", 
		CLOSED_QUESTION_1  = "1. Ik zou liever op TV zijn dan er naar kijken.", 
		CLOSED_QUESTION_2  = "2. Ik kijk liever netflix met vrienden dan in mijn eentje.", 
		CLOSED_QUESTION_3  = "3. Ik kijk liever documentaires over problemen in de wereld dan natuurdocumentaires", 
		CLOSED_QUESTION_4  = "4. Ik kijk liever Eurovisie song festival dan Mythbusters", 
		CLOSED_QUESTION_5  = "5. Ik kijk liever The Voice of Holland dan dat ik Harry Potter lees", 
		CLOSED_QUESTION_6  = "6. Ik speel liever multiplayer dan singleplayer games", 
		CLOSED_QUESTION_7  = "7. Ik besteed mijn weekenden liever in de kroeg dan op de bank.", 
		CLOSED_QUESTION_8  = "8. Ik vind het belangrijk om het nieuws in de gaten te houden.", 
		CLOSED_QUESTION_9  = "9. Ik kijk liever naar internationaal nieuws dan naar nationaal nieuws", 
		CLOSED_QUESTION_10 = "10. Als ik tijdens het ontbijt televisie zou kijken dan zet ik eerder het nieuws aan dan ontbijtshows.", 
		CLOSED_QUESTION_11 = "11. Als ik wakker word post ik meteen iets op mijn social media", 
		CLOSED_QUESTION_12 = "12. Ik wil graag vlogs maken of livestreamen, of doe dit al.",
		CLOSED_QUESTION_13 = "13. Ik maak liever een blog over mijn leven dan over recepten", 
		CLOSED_QUESTION_14 = "14. Ik post liever op mijn social media dan dat ik andermans posts lees", 
		CLOSED_QUESTION_15 = "15. Ik bezoek regelmatig nieuwswebsites / nieuwsapps", 
		CLOSED_QUESTION_16 = "16. Ik reageer altijd meteen op social media berichten", 
		CLOSED_QUESTION_17 = "17. Ik ga graag de discussie aan in een comment sectie", 
		CLOSED_QUESTION_18 = "18. Ik gebruik social media om mijzelf te uiten.", 
		CLOSED_QUESTION_19 = "19. Ik deel veel informatie op social media.", 
		CLOSED_QUESTION_20 = "20. Als er iets belangrijks is gebeurd dan deel ik dat meteen met mijn volgers.";

	// Refer to static strings to see what question matches the deserialised answer.
	public string o1, o3, o4, o5, o6, o7, o8, o9, o10, o11, o12, o13, o14, o15, o16, o17, o18, o19, o20;
	public int o2;
	public int q1, q2, q3, q4, q5, q6, q7, q8, q9, q10, q11, q12, q13, q14, q15, q16, q17, q18, q19, q20;
}

[Serializable]
public class MirrorStorage
{
	public List<DataMirror> results;
	public List<int[]> axisVals;

	public void SetupAxisVals()
	{
		axisVals = new List<int[]>();
		for (int i = 0; i < results.Count; i++)
		{
			axisVals.Add(new int[20] {
				results[i].q1, results[i].q2, results[i].q3, results[i].q4, results[i].q5,
				results[i].q6, results[i].q7, results[i].q8, results[i].q9, results[i].q10,
				results[i].q11, results[i].q12, results[i].q13, results[i].q14, results[i].q15,
				results[i].q16, results[i].q17, results[i].q18, results[i].q19, results[i].q20 });
		}
	}
}

//map func
// 1-9 naar -2 -> 2
//
