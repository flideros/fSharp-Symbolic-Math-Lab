namespace MathML

[<Measure>] type em /// an em (font-relative unit traditionally used for horizontal lengths)
[<Measure>] type ex /// an ex (font-relative unit traditionally used for vertical lengths)
[<Measure>] type px /// pixels, or size of a pixel in the current display
[<Measure>] type inch /// inches (1 inch = 2.54 centimeters)
[<Measure>] type cm /// centimeters
[<Measure>] type mm /// millimeters
[<Measure>] type pt /// points (1 point = 1/72 inch)
[<Measure>] type pc /// picas (1 pica = 12 points)
[<Measure>] type pct /// percentage of the default value

type NamedSpace = 
    | VeryVeryThinMathSpace /// 1/18em
    | VeryThinMathSpace /// 2/18em
    | ThinMathSpace /// 3/18em
    | MediumMathSpace /// 4/18em
    | ThickMathSpace /// 5/18em
    | VeryThickMathSpace /// 6/18em
    | VeryVeryThickMathSpace /// 7/18em
    | NegativeVeryVeryThinMathSpace /// -1/18em
    | NegativeVeryThinMathSpace /// -2/18em
    | NegativeThinMathSpace /// -3/18em
    | NegativeMediumMathSpace /// -4/18em
    | NegativeThickMathSpace /// -5/18em
    | NegativeVeryThickMathSpace /// -6/18em
    | NegativeVeryVeryThickMathSpace /// -7/18em

type Length =  
    | Numb of float
    | EM of float<em>
    | EX of float<ex>
    | PX of float<px>
    | IN of float<inch>
    | CM of float<cm>
    | MM of float<mm>
    | PT of float<pt>
    | PC of float<pc>
    | Pct of float<pct>
    | NamedLength of NamedSpace
    | KeyWord of obj

type Idref = string
type Uri = string

type _Id = string
type _LineBreakMultChar = string /// hex character code
type _LQuote = string /// character code
type _RQuote = string /// character code
type _ActionType = | Toggle | StatusLine | ToolTip | Input | Highlight | Other of string
type _Align = | Left | Right | Top | Bottom | Center | Baseline | Axis
type _CharAlign = | Left | Right | Center
type _CharSpacing = | Loose | Medium | Tight | Length of Length 
type _ColumnAlign = | Left | Right | Center
type _ColumnLines = | None | Solid | Dashed
type _ColumnWidth = | Length of Length | Auto | Fit
type _Crossout = | None | UpDiagonalStrike | DownDiagonalStrike | VerticalStrike | HorizontalStrike
type _DenomAlign = | Left | Right | Center
type _Dir = | Ltr | Rtl
type _Display = | Block | Inline
type _Edge = | Left | Right
type _Form = | Prefix | Infix | Postfix
type _Frame = | None | Solid | Dashed
type _GroupAlign = | Left | Center | Right | DecimalPoint
type _IndentAlign = | Left | Center | Right | Auto | Id
type _IndentAlignFirst = | Left | Center | Right | Auto | Id | IndentAlign
type _IndentAlignLast = | Left | Center | Right | Auto | Id | IndentAlign
type _LineBreak = | Auto | NewLine | NoBreak | GoodBreak | BadBreak
type _LineBreakStyle = | Before | After | Duplicate | InfixLinebBreakStyle
type _Location = | W | NW | N | NE | E | SE | S | SW
type _LongDivStyle = | LeftTop | StackedRightRight | MediumStackedRightRight | ShortStackedRightRight | RightTop | LeftRight1  | LeftRight2 | RightRight | StackedLeftLeft | StackedLeftLineTop//1 "left/\right" | "2 left)(right" | ":right=right"
type _MathVariant = 
    | Normal 
    | Bold 
    | Italic 
    | BoldItalic 
    | DoubleStruck 
    | LatinScript
    | LatinScriptBold
    | Fraktur
    | FrakturBold 
    | SansSerif 
    | SansSerifBold 
    | SansSerifItalic 
    | SansSerifBoldItalic 
    | MonoSpace   
type _Notation = | LongDiv | Actuarial | PhasOrAngle | Radical | Box | RoundedBox | Circle | Left | Right | Top | Bottom | UpDiagonalStrike | DownDiagonalStrike | VerticalStrike | HorizontalStrike | NorthEastArrow | Madruwb | Text
type _NumAlign = | Left | Right | Center
type _Overflow = | Linebreak | Scroll | Elide | Truncate | Scale
type _RowAlign = | Top | Bottom | Center | Baseline | Axis
type _RowLines = | None | Solid | Dashed
type _Side = | Left | Right | LeftOverlap | RightOerlap
type _StackAlign = | Left | Center | Right | DecimalPoint

