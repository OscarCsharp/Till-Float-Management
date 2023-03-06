using System;
using System.Text;
using static FillfloatManagement;

class FillfloatManagement
{
    public class Till{

        public int fiftyrands { get; set; }
        public int twentyrands { get; set; }
        public int tenrands { get; set; }
        public int fiverands { get; set; }
        public int tworands { get; set; }
        public int onerands { get; set; }

        public Till(int change)
        {
            fiftyrands = (change / 50);
            change %= 50;

            twentyrands = (int)(change / 20);
            change %= 20;

            tenrands = (int)(change / 10);
            change %= 10;

            fiverands = (int)(change / 5);
            change %= 5;

            tworands = (int)(change / 2);
            change %= 2;

            onerands = (int)(change / 1);
            change %= 1;

        }

        public string RandNotesChnage(int note, string noteType, string currentNotesChange)
        {
            string changeAmount = currentNotesChange;
            if (note == 1)
            {
                if (changeAmount.Length > 0)
                {
                    changeAmount = changeAmount + "-" + noteType;
                }
                else
                {
                    changeAmount = changeAmount + noteType;
                }
                return changeAmount;
            }
            else if (note > 1)
            {
                for (int i = 0; i < note; i++)
                {

                    if (changeAmount.Length > 0)
                    {
                        changeAmount = changeAmount + "-"+noteType;
                    }
                    else
                    {
                        changeAmount = changeAmount + noteType;
                    }
                }
                return changeAmount;
            }
            return currentNotesChange;
        }



    }


    // Main Method
    static public void Main(String[] args)
    {
        float fiftyrands = 5*50;
        float twentyrands = 5*20;
        float tenrands = 6*10;
        float fiverands = 12*5;
        float tworands = 10*2;
        float onerands = 10;

        float totalrands = fiftyrands + twentyrands + tenrands + fiverands + tworands + onerands;

        var path = Environment.CurrentDirectory+"/items.txt";

        try
        {
            Console.WriteLine("{0,10} {1,10} {2,-5} {3,10} {4,10}","Till Start", "Transaction Total", "Paid", "Change Total", "Change Breakdown");
            //Create text file
            using (StreamWriter writer = System.IO.File.CreateText("TillSlip.txt"))
            {   string line = "Transaction Total, Paid, Change Total, Change Breakdown";
                writer.WriteLine(line);
            }


            string[] lines = File.ReadAllLines(path, Encoding.UTF8);

            foreach (string line in lines)
            {
                int comma = line.IndexOf(",");

                string tempLine = line.Substring(0, comma);
                string[] items = tempLine.Trim().Split(';').Select(sValue => sValue.Trim()).ToArray();
                List<string> costPrices = new List<string>();

                foreach (var item in items)
                {

                    string cost = item.Substring(item.IndexOf('R'));
                    costPrices.Add(cost.Trim());

                }

                float costTotal = getAmount(costPrices.ToArray());
                string notes = line.Substring(comma+1);


                string[] paidNotes = notes.Trim().Split('-').Select(sValue => sValue.Trim()).ToArray();

                float amountPaid = getAmount(paidNotes);
                Console.WriteLine("{0,-10} {1,-10} {2,4} {3,10} {4,26}", totalrands.ToString(), costTotal.ToString(), amountPaid.ToString(), (amountPaid - costTotal).ToString(), changeBreakdown((amountPaid - costTotal)));


                //Update file
                using (StreamWriter writer = System.IO.File.AppendText("TillSlip.txt"))
                {
                    string data = totalrands.ToString() + "," + costTotal.ToString() + "," + amountPaid.ToString() + "," + (amountPaid - costTotal).ToString() + "," + changeBreakdown((amountPaid - costTotal));
                    writer.WriteLine(data);
                }

                totalrands += (amountPaid - costTotal);
               
            }

            Console.WriteLine("Your Ouput file is at :" + Environment.CurrentDirectory + "/TillSlip.txt");
        }
        catch (FileNotFoundException ex)
        {
           Console.WriteLine(ex);
        }


       
       
        Console.ReadKey();
        Console.WriteLine("This is the End");

    }

    private static float getAmount(string[] notes)
    {
        float amoutPaid = 0;
        foreach (string note in notes)
        {
            if (note.Length > 0)
            {
                amoutPaid += float.Parse((note.Substring(1)));
            }
        }
        return amoutPaid;
    }

    private static string changeBreakdown(float change)
    {
        string changeAmount ="";
        int intChange = (int)change;
        Till till = new Till(intChange);

        if (intChange == 0)
        {
            changeAmount = "R0";
            return changeAmount;
        }
        else
        {
            changeAmount = "";
            changeAmount = till.RandNotesChnage(till.fiftyrands, "R50", changeAmount);
            changeAmount = till.RandNotesChnage(till.twentyrands, "R20", changeAmount);
            changeAmount = till.RandNotesChnage(till.fiverands, "R5", changeAmount);
            changeAmount = till.RandNotesChnage(till.tworands, "R2", changeAmount);
            changeAmount = till.RandNotesChnage(till.onerands, "R1", changeAmount);

            return changeAmount;
        
        }
    }
}


