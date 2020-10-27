using System;

using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using EnigmaRampageLibrary;

namespace EnigmaRampageAndroidLibrary.Common
{
    /// <summary>
    /// Handles Puzzle Play functions
    /// </summary>
    public class PuzzlePlayer
    {      
        private static ImageView[] sPicBoxes;
        private static Bitmap[] sImages;
        private RelativeLayout mPuzzlePanel;
        private readonly int mCurrentLvl;

        /// <summary>
        /// Initialization
        /// </summary>
        public PuzzlePlayer(RelativeLayout puzzlePanel, int currentLvl)
        {            
            this.mPuzzlePanel = puzzlePanel;
            this.mCurrentLvl = currentLvl;
        }

        /// <summary>
        /// Method for breaking down the image into puzzle pieces
        /// </summary>
        /// <param name="puzzlePanel"></param>
        /// <param name="Lvl"></param>
        /// <param name="image"></param>
        /// <param name="context"></param>
        public void Play(Bitmap image, Context context)
        {
            // Clear the arrays
            sImages = null; 
            sPicBoxes = null;

            // Calling the Garbage Collector to clean up the heap
            GC.Collect();

            sImages = new Bitmap[mCurrentLvl];
            sPicBoxes = new ImageView[mCurrentLvl];

            int numRow = (int)Math.Sqrt(mCurrentLvl); // # of rows
            int numCol = numRow; // # of columns
            int unitX = mPuzzlePanel.Width / numRow; // Width of a single puzzle piece
            int unitY = mPuzzlePanel.Height / numCol; // Height of a single puzzle piece
            int[] indice = new int[mCurrentLvl]; // Helper array for shuffle
            mPuzzlePanel.RemoveAllViews(); // Remove all views from puzzlePanel layout

            // Draw all the puzzle pieces depending on the current level
            for (int i = 0; i < mCurrentLvl; i++)
            {
                indice[i] = i;
                if (sPicBoxes[i] == null)
                {
                    sPicBoxes[i] = new MyImageView(context, null); // Create a new instance of imageView if doesn't exists
                    sPicBoxes[i].SetOnTouchListener(new MyTouchListener(sImages, mCurrentLvl, sPicBoxes)); // Add Touch Listner for each imageView
                }

                ViewGroup parent = (ViewGroup)sPicBoxes[i].Parent; // Get the parent view into a ViewGroup
                if (parent != null)
                {
                    // Remove from the parent
                    parent.RemoveView(sPicBoxes[i]);
                }

                mPuzzlePanel.AddView(sPicBoxes[i]); // Add the imageView to the puzzlePanel layout

                // Set the picBox imageView bounds
                sPicBoxes[i].LayoutParameters.Width = unitX; // Set the Width of a imageView
                sPicBoxes[i].LayoutParameters.Height = unitY; // Set the Height of a imageView

                ((MyImageView)sPicBoxes[i]).Index = i; // Set the index for the picBox imageView
                BitmapMaker.CreateBitmapImage(image, sImages, i, numRow, numCol, unitX, unitY); // Draw the image piece into imageView

                // Set location of the picBox imageView
                sPicBoxes[i].SetX(unitX * (i % numCol)); // Set X position of imageView
                sPicBoxes[i].SetY(unitY * (i / numCol)); // Set Y position of imageView 

                // Set a boader to the picBox imageView
                sPicBoxes[i].SetPaddingRelative(2, 1, 2, 1); // Set padding to image pieces
                sPicBoxes[i].SetBackgroundColor(Color.DarkGray); // Set background color for the imageView
            }

            Shuffle(indice); // Shuffle image pieces and populate picBoxes

            // Check if the puzzle is still the same order after shuffling
            while (SuccessChecker.IsSuccessful(mCurrentLvl, sPicBoxes))
            {
                // If true, shuffle again
                Shuffle(indice);
            }
        }

        /// <summary>
        /// Method to shuffle and populate picBoxes with the image pieces
        /// </summary>
        /// <param name="indice"></param>
        private void Shuffle(int[] indice)
        {
            Shuffler.Shuffle(ref indice); // Call the shuffle method
            for (int i = 0; i < mCurrentLvl; i++)
            {
                // Set puzzle pieces into the picBoxes with the shuffled order
                sPicBoxes[i].SetImageBitmap(sImages[indice[i]]); // Set image
                ((MyImageView)sPicBoxes[i]).ImageIndex = indice[i]; // Set image index
            }
        }
    }
}