type MathMLAttribute =
    | Accent of bool
    | AccentUnder of bool
    | ActionType of _ActionType
    | Align of _Align
    | AlignmentScope of bool
    | Alt of Uri
    | AltImg of Uri
    | AltImgHeight of Length
    | AltImgValign of Length
    | AltImgWidth of Length
    | AltText of string // Alternate string
    | Bevelled of bool
    | CdGroup of Uri
    | CharAlign of _CharAlign
    | CharSpacing of Length
    | Class of string //Associates the element with a set of style classes for use with [XSLT] and [CSS21].
    | Close of string //Specifies the closing delimiter.
    | ColumnAlign of _ColumnAlign
    | ColumnLines of _ColumnLines
    | ColumnSpacing of Length
    | ColumnSpan of uint32
    | ColumnWidth of Length
    | Crossout of _Crossout
    | DecimalPoint of char
    | DenomAlign of _DenomAlign
    | Depth of Length
    | Dir of _Dir
    | Display of _Display
    | DisplayStyle of bool
    | Edge of _Edge
    | EqualColumns of bool
    | EqualRows of bool
    | Fence of bool
    | Form of _Form
    | Frame of _Frame
    | FrameSpacing of Length*Length
    | GroupAlign of _GroupAlign
    | Height of Length
    | Href of Uri
    | Id of _Id
    | IndentAlignFirst of _IndentAlignFirst
    | IndentAlignLast of _IndentAlignLast
    | IndentAlign of _IndentAlign
    | IndentShift of Length
    | IndentShiftFirst of Length
    | IndentShiftLast of Length
    | IndentTarget of Idref
    | InfixLineBreakStyle of _LineBreakStyle
    | LargeOp of bool
    | LeftOverhang of Length
    | Length of uint32
    | LineBreak of _LineBreak
    | LineBreakMultChar of _LineBreakMultChar
    | LineBreakStyle of _LineBreakStyle
    | LineLeading of Length
    | LineThickness of Length
    | Location of _Location
    | LongDivStyle of _LongDivStyle
    | LQuote of _LQuote
    | LSpace of Length
    | MathBackground of System.Windows.Media.Brush
    | MathColor of System.Windows.Media.Brush
    | MathSize of Length
    | MathVariant of _MathVariant
    | MaxSize of Length
    | MaxWidth of Length
    | MinLabelSpacing of Length
    | MinSize of Length
    | MovableLimits of bool
    | MsLineThickness of Length
    | Notation of _Notation
    | NumAlign of _NumAlign
    | Open of string // Specifies the opening delimiter.
    | Overflow of _Overflow
    | Position of int
    | RightOverhang of Length
    | RowAlign of _RowAlign
    | RowLines of _RowLines
    | RowSpacing of Length
    | RowSpan of uint32
    | RQuote of _RQuote
    | RSpace of Length
    | ScriptLevel of char*uint32
    | ScriptMinSize of Length
    | ScriptSizeMultiplier of float
    | Selection of uint32
    | Separator of bool
    | Separators of string //Specifies a sequence of zero or more separator characters, optionally separated by whitespace.
    | Shift of int
    | Side of _Side
    | Src of Uri
    | StackAlign of _StackAlign
    | Stretchy of bool
    | Style of string //Associates style information with the element for use with [XSLT] and [CSS21].
    | SubScriptShift of Length
    | SuperScriptShift of Length
    | Symmetric of bool
    | VAlign of Length
    | VOffset of Length
    | Width of Length
    | Xmlns of Uri
    | Xref of Idref

type CharacterCode = 
    | Unicode of int
    | Char of char
    | UnicodeArray of CharacterCode array

type TokenElement = 
    | Mi       /// identifier
    | Mn       /// number
    | Mo       /// operator, fence, or separator
    | Mtext    /// text
    | Mspace   /// space
    | Ms       /// string literal
    | Mglyph   /// non-standard symbol as image
type GeneralLayoutElement = 
    | Mrow     /// group any number of sub-expressions horizontally
    | Mfrac    /// form a fraction from two sub-expressions
    | Msqrt    /// form a square root (radical without an index)
    | Mroot    /// form a radical with specified index
    | Mstyle   /// style change
    | Merror   /// enclose a syntax error message from a preprocessor
    | Mpadded  /// adjust space around content
    | Mphantom /// make content invisible but preserve its size
    | Mfenced  /// surround content with a pair of fences
    | Menclose /// enclose content with a stretching symbol such as a long division sign.
type ScriptElement = 
    | Msub         /// attach a subscript to a base
    | Msup         /// attach a superscript to a base
    | Msubsup      /// attach a subscript-superscript pair to a base
    | Munde        /// attach an underscript to a base
    | Mover        /// attach an overscript to a base
    | Munderover   /// attach an underscript-overscript pair to a base
    | Mmultiscripts /// attach prescripts and tensor indices to a base
type TableElement = 
    | Mtable      /// table or matrix
    | Mlabeledtr  /// row in a table or matrix with a label or equation number
    | Mtr         /// row in a table or matrix
    | Mtd         /// one entry in a table or matrix
    | Maligngroup /// alignment marker
    | Malignmark  /// alignment marker
type MathLayoutElement = 
    | Mstack    /// columns of aligned characters
    | Mlongdiv  /// similar to msgroup, with the addition of a divisor and result
    | Msgroup   /// a group of rows in an mstack that are shifted by similar amounts
    | Msrow     ///	a row in an mstack
    | Mscarries /// row in an mstack that whose contents represent carries or borrows
    | Mscarry   /// one entry in an mscarries
    | Msline    /// horizontal line inside of mstack
type EnliveningExpressionElement = 
    | Maction   /// bind actions to a sub-expression


type MathMLElement = 
    | Math
    | Token of TokenElement 
    | GeneralLayout of GeneralLayoutElement 
    | Script of ScriptElement 
    | Table of TableElement 
    | MathLayout of MathLayoutElement 
    | Enlivening of EnliveningExpressionElement

