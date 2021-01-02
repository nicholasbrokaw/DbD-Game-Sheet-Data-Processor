using System;
using System.Collections.Generic;
using System.IO;

/**
 * @author Knilax
 * @desc Sheet in total, consisting of Entries
 */
public class Sheet
{

	// Properties
	public List<Entry> Entries = new List<Entry>();

	/**
   * @desc Constructor for path
   */
	public Sheet(string path)
	{

		// Open input file
		StreamReader sheetFile = null;
		try
		{
			sheetFile = new StreamReader(
				new FileStream(path, FileMode.Open));
		}
		catch (Exception exc)
		{
			Environment.Exit(1);
		}

		// Skip header
		for(int i = 0; i < 5; i++) sheetFile.ReadLine();

		// Add entries
		string currentLine;
		while ((currentLine = sheetFile.ReadLine()) != null)
    {
			Entries.Add(new Entry(currentLine));
    }

		// DEBUG
		foreach (Entry entry in Entries)
			entry.WriteAll();

		// Close input file
		sheetFile.Close();

	} // end Sheet constructor for path

	/**
	 * @author Nicholas Brokaw
	 * @desc Constructor for StreamReader
	 */
	public Sheet(StreamReader sheetFile)
	{
		if(sheetFile != null)
		{
			// Skip header
			for (int i = 0; i < 5; i++) sheetFile.ReadLine();

			// Add entries
			string currentLine;
			while ((currentLine = sheetFile.ReadLine()) != null)
			{
				Entries.Add(new Entry(currentLine));
			}

			// DEBUG
			foreach (Entry entry in Entries)
				entry.WriteAll();

			// Close input file
			sheetFile.Close();
		}
		else
		{
			Console.Error.Write("Sheet data was null. Is it missing from Google Drive?");
			Environment.Exit(1);
		}
	} // end Sheet constructor for StreamReader

	/**
	 * @desc Finds percent of perk appearance
	 * @param killer {bool} If killer perk
	 * @param perkName {string} Name of perk
	 */
	public float PerkAppearanceRate(bool killer, string perkToFind)
  {
		int totalEntries = 0;
		int totalAppearances = 0;
		foreach (Entry entry in Entries)
    {
			// Search killer's perks
			if (killer)
      {
				totalEntries += 1;
				foreach (string perk in entry.PerksKiller)
					if (perk == perkToFind) totalAppearances++;
      }
			// Search survivors' perks
			else
			{
				totalEntries += 4;
				foreach (string perk in entry.PerksSurvivor1)
					if (perk == perkToFind) totalAppearances++;
				foreach (string perk in entry.PerksSurvivor2)
					if (perk == perkToFind) totalAppearances++;
				foreach (string perk in entry.PerksSurvivor3)
					if (perk == perkToFind) totalAppearances++;
				foreach (string perk in entry.PerksSurvivor4)
					if (perk == perkToFind) totalAppearances++;
			}
		} // end foreach

		// Return percent
		return (float) Math.Round((double) totalAppearances / totalEntries * 100,
			2);

	} // end PerkAppearanceRate

} // end Sheet