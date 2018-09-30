using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Solitaire
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        Boolean s_matCard = false;
        Boolean s_potCard = false;
        Point currentMousePosition;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        List<Card> mystack;
        List<Card> cards;
        List<Card> cardsMat;
        List<Card> stack1;
        List<Card> stack2;
        List<Card> stack3;
        List<Card> stack4;
        List<Card> stack5;
        List<Card> stack6;
        List<Card> stack7;
        List<Card> stack;
        Texture2D card;
        Texture2D back;
        Texture2D beginPlayT;
        MouseState _currentMouseState;
        MouseState _previousMouseState;
        int currentCard;
        int curentCard;
        bool gameOver = true;
        bool againCards = true;
        bool beginPlayClicked;
        bool selectedMatCard = false;
        bool selectedPotCard = false;
        bool selectedStackCard = false;
        bool selectedAnywhere = false;
        int countStack1 = 0, countStack2 = 0, countStack3 = 0, countStack4 = 0;
        int insertStack1 = -1, insertStack2 = -1, insertStack3 = -1, insertStack4 = -1;
        string whichstack = "stack1";
        int selectedPast = -1;
        int selectedS = -1;
        int selectedStack = -1;
        int selectedStackIndex = -1;
        int selectedPaste = -1;
        int selectedPIndex = -1;
        int selectedPasteIndex = -1;
        bool continuePot = false;
        Form rules = new Form();
        Panel panel = new Panel();
        
        public class Card
        {
            public static int DIAMOND = 1;

            public static int HEART = 2;

            public static int CLUB = 3;

            public static int SPADE = 4;
            
            public int suit;

            public int rank;
            
            public string card;
            
            public Rectangle place;
            
            public string whatplace;

            public bool back;
        }

        static public class FisherYates
        {
            static Random r = new Random();

            static public List<Card> Shuffle(List<Card> deck)
            {
                for (int n = deck.Count - 1; n > 0; --n)
                {
                    int k = r.Next(n + 1);

                    int tempsuit = deck[n].suit;

                    int temp = deck[n].rank;

                    string card = deck[n].card;

                    deck[n].rank = deck[k].rank;

                    deck[k].rank = temp;

                    deck[n].suit = deck[k].suit;

                    deck[k].suit = tempsuit;

                    deck[n].card = deck[k].card;

                    deck[k].card = card;
                }

                return deck;
            }
        }

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 900;
            graphics.PreferredBackBufferHeight = 700;
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            SpriteBatchEx.GraphicsDevice = GraphicsDevice;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            spriteFont = Content.Load<SpriteFont>("SpriteFont1");
            back = Content.Load<Texture2D>("back");
            beginPlayT = Content.Load<Texture2D>("playBtn");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private bool rulesClick()
        {
            if (_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && _currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                var mousePosition = new Point(_currentMouseState.X, _currentMouseState.Y);

                Rectangle somRectangle = new Rectangle(20, 400, 150, 50);

                Rectangle are = somRectangle;

                if (are.Contains(mousePosition))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _previousMouseState = _currentMouseState;

            _currentMouseState = Mouse.GetState();

            if (rulesClick())
            {
                rules = new Form();
                rules.Width = 400;
                rules.Height = 330;
                panel = new Panel();
                panel.Width = rules.Width;
                panel.Height = rules.Height;
                rules.Controls.Add(panel);
                Label lbl = new Label();
                lbl.Text = "Rules de Solitaire";
                lbl.SetBounds(0, 30, 50, 30);
                panel.Controls.Add(lbl);
                TextBox tb = new TextBox();
                tb.Multiline = true;
                tb.WordWrap = true;
                tb.Text = "To select one of the seven stack cards, right click on the showing\r\n portion of the card"
                        + "in order to move the card or stack\r\n of cards under the selection.  For the last card, click only\r\n the"
                        + "location where the showing portion would be if it was\r\n not the last card.  Then left click similarly the 'showing' portion of one of the seven stacks"
                        + "to move the card or cards selected.\r\n  Similarly, also, left click one of the four top stacks to place a card (but only when it is the case that the last card of the seven stack cards' card\r\n is selected)."
                        + "  To advance through the hiding cards, left click the\r\n back card.  To paste a selected drawn card click the whole area with\r\n the left mouse button"
                        + "and to have it placed in one of the four top stacks\r\n left click the top stack you would have it placed on top of.\r\n"
                        + "Do the same to place A drawn card to one of the seven stacks (except in that case, you need to left click the 'showing' portion of the card\r\n your card will be on top of.";
                tb.SetBounds(50, 0, 350, 270);
                panel.Controls.Add(tb);
                Button ok = new Button();
                ok.Text = "OK";
                ok.SetBounds(180, 280, 50, 40);
                ok.MouseClick += (sender, e) =>
                {
                    rules.Visible = false;
                };
                panel.Controls.Add(ok);
                rules.MaximizeBox = false;
                rules.FormBorderStyle = FormBorderStyle.FixedSingle;
                rules.Height += 80;
                rules.Width += 50;
                rules.Visible = true;
            }

            if(beginPlay())
            {
                if (beginPlayClicked || againCards) {
                    s_matCard = false;
                    s_potCard = false;
                    dealCards();
                    againCards = false;
                    beginPlayClicked = false;
                }
            }

            if(nexts())
            {
                if (beginPlayClicked) {
                    if (cards.Count == 0)
                    {
                        currentCard = 0;
                        if (cardsMat.Count > 0)
                        {
                            for (int i = 0; i < cardsMat.Count; i++)
                                cards.Add(cardsMat[i]);
                            cardsMat.Clear();
                        }
                    }
                    try
                    {
                        s_matCard = false;
                        s_potCard = false;
                        cards[0].whatplace = "mat";
                        cardsMat.Add(cards[0]);
                        cards.Remove(cards[0]);
                        currentCard++;
                    } catch(Exception e)
                    {
                    }
                    beginPlayClicked = false;
                }
            }

            selectAnywhere();

            if (selectMatCard())
            {
                selectedMatCard = true;
                s_matCard = true;
            }

            if(selectedMatCard)
            {
                if (selectStackPaste())
                {
                    //selectedMatCard = false;
                    try
                    {
                        if (selectedPasteIndex != -1)
                        {
                            if (selectedPaste == 1 && selectedPasteIndex == stack1.Count - 1)
                            {
                                if (cardsMat[cardsMat.Count - 1].rank == stack1[stack1.Count - 1].rank - 1)
                                {
                                    stack1.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack1[stack1.Count - 1].back = false;
                                    stack1[stack1.Count - 1].whatplace = "stak1";
                                }
                            }
                            if (selectedPaste == 2 && selectedPasteIndex == stack2.Count - 1)
                            {
                                if (cardsMat[cardsMat.Count - 1].rank == stack2[stack2.Count - 1].rank - 1)
                                {
                                    stack2.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack2[stack2.Count - 1].back = false;
                                    stack2[stack2.Count - 1].whatplace = "stak2";
                                }
                            }
                            if (selectedPaste == 3 && selectedPasteIndex == stack3.Count - 1)
                            {
                                if (cardsMat[cardsMat.Count - 1].rank == stack3[stack3.Count - 1].rank - 1)
                                {
                                    stack3.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack3[stack3.Count - 1].back = false;
                                    stack3[stack3.Count - 1].whatplace = "stak3";
                                }
                            }
                            if (selectedPaste == 4 && selectedPasteIndex == stack4.Count - 1)
                            {
                                if (cardsMat[cardsMat.Count - 1].rank == stack4[stack4.Count - 1].rank - 1)
                                {
                                    stack4.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack4[stack4.Count - 1].back = false;
                                    stack4[stack4.Count - 1].whatplace = "stak4";
                                }
                            }
                            if (selectedPaste == 5 && selectedPasteIndex == stack5.Count - 1)
                            {
                                if (cardsMat[cardsMat.Count - 1].rank == stack5[stack5.Count - 1].rank - 1)
                                {
                                    stack5.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack5[stack5.Count - 1].back = false;
                                    stack5[stack5.Count - 1].whatplace = "stak5";
                                }
                            }
                            if (selectedPaste == 6 && selectedPasteIndex == stack6.Count - 1)
                            {
                                if (cardsMat[cardsMat.Count - 1].rank == stack6[stack6.Count - 1].rank - 1)
                                {
                                    stack6.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack6[stack6.Count - 1].back = false;
                                    stack6[stack6.Count - 1].whatplace = "stak6";
                                }
                            }
                            if (selectedPaste == 7 && selectedPasteIndex == stack7.Count - 1)
                            {
                                if (cardsMat[cardsMat.Count - 1].rank == stack7[stack7.Count - 1].rank - 1)
                                {
                                    stack7.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack7[stack7.Count - 1].back = false;
                                    stack7[stack7.Count - 1].whatplace = "stak7";
                                }
                            }
                        }
                        else if ((selectedPast == 1 && stack1.Count == 0) ||
                          (selectedPast == 2 && stack2.Count == 0) ||
                          (selectedPast == 3 && stack3.Count == 0) ||
                          (selectedPast == 4 && stack4.Count == 0) ||
                          (selectedPast == 5 && stack5.Count == 0) ||
                          (selectedPast == 6 && stack6.Count == 0) ||
                          (selectedPast == 7 && stack7.Count == 0))
                        {
                            if (selectedPast == 1 && selectedPasteIndex == -1)
                            {
                                if (cardsMat[cardsMat.Count - 1].rank == 13 ||
                                    cardsMat[cardsMat.Count - 1].rank == 1)
                                {
                                    stack1.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack1[stack1.Count - 1].back = false;
                                    stack1[stack1.Count - 1].whatplace = "stak1";
                                }
                            }
                            if (selectedPast == 2 && selectedPasteIndex == -1)
                            {
                                if (cardsMat[cardsMat.Count - 1].rank == 13 ||
                                    cardsMat[cardsMat.Count - 1].rank == 1)
                                {
                                    stack2.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack2[stack2.Count - 1].back = false;
                                    stack2[stack2.Count - 1].whatplace = "stak2";
                                }
                            }
                            if (selectedPast == 3 && selectedPasteIndex == -1)
                            {
                                if (cardsMat[cardsMat.Count - 1].rank == 13 ||
                                    cardsMat[cardsMat.Count - 1].rank == 1)
                                {
                                    stack3.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack3[stack3.Count - 1].back = false;
                                    stack3[stack3.Count - 1].whatplace = "stak3";
                                }
                            }
                            if (selectedPast == 4 && selectedPasteIndex == -1)
                            {
                                if (cardsMat[cardsMat.Count - 1].rank == 13 ||
                                    cardsMat[cardsMat.Count - 1].rank == 1)
                                {
                                    stack4.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack4[stack4.Count - 1].back = false;
                                    stack4[stack4.Count - 1].whatplace = "stak4";
                                }
                            }
                            if (selectedPast == 5 && selectedPasteIndex == -1)
                            {
                                if (cardsMat[cardsMat.Count - 1].rank == 13 ||
                                    cardsMat[cardsMat.Count - 1].rank == 1)
                                {
                                    stack5.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack5[stack5.Count - 1].back = false;
                                    stack5[stack5.Count - 1].whatplace = "stak5";
                                }
                            }
                            if (selectedPast == 6 && selectedPasteIndex == -1)
                            {
                                if (cardsMat[cardsMat.Count - 1].rank == 13 ||
                                    cardsMat[cardsMat.Count - 1].rank == 1)
                                {
                                    stack6.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack6[stack6.Count - 1].back = false;
                                    stack6[stack6.Count - 1].whatplace = "stak6";
                                }
                            }
                            if (selectedPast == 7 && selectedPasteIndex == -1)
                            {
                                if (cardsMat[cardsMat.Count - 1].rank == 13 ||
                                    cardsMat[cardsMat.Count - 1].rank == 1)
                                {
                                    stack7.Add(cardsMat[cardsMat.Count - 1]);
                                    cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                                    stack7[stack7.Count - 1].back = false;
                                    stack7[stack7.Count - 1].whatplace = "stak7";
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
                if (stackClick())
                {
                    int countStack = 0;
                    selectedMatCard = false;
                    s_matCard = false;
                    for (int i = 0; i < stack.Count; i++)
                    {
                        Card card = stack[i];
                        if (card.whatplace.Equals(whichstack))
                        {
                            countStack++;
                        }
                    }
                    if (curentCard != cardsMat.Count - 1 && curentCard > cardsMat.Count - 1)
                    {
                        curentCard = curentCard - 24;
                    }
                    if(curentCard < 0)
                    {
                        if (cardsMat.Count == 0)
                            curentCard = 0;
                        else if (cardsMat.Count > 0)
                            curentCard = cardsMat.Count - 1;
                    }
                    if (cardsMat.Count > 0 || 1 == 1)
                    {
                        if (countStack == 0 && cardsMat[cardsMat.Count - 1].card.Equals("ace"))
                        {
                            Card newCard = new Card();
                            newCard.card = "ace";
                            newCard.rank = cardsMat[cardsMat.Count - 1].rank;
                            newCard.suit = cardsMat[cardsMat.Count - 1].suit;
                            newCard.whatplace = whichstack;
                            stack.Add(newCard);
                            cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                        }
                        else
                        {
                            int nets = -1;
                            for (int i = 0; i < stack.Count; i++)
                            {
                                Card card = stack[i];
                                if (card.whatplace.Equals(whichstack))
                                {
                                    if (cardsMat[cardsMat.Count - 1].rank - 1 == card.rank && card.suit.Equals(cardsMat[cardsMat.Count - 1].suit))
                                        nets = i + 1;
                                }
                            }
                            if (nets != -1)
                            {
                                cardsMat[cardsMat.Count - 1].whatplace = whichstack;
                                stack.Insert(nets, cardsMat[cardsMat.Count - 1]);
                                cardsMat.Remove(cardsMat[cardsMat.Count - 1]);
                            }
                        }
                    }
                }
            }

            if(selectPotCard())
            {
                selectedStackCard = false;
                selectedPotCard = true;
                s_potCard = true;
            }

            if (selectedMatCard)
            {
                selectedStackCard = false;
                s_potCard = false;
            }

            if (selectedPotCard)
                s_matCard = false;

            if (selectStackCard())
            {
                selectedStackCard = true;
                selectedMatCard = false;
            }

            if (selectedStackCard)
            {
                s_matCard = false;
                if (stackClick())
                {
                    int countStack = 0;
                    int stackIndex = -1;
                    if (whichstack.Equals("stack1"))
                    {
                        for (int i = 0; i < stack.Count; i++)
                        {
                            if (stack[i].whatplace.Equals("stack1"))
                            {
                                countStack++;
                                stackIndex = i;
                            }
                        }
                    }
                    if (whichstack.Equals("stack2"))
                    {
                        for (int i = 0; i < stack.Count; i++)
                        {
                            if (stack[i].whatplace.Equals("stack2"))
                            {
                                countStack++;
                                stackIndex = i;
                            }
                        }
                    }
                    if (whichstack.Equals("stack3"))
                    {
                        for (int i = 0; i < stack.Count; i++)
                        {
                            if (stack[i].whatplace.Equals("stack3"))
                            {
                                countStack++;
                                stackIndex = i;
                            }
                        }
                    }
                    if (whichstack.Equals("stack4"))
                    {
                        for (int i = 0; i < stack.Count; i++)
                        {
                            if (stack[i].whatplace.Equals("stack4"))
                            {
                                countStack++;
                                stackIndex = i;
                            }
                        }
                    }
                    int compareTo = -1, compareTo2 = -1;
                    if (selectedStack == 1)
                    {
                        if (mystack.Count > 0)
                        {
                            if (countStack != 0)
                            {
                                compareTo = stack[stackIndex].rank + 1;
                                compareTo2 = stack[stackIndex].suit;
                            }
                            else
                            {
                                compareTo = 1;
                                compareTo2 = mystack[mystack.Count - 1].suit;
                            }
                            if (mystack[mystack.Count - 1].rank == compareTo &&
                                mystack[mystack.Count - 1].suit == compareTo2)
                            {
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = whichstack;
                                newCard.rank = compareTo;
                                newCard.suit = compareTo2;
                                int index = 0;
                                if (stackIndex == -1)
                                    index = 0;
                                else
                                    index = stackIndex + 1;
                                stack.Insert(index, newCard);
                                mystack.Clear();
                                if (stack1.Count != 0)
                                    stack1[stack1.Count - 1].back = false;
                            }
                        }
                    }
                    if (selectedStack == 2)
                    {
                        if (mystack.Count > 0)
                        {
                            if (countStack != 0)
                            {
                                compareTo = stack[stackIndex].rank + 1;
                                compareTo2 = stack[stackIndex].suit;
                            }
                            else
                            {
                                compareTo = 1;
                                compareTo2 = mystack[mystack.Count - 1].suit;
                            }
                            if (mystack[mystack.Count - 1].rank == compareTo &&
                                mystack[mystack.Count - 1].suit == compareTo2)
                            {
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = whichstack;
                                newCard.rank = compareTo;
                                newCard.suit = compareTo2;
                                int index = 0;
                                if (stackIndex == -1)
                                    index = 0;
                                else
                                    index = stackIndex + 1;
                                stack.Insert(index, newCard);
                                mystack.Clear();
                                if (stack2.Count != 0)
                                    stack2[stack2.Count - 1].back = false;
                            }
                        }
                    }
                    if (selectedStack == 3)
                    {
                        if (mystack.Count > 0)
                        {
                            if (countStack != 0)
                            {
                                compareTo = stack[stackIndex].rank + 1;
                                compareTo2 = stack[stackIndex].suit;
                            }
                            else
                            {
                                compareTo = 1;
                                compareTo2 = mystack[mystack.Count - 1].suit;
                            }
                            if (mystack[mystack.Count - 1].rank == compareTo &&
                                mystack[mystack.Count - 1].suit == compareTo2)
                            {
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = whichstack;
                                newCard.rank = compareTo;
                                newCard.suit = compareTo2;
                                int index = 0;
                                if (stackIndex == -1)
                                    index = 0;
                                else
                                    index = stackIndex + 1;
                                stack.Insert(index, newCard);
                                mystack.Clear();
                                if (stack3.Count != 0)
                                    stack3[stack3.Count - 1].back = false;
                            }
                        }
                    }
                    if (selectedStack == 4)
                    {
                        if (mystack.Count > 0)
                        {
                            if (countStack != 0)
                            {
                                compareTo = stack[stackIndex].rank + 1;
                                compareTo2 = stack[stackIndex].suit;
                            }
                            else
                            {
                                compareTo = 1;
                                compareTo2 = mystack[mystack.Count - 1].suit;
                            }
                            if (mystack[mystack.Count - 1].rank == compareTo &&
                                mystack[mystack.Count - 1].suit == compareTo2)
                            {
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = whichstack;
                                newCard.rank = compareTo;
                                newCard.suit = compareTo2;
                                int index = 0;
                                if (stackIndex == -1)
                                    index = 0;
                                else
                                    index = stackIndex + 1;
                                stack.Insert(index, newCard);
                                mystack.Clear();
                                if (stack4.Count != 0)
                                    stack4[stack4.Count - 1].back = false;
                            }
                        }
                    }
                    if (selectedStack == 5)
                    {
                        if (mystack.Count > 0)
                        {
                            if (countStack != 0)
                            {
                                compareTo = stack[stackIndex].rank + 1;
                                compareTo2 = stack[stackIndex].suit;
                            }
                            else
                            {
                                compareTo = 1;
                                compareTo2 = mystack[mystack.Count - 1].suit;
                            }
                            if (mystack[mystack.Count - 1].rank == compareTo &&
                                mystack[mystack.Count - 1].suit == compareTo2)
                            {
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = whichstack;
                                newCard.rank = compareTo;
                                newCard.suit = compareTo2;
                                int index = 0;
                                if (stackIndex == -1)
                                    index = 0;
                                else
                                    index = stackIndex + 1;
                                stack.Insert(index, newCard);
                                mystack.Clear();
                                if (stack5.Count != 0)
                                    stack5[stack5.Count - 1].back = false;
                            }
                        }
                    }
                    if (selectedStack == 6)
                    {
                        if (mystack.Count > 0)
                        {
                            if (countStack != 0)
                            {
                                compareTo = stack[stackIndex].rank + 1;
                                compareTo2 = stack[stackIndex].suit;
                            }
                            else
                            {
                                compareTo = 1;
                                compareTo2 = mystack[mystack.Count - 1].suit;
                            }
                            if (mystack[mystack.Count - 1].rank == compareTo &&
                                mystack[mystack.Count - 1].suit == compareTo2)
                            {
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = whichstack;
                                newCard.rank = compareTo;
                                newCard.suit = compareTo2;
                                int index = 0;
                                if (stackIndex == -1)
                                    index = 0;
                                else
                                    index = stackIndex + 1;
                                stack.Insert(index, newCard);
                                mystack.Clear();
                                if (stack6.Count != 0)
                                    stack6[stack6.Count - 1].back = false;
                            }
                        }
                    }
                    if (selectedStack == 7)
                    {
                        if (mystack.Count > 0)
                        {
                            if (countStack != 0)
                            {
                                compareTo = stack[stackIndex].rank + 1;
                                compareTo2 = stack[stackIndex].suit;
                            }
                            else
                            {
                                compareTo = 1;
                                compareTo2 = mystack[mystack.Count - 1].suit;
                            }
                            if (mystack[mystack.Count - 1].rank == compareTo &&
                                mystack[mystack.Count - 1].suit == compareTo2)
                            {
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = whichstack;
                                newCard.rank = compareTo;
                                newCard.suit = compareTo2;
                                int index = 0;
                                if (stackIndex == -1)
                                    index = 0;
                                else
                                    index = stackIndex + 1;
                                stack.Insert(index, newCard);
                                mystack.Clear();
                                if (stack7.Count != 0)
                                    stack7[stack7.Count - 1].back = false;
                            }
                        }
                    }
                }
                if (selectStackPaste())
                {
                    s_potCard = false;
                    Card org = null, dest = null;
                    try
                    {
                        if(mystack.Count > 0)
                        {
                            bool pass = true;
                            try
                            {
                                if (selectedPasteIndex == -1)
                                    selectedPasteIndex = 0;
                                if (selectedPaste == 1)
                                {
                                    if (selectedStack == 1)
                                    {
                                        org = stack1[selectedStackIndex];
                                    }
                                    if (selectedStack == 2)
                                    {
                                        org = stack2[selectedStackIndex];
                                    }
                                    if (selectedStack == 3)
                                    {
                                        org = stack3[selectedStackIndex];
                                    }
                                    if (selectedStack == 4)
                                    {
                                        org = stack4[selectedStackIndex];
                                    }
                                    if (selectedStack == 5)
                                    {
                                        org = stack5[selectedStackIndex];
                                    }
                                    if (selectedStack == 6)
                                    {
                                        org = stack6[selectedStackIndex];
                                    }
                                    if (selectedStack == 7)
                                    {
                                        org = stack7[selectedStackIndex];
                                    }
                                    if (stack1.Count == 1)
                                    {
                                        dest = stack1[0];
                                    }
                                    else
                                    {
                                        if (selectedPasteIndex < stack1.Count)
                                            dest = stack1[selectedPasteIndex];
                                        else if (stack1.Count == 0)
                                            dest = null;
                                    }
                                }
                                if (selectedPaste == 2)
                                {
                                    if (selectedStack == 1)
                                    {
                                        org = stack1[selectedStackIndex];
                                    }
                                    if (selectedStack == 2)
                                    {
                                        org = stack2[selectedStackIndex];
                                    }
                                    if (selectedStack == 3)
                                    {
                                        org = stack3[selectedStackIndex];
                                    }
                                    if (selectedStack == 4)
                                    {
                                        org = stack4[selectedStackIndex];
                                    }
                                    if (selectedStack == 5)
                                    {
                                        org = stack5[selectedStackIndex];
                                    }
                                    if (selectedStack == 6)
                                    {
                                        org = stack6[selectedStackIndex];
                                    }
                                    if (selectedStack == 7)
                                    {
                                        org = stack7[selectedStackIndex];
                                    }
                                    if (stack2.Count == 1)
                                    {
                                        dest = stack2[0];
                                    }
                                    else
                                    {
                                        if (selectedPasteIndex < stack2.Count)
                                            dest = stack2[selectedPasteIndex];
                                        else if (stack2.Count == 0)
                                            dest = null;
                                    }
                                }
                                if (selectedPaste == 3)
                                {
                                    if (selectedStack == 1)
                                    {
                                        org = stack1[selectedStackIndex];
                                    }
                                    if (selectedStack == 2)
                                    {
                                        org = stack2[selectedStackIndex];
                                    }
                                    if (selectedStack == 3)
                                    {
                                        org = stack3[selectedStackIndex];
                                    }
                                    if (selectedStack == 4)
                                    {
                                        org = stack4[selectedStackIndex];
                                    }
                                    if (selectedStack == 5)
                                    {
                                        org = stack5[selectedStackIndex];
                                    }
                                    if (selectedStack == 6)
                                    {
                                        org = stack6[selectedStackIndex];
                                    }
                                    if (selectedStack == 7)
                                    {
                                        org = stack7[selectedStackIndex];
                                    }
                                    if (stack3.Count == 1)
                                    {
                                        dest = stack3[0];
                                    }
                                    else
                                    {
                                        if (selectedPasteIndex < stack3.Count)
                                            dest = stack3[selectedPasteIndex];
                                        else if (stack3.Count == 0)
                                            dest = null;
                                    }
                                }
                                if (selectedPaste == 4)
                                {
                                    if (selectedStack == 1)
                                    {
                                        org = stack1[selectedStackIndex];
                                    }
                                    if (selectedStack == 2)
                                    {
                                        org = stack2[selectedStackIndex];
                                    }
                                    if (selectedStack == 3)
                                    {
                                        org = stack3[selectedStackIndex];
                                    }
                                    if (selectedStack == 4)
                                    {
                                        org = stack4[selectedStackIndex];
                                    }
                                    if (selectedStack == 5)
                                    {
                                        org = stack5[selectedStackIndex];
                                    }
                                    if (selectedStack == 6)
                                    {
                                        org = stack6[selectedStackIndex];
                                    }
                                    if (selectedStack == 7)
                                    {
                                        org = stack7[selectedStackIndex];
                                    }
                                    if (stack4.Count == 1)
                                    {
                                        dest = stack4[0];
                                    }
                                    else
                                    {
                                        if (selectedPasteIndex < stack4.Count)
                                            dest = stack4[selectedPasteIndex];
                                        else if (stack4.Count == 0)
                                            dest = null;
                                    }
                                }
                                if (selectedPaste == 5)
                                {
                                    if (selectedStack == 1)
                                    {
                                        org = stack1[selectedStackIndex];
                                    }
                                    if (selectedStack == 2)
                                    {
                                        org = stack2[selectedStackIndex];
                                    }
                                    if (selectedStack == 3)
                                    {
                                        org = stack3[selectedStackIndex];
                                    }
                                    if (selectedStack == 4)
                                    {
                                        org = stack4[selectedStackIndex];
                                    }
                                    if (selectedStack == 5)
                                    {
                                        org = stack5[selectedStackIndex];
                                    }
                                    if (selectedStack == 6)
                                    {
                                        org = stack6[selectedStackIndex];
                                    }
                                    if (selectedStack == 7)
                                    {
                                        org = stack7[selectedStackIndex];
                                    }
                                    if (stack5.Count == 1)
                                    {
                                        dest = stack5[0];
                                    }
                                    else
                                    {
                                        if (selectedPasteIndex < stack5.Count)
                                            dest = stack5[selectedPasteIndex];
                                        else if (stack5.Count == 0)
                                            dest = null;
                                    }
                                }
                                if (selectedPaste == 6)
                                {
                                    if (selectedStack == 1)
                                    {
                                        org = stack1[selectedStackIndex];
                                    }
                                    if (selectedStack == 2)
                                    {
                                        org = stack2[selectedStackIndex];
                                    }
                                    if (selectedStack == 3)
                                    {
                                        org = stack3[selectedStackIndex];
                                    }
                                    if (selectedStack == 4)
                                    {
                                        org = stack4[selectedStackIndex];
                                    }
                                    if (selectedStack == 5)
                                    {
                                        org = stack5[selectedStackIndex];
                                    }
                                    if (selectedStack == 6)
                                    {
                                        org = stack6[selectedStackIndex];
                                    }
                                    if (selectedStack == 7)
                                    {
                                        org = stack7[selectedStackIndex];
                                    }
                                    if (stack6.Count == 1)
                                    {
                                        dest = stack6[0];
                                    }
                                    else
                                    {
                                        if (selectedPasteIndex < stack6.Count)
                                            dest = stack6[selectedPasteIndex];
                                        else if (stack6.Count == 0)
                                            dest = null;
                                    }
                                }
                                if (selectedPaste == 7)
                                {
                                    if (selectedStack == 1)
                                    {
                                        org = stack1[selectedStackIndex];
                                    }
                                    if (selectedStack == 2)
                                    {
                                        org = stack2[selectedStackIndex];
                                    }
                                    if (selectedStack == 3)
                                    {
                                        org = stack3[selectedStackIndex];
                                    }
                                    if (selectedStack == 4)
                                    {
                                        org = stack4[selectedStackIndex];
                                    }
                                    if (selectedStack == 5)
                                    {
                                        org = stack5[selectedStackIndex];
                                    }
                                    if (selectedStack == 6)
                                    {
                                        org = stack6[selectedStackIndex];
                                    }
                                    if (selectedStack == 7)
                                    {
                                        org = stack7[selectedStackIndex];
                                    }
                                    if (stack7.Count == 1)
                                    {
                                        dest = stack7[0];
                                    }
                                    else
                                    {
                                        if (selectedPasteIndex < stack7.Count)
                                            dest = stack7[selectedPasteIndex];
                                        else if (stack7.Count == 0)
                                            dest = null;
                                    }
                                }
                                if (dest != null)
                                {
                                    if (dest.rank - 1 == org.rank)
                                        pass = true;
                                }
                                else
                                {
                                    if (org.rank == 13)
                                        pass = true;
                                }
                            }
                            catch(Exception e)
                            {
                            }
                            pass = true;
                            if (selectedPaste == 1)
                            {
                                if (pass || (selectedPasteIndex == 0 && stack1.Count == 0))
                                {
                                    if (1 == 1 || mystack[selectedStackIndex].back == false)
                                    {
                                        int ss = 0;
                                        Boolean isit = false;
                                        if (stack1.Count > 0)
                                        {
                                            if (mystack[0].suit == Card.SPADE && (stack1[stack1.Count - 1].suit == Card.HEART || stack1[stack1.Count - 1].suit == Card.DIAMOND))
                                                isit = true;
                                            if (mystack[0].suit == Card.CLUB && (stack1[stack1.Count - 1].suit == Card.HEART || stack1[stack1.Count - 1].suit == Card.DIAMOND))
                                                isit = true;
                                            if (mystack[0].suit == Card.HEART && (stack1[stack1.Count - 1].suit == Card.SPADE || stack1[stack1.Count - 1].suit == Card.CLUB))
                                                isit = true;
                                            if (mystack[0].suit == Card.DIAMOND && (stack1[stack1.Count - 1].suit == Card.SPADE || stack1[stack1.Count - 1].suit == Card.CLUB))
                                                isit = true;
                                            if (stack1[stack1.Count - 1].back == true)
                                                isit = true;
                                            else if (mystack[0].rank != stack1[stack1.Count - 1].rank - 1)
                                                isit = false;
                                        }
                                        else if(mystack[0].card.Equals("king") || mystack[0].card.Equals("ace"))
                                            isit = true;
                                        if (isit)
                                        {
                                            while (ss < mystack.Count)
                                            {
                                                stack1.Add(mystack[ss]);
                                                mystack.Remove(mystack[ss]);

                                                stack1[stack1.Count - 1].whatplace = "stak1";
                                            }
                                            if (selectedStack == 1)
                                            {
                                                stack1[stack1.Count - 1].back = false;
                                            }
                                            if (selectedStack == 2)
                                            {
                                                stack2[stack2.Count - 1].back = false;
                                            }
                                            if (selectedStack == 3)
                                            {
                                                stack3[stack3.Count - 1].back = false;
                                            }
                                            if (selectedStack == 4)
                                            {
                                                stack4[stack4.Count - 1].back = false;
                                            }
                                            if (selectedStack == 5)
                                            {
                                                stack5[stack5.Count - 1].back = false;
                                            }
                                            if (selectedStack == 6)
                                            {
                                                stack6[stack6.Count - 1].back = false;
                                            }
                                            if (selectedStack == 7)
                                            {
                                                stack7[stack7.Count - 1].back = false;
                                            }
                                        }
                                    }
                                    if (selectedStack == 3 && stack3[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack3.Count)
                                        {
                                            stack1.Add(stack3[selectedStackIndex]);
                                            stack3.Remove(stack3[selectedStackIndex]);

                                            if (stack3.Count != 0)
                                                stack3[stack3.Count - 1].back = false;
                                            stack1[stack1.Count - 1].whatplace = "stak1";
                                        }
                                    }
                                    if (selectedStack == 4 && stack4[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack4.Count)
                                        {
                                            stack1.Add(stack4[selectedStackIndex]);
                                            stack4.Remove(stack4[selectedStackIndex]);

                                            if (stack4.Count != 0)
                                                stack4[stack4.Count - 1].back = false;
                                            stack1[stack1.Count - 1].whatplace = "stak1";
                                        }
                                    }
                                    if (selectedStack == 5 && stack5[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack5.Count)
                                        {
                                            stack1.Add(stack5[selectedStackIndex]);
                                            stack5.Remove(stack5[selectedStackIndex]);

                                            if (stack5.Count != 0)
                                                stack5[stack5.Count - 1].back = false;
                                            stack1[stack1.Count - 1].whatplace = "stak1";
                                        }
                                    }
                                    if (selectedStack == 6 && stack6[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack6.Count)
                                        {
                                            stack1.Add(stack6[selectedStackIndex]);
                                            stack6.Remove(stack6[selectedStackIndex]);

                                            if (stack6.Count != 0)
                                                stack6[stack6.Count - 1].back = false;
                                            stack1[stack1.Count - 1].whatplace = "stak1";
                                        }
                                    }
                                    if (selectedStack == 7 && stack7[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack7.Count)
                                        {
                                            stack1.Add(stack7[selectedStackIndex]);
                                            stack7.Remove(stack7[selectedStackIndex]);

                                            if (stack7.Count != 0)
                                                stack7[stack7.Count - 1].back = false;
                                            stack1[stack1.Count - 1].whatplace = "stak1";
                                        }
                                    }
                                }
                            }
                            if (selectedPaste == 2)
                            {
                                if (pass || (selectedPasteIndex == 0 && stack2.Count == 0))
                                {
                                    if (1 == 1 || mystack[selectedStackIndex].back == false)
                                    {
                                        int ss = 0;
                                        Boolean isit = false;
                                        if (stack2.Count > 0)
                                        {
                                            if (mystack[0].suit == Card.SPADE && (stack2[stack2.Count - 1].suit == Card.HEART || stack2[stack2.Count - 1].suit == Card.DIAMOND))
                                                isit = true;
                                            if (mystack[0].suit == Card.CLUB && (stack2[stack2.Count - 1].suit == Card.HEART || stack2[stack2.Count - 1].suit == Card.DIAMOND))
                                                isit = true;
                                            if (mystack[0].suit == Card.HEART && (stack2[stack2.Count - 1].suit == Card.SPADE || stack2[stack2.Count - 1].suit == Card.CLUB))
                                                isit = true;
                                            if (mystack[0].suit == Card.DIAMOND && (stack2[stack2.Count - 1].suit == Card.SPADE || stack2[stack2.Count - 1].suit == Card.CLUB))
                                                isit = true;
                                            if (stack2[stack2.Count - 1].back == true)
                                                isit = true;
                                            else if (mystack[0].rank != stack2[stack2.Count - 1].rank - 1)
                                                isit = false;
                                        }
                                        else if (mystack[0].card.Equals("king") || mystack[0].card.Equals("ace"))
                                            isit = true;
                                        if (isit)
                                        {
                                            while (ss < mystack.Count)
                                            {
                                                stack2.Add(mystack[ss]);
                                                mystack.Remove(mystack[ss]);

                                                stack2[stack2.Count - 1].whatplace = "stak2";
                                            }
                                            if (selectedStack == 1)
                                            {
                                                stack1[stack1.Count - 1].back = false;
                                            }
                                            if (selectedStack == 2)
                                            {
                                                stack2[stack2.Count - 1].back = false;
                                            }
                                            if (selectedStack == 3)
                                            {
                                                stack3[stack3.Count - 1].back = false;
                                            }
                                            if (selectedStack == 4)
                                            {
                                                stack4[stack4.Count - 1].back = false;
                                            }
                                            if (selectedStack == 5)
                                            {
                                                stack5[stack5.Count - 1].back = false;
                                            }
                                            if (selectedStack == 6)
                                            {
                                                stack6[stack6.Count - 1].back = false;
                                            }
                                            if (selectedStack == 7)
                                            {
                                                stack7[stack7.Count - 1].back = false;
                                            }
                                        }
                                    }
                                    if (selectedStack == 3 && stack3[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack3.Count)
                                        {
                                            stack2.Add(stack3[selectedStackIndex]);
                                            stack3.Remove(stack3[selectedStackIndex]);

                                            if (stack3.Count != 0)
                                                stack3[stack3.Count - 1].back = false;
                                            stack2[stack2.Count - 1].whatplace = "stak2";
                                        }
                                    }
                                    if (selectedStack == 4 && stack4[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack4.Count)
                                        {
                                            stack2.Add(stack4[selectedStackIndex]);
                                            stack4.Remove(stack4[selectedStackIndex]);

                                            if (stack4.Count != 0)
                                                stack4[stack4.Count - 1].back = false;
                                            stack2[stack2.Count - 1].whatplace = "stak2";
                                        }
                                    }
                                    if (selectedStack == 5 && stack5[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack5.Count)
                                        {
                                            stack2.Add(stack5[selectedStackIndex]);
                                            stack5.Remove(stack5[selectedStackIndex]);

                                            if (stack5.Count != 0)
                                                stack5[stack5.Count - 1].back = false;
                                            stack2[stack2.Count - 1].whatplace = "stak2";
                                        }
                                    }
                                    if (selectedStack == 6 && stack6[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack6.Count)
                                        {
                                            stack2.Add(stack6[selectedStackIndex]);
                                            stack6.Remove(stack6[selectedStackIndex]);

                                            if (stack6.Count != 0)
                                                stack6[stack6.Count - 1].back = false;
                                            stack2[stack2.Count - 1].whatplace = "stak2";
                                        }
                                    }
                                    if (selectedStack == 7 && stack7[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack7.Count)
                                        {
                                            stack2.Add(stack7[selectedStackIndex]);
                                            stack7.Remove(stack7[selectedStackIndex]);

                                            if (stack7.Count != 0)
                                                stack7[stack7.Count - 1].back = false;
                                            stack2[stack2.Count - 1].whatplace = "stak2";
                                        }
                                    }
                                }
                            }
                            if (selectedPaste == 3)
                            {
                                if (pass || (selectedPasteIndex == 0 && stack3.Count == 0))
                                {
                                    if (1 == 1 || mystack[selectedStackIndex].back == false)
                                    {
                                        int ss = 0;
                                        Boolean isit = false;
                                        if (stack3.Count > 0)
                                        {
                                            if (mystack[0].suit == Card.SPADE && (stack3[stack3.Count - 1].suit == Card.HEART || stack3[stack3.Count - 1].suit == Card.DIAMOND))
                                                isit = true;
                                            if (mystack[0].suit == Card.CLUB && (stack3[stack3.Count - 1].suit == Card.HEART || stack3[stack3.Count - 1].suit == Card.DIAMOND))
                                                isit = true;
                                            if (mystack[0].suit == Card.HEART && (stack3[stack3.Count - 1].suit == Card.SPADE || stack3[stack3.Count - 1].suit == Card.CLUB))
                                                isit = true;
                                            if (mystack[0].suit == Card.DIAMOND && (stack3[stack3.Count - 1].suit == Card.SPADE || stack3[stack3.Count - 1].suit == Card.CLUB))
                                                isit = true;
                                            if (stack3[stack3.Count - 1].back == true)
                                                isit = true;
                                            else if (mystack[0].rank != stack3[stack3.Count - 1].rank - 1)
                                                isit = false;
                                        }
                                        else if (mystack[0].card.Equals("king") || mystack[0].card.Equals("ace"))
                                            isit = true;
                                        if (isit)
                                        {
                                            while (ss < mystack.Count)
                                            {
                                                stack3.Add(mystack[ss]);
                                                mystack.Remove(mystack[ss]);

                                                stack3[stack3.Count - 1].whatplace = "stak3";
                                            }
                                            if (selectedStack == 1)
                                            {
                                                stack1[stack1.Count - 1].back = false;
                                            }
                                            if (selectedStack == 2)
                                            {
                                                stack2[stack2.Count - 1].back = false;
                                            }
                                            if (selectedStack == 3)
                                            {
                                                stack3[stack3.Count - 1].back = false;
                                            }
                                            if (selectedStack == 4)
                                            {
                                                stack4[stack4.Count - 1].back = false;
                                            }
                                            if (selectedStack == 5)
                                            {
                                                stack5[stack5.Count - 1].back = false;
                                            }
                                            if (selectedStack == 6)
                                            {
                                                stack6[stack6.Count - 1].back = false;
                                            }
                                            if (selectedStack == 7)
                                            {
                                                stack7[stack7.Count - 1].back = false;
                                            }
                                        }
                                    }
                                    if (selectedStack == 2 && stack2[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack2.Count)
                                        {
                                            stack3.Add(stack2[selectedStackIndex]);
                                            stack2.Remove(stack2[selectedStackIndex]);

                                            if (stack2.Count != 0)
                                                stack2[stack2.Count - 1].back = false;
                                            stack3[stack3.Count - 1].whatplace = "stak3";
                                        }
                                    }
                                    if (selectedStack == 4 && stack4[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack4.Count)
                                        {
                                            stack3.Add(stack4[selectedStackIndex]);
                                            stack4.Remove(stack4[selectedStackIndex]);

                                            if (stack4.Count != 0)
                                                stack4[stack4.Count - 1].back = false;
                                            stack3[stack3.Count - 1].whatplace = "stak3";
                                        }
                                    }
                                    if (selectedStack == 5 && stack5[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack5.Count)
                                        {
                                            stack3.Add(stack5[selectedStackIndex]);
                                            stack5.Remove(stack5[selectedStackIndex]);

                                            if (stack5.Count != 0)
                                                stack5[stack5.Count - 1].back = false;
                                            stack3[stack3.Count - 1].whatplace = "stak3";
                                        }
                                    }
                                    if (selectedStack == 6 && stack6[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack6.Count)
                                        {
                                            stack3.Add(stack6[selectedStackIndex]);
                                            stack6.Remove(stack6[selectedStackIndex]);

                                            if (stack6.Count != 0)
                                                stack6[stack6.Count - 1].back = false;
                                            stack3[stack3.Count - 1].whatplace = "stak3";
                                        }
                                    }
                                    if (selectedStack == 7 && stack7[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack7.Count)
                                        {
                                            stack3.Add(stack7[selectedStackIndex]);
                                            stack7.Remove(stack7[selectedStackIndex]);

                                            if (stack7.Count != 0)
                                                stack7[stack7.Count - 1].back = false;
                                            stack3[stack3.Count - 1].whatplace = "stak3";
                                        }
                                    }
                                }
                            }
                            if (selectedPaste == 4)
                            {
                                if (pass || (selectedPasteIndex == 0 && stack4.Count == 0))
                                {
                                    if (1 == 1 || mystack[selectedStackIndex].back == false)
                                    {
                                        int ss = 0;
                                        Boolean isit = false;
                                        if (stack4.Count > 0)
                                        {
                                            if (mystack[0].suit == Card.SPADE && (stack4[stack4.Count - 1].suit == Card.HEART || stack4[stack4.Count - 1].suit == Card.DIAMOND))
                                                isit = true;
                                            if (mystack[0].suit == Card.CLUB && (stack4[stack4.Count - 1].suit == Card.HEART || stack4[stack4.Count - 1].suit == Card.DIAMOND))
                                                isit = true;
                                            if (mystack[0].suit == Card.HEART && (stack4[stack4.Count - 1].suit == Card.SPADE || stack4[stack4.Count - 1].suit == Card.CLUB))
                                                isit = true;
                                            if (mystack[0].suit == Card.DIAMOND && (stack4[stack4.Count - 1].suit == Card.SPADE || stack4[stack4.Count - 1].suit == Card.CLUB))
                                                isit = true;
                                            if (stack4[stack4.Count - 1].back == true)
                                                isit = true;
                                            else if (mystack[0].rank != stack4[stack4.Count - 1].rank - 1)
                                                isit = false;
                                        }
                                        else if (mystack[0].card.Equals("king") || mystack[0].card.Equals("ace"))
                                            isit = true;
                                        if (isit)
                                        {
                                            while (ss < mystack.Count)
                                            {
                                                stack4.Add(mystack[ss]);
                                                mystack.Remove(mystack[ss]);

                                                stack4[stack4.Count - 1].whatplace = "stak4";
                                            }
                                            if (selectedStack == 1)
                                            {
                                                stack1[stack1.Count - 1].back = false;
                                            }
                                            if (selectedStack == 2)
                                            {
                                                stack2[stack2.Count - 1].back = false;
                                            }
                                            if (selectedStack == 3)
                                            {
                                                stack3[stack3.Count - 1].back = false;
                                            }
                                            if (selectedStack == 4)
                                            {
                                                stack4[stack4.Count - 1].back = false;
                                            }
                                            if (selectedStack == 5)
                                            {
                                                stack5[stack5.Count - 1].back = false;
                                            }
                                            if (selectedStack == 6)
                                            {
                                                stack6[stack6.Count - 1].back = false;
                                            }
                                            if (selectedStack == 7)
                                            {
                                                stack7[stack7.Count - 1].back = false;
                                            }
                                        }
                                    }
                                    if (selectedStack == 2 && stack2[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack2.Count)
                                        {
                                            stack4.Add(stack2[selectedStackIndex]);
                                            stack2.Remove(stack2[selectedStackIndex]);

                                            if (stack2.Count != 0)
                                                stack2[stack2.Count - 1].back = false;
                                            stack4[stack4.Count - 1].whatplace = "stak4";
                                        }
                                    }
                                    if (selectedStack == 3 && stack3[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack3.Count)
                                        {
                                            stack4.Add(stack3[selectedStackIndex]);
                                            stack3.Remove(stack3[selectedStackIndex]);

                                            if (stack3.Count != 0)
                                                stack3[stack3.Count - 1].back = false;
                                            stack4[stack4.Count - 1].whatplace = "stak4";
                                        }
                                    }
                                    if (selectedStack == 5 && stack5[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack5.Count)
                                        {
                                            stack4.Add(stack5[selectedStackIndex]);
                                            stack5.Remove(stack5[selectedStackIndex]);

                                            if (stack5.Count != 0)
                                                stack5[stack5.Count - 1].back = false;
                                            stack4[stack4.Count - 1].whatplace = "stak4";
                                        }
                                    }
                                    if (selectedStack == 6 && stack6[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack6.Count)
                                        {
                                            stack4.Add(stack6[selectedStackIndex]);
                                            stack6.Remove(stack6[selectedStackIndex]);

                                            if (stack6.Count != 0)
                                                stack6[stack6.Count - 1].back = false;
                                            stack4[stack4.Count - 1].whatplace = "stak4";
                                        }
                                    }
                                    if (selectedStack == 7 && stack7[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack7.Count)
                                        {
                                            stack4.Add(stack7[selectedStackIndex]);
                                            stack7.Remove(stack7[selectedStackIndex]);

                                            if (stack7.Count != 0)
                                                stack7[stack7.Count - 1].back = false;
                                            stack4[stack4.Count - 1].whatplace = "stak4";
                                        }
                                    }
                                }
                            }
                            if (selectedPaste == 5)
                            {
                                if (pass || (selectedPasteIndex == 0 && stack5.Count == 0))
                                {
                                    if (1 == 1 || mystack[selectedStackIndex].back == false)
                                    {
                                        int ss = 0;
                                        Boolean isit = false;
                                        if (stack5.Count > 0)
                                        {
                                            if (mystack[0].suit == Card.SPADE && (stack5[stack5.Count - 1].suit == Card.HEART || stack5[stack5.Count - 1].suit == Card.DIAMOND))
                                                isit = true;
                                            if (mystack[0].suit == Card.CLUB && (stack5[stack5.Count - 1].suit == Card.HEART || stack5[stack5.Count - 1].suit == Card.DIAMOND))
                                                isit = true;
                                            if (mystack[0].suit == Card.HEART && (stack5[stack5.Count - 1].suit == Card.SPADE || stack5[stack5.Count - 1].suit == Card.CLUB))
                                                isit = true;
                                            if (mystack[0].suit == Card.DIAMOND && (stack5[stack5.Count - 1].suit == Card.SPADE || stack5[stack5.Count - 1].suit == Card.CLUB))
                                                isit = true;
                                            if (stack5[stack5.Count - 1].back == true)
                                                isit = true;
                                            else if (mystack[0].rank != stack5[stack5.Count - 1].rank - 1)
                                                isit = false;
                                        }
                                        else if (mystack[0].card.Equals("king") || mystack[0].card.Equals("ace"))
                                            isit = true;
                                        if (isit)
                                        {
                                            while (ss < mystack.Count)
                                            {
                                                stack5.Add(mystack[ss]);
                                                mystack.Remove(mystack[ss]);

                                                stack5[stack5.Count - 1].whatplace = "stak5";
                                            }
                                            if (selectedStack == 1)
                                            {
                                                stack1[stack1.Count - 1].back = false;
                                            }
                                            if (selectedStack == 2)
                                            {
                                                stack2[stack2.Count - 1].back = false;
                                            }
                                            if (selectedStack == 3)
                                            {
                                                stack3[stack3.Count - 1].back = false;
                                            }
                                            if (selectedStack == 4)
                                            {
                                                stack4[stack4.Count - 1].back = false;
                                            }
                                            if (selectedStack == 5)
                                            {
                                                stack5[stack5.Count - 1].back = false;
                                            }
                                            if (selectedStack == 6)
                                            {
                                                stack6[stack6.Count - 1].back = false;
                                            }
                                            if (selectedStack == 7)
                                            {
                                                stack7[stack7.Count - 1].back = false;
                                            }
                                        }
                                    }
                                    if (selectedStack == 2 && stack2[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack2.Count)
                                        {
                                            stack5.Add(stack2[selectedStackIndex]);
                                            stack2.Remove(stack2[selectedStackIndex]);

                                            if (stack2.Count != 0)
                                                stack2[stack2.Count - 1].back = false;
                                            stack5[stack5.Count - 1].whatplace = "stak5";
                                        }
                                    }
                                    if (selectedStack == 3 && stack3[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack3.Count)
                                        {
                                            stack5.Add(stack3[selectedStackIndex]);
                                            stack3.Remove(stack3[selectedStackIndex]);

                                            if (stack3.Count != 0)
                                                stack3[stack3.Count - 1].back = false;
                                            stack5[stack5.Count - 1].whatplace = "stak5";
                                        }
                                    }
                                    if (selectedStack == 4 && stack4[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack4.Count)
                                        {
                                            stack5.Add(stack4[selectedStackIndex]);
                                            stack4.Remove(stack4[selectedStackIndex]);

                                            if (stack4.Count != 0)
                                                stack4[stack4.Count - 1].back = false;
                                            stack5[stack5.Count - 1].whatplace = "stak5";
                                        }
                                    }
                                    if (selectedStack == 6 && stack6[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack6.Count)
                                        {
                                            stack5.Add(stack6[selectedStackIndex]);
                                            stack6.Remove(stack6[selectedStackIndex]);

                                            if (stack6.Count != 0)
                                                stack6[stack6.Count - 1].back = false;
                                            stack5[stack5.Count - 1].whatplace = "stak5";
                                        }
                                    }
                                    if (selectedStack == 7 && stack7[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack7.Count)
                                        {
                                            stack5.Add(stack7[selectedStackIndex]);
                                            stack7.Remove(stack7[selectedStackIndex]);

                                            if (stack7.Count != 0)
                                                stack7[stack7.Count - 1].back = false;
                                            stack5[stack5.Count - 1].whatplace = "stak5";
                                        }
                                    }
                                }
                            }
                            if (selectedPaste == 6)
                            {
                                if (pass || (selectedPasteIndex == 0 && stack6.Count == 0))
                                {
                                    if (1 == 1 || mystack[selectedStackIndex].back == false)
                                    {
                                        int ss = 0;
                                        Boolean isit = false;
                                        if (stack6.Count > 0)
                                        {
                                            if (mystack[0].suit == Card.SPADE && (stack6[stack6.Count - 1].suit == Card.HEART || stack6[stack6.Count - 1].suit == Card.DIAMOND))
                                                isit = true;
                                            if (mystack[0].suit == Card.CLUB && (stack6[stack6.Count - 1].suit == Card.HEART || stack6[stack6.Count - 1].suit == Card.DIAMOND))
                                                isit = true;
                                            if (mystack[0].suit == Card.HEART && (stack6[stack6.Count - 1].suit == Card.SPADE || stack6[stack6.Count - 1].suit == Card.CLUB))
                                                isit = true;
                                            if (mystack[0].suit == Card.DIAMOND && (stack6[stack6.Count - 1].suit == Card.SPADE || stack6[stack6.Count - 1].suit == Card.CLUB))
                                                isit = true;
                                            if (stack6[stack6.Count - 1].back == true)
                                                isit = true;
                                            else if (mystack[0].rank != stack6[stack6.Count - 1].rank - 1)
                                                isit = false;
                                        }
                                        else if (mystack[0].card.Equals("king") || mystack[0].card.Equals("ace"))
                                            isit = true;
                                        if (isit)
                                        {
                                            while (ss < mystack.Count)
                                            {
                                                stack6.Add(mystack[ss]);
                                                mystack.Remove(mystack[ss]);

                                                stack6[stack6.Count - 1].whatplace = "stak6";
                                            }
                                            if (selectedStack == 1)
                                            {
                                                stack1[stack1.Count - 1].back = false;
                                            }
                                            if (selectedStack == 2)
                                            {
                                                stack2[stack2.Count - 1].back = false;
                                            }
                                            if (selectedStack == 3)
                                            {
                                                stack3[stack3.Count - 1].back = false;
                                            }
                                            if (selectedStack == 4)
                                            {
                                                stack4[stack4.Count - 1].back = false;
                                            }
                                            if (selectedStack == 5)
                                            {
                                                stack5[stack5.Count - 1].back = false;
                                            }
                                            if (selectedStack == 6)
                                            {
                                                stack6[stack6.Count - 1].back = false;
                                            }
                                            if (selectedStack == 7)
                                            {
                                                stack7[stack7.Count - 1].back = false;
                                            }
                                        }
                                    }
                                    if (selectedStack == 2 && stack2[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack2.Count)
                                        {
                                            stack6.Add(stack2[selectedStackIndex]);
                                            stack2.Remove(stack2[selectedStackIndex]);

                                            if (stack2.Count != 0)
                                                stack2[stack2.Count - 1].back = false;
                                            stack6[stack6.Count - 1].whatplace = "stak6";
                                        }
                                    }
                                    if (selectedStack == 3 && stack3[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack3.Count)
                                        {
                                            stack6.Add(stack3[selectedStackIndex]);
                                            stack3.Remove(stack3[selectedStackIndex]);

                                            if (stack3.Count != 0)
                                                stack3[stack3.Count - 1].back = false;
                                            stack6[stack6.Count - 1].whatplace = "stak6";
                                        }
                                    }
                                    if (selectedStack == 4 && stack4[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack4.Count)
                                        {
                                            stack6.Add(stack4[selectedStackIndex]);
                                            stack4.Remove(stack4[selectedStackIndex]);

                                            if (stack4.Count != 0)
                                                stack4[stack4.Count - 1].back = false;
                                            stack6[stack6.Count - 1].whatplace = "stak6";
                                        }
                                    }
                                    if (selectedStack == 5 && stack5[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack5.Count)
                                        {
                                            stack6.Add(stack5[selectedStackIndex]);
                                            stack5.Remove(stack5[selectedStackIndex]);

                                            if (stack5.Count != 0)
                                                stack5[stack5.Count - 1].back = false;
                                            stack6[stack6.Count - 1].whatplace = "stak6";
                                        }
                                    }
                                    if (selectedStack == 7 && stack7[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack7.Count)
                                        {
                                            stack6.Add(stack7[selectedStackIndex]);
                                            stack7.Remove(stack7[selectedStackIndex]);

                                            if (stack7.Count != 0)
                                                stack7[stack7.Count - 1].back = false;
                                            stack6[stack6.Count - 1].whatplace = "stak6";
                                        }
                                    }
                                }
                            }
                            if (selectedPaste == 7)
                            {
                                if (pass || (selectedPasteIndex == 0 && stack7.Count == 0))
                                {
                                    if (1 == 1 || mystack[selectedStackIndex].back == false)
                                    {
                                        int ss = 0;
                                        Boolean isit = false;
                                        if (stack7.Count > 0)
                                        {
                                            if (mystack[0].suit == Card.SPADE && (stack7[stack7.Count - 1].suit == Card.HEART || stack7[stack7.Count - 1].suit == Card.DIAMOND))
                                                isit = true;
                                            if (mystack[0].suit == Card.CLUB && (stack7[stack7.Count - 1].suit == Card.HEART || stack7[stack7.Count - 1].suit == Card.DIAMOND))
                                                isit = true;
                                            if (mystack[0].suit == Card.HEART && (stack7[stack7.Count - 1].suit == Card.SPADE || stack7[stack7.Count - 1].suit == Card.CLUB))
                                                isit = true;
                                            if (mystack[0].suit == Card.DIAMOND && (stack7[stack7.Count - 1].suit == Card.SPADE || stack7[stack7.Count - 1].suit == Card.CLUB))
                                                isit = true;
                                            if (stack7[stack7.Count - 1].back == true)
                                                isit = true;
                                            else if (mystack[0].rank != stack7[stack7.Count - 1].rank - 1)
                                                isit = false;
                                        }
                                        else if (mystack[0].card.Equals("king") || mystack[0].card.Equals("ace"))
                                            isit = true;
                                        if (isit)
                                        {
                                            while (ss < mystack.Count)
                                            {
                                                stack7.Add(mystack[ss]);
                                                mystack.Remove(mystack[ss]);

                                                stack7[stack7.Count - 1].whatplace = "stak7";
                                            }
                                            if (selectedStack == 1)
                                            {
                                                stack1[stack1.Count - 1].back = false;
                                            }
                                            if (selectedStack == 2)
                                            {
                                                stack2[stack2.Count - 1].back = false;
                                            }
                                            if (selectedStack == 3)
                                            {
                                                stack3[stack3.Count - 1].back = false;
                                            }
                                            if (selectedStack == 4)
                                            {
                                                stack4[stack4.Count - 1].back = false;
                                            }
                                            if (selectedStack == 5)
                                            {
                                                stack5[stack5.Count - 1].back = false;
                                            }
                                            if (selectedStack == 6)
                                            {
                                                stack6[stack6.Count - 1].back = false;
                                            }
                                            if (selectedStack == 7)
                                            {
                                                stack7[stack7.Count - 1].back = false;
                                            }
                                        }
                                    }
                                    if (selectedStack == 2 && stack2[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack2.Count)
                                        {
                                            stack7.Add(stack2[selectedStackIndex]);
                                            stack2.Remove(stack2[selectedStackIndex]);

                                            if (stack2.Count != 0)
                                                stack2[stack2.Count - 1].back = false;
                                            stack7[stack7.Count - 1].whatplace = "stak7";
                                        }
                                    }
                                    if (selectedStack == 3 && stack3[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack3.Count)
                                        {
                                            stack7.Add(stack3[selectedStackIndex]);
                                            stack3.Remove(stack3[selectedStackIndex]);

                                            if (stack3.Count != 0)
                                                stack3[stack3.Count - 1].back = false;
                                            stack7[stack7.Count - 1].whatplace = "stak7";
                                        }
                                    }
                                    if (selectedStack == 4 && stack4[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack4.Count)
                                        {
                                            stack7.Add(stack4[selectedStackIndex]);
                                            stack4.Remove(stack4[selectedStackIndex]);

                                            if (stack4.Count != 0)
                                                stack4[stack4.Count - 1].back = false;
                                            stack7[stack7.Count - 1].whatplace = "stak7";
                                        }
                                    }
                                    if (selectedStack == 5 && stack5[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack5.Count)
                                        {
                                            stack7.Add(stack5[selectedStackIndex]);
                                            stack5.Remove(stack5[selectedStackIndex]);

                                            if (stack5.Count != 0)
                                                stack5[stack5.Count - 1].back = false;
                                            stack7[stack7.Count - 1].whatplace = "stak7";
                                        }
                                    }
                                    if (selectedStack == 6 && stack6[selectedStackIndex].back == false)
                                    {
                                        while (selectedStackIndex < stack6.Count)
                                        {
                                            stack7.Add(stack6[selectedStackIndex]);
                                            stack6.Remove(stack6[selectedStackIndex]);

                                            if (stack6.Count != 0)
                                                stack6[stack6.Count - 1].back = false;
                                            stack7[stack7.Count - 1].whatplace = "stak7";
                                        }
                                    }
                                }
                            }
                        }
                        selectedStackCard = false;
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
            
            if (selectedStackCard == false && selectPotCard())
            {
                selectedPotCard = true;
                selectedS = -1;
            }

            if(selectedPotCard && selectedS == -1)
            {
                try
                {
                    if (selectStackPaste())
                    {
                        int compareTo = -1;
                        compareTo = stack[selectedStackIndex].rank + 1;
                        if (selectedPaste == 1)
                        {
                            if (stack1[selectedPasteIndex].rank == compareTo)
                            {
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = "stak1";
                                newCard.rank = stack[selectedStackIndex].rank;
                                newCard.suit = stack[selectedStackIndex].suit;
                                if (stack1.Count == 0)
                                    selectedPasteIndex = 0;
                                else
                                    selectedPasteIndex++;
                                stack1.Insert(selectedPasteIndex, newCard);
                                stack.Remove(stack[selectedStackIndex]);
                                if (stack1.Count != 0)
                                    stack1[stack1.Count - 1].back = false;
                            }
                        }
                        if (selectedPaste == 2)
                        {
                            if (stack2[selectedPasteIndex].rank == compareTo)
                            {
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = "stak2";
                                newCard.rank = stack[selectedStackIndex].rank;
                                newCard.suit = stack[selectedStackIndex].suit;
                                if (stack2.Count == 0)
                                    selectedPasteIndex = 0;
                                else
                                    selectedPasteIndex++;
                                stack2.Insert(selectedPasteIndex, newCard);
                                stack.Remove(stack[selectedStackIndex]);
                                if (stack2.Count != 0)
                                    stack2[stack2.Count - 1].back = false;
                            }
                        }
                        if (selectedPaste == 3)
                        {
                            if (stack3[selectedPasteIndex].rank == compareTo)
                            {
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = "stak3";
                                newCard.rank = stack[selectedStackIndex].rank;
                                newCard.suit = stack[selectedStackIndex].suit;
                                if (stack3.Count == 0)
                                    selectedPasteIndex = 0;
                                else
                                    selectedPasteIndex++;
                                stack3.Insert(selectedPasteIndex, newCard);
                                stack.Remove(stack[selectedStackIndex]);
                                if (stack3.Count != 0)
                                    stack3[stack3.Count - 1].back = false;
                            }
                        }
                        if (selectedPaste == 4)
                        {
                            if (stack4[selectedPasteIndex].rank == compareTo)
                            {
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = "stak4";
                                newCard.rank = stack[selectedStackIndex].rank;
                                newCard.suit = stack[selectedStackIndex].suit;
                                if (stack4.Count == 0)
                                    selectedPasteIndex = 0;
                                else
                                    selectedPasteIndex++;
                                stack4.Insert(selectedPasteIndex, newCard);
                                stack.Remove(stack[selectedStackIndex]);
                                if (stack4.Count != 0)
                                    stack4[stack4.Count - 1].back = false;
                            }
                        }
                        if (selectedPaste == 5)
                        {
                            if (stack5[selectedPasteIndex].rank == compareTo)
                            {
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = "stak5";
                                newCard.rank = stack[selectedStackIndex].rank;
                                newCard.suit = stack[selectedStackIndex].suit;
                                if (stack5.Count == 0)
                                    selectedPasteIndex = 0;
                                else
                                    selectedPasteIndex++;
                                stack5.Insert(selectedPasteIndex, newCard);
                                stack.Remove(stack[selectedStackIndex]);
                                if (stack5.Count != 0)
                                    stack5[stack5.Count - 1].back = false;
                            }
                        }
                        if (selectedPaste == 6)
                        {
                            if (stack6[selectedPasteIndex].rank == compareTo)
                            {
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = "stak6";
                                newCard.rank = stack[selectedStackIndex].rank;
                                newCard.suit = stack[selectedStackIndex].suit;
                                if (stack6.Count == 0)
                                    selectedPasteIndex = 0;
                                else
                                    selectedPasteIndex++;
                                stack6.Insert(selectedPasteIndex, newCard);
                                stack.Remove(stack[selectedStackIndex]);
                                if (stack6.Count != 0)
                                    stack6[stack6.Count - 1].back = false;
                            }
                        }
                        if (selectedPaste == 7)
                        {
                            if (stack7[selectedPasteIndex].rank == compareTo)
                            {
                                Card newCard = new Card();
                                newCard.back = false;
                                newCard.whatplace = "stak7";
                                newCard.rank = stack[selectedStackIndex].rank;
                                newCard.suit = stack[selectedStackIndex].suit;
                                if (stack7.Count == 0)
                                    selectedPasteIndex = 0;
                                else
                                    selectedPasteIndex++;
                                stack7.Insert(selectedPasteIndex, newCard);
                                stack.Remove(stack[selectedStackIndex]);
                                if (stack7.Count != 0)
                                    stack7[stack7.Count - 1].back = false;
                            }
                        }
                        selectedPotCard = false;
                    }
                } catch(Exception e)
                {
                }
            }

            if (selectedStackCard)
            {
                selectedAnywhere = false;
            }

            if(selectedAnywhere)
            {
                selectedStackCard = false;
            }

            if(!selectedStackCard)
            {
                selectedPotCard = selectPotCard();
            }
            else
            {
                selectedPotCard = false;
            }

            if (selectedPotCard || continuePot) {
                continuePot = true;
                s_matCard = false;
                s_potCard = true;
                if (selectStackPaste()) {
                    continuePot = false;
                    selectedPotCard = false;
                    Card org = null, dest = null;
                    try
                    {
                        if (selectedPaste != -1)
                        {
                            if (selectedPasteIndex == -1)
                                selectedPasteIndex = 0;
                            if (selectedStack == 1)
                            {
                                org = stack[selectedStackIndex];
                            }
                            if (selectedStack == 2)
                            {
                                org = stack[selectedStackIndex];
                            }
                            if (selectedStack == 3)
                            {
                                org = stack[selectedStackIndex];
                            }
                            if (selectedStack == 4)
                            {
                                org = stack[selectedStackIndex];
                            }
                            if (selectedPaste == 1)
                            {
                                if (stack1.Count == 1)
                                {
                                    dest = stack1[0];
                                }
                                else
                                {
                                    if (selectedPasteIndex < stack1.Count)
                                        dest = stack1[selectedPasteIndex];
                                    else if (stack1.Count == 0)
                                        dest = null;
                                }
                            }
                            if (selectedPaste == 2)
                            {
                                if (stack2.Count == 1)
                                {
                                    dest = stack2[0];
                                }
                                else
                                {
                                    if (selectedPasteIndex < stack2.Count)
                                        dest = stack2[selectedPasteIndex];
                                    else if (stack2.Count == 0)
                                        dest = null;
                                }
                            }
                            if (selectedPaste == 3)
                            {
                                if (stack3.Count == 1)
                                {
                                    dest = stack3[0];
                                }
                                else
                                {
                                    if (selectedPasteIndex < stack3.Count)
                                        dest = stack3[selectedPasteIndex];
                                    else if (stack3.Count == 0)
                                        dest = null;
                                }
                            }
                            if (selectedPaste == 4)
                            {
                                if (stack4.Count == 1)
                                {
                                    dest = stack4[0];
                                }
                                else
                                {
                                    if (selectedPasteIndex < stack4.Count)
                                        dest = stack4[selectedPasteIndex];
                                    else if (stack4.Count == 0)
                                        dest = null;
                                }
                            }
                            if (selectedPaste == 5)
                            {
                                if (stack5.Count == 1)
                                {
                                    dest = stack5[0];
                                }
                                else
                                {
                                    if (selectedPasteIndex < stack5.Count)
                                        dest = stack5[selectedPasteIndex];
                                    else if (stack5.Count == 0)
                                        dest = null;
                                }
                            }
                            if (selectedPaste == 6)
                            {
                                if (stack6.Count == 1)
                                {
                                    dest = stack6[0];
                                }
                                else
                                {
                                    if (selectedPasteIndex < stack6.Count)
                                        dest = stack6[selectedPasteIndex];
                                    else if (stack6.Count == 0)
                                        dest = null;
                                }
                            }
                            if (selectedPaste == 7)
                            {
                                if (stack7.Count == 1)
                                {
                                    dest = stack7[0];
                                }
                                else
                                {
                                    if (selectedPasteIndex < stack7.Count)
                                        dest = stack7[selectedPasteIndex];
                                    else if (stack7.Count == 0)
                                        dest = null;
                                }
                            }
                            bool pass = false;
                            if (dest != null)
                            {
                                if (dest.rank - 1 == org.rank)
                                    pass = true;
                            }
                            else
                            {
                                if (org.rank == 13)
                                    pass = true;
                            }
                            if (selectedPaste == 1)
                            {
                                if (pass || (selectedPasteIndex == 0 && stack1.Count == 0))
                                {
                                    stack1.Add(stack[selectedStackIndex]);
                                    stack.Remove(stack[selectedStackIndex]);
                                    stack1[stack1.Count - 1].whatplace = "stak1";
                                }
                            }
                            if (selectedPaste == 2)
                            {
                                if (pass || (selectedPasteIndex == 0 && stack2.Count == 0))
                                {
                                    stack2.Add(stack[selectedStackIndex]);
                                    stack.Remove(stack[selectedStackIndex]);
                                    stack2[stack2.Count - 1].whatplace = "stak2";
                                }
                            }
                            if (selectedPaste == 3)
                            {
                                if (pass || (selectedPasteIndex == 0 && stack3.Count == 0))
                                {
                                    stack3.Add(stack[selectedStackIndex]);
                                    stack.Remove(stack[selectedStackIndex]);
                                    stack3[stack3.Count - 1].whatplace = "stak3";
                                }
                            }
                            if (selectedPaste == 4)
                            {
                                if (pass || (selectedPasteIndex == 0 && stack4.Count == 0))
                                {
                                    stack4.Add(stack[selectedStackIndex]);
                                    stack.Remove(stack[selectedStackIndex]);
                                    stack4[stack4.Count - 1].whatplace = "stak4";
                                }
                            }
                            if (selectedPaste == 5)
                            {
                                if (pass || (selectedPasteIndex == 0 && stack5.Count == 0))
                                {
                                    stack5.Add(stack[selectedStackIndex]);
                                    stack.Remove(stack[selectedStackIndex]);
                                    stack5[stack5.Count - 1].whatplace = "stak5";
                                }
                            }
                            if (selectedPaste == 6)
                            {
                                if (pass || (selectedPasteIndex == 0 && stack6.Count == 0))
                                {
                                    stack6.Add(stack[selectedStackIndex]);
                                    stack.Remove(stack[selectedStackIndex]);
                                    stack6[stack6.Count - 1].whatplace = "stak6";
                                }
                            }
                            if (selectedPaste == 7)
                            {
                                if (pass || (selectedPasteIndex == 0 && stack7.Count == 0))
                                {
                                    stack7.Add(stack[selectedStackIndex]);
                                    stack.Remove(stack[selectedStackIndex]);
                                    stack7[stack7.Count - 1].whatplace = "stak1";
                                }
                            }
                        }
                    } catch(Exception e) {
                    }
                }
            }

            base.Update(gameTime);
        }

        public bool selectStackCard()
        {
            if (_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && _currentMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                if (mystack == null)
                    mystack = new List<Card>();

                if (cards == null)
                    return false;

                var mousePosition = new Point(_currentMouseState.X, _currentMouseState.Y);

                for (int i = 0; i < stack1.Count; i++)
                {
                    Rectangle someRectangle = stack1[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        if (stack1[i].back == false)
                        {
                            selectedStack = 1;

                            selectedStackIndex = i;

                            mystack.Add(stack1[i]);

                            stack1.Remove(stack1[i]);
                        }
                        else
                            return false;

                        return true;
                    }
                }
                for (int i = 0; i < stack2.Count; i++)
                {
                    Rectangle someRectangle = stack2[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        if (stack2[i].back == false)
                        {
                            selectedStack = 2;

                            selectedStackIndex = i;

                            mystack.Add(stack2[i]);

                            stack2.Remove(stack2[i]);
                        }
                        else
                            return false;

                        return true;
                    }
                }
                for (int i = 0; i < stack3.Count; i++)
                {
                    Rectangle someRectangle = stack3[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        if (stack3[i].back == false)
                        {
                            selectedStack = 3;

                            selectedStackIndex = i;

                            mystack.Add(stack3[i]);

                            stack3.Remove(stack3[i]);
                        }
                        else
                            return false;

                        return true;
                    }
                }
                for (int i = 0; i < stack4.Count; i++)
                {
                    Rectangle someRectangle = stack4[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        if (stack4[i].back == false)
                        {
                            selectedStack = 4;

                            selectedStackIndex = i;

                            mystack.Add(stack4[i]);

                            stack4.Remove(stack4[i]);
                        }
                        else
                            return false;

                        return true;
                    }
                }
                for (int i = 0; i < stack5.Count; i++)
                {
                    Rectangle someRectangle = stack5[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        if (stack5[i].back == false)
                        {
                            selectedStack = 5;

                            selectedStackIndex = i;

                            mystack.Add(stack5[i]);

                            stack5.Remove(stack5[i]);
                        }
                        else
                            return false;

                        return true;
                    }
                }
                for (int i = 0; i < stack6.Count; i++)
                {
                    Rectangle someRectangle = stack6[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        if (stack6[i].back == false)
                        {
                            selectedStack = 6;

                            selectedStackIndex = i;

                            mystack.Add(stack6[i]);

                            stack6.Remove(stack6[i]);
                        }
                        else
                            return false;

                        return true;
                    }
                }
                for (int i = 0; i < stack7.Count; i++)
                {
                    Rectangle someRectangle = stack7[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        if (stack7[i].back == false)
                        {
                            selectedStack = 7;

                            selectedStackIndex = i;

                            mystack.Add(stack7[i]);

                            stack7.Remove(stack7[i]);
                        }
                        else
                            return false;

                        return true;
                    }
                }
            }

            return false;
        }

        public bool selectPotCard()
        {
            if (_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && _currentMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                if (cards == null)
                    return false;

                countStack1 = 0; countStack2 = 0; countStack3 = 0; countStack4 = 0;

                selectedPIndex = selectedStackIndex;

                selectedS = selectedStack;

                var mousePosition = new Point(_currentMouseState.X, _currentMouseState.Y);

                for (int i = 0; i < stack.Count; i++)
                {
                    if (stack[i].whatplace.Equals("stack1"))
                    {
                        countStack1++;
                        insertStack1 = i;
                    }
                }

                for (int i = 0; i < stack.Count; i++)
                {
                    if (stack[i].whatplace.Equals("stack2"))
                    {
                        countStack2++;
                        insertStack2 = i;
                    }
                }

                for (int i = 0; i < stack.Count; i++)
                {
                    if (stack[i].whatplace.Equals("stack3"))
                    {
                        countStack3++;
                        insertStack3 = i;
                    }
                }

                for (int i = 0; i < stack.Count; i++)
                {
                    if (stack[i].whatplace.Equals("stack4"))
                    {
                        countStack4++;
                        insertStack4 = i;
                    }
                }

                Rectangle somRectangle = new Rectangle(300, 20, 80, 160);

                Rectangle are = somRectangle;

                if (are.Contains(mousePosition) && countStack1 != 0)
                {
                    selectedStack = 1;

                    selectedStackIndex = insertStack1;

                    if (selectedStackIndex < 0)
                        selectedStackIndex = 0;

                    return true;
                }

                somRectangle = new Rectangle(400, 20, 80, 160);

                are = somRectangle;

                if (are.Contains(mousePosition) && countStack2 != 0)
                {
                    selectedStack = 2;

                    selectedStackIndex = insertStack2;

                    if (selectedStackIndex < 0)
                        selectedStackIndex = 0;

                    return true;
                }

                somRectangle = new Rectangle(500, 20, 80, 160);

                are = somRectangle;

                if (are.Contains(mousePosition) && countStack3 != 0)
                {
                    selectedStack = 3;

                    selectedStackIndex = insertStack3;

                    if (selectedStackIndex < 0)
                        selectedStackIndex = 0;

                    return true;
                }

                somRectangle = new Rectangle(600, 20, 80, 160);

                are = somRectangle;

                if (are.Contains(mousePosition) && countStack4 != 0)
                {
                    selectedStack = 4;

                    selectedStackIndex = insertStack4;

                    if (selectedStackIndex < 0)
                        selectedStackIndex = 0;

                    return true;
                }
            }

            return false;
        }

        public bool selectStackPaste()
        {
            if (_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && _currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                if (cards == null)
                    return false;

                var mousePosition = new Point(_currentMouseState.X, _currentMouseState.Y);

                Rectangle somRectangle = new Rectangle(200, 220, 80, 30);

                Rectangle are = somRectangle;

                if (are.Contains(mousePosition))
                {
                    selectedPaste = 1;

                    selectedPasteIndex = -1;

                    selectedPast = selectedPaste;
                }

                somRectangle = new Rectangle(300, 220, 80, 30);

                are = somRectangle;

                if (are.Contains(mousePosition))
                {
                    selectedPaste = 2;

                    selectedPasteIndex = -1;

                    selectedPast = selectedPaste;
                }

                somRectangle = new Rectangle(400, 220, 80, 30);

                are = somRectangle;

                if (are.Contains(mousePosition))
                {
                    selectedPaste = 3;

                    selectedPasteIndex = -1;

                    selectedPast = selectedPaste;
                }

                somRectangle = new Rectangle(500, 220, 80, 30);

                are = somRectangle;

                if (are.Contains(mousePosition))
                {
                    selectedPaste = 4;

                    selectedPasteIndex = -1;

                    selectedPast = selectedPaste;
                }
                
                somRectangle = new Rectangle(600, 220, 80, 30);

                are = somRectangle;

                if (are.Contains(mousePosition))
                {
                    selectedPaste = 5;

                    selectedPasteIndex = -1;

                    selectedPast = selectedPaste;
                }

                somRectangle = new Rectangle(700, 220, 80, 30);

                are = somRectangle;

                if (are.Contains(mousePosition))
                {
                    selectedPaste = 6;

                    selectedPasteIndex = -1;

                    selectedPast = selectedPaste;
                }

                somRectangle = new Rectangle(800, 220, 80, 30);

                are = somRectangle;

                if (are.Contains(mousePosition))
                {
                    selectedPaste = 7;

                    selectedPasteIndex = -1;

                    selectedPast = selectedPaste;
                }

                for (int i = 0; i < stack1.Count; i++)
                {
                    Rectangle someRectangle = stack1[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        selectedPaste = 1;

                        selectedPasteIndex = i;

                        return true;
                    }
                    else
                        selectedPaste = -1;
                }
                for (int i = 0; i < stack2.Count; i++)
                {
                    Rectangle someRectangle = stack2[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        selectedPaste = 2;

                        selectedPasteIndex = i;

                        return true;
                    }
                    else
                        selectedPaste = -1;
                }
                for (int i = 0; i < stack3.Count; i++)
                {
                    Rectangle someRectangle = stack3[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        selectedPaste = 3;

                        selectedPasteIndex = i;

                        return true;
                    }
                    else
                        selectedPaste = -1;
                }
                for (int i = 0; i < stack4.Count; i++)
                {
                    Rectangle someRectangle = stack4[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        selectedPaste = 4;

                        selectedPasteIndex = i;

                        return true;
                    }
                    else
                        selectedPaste = -1;
                }
                for (int i = 0; i < stack5.Count; i++)
                {
                    Rectangle someRectangle = stack5[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        selectedPaste = 5;

                        selectedPasteIndex = i;

                        return true;
                    }
                    else
                        selectedPaste = -1;
                }
                for (int i = 0; i < stack6.Count; i++)
                {
                    Rectangle someRectangle = stack6[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        selectedPaste = 6;

                        selectedPasteIndex = i;

                        return true;
                    }
                    else
                        selectedPaste = -1;
                }
                for (int i = 0; i < stack7.Count; i++)
                {
                    Rectangle someRectangle = stack7[i].place;
                    someRectangle.Width = 80;
                    someRectangle.Height = 30;

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        selectedPaste = 7;

                        selectedPasteIndex = i;

                        return true;
                    }
                    else
                        selectedPaste = -1;
                }

                selectedPIndex = selectedPasteIndex;

                if (selectedPaste != -1)
                    return true;

                if (selectedPaste == -1)
                    selectedPaste = selectedPast;

                if (selectedPaste != -1)
                    return true;
            }

            return false;
        }

        public bool selectMatCard()
        {
            if (_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && _currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                var mousePosition = new Point(_currentMouseState.X, _currentMouseState.Y);

                Rectangle someRectangle = new Rectangle(160, 20, 80, 160);

                Rectangle area = someRectangle;

                if (area.Contains(mousePosition))
                {
                    return true;
                }
            }

            return false;
        }

        public bool selectAnywhere()
        {
            if (_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && _currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                if (cards == null)
                    return false;

                /*
                selectedMatCard = false;
                selectedStackCard = false;
                selectedPotCard = false;
                */

                s_matCard = false;
                s_potCard = false;

                var mousePosition = new Point(_currentMouseState.X, _currentMouseState.Y);

                Rectangle someRectangle = new Rectangle(0, 0, 700, 900);

                Rectangle area = someRectangle;

                if (area.Contains(mousePosition))
                {
                    return true;
                }
            }

            return false;
        }

        public bool stackClick()
        {
            if (_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && 
                _currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                if (cards == null)
                    return false;

                var mousePosition = new Point(_currentMouseState.X, _currentMouseState.Y);

                Rectangle someRectangle = new Rectangle(300, 20, 80, 160);

                Rectangle area = someRectangle;

                if (area.Contains(mousePosition))
                {
                    whichstack = "stack1";
                    return true;
                }

                someRectangle = new Rectangle(400, 20, 80, 160);

                area = someRectangle;

                if (area.Contains(mousePosition))
                {
                    whichstack = "stack2";
                    return true;
                }

                someRectangle = new Rectangle(500, 20, 80, 160);

                area = someRectangle;

                if (area.Contains(mousePosition))
                {
                    whichstack = "stack3";
                    return true;
                }

                someRectangle = new Rectangle(600, 20, 80, 160);

                area = someRectangle;

                if (area.Contains(mousePosition))
                {
                    whichstack = "stack4";
                    return true;
                }
            }

            return false;
        }

        public bool nexts()
        {
            if(!gameOver)
                if (_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && _currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    var mousePosition = new Point(_currentMouseState.X, _currentMouseState.Y);

                    Rectangle someRectangle = new Rectangle(80, 20, 80, 160);

                    Rectangle area = someRectangle;

                    if (area.Contains(mousePosition))
                    {
                        beginPlayClicked = true;
                    }
                }

            if (beginPlayClicked)
                return true;
            else
                return false;
        }

        public bool beginPlay()
        {
            if (_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && _currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                var mousePosition = new Point(_currentMouseState.X, _currentMouseState.Y);

                Rectangle someRectangle = new Rectangle(20, 200, 80, 80);

                Rectangle area = someRectangle;

                if (area.Contains(mousePosition))
                {
                    beginPlayClicked = true;
                }
            }

            if (beginPlayClicked)
            {
                gameOver = false;

                cards = new List<Card>();

                for (int suit = Card.DIAMOND; suit <= Card.SPADE; suit++)
                {
                    for (int rank = 1; rank <= 13; rank++)
                    {
                        Card card = new Card();
                        cards.Add(card);

                        card.rank = rank;
                        card.suit = suit;
                        card.place = new Rectangle(160, 20, 80, 160);

                        if (rank == 1)
                        {
                            card.card = "ace";
                        }
                        else if (rank == 2)
                        {
                            card.card = "two";
                        }
                        else if (rank == 3)
                        {
                            card.card = "three";
                        }
                        else if (rank == 4)
                        {
                            card.card = "four";
                        }
                        else if (rank == 5)
                        {
                            card.card = "five";
                        }
                        else if (rank == 6)
                        {
                            card.card = "six";
                        }
                        else if (rank == 7)
                        {
                            card.card = "seven";
                        }
                        else if (rank == 8)
                        {
                            card.card = "eight";
                        }
                        else if (rank == 9)
                        {
                            card.card = "nine";
                        }
                        else if (rank == 10)
                        {
                            card.card = "ten";
                        }
                        else if (rank == 11)
                        {
                            card.card = "jack";
                        }
                        else if (rank == 12)
                        {
                            card.card = "queen";
                        }
                        else if (rank == 13)
                        {
                            card.card = "king";
                        }
                    }
                }

                cards = FisherYates.Shuffle(cards);

                cardsMat = new List<Card>();
                mystack = new List<Card>();
                stack1 = new List<Card>();
                stack2 = new List<Card>();
                stack3 = new List<Card>();
                stack4 = new List<Card>();
                stack5 = new List<Card>();
                stack6 = new List<Card>();
                stack7 = new List<Card>();
                stack = new List<Card>();

                return true;

            } else {
            
                return false;
            }
        }

        public void dealCards()
        {
            cards[0].whatplace = "mat";
            cards[0].place = new Rectangle(160, 20, 80, 160);
            cardsMat.Add(cards[0]);
            cards.Remove(cards[0]);

            int i = 0;
            cards[i].whatplace = "stak1";
            stack1.Add(cards[i]);
            cards.Remove(cards[i]);

            cards[i].back = true;
            stack2.Add(cards[i]);
            stack2[stack2.Count - 1].whatplace = "stak2";
            cards.Remove(cards[i]);
            stack2.Add(cards[i]);
            stack2[stack2.Count - 1].whatplace = "stak2";
            cards.Remove(cards[i]);

            cards[i].back = true;
            stack3.Add(cards[i]);
            stack3[stack3.Count - 1].whatplace = "stak3";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack3.Add(cards[i]);
            stack3[stack3.Count - 1].whatplace = "stak3";
            cards.Remove(cards[i]);
            stack3.Add(cards[i]);
            stack3[stack3.Count - 1].whatplace = "stak3";
            cards.Remove(cards[i]);

            cards[i].back = true;
            stack4.Add(cards[i]);
            stack4[stack4.Count - 1].whatplace = "stak4";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack4.Add(cards[i]);
            stack4[stack4.Count - 1].whatplace = "stak4";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack4.Add(cards[i]);
            stack4[stack4.Count - 1].whatplace = "stak4";
            cards.Remove(cards[i]);
            stack4.Add(cards[i]);
            stack4[stack4.Count - 1].whatplace = "stak4";
            cards.Remove(cards[i]);

            cards[i].back = true;
            stack5.Add(cards[i]);
            stack5[stack5.Count - 1].whatplace = "stak5";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack5.Add(cards[i]);
            stack5[stack5.Count - 1].whatplace = "stak5";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack5.Add(cards[i]);
            stack5[stack5.Count - 1].whatplace = "stak5";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack5.Add(cards[i]);
            stack5[stack5.Count - 1].whatplace = "stak5";
            cards.Remove(cards[i]);
            stack5.Add(cards[i]);
            stack5[stack5.Count - 1].whatplace = "stak5";
            cards.Remove(cards[i]);

            cards[i].back = true;
            stack6.Add(cards[i]);
            stack6[stack6.Count - 1].whatplace = "stak6";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack6.Add(cards[i]);
            stack6[stack6.Count - 1].whatplace = "stak6";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack6.Add(cards[i]);
            stack6[stack6.Count - 1].whatplace = "stak6";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack6.Add(cards[i]);
            stack6[stack6.Count - 1].whatplace = "stak6";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack6.Add(cards[i]);
            stack6[stack6.Count - 1].whatplace = "stak6";
            cards.Remove(cards[i]);
            stack6.Add(cards[i]);
            stack6[stack6.Count - 1].whatplace = "stak6";
            cards.Remove(cards[i]);

            cards[i].back = true;
            stack7.Add(cards[i]);
            stack7[stack7.Count - 1].whatplace = "stak7";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack7.Add(cards[i]);
            stack7[stack7.Count - 1].whatplace = "stak7";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack7.Add(cards[i]);
            stack7[stack7.Count - 1].whatplace = "stak7";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack7.Add(cards[i]);
            stack7[stack7.Count - 1].whatplace = "stak7";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack7.Add(cards[i]);
            stack7[stack7.Count - 1].whatplace = "stak7";
            cards.Remove(cards[i]);
            cards[i].back = true;
            stack7.Add(cards[i]);
            stack7[stack7.Count - 1].whatplace = "stak7";
            cards.Remove(cards[i]);
            stack7.Add(cards[i]);
            stack7[stack7.Count - 1].whatplace = "stak7";
            cards.Remove(cards[i]);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(80, 20), new Vector2(160, 20), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(80, 180), new Vector2(160, 180), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(80, 20), new Vector2(80, 180), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(160, 20), new Vector2(160, 180), Color.White, 1);

            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(160, 20), new Vector2(240, 20), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(160, 180), new Vector2(240, 180), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(160, 20), new Vector2(160, 180), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(240, 20), new Vector2(240, 180), Color.White, 1);


            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(300, 20), new Vector2(380, 20), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(300, 180), new Vector2(380, 180), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(300, 20), new Vector2(300, 180), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(380, 20), new Vector2(380, 180), Color.White, 1);

            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(400, 20), new Vector2(480, 20), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(400, 180), new Vector2(480, 180), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(400, 20), new Vector2(400, 180), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(480, 20), new Vector2(480, 180), Color.White, 1);

            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(500, 20), new Vector2(580, 20), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(500, 180), new Vector2(580, 180), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(500, 20), new Vector2(500, 180), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(580, 20), new Vector2(580, 180), Color.White, 1);

            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(600, 20), new Vector2(680, 20), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(600, 180), new Vector2(680, 180), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(600, 20), new Vector2(600, 180), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(680, 20), new Vector2(680, 180), Color.White, 1);


            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(200, 220), new Vector2(280, 220), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(200, 380), new Vector2(280, 380), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(200, 220), new Vector2(200, 380), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(280, 220), new Vector2(280, 380), Color.White, 1);

            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(300, 220), new Vector2(380, 220), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(300, 380), new Vector2(380, 380), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(300, 220), new Vector2(300, 380), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(380, 220), new Vector2(380, 380), Color.White, 1);

            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(400, 220), new Vector2(480, 220), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(400, 380), new Vector2(480, 380), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(400, 220), new Vector2(400, 380), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(480, 220), new Vector2(480, 380), Color.White, 1);

            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(500, 220), new Vector2(580, 220), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(500, 380), new Vector2(580, 380), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(500, 220), new Vector2(500, 380), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(580, 220), new Vector2(580, 380), Color.White, 1);

            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(600, 220), new Vector2(680, 220), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(600, 380), new Vector2(680, 380), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(600, 220), new Vector2(600, 380), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(680, 220), new Vector2(680, 380), Color.White, 1);

            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(700, 220), new Vector2(780, 220), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(700, 380), new Vector2(780, 380), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(700, 220), new Vector2(700, 380), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(780, 220), new Vector2(780, 380), Color.White, 1);

            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(800, 220), new Vector2(880, 220), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(800, 380), new Vector2(880, 380), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(800, 220), new Vector2(800, 380), Color.White, 1);
            SpriteBatchEx.DrawLine(spriteBatch, new Vector2(880, 220), new Vector2(880, 380), Color.White, 1);

            spriteBatch.DrawString(spriteFont, "Rules?", new Vector2(20, 400), Color.White);

            spriteBatch.Draw(beginPlayT, new Rectangle(20, 200, 80, 80), Color.White);

            if (cards != null)
            {
                for (int i = 0; i < cards.Count; ++i)
                    spriteBatch.Draw(back, new Rectangle(80, 20, 80, 160), Color.White);
                for (int i = 0; !gameOver && i <= cardsMat.Count - 1 && cardsMat.Count > 0; i++)
                {
                    try
                    {
                        card = Content.Load<Texture2D>(cardsMat[i].suit + "-" + cardsMat[i].rank);
                        spriteBatch.Draw(card, cardsMat[i].place, Color.White);
                    }
                    catch (Exception e)
                    {
                    }
                }
                int count1 = 0, count2 = 0, count3 = 0, count4 = 0, count5 = 0, count6 = 0, count7 = 0;
                for (int i = 0; i < stack1.Count && stack1.Count > 0; i++)
                {
                    if (stack1[i].whatplace.Equals("stak1"))
                    {
                        stack1[i].place = new Rectangle(200, 220 + count1++ * 25, 80, 160);
                        if (stack1[i].back)
                        {
                            spriteBatch.Draw(back, stack1[i].place, Color.White);
                        }
                        else
                        {
                            card = Content.Load<Texture2D>(stack1[i].suit + "-" + stack1[i].rank);
                            spriteBatch.Draw(card, stack1[i].place, Color.White);
                        }
                    }
                }
                for (int i = 0; i < stack2.Count && stack2.Count > 0; i++)
                {
                    if (stack2[i].whatplace.Equals("stak2"))
                    {
                        stack2[i].place = new Rectangle(300, 220 + count2++ * 25, 80, 160);
                        if (stack2[i].back)
                        {
                            spriteBatch.Draw(back, stack2[i].place, Color.White);
                        }
                        else
                        {
                            card = Content.Load<Texture2D>(stack2[i].suit + "-" + stack2[i].rank);
                            spriteBatch.Draw(card, stack2[i].place, Color.White);
                        }
                    }
                }
                for (int i = 0; i < stack3.Count && stack3.Count > 0; i++)
                {
                    if (stack3[i].whatplace.Equals("stak3"))
                    {
                        stack3[i].place = new Rectangle(400, 220 + count3++ * 25, 80, 160);
                        if (stack3[i].back)
                        {
                            spriteBatch.Draw(back, stack3[i].place, Color.White);
                        }
                        else
                        {
                            card = Content.Load<Texture2D>(stack3[i].suit + "-" + stack3[i].rank);
                            spriteBatch.Draw(card, stack3[i].place, Color.White);
                        }
                    }
                }
                for (int i = 0; i < stack4.Count && stack4.Count > 0; i++)
                {
                    if (stack4[i].whatplace.Equals("stak4"))
                    {
                        stack4[i].place = new Rectangle(500, 220 + count4++ * 25, 80, 160);
                        if (stack4[i].back)
                        {
                            spriteBatch.Draw(back, stack4[i].place, Color.White);
                        }
                        else
                        {
                            card = Content.Load<Texture2D>(stack4[i].suit + "-" + stack4[i].rank);
                            spriteBatch.Draw(card, stack4[i].place, Color.White);
                        }
                    }
                }
                for (int i = 0; i < stack5.Count && stack5.Count > 0; i++)
                {
                    if (stack5[i].whatplace.Equals("stak5"))
                    {
                        stack5[i].place = new Rectangle(600, 220 + count5++ * 25, 80, 160);
                        if (stack5[i].back)
                        {
                            spriteBatch.Draw(back, stack5[i].place, Color.White);
                        }
                        else
                        {
                            card = Content.Load<Texture2D>(stack5[i].suit + "-" + stack5[i].rank);
                            spriteBatch.Draw(card, stack5[i].place, Color.White);
                        }
                    }
                }
                for (int i = 0; i < stack6.Count && stack6.Count > 0; i++)
                {
                    if (stack6[i].whatplace.Equals("stak6"))
                    {
                        stack6[i].place = new Rectangle(700, 220 + count6++ * 25, 80, 160);
                        if (stack6[i].back)
                        {
                            spriteBatch.Draw(back, stack6[i].place, Color.White);
                        }
                        else
                        {
                            card = Content.Load<Texture2D>(stack6[i].suit + "-" + stack6[i].rank);
                            spriteBatch.Draw(card, stack6[i].place, Color.White);
                        }
                    }
                }
                for (int i = 0; i < stack7.Count && stack7.Count > 0; i++)
                {
                    if (stack7[i].whatplace.Equals("stak7"))
                    {
                        stack7[i].place = new Rectangle(800, 220 + count7++ * 25, 80, 160);
                        if (stack7[i].back)
                        {
                            spriteBatch.Draw(back, stack7[i].place, Color.White);
                        }
                        else
                        {
                            card = Content.Load<Texture2D>(stack7[i].suit + "-" + stack7[i].rank);
                            spriteBatch.Draw(card, stack7[i].place, Color.White);
                        }
                    }
                }
                for (int i = 0; i < stack.Count && stack.Count > 0; i++)
                {
                    if (stack[i].whatplace.Equals("stack1"))
                    {
                        stack[i].place = new Rectangle(300, 20, 80, 160);
                        card = Content.Load<Texture2D>(stack[i].suit + "-" + stack[i].rank);
                        spriteBatch.Draw(card, stack[i].place, Color.White);
                    }
                }
                for (int i = 0; i < stack.Count && stack.Count > 0; i++)
                {
                    if (stack[i].whatplace.Equals("stack2"))
                    {
                        stack[i].place = new Rectangle(400, 20, 80, 160);
                        card = Content.Load<Texture2D>(stack[i].suit + "-" + stack[i].rank);
                        spriteBatch.Draw(card, stack[i].place, Color.White);
                    }
                }
                for (int i = 0; i < stack.Count && stack.Count > 0; i++)
                {
                    if (stack[i].whatplace.Equals("stack3"))
                    {
                        stack[i].place = new Rectangle(500, 20, 80, 160);
                        card = Content.Load<Texture2D>(stack[i].suit + "-" + stack[i].rank);
                        spriteBatch.Draw(card, stack[i].place, Color.White);
                    }
                }
                for (int i = 0; i < stack.Count && stack.Count > 0; i++)
                {
                    if (stack[i].whatplace.Equals("stack4"))
                    {
                        stack[i].place = new Rectangle(600, 20, 80, 160);
                        card = Content.Load<Texture2D>(stack[i].suit + "-" + stack[i].rank);
                        spriteBatch.Draw(card, stack[i].place, Color.White);
                    }
                }
            }
            if (stack != null)
            {
                if (stack.Count == 52)
                {
                    spriteBatch.DrawString(spriteFont, "You won!", new Vector2(720, 60), Color.White);
                }
            }
            if (_previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released &&
                _currentMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                var mousePosition = new Point(_currentMouseState.X, _currentMouseState.Y);

                Rectangle someRectangle = new Rectangle(300, 20, 80, 160);

                Rectangle area = someRectangle;

                if (area.Contains(mousePosition))
                {
                    whichstack = "stack1";
                }

                someRectangle = new Rectangle(400, 20, 80, 160);

                area = someRectangle;

                if (area.Contains(mousePosition))
                {
                    whichstack = "stack2";
                }

                someRectangle = new Rectangle(500, 20, 80, 160);

                area = someRectangle;

                if (area.Contains(mousePosition))
                {
                    whichstack = "stack3";
                }

                someRectangle = new Rectangle(600, 20, 80, 160);

                area = someRectangle;

                if (area.Contains(mousePosition))
                {
                    whichstack = "stack4";
                }
            }
            if (s_potCard)
            {
                if(whichstack.Equals("stack1"))
                for (int i = 0; i < stack.Count && stack.Count > 0; i++)
                {
                    if (stack[i].whatplace.Equals("stack1"))
                    {
                        stack[i].place = new Rectangle(300, 20, 80, 160);
                        card = Content.Load<Texture2D>(stack[i].suit + "-" + stack[i].rank);
                        spriteBatch.Draw(card, new Rectangle(currentMousePosition.X, currentMousePosition.Y, 80, 160), Color.White);
                    }
                }
                if (whichstack.Equals("stack2"))
                for (int i = 0; i < stack.Count && stack.Count > 0; i++)
                {
                    if (stack[i].whatplace.Equals("stack2"))
                    {
                        stack[i].place = new Rectangle(400, 20, 80, 160);
                        card = Content.Load<Texture2D>(stack[i].suit + "-" + stack[i].rank);
                        spriteBatch.Draw(card, new Rectangle(currentMousePosition.X, currentMousePosition.Y, 80, 160), Color.White);
                    }
                }
                if (whichstack.Equals("stack3"))
                for (int i = 0; i < stack.Count && stack.Count > 0; i++)
                {
                    if (stack[i].whatplace.Equals("stack3"))
                    {
                        stack[i].place = new Rectangle(500, 20, 80, 160);
                        card = Content.Load<Texture2D>(stack[i].suit + "-" + stack[i].rank);
                        spriteBatch.Draw(card, new Rectangle(currentMousePosition.X, currentMousePosition.Y, 80, 160), Color.White);
                    }
                }
                if (whichstack.Equals("stack4"))
                for (int i = 0; i < stack.Count && stack.Count > 0; i++)
                {
                    if (stack[i].whatplace.Equals("stack4"))
                    {
                        stack[i].place = new Rectangle(600, 20, 80, 160);
                        card = Content.Load<Texture2D>(stack[i].suit + "-" + stack[i].rank);
                        spriteBatch.Draw(card, new Rectangle(currentMousePosition.X, currentMousePosition.Y, 80, 160), Color.White);
                    }
                }
            }
            try
            {
                if(selectedStack != -1 && selectedStackCard)
                {
                    int yy = 0;
                    if(selectedStack == 1)
                    for (int i = selectedStackIndex; i < stack1.Count && stack1.Count > 0; i++)
                    {
                        if (stack1[i].whatplace.Equals("stak1"))
                        {
                            card = Content.Load<Texture2D>(stack1[i].suit + "-" + stack1[i].rank);
                            spriteBatch.Draw(card, new Rectangle(currentMousePosition.X, currentMousePosition.Y + yy, 80, 160), Color.White);
                            yy += 25;
                        }
                    }
                    yy = 0;
                    if (selectedStack == 2)
                    for (int i = selectedStackIndex; i < stack2.Count && stack2.Count > 0; i++)
                    {
                        if (stack2[i].whatplace.Equals("stak2"))
                        {
                            card = Content.Load<Texture2D>(stack2[i].suit + "-" + stack2[i].rank);
                            spriteBatch.Draw(card, new Rectangle(currentMousePosition.X, currentMousePosition.Y + yy, 80, 160), Color.White);
                            yy += 25;
                        }
                    }
                    yy = 0;
                    if (selectedStack == 3)
                    for (int i = selectedStackIndex; i < stack3.Count && stack3.Count > 0; i++)
                    {
                        if (stack3[i].whatplace.Equals("stak3"))
                        {
                            card = Content.Load<Texture2D>(stack3[i].suit + "-" + stack3[i].rank);
                            spriteBatch.Draw(card, new Rectangle(currentMousePosition.X, currentMousePosition.Y + yy, 80, 160), Color.White);
                            yy += 25;
                        }
                    }
                    yy = 0;
                    if (selectedStack == 4)
                    for (int i = selectedStackIndex; i < stack4.Count && stack4.Count > 0; i++)
                    {
                        if (stack4[i].whatplace.Equals("stak4"))
                        {
                            card = Content.Load<Texture2D>(stack4[i].suit + "-" + stack4[i].rank);
                            spriteBatch.Draw(card, new Rectangle(currentMousePosition.X, currentMousePosition.Y + yy, 80, 160), Color.White);
                            yy += 25;
                        }
                    }
                    yy = 0;
                    if (selectedStack == 5)
                    for (int i = selectedStackIndex; i < stack5.Count && stack5.Count > 0; i++)
                    {
                        if (stack5[i].whatplace.Equals("stak5"))
                        {
                            card = Content.Load<Texture2D>(stack5[i].suit + "-" + stack5[i].rank);
                            spriteBatch.Draw(card, new Rectangle(currentMousePosition.X, currentMousePosition.Y + yy, 80, 160), Color.White);
                            yy += 25;
                        }
                    }
                    yy = 0;
                    if (selectedStack == 6)
                    for (int i = selectedStackIndex; i < stack6.Count && stack6.Count > 0; i++)
                    {
                        if (stack6[i].whatplace.Equals("stak6"))
                        {
                            card = Content.Load<Texture2D>(stack6[i].suit + "-" + stack6[i].rank);
                            spriteBatch.Draw(card, new Rectangle(currentMousePosition.X, currentMousePosition.Y + yy, 80, 160), Color.White);
                            yy += 25;
                        }
                    }
                    yy = 0;
                    if (selectedStack == 7)
                    for (int i = selectedStackIndex; i < stack7.Count && stack7.Count > 0; i++)
                    {
                        if (stack7[i].whatplace.Equals("stak7"))
                        {
                            card = Content.Load<Texture2D>(stack7[i].suit + "-" + stack7[i].rank);
                            spriteBatch.Draw(card, new Rectangle(currentMousePosition.X, currentMousePosition.Y + yy, 80, 160), Color.White);
                            yy += 25;
                        }
                    }
                }
            }
            catch(Exception e)
            {
            }
            if(mystack != null)
            if (mystack.Count > 0)
            {
                int yy = 0;
                for (int i = 0; i < mystack.Count && mystack.Count > 0; i++)
                {
                    card = Content.Load<Texture2D>(mystack[i].suit + "-" + mystack[i].rank);
                    spriteBatch.Draw(card, new Rectangle(currentMousePosition.X, currentMousePosition.Y + yy, 80, 160), Color.White);
                    yy += 25;
                }
            }
            if (s_matCard)
            {
                try
                {
                    for (int i = 0; !gameOver && i <= cardsMat.Count - 1 && cardsMat.Count > 0; i++)
                    {
                        try
                        {
                            card = Content.Load<Texture2D>(cardsMat[i].suit + "-" + cardsMat[i].rank);
                            spriteBatch.Draw(card, new Rectangle(currentMousePosition.X, currentMousePosition.Y, 80, 160), Color.White);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }
                    }
                }
                catch (Exception e)
                {
                }
            }

            currentMousePosition.X = _currentMouseState.X;
            currentMousePosition.Y = _currentMouseState.Y;

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