type Operator = 
    { character : CharacterCode
      glyph : string
      name : string
      form : _Form 
      priority : int //Significance for the proper grouping of sub-expressions
      lspace : Length
      rspace : Length
      properties: MathMLAttribute list 
    } 

type Element = 
    { element : MathMLElement; 
      attributes : MathMLAttribute list; 
      openTag : string; 
      closeTag : string; 
      symbol : string;
      arguments: Element list;
      operator: Operator option
      }

module Operator =

    let getValueFromLength (emSquare:float<em>) length =
        match length with
        | NamedLength l when l = VeryVeryThinMathSpace -> (1./18.<em>) * emSquare
        | NamedLength l when l = VeryThinMathSpace -> (2./18.<em>) * emSquare
        | NamedLength l when l = ThinMathSpace -> (3./18.<em>) * emSquare
        | NamedLength l when l = MediumMathSpace -> (4./18.<em>) * emSquare
        | NamedLength l when l = ThickMathSpace -> (5./18.<em>) * emSquare
        | NamedLength l when l = VeryThickMathSpace -> (6./18.<em>) * emSquare
        | NamedLength l when l = VeryVeryThickMathSpace -> (7./18.<em>) * emSquare
        | NamedLength l when l = NegativeVeryVeryThinMathSpace -> (-1./18.<em>) * emSquare
        | NamedLength l when l = NegativeVeryThinMathSpace -> (-2./18.<em>) * emSquare
        | NamedLength l when l = NegativeThinMathSpace -> (-3./18.<em>) * emSquare
        | NamedLength l when l = NegativeMediumMathSpace -> (-4./18.<em>) * emSquare
        | NamedLength l when l = NegativeThickMathSpace -> (-5./18.<em>) * emSquare
        | NamedLength l when l = NegativeVeryThickMathSpace -> (-6./18.<em>) * emSquare
        | NamedLength l when l = NegativeVeryVeryThickMathSpace -> (-7./18.<em>) * emSquare
        | EM em -> float em * float emSquare
        | Numb n -> n
        | _ -> 0.

