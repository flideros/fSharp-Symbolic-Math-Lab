namespace Math.Presentation

module WolframCodeBlock =

    let animationWindow =
        "Needs[\"NETLink`\"];

        SetAttributes[AnimationWindow, HoldFirst];
        Options[AnimationWindow] = {WindowSize -> {600, 600}, Format -> Automatic, FramePause -> 0};

        AnimationWindow[func_, range_, opts___?OptionQ] :=
	        NETBlock[
    	        Module[{form, box, funcStr, size, pauseTime},
			        {size, pauseTime} = {WindowSize, FramePause} /. Flatten[{opts}] /. Options[AnimationWindow];
    		        form = NETNew[\"System.Windows.Forms.Form\"];
    		        form@Width = size[[1]];
    		        form@Height = size[[2]];
    		        box = NETNew[\"Wolfram.NETLink.UI.MathPictureBox\"];
    		        box@Parent = form;
			        LoadNETType[\"System.Windows.Forms.DockStyle\"];
			        LoadNETType[\"System.Drawing.Color\"];
    		        box@Dock = DockStyle`Fill;
    		        box@BackColor = Color`White;
			        box@PictureType = Format /. Flatten[{opts}] /. Options[AnimationWindow] /. Automatic -> \"Automatic\";
			        ShowNETWindow[form];
			        funcStr = ToString[Unevaluated[func], InputForm];
      		        Do[
      			        box@MathCommand = funcStr;
      			        Pause[pauseTime];
      			        If[form@IsDisposed, Break[]],
      			        range
			        ]
		        ]
	        ]"

    let windowSpy = 
        "Needs[\"NETLink`\"]
        GetKeyState = DefineDLLFunction[\"GetKeyState\", \"user32.dll\", \"SHORT\", {\"int\"}];
        WindowFromPoint = DefineDLLFunction[\"WindowFromPoint\", \"user32.dll\", \"HWND\", {\"System.Drawing.Point\"}, ReferencedAssemblies->{\"System.Drawing\"}];
        GetCursorPos = DefineDLLFunction[\"GetCursorPos\", \"user32.dll\", \"BOOL\", {\"System.Drawing.Point&\"}, ReferencedAssemblies->{\"System.Drawing\"}];
        GetWindowText = DefineDLLFunction[\"GetWindowText\", \"user32.dll\", \"int\", {\"HWND\", \"System.Text.StringBuilder\", \"int\"}];
        GetClassName = DefineDLLFunction[\"GetClassName\", \"user32.dll\", \"int\", {\"HWND\", \"System.Text.StringBuilder\", \"int\"}];
        GetWindowRect = DefineDLLFunction[\"GetWindowRect\", \"user32.dll\", \"BOOL\", {\"HWND\", \"System.Drawing.Rectangle&\"}, ReferencedAssemblies->{\"System.Drawing\"}];
        WindowSpy[] :=
        	NETBlock[
        		Module[{rect, pt, sb1, sb2, dummyForm, lastWnd, curWnd},
        			(* This is the Rectangle we need to pass in to GetWindowRect. It is passed by reference and its fields
        			   are filled in by the function call.
        			*)
        			rect = NETNew[\"System.Drawing.Rectangle\",0,0,0,0];
        			(* This is the Point we need to pass in to GetCursorPos. It is passed by reference and its fields
        			   are filled in by the function call.
        			*)
        			pt = NETNew[\"System.Drawing.Point\"];
        			(* We need two StringBuilders to pass in to the two calls that return text in an out parameter.
        			   We create them to hold 100 chars and then pass 100 as the max string length to the functions.
        			*)
        			sb1 = NETNew[\"System.Text.StringBuilder\", 100];
        			sb2 = NETNew[\"System.Text.StringBuilder\", 100];
        			(* It seems that for proper detection of the mouse click that terminates the function, we need
        			   to have a form visible. Therefore we create one and position it offscreen.
        			*)
        			dummyForm = NETNew[\"System.Windows.Forms.Form\"];
        			dummyForm@Location = NETNew[\"System.Drawing.Point\", -1000, -1000];
        			(* Need to use FormStartPosition->Manual to force the form to respect the Location we just set. *)
        			ShowNETWindow[dummyForm, FormStartPosition->Manual];
        			
        			While[True,
        				(* This means \"if left mouse button was clicked, break out of the loop.\" *)
        				If[GetKeyState[1] < 0, Break[]];
        				GetCursorPos[pt];
        				curWnd = WindowFromPoint[pt];
        				(* The HWND is returned by WindowFromPoint is an IntPtr, but for comparing one IntPtr
        				   to another it is easiest to simply convert them to ints.
        				*)
        				If[curWnd@ToInt32[] =!= lastWnd@ToInt32[],
        					GetClassName[curWnd, sb1, 100];
        					GetWindowText[curWnd, sb2, 100];
        					GetWindowRect[curWnd, rect];
        					Print[StringForm[\"HWND: `1`     ClassName: `2`       Text: `3`       Top: `4`  Left: `5`  Width: `6`  Height: `7`\",
        								curWnd@ToInt32[], sb1@ToString[], sb2@ToString[],
        								rect@X, rect@Y, rect@Width - rect@X, rect@Height - rect@Y]];
        					lastWnd = curWnd
        				]
        			];
        			dummyForm@Dispose[]
        		]
        	]"

    let asteroids = 
        "BeginPackage[\"Asteroids`\", {\"NETLink`\"}];
        
        Needs[\"Geometry`Rotations`\"];
        
        
        Asteroids::usage = \"Asteroids[] launches a Mathematica-programmed version of the classic Asteroids arcade game.\";
        
        
        Begin[\"`Private`\"];
        
        wasOff1 = Head[General::spell] === $Off;
        wasOff2 = Head[General::spell1] === $Off;
        Off[General::spell];
        Off[General::spell1];
        
        (************************************  Constants  *******************************************)
        
        (* Change these to alter properties/behavior of the game. *)
        
        $FieldWidth = 500;
        $FieldHeight = 400;
        
        $FrameRate = 18;
        
        $NumShips = 3;
        $ShipBonusScore = 5000;
        
        $SmallAsteroidRadius = 7.;
        $MediumAsteroidRadius = 12.;
        $LargeAsteroidRadius = 18.;
        
        (* Speeds in pixels/frame (first number in these computations is pixels/second) *)
        $SmallAsteroidSpeed = 60.0/$FrameRate;
        $MediumAsteroidSpeed = 45.0/$FrameRate;
        $LargeAsteroidSpeed = 35.0/$FrameRate;
        
        $BulletSpeed = 300./$FrameRate;
        $MaxBullets = 5;
        $FireInterval = 225;  (* millis between firing *)
        
        $ThrustBoost = 0.5;
        (* Fraction of speed loss per frame. First number is fraction/second. *)
        $SpeedAttrition = .25/$FrameRate;
        
        $NumStartingAsteroids = 4;
        
        $ShipRotationSpeed = 2Pi/18.;
        
        $ShipOutline = {{7,0}, {-4,4}, {-3,0}, {-4,-4}};
        $ShipHitOutline = {{7,0}, {2,2}, {-4,4}, {-3,0}, {-4,-4}, {2,-2}};
        $ThrustOutline = {{-8.,0.}, {-3.5,2.}, {-3.,0.}, {-3.5,-2.}};
        $AsteroidOutlines = {
            {{3.,-9.}, {5.,-5.}, {9.,-1.}, {9.,5.}, {-1.,9.}, {-3.,5.}, {-7.,5.}, {-9.,0.}, {-7.,-5.}, {-3.,-3.}, {-3.,-9.}} / 9,
            {{3.,-9.}, {3.,-5.}, {9.,-3.}, {7.,5.}, {1.,3.}, {3.,9.}, {-3.,9.}, {-9.,5.}, {-9.,-3.}, {-5.,-5.}, {-3.,-9.}} / 9,
            {{3.,-9.}, {3.,-3.}, {7.,-7.}, {9.,-1.}, {7.,5.}, {1.,9.}, {-1.,3.}, {-3.,7.}, {-9.,1.}, {-9.,-3.}, {-1.,-9.}} / 9
        };
        
        
        (**********************************  Global Variables  *************************************)
        
        (* These globals are like instance variables if Asteroids was a class. *)
        
        $asteroids;
        $ship;
        $bullets;
        $lastFireTime;
        $isThrusting;
        $curSound;
        $score;
        $gameState;
        $shipsLeft;
        $level;
        $lastFrameTicks;
        $bonusCount;
        
        (* All the following globals hold .NET objects. *)
        
        $mat;
        $keyArray;
        $scorePanel;
        
        $pen;
        $pen2;
        $font;
        $brush;
        $format;
        
        $shipPts;
        $thrustPts;
        $largeAsteroidPts;
        $mediumAsteroidPts;
        $smallAsteroidPts;
        
        
        (**********************************  Functions  *************************************)
        
        (* Not truly random because field is not square. *)
        randomEdgePosition[] :=
        	Switch[Random[Integer, {1, 4}],
        		1, {Random[Integer, {1, $FieldWidth}], 0},
        		2, {$FieldWidth, Random[Integer, {1, $FieldHeight}]},
        		3, {Random[Integer, {1, $FieldWidth}], $FieldHeight},
        		4, {0, Random[Integer, {1, $FieldHeight}]}
        	]
        	
        	
        resetGame[] :=
        	(
        		$asteroids = Table[createAsteroid[\"large\", randomEdgePosition[]], {$NumStartingAsteroids}];
        		$ship = Ship[$FieldWidth/2, $FieldHeight/2, 0, 0, 0., 0];
        		$bullets = {};
        		$lastFireTime = 0;
        		$score = 0;
        		$gameState = \"new\";
        		$shipsLeft = $NumShips;
        		$level = 0;
        		$bonusCount = 1;
        	)
        
        
        createAsteroid[size_String, {posX_, posY_}] :=
        	Module[{pts, speed, radius},
        		Switch[size,
        			\"large\",
        		        pts = $largeAsteroidPts[[ Random[Integer, {1, Length[$largeAsteroidPts]}] ]];
        		        radius = $LargeAsteroidRadius;
        				speed = $LargeAsteroidSpeed (1 + Random[Real, {-.1, .1}]),
        			\"medium\", 
        		        pts = $mediumAsteroidPts[[ Random[Integer, {1, Length[$mediumAsteroidPts]}] ]];
        		        radius = $MediumAsteroidRadius;
        				speed = $MediumAsteroidSpeed (1 + Random[Real, {-.1, .1}]),
        			\"small\", 
        		        pts = $smallAsteroidPts[[ Random[Integer, {1, Length[$smallAsteroidPts]}] ]];
        		        radius = $SmallAsteroidRadius;
        				speed = $SmallAsteroidSpeed (1 + Random[Real, {-.1, .1}])
        		];
        		asteroid[posX, posY, speed, Random[Real, {0, 2Pi}], radius, pts]
        	]
        	
        	
        (* This function is wired to the score panel's Paint event. *)
        onPaintScore[obj_, evt_] :=
            Module[{g},
        		g = evt@Graphics;
        		g@DrawLine[$pen, 0, 59, $FieldWidth, 59];
        		g@DrawString[ToString[$score], $font, $brush, $FieldWidth/2., 20., $format];
        		Do[
        		    $mat@Reset[];
        		    $mat@Translate[15 i, 35];
        		    $mat@Rotate[-90.];
        		    g@Transform = $mat;
        		    g@DrawPolygon[$pen2, $shipPts],
                    {i, $shipsLeft}
                ]
            ]
            
        
        (* This function is wired to the game panel's Paint event. *)
        onPaintGame[obj_, evt_] :=
        	Module[{g, posX, posY, lastXPos, lastYPos, pts, shipCenter, ticks, timeSinceExplodeBegan},
        		g = evt@Graphics;
        		Function[thisBullet,
        		    g@DrawRectangle[$pen, thisBullet[[1]], thisBullet[[2]], 2, 2]
        		] /@ $bullets;
        		$mat@Reset[];
        		Switch[$gameState,
        		    \"new\",
        		        g@DrawString[\"CLICK TO START\", $font, $brush, $FieldWidth/2., $FieldHeight/2., $format],
        		    \"running\",
        		        $mat@Translate[$ship[[1]], $ship[[2]]];
        		        $mat@Rotate[$ship[[5]] 360./(2Pi)];
        		        g@Transform = $mat;
        		        g@DrawPolygon[$pen, $shipPts];
        		        If[$isThrusting,
        		            g@DrawPolygon[$pen2, $thrustPts];
        		        ];
        		        $mat@Reset[],
        		    \"shipexploding\",
        		        timeSinceExplodeBegan = Environment`TickCount - $ship[[6]];
        		        Function[{pt1, pt2},
        		            $mat@Translate[$ship[[1]], $ship[[2]]];
        		            $mat@Rotate[$ship[[5]] 360./(2Pi) + (timeSinceExplodeBegan)/4000 Random[Real, {-180, 180}]];
        		            g@Transform = $mat;
        		            g@DrawLine[$pen, pt1[[1]], pt1[[2]], pt2[[1]], pt2[[2]]];
        		            $mat@Reset[];
        		        ] @@@ Partition[$ShipHitOutline, 2, 1, {1,1}],
        		    \"over\",
        		        g@DrawString[\"GAME OVER\", $font, $brush, $FieldWidth/2., $FieldHeight/2., $format]
        		];
        		lastXPos = lastYPos = 0;
        		Function[thisAsteroid,
        			posX = thisAsteroid[[1]];
        			posY = thisAsteroid[[2]];
        			$mat@Translate[posX - lastXPos, posY - lastYPos];
        			lastXPos = posX;
        			lastYPos = posY;
        			g@Transform = $mat;
        			g@DrawPolygon[$pen, thisAsteroid[[-1]]];
        		] /@ $asteroids;
        	]
        
        
        (* Wired to the timer's Tick event. *)
        updateGame[] :=
        	Module[{timeDelta, keys, v, h, newMoveAngle, newSpeed},
        	    ticks = Environment`TickCount;
        	    timeDelta = ticks - $lastFrameTicks;
        	    $lastFrameTicks = ticks;
        		$asteroids = move[#, timeDelta]& /@ $asteroids;
        		$bullets = DeleteCases[move[#, timeDelta]& /@ $bullets, Null];
        		$bullets = Select[$bullets, !hitAsteroid[#]&];
        		If[$score > $ShipBonusScore $bonusCount, $shipsLeft++; $bonusCount++; $scorePanel@Invalidate[]];
        		Switch[$gameState,
        		    \"running\",
        		        $ship = move[$ship, timeDelta];
                        If[shipHit[],
        			        playSnd[\"shipexplode\"];
        			        $gameState = \"shipexploding\";
        			        $ship = ReplacePart[$ship, Environment`TickCount, 6];
        			        Return[]
        		        ];
        		        GetKeyboardState[$keyArray];
        		        keys = NETObjectToExpression[$keyArray];
        		        If[keys[[38]] > 127,  (* Left arrow *)
        			        $ship = ReplacePart[$ship, $ship[[5]] - $ShipRotationSpeed, 5]
        		        ];
        		        If[keys[[40]] > 127,  (* Right arrow *)
        			        $ship = ReplacePart[$ship, $ship[[5]] + $ShipRotationSpeed, 5]
        		        ];
        		        If[keys[[39]] > 127,  (* Up arrow *)
        		            $isThrusting = True;
        			        playSnd[\"thrust\"];
        		            v = $ThrustBoost Sin[$ship[[5]]] + $ship[[3]] Sin[$ship[[4]]];
        		            h = $ThrustBoost Cos[$ship[[5]]] + $ship[[3]] Cos[$ship[[4]]];
        		            If[h == 0., h = 0.00001];
        		            If[v == 0., v = 0.00001];
        		            newMoveAngle = ArcTan[v/h];
        		            If[h == 0., h = 0.0001];
        		            newSpeed = v/Sin[newMoveAngle];
        			        $ship = ReplacePart[$ship, {newSpeed, newMoveAngle}, {{3}, {4}}, {{1}, {2}}],
        		        (* else *)
        		            $isThrusting = False;
        		            playSnd[\"nothrust\"];
        		            (* Speed wanes unless thrust is applied. *)
        			        $ship = ReplacePart[$ship, $ship[[3]](1 - $SpeedAttrition), 3]
        		        ];
        		        If[keys[[41]] > 127,  (* Down arrow *)
        		            (* \"Hyperspace\" jump to random position. *)
        			        $ship = ReplacePart[$ship, {Random[Integer, {0, $FieldWidth}], Random[Integer, {0, $FieldHeight}]}, {{1}, {2}}, {{1}, {2}}]
        		        ];
        		        If[keys[[33]] > 127 || keys[[18]] > 127,  (* Space or Control *)
        		            If[Length[$bullets] < $MaxBullets,
        		                ticks = Environment`TickCount;
        		                If[ticks >= $lastFireTime + $FireInterval,
        		                    $lastFireTime = ticks;
        			                AppendTo[$bullets, Bullet[$ship[[1]] + 8 Cos[$ship[[5]]], $ship[[2]] + 8 Sin[$ship[[5]]], $BulletSpeed, $ship[[5]]]];
        			                playSnd[\"fire\"]
        			            ]
        			        ]
        		        ];
        		        If[Length[$asteroids] == 0,
        		            $level++;
        		            $asteroids = Table[createAsteroid[\"large\", randomEdgePosition[]], {$NumStartingAsteroids + $level}];
        		        ],
        		    \"shipexploding\",
        		        timeSinceExplodeBegan = Environment`TickCount - $ship[[6]];
        		        If[timeSinceExplodeBegan > 2500,
        		            $gameState = \"waitingforship\"
        		        ],
        		    \"waitingforship\",
        		        If[$shipsLeft > 0,
        		            $ship = Ship[$FieldWidth/2, $FieldHeight/2, 0, 0, 0., 0];
        		            $bullets = {};
        		            $lastFireTime = 0;
        		            $gameState = \"running\";
            		        $shipsLeft--;
            		        $scorePanel@Invalidate[],
            		    (* else *)
        		            $gameState = \"over\"
            		    ]
        		];
        	]
        
        
        playSnd[soundType_String] :=
            Module[{soundFile},
                soundFile =
                    Switch[soundType,
                        \"thrust\", ToFileName[{$InstallationDirectory, \"SystemFiles\", \"Links\", \"NETLink\", \"Examples\", \"Part1\", \"Windows and Dialogs\", \"AsteroidsGame\"}, \"thrust.wav\"],
                        \"fire\", ToFileName[{$InstallationDirectory, \"SystemFiles\", \"Links\", \"NETLink\", \"Examples\", \"Part1\", \"Windows and Dialogs\", \"AsteroidsGame\"}, \"fire.wav\"],
                        \"asteroidexplode\", ToFileName[{$InstallationDirectory, \"SystemFiles\", \"Links\", \"NETLink\", \"Examples\", \"Part1\", \"Windows and Dialogs\", \"AsteroidsGame\"}, \"asteroidexplode.wav\"],
                        \"shipexplode\", ToFileName[{$InstallationDirectory, \"SystemFiles\", \"Links\", \"NETLink\", \"Examples\", \"Part1\", \"Windows and Dialogs\", \"AsteroidsGame\"}, \"shipexplode.wav\"]
                    ];                
                Which[
                    soundType == \"thrust\",
                        If[$curSound != \"thrust\", PlaySound[soundFile, 0, 9 (* SND_LOOP | SND_ASYNC *)]],
                    soundType == \"nothrust\",
                        If[$curSound == \"thrust\", PlaySound[Null, 0, 1 (* SND_ASYNC *)]],
                    True,
                        PlaySound[soundFile, 0, 1 (* SND_ASYNC *)]
                ];
                $curSound = soundType
            ]
        
            
        move[Ship[posX_, posY_, speed_, moveDir_, noseDir_, explodeTicks__], timeDelta_] :=
            Module[{mewX, newY},
        		{newX, newY} = wrapCoords[posX + speed (timeDelta $FrameRate/1000.) Cos[moveDir], posY + speed (timeDelta $FrameRate/1000.) Sin[moveDir]];
        		Ship[newX, newY, speed, moveDir, noseDir, explodeTicks]
            ]
        
        move[Bullet[posX_, posY_, speed_, dir_], timeDelta_] :=
        	Module[{newX, newY},
        		newX = posX + speed (timeDelta $FrameRate/1000.) Cos[dir];
        		newY = posY + speed (timeDelta $FrameRate/1000.) Sin[dir];
        		If[newX > $FieldWidth || newX < 0 || newY > $FieldHeight || newY < 0,
        		    (* Has left screen. *)
        		    Null,
        		(* else *)
        		    Bullet[newX, newY, speed, dir]	
        		]
        	]
        
        move[asteroid[posX_, posY_, speed_, dir_, rest___], timeDelta_] :=
        	Module[{newX, newY},
        		{newX, newY} = wrapCoords[posX + speed (timeDelta $FrameRate/1000.) Cos[dir], posY + speed (timeDelta $FrameRate/1000.) Sin[dir]];
        		asteroid[newX, newY, speed, dir, rest]	
        	]
        
        
        (* Wraps coords when they hit the boundaries of the panel, so that objects reappear on the other side. *)
        wrapCoords[posX_, posY_] :=
            Module[{newX = posX, newY = posY},
        		Which[
        			newX > $FieldWidth, newX = newX - $FieldWidth,
        			newX < 0, newX = newX + $FieldWidth
        		];
        		Which[
        			newY > $FieldHeight, newY = newY - $FieldHeight,
        			newY < 0, newY = newY + $FieldHeight
        		];
        		{newX, newY}
            ]
        
        
        (* Decides whether a bullet has hit any of the asteroids. *)
        hitAsteroid[Bullet[posX_, posY_, __]] :=
            Module[{asteroidIndex = 1, hit, radius, asteroidX, asteroidY, newRadius},
                hit = Scan[(If[Sqrt[(posX - #[[1]])^2 + (posY - #[[2]])^2] < #[[-2]], Return[asteroidIndex]]; asteroidIndex++)&, $asteroids];
                If[IntegerQ[hit],
        			playSnd[\"asteroidexplode\"];
                    {asteroidX, asteroidY, radius} = Extract[$asteroids[[hit]], {{1}, {2}, {5}}];
                    $asteroids = Delete[$asteroids, {hit}];
        		    Switch[radius,
        			    $LargeAsteroidRadius,
        			        $score += 20;
                            AppendTo[$asteroids, createAsteroid[\"medium\", {asteroidX, asteroidY}]];
                            AppendTo[$asteroids, createAsteroid[\"medium\", {asteroidX, asteroidY}]],
        			    $MediumAsteroidRadius, 
        			        $score += 50;
                            AppendTo[$asteroids, createAsteroid[\"small\", {asteroidX, asteroidY}]];
                            AppendTo[$asteroids, createAsteroid[\"small\", {asteroidX, asteroidY}]],
        			    $SmallAsteroidRadius, 
        			        $score += 100
        		    ];
        		    $scorePanel@Invalidate[];
                    True,
                (* else *)
                    False
                ]
            ]
        
        
        shipHit[] :=
            Module[{nose, leftSide, rightSide, leftWing, rightWing, tail, posX, posY, noseDir, hit, astX, astY, radius},
                {posX, posY, noseDir} = Extract[$ship, {{1}, {2}, {5}}];
                {nose, leftSide, rightSide, leftWing, rightWing, tail} = (# + {posX, posY}&) /@ (Rotate2D[#, noseDir]&) /@ $ShipHitOutline;
                hit = Scan[
                         Function[{thisAsteroid},
                             {astX, astY, radius} = Extract[thisAsteroid, {{1}, {2}, {5}}];
                             hit = Scan[If[Sqrt[(astX - #[[1]])^2 + (astY - #[[2]])^2] <= radius, Return[True]]&,
                                    {nose, leftSide, rightSide, leftWing, rightWing, tail}];
                             If[TrueQ[hit], Return[True]]
                         ],
                         $asteroids
                      ];
                TrueQ[hit]
            ]
            
            
        Asteroids[] :=
        	NETBlock[
        		Module[{frm, timer, gamePanel, timerDelegate, onTimerTick, onClick},
        		    InstallNET[];
        		    
        			(*****  DLL function defs  *****)
        			GetKeyboardState = DefineDLLFunction[\"GetKeyboardState\", \"user32.dll\", \"bool\", {\"byte[]\"}];			
        			PlaySound = DefineDLLFunction[\"PlaySound\", \"winmm.dll\", \"bool\", {\"string\", \"int\", \"int\"}];
        			
        			(*****  Prepare the game  *****)
        		    $shipPts = NETNew[\"System.Drawing.Point\", ##]& @@@ $ShipOutline;
        		    $thrustPts = NETNew[\"System.Drawing.PointF\", ##]& @@@ $ThrustOutline;	        
        	        $largeAsteroidPts =  Apply[NETNew[\"System.Drawing.PointF\", ##]&, ($LargeAsteroidRadius $AsteroidOutlines), {2}];
        	        $mediumAsteroidPts =  Apply[NETNew[\"System.Drawing.PointF\", ##]&, ($MediumAsteroidRadius $AsteroidOutlines), {2}];
        	        $smallAsteroidPts =  Apply[NETNew[\"System.Drawing.PointF\", ##]&, ($SmallAsteroidRadius $AsteroidOutlines), {2}];
                    
        		    resetGame[];
        		    
        			(*****  Load some necessary types from which we need to access static members.  *****)
        			LoadNETType[\"System.Drawing.Color\"];
        			LoadNETType[\"System.Drawing.StringAlignment\"];
        			LoadNETType[\"System.Windows.Forms.AnchorStyles\"];
        			LoadNETType[\"System.Windows.Forms.FormBorderStyle\"];
        			LoadNETType[\"System.Environment\"];
        			
        			(*****  Create the UI  *****)        
        			frm = NETNew[\"System.Windows.Forms.Form\"];
        			frm@Text = \"Asteroids\";
                    frm@FormBorderStyle = FormBorderStyle`FixedSingle;
                    frm@MaximizeBox = False;
        			frm@ClientSize = NETNew[\"System.Drawing.Size\", $FieldWidth, $FieldHeight + 60];
        			$scorePanel = NETNew[\"Wolfram.NETLink.UI.DoubleBufferedPanel\"];
        			$scorePanel@Size = NETNew[\"System.Drawing.Size\", $FieldWidth, 60];
        			$scorePanel@Parent = frm;
        			$scorePanel@BackColor = Color`Black;
        			gamePanel = NETNew[\"Wolfram.NETLink.UI.DoubleBufferedPanel\"];
        			gamePanel@Parent = frm;
        			gamePanel@BackColor = Color`Black;
        			gamePanel@Size = NETNew[\"System.Drawing.Size\", $FieldWidth, $FieldHeight];
        			gamePanel@Top = 60;
        			AddEventHandler[$scorePanel@Paint, onPaintScore];
        			AddEventHandler[gamePanel@Paint, onPaintGame];
        			AddEventHandler[gamePanel@Click, startGame, SendDelegateArguments -> None];
        			AddEventHandler[frm@Closing, (RemoveEventHandler[timer@Tick, timerDelegate]; timer@Stop[])&, SendDelegateArguments -> None];
        			startGame[] :=
        			    Switch[$gameState,
        			        \"over\",
                                resetGame[];
                                $gameState = \"new\";
        			            $scorePanel@Invalidate[],
        			        \"new\",
        			            $gameState = \"running\";
        			            $shipsLeft--;
        			            $scorePanel@Invalidate[]
                        ];
                        
        			(*****  Prepare the timer  *****)
        			timer = NETNew[\"System.Windows.Forms.Timer\"];
        			timer@Interval = Round[1000./$FrameRate];
        			onTimerTick[] := (updateGame[]; gamePanel@Refresh[]);
        			timerDelegate = AddEventHandler[timer@Tick, onTimerTick, SendDelegateArguments -> None];
        			timer@Start[];
        
        			$pen = NETNew[\"System.Drawing.Pen\", Color`White, 1];
        			$pen2 = NETNew[\"System.Drawing.Pen\", Color`White, 2];
        			$font = NETNew[\"System.Drawing.Font\", \"Arial\", 16];
                    $brush = NETNew[\"System.Drawing.SolidBrush\", Color`White];
                    $format = NETNew[\"System.Drawing.StringFormat\"];
                    $format@Alignment = StringAlignment`Center;
        
        			$mat = NETNew[\"System.Drawing.Drawing2D.Matrix\"];
        			$keyArray = NETNew[\"System.Byte[]\", 256];
        			
        			(*****  Show the UI and run the game  *****)
        			
        			DoNETModal[frm];
        			
        			(*****  Turns off any looping sounds.  ******)
        			PlaySound[Null, 0, 1 (* SND_ASYNC *)];			
        		]
        	]
        
        If[!wasOff1, On[General::spell]];
        If[!wasOff2, On[General::spell1]];
        
        End[];
        
        EndPackage[];"