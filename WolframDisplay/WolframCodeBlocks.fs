namespace Math.Presentation

open System
open System.IO

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

    let asteroids = fun () ->
        let sr = new StreamReader("AsteroidsGame.txt")
        let out = try sr.ReadToEnd() finally "" |> ignore
        out