module Element =    
    let private isValidElementAttributeOf defaultAttrs attr = List.exists (fun elem -> elem.GetType() = attr.GetType()) defaultAttrs
    let private scrubAttributes attrList defaultAttr = 
        let newValidAttributes =
            List.choose (fun elem ->
                match elem with
                | elem when isValidElementAttributeOf defaultAttr elem -> Option.Some elem
                | _ -> Option.None) attrList
        let remainingDefaultAttributes = 
            List.choose (fun elem ->
                match elem with
                | elem when isValidElementAttributeOf newValidAttributes elem -> Option.None
                | _ -> Option.Some elem) defaultAttr
        List.concat [newValidAttributes; remainingDefaultAttributes]
    let private getAttrString (x:MathMLAttribute) = 
        let addOrRemoveSpace x = match x with | "" -> "" | _ -> " " + x
        let convertLength (l:string)=
            match l.Contains("(") with
            | true -> 
                let value =                    
                    let start = match l.IndexOf(" ") with | x when x > 0 -> x | _ -> 0
                    let last = match l.IndexOf(")") with | x when x > start -> x | _ -> l.Length
                    l.Substring(start,last-start).Trim()
                let unit =
                    let start = match l.IndexOf("(") with | x when x > 0 -> x | _ -> 0
                    let last = match l.IndexOf(" ") with | x when x > start -> x | _ -> l.Length
                    l.Substring(start,last-start).ToLower().Trim()
                match unit with
                | "em" | "ex" | "px" | "cm" | "in" | "mm" | "pt" | "pc" -> "=\"" + value + unit
                | "pct" -> "=\"" + value + "%"
                | "namedlength" | "keyword" | "numb" -> "=\"" + value
                | _ -> l
            |false -> l
        (x.GetType().Name.ToLowerInvariant() 
         + convertLength (x.ToString().Replace(x.GetType().Name + " ","=\""))
         + "\"").ToString().Replace("\"\"", "\"")
        |> addOrRemoveSpace
    
    let rec recurseElement eToken eRow eSuperscript eSubscript eSuperSubscript eFraction  el : 'r =
        let recurse = recurseElement eToken eRow eSuperscript eSubscript eSuperSubscript eFraction 
        match el.element with 
        | Math -> eRow (List.map (fun x -> recurse x) el.arguments)
        | Token _ -> eToken el
        | GeneralLayout Mrow -> eRow (List.map (fun x -> recurse x) el.arguments)
        | GeneralLayout Mfrac -> eFraction (recurse el.arguments.[0],recurse el.arguments.[1],el.attributes)
        | Script Msup -> eSuperscript (recurse el.arguments.[0],recurse el.arguments.[1],el.attributes)
        | Script Msub -> eSubscript (recurse el.arguments.[0],recurse el.arguments.[1],el.attributes)
        | Script Msubsup -> eSuperSubscript (recurse el.arguments.[0],recurse el.arguments.[1],recurse el.arguments.[2],el.attributes)
        
    let build (elem : MathMLElement) (attr : MathMLAttribute list) (arguments : Element list) (symbol : string) (operator : Operator option) =                 
        let openTag attrString = 
            match elem with
            | Math _ -> "<math " + attrString + ">"
            | Token x -> "<" + (x.ToString().ToLower()) + attrString + ">"
            | GeneralLayout x -> "<" + (x.ToString().ToLower()) + attrString + ">"
            | Script x -> "<" + (x.ToString().ToLower()) + attrString + ">"
            | Table x -> "<" + (x.ToString().ToLower()) + attrString + ">"
            | MathLayout x -> "<" + (x.ToString().ToLower()) + attrString + ">"
            | Enlivening x -> "<" + (x.ToString().ToLower()) + attrString + ">"
        let closeTag = 
            match elem with
            | Math _ -> "</math>"
            | Token x -> "</" + x.ToString().ToLower() + ">"
            | GeneralLayout x -> "</" + x.ToString().ToLower() + ">"
            | Script x -> "</" + x.ToString().ToLower() + ">"
            | Table x -> "</" + x.ToString().ToLower() + ">"
            | MathLayout x -> "</" + x.ToString().ToLower() + ">"
            | Enlivening x -> "</" + x.ToString().ToLower() + ">"
        match elem with
        | Math ->
            let defaultAttributes = 
                [//2.2 Top-Level <math> Element
                Display Inline;
                MaxWidth (KeyWord "available width");
                Overflow Linebreak; 
                AltImg "none";                                      
                AltImgWidth (KeyWord "altimg width"); 
                AltImgHeight (KeyWord "altimg height");
                AltImgValign (EM 0.0<em>); 
                AltText ""; 
                CdGroup ""; 
                                     
                //3.3.4 Style Change <mstyle>
                ScriptLevel ('+',0u);
                DisplayStyle false ///When display="block", displaystyle is initialized to "true" when display="inline", displaystyle is initialized to "false"
                ScriptSizeMultiplier 0.70;
                ScriptMinSize (PT 8.0<pt>);
                InfixLineBreakStyle Before;
                DecimalPoint '.';
                                     
                //2.1.6 Attributes Shared by all MathML Elements 
                Id "none"; 
                Xref "none"; 
                Class "none"; 
                Style "none"; 
                Href "none";
                                     
                //3.1.10 Mathematics style attributes common to presentation elements 
                MathColor System.Windows.Media.Brushes.Black; 
                MathBackground System.Windows.Media.Brushes.Transparent; 

                //3.2.2 Mathematics style attributes common to token elements 
                MathVariant Normal;
                MathSize (EM 1.0<em>);
                Dir Ltr;
                                     
                //The rest after I compared this to the MathML schema
                Accent false;
                AccentUnder false;
                Align _Align.Center;
                AlignmentScope true;
                Bevelled false;
                CharAlign _CharAlign.Center;
                CharSpacing (KeyWord _CharSpacing.Medium);
                Close ")";
                ColumnAlign _ColumnAlign.Center;
                ColumnLines _ColumnLines.None;
                ColumnSpacing (EM 0.8<em>);
                ColumnSpan 1u;
                ColumnWidth (KeyWord _ColumnWidth.Auto);
                Crossout _Crossout.None;
                DenomAlign _DenomAlign.Center;
                Depth (EX 0.0<ex>);
                Edge _Edge.Left;
                EqualColumns false;
                EqualRows false;
                Fence false;
                Form _Form.Infix;
                Frame _Frame.None;
                FrameSpacing (EM 0.4<em>,EX 0.5<ex>);
                GroupAlign _GroupAlign.Left;
                Height (KeyWord "fromimage");
                IndentAlign _IndentAlign.Auto;
                IndentAlignFirst _IndentAlignFirst.IndentAlign;
                IndentAlignLast _IndentAlignLast.IndentAlign;
                IndentShift (Numb 0.0);
                IndentShiftFirst (KeyWord "indentshift");
                IndentShiftLast (KeyWord "indentshift");
                IndentTarget "none";
                LargeOp false;
                LeftOverhang (Numb 0.0);
                Length 0u; //<msline/> Specifies the the number of columns that should be spanned by the line.
                LineBreak _LineBreak.Auto;
                LineBreakMultChar "02062";//&InvisibleTimes;
                LineBreakStyle _LineBreakStyle.Before;
                LineLeading (Pct 100.0<pct>);
                LineThickness (KeyWord "medium"); //"thin" | "medium" | "thick"
                Location N;
                LongDivStyle LeftTop;
                LQuote "&quot;";
                LSpace (NamedLength ThickMathSpace);
                MaxSize (KeyWord "infinity");
                MinLabelSpacing (EM 0.8<em>);
                MinSize (Pct 100.0<pct>);
                MovableLimits false;
                MsLineThickness (KeyWord "medium");
                Notation LongDiv;
                NumAlign _NumAlign.Center;
                Open "(";
                Position 0;
                RightOverhang (Numb 0.0);
                RowAlign _RowAlign.Baseline;
                RowLines _RowLines.None;
                RowSpacing (EX 1.0<ex>);
                RowSpan 1u;
                RQuote "&quot;";
                RSpace (NamedLength ThickMathSpace);
                Selection 1u;
                Separator false;
                Separators ",";
                Shift 0;
                Side _Side.Right;
                StackAlign _StackAlign.DecimalPoint;
                Stretchy false;
                SubScriptShift (KeyWord "automatic");
                SuperScriptShift (KeyWord "automatic");
                Symmetric false;
                VAlign (EX 0.0<ex>);
                Width (KeyWord "automatic");
                ]
            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}

        | Token Mi -> 
            let defaultAttributes = 
                [//2.1.6 Attributes Shared by all MathML Elements 
                Id "none"; 
                Xref "none"; 
                Class "none"; 
                Style "none"; 
                Href "none";
                                     
                //3.1.10 Mathematics style attributes common to presentation elements 
                MathColor System.Windows.Media.Brushes.Black; 
                MathBackground System.Windows.Media.Brushes.Transparent; 

                //3.2.2 Mathematics style attributes common to token elements 
                MathVariant Italic;
                MathSize (EM 1.0<em>);
                Dir Ltr;
                ]
            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = []
              operator = Option.None}
        
        | Token Mn ->
            let defaultAttributes =  
                [//2.1.6 Attributes Shared by all MathML Elements 
                    Id "none"; 
                    Xref "none"; 
                    Class "none"; 
                    Style "none"; 
                    Href "none";
                                     
                    //3.1.10 Mathematics style attributes common to presentation elements 
                    MathColor System.Windows.Media.Brushes.Black; 
                    MathBackground System.Windows.Media.Brushes.Transparent; 

                    //3.2.2 Mathematics style attributes common to token elements 
                    MathVariant Normal;
                    MathSize (EM 1.0<em>);
                    Dir Ltr;                                     
                    ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = []
              operator = Option.None}
        
        | Token Mo ->
            let defaultAttributes =  
               [//2.1.6 Attributes Shared by all MathML Elements 
                Id "none"; 
                Xref "none"; 
                Class "none"; 
                Style "none"; 
                Href "none";
                                     
                //3.1.10 Mathematics style attributes common to presentation elements 
                MathColor System.Windows.Media.Brushes.Black; 
                MathBackground System.Windows.Media.Brushes.Transparent; 

                //3.2.2 Mathematics style attributes common to token elements 
                MathVariant Normal;
                MathSize (EM 1.0<em>);
                Dir Ltr;                                     
                                     
                //3.2.5.2.1 Dictionary-based attributes 
                Fence false;
                Form _Form.Infix; 
                Separator false;
                LSpace (NamedLength ThickMathSpace); 
                RSpace (NamedLength ThickMathSpace); 
                Stretchy false
                Symmetric false
                MaxSize (KeyWord "infinity");                                     
                MinSize (Pct 100.0<pct>);
                LargeOp false;
                MovableLimits false;
                Accent false;
                                     
                //3.2.5.2.2 Linebreaking attributes 
                LineBreak _LineBreak.Auto;
                LineBreakMultChar "02062"; //&InvisibleTimes;
                LineBreakStyle _LineBreakStyle.Before;
                LineLeading (Pct 100.0<pct>);
                                     
                //3.2.5.2.3 Indentation attributes
                IndentAlign _IndentAlign.Auto;
                IndentAlignFirst _IndentAlignFirst.IndentAlign;
                IndentAlignLast _IndentAlignLast.IndentAlign;
                IndentShift (Numb 0.0);
                IndentShiftFirst (KeyWord "indentshift");
                IndentShiftLast (KeyWord "indentshift");
                IndentTarget "none";                                     
                ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            let newAttr = 
                match operator with
                | Some o -> List.concat[o.properties; attr; [LSpace o.lspace;RSpace o.rspace;Form o.form]]
                | Option.None -> attr
            { element = elem; 
              attributes = (scrubAttributes newAttr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = operator}
        
        | Token Mtext ->
            let defaultAttributes =  
                                    [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent; 

                                     //3.2.2 Mathematics style attributes common to token elements 
                                     MathVariant Normal;
                                     MathSize (EM 1.0<em>);
                                     Dir Ltr;  
                                     ]
                                     
            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = []
              operator = Option.None}
        
        | Token Mspace ->
            let defaultAttributes =  
                                    [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent; 

                                     //3.2.2 Mathematics style attributes common to token elements 
                                     MathVariant Normal;
                                     MathSize (EM 1.0<em>);
                                     Dir Ltr;
                                     
                                     //3.2.7.2 Attributes 
                                     Width (EM 0.0<em>);
                                     Height (EX 0.0<ex>);
                                     Depth (EX 0.0<ex>);
                                     LineBreak _LineBreak.Auto
                                     
                                     //3.2.5.2.3 Indentation attributes 
                                     IndentAlign _IndentAlign.Auto;
                                     IndentAlignFirst _IndentAlignFirst.IndentAlign;
                                     IndentAlignLast _IndentAlignLast.IndentAlign;
                                     IndentShift (Numb 0.0);
                                     IndentShiftFirst (KeyWord "indentshift");
                                     IndentShiftLast (KeyWord "indentshift");
                                     IndentTarget "none";                                     
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = []
              operator = Option.None}
        
        | Token Ms ->
            let defaultAttributes =  
                                    [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent; 

                                     //3.2.2 Mathematics style attributes common to token elements 
                                     MathVariant Normal;
                                     MathSize (EM 1.0<em>);
                                     Dir Ltr;
                                     
                                     //3.2.8.2 Attributes 
                                     RQuote "&quot;";
                                     LQuote "&quot;";
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = []
              operator = Option.None}
        
        | Token Mglyph ->
            let defaultAttributes =  
                                    [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent; 

                                     //3.2.1.2.2 Attributes 
                                     Src "required"
                                     Width (KeyWord "fromimage");
                                     Height (KeyWord "fromimage");                                     
                                     VAlign (EX 0.0<ex>);
                                     Alt "required"
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = []
              operator = Option.None}
        
        | GeneralLayout Mrow ->
            let defaultAttributes = 
                [//2.1.6 Attributes Shared by all MathML Elements 
                    Id "none"; 
                    Xref "none"; 
                    Class "none"; 
                    Style "none"; 
                    Href "none";
                                     
                    //3.1.10 Mathematics style attributes common to presentation elements 
                    MathColor System.Windows.Media.Brushes.Black; 
                    MathBackground System.Windows.Media.Brushes.Transparent;

                    //3.3.1.2 Attributes
                    Dir Ltr
                ]
            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | GeneralLayout Mfrac ->
            let defaultAttributes =
                [//2.1.6 Attributes Shared by all MathML Elements 
                 Id "none"; 
                 Xref "none"; 
                 Class "none"; 
                 Style "none"; 
                 Href "none";
                                     
                 //3.1.10 Mathematics style attributes common to presentation elements 
                 MathColor System.Windows.Media.Brushes.Black; 
                 MathBackground System.Windows.Media.Brushes.Transparent;

                 //3.3.2.2 Attributes
                 LineThickness (KeyWord "medium"); //"thin" | "medium" | "thick"
                 NumAlign _NumAlign.Center;
                 DenomAlign _DenomAlign.Center;
                 Bevelled false;
                ]
            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | GeneralLayout Msqrt ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | GeneralLayout Mroot ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | GeneralLayout Mstyle ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;

                                     //3.3.4.2 Attributes 
                                     ScriptLevel ('+',0u)
                                     DisplayStyle false ///When display="block", displaystyle is initialized to "true" when display="inline", displaystyle is initialized to "false"
                                     ScriptSizeMultiplier 0.71
                                     ScriptMinSize (PT 8.0<pt>)
                                     InfixLineBreakStyle Before;
                                     DecimalPoint '.'

                                     //The rest after I compared this to the MathML schema
                                     Accent false;
                                     AccentUnder false;
                                     Align _Align.Center;
                                     AlignmentScope true;
                                     Bevelled false;
                                     CharAlign _CharAlign.Center;
                                     CharSpacing (KeyWord _CharSpacing.Medium);
                                     Close ")";
                                     ColumnAlign _ColumnAlign.Center;
                                     ColumnLines _ColumnLines.None;
                                     ColumnSpacing (EM 0.8<em>);
                                     ColumnSpan 1u;
                                     ColumnWidth (KeyWord _ColumnWidth.Auto);
                                     Crossout _Crossout.None;
                                     DenomAlign _DenomAlign.Center;
                                     Depth (EX 0.0<ex>);
                                     Edge _Edge.Left;
                                     EqualColumns false;
                                     EqualRows false;
                                     Fence false;
                                     Form _Form.Infix;
                                     Frame _Frame.None;
                                     FrameSpacing (EM 0.4<em>,EX 0.5<ex>);
                                     GroupAlign _GroupAlign.Left;
                                     Height (KeyWord "fromimage");
                                     IndentAlign _IndentAlign.Auto;
                                     IndentAlignFirst _IndentAlignFirst.IndentAlign;
                                     IndentAlignLast _IndentAlignLast.IndentAlign;
                                     IndentShift (Numb 0.0);
                                     IndentShiftFirst (KeyWord "indentshift");
                                     IndentShiftLast (KeyWord "indentshift");
                                     IndentTarget "none";
                                     LargeOp false;
                                     LeftOverhang (Numb 0.0);
                                     Length 0u; //<msline/> Specifies the the number of columns that should be spanned by the line.
                                     LineBreak _LineBreak.Auto;
                                     LineBreakMultChar "02062";//&InvisibleTimes;
                                     LineBreakStyle _LineBreakStyle.Before;
                                     LineLeading (Pct 100.0<pct>);
                                     LineThickness (KeyWord "medium"); //"thin" | "medium" | "thick"
                                     Location N;
                                     LongDivStyle LeftTop;
                                     LQuote "&quot;";
                                     LSpace (NamedLength ThickMathSpace);
                                     MaxSize (KeyWord "infinity");
                                     MinLabelSpacing (EM 0.8<em>);
                                     MinSize (Pct 100.0<pct>);
                                     MovableLimits false;
                                     MsLineThickness (KeyWord "medium");
                                     Notation LongDiv;
                                     NumAlign _NumAlign.Center;
                                     Open ")";
                                     Position 0;
                                     RightOverhang (Numb 0.0);
                                     RowAlign _RowAlign.Baseline;
                                     RowLines _RowLines.None;
                                     RowSpacing (EX 1.0<ex>);
                                     RowSpan 1u;
                                     RQuote "&quot;";
                                     RSpace (NamedLength ThickMathSpace);
                                     Selection 1u;
                                     Separator false;
                                     Separators ",";
                                     Shift 0;
                                     Side _Side.Right;
                                     StackAlign _StackAlign.DecimalPoint;
                                     Stretchy false;
                                     SubScriptShift (KeyWord "automatic");
                                     SuperScriptShift (KeyWord "automatic");
                                     Symmetric false;
                                     VAlign (EX 0.0<ex>);
                                     Width (KeyWord "automatic");
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | GeneralLayout Merror ->
            let defaultAttributes = 
                [//2.1.6 Attributes Shared by all MathML Elements 
                 Id "none"; 
                 Xref "none"; 
                 Class "none"; 
                 Style "none"; 
                 Href "none";
                                     
                 //3.1.10 Mathematics style attributes common to presentation elements 
                 MathColor System.Windows.Media.Brushes.Black; 
                 MathBackground System.Windows.Media.Brushes.Transparent;

                //
                ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | GeneralLayout Mpadded ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;

                                     //3.3.6.2 Attributes
                                     Depth (KeyWord "same as content");                                   
                                     Height (KeyWord "same as content");
                                     Width (KeyWord "same as content");
                                     LSpace (EM 0.8<em>);
                                     VOffset (EM 0.8<em>);
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | GeneralLayout Mphantom ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | GeneralLayout Mfenced ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;

                                     //3.3.8.2 Attributes
                                     Open ")";
                                     Close ")";
                                     Separators ",";
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | GeneralLayout Menclose ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;

                                     //3.3.9.2 Attributes
                                     Notation LongDiv;
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | Script Msub ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;

                                     //3.4.1.2 Attributes 
                                     SubScriptShift (KeyWord "automatic");
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | Script Msup ->
            let defaultAttributes = //2.1.6 Attributes Shared by all MathML Elements
                [Id "none"; 
                 Xref "none"; 
                 Class "none"; 
                 Style "none"; 
                 Href "none";
                                     
                 //3.1.10 Mathematics style attributes common to presentation elements 
                 MathColor System.Windows.Media.Brushes.Black; 
                 MathBackground System.Windows.Media.Brushes.Transparent;

                 //3.4.2.2 Attributes 
                 SuperScriptShift (KeyWord "automatic");
                ]
            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            match arguments.Length = 2 with
            | false ->
                { element = GeneralLayout Merror; 
                  attributes = (scrubAttributes attr defaultAttributes); 
                  openTag = openTag aString;
                  closeTag = closeTag;
                  symbol = "Msup requires 2 arguments; base(index 0) & superscript(index 0)";
                  arguments = arguments
                  operator = Option.None}
            | true ->             
                { element = elem; 
                  attributes = (scrubAttributes attr defaultAttributes); 
                  openTag = openTag aString;
                  closeTag = closeTag;
                  symbol = symbol;
                  arguments = arguments
                  operator = Option.None}
        
        | Script Msubsup ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;

                                     //3.4.3.2 Attributes 
                                     SubScriptShift (KeyWord "automatic");
                                     SuperScriptShift (KeyWord "automatic");
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | Script Munde ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;

                                     //3.4.4.2 Attributes
                                     AccentUnder false; //automatic
                                     Align _Align.Center;
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | Script Mover ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;

                                     //3.4.5.2 Attributes
                                     Accent false; //automatic
                                     Align _Align.Center;
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | Script Munderover ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;

                                     //3.4.6.2 Attributes
                                     Accent false; //automatic
                                     AccentUnder false; //automatic
                                     Align _Align.Center;
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | Script Mmultiscripts ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;

                                     //3.4.7.2 Attributes 
                                     SubScriptShift (KeyWord "automatic");
                                     SuperScriptShift (KeyWord "automatic");
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | Table Mtable ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;

                                     //3.5.1.2 Attributes
                                     Align _Align.Axis;
                                     AlignmentScope true;                                     
                                     ColumnAlign _ColumnAlign.Center;
                                     ColumnLines _ColumnLines.None;
                                     ColumnSpacing (EM 0.8<em>);
                                     ColumnWidth (KeyWord _ColumnWidth.Auto);
                                     DisplayStyle false
                                     EqualColumns false;
                                     EqualRows false;                                     
                                     Frame _Frame.None;
                                     FrameSpacing (EM 0.4<em>,EX 0.5<ex>);
                                     GroupAlign _GroupAlign.Left;                                     
                                     MinLabelSpacing (EM 0.8<em>);                                   
                                     RowAlign _RowAlign.Baseline;
                                     RowLines _RowLines.None;
                                     RowSpacing (EX 1.0<ex>);
                                     Side _Side.Right;                                     
                                     Width (KeyWord "automatic");
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | Table Mlabeledtr ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;

                                     //3.5.3.2 Attributes
                                     RowAlign _RowAlign.Baseline; //inherited
                                     ColumnAlign _ColumnAlign.Center; //inherited
                                     GroupAlign _GroupAlign.Left; //inherited
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | Table Mtr ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;

                                     //3.5.2.2 Attributes
                                     RowAlign _RowAlign.Baseline; //inherited
                                     ColumnAlign _ColumnAlign.Center; //inherited
                                     GroupAlign _GroupAlign.Left; //inherited
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | Table Mtd ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;

                                     //3.5.4.2 Attributes
                                     RowSpan 1u;
                                     ColumnSpan 1u;
                                     RowAlign _RowAlign.Baseline; //inherited
                                     ColumnAlign _ColumnAlign.Center; //inherited
                                     GroupAlign _GroupAlign.Left; //inherited
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | Table Maligngroup ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;

                                     //3.5.5.6 <maligngroup/> Attributes 
                                     GroupAlign _GroupAlign.Left; //inherited
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | Table Malignmark ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;

                                     //3.5.5.5 <malignmark/> Attributes 
                                     Edge _Edge.Left;
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | MathLayout Mstack ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;

                                     //3.6.1.2 Attributes
                                     Align _Align.Baseline;
                                     StackAlign _StackAlign.DecimalPoint;
                                     CharAlign _CharAlign.Right;
                                     CharSpacing (KeyWord _CharSpacing.Medium);
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | MathLayout Mlongdiv ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;

                                     //3.6.2.2 Attributes 
                                     LongDivStyle LeftTop;
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | MathLayout Msgroup ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;

                                     //3.6.3.2 Attributes
                                     Position 0;
                                     Shift 0;
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | MathLayout Msrow ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;

                                     //3.6.4.2 Attributes 
                                     Position 0;
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | MathLayout Mscarries ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;

                                     //3.6.5.2 Attributes
                                     Position 0;
                                     Location N;
                                     Crossout _Crossout.None;
                                     ScriptSizeMultiplier 0.6 //inherited
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | MathLayout Mscarry ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;

                                     //3.6.6.2 Attributes
                                     Location N; //inherited
                                     Crossout _Crossout.None; //inherited
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | MathLayout Msline ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;

                                     //3.6.7.2 Attributes
                                     Position 0;
                                     Length 0u; //<msline/> Specifies the the number of columns that should be spanned by the line.
                                     LeftOverhang (Numb 0.0);
                                     RightOverhang (Numb 0.0);
                                     MsLineThickness (KeyWord "medium");
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}
        
        | Enlivening Maction ->
            let defaultAttributes = [//2.1.6 Attributes Shared by all MathML Elements 
                                     Id "none"; 
                                     Xref "none"; 
                                     Class "none"; 
                                     Style "none"; 
                                     Href "none";
                                     
                                     //3.1.10 Mathematics style attributes common to presentation elements 
                                     MathColor System.Windows.Media.Brushes.Black; 
                                     MathBackground System.Windows.Media.Brushes.Transparent;

                                     //3.7.1.1 Attributes
                                     ActionType (Other "none")
                                     Selection 1u;
                                     ]

            let aString = (List.fold (fun acc x -> acc + x) "" (List.map (fun x -> getAttrString x) (scrubAttributes attr defaultAttributes)))
            { element = elem; 
              attributes = (scrubAttributes attr defaultAttributes); 
              openTag = openTag aString;
              closeTag = closeTag;
              symbol = symbol;
              arguments = arguments
              operator = Option.None}

    //Top-Level Constructor
    let math a = (build (Math) a)
    
    //Token Constructors
    let mi a = (build (Token Mi) a) // identifier
    let mn a = (build (Token Mn) a) // number
    let mo a = (build (Token Mo) a) // operator, fence, or separator
    let mtext a = (build (Token Mtext) a) // text
    let mspace a = (build (Token Mspace) a) // space
    let ms a = (build (Token Ms) a) // string literal

    //General Layout Constructors
    let mrow a = (build (GeneralLayout Mrow) a)   // group any number of sub-expressions horizontally
    let mfrac a = (build (GeneralLayout Mfrac) a) // form a fraction from two sub-expressions
    let msqrt a = (build (GeneralLayout Msqrt) a) // form a square root (radical without an index)
    let mroot a = (build (GeneralLayout Mroot) a) // form a radical with specified index
    let mstyle a = (build (GeneralLayout Mstyle) a) // style change
    let merror a = (build (GeneralLayout Merror) a) // enclose a syntax error message from a preprocessor
    let mpadded a = (build (GeneralLayout Mpadded) a) // adjust space around content
    let mphantom a = (build (GeneralLayout Mphantom) a) // make content invisible but preserve its size
    let mfenced a = (build (GeneralLayout Mfenced) a) // surround content with a pair of fences
    let menclose a = (build (GeneralLayout Menclose) a) // enclose content with a stretching symbol such as a long division sign.

    //Script Constructors
    let msub a = (build (Script Msub) a)
    let msup a = (build (Script Msup) a)
    let msubsup a = (build (Script Msubsup) a)
    let munde a = (build (Script Munde) a)
    let mover a = (build (Script Mover) a)
    let munderover a = (build (Script Munderover) a)        
    let mmultiscripts a = (build (Script Mmultiscripts) a)

    //Table Constructors
    let mtable a = (build (Table Mtable) a)
    let mlabeledtr a = (build (Table Mlabeledtr) a)
    let mtr a = (build (Table Mtr) a)
    let mtd a = (build (Table Mtd) a)
    let maligngroup a = (build (Table Maligngroup) a)
    let malignmark a = (build (Table Malignmark) a)

    //Math Layout Constructors
    let mstack a = (build (MathLayout Mstack) a)
    let mlongdiv a = (build (MathLayout Mlongdiv) a)
    let msgroup a = (build (MathLayout Msgroup) a)
    let msrow a = (build (MathLayout Msrow) a)
    let mscarries a = (build (MathLayout Mscarries) a)
    let mscarry a = (build (MathLayout Mscarry) a)        
    let msline a = (build (MathLayout Msline) a)

    //Math Layout Constructors EnliveningExpressionElement = | Maction
    let enliveningExpression a = (build (Enlivening Maction) a)


