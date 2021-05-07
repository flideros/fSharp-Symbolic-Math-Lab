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