static class SpriteBatchEx
{
    /// <summary>
    /// Draws a single line. 
    /// Require SpriteBatch.Begin() and SpriteBatch.End()
    /// </summary>
    public static void DrawLine(this SpriteBatch spriteBatch, Vector2 begin, Vector2 end, Color color, int width = 1)
    {
        Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);

        Vector2 v = Vector2.Normalize(begin - end);

        float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));

        if (begin.Y > end.Y) angle = MathHelper.TwoPi - angle;

        spriteBatch.Draw(TexGen.White, r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
    }

    /// <summary>
    /// The graphics device, set this before drawing lines
    /// </summary>
    public static GraphicsDevice GraphicsDevice;

    static class TexGen
    {
        static Texture2D white = null;
        /// <summary>
        /// Returns a single pixel white texture, if it doesn't exist, it creates one
        /// </summary>
        /// <exception cref="System.Exception">Please set the SpriteBatchEx.GraphicsDevice to your graphicsdevice before drawing lines.</exception>
        public static Texture2D White
        {
            get
            {
                if (white == null)
                {
                    if (SpriteBatchEx.GraphicsDevice == null)
                        throw new Exception("Please set the SpriteBatchEx.GraphicsDevice to your GraphicsDevice before drawing lines.");

                    white = new Texture2D(SpriteBatchEx.GraphicsDevice, 1, 1);

                    Color[] color = new Color[1];

                    color[0] = Color.White;

                    white.SetData<Color>(color);
                }

                return white;
            }
        }
    }
}