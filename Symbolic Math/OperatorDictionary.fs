module OperatorDictionary

//This is where I will connect to a database to get operator defenitions one I create the database table

type Operator = 
    { character : string
      glyph : string
      name : string
      form : string
      priority : string
      lspace : string
      rspace : string
      properties: string list    
    }  

let acuteAccentPostfix = {character = "&#xB4;"; glyph = "´"; name="acute accent"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let acuteAngleInfix = {character = "&#x299F;"; glyph = "⦟"; name="acute angle"; form="infix"; priority="270"; lspace="3"; rspace="3"; properties=[]}
let allEqualToInfix = {character = "&#x224C;"; glyph = "≌"; name="all equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let almostEqualOrEqualToInfix = {character = "&#x224A;"; glyph = "≊"; name="almost equal or equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let almostEqualToInfix = {character = "&#x2248;"; glyph = "≈"; name="almost equal to"; form="infix"; priority="247"; lspace="5"; rspace="5"; properties=[]}
let almostEqualToWithCircumflexAccentInfix = {character = "&#x2A6F;"; glyph = "⩯"; name="almost equal to with circumflex accent"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let amalgamationOrCoproductInfix = {character = "&#x2A3F;"; glyph = "⨿"; name="amalgamation or coproduct"; form="infix"; priority="390"; lspace="4"; rspace="4"; properties=[]}
let ampersandPostfix = {character = "&amp;"; glyph = "&"; name="ampersand"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=[]}
let anglePrefix = {character = "&#x2220;"; glyph = "∠"; name="angle"; form="prefix"; priority="670"; lspace="0"; rspace="0"; properties=[]}
let angleWithSInsideInfix = {character = "&#x299E;"; glyph = "⦞"; name="angle with s inside"; form="infix"; priority="270"; lspace="3"; rspace="3"; properties=[]}
let angleWithUnderbarInfix = {character = "&#x29A4;"; glyph = "⦤"; name="angle with underbar"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let anticlockwiseClosedCircleArrowInfix = {character = "&#x2940;"; glyph = "⥀"; name="anticlockwise closed circle arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let anticlockwiseContourIntegralPrefix = {character = "&#x2233;"; glyph = "∳"; name="anticlockwise contour integral"; form="prefix"; priority="310"; lspace="0"; rspace="1"; properties=["largeop"; "symmetric"]}
let anticlockwiseIntegrationPrefix = {character = "&#x2A11;"; glyph = "⨑"; name="anticlockwise integration"; form="prefix"; priority="310"; lspace="1"; rspace="2"; properties=["largeop"; "movablelimits"; "symmetric"]}
let anticlockwiseOpenCircleArrowInfix = {character = "&#x21BA;"; glyph = "↺"; name="anticlockwise open circle arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let anticlockwiseTopSemicircleArrowInfix = {character = "&#x21B6;"; glyph = "↶"; name="anticlockwise top semicircle arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let apostrophePostfix = {character = "'"; glyph = "'"; name="apostrophe"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let approachesTheLimitInfix = {character = "&#x2250;"; glyph = "≐"; name="approaches the limit"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let approximatelyButNotActuallyEqualToInfix = {character = "&#x2246;"; glyph = "≆"; name="approximately but not actually equal to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let approximatelyEqualOrEqualToInfix = {character = "&#x2A70;"; glyph = "⩰"; name="approximately equal or equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let approximatelyEqualToInfix = {character = "&#x2245;"; glyph = "≅"; name="approximately equal to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let approximatelyEqualToOrTheImageOfInfix = {character = "&#x2252;"; glyph = "≒"; name="approximately equal to or the image of"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let arrowPointingDownwardsThenCurvingLeftwardsInfix = {character = "&#x2936;"; glyph = "⤶"; name="arrow pointing downwards then curving leftwards"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let arrowPointingDownwardsThenCurvingRightwardsInfix = {character = "&#x2937;"; glyph = "⤷"; name="arrow pointing downwards then curving rightwards"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let arrowPointingRightwardsThenCurvingDownwardsInfix = {character = "&#x2935;"; glyph = "⤵"; name="arrow pointing rightwards then curving downwards"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let arrowPointingRightwardsThenCurvingUpwardsInfix = {character = "&#x2934;"; glyph = "⤴"; name="arrow pointing rightwards then curving upwards"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let assertionInfix = {character = "&#x22A6;"; glyph = "⊦"; name="assertion"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let asteriskInfix = {character = "*"; glyph = "*"; name="asterisk"; form="infix"; priority="390"; lspace="3"; rspace="3"; properties=[]}
let asteriskOperatorInfix = {character = "&#x2217;"; glyph = "∗"; name="asterisk operator"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let asymptoticallyEqualToInfix = {character = "&#x2243;"; glyph = "≃"; name="asymptotically equal to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let becauseInfix = {character = "&#x2235;"; glyph = "∵"; name="because"; form="infix"; priority="70"; lspace="5"; rspace="5"; properties=[]}
let betweenInfix = {character = "&#x226C;"; glyph = "≬"; name="between"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let bigReverseSolidusInfix = {character = "&#x29F9;"; glyph = "⧹"; name="big reverse solidus"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let bigSolidusInfix = {character = "&#x29F8;"; glyph = "⧸"; name="big solidus"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let blackBowtieInfix = {character = "&#x29D3;"; glyph = "⧓"; name="black bowtie"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let blackCircleInfix = {character = "&#x25CF;"; glyph = "●"; name="black circle"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let blackCircleWithDownArrowInfix = {character = "&#x29ED;"; glyph = "⧭"; name="black circle with down arrow"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let blackDiamondInfix = {character = "&#x25C6;"; glyph = "◆"; name="black diamond"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let blackDiamondWithDownArrowInfix = {character = "&#x29EA;"; glyph = "⧪"; name="black diamond with down arrow"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let blackDownPointingSmallTriangleInfix = {character = "&#x25BE;"; glyph = "▾"; name="black down-pointing small triangle"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let blackDownPointingTriangleInfix = {character = "&#x25BC;"; glyph = "▼"; name="black down-pointing triangle"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let blackHourglassInfix = {character = "&#x29D7;"; glyph = "⧗"; name="black hourglass"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let blackLeftPointingPointerInfix = {character = "&#x25C4;"; glyph = "◄"; name="black left-pointing pointer"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let blackLeftPointingSmallTriangleInfix = {character = "&#x25C2;"; glyph = "◂"; name="black left-pointing small triangle"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let blackLeftPointingTriangleInfix = {character = "&#x25C0;"; glyph = "◀"; name="black left-pointing triangle"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let blackLozengeInfix = {character = "&#x29EB;"; glyph = "⧫"; name="black lozenge"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let blackParallelogramInfix = {character = "&#x25B0;"; glyph = "▰"; name="black parallelogram"; form="infix"; priority="260"; lspace="3"; rspace="3"; properties=[]}
let blackRightPointingSmallTriangleInfix = {character = "&#x25B8;"; glyph = "▸"; name="black right-pointing small triangle"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let blackRightPointingTriangleInfix = {character = "&#x25B6;"; glyph = "▶"; name="black right-pointing triangle"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let blackSmallSquareInfix = {character = "&#x25AA;"; glyph = "▪"; name="black small square"; form="infix"; priority="260"; lspace="3"; rspace="3"; properties=[]}
let blackSquareInfix = {character = "&#x25A0;"; glyph = "■"; name="black square"; form="infix"; priority="260"; lspace="3"; rspace="3"; properties=[]}
let blackUpPointingSmallTriangleInfix = {character = "&#x25B4;"; glyph = "▴"; name="black up-pointing small triangle"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let blackUpPointingTriangleInfix = {character = "&#x25B2;"; glyph = "▲"; name="black up-pointing triangle"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let blackVerticalRectangleInfix = {character = "&#x25AE;"; glyph = "▮"; name="black vertical rectangle"; form="infix"; priority="260"; lspace="3"; rspace="3"; properties=[]}
let bottomArcAnticlockwiseArrowInfix = {character = "&#x293B;"; glyph = "⤻"; name="bottom arc anticlockwise arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let bottomCurlyBracketPostfix = {character = "&#x23DF;"; glyph = "⏟"; name="bottom curly bracket"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["stretchy"; "accent"]}
let bottomParenthesisPostfix = {character = "&#x23DD;"; glyph = "⏝"; name="bottom parenthesis"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["stretchy"; "accent"]}
let bottomSquareBracketPostfix = {character = "&#x23B5;"; glyph = "⎵"; name="bottom square bracket"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["stretchy"; "accent"]}
let bottomTortoiseShellBracketPostfix = {character = "&#x23E1;"; glyph = "⏡"; name="bottom tortoise shell bracket"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["stretchy"; "accent"]}
let bowtieInfix = {character = "&#x22C8;"; glyph = "⋈"; name="bowtie"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let bowtieWithLeftHalfBlackInfix = {character = "&#x29D1;"; glyph = "⧑"; name="bowtie with left half black"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let bowtieWithRightHalfBlackInfix = {character = "&#x29D2;"; glyph = "⧒"; name="bowtie with right half black"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let brevePostfix = {character = "&#x2D8;"; glyph = "˘"; name="breve"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let bulletInfix = {character = "&#x2022;"; glyph = "•"; name="bullet"; form="infix"; priority="390"; lspace="4"; rspace="4"; properties=[]}
let bulletOperatorInfix = {character = "&#x2219;"; glyph = "∙"; name="bullet operator"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let bullseyeInfix = {character = "&#x25CE;"; glyph = "◎"; name="bullseye"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let caronPostfix = {character = "&#x2C7;"; glyph = "ˇ"; name="caron"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["stretchy"; "accent"]}
let cedillaPostfix = {character = "&#xB8;"; glyph = "¸"; name="cedilla"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let circledAnticlockwiseRotatedDivisionSignInfix = {character = "&#x29BC;"; glyph = "⦼"; name="circled anticlockwise-rotated division sign"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let circledAsteriskOperatorInfix = {character = "&#x229B;"; glyph = "⊛"; name="circled asterisk operator"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let circledBulletInfix = {character = "&#x29BF;"; glyph = "⦿"; name="circled bullet"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let circledDashInfix = {character = "&#x229D;"; glyph = "⊝"; name="circled dash"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let circledDivisionSignInfix = {character = "&#x2A38;"; glyph = "⨸"; name="circled division sign"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let circledDivisionSlashInfix = {character = "&#x2298;"; glyph = "⊘"; name="circled division slash"; form="infix"; priority="300"; lspace="4"; rspace="4"; properties=[]}
let circledDotOperatorInfix = {character = "&#x2299;"; glyph = "⊙"; name="circled dot operator"; form="infix"; priority="710"; lspace="4"; rspace="4"; properties=[]}
let circledEqualsInfix = {character = "&#x229C;"; glyph = "⊜"; name="circled equals"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let circledGreaterThanInfix = {character = "&#x29C1;"; glyph = "⧁"; name="circled greater-than"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let circleDividedByHorizontalBarAndTopHalfDividedByVerticalBarInfix = {character = "&#x29BA;"; glyph = "⦺"; name="circle divided by horizontal bar and top half divided by vertical bar"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let circledLessThanInfix = {character = "&#x29C0;"; glyph = "⧀"; name="circled less-than"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let circledMinusInfix = {character = "&#x2296;"; glyph = "⊖"; name="circled minus"; form="infix"; priority="300"; lspace="4"; rspace="4"; properties=[]}
let circledMultiplicationSignWithCircumflexAccentInfix = {character = "&#x2A36;"; glyph = "⨶"; name="circled multiplication sign with circumflex accent"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let circledParallelInfix = {character = "&#x29B7;"; glyph = "⦷"; name="circled parallel"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let circledPerpendicularInfix = {character = "&#x29B9;"; glyph = "⦹"; name="circled perpendicular"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let circledPlusInfix = {character = "&#x2295;"; glyph = "⊕"; name="circled plus"; form="infix"; priority="300"; lspace="4"; rspace="4"; properties=[]}
let circledReverseSolidusInfix = {character = "&#x29B8;"; glyph = "⦸"; name="circled reverse solidus"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let circledRingOperatorInfix = {character = "&#x229A;"; glyph = "⊚"; name="circled ring operator"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let circledTimesInfix = {character = "&#x2297;"; glyph = "⊗"; name="circled times"; form="infix"; priority="410"; lspace="4"; rspace="4"; properties=[]}
let circledVerticalBarInfix = {character = "&#x29B6;"; glyph = "⦶"; name="circled vertical bar"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let circledWhiteBulletInfix = {character = "&#x29BE;"; glyph = "⦾"; name="circled white bullet"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let circleWithHorizontalBarInfix = {character = "&#x29B5;"; glyph = "⦵"; name="circle with horizontal bar"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let circleWithSmallCircleToTheRightInfix = {character = "&#x29C2;"; glyph = "⧂"; name="circle with small circle to the right"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let circleWithSuperimposedXInfix = {character = "&#x29BB;"; glyph = "⦻"; name="circle with superimposed x"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let circleWithTwoHorizontalStrokesToTheRightInfix = {character = "&#x29C3;"; glyph = "⧃"; name="circle with two horizontal strokes to the right"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let circleWithVerticalFillInfix = {character = "&#x25CD;"; glyph = "◍"; name="circle with vertical fill"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let circulationFunctionPrefix = {character = "&#x2A10;"; glyph = "⨐"; name="circulation function"; form="prefix"; priority="310"; lspace="1"; rspace="2"; properties=["largeop"; "movablelimits"; "symmetric"]}
let circumflexAccentInfix = {character = "^"; glyph = "^"; name="circumflex accent"; form="infix"; priority="780"; lspace="1"; rspace="1"; properties=[]}
let circumflexAccentPostfix = {character = "^"; glyph = "^"; name="circumflex accent"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["stretchy"; "accent"]}
let clockwiseClosedCircleArrowInfix = {character = "&#x2941;"; glyph = "⥁"; name="clockwise closed circle arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let clockwiseContourIntegralPrefix = {character = "&#x2232;"; glyph = "∲"; name="clockwise contour integral"; form="prefix"; priority="310"; lspace="0"; rspace="1"; properties=["largeop"; "symmetric"]}
let clockwiseIntegralPrefix = {character = "&#x2231;"; glyph = "∱"; name="clockwise integral"; form="prefix"; priority="310"; lspace="0"; rspace="1"; properties=["largeop"; "symmetric"]}
let clockwiseOpenCircleArrowInfix = {character = "&#x21BB;"; glyph = "↻"; name="clockwise open circle arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let clockwiseTopSemicircleArrowInfix = {character = "&#x21B7;"; glyph = "↷"; name="clockwise top semicircle arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let closedIntersectionWithSerifsInfix = {character = "&#x2A4D;"; glyph = "⩍"; name="closed intersection with serifs"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let closedSubsetInfix = {character = "&#x2ACF;"; glyph = "⫏"; name="closed subset"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let closedSubsetOrEqualToInfix = {character = "&#x2AD1;"; glyph = "⫑"; name="closed subset or equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let closedSupersetInfix = {character = "&#x2AD0;"; glyph = "⫐"; name="closed superset"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let closedSupersetOrEqualToInfix = {character = "&#x2AD2;"; glyph = "⫒"; name="closed superset or equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let closedUnionWithSerifsAndSmashProductInfix = {character = "&#x2A50;"; glyph = "⩐"; name="closed union with serifs and smash product"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let closedUnionWithSerifsInfix = {character = "&#x2A4C;"; glyph = "⩌"; name="closed union with serifs"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let colonEqualsInfix = {character = "&#x2254;"; glyph = "≔"; name="colon equals"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let colonInfix = {character = ":"; glyph = ":"; name="colon"; form="infix"; priority="100"; lspace="1"; rspace="2"; properties=[]}
let combiningCircumflexAccentPostfix = {character = "&#x302;"; glyph = "̂"; name="combining circumflex accent"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["stretchy"; "accent"]}
let combiningFourDotsAbovePostfix = {character = "&#x20DC;"; glyph = "⃜"; name="combining four dots above"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let combiningInvertedBrevePostfix = {character = "&#x311;"; glyph = "̑"; name="combining inverted breve"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let combiningThreeDotsAbovePostfix = {character = "&#x20DB;"; glyph = "⃛"; name="combining three dots above"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let commaInfix = {character = ","; glyph = ","; name="comma"; form="infix"; priority="40"; lspace="0"; rspace="3"; properties=["separator"; "linebreakstyle=after"]}
let commercialAtInfix = {character = "@"; glyph = "@"; name="commercial at"; form="infix"; priority="825"; lspace="1"; rspace="1"; properties=[]}
let complementInfix = {character = "&#x2201;"; glyph = "∁"; name="complement"; form="infix"; priority="240"; lspace="1"; rspace="2"; properties=[]}
let congruentWithDotAboveInfix = {character = "&#x2A6D;"; glyph = "⩭"; name="congruent with dot above"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let containsAsMemberInfix = {character = "&#x220B;"; glyph = "∋"; name="contains as member"; form="infix"; priority="160"; lspace="5"; rspace="5"; properties=[]}
let containsAsNormalSubgroupInfix = {character = "&#x22B3;"; glyph = "⊳"; name="contains as normal subgroup"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let containsAsNormalSubgroupOrEqualToInfix = {character = "&#x22B5;"; glyph = "⊵"; name="contains as normal subgroup or equal to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let containsWithLongHorizontalStrokeInfix = {character = "&#x22FA;"; glyph = "⋺"; name="contains with long horizontal stroke"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let containsWithOverbarInfix = {character = "&#x22FD;"; glyph = "⋽"; name="contains with overbar"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let containsWithVerticalBarAtEndOfHorizontalStrokeInfix = {character = "&#x22FB;"; glyph = "⋻"; name="contains with vertical bar at end of horizontal stroke"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let contourIntegralPrefix = {character = "&#x222E;"; glyph = "∮"; name="contour integral"; form="prefix"; priority="310"; lspace="0"; rspace="1"; properties=["largeop"; "symmetric"]}
let correspondsToInfix = {character = "&#x2258;"; glyph = "≘"; name="corresponds to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let cubeRootPrefix = {character = "&#x221B;"; glyph = "∛"; name="cube root"; form="prefix"; priority="845"; lspace="1"; rspace="1"; properties=[]}
let curlyLogicalAndInfix = {character = "&#x22CF;"; glyph = "⋏"; name="curly logical and"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let curlyLogicalOrInfix = {character = "&#x22CE;"; glyph = "⋎"; name="curly logical or"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let degreeSignPostfix = {character = "&#xB0;"; glyph = "°"; name="degree sign"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=[]}
let deltaEqualToInfix = {character = "&#x225C;"; glyph = "≜"; name="delta equal to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let diaeresisPostfix = {character = "&#xA8;"; glyph = "¨"; name="diaeresis"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let diamondOperatorInfix = {character = "&#x22C4;"; glyph = "⋄"; name="diamond operator"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let differenceBetweenInfix = {character = "&#x224F;"; glyph = "≏"; name="difference between"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let differenceBetweenWithSlashInfix = {character = "&#x224F;&#x338;"; glyph = "≏̸"; name="difference between with slash"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let dividesInfix = {character = "&#x2223;"; glyph = "∣"; name="divides"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let divisionSignInfix = {character = "&#xF7;"; glyph = "÷"; name="division sign"; form="infix"; priority="660"; lspace="4"; rspace="4"; properties=[]}
let divisionSlashInfix = {character = "&#x2215;"; glyph = "∕"; name="division slash"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=["stretchy"]}
let divisionTimesInfix = {character = "&#x22C7;"; glyph = "⋇"; name="division times"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let doesNotContainAsMemberInfix = {character = "&#x220C;"; glyph = "∌"; name="does not contain as member"; form="infix"; priority="240"; lspace="5"; rspace="5"; properties=[]}
let doesNotContainAsNormalSubgroupInfix = {character = "&#x22EB;"; glyph = "⋫"; name="does not contain as normal subgroup"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let doesNotContainAsNormalSubgroupOrEqualInfix = {character = "&#x22ED;"; glyph = "⋭"; name="does not contain as normal subgroup or equal"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let doesNotDivideInfix = {character = "&#x2224;"; glyph = "∤"; name="does not divide"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let doesNotDivideWithReversedNegationSlashInfix = {character = "&#x2AEE;"; glyph = "⫮"; name="does not divide with reversed negation slash"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let doesNotForceInfix = {character = "&#x22AE;"; glyph = "⊮"; name="does not force"; form="infix"; priority="170"; lspace="5"; rspace="5"; properties=[]}
let doesNotPrecedeInfix = {character = "&#x2280;"; glyph = "⊀"; name="does not precede"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let doesNotPrecedeOrEqualInfix = {character = "&#x22E0;"; glyph = "⋠"; name="does not precede or equal"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let doesNotProveInfix = {character = "&#x22AC;"; glyph = "⊬"; name="does not prove"; form="infix"; priority="170"; lspace="5"; rspace="5"; properties=[]}
let doesNotSucceedInfix = {character = "&#x2281;"; glyph = "⊁"; name="does not succeed"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let doesNotSucceedOrEqualInfix = {character = "&#x22E1;"; glyph = "⋡"; name="does not succeed or equal"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let dotAbovePostfix = {character = "&#x2D9;"; glyph = "˙"; name="dot above"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let dotMinusInfix = {character = "&#x2238;"; glyph = "∸"; name="dot minus"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let dotOperatorInfix = {character = "&#x22C5;"; glyph = "⋅"; name="dot operator"; form="infix"; priority="390"; lspace="4"; rspace="4"; properties=[]}
let dotPlusInfix = {character = "&#x2214;"; glyph = "∔"; name="dot plus"; form="infix"; priority="275"; lspace="4"; rspace="4"; properties=[]}
let dottedCircleInfix = {character = "&#x25CC;"; glyph = "◌"; name="dotted circle"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let dottedFenceInfix = {character = "&#x2999;"; glyph = "⦙"; name="dotted fence"; form="infix"; priority="270"; lspace="3"; rspace="3"; properties=[]}
let doubleAcuteAccentPostfix = {character = "&#x2DD;"; glyph = "˝"; name="double acute accent"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let doubleColonEqualInfix = {character = "&#x2A74;"; glyph = "⩴"; name="double colon equal"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let doubleDownTackInfix = {character = "&#x2AEA;"; glyph = "⫪"; name="double down tack"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let doubleEndedMultimapInfix = {character = "&#x29DF;"; glyph = "⧟"; name="double-ended multimap"; form="infix"; priority="270"; lspace="3"; rspace="3"; properties=[]}
let doubleHighReversed9QuotationMarkPostfix = {character = "&#x201F;"; glyph = "‟"; name="double high-reversed-9 quotation mark"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let doubleIntegralPrefix = {character = "&#x222C;"; glyph = "∬"; name="double integral"; form="prefix"; priority="300"; lspace="0"; rspace="1"; properties=["largeop"; "symmetric"]}
let doubleIntersectionInfix = {character = "&#x22D2;"; glyph = "⋒"; name="double intersection"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let doubleLeftArcGreaterThanBracketPrefix = {character = "&#x2995;"; glyph = "⦕"; name="double left arc greater-than bracket"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let doubleLineEqualToOrGreaterThanInfix = {character = "&#x2A9A;"; glyph = "⪚"; name="double-line equal to or greater-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let doubleLineEqualToOrLessThanInfix = {character = "&#x2A99;"; glyph = "⪙"; name="double-line equal to or less-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let doubleLineSlantedEqualToOrGreaterThanInfix = {character = "&#x2A9C;"; glyph = "⪜"; name="double-line slanted equal to or greater-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let doubleLineSlantedEqualToOrLessThanInfix = {character = "&#x2A9B;"; glyph = "⪛"; name="double-line slanted equal to or less-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let doubleLineSlantedGreaterThanOrEqualToInfix = {character = "&#x2AFA;"; glyph = "⫺"; name="double-line slanted greater-than or equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let doubleLineSlantedLessThanOrEqualToInfix = {character = "&#x2AF9;"; glyph = "⫹"; name="double-line slanted less-than or equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let doubleLogicalAndInfix = {character = "&#x2A53;"; glyph = "⩓"; name="double logical and"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let doubleLogicalOrInfix = {character = "&#x2A54;"; glyph = "⩔"; name="double logical or"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let doubleLow9QuotationMarkPostfix = {character = "&#x201E;"; glyph = "„"; name="double low-9 quotation mark"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let doubleNestedGreaterThanInfix = {character = "&#x2AA2;"; glyph = "⪢"; name="double nested greater-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let doubleNestedGreaterThanWithSlashInfix = {character = "&#x2AA2;&#x338;"; glyph = "⪢̸"; name="double nested greater-than with slash"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let doubleNestedLessThanInfix = {character = "&#x2AA1;"; glyph = "⪡"; name="double nested less-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let doubleNestedLessThanWithSlashInfix = {character = "&#x2AA1;&#x338;"; glyph = "⪡̸"; name="double nested less-than with slash"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let doubleNestedLessThanWithUnderbarInfix = {character = "&#x2AA3;"; glyph = "⪣"; name="double nested less-than with underbar"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let doublePlusInfix = {character = "&#x29FA;"; glyph = "⧺"; name="double plus"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let doublePrecedesInfix = {character = "&#x2ABB;"; glyph = "⪻"; name="double precedes"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let doublePrimePostfix = {character = "&#x2033;"; glyph = "″"; name="double prime"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let doubleRightArcLessThanBracketPostfix = {character = "&#x2996;"; glyph = "⦖"; name="double right arc less-than bracket"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let doubleSolidusOperatorInfix = {character = "&#x2AFD;"; glyph = "⫽"; name="double solidus operator"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let doubleSquareIntersectionInfix = {character = "&#x2A4E;"; glyph = "⩎"; name="double square intersection"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let doubleSquareUnionInfix = {character = "&#x2A4F;"; glyph = "⩏"; name="double square union"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let doubleStrokeNotSignInfix = {character = "&#x2AEC;"; glyph = "⫬"; name="double stroke not sign"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let doubleStruckItalicCapitalDPrefix = {character = "&#x2145;"; glyph = "ⅅ"; name="double-struck italic capital d"; form="prefix"; priority="845"; lspace="2"; rspace="1"; properties=[]}
let doubleStruckItalicSmallDPrefix = {character = "&#x2146;"; glyph = "ⅆ"; name="double-struck italic small d"; form="prefix"; priority="845"; lspace="2"; rspace="0"; properties=[]}
let doubleSubsetInfix = {character = "&#x22D0;"; glyph = "⋐"; name="double subset"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let doubleSucceedsInfix = {character = "&#x2ABC;"; glyph = "⪼"; name="double succeeds"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let doubleSupersetInfix = {character = "&#x22D1;"; glyph = "⋑"; name="double superset"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let doubleUnionInfix = {character = "&#x22D3;"; glyph = "⋓"; name="double union"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let doubleUpTackInfix = {character = "&#x2AEB;"; glyph = "⫫"; name="double up tack"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let doubleVerticalBarDoubleLeftTurnstileInfix = {character = "&#x2AE5;"; glyph = "⫥"; name="double vertical bar double left turnstile"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let doubleVerticalBarDoubleRightTurnstileInfix = {character = "&#x22AB;"; glyph = "⊫"; name="double vertical bar double right turnstile"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let doubleVerticalBarLeftTurnstileInfix = {character = "&#x2AE3;"; glyph = "⫣"; name="double vertical bar left turnstile"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let doubleVerticalLinePostfix = {character = "&#x2016;"; glyph = "‖"; name="double vertical line"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"]}
let doubleVerticalLinePrefix = {character = "&#x2016;"; glyph = "‖"; name="double vertical line"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"]}
let downFishTailInfix = {character = "&#x297F;"; glyph = "⥿"; name="down fish tail"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let downPointingTriangleWithLeftHalfBlackInfix = {character = "&#x29E8;"; glyph = "⧨"; name="down-pointing triangle with left half black"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let downPointingTriangleWithRightHalfBlackInfix = {character = "&#x29E9;"; glyph = "⧩"; name="down-pointing triangle with right half black"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let downRightDiagonalEllipsisInfix = {character = "&#x22F1;"; glyph = "⋱"; name="down right diagonal ellipsis"; form="infix"; priority="150"; lspace="5"; rspace="5"; properties=[]}
let downTackInfix = {character = "&#x22A4;"; glyph = "⊤"; name="down tack"; form="infix"; priority="170"; lspace="5"; rspace="5"; properties=[]}
let downTackWithCircleBelowInfix = {character = "&#x2AF1;"; glyph = "⫱"; name="down tack with circle below"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let downwardsArrowFromBarInfix = {character = "&#x21A7;"; glyph = "↧"; name="downwards arrow from bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let downwardsArrowInfix = {character = "&#x2193;"; glyph = "↓"; name="downwards arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let downwardsArrowLeftwardsOfUpwardsArrowInfix = {character = "&#x21F5;"; glyph = "⇵"; name="downwards arrow leftwards of upwards arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let downwardsArrowToBarInfix = {character = "&#x2913;"; glyph = "⤓"; name="downwards arrow to bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let downwardsArrowWithCornerLeftwardsInfix = {character = "&#x21B5;"; glyph = "↵"; name="downwards arrow with corner leftwards"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let downwardsArrowWithDoubleStrokeInfix = {character = "&#x21DF;"; glyph = "⇟"; name="downwards arrow with double stroke"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let downwardsArrowWithHorizontalStrokeInfix = {character = "&#x2908;"; glyph = "⤈"; name="downwards arrow with horizontal stroke"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let downwardsArrowWithTipLeftwardsInfix = {character = "&#x21B2;"; glyph = "↲"; name="downwards arrow with tip leftwards"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let downwardsArrowWithTipRightwardsInfix = {character = "&#x21B3;"; glyph = "↳"; name="downwards arrow with tip rightwards"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let downwardsDashedArrowInfix = {character = "&#x21E3;"; glyph = "⇣"; name="downwards dashed arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let downwardsDoubleArrowInfix = {character = "&#x21D3;"; glyph = "⇓"; name="downwards double arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let downwardsHarpoonWithBarbLeftBesideDownwardsHarpoonWithBarbRightInfix = {character = "&#x2965;"; glyph = "⥥"; name="downwards harpoon with barb left beside downwards harpoon with barb right"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let downwardsHarpoonWithBarbLeftBesideUpwardsHarpoonWithBarbRightInfix = {character = "&#x296F;"; glyph = "⥯"; name="downwards harpoon with barb left beside upwards harpoon with barb right"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let downwardsHarpoonWithBarbLeftFromBarInfix = {character = "&#x2961;"; glyph = "⥡"; name="downwards harpoon with barb left from bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let downwardsHarpoonWithBarbLeftToBarInfix = {character = "&#x2959;"; glyph = "⥙"; name="downwards harpoon with barb left to bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let downwardsHarpoonWithBarbLeftwardsInfix = {character = "&#x21C3;"; glyph = "⇃"; name="downwards harpoon with barb leftwards"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let downwardsHarpoonWithBarbRightFromBarInfix = {character = "&#x295D;"; glyph = "⥝"; name="downwards harpoon with barb right from bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let downwardsHarpoonWithBarbRightToBarInfix = {character = "&#x2955;"; glyph = "⥕"; name="downwards harpoon with barb right to bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let downwardsHarpoonWithBarbRightwardsInfix = {character = "&#x21C2;"; glyph = "⇂"; name="downwards harpoon with barb rightwards"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let downwardsPairedArrowsInfix = {character = "&#x21CA;"; glyph = "⇊"; name="downwards paired arrows"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let downwardsQuadrupleArrowInfix = {character = "&#x27F1;"; glyph = "⟱"; name="downwards quadruple arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let downwardsTripleArrowInfix = {character = "&#x290B;"; glyph = "⤋"; name="downwards triple arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let downwardsTwoHeadedArrowInfix = {character = "&#x21A1;"; glyph = "↡"; name="downwards two headed arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let downwardsWhiteArrowInfix = {character = "&#x21E9;"; glyph = "⇩"; name="downwards white arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let downwardsZigzagArrowInfix = {character = "&#x21AF;"; glyph = "↯"; name="downwards zigzag arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let elementOfInfix = {character = "&#x2208;"; glyph = "∈"; name="element of"; form="infix"; priority="240"; lspace="5"; rspace="5"; properties=[]}
let elementOfOpeningDownwardsInfix = {character = "&#x2AD9;"; glyph = "⫙"; name="element of opening downwards"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let elementOfWithDotAboveInfix = {character = "&#x22F5;"; glyph = "⋵"; name="element of with dot above"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let elementOfWithLongHorizontalStrokeInfix = {character = "&#x22F2;"; glyph = "⋲"; name="element of with long horizontal stroke"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let elementOfWithOverbarInfix = {character = "&#x22F6;"; glyph = "⋶"; name="element of with overbar"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let elementOfWithTwoHorizontalStrokesInfix = {character = "&#x22F9;"; glyph = "⋹"; name="element of with two horizontal strokes"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let elementOfWithUnderbarInfix = {character = "&#x22F8;"; glyph = "⋸"; name="element of with underbar"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let elementOfWithVerticalBarAtEndOfHorizontalStrokeInfix = {character = "&#x22F3;"; glyph = "⋳"; name="element of with vertical bar at end of horizontal stroke"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let emptySetWithLeftArrowAboveInfix = {character = "&#x29B4;"; glyph = "⦴"; name="empty set with left arrow above"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let emptySetWithOverbarInfix = {character = "&#x29B1;"; glyph = "⦱"; name="empty set with overbar"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let emptySetWithRightArrowAboveInfix = {character = "&#x29B3;"; glyph = "⦳"; name="empty set with right arrow above"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let emptySetWithSmallCircleAboveInfix = {character = "&#x29B2;"; glyph = "⦲"; name="empty set with small circle above"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let endOfProofInfix = {character = "&#x220E;"; glyph = "∎"; name="end of proof"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let equalAndParallelToInfix = {character = "&#x22D5;"; glyph = "⋕"; name="equal and parallel to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let equalsColonInfix = {character = "&#x2255;"; glyph = "≕"; name="equals colon"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let equalsSignAbovePlusSignInfix = {character = "&#x2A71;"; glyph = "⩱"; name="equals sign above plus sign"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let equalsSignAboveRightwardsArrowInfix = {character = "&#x2971;"; glyph = "⥱"; name="equals sign above rightwards arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let equalsSignAboveTildeOperatorInfix = {character = "&#x2A73;"; glyph = "⩳"; name="equals sign above tilde operator"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let equalsSignAndSlantedParallelInfix = {character = "&#x29E3;"; glyph = "⧣"; name="equals sign and slanted parallel"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let equalsSignAndSlantedParallelWithTildeAboveInfix = {character = "&#x29E4;"; glyph = "⧤"; name="equals sign and slanted parallel with tilde above"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let equalsSignEqualsInfix = {character = "="; glyph = "="; name="equals sign"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let equalsSignWithBumpyAboveInfix = {character = "&#x2AAE;"; glyph = "⪮"; name="equals sign with bumpy above"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let equalsSignWithDotBelowInfix = {character = "&#x2A66;"; glyph = "⩦"; name="equals sign with dot below"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let equalsSignWithTwoDotsAboveAndTwoDotsBelowInfix = {character = "&#x2A77;"; glyph = "⩷"; name="equals sign with two dots above and two dots below"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let equalsWithAsteriskInfix = {character = "&#x2A6E;"; glyph = "⩮"; name="equals with asterisk"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let equalToByDefinitionInfix = {character = "&#x225D;"; glyph = "≝"; name="equal to by definition"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let equalToOrGreaterThanInfix = {character = "&#x22DD;"; glyph = "⋝"; name="equal to or greater-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let equalToOrLessThanInfix = {character = "&#x22DC;"; glyph = "⋜"; name="equal to or less-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let equalToOrPrecedesInfix = {character = "&#x22DE;"; glyph = "⋞"; name="equal to or precedes"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let equalToOrSucceedsInfix = {character = "&#x22DF;"; glyph = "⋟"; name="equal to or succeeds"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let equiangularToInfix = {character = "&#x225A;"; glyph = "≚"; name="equiangular to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let equivalentToInfix = {character = "&#x224D;"; glyph = "≍"; name="equivalent to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let equivalentWithFourDotsAboveInfix = {character = "&#x2A78;"; glyph = "⩸"; name="equivalent with four dots above"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let errorBarredBlackCircleInfix = {character = "&#x29F3;"; glyph = "⧳"; name="error-barred black circle"; form="infix"; priority="260"; lspace="3"; rspace="3"; properties=[]}
let errorBarredBlackDiamondInfix = {character = "&#x29F1;"; glyph = "⧱"; name="error-barred black diamond"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let errorBarredBlackSquareInfix = {character = "&#x29EF;"; glyph = "⧯"; name="error-barred black square"; form="infix"; priority="270"; lspace="3"; rspace="3"; properties=[]}
let errorBarredWhiteCircleInfix = {character = "&#x29F2;"; glyph = "⧲"; name="error-barred white circle"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let errorBarredWhiteDiamondInfix = {character = "&#x29F0;"; glyph = "⧰"; name="error-barred white diamond"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let errorBarredWhiteSquareInfix = {character = "&#x29EE;"; glyph = "⧮"; name="error-barred white square"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let estimatesInfix = {character = "&#x2259;"; glyph = "≙"; name="estimates"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let excessInfix = {character = "&#x2239;"; glyph = "∹"; name="excess"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let exclamationMarkPostfix = {character = "!"; glyph = "!"; name="exclamation mark"; form="postfix"; priority="810"; lspace="1"; rspace="0"; properties=[]}
let fallingDiagonalCrossingNorthEastArrowInfix = {character = "&#x292F;"; glyph = "⤯"; name="falling diagonal crossing north east arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let fallingDiagonalCrossingRisingDiagonalInfix = {character = "&#x292C;"; glyph = "⤬"; name="falling diagonal crossing rising diagonal"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let feminineOrdinalIndicatorPostfix = {character = "&#xAA;"; glyph = "ª"; name="feminine ordinal indicator"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let finitePartIntegralPrefix = {character = "&#x2A0D;"; glyph = "⨍"; name="finite part integral"; form="prefix"; priority="310"; lspace="1"; rspace="2"; properties=["largeop"; "symmetric"]}
let fisheyeInfix = {character = "&#x25C9;"; glyph = "◉"; name="fisheye"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let forAllPrefix = {character = "&#x2200;"; glyph = "∀"; name="for all"; form="prefix"; priority="230"; lspace="2"; rspace="1"; properties=[]}
let forcesInfix = {character = "&#x22A9;"; glyph = "⊩"; name="forces"; form="infix"; priority="170"; lspace="5"; rspace="5"; properties=[]}
let fourthRootPrefix = {character = "&#x221C;"; glyph = "∜"; name="fourth root"; form="prefix"; priority="845"; lspace="1"; rspace="1"; properties=[]}
let fractionSlashInfix = {character = "&#x2044;"; glyph = "⁄"; name="fraction slash"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=["stretchy"]}
let fullStopInfix = {character = "."; glyph = "."; name="full stop"; form="infix"; priority="390"; lspace="3"; rspace="3"; properties=[]}
let functionApplicationInfix = {character = "&#x2061;"; glyph = "⁡"; name="function application"; form="infix"; priority="850"; lspace="0"; rspace="0"; properties=[]}
let geometricallyEqualToInfix = {character = "&#x2251;"; glyph = "≑"; name="geometrically equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let geometricallyEquivalentToInfix = {character = "&#x224E;"; glyph = "≎"; name="geometrically equivalent to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let geometricallyEquivalentToWithSlashInfix = {character = "&#x224E;&#x338;"; glyph = "≎̸"; name="geometrically equivalent to with slash"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let geometricProportionInfix = {character = "&#x223A;"; glyph = "∺"; name="geometric proportion"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let gleichStarkInfix = {character = "&#x29E6;"; glyph = "⧦"; name="gleich stark"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let graveAccentPostfix = {character = "`"; glyph = "`"; name="grave accent"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let greaterThanAboveDoubleLineEqualAboveLessThanInfix = {character = "&#x2A8C;"; glyph = "⪌"; name="greater-than above double-line equal above less-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let greaterThanAboveLessThanAboveDoubleLineEqualInfix = {character = "&#x2A92;"; glyph = "⪒"; name="greater-than above less-than above double-line equal"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let greaterThanAboveRightwardsArrowInfix = {character = "&#x2978;"; glyph = "⥸"; name="greater-than above rightwards arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let greaterThanAboveSimilarAboveLessThanInfix = {character = "&#x2A90;"; glyph = "⪐"; name="greater-than above similar above less-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let greaterThanAboveSimilarOrEqualInfix = {character = "&#x2A8E;"; glyph = "⪎"; name="greater-than above similar or equal"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let greaterThanAboveSlantedEqualAboveLessThanAboveSlantedEqualInfix = {character = "&#x2A94;"; glyph = "⪔"; name="greater-than above slanted equal above less-than above slanted equal"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let greaterThanAndNotApproximateInfix = {character = "&#x2A8A;"; glyph = "⪊"; name="greater-than and not approximate"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let greaterThanAndSingleLineNotEqualToInfix = {character = "&#x2A88;"; glyph = "⪈"; name="greater-than and single-line not equal to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let greaterThanBesideLessThanInfix = {character = "&#x2AA5;"; glyph = "⪥"; name="greater-than beside less-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let greaterThanButNotEqualToInfix = {character = "&#x2269;"; glyph = "≩"; name="greater-than but not equal to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let greaterThanButNotEquivalentToInfix = {character = "&#x22E7;"; glyph = "⋧"; name="greater-than but not equivalent to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let greaterThanClosedByCurveAboveSlantedEqualInfix = {character = "&#x2AA9;"; glyph = "⪩"; name="greater-than closed by curve above slanted equal"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let greaterThanClosedByCurveInfix = {character = "&#x2AA7;"; glyph = "⪧"; name="greater-than closed by curve"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let greaterThanEqualToOrLessThanInfix = {character = "&#x22DB;"; glyph = "⋛"; name="greater-than equal to or less-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let greaterThanOrApproximateInfix = {character = "&#x2A86;"; glyph = "⪆"; name="greater-than or approximate"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let greaterThanOrEquivalentToInfix = {character = "&#x2273;"; glyph = "≳"; name="greater-than or equivalent to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let greaterThanOrLessThanInfix = {character = "&#x2277;"; glyph = "≷"; name="greater-than or less-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let greaterThanOrSlantedEqualToInfix = {character = "&#x2A7E;"; glyph = "⩾"; name="greater-than or slanted equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let greaterThanOrSlantedEqualToWithDotAboveInfix = {character = "&#x2A82;"; glyph = "⪂"; name="greater-than or slanted equal to with dot above"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let greaterThanOrSlantedEqualToWithDotAboveLeftInfix = {character = "&#x2A84;"; glyph = "⪄"; name="greater-than or slanted equal to with dot above left"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let greaterThanOrSlantedEqualToWithDotInsideInfix = {character = "&#x2A80;"; glyph = "⪀"; name="greater-than or slanted equal to with dot inside"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let greaterThanOrSlantedEqualToWithSlashInfix = {character = "&#x2A7E;&#x338;"; glyph = "⩾̸"; name="greater-than or slanted equal to with slash"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let greaterThanOverEqualToInfix = {character = "&#x2267;"; glyph = "≧"; name="greater-than over equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let greaterThanOverlappingLessThanInfix = {character = "&#x2AA4;"; glyph = "⪤"; name="greater-than overlapping less-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let greaterThanSignInfix = {character = ">"; glyph = ">"; name="greater-than sign"; form="infix"; priority="243"; lspace="5"; rspace="5"; properties=[]}
let greaterThanWithCircleInsideInfix = {character = "&#x2A7A;"; glyph = "⩺"; name="greater-than with circle inside"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let greaterThanWithDotInfix = {character = "&#x22D7;"; glyph = "⋗"; name="greater-than with dot"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let greaterThanWithQuestionMarkAboveInfix = {character = "&#x2A7C;"; glyph = "⩼"; name="greater-than with question mark above"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let greateThanOrEqualToInfix = {character = "&#x2265;"; glyph = "≥"; name="greater-than or equal to"; form="infix"; priority="242"; lspace="5"; rspace="5"; properties=[]}
let greekReversedLunateEpsilonSymbolInfix = {character = "&#x3F6;"; glyph = "϶"; name="greek reversed lunate epsilon symbol"; form="infix"; priority="110"; lspace="5"; rspace="5"; properties=[]}
let hermitianConjugateMatrixInfix = {character = "&#x22B9;"; glyph = "⊹"; name="hermitian conjugate matrix"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let homotheticInfix = {character = "&#x223B;"; glyph = "∻"; name="homothetic"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let horizontalEllipsisInfix = {character = "&#x2026;"; glyph = "…"; name="horizontal ellipsis"; form="infix"; priority="150"; lspace="0"; rspace="0"; properties=[]}
let hyphenBulletInfix = {character = "&#x2043;"; glyph = "⁃"; name="hyphen bullet"; form="infix"; priority="390"; lspace="4"; rspace="4"; properties=[]}
let hyphenMinusInfix = {character = "-"; glyph = "-"; name="hyphen-minus"; form="infix"; priority="275"; lspace="4"; rspace="4"; properties=[]}
let hyphenMinusPrefix = {character = "-"; glyph = "-"; name="hyphen-minus"; form="prefix"; priority="275"; lspace="0"; rspace="1"; properties=[]}
let identicalToAndSlantedParallelInfix = {character = "&#x29E5;"; glyph = "⧥"; name="identical to and slanted parallel"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let identicalToInfix = {character = "&#x2261;"; glyph = "≡"; name="identical to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let identicalWithDotAboveInfix = {character = "&#x2A67;"; glyph = "⩧"; name="identical with dot above"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let imageOfInfix = {character = "&#x22B7;"; glyph = "⊷"; name="image of"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let imageOfOrApproximatelyEqualToInfix = {character = "&#x2253;"; glyph = "≓"; name="image of or approximately equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let incompleteInfinityInfix = {character = "&#x29DC;"; glyph = "⧜"; name="incomplete infinity"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let increasesAsInfix = {character = "&#x29E1;"; glyph = "⧡"; name="increases as"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let incrementInfix = {character = "&#x2206;"; glyph = "∆"; name="increment"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let infinityNegatedWithVerticalBarInfix = {character = "&#x29DE;"; glyph = "⧞"; name="infinity negated with vertical bar"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let integralAroundAPointOperatorPrefix = {character = "&#x2A15;"; glyph = "⨕"; name="integral around a point operator"; form="prefix"; priority="310"; lspace="1"; rspace="2"; properties=["largeop"; "symmetric"]}
let integralAverageWithSlashPrefix = {character = "&#x2A0F;"; glyph = "⨏"; name="integral average with slash"; form="prefix"; priority="310"; lspace="1"; rspace="2"; properties=["largeop"; "symmetric"]}
let integralPrefix = {character = "&#x222B;"; glyph = "∫"; name="integral"; form="prefix"; priority="310"; lspace="0"; rspace="1"; properties=["largeop"; "symmetric"]}
let integralWithDoubleStrokePrefix = {character = "&#x2A0E;"; glyph = "⨎"; name="integral with double stroke"; form="prefix"; priority="310"; lspace="1"; rspace="2"; properties=["largeop"; "symmetric"]}
let integralWithIntersectionPrefix = {character = "&#x2A19;"; glyph = "⨙"; name="integral with intersection"; form="prefix"; priority="310"; lspace="1"; rspace="2"; properties=["largeop"; "symmetric"]}
let integralWithLeftwardsArrowWithHookPrefix = {character = "&#x2A17;"; glyph = "⨗"; name="integral with leftwards arrow with hook"; form="prefix"; priority="310"; lspace="1"; rspace="2"; properties=["largeop"; "symmetric"]}
let integralWithOverbarPrefix = {character = "&#x2A1B;"; glyph = "⨛"; name="integral with overbar"; form="prefix"; priority="310"; lspace="1"; rspace="2"; properties=["largeop"; "symmetric"]}
let integralWithTimesSignPrefix = {character = "&#x2A18;"; glyph = "⨘"; name="integral with times sign"; form="prefix"; priority="310"; lspace="1"; rspace="2"; properties=["largeop"; "symmetric"]}
let integralWithUnderbarPrefix = {character = "&#x2A1C;"; glyph = "⨜"; name="integral with underbar"; form="prefix"; priority="310"; lspace="1"; rspace="2"; properties=["largeop"; "symmetric"]}
let integralWithUnionPrefix = {character = "&#x2A1A;"; glyph = "⨚"; name="integral with union"; form="prefix"; priority="310"; lspace="1"; rspace="2"; properties=["largeop"; "symmetric"]}
let intercalateInfix = {character = "&#x22BA;"; glyph = "⊺"; name="intercalate"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let interiorProductInfix = {character = "&#x2A3C;"; glyph = "⨼"; name="interior product"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let intersectionAboveBarAboveUnionInfix = {character = "&#x2A49;"; glyph = "⩉"; name="intersection above bar above union"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let intersectionAboveUnionInfix = {character = "&#x2A47;"; glyph = "⩇"; name="intersection above union"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let intersectionBesideAndJoinedWithIntersectionInfix = {character = "&#x2A4B;"; glyph = "⩋"; name="intersection beside and joined with intersection"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let intersectionInfix = {character = "&#x2229;"; glyph = "∩"; name="intersection"; form="infix"; priority="350"; lspace="4"; rspace="4"; properties=[]}
let intersectionWithDotInfix = {character = "&#x2A40;"; glyph = "⩀"; name="intersection with dot"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let intersectionWithLogicalAndInfix = {character = "&#x2A44;"; glyph = "⩄"; name="intersection with logical and"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let intersectionWithOverbarInfix = {character = "&#x2A43;"; glyph = "⩃"; name="intersection with overbar"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let invertedLazySInfix = {character = "&#x223E;"; glyph = "∾"; name="inverted lazy s"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let invisiblePlusInfix = {character = "&#x2064;"; glyph = "⁤"; name="invisible plus"; form="infix"; priority="880"; lspace="0"; rspace="0"; properties=[]}
let invisibleSeparatorInfix = {character = "&#x2063;"; glyph = "⁣"; name="invisible separator"; form="infix"; priority="40"; lspace="0"; rspace="0"; properties=["separator"; "linebreakstyle=after"]}
let invisibleTimesInfix = {character = "&#x2062;"; glyph = "⁢"; name="invisible times"; form="infix"; priority="390"; lspace="0"; rspace="0"; properties=[]}
let joinInfix = {character = "&#x2A1D;"; glyph = "⨝"; name="join"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let largeLeftTriangleOperatorInfix = {character = "&#x2A1E;"; glyph = "⨞"; name="large left triangle operator"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let largerThanInfix = {character = "&#x2AAB;"; glyph = "⪫"; name="larger than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let largerThanOrEqualToInfix = {character = "&#x2AAD;"; glyph = "⪭"; name="larger than or equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let largeTripleVerticalBarOperatorPrefix = {character = "&#x2AFC;"; glyph = "⫼"; name="large triple vertical bar operator"; form="prefix"; priority="330"; lspace="1"; rspace="2"; properties=["largeop"; "movablelimits"; "symmetric"]}
let leftAngleBracketWithDotPrefix = {character = "&#x2991;"; glyph = "⦑"; name="left angle bracket with dot"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let leftArcLessThanBracketPrefix = {character = "&#x2993;"; glyph = "⦓"; name="left arc less-than bracket"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let leftBarbDownRightBarbDownHarpoonInfix = {character = "&#x2950;"; glyph = "⥐"; name="left barb down right barb down harpoon"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftBarbDownRightBarbUpHarpoonInfix = {character = "&#x294B;"; glyph = "⥋"; name="left barb down right barb up harpoon"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftBarbUpRightBarbDownHarpoonInfix = {character = "&#x294A;"; glyph = "⥊"; name="left barb up right barb down harpoon"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftBarbUpRightBarbUpHarpoonInfix = {character = "&#x294E;"; glyph = "⥎"; name="left barb up right barb up harpoon"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftBlackTortoiseShellBracketPrefix = {character = "&#x2997;"; glyph = "⦗"; name="left black tortoise shell bracket"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let leftCeilingPrefix = {character = "&#x2308;"; glyph = "⌈"; name="left ceiling"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let leftCurlyBracketPrefix = {character = "{"; glyph = "{"; name="left curly bracket"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let leftDoubleQuotationMarkPrefix = {character = "&#x201C;"; glyph = "“"; name="left double quotation mark"; form="prefix"; priority="10"; lspace="0"; rspace="0"; properties=["fence"]}
let leftFishTailInfix = {character = "&#x297C;"; glyph = "⥼"; name="left fish tail"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftFloorPrefix = {character = "&#x230A;"; glyph = "⌊"; name="left floor"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let leftHalfBlackCircleInfix = {character = "&#x25D6;"; glyph = "◖"; name="left half black circle"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let leftNormalFactorSemidirectProductInfix = {character = "&#x22C9;"; glyph = "⋉"; name="left normal factor semidirect product"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let leftParenthesisPrefix = {character = "("; glyph = "("; name="left parenthesis"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let leftPointingAngleBracketPrefix = {character = "&#x2329;"; glyph = "〈"; name="left-pointing angle bracket"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let leftPointingCurvedAngleBracketPrefix = {character = "&#x29FC;"; glyph = "⧼"; name="left-pointing curved angle bracket"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let leftRightArrowInfix = {character = "&#x2194;"; glyph = "↔"; name="left right arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftRightArrowThroughSmallCircleInfix = {character = "&#x2948;"; glyph = "⥈"; name="left right arrow through small circle"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftRightArrowWithDoubleVerticalStrokeInfix = {character = "&#x21FC;"; glyph = "⇼"; name="left right arrow with double vertical stroke"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftRightArrowWithStrokeInfix = {character = "&#x21AE;"; glyph = "↮"; name="left right arrow with stroke"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftRightArrowWithVerticalStrokeInfix = {character = "&#x21F9;"; glyph = "⇹"; name="left right arrow with vertical stroke"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftRightDoubleArrowInfix = {character = "&#x21D4;"; glyph = "⇔"; name="left right double arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftRightDoubleArrowWithStrokeInfix = {character = "&#x21CE;"; glyph = "⇎"; name="left right double arrow with stroke"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftRightDoubleArrowWithVerticalStrokeInfix = {character = "&#x2904;"; glyph = "⤄"; name="left right double arrow with vertical stroke"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftRightOpenHeadedArrowInfix = {character = "&#x21FF;"; glyph = "⇿"; name="left right open-headed arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftRightWaveArrowInfix = {character = "&#x21AD;"; glyph = "↭"; name="left right wave arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftSemidirectProductInfix = {character = "&#x22CB;"; glyph = "⋋"; name="left semidirect product"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let leftSideArcAnticlockwiseArrowInfix = {character = "&#x2939;"; glyph = "⤹"; name="left-side arc anticlockwise arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let leftSingleQuotationMarkPrefix = {character = "&#x2018;"; glyph = "‘"; name="left single quotation mark"; form="prefix"; priority="10"; lspace="0"; rspace="0"; properties=["fence"]}
let leftSquareBracketPrefix = {character = "["; glyph = "["; name="left square bracket"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let leftSquareBracketWithTickInBottomCornerPrefix = {character = "&#x298F;"; glyph = "⦏"; name="left square bracket with tick in bottom corner"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let leftSquareBracketWithTickInTopCornerPrefix = {character = "&#x298D;"; glyph = "⦍"; name="left square bracket with tick in top corner"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let leftSquareBracketWithUnderbarPrefix = {character = "&#x298B;"; glyph = "⦋"; name="left square bracket with underbar"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let leftTackInfix = {character = "&#x22A3;"; glyph = "⊣"; name="left tack"; form="infix"; priority="170"; lspace="5"; rspace="5"; properties=[]}
let leftTriangleBesideVerticalBarInfix = {character = "&#x29CF;"; glyph = "⧏"; name="left triangle beside vertical bar"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let leftTriangleBesideVerticalBarWithSlashInfix = {character = "&#x29CF;&#x338;"; glyph = "⧏̸"; name="left triangle beside vertical bar with slash"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let leftwardsArrowAboveShortRightwardsArrowInfix = {character = "&#x2943;"; glyph = "⥃"; name="leftwards arrow above short rightwards arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftwardsArrowAboveTildeOperatorInfix = {character = "&#x2973;"; glyph = "⥳"; name="leftwards arrow above tilde operator"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftwardsArrowFromBarInfix = {character = "&#x21A4;"; glyph = "↤"; name="leftwards arrow from bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftwardsArrowFromBarToBlackDiamondInfix = {character = "&#x291F;"; glyph = "⤟"; name="leftwards arrow from bar to black diamond"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftwardsArrowInfix = {character = "&#x2190;"; glyph = "←"; name="leftwards arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftwardsArrowOverRightwardsArrowInfix = {character = "&#x21C6;"; glyph = "⇆"; name="leftwards arrow over rightwards arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftwardsArrowTailInfix = {character = "&#x2919;"; glyph = "⤙"; name="leftwards arrow-tail"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftwardsArrowThroughLessThanInfix = {character = "&#x2977;"; glyph = "⥷"; name="leftwards arrow through less-than"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftwardsArrowThroughSubsetInfix = {character = "&#x297A;"; glyph = "⥺"; name="leftwards arrow through subset"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftwardsArrowToBarInfix = {character = "&#x21E4;"; glyph = "⇤"; name="leftwards arrow to bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftwardsArrowToBarOverRightwardsArrowToBarInfix = {character = "&#x21B9;"; glyph = "↹"; name="leftwards arrow to bar over rightwards arrow to bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftwardsArrowToBlackDiamondInfix = {character = "&#x291D;"; glyph = "⤝"; name="leftwards arrow to black diamond"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftwardsArrowWithDoubleVerticalStrokeInfix = {character = "&#x21FA;"; glyph = "⇺"; name="leftwards arrow with double vertical stroke"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftwardsArrowWithHookInfix = {character = "&#x21A9;"; glyph = "↩"; name="leftwards arrow with hook"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftwardsArrowWithLoopInfix = {character = "&#x21AB;"; glyph = "↫"; name="leftwards arrow with loop"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftwardsArrowWithPlusBelowInfix = {character = "&#x2946;"; glyph = "⥆"; name="leftwards arrow with plus below"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftwardsArrowWithStrokeInfix = {character = "&#x219A;"; glyph = "↚"; name="leftwards arrow with stroke"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftwardsArrowWithTailInfix = {character = "&#x21A2;"; glyph = "↢"; name="leftwards arrow with tail"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftwardsArrowWithVerticalStrokeInfix = {character = "&#x21F7;"; glyph = "⇷"; name="leftwards arrow with vertical stroke"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftwardsDashedArrowInfix = {character = "&#x21E0;"; glyph = "⇠"; name="leftwards dashed arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftwardsDoubleArrowFromBarInfix = {character = "&#x2906;"; glyph = "⤆"; name="leftwards double arrow from bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftwardsDoubleArrowInfix = {character = "&#x21D0;"; glyph = "⇐"; name="leftwards double arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftwardsDoubleArrowTailInfix = {character = "&#x291B;"; glyph = "⤛"; name="leftwards double arrow-tail"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftwardsDoubleArrowWithStrokeInfix = {character = "&#x21CD;"; glyph = "⇍"; name="leftwards double arrow with stroke"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftwardsDoubleArrowWithVerticalStrokeInfix = {character = "&#x2902;"; glyph = "⤂"; name="leftwards double arrow with vertical stroke"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftwardsDoubleDashArrowInfix = {character = "&#x290C;"; glyph = "⤌"; name="leftwards double dash arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftwardsHarpoonOverRightwardsHarpoonInfix = {character = "&#x21CB;"; glyph = "⇋"; name="leftwards harpoon over rightwards harpoon"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftwardsHarpoonWithBarbDownAboveRightwardsHarpoonWithBarbDownInfix = {character = "&#x2967;"; glyph = "⥧"; name="leftwards harpoon with barb down above rightwards harpoon with barb down"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftwardsHarpoonWithBarbDownBelowLongDashInfix = {character = "&#x296B;"; glyph = "⥫"; name="leftwards harpoon with barb down below long dash"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftwardsHarpoonWithBarbDownFromBarInfix = {character = "&#x295E;"; glyph = "⥞"; name="leftwards harpoon with barb down from bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftwardsHarpoonWithBarbDownToBarInfix = {character = "&#x2956;"; glyph = "⥖"; name="leftwards harpoon with barb down to bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let leftwardsHarpoonWithBarbDownwardsInfix = {character = "&#x21BD;"; glyph = "↽"; name="leftwards harpoon with barb downwards"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftwardsHarpoonWithBarbUpAboveLeftwardsHarpoonWithBarbDownInfix = {character = "&#x2962;"; glyph = "⥢"; name="leftwards harpoon with barb up above leftwards harpoon with barb down"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftwardsHarpoonWithBarbUpAboveLongDashInfix = {character = "&#x296A;"; glyph = "⥪"; name="leftwards harpoon with barb up above long dash"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftwardsHarpoonWithBarbUpAboveRightwardsHarpoonWithBarbUpInfix = {character = "&#x2966;"; glyph = "⥦"; name="leftwards harpoon with barb up above rightwards harpoon with barb up"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let leftwardsHarpoonWithBarbUpFromBarInfix = {character = "&#x295A;"; glyph = "⥚"; name="leftwards harpoon with barb up from bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftwardsHarpoonWithBarbUpToBarInfix = {character = "&#x2952;"; glyph = "⥒"; name="leftwards harpoon with barb up to bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftwardsHarpoonWithBarbUpwardsInfix = {character = "&#x21BC;"; glyph = "↼"; name="leftwards harpoon with barb upwards"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftwardsOpenHeadedArrowInfix = {character = "&#x21FD;"; glyph = "⇽"; name="leftwards open-headed arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftwardsPairedArrowsInfix = {character = "&#x21C7;"; glyph = "⇇"; name="leftwards paired arrows"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftwardsQuadrupleArrowInfix = {character = "&#x2B45;"; glyph = "⭅"; name="leftwards quadruple arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let leftwardsSquiggleArrowInfix = {character = "&#x21DC;"; glyph = "⇜"; name="leftwards squiggle arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftwardsTripleArrowInfix = {character = "&#x21DA;"; glyph = "⇚"; name="leftwards triple arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftwardsTripleDashArrowInfix = {character = "&#x290E;"; glyph = "⤎"; name="leftwards triple dash arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftwardsTwoHeadedArrowInfix = {character = "&#x219E;"; glyph = "↞"; name="leftwards two headed arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftwardsWaveArrowInfix = {character = "&#x219C;"; glyph = "↜"; name="leftwards wave arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftwardsWhiteArrowInfix = {character = "&#x21E6;"; glyph = "⇦"; name="leftwards white arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let leftWhiteCurlyBracketPrefix = {character = "&#x2983;"; glyph = "⦃"; name="left white curly bracket"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let leftWhiteParenthesisPrefix = {character = "&#x2985;"; glyph = "⦅"; name="left white parenthesis"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let leftWigglyFenceInfix = {character = "&#x29D8;"; glyph = "⧘"; name="left wiggly fence"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let lessThanAboveDoubleLineEqualAboveGreaterThanInfix = {character = "&#x2A8B;"; glyph = "⪋"; name="less-than above double-line equal above greater-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let lessThanAboveGreaterThanAboveDoubleLineEqualInfix = {character = "&#x2A91;"; glyph = "⪑"; name="less-than above greater-than above double-line equal"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let lessThanAboveLeftwardsArrowInfix = {character = "&#x2976;"; glyph = "⥶"; name="less-than above leftwards arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let lessThanAboveSimilarAboveGreaterThanInfix = {character = "&#x2A8F;"; glyph = "⪏"; name="less-than above similar above greater-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let lessThanAboveSimilarOrEqualInfix = {character = "&#x2A8D;"; glyph = "⪍"; name="less-than above similar or equal"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let lessThanAboveSlantedEqualAboveGreaterThanAboveSlantedEqualInfix = {character = "&#x2A93;"; glyph = "⪓"; name="less-than above slanted equal above greater-than above slanted equal"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let lessThanAndNotApproximateInfix = {character = "&#x2A89;"; glyph = "⪉"; name="less-than and not approximate"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let lessThanAndSingleLineNotEqualToInfix = {character = "&#x2A87;"; glyph = "⪇"; name="less-than and single-line not equal to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let lessThanButNotEqualToInfix = {character = "&#x2268;"; glyph = "≨"; name="less-than but not equal to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let lessThanButNotEquivalentToInfix = {character = "&#x22E6;"; glyph = "⋦"; name="less-than but not equivalent to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let lessThanClosedByCurveAboveSlantedEqualInfix = {character = "&#x2AA8;"; glyph = "⪨"; name="less-than closed by curve above slanted equal"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let lessThanClosedByCurveInfix = {character = "&#x2AA6;"; glyph = "⪦"; name="less-than closed by curve"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let lessThanEqualToOrGreaterThanInfix = {character = "&#x22DA;"; glyph = "⋚"; name="less-than equal to or greater-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let lessThanOrApproximateInfix = {character = "&#x2A85;"; glyph = "⪅"; name="less-than or approximate"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let lessThanOrEqualToInfix = {character = "&#x2264;"; glyph = "≤"; name="less-than or equal to"; form="infix"; priority="241"; lspace="5"; rspace="5"; properties=[]}
let lessThanOrEquivalentToInfix = {character = "&#x2272;"; glyph = "≲"; name="less-than or equivalent to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let lessThanOrGreaterThanInfix = {character = "&#x2276;"; glyph = "≶"; name="less-than or greater-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let lessThanOrSlantedEqualToInfix = {character = "&#x2A7D;"; glyph = "⩽"; name="less-than or slanted equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let lessThanOrSlantedEqualToWithDotAboveInfix = {character = "&#x2A81;"; glyph = "⪁"; name="less-than or slanted equal to with dot above"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let lessThanOrSlantedEqualToWithDotAboveRightInfix = {character = "&#x2A83;"; glyph = "⪃"; name="less-than or slanted equal to with dot above right"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let lessThanOrSlantedEqualToWithDotInsideInfix = {character = "&#x2A7F;"; glyph = "⩿"; name="less-than or slanted equal to with dot inside"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let lessThanOrSlantedEqualToWithSlashInfix = {character = "&#x2A7D;&#x338;"; glyph = "⩽̸"; name="less-than or slanted equal to with slash"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let lessThanOverEqualToInfix = {character = "&#x2266;"; glyph = "≦"; name="less-than over equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let lessThanOverEqualToWithSlashInfix = {character = "&#x2266;&#x338;"; glyph = "≦̸"; name="less-than over equal to with slash"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let lessThanSignInfix = {character = "&lt;"; glyph = "<"; name="less-than sign"; form="infix"; priority="245"; lspace="5"; rspace="5"; properties=[]}
let lessThanWithCircleInsideInfix = {character = "&#x2A79;"; glyph = "⩹"; name="less-than with circle inside"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let lessThanWithDotInfix = {character = "&#x22D6;"; glyph = "⋖"; name="less-than with dot"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let lessThanWithQuestionMarkAboveInfix = {character = "&#x2A7B;"; glyph = "⩻"; name="less-than with question mark above"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let lightLeftTortoiseShellBracketOrnamentPrefix = {character = "&#x2772;"; glyph = "❲"; name="light left tortoise shell bracket ornament"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let lightRightTortoiseShellBracketOrnamentPostfix = {character = "&#x2773;"; glyph = "❳"; name="light right tortoise shell bracket ornament"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let lightVerticalBarInfix = {character = "&#x2758;"; glyph = "❘"; name="light vertical bar"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let lineIntegrationNotIncludingThePolePrefix = {character = "&#x2A14;"; glyph = "⨔"; name="line integration not including the pole"; form="prefix"; priority="310"; lspace="1"; rspace="2"; properties=["largeop"; "movablelimits"; "symmetric"]}
let lineIntegrationWithRectangularPathAroundPolePrefix = {character = "&#x2A12;"; glyph = "⨒"; name="line integration with rectangular path around pole"; form="prefix"; priority="310"; lspace="1"; rspace="2"; properties=["largeop"; "movablelimits"; "symmetric"]}
let lineIntegrationWithSemicircularPathAroundPolePrefix = {character = "&#x2A13;"; glyph = "⨓"; name="line integration with semicircular path around pole"; form="prefix"; priority="310"; lspace="1"; rspace="2"; properties=["largeop"; "movablelimits"; "symmetric"]}
let logicalAndInfix = {character = "&#x2227;"; glyph = "∧"; name="logical and"; form="infix"; priority="200"; lspace="4"; rspace="4"; properties=[]}
let logicalAndWithDotAboveInfix = {character = "&#x2A51;"; glyph = "⩑"; name="logical and with dot above"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let logicalAndWithDoubleOverbarInfix = {character = "&#x2A5E;"; glyph = "⩞"; name="logical and with double overbar"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let logicalAndWithDoubleUnderbarInfix = {character = "&#x2A60;"; glyph = "⩠"; name="logical and with double underbar"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let logicalAndWithHorizontalDashInfix = {character = "&#x2A5C;"; glyph = "⩜"; name="logical and with horizontal dash"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let logicalAndWithMiddleStemInfix = {character = "&#x2A5A;"; glyph = "⩚"; name="logical and with middle stem"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let logicalAndWithUnderbarInfix = {character = "&#x2A5F;"; glyph = "⩟"; name="logical and with underbar"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let logicalOrInfix = {character = "&#x2228;"; glyph = "∨"; name="logical or"; form="infix"; priority="190"; lspace="4"; rspace="4"; properties=[]}
let logicalOrOverlappingLogicalAndInfix = {character = "&#x2A59;"; glyph = "⩙"; name="logical or overlapping logical and"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let logicalOrWithDotAboveInfix = {character = "&#x2A52;"; glyph = "⩒"; name="logical or with dot above"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let logicalOrWithDoubleOverbarInfix = {character = "&#x2A62;"; glyph = "⩢"; name="logical or with double overbar"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let logicalOrWithDoubleUnderbarInfix = {character = "&#x2A63;"; glyph = "⩣"; name="logical or with double underbar"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let logicalOrWithHorizontalDashInfix = {character = "&#x2A5D;"; glyph = "⩝"; name="logical or with horizontal dash"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let logicalOrWithMiddleStemInfix = {character = "&#x2A5B;"; glyph = "⩛"; name="logical or with middle stem"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let longDashFromLeftMemberOfDoubleVerticalInfix = {character = "&#x2AE6;"; glyph = "⫦"; name="long dash from left member of double vertical"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let longLeftRightArrowInfix = {character = "&#x27F7;"; glyph = "⟷"; name="long left right arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let longLeftRightDoubleArrowInfix = {character = "&#x27FA;"; glyph = "⟺"; name="long left right double arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let longLeftwardsArrowFromBarInfix = {character = "&#x27FB;"; glyph = "⟻"; name="long leftwards arrow from bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let longLeftwardsArrowInfix = {character = "&#x27F5;"; glyph = "⟵"; name="long leftwards arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let longLeftwardsDoubleArrowFromBarInfix = {character = "&#x27FD;"; glyph = "⟽"; name="long leftwards double arrow from bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let longLeftwardsDoubleArrowInfix = {character = "&#x27F8;"; glyph = "⟸"; name="long leftwards double arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let longRightwardsArrowFromBarInfix = {character = "&#x27FC;"; glyph = "⟼"; name="long rightwards arrow from bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let longRightwardsArrowInfix = {character = "&#x27F6;"; glyph = "⟶"; name="long rightwards arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let longRightwardsDoubleArrowFromBarInfix = {character = "&#x27FE;"; glyph = "⟾"; name="long rightwards double arrow from bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let longRightwardsDoubleArrowInfix = {character = "&#x27F9;"; glyph = "⟹"; name="long rightwards double arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let longRightwardsSquiggleArrowInfix = {character = "&#x27FF;"; glyph = "⟿"; name="long rightwards squiggle arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let lowerLeftSemicircularAnticlockwiseArrowInfix = {character = "&#x293F;"; glyph = "⤿"; name="lower left semicircular anticlockwise arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let lowerRightSemicircularClockwiseArrowInfix = {character = "&#x293E;"; glyph = "⤾"; name="lower right semicircular clockwise arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let lowLineInfix = {character = "_"; glyph = "_"; name="low line"; form="infix"; priority="900"; lspace="1"; rspace="1"; properties=[]}
let lowLinePostfix = {character = "_"; glyph = "_"; name="low line"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["stretchy"; "accent"]}
let macronPostfix = {character = "&#xAF;"; glyph = "¯"; name="macron"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["stretchy"; "accent"]}
let masculineOrdinalIndicatorPostfix = {character = "&#xBA;"; glyph = "º"; name="masculine ordinal indicator"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let mathematicalLeftAngleBracketPrefix = {character = "&#x27E8;"; glyph = "⟨"; name="mathematical left angle bracket"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let mathematicalLeftDoubleAngleBracketPrefix = {character = "&#x27EA;"; glyph = "⟪"; name="mathematical left double angle bracket"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let mathematicalLeftFlattenedParenthesisPrefix = {character = "&#x27EE;"; glyph = "⟮"; name="mathematical left flattened parenthesis"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let mathematicalLeftWhiteSquareBracketPrefix = {character = "&#x27E6;"; glyph = "⟦"; name="mathematical left white square bracket"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let mathematicalLeftWhiteTortoiseShellBracketPrefix = {character = "&#x27EC;"; glyph = "⟬"; name="mathematical left white tortoise shell bracket"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let mathematicalRightAngleBracketPostfix = {character = "&#x27E9;"; glyph = "⟩"; name="mathematical right angle bracket"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let mathematicalRightDoubleAngleBracketPostfix = {character = "&#x27EB;"; glyph = "⟫"; name="mathematical right double angle bracket"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let mathematicalRightFlattenedParenthesisPostfix = {character = "&#x27EF;"; glyph = "⟯"; name="mathematical right flattened parenthesis"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let mathematicalRightWhiteSquareBracketPostfix = {character = "&#x27E7;"; glyph = "⟧"; name="mathematical right white square bracket"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let mathematicalRightWhiteTortoiseShellBracketPostfix = {character = "&#x27ED;"; glyph = "⟭"; name="mathematical right white tortoise shell bracket"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let measuredAngleOpeningLeftInfix = {character = "&#x299B;"; glyph = "⦛"; name="measured angle opening left"; form="infix"; priority="270"; lspace="3"; rspace="3"; properties=[]}
let measuredAnglePrefix = {character = "&#x2221;"; glyph = "∡"; name="measured angle"; form="prefix"; priority="670"; lspace="0"; rspace="0"; properties=[]}
let measuredAngleWithOpenArmEndingInArrowPointingDownAndLeftInfix = {character = "&#x29AB;"; glyph = "⦫"; name="measured angle with open arm ending in arrow pointing down and left"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let measuredAngleWithOpenArmEndingInArrowPointingDownAndRightInfix = {character = "&#x29AA;"; glyph = "⦪"; name="measured angle with open arm ending in arrow pointing down and right"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let measuredAngleWithOpenArmEndingInArrowPointingLeftAndDownInfix = {character = "&#x29AF;"; glyph = "⦯"; name="measured angle with open arm ending in arrow pointing left and down"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let measuredAngleWithOpenArmEndingInArrowPointingLeftAndUpInfix = {character = "&#x29AD;"; glyph = "⦭"; name="measured angle with open arm ending in arrow pointing left and up"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let measuredAngleWithOpenArmEndingInArrowPointingRightAndDownInfix = {character = "&#x29AE;"; glyph = "⦮"; name="measured angle with open arm ending in arrow pointing right and down"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let measuredAngleWithOpenArmEndingInArrowPointingRightAndUpInfix = {character = "&#x29AC;"; glyph = "⦬"; name="measured angle with open arm ending in arrow pointing right and up"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let measuredAngleWithOpenArmEndingInArrowPointingUpAndLeftInfix = {character = "&#x29A9;"; glyph = "⦩"; name="measured angle with open arm ending in arrow pointing up and left"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let measuredAngleWithOpenArmEndingInArrowPointingUpAndRightInfix = {character = "&#x29A8;"; glyph = "⦨"; name="measured angle with open arm ending in arrow pointing up and right"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let measuredByInfix = {character = "&#x225E;"; glyph = "≞"; name="measured by"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let measuredRightAngleWithDotInfix = {character = "&#x299D;"; glyph = "⦝"; name="measured right angle with dot"; form="infix"; priority="270"; lspace="3"; rspace="3"; properties=[]}
let middleDotInfix = {character = "&#xB7;"; glyph = "·"; name="middle dot"; form="infix"; priority="400"; lspace="4"; rspace="4"; properties=[]}
let midlineHorizontalEllipsisInfix = {character = "&#x22EF;"; glyph = "⋯"; name="midline horizontal ellipsis"; form="infix"; priority="150"; lspace="0"; rspace="0"; properties=[]}
let minusOrPlusSignInfix = {character = "&#x2213;"; glyph = "∓"; name="minus-or-plus sign"; form="infix"; priority="275"; lspace="4"; rspace="4"; properties=[]}
let minusOrPlusSignPrefix = {character = "&#x2213;"; glyph = "∓"; name="minus-or-plus sign"; form="prefix"; priority="275"; lspace="0"; rspace="1"; properties=[]}
let minusSignInfix = {character = "&#x2212;"; glyph = "−"; name="minus sign"; form="infix"; priority="275"; lspace="4"; rspace="4"; properties=[]}
let minusSignInTriangleInfix = {character = "&#x2A3A;"; glyph = "⨺"; name="minus sign in triangle"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let minusSignPrefix = {character = "&#x2212;"; glyph = "−"; name="minus sign"; form="prefix"; priority="275"; lspace="0"; rspace="1"; properties=[]}
let minusSignWithCommaAboveInfix = {character = "&#x2A29;"; glyph = "⨩"; name="minus sign with comma above"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let minusSignWithDotBelowInfix = {character = "&#x2A2A;"; glyph = "⨪"; name="minus sign with dot below"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let minusSignWithFallingDotsInfix = {character = "&#x2A2B;"; glyph = "⨫"; name="minus sign with falling dots"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let minusSignWithRisingDotsInfix = {character = "&#x2A2C;"; glyph = "⨬"; name="minus sign with rising dots"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let minusTildeInfix = {character = "&#x2242;"; glyph = "≂"; name="minus tilde"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let minusTildeWithSlashInfix = {character = "&#x2242;&#x338;"; glyph = "≂̸"; name="minus tilde with slash"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let minyInfix = {character = "&#x29FF;"; glyph = "⧿"; name="miny"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let modelsInfix = {character = "&#x22A7;"; glyph = "⊧"; name="models"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let modifierLetterAcuteAccentPostfix = {character = "&#x2CA;"; glyph = "ˊ"; name="modifier letter acute accent"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let modifierLetterCircumflexAccentPostfix = {character = "&#x2C6;"; glyph = "ˆ"; name="modifier letter circumflex accent"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["stretchy"; "accent"]}
let modifierLetterGraveAccentPostfix = {character = "&#x2CB;"; glyph = "ˋ"; name="modifier letter grave accent"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let modifierLetterLowMacronPostfix = {character = "&#x2CD;"; glyph = "ˍ"; name="modifier letter low macron"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["stretchy"; "accent"]}
let modifierLetterLowTildePostfix = {character = "&#x2F7;"; glyph = "˷"; name="modifier letter low tilde"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["stretchy"; "accent"]}
let modifierLetterMacronPostfix = {character = "&#x2C9;"; glyph = "ˉ"; name="modifier letter macron"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["stretchy"; "accent"]}
let moduloTwoSumPrefix = {character = "&#x2A0A;"; glyph = "⨊"; name="modulo two sum"; form="prefix"; priority="290"; lspace="1"; rspace="2"; properties=["largeop"; "movablelimits"; "symmetric"]}
let muchGreaterThanInfix = {character = "&#x226B;"; glyph = "≫"; name="much greater-than"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let muchGreaterThanWithSlashInfix = {character = "&#x226B;&#x338;"; glyph = "≫̸"; name="much greater than with slash"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let muchLessThanInfix = {character = "&#x226A;"; glyph = "≪"; name="much less-than"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let muchLessThanWithSlashInfix = {character = "&#x226A;&#x338;"; glyph = "≪̸"; name="much less than with slash"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let multimapInfix = {character = "&#x22B8;"; glyph = "⊸"; name="multimap"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let multipleCharacterOperatorDoubleExclamationPostfix = {character = "!!"; glyph = "!!"; name="multiple character operator: !!"; form="postfix"; priority="810"; lspace="1"; rspace="0"; properties=[]}
let multipleCharacterOperatorDoubleForwardSlashInfix = {character = "//"; glyph = "//"; name="multiple character operator: //"; form="infix"; priority="820"; lspace="1"; rspace="1"; properties=[]}
let multipleCharacterOperatorDoubleMinusPostfix = {character = "--"; glyph = "--"; name="multiple character operator: --"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=[]}
let multipleCharacterOperatorDoublePlusPostfix = {character = "++"; glyph = "++"; name="multiple character operator: ++"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=[]}
let multipleCharacterOperatorDoubleVerticalLineInfix = {character = "||"; glyph = "||"; name="multiple character operator: ||"; form="infix"; priority="270"; lspace="2"; rspace="2"; properties=["fence"; "stretchy"; "symmetric"]}
let multipleCharacterOperatorEqualsEqualsInfix = {character = "=="; glyph = "=="; name="multiple character operator: =="; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let multipleCharacterOperatorExclamationEqualsInfix = {character = "!="; glyph = "!="; name="multiple character operator: !="; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let multipleCharacterOperatorForwardSlashEqualsInfix = {character = "/="; glyph = "/="; name="multiple character operator: /="; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let multipleCharacterOperatorGreaterThanOrEqualsInfix = {character = ">="; glyph = ">="; name="multiple character operator: >="; form="infix"; priority="243"; lspace="5"; rspace="5"; properties=[]}
let multipleCharacterOperatorLessThanEqualToInfix = {character = "&lt;="; glyph = "<="; name="multiple character operator: <="; form="infix"; priority="241"; lspace="5"; rspace="5"; properties=[]}
let multipleCharacterOperatorLessThanOrGreaterThanInfix = {character = "&lt;>"; glyph = "<>"; name="multiple character operator: <>"; form="infix"; priority="780"; lspace="1"; rspace="1"; properties=[]}
let multipleCharacterOperatorMinusEqualsInfix = {character = "-="; glyph = "-="; name="multiple character operator: -="; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let multipleCharacterOperatorMinusGreaterThanInfix = {character = "->"; glyph = "->"; name="multiple character operator: ->"; form="infix"; priority="90"; lspace="5"; rspace="5"; properties=[]}
let multipleCharacterOperatorPlusEqualsInfix = {character = "+="; glyph = "+="; name="multiple character operator: +="; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let multipleCharacterOperatorSemiColonEqualsInfix = {character = ":="; glyph = ":="; name="multiple character operator: :="; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let multipleCharacterOperatorStarEqualsInfix = {character = "*="; glyph = "*="; name="multiple character operator: *="; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let multipleCharacterOperatorThreeDotPostfix = {character = "..."; glyph = "..."; name="multiple character operator: ..."; form="postfix"; priority="100"; lspace="0"; rspace="0"; properties=[]}
let multipleCharacterOperatorTripleVerticalLineInfix = {character = "|||"; glyph = "|||"; name="multiple character operator: |||"; form="infix"; priority="270"; lspace="2"; rspace="2"; properties=["fence"; "stretchy"; "symmetric"]}
let multipleCharacterOperatorTwoAmpersandInfix = {character = "&amp;&amp;"; glyph = "&&"; name="multiple character operator: &&"; form="infix"; priority="200"; lspace="4"; rspace="4"; properties=[]}
let multipleCharacterOperatorTwoDotPostfix = {character = ".."; glyph = ".."; name="multiple character operator: .."; form="postfix"; priority="100"; lspace="0"; rspace="0"; properties=[]}
let multipleCharacterOperatorTwoStarInfix = {character = "**"; glyph = "**"; name="multiple character operator: **"; form="infix"; priority="780"; lspace="1"; rspace="1"; properties=[]}
let multipleCharacterThreeVerticalLineOperatorPostfix = {character = "|||"; glyph = "|||"; name="multiple character operator: |||"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let multipleCharacterThreeVerticalLineOperatorPrefix = {character = "|||"; glyph = "|||"; name="multiple character operator: |||"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let multipleCharacterTwoVerticalLineOperatorPostfix = {character = "||"; glyph = "||"; name="multiple character operator: ||"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let multipleCharacterTwoVerticalLineOperatorPrefix = {character = "||"; glyph = "||"; name="multiple character operator: ||"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let multiplicationSignInDoubleCircleInfix = {character = "&#x2A37;"; glyph = "⨷"; name="multiplication sign in double circle"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let multiplicationSignInfix = {character = "&#xD7;"; glyph = "×"; name="multiplication sign"; form="infix"; priority="390"; lspace="4"; rspace="4"; properties=[]}
let multiplicationSignInLeftHalfCircleInfix = {character = "&#x2A34;"; glyph = "⨴"; name="multiplication sign in left half circle"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let multiplicationSignInRightHalfCircleInfix = {character = "&#x2A35;"; glyph = "⨵"; name="multiplication sign in right half circle"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let multiplicationSignInTriangleInfix = {character = "&#x2A3B;"; glyph = "⨻"; name="multiplication sign in triangle"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let multiplicationSignWithDotAboveInfix = {character = "&#x2A30;"; glyph = "⨰"; name="multiplication sign with dot above"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let multiplicationSignWithUnderbarInfix = {character = "&#x2A31;"; glyph = "⨱"; name="multiplication sign with underbar"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let multisetInfix = {character = "&#x228C;"; glyph = "⊌"; name="multiset"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let multisetMultiplicationInfix = {character = "&#x228D;"; glyph = "⊍"; name="multiset multiplication"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let multisetUnionInfix = {character = "&#x228E;"; glyph = "⊎"; name="multiset union"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let musicFlatSignPostfix = {character = "&#x266D;"; glyph = "♭"; name="music flat sign"; form="postfix"; priority="800"; lspace="0"; rspace="2"; properties=[]}
let musicNaturalSignPostfix = {character = "&#x266E;"; glyph = "♮"; name="music natural sign"; form="postfix"; priority="800"; lspace="0"; rspace="2"; properties=[]}
let musicSharpSignPostfix = {character = "&#x266F;"; glyph = "♯"; name="music sharp sign"; form="postfix"; priority="800"; lspace="0"; rspace="2"; properties=[]}
let nablaPrefix = {character = "&#x2207;"; glyph = "∇"; name="nabla"; form="prefix"; priority="740"; lspace="2"; rspace="1"; properties=[]}
let nandInfix = {character = "&#x22BC;"; glyph = "⊼"; name="nand"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let nAryCircledDotOperatorPrefix = {character = "&#x2A00;"; glyph = "⨀"; name="n-ary circled dot operator"; form="prefix"; priority="330"; lspace="1"; rspace="2"; properties=["largeop"; "movablelimits"; "symmetric"]}
let nAryCircledPlusOperatorPrefix = {character = "&#x2A01;"; glyph = "⨁"; name="n-ary circled plus operator"; form="prefix"; priority="300"; lspace="1"; rspace="2"; properties=["largeop"; "movablelimits"; "symmetric"]}
let nAryCircledTimesOperatorPrefix = {character = "&#x2A02;"; glyph = "⨂"; name="n-ary circled times operator"; form="prefix"; priority="330"; lspace="1"; rspace="2"; properties=["largeop"; "movablelimits"; "symmetric"]}
let nAryCoproductPrefix = {character = "&#x2210;"; glyph = "∐"; name="n-ary coproduct"; form="prefix"; priority="350"; lspace="1"; rspace="2"; properties=["largeop"; "movablelimits"; "symmetric"]}
let nAryIntersectionPrefix = {character = "&#x22C2;"; glyph = "⋂"; name="n-ary intersection"; form="prefix"; priority="330"; lspace="1"; rspace="2"; properties=["largeop"; "movablelimits"; "symmetric"]}
let nAryLogicalAndPrefix = {character = "&#x22C0;"; glyph = "⋀"; name="n-ary logical and"; form="prefix"; priority="330"; lspace="1"; rspace="2"; properties=["largeop"; "movablelimits"; "symmetric"]}
let nAryLogicalOrPrefix = {character = "&#x22C1;"; glyph = "⋁"; name="n-ary logical or"; form="prefix"; priority="330"; lspace="1"; rspace="2"; properties=["largeop"; "movablelimits"; "symmetric"]}
let nAryProductPrefix = {character = "&#x220F;"; glyph = "∏"; name="n-ary product"; form="prefix"; priority="350"; lspace="1"; rspace="2"; properties=["largeop"; "movablelimits"; "symmetric"]}
let nArySquareIntersectionOperatorPrefix = {character = "&#x2A05;"; glyph = "⨅"; name="n-ary square intersection operator"; form="prefix"; priority="330"; lspace="1"; rspace="2"; properties=["largeop"; "movablelimits"; "symmetric"]}
let nArySquareUnionOperatorPrefix = {character = "&#x2A06;"; glyph = "⨆"; name="n-ary square union operator"; form="prefix"; priority="330"; lspace="1"; rspace="2"; properties=["largeop"; "movablelimits"; "symmetric"]}
let nArySummationPrefix = {character = "&#x2211;"; glyph = "∑"; name="n-ary summation"; form="prefix"; priority="290"; lspace="1"; rspace="2"; properties=["largeop"; "movablelimits"; "symmetric"]}
let nAryTimesOperatorPrefix = {character = "&#x2A09;"; glyph = "⨉"; name="n-ary times operator"; form="prefix"; priority="330"; lspace="1"; rspace="2"; properties=["largeop"; "movablelimits"; "symmetric"]}
let nAryUnionOperatorWithDotPrefix = {character = "&#x2A03;"; glyph = "⨃"; name="n-ary union operator with dot"; form="prefix"; priority="320"; lspace="1"; rspace="2"; properties=["largeop"; "movablelimits"; "symmetric"]}
let nAryUnionOperatorWithPlusPrefix = {character = "&#x2A04;"; glyph = "⨄"; name="n-ary union operator with plus"; form="prefix"; priority="320"; lspace="1"; rspace="2"; properties=["largeop"; "movablelimits"; "symmetric"]}
let nAryUnionPrefix = {character = "&#x22C3;"; glyph = "⋃"; name="n-ary union"; form="prefix"; priority="320"; lspace="1"; rspace="2"; properties=["largeop"; "movablelimits"; "symmetric"]}
let nAryWhiteVerticalBarPrefix = {character = "&#x2AFF;"; glyph = "⫿"; name="n-ary white vertical bar"; form="prefix"; priority="330"; lspace="1"; rspace="2"; properties=["largeop"; "movablelimits"; "symmetric"]}
let negatedDoubleVerticalBarDoubleRightTurnstileInfix = {character = "&#x22AF;"; glyph = "⊯"; name="negated double vertical bar double right turnstile"; form="infix"; priority="170"; lspace="5"; rspace="5"; properties=[]}
let neitherApproximatelyNorActuallyEqualToInfix = {character = "&#x2247;"; glyph = "≇"; name="neither approximately nor actually equal to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let neitherASubsetOfNorEqualToInfix = {character = "&#x2288;"; glyph = "⊈"; name="neither a subset of nor equal to"; form="infix"; priority="240"; lspace="5"; rspace="5"; properties=[]}
let neitherASupersetOfNorEqualToInfix = {character = "&#x2289;"; glyph = "⊉"; name="neither a superset of nor equal to"; form="infix"; priority="240"; lspace="5"; rspace="5"; properties=[]}
let neitherGreaterThanNorEqualToInfix = {character = "&#x2271;"; glyph = "≱"; name="neither greater-than nor equal to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let neitherGreaterThanNorEquivalentToInfix = {character = "&#x2275;"; glyph = "≵"; name="neither greater-than nor equivalent to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let neitherGreaterThanNorLessThanInfix = {character = "&#x2279;"; glyph = "≹"; name="neither greater-than nor less-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let neitherLessThanNorEqualToInfix = {character = "&#x2270;"; glyph = "≰"; name="neither less-than nor equal to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let neitherLessThanNorEquivalentToInfix = {character = "&#x2274;"; glyph = "≴"; name="neither less-than nor equivalent to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let neitherLessThanNorGreaterThanInfix = {character = "&#x2278;"; glyph = "≸"; name="neither less-than nor greater-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let nonforkingInfix = {character = "&#x2ADD;"; glyph = "⫝"; name="nonforking"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let nonforkingWithSlashInfix = {character = "&#x2ADD;&#x338;"; glyph = "⫝̸"; name="nonforking with slash"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let norInfix = {character = "&#x22BD;"; glyph = "⊽"; name="nor"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let normalSubgroupOfInfix = {character = "&#x22B2;"; glyph = "⊲"; name="normal subgroup of"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let normalSubgroupOfOrEqualToInfix = {character = "&#x22B4;"; glyph = "⊴"; name="normal subgroup of or equal to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let northEastAndSouthWestArrowInfix = {character = "&#x2922;"; glyph = "⤢"; name="north east and south west arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let northEastArrowAndSouthEastArrowInfix = {character = "&#x2928;"; glyph = "⤨"; name="north east arrow and south east arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let northEastArrowCrossingNorthWestArrowInfix = {character = "&#x2931;"; glyph = "⤱"; name="north east arrow crossing north west arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let northEastArrowCrossingSouthEastArrowInfix = {character = "&#x292E;"; glyph = "⤮"; name="north east arrow crossing south east arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let northEastArrowInfix = {character = "&#x2197;"; glyph = "↗"; name="north east arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let northEastArrowWithHookInfix = {character = "&#x2924;"; glyph = "⤤"; name="north east arrow with hook"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let northEastDoubleArrowInfix = {character = "&#x21D7;"; glyph = "⇗"; name="north east double arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let northWestAndSouthEastArrowInfix = {character = "&#x2921;"; glyph = "⤡"; name="north west and south east arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let northWestArrowAndNorthEastArrowInfix = {character = "&#x2927;"; glyph = "⤧"; name="north west arrow and north east arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let northWestArrowCrossingNorthEastArrowInfix = {character = "&#x2932;"; glyph = "⤲"; name="north west arrow crossing north east arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let northWestArrowInfix = {character = "&#x2196;"; glyph = "↖"; name="north west arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let northWestArrowToCornerInfix = {character = "&#x21F1;"; glyph = "⇱"; name="north west arrow to corner"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let northWestArrowToLongBarInfix = {character = "&#x21B8;"; glyph = "↸"; name="north west arrow to long bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let northWestArrowWithHookInfix = {character = "&#x2923;"; glyph = "⤣"; name="north west arrow with hook"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let northWestDoubleArrowInfix = {character = "&#x21D6;"; glyph = "⇖"; name="north west double arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let notAlmostEqualToInfix = {character = "&#x2249;"; glyph = "≉"; name="not almost equal to"; form="infix"; priority="250"; lspace="5"; rspace="5"; properties=[]}
let notAnElementOfInfix = {character = "&#x2209;"; glyph = "∉"; name="not an element of"; form="infix"; priority="240"; lspace="5"; rspace="5"; properties=[]}
let notASubsetOfInfix = {character = "&#x2284;"; glyph = "⊄"; name="not a subset of"; form="infix"; priority="240"; lspace="5"; rspace="5"; properties=[]}
let notASupersetOfInfix = {character = "&#x2285;"; glyph = "⊅"; name="not a superset of"; form="infix"; priority="240"; lspace="5"; rspace="5"; properties=[]}
let notAsymptoticallyEqualToInfix = {character = "&#x2244;"; glyph = "≄"; name="not asymptotically equal to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let notEqualToInfix = {character = "&#x2260;"; glyph = "≠"; name="not equal to"; form="infix"; priority="255"; lspace="5"; rspace="5"; properties=[]}
let notEquivalentToInfix = {character = "&#x226D;"; glyph = "≭"; name="not equivalent to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let notGreaterThanInfix = {character = "&#x226F;"; glyph = "≯"; name="not greater-than"; form="infix"; priority="244"; lspace="5"; rspace="5"; properties=[]}
let notIdenticalToInfix = {character = "&#x2262;"; glyph = "≢"; name="not identical to"; form="infix"; priority="252"; lspace="5"; rspace="5"; properties=[]}
let notLessThanInfix = {character = "&#x226E;"; glyph = "≮"; name="not less-than"; form="infix"; priority="246"; lspace="5"; rspace="5"; properties=[]}
let notNormalSubgroupOfInfix = {character = "&#x22EA;"; glyph = "⋪"; name="not normal subgroup of"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let notNormalSubgroupOfOrEqualToInfix = {character = "&#x22EC;"; glyph = "⋬"; name="not normal subgroup of or equal to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let notParallelToInfix = {character = "&#x2226;"; glyph = "∦"; name="not parallel to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let notSignPrefix = {character = "&#xAC;"; glyph = "¬"; name="not sign"; form="prefix"; priority="680"; lspace="2"; rspace="1"; properties=[]}
let notSquareImageOfOrEqualToInfix = {character = "&#x22E2;"; glyph = "⋢"; name="not square image of or equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let notSquareOriginalOfOrEqualToInfix = {character = "&#x22E3;"; glyph = "⋣"; name="not square original of or equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let notTildeInfix = {character = "&#x2241;"; glyph = "≁"; name="not tilde"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let notTrueInfix = {character = "&#x22AD;"; glyph = "⊭"; name="not true"; form="infix"; priority="170"; lspace="5"; rspace="5"; properties=[]}
let obliqueAngleOpeningDownInfix = {character = "&#x29A7;"; glyph = "⦧"; name="oblique angle opening down"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let obliqueAngleOpeningUpInfix = {character = "&#x29A6;"; glyph = "⦦"; name="oblique angle opening up"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let originalOfInfix = {character = "&#x22B6;"; glyph = "⊶"; name="original of"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let overlinePostfix = {character = "&#x203E;"; glyph = "‾"; name="overline"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["stretchy"; "accent"]}
let parallelToInfix = {character = "&#x2225;"; glyph = "∥"; name="parallel to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let parallelWithHorizontalStrokeInfix = {character = "&#x2AF2;"; glyph = "⫲"; name="parallel with horizontal stroke"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let parallelWithTildeOperatorInfix = {character = "&#x2AF3;"; glyph = "⫳"; name="parallel with tilde operator"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let partialDifferentialPrefix = {character = "&#x2202;"; glyph = "∂"; name="partial differential"; form="prefix"; priority="740"; lspace="2"; rspace="1"; properties=[]}
let percentSignInfix = {character = "%"; glyph = "%"; name="percent sign"; form="infix"; priority="640"; lspace="3"; rspace="3"; properties=[]}
let perpendicularWithSInfix = {character = "&#x2AE1;"; glyph = "⫡"; name="perpendicular with s"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let pitchforkInfix = {character = "&#x22D4;"; glyph = "⋔"; name="pitchfork"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let pitchforkWithTeeTopInfix = {character = "&#x2ADA;"; glyph = "⫚"; name="pitchfork with tee top"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let plusMinusSignInfix = {character = "&#xB1;"; glyph = "±"; name="plus-minus sign"; form="infix"; priority="275"; lspace="4"; rspace="4"; properties=[]}
let plusMinusSignPrefix = {character = "&#xB1;"; glyph = "±"; name="plus-minus sign"; form="prefix"; priority="275"; lspace="0"; rspace="1"; properties=[]}
let plusSignAboveEqualsSignInfix = {character = "&#x2A72;"; glyph = "⩲"; name="plus sign above equals sign"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let plusSignInfix = {character = "+"; glyph = "+"; name="plus sign"; form="infix"; priority="275"; lspace="4"; rspace="4"; properties=[]}
let plusSignInLeftHalfCircleInfix = {character = "&#x2A2D;"; glyph = "⨭"; name="plus sign in left half circle"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let plusSignInRightHalfCircleInfix = {character = "&#x2A2E;"; glyph = "⨮"; name="plus sign in right half circle"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let plusSignInTriangleInfix = {character = "&#x2A39;"; glyph = "⨹"; name="plus sign in triangle"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let plusSignPrefix = {character = "+"; glyph = "+"; name="plus sign"; form="prefix"; priority="275"; lspace="0"; rspace="1"; properties=[]}
let plusSignWithBlackTriangleInfix = {character = "&#x2A28;"; glyph = "⨨"; name="plus sign with black triangle"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let plusSignWithCircumflexAccentAboveInfix = {character = "&#x2A23;"; glyph = "⨣"; name="plus sign with circumflex accent above"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let plusSignWithDotBelowInfix = {character = "&#x2A25;"; glyph = "⨥"; name="plus sign with dot below"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let plusSignWithSmallCircleAboveInfix = {character = "&#x2A22;"; glyph = "⨢"; name="plus sign with small circle above"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let plusSignWithSubscriptTwoInfix = {character = "&#x2A27;"; glyph = "⨧"; name="plus sign with subscript two"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let plusSignWithTildeAboveInfix = {character = "&#x2A24;"; glyph = "⨤"; name="plus sign with tilde above"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let plusSignWithTildeBelowInfix = {character = "&#x2A26;"; glyph = "⨦"; name="plus sign with tilde below"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let precedesAboveAlmostEqualToInfix = {character = "&#x2AB7;"; glyph = "⪷"; name="precedes above almost equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let precedesAboveEqualsSignInfix = {character = "&#x2AB3;"; glyph = "⪳"; name="precedes above equals sign"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let precedesAboveNotAlmostEqualToInfix = {character = "&#x2AB9;"; glyph = "⪹"; name="precedes above not almost equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let precedesAboveNotEqualToInfix = {character = "&#x2AB5;"; glyph = "⪵"; name="precedes above not equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let precedesAboveSingleLineEqualsSignInfix = {character = "&#x2AAF;"; glyph = "⪯"; name="precedes above single-line equals sign"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let precedesAboveSingleLineEqualsSignWithSlashInfix = {character = "&#x2AAF;&#x338;"; glyph = "⪯̸"; name="precedes above single-line equals sign with slash"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let precedesAboveSingleLineNotEqualToInfix = {character = "&#x2AB1;"; glyph = "⪱"; name="precedes above single-line not equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let precedesButNotEquivalentToInfix = {character = "&#x22E8;"; glyph = "⋨"; name="precedes but not equivalent to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let precedesInfix = {character = "&#x227A;"; glyph = "≺"; name="precedes"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let precedesOrEqualToInfix = {character = "&#x227C;"; glyph = "≼"; name="precedes or equal to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let precedesOrEquivalentToInfix = {character = "&#x227E;"; glyph = "≾"; name="precedes or equivalent to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let precedesUnderRelationInfix = {character = "&#x22B0;"; glyph = "⊰"; name="precedes under relation"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let primePostfix = {character = "&#x2032;"; glyph = "′"; name="prime"; form="postfix"; priority="800"; lspace="0"; rspace="0"; properties=[]}
let proportionalToInfix = {character = "&#x221D;"; glyph = "∝"; name="proportional to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let proportionInfix = {character = "&#x2237;"; glyph = "∷"; name="proportion"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let quadrupleIntegralOperatorPrefix = {character = "&#x2A0C;"; glyph = "⨌"; name="quadruple integral operator"; form="prefix"; priority="310"; lspace="0"; rspace="1"; properties=["largeop"; "symmetric"]}
let quadruplePrimePostfix = {character = "&#x2057;"; glyph = "⁗"; name="quadruple prime"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let quaternionIntegralOperatorPrefix = {character = "&#x2A16;"; glyph = "⨖"; name="quaternion integral operator"; form="prefix"; priority="310"; lspace="1"; rspace="2"; properties=["largeop"; "symmetric"]}
let questionedEqualToInfix = {character = "&#x225F;"; glyph = "≟"; name="questioned equal to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let questionMarkInfix = {character = "?"; glyph = "?"; name="question mark"; form="infix"; priority="835"; lspace="1"; rspace="1"; properties=[]}
let quotationMarkPostfix = {character = "\; glyph = "\; name="quotation mark"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let ratioInfix = {character = "&#x2236;"; glyph = "∶"; name="ratio"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let reversedAngleInfix = {character = "&#x29A3;"; glyph = "⦣"; name="reversed angle"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let reversedAngleWithUnderbarInfix = {character = "&#x29A5;"; glyph = "⦥"; name="reversed angle with underbar"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let reversedDoublePrimePostfix = {character = "&#x2036;"; glyph = "‶"; name="reversed double prime"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let reversedDoubleStrokeNotSignInfix = {character = "&#x2AED;"; glyph = "⫭"; name="reversed double stroke not sign"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let reversedEmptySetInfix = {character = "&#x29B0;"; glyph = "⦰"; name="reversed empty set"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let reversedPrimePostfix = {character = "&#x2035;"; glyph = "‵"; name="reversed prime"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let reversedTildeEqualsInfix = {character = "&#x22CD;"; glyph = "⋍"; name="reversed tilde equals"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let reversedTildeInfix = {character = "&#x223D;"; glyph = "∽"; name="reversed tilde"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let reversedTildeWithUnderlineInfix = {character = "&#x223D;&#x331;"; glyph = "∽̱"; name="reversed tilde with underline"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let reversedTriplePrimePostfix = {character = "&#x2037;"; glyph = "‷"; name="reversed triple prime"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let reverseSolidusInfix = {character = @"\"; glyph = @"\"; name="reverse solidus"; form="infix"; priority="650"; lspace="0"; rspace="0"; properties=[]}
let reverseSolidusOperatorInfix = {character = "&#x29F5;"; glyph = "⧵"; name="reverse solidus operator"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let reverseSolidusWithHorizontalStrokeInfix = {character = "&#x29F7;"; glyph = "⧷"; name="reverse solidus with horizontal stroke"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let rightAngleBracketWithDotPostfix = {character = "&#x2992;"; glyph = "⦒"; name="right angle bracket with dot"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let rightAngleInfix = {character = "&#x221F;"; glyph = "∟"; name="right angle"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let rightAngleVariantWithSquareInfix = {character = "&#x299C;"; glyph = "⦜"; name="right angle variant with square"; form="infix"; priority="270"; lspace="3"; rspace="3"; properties=[]}
let rightAngleWithArcInfix = {character = "&#x22BE;"; glyph = "⊾"; name="right angle with arc"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let rightArcGreaterThanBracketPostfix = {character = "&#x2994;"; glyph = "⦔"; name="right arc greater-than bracket"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let rightArrowWithSmallCircleInfix = {character = "&#x21F4;"; glyph = "⇴"; name="right arrow with small circle"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightBlackTortoiseShellBracketPostfix = {character = "&#x2998;"; glyph = "⦘"; name="right black tortoise shell bracket"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let rightCeilingPostfix = {character = "&#x2309;"; glyph = "⌉"; name="right ceiling"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let rightCurlyBracketPostfix = {character = "}"; glyph = "}"; name="right curly bracket"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let rightDoubleArrowWithRoundedHeadInfix = {character = "&#x2970;"; glyph = "⥰"; name="right double arrow with rounded head"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightDoubleQuotationMarkPostfix = {character = "&#x201D;"; glyph = "”"; name="right double quotation mark"; form="postfix"; priority="10"; lspace="0"; rspace="0"; properties=["fence"]}
let rightDoubleWigglyFenceInfix = {character = "&#x29DB;"; glyph = "⧛"; name="right double wiggly fence"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let rightFishTailInfix = {character = "&#x297D;"; glyph = "⥽"; name="right fish tail"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightFloorPostfix = {character = "&#x230B;"; glyph = "⌋"; name="right floor"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let rightHalfBlackCircleInfix = {character = "&#x25D7;"; glyph = "◗"; name="right half black circle"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let righthandInteriorProductInfix = {character = "&#x2A3D;"; glyph = "⨽"; name="righthand interior product"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let rightNormalFactorSemidirectProductInfix = {character = "&#x22CA;"; glyph = "⋊"; name="right normal factor semidirect product"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let rightParenthesisPostfix = {character = ")"; glyph = ")"; name="right parenthesis"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let rightPointingAngleBracketPostfix = {character = "&#x232A;"; glyph = "〉"; name="right-pointing angle bracket"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let rightPointingCurvedAngleBracketPostfix = {character = "&#x29FD;"; glyph = "⧽"; name="right-pointing curved angle bracket"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let rightSemidirectProductInfix = {character = "&#x22CC;"; glyph = "⋌"; name="right semidirect product"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let rightSideArcClockwiseArrowInfix = {character = "&#x2938;"; glyph = "⤸"; name="right-side arc clockwise arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let rightSingleQuotationMarkPostfix = {character = "&#x2019;"; glyph = "’"; name="right single quotation mark"; form="postfix"; priority="10"; lspace="0"; rspace="0"; properties=["fence"]}
let rightSquareBracketPostfix = {character = "]"; glyph = "]"; name="right square bracket"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let rightSquareBracketWithTickInBottomCornerPostfix = {character = "&#x298E;"; glyph = "⦎"; name="right square bracket with tick in bottom corner"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let rightSquareBracketWithTickInTopCornerPostfix = {character = "&#x2990;"; glyph = "⦐"; name="right square bracket with tick in top corner"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let rightSquareBracketWithUnderbarPostfix = {character = "&#x298C;"; glyph = "⦌"; name="right square bracket with underbar"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let rightTackInfix = {character = "&#x22A2;"; glyph = "⊢"; name="right tack"; form="infix"; priority="170"; lspace="5"; rspace="5"; properties=[]}
let rightTriangleAboveLeftTriangleInfix = {character = "&#x29CE;"; glyph = "⧎"; name="right triangle above left triangle"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let rightTriangleInfix = {character = "&#x22BF;"; glyph = "⊿"; name="right triangle"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let rightwardsArrowAboveAlmostEqualToInfix = {character = "&#x2975;"; glyph = "⥵"; name="rightwards arrow above almost equal to"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsArrowAboveShortLeftwardsArrowInfix = {character = "&#x2942;"; glyph = "⥂"; name="rightwards arrow above short leftwards arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsArrowAboveTildeOperatorInfix = {character = "&#x2974;"; glyph = "⥴"; name="rightwards arrow above tilde operator"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsArrowFromBarInfix = {character = "&#x21A6;"; glyph = "↦"; name="rightwards arrow from bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightwardsArrowFromBarToBlackDiamondInfix = {character = "&#x2920;"; glyph = "⤠"; name="rightwards arrow from bar to black diamond"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsArrowInfix = {character = "&#x2192;"; glyph = "→"; name="rightwards arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightwardsArrowOverLeftwardsArrowInfix = {character = "&#x21C4;"; glyph = "⇄"; name="rightwards arrow over leftwards arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightwardsArrowTailInfix = {character = "&#x291A;"; glyph = "⤚"; name="rightwards arrow-tail"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsArrowThroughXInfix = {character = "&#x2947;"; glyph = "⥇"; name="rightwards arrow through x"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsArrowToBarInfix = {character = "&#x21E5;"; glyph = "⇥"; name="rightwards arrow to bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightwardsArrowToBlackDiamondInfix = {character = "&#x291E;"; glyph = "⤞"; name="rightwards arrow to black diamond"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsArrowWithCornerDownwardsInfix = {character = "&#x21B4;"; glyph = "↴"; name="rightwards arrow with corner downwards"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let rightwardsArrowWithDottedStemInfix = {character = "&#x2911;"; glyph = "⤑"; name="rightwards arrow with dotted stem"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsArrowWithDoubleVerticalStrokeInfix = {character = "&#x21FB;"; glyph = "⇻"; name="rightwards arrow with double vertical stroke"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsArrowWithHookInfix = {character = "&#x21AA;"; glyph = "↪"; name="rightwards arrow with hook"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightwardsArrowWithLoopInfix = {character = "&#x21AC;"; glyph = "↬"; name="rightwards arrow with loop"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightwardsArrowWithPlusBelowInfix = {character = "&#x2945;"; glyph = "⥅"; name="rightwards arrow with plus below"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsArrowWithStrokeInfix = {character = "&#x219B;"; glyph = "↛"; name="rightwards arrow with stroke"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsArrowWithTailInfix = {character = "&#x21A3;"; glyph = "↣"; name="rightwards arrow with tail"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightwardsArrowWithTailWithDoubleVerticalStrokeInfix = {character = "&#x2915;"; glyph = "⤕"; name="rightwards arrow with tail with double vertical stroke"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsArrowWithTailWithVerticalStrokeInfix = {character = "&#x2914;"; glyph = "⤔"; name="rightwards arrow with tail with vertical stroke"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsArrowWithVerticalStrokeInfix = {character = "&#x21F8;"; glyph = "⇸"; name="rightwards arrow with vertical stroke"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsDashedArrowInfix = {character = "&#x21E2;"; glyph = "⇢"; name="rightwards dashed arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightwardsDoubleArrowFromBarInfix = {character = "&#x2907;"; glyph = "⤇"; name="rightwards double arrow from bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsDoubleArrowInfix = {character = "&#x21D2;"; glyph = "⇒"; name="rightwards double arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightwardsDoubleArrowTailInfix = {character = "&#x291C;"; glyph = "⤜"; name="rightwards double arrow-tail"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsDoubleArrowWithStrokeInfix = {character = "&#x21CF;"; glyph = "⇏"; name="rightwards double arrow with stroke"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsDoubleArrowWithVerticalStrokeInfix = {character = "&#x2903;"; glyph = "⤃"; name="rightwards double arrow with vertical stroke"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsDoubleDashArrowInfix = {character = "&#x290D;"; glyph = "⤍"; name="rightwards double dash arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightwardsHarpoonOverLeftwardsHarpoonInfix = {character = "&#x21CC;"; glyph = "⇌"; name="rightwards harpoon over leftwards harpoon"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightwardsHarpoonWithBarbDownAboveLeftwardsHarpoonWithBarbDownInfix = {character = "&#x2969;"; glyph = "⥩"; name="rightwards harpoon with barb down above leftwards harpoon with barb down"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsHarpoonWithBarbDownBelowLongDashInfix = {character = "&#x296D;"; glyph = "⥭"; name="rightwards harpoon with barb down below long dash"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsHarpoonWithBarbDownFromBarInfix = {character = "&#x295F;"; glyph = "⥟"; name="rightwards harpoon with barb down from bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightwardsHarpoonWithBarbDownToBarInfix = {character = "&#x2957;"; glyph = "⥗"; name="rightwards harpoon with barb down to bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let rightwardsHarpoonWithBarbDownwardsInfix = {character = "&#x21C1;"; glyph = "⇁"; name="rightwards harpoon with barb downwards"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightwardsHarpoonWithBarbUpAboveLeftwardsHarpoonWithBarbUpInfix = {character = "&#x2968;"; glyph = "⥨"; name="rightwards harpoon with barb up above leftwards harpoon with barb up"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsHarpoonWithBarbUpAboveLongDashInfix = {character = "&#x296C;"; glyph = "⥬"; name="rightwards harpoon with barb up above long dash"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsHarpoonWithBarbUpAboveRightwardsHarpoonWithBarbDownInfix = {character = "&#x2964;"; glyph = "⥤"; name="rightwards harpoon with barb up above rightwards harpoon with barb down"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsHarpoonWithBarbUpFromBarInfix = {character = "&#x295B;"; glyph = "⥛"; name="rightwards harpoon with barb up from bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightwardsHarpoonWithBarbUpToBarInfix = {character = "&#x2953;"; glyph = "⥓"; name="rightwards harpoon with barb up to bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightwardsHarpoonWithBarbUpwardsInfix = {character = "&#x21C0;"; glyph = "⇀"; name="rightwards harpoon with barb upwards"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightwardsOpenHeadedArrowInfix = {character = "&#x21FE;"; glyph = "⇾"; name="rightwards open-headed arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightwardsPairedArrowsInfix = {character = "&#x21C9;"; glyph = "⇉"; name="rightwards paired arrows"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightwardsQuadrupleArrowInfix = {character = "&#x2B46;"; glyph = "⭆"; name="rightwards quadruple arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let rightwardsSquiggleArrowInfix = {character = "&#x21DD;"; glyph = "⇝"; name="rightwards squiggle arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightwardsTripleArrowInfix = {character = "&#x21DB;"; glyph = "⇛"; name="rightwards triple arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightwardsTripleDashArrowInfix = {character = "&#x290F;"; glyph = "⤏"; name="rightwards triple dash arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightwardsTwoHeadedArrowFromBarInfix = {character = "&#x2905;"; glyph = "⤅"; name="rightwards two-headed arrow from bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsTwoHeadedArrowInfix = {character = "&#x21A0;"; glyph = "↠"; name="rightwards two headed arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightwardsTwoHeadedArrowWithDoubleVerticalStrokeInfix = {character = "&#x2901;"; glyph = "⤁"; name="rightwards two-headed arrow with double vertical stroke"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsTwoHeadedArrowWithTailInfix = {character = "&#x2916;"; glyph = "⤖"; name="rightwards two-headed arrow with tail"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsTwoHeadedArrowWithTailWithDoubleVerticalStrokeInfix = {character = "&#x2918;"; glyph = "⤘"; name="rightwards two-headed arrow with tail with double vertical stroke"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsTwoHeadedArrowWithTailWithVerticalStrokeInfix = {character = "&#x2917;"; glyph = "⤗"; name="rightwards two-headed arrow with tail with vertical stroke"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsTwoHeadedArrowWithVerticalStrokeInfix = {character = "&#x2900;"; glyph = "⤀"; name="rightwards two-headed arrow with vertical stroke"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let rightwardsTwoHeadedTripleDashArrowInfix = {character = "&#x2910;"; glyph = "⤐"; name="rightwards two-headed triple dash arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightwardsWaveArrowInfix = {character = "&#x219D;"; glyph = "↝"; name="rightwards wave arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightwardsWhiteArrowFromWallInfix = {character = "&#x21F0;"; glyph = "⇰"; name="rightwards white arrow from wall"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightwardsWhiteArrowInfix = {character = "&#x21E8;"; glyph = "⇨"; name="rightwards white arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let rightWhiteCurlyBracketPostfix = {character = "&#x2984;"; glyph = "⦄"; name="right white curly bracket"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let rightWhiteParenthesisPostfix = {character = "&#x2986;"; glyph = "⦆"; name="right white parenthesis"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let rightWigglyFenceInfix = {character = "&#x29D9;"; glyph = "⧙"; name="right wiggly fence"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let ringAbovePostfix = {character = "&#x2DA;"; glyph = "˚"; name="ring above"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let ringEqualToInfix = {character = "&#x2257;"; glyph = "≗"; name="ring equal to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let ringInEqualToInfix = {character = "&#x2256;"; glyph = "≖"; name="ring in equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let ringOperatorInfix = {character = "&#x2218;"; glyph = "∘"; name="ring operator"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let risingDiagonalCrossingFallingDiagonalInfix = {character = "&#x292B;"; glyph = "⤫"; name="rising diagonal crossing falling diagonal"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let risingDiagonalCrossingSouthEastArrowInfix = {character = "&#x2930;"; glyph = "⤰"; name="rising diagonal crossing south east arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let ruleDelayedInfix = {character = "&#x29F4;"; glyph = "⧴"; name="rule-delayed"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let semicolonInfix = {character = ";"; glyph = ";"; name="semicolon"; form="infix"; priority="30"; lspace="0"; rspace="3"; properties=["separator"; "linebreakstyle=after"]}
let semidirectProductWithBottomClosedInfix = {character = "&#x2A32;"; glyph = "⨲"; name="semidirect product with bottom closed"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let setMinusInfix = {character = "&#x2216;"; glyph = "∖"; name="set minus"; form="infix"; priority="650"; lspace="4"; rspace="4"; properties=[]}
let shortDownTackInfix = {character = "&#x2ADF;"; glyph = "⫟"; name="short down tack"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let shortDownTackWithOverbarInfix = {character = "&#x2AE7;"; glyph = "⫧"; name="short down tack with overbar"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let shortLeftTackInfix = {character = "&#x2ADE;"; glyph = "⫞"; name="short left tack"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let shortRightwardsArrowAboveLeftwardsArrowInfix = {character = "&#x2944;"; glyph = "⥄"; name="short rightwards arrow above leftwards arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let shortUpTackAboveShortDownTackInfix = {character = "&#x2AE9;"; glyph = "⫩"; name="short up tack above short down tack"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let shortUpTackInfix = {character = "&#x2AE0;"; glyph = "⫠"; name="short up tack"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let shortUpTackWithUnderbarInfix = {character = "&#x2AE8;"; glyph = "⫨"; name="short up tack with underbar"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let shuffleProductInfix = {character = "&#x29E2;"; glyph = "⧢"; name="shuffle product"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let similarAboveGreaterThanAboveEqualsSignInfix = {character = "&#x2AA0;"; glyph = "⪠"; name="similar above greater-than above equals sign"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let similarAboveLessThanAboveEqualsSignInfix = {character = "&#x2A9F;"; glyph = "⪟"; name="similar above less-than above equals sign"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let similarMinusSimilarInfix = {character = "&#x2A6C;"; glyph = "⩬"; name="similar minus similar"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let similarOrGreaterThanInfix = {character = "&#x2A9E;"; glyph = "⪞"; name="similar or greater-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let similarOrLessThanInfix = {character = "&#x2A9D;"; glyph = "⪝"; name="similar or less-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let sineWaveInfix = {character = "&#x223F;"; glyph = "∿"; name="sine wave"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let singleHighReversed9QuotationMarkPostfix = {character = "&#x201B;"; glyph = "‛"; name="single high-reversed-9 quotation mark"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let singleLow9QuotationMarkPostfix = {character = "&#x201A;"; glyph = "‚"; name="single low-9 quotation mark"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let sInTriangleInfix = {character = "&#x29CC;"; glyph = "⧌"; name="s in triangle"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let slantedEqualToOrGreaterThanInfix = {character = "&#x2A96;"; glyph = "⪖"; name="slanted equal to or greater-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let slantedEqualToOrGreaterThanWithDotInsideInfix = {character = "&#x2A98;"; glyph = "⪘"; name="slanted equal to or greater-than with dot inside"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let slantedEqualToOrLessThanInfix = {character = "&#x2A95;"; glyph = "⪕"; name="slanted equal to or less-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let slantedEqualToOrLessThanWithDotInsideInfix = {character = "&#x2A97;"; glyph = "⪗"; name="slanted equal to or less-than with dot inside"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let slopingLargeAndInfix = {character = "&#x2A58;"; glyph = "⩘"; name="sloping large and"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let slopingLargeOrInfix = {character = "&#x2A57;"; glyph = "⩗"; name="sloping large or"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let smallContainsAsMemberInfix = {character = "&#x220D;"; glyph = "∍"; name="small contains as member"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let smallContainsWithOverbarInfix = {character = "&#x22FE;"; glyph = "⋾"; name="small contains with overbar"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let smallContainsWithVerticalBarAtEndOfHorizontalStrokeInfix = {character = "&#x22FC;"; glyph = "⋼"; name="small contains with vertical bar at end of horizontal stroke"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let smallElementOfInfix = {character = "&#x220A;"; glyph = "∊"; name="small element of"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let smallElementOfWithOverbarInfix = {character = "&#x22F7;"; glyph = "⋷"; name="small element of with overbar"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let smallElementOfWithVerticalBarAtEndOfHorizontalStrokeInfix = {character = "&#x22F4;"; glyph = "⋴"; name="small element of with vertical bar at end of horizontal stroke"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let smallerThanInfix = {character = "&#x2AAA;"; glyph = "⪪"; name="smaller than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let smallerThanOrEqualToInfix = {character = "&#x2AAC;"; glyph = "⪬"; name="smaller than or equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let smallTildePostfix = {character = "&#x2DC;"; glyph = "˜"; name="small tilde"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["stretchy"; "accent"]}
let smallVeeWithUnderbarInfix = {character = "&#x2A61;"; glyph = "⩡"; name="small vee with underbar"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let smashProductInfix = {character = "&#x2A33;"; glyph = "⨳"; name="smash product"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let solidusInfix = {character = "/"; glyph = "/"; name="solidus"; form="infix"; priority="660"; lspace="1"; rspace="1"; properties=[]}
let solidusWithOverbarInfix = {character = "&#x29F6;"; glyph = "⧶"; name="solidus with overbar"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let southEastArrowAndSouthWestArrowInfix = {character = "&#x2929;"; glyph = "⤩"; name="south east arrow and south west arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let southEastArrowCrossingNorthEastArrowInfix = {character = "&#x292D;"; glyph = "⤭"; name="south east arrow crossing north east arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let southEastArrowInfix = {character = "&#x2198;"; glyph = "↘"; name="south east arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let southEastArrowToCornerInfix = {character = "&#x21F2;"; glyph = "⇲"; name="south east arrow to corner"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let southEastArrowWithHookInfix = {character = "&#x2925;"; glyph = "⤥"; name="south east arrow with hook"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let southEastDoubleArrowInfix = {character = "&#x21D8;"; glyph = "⇘"; name="south east double arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let southWestArrowAndNorthWestArrowInfix = {character = "&#x292A;"; glyph = "⤪"; name="south west arrow and north west arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let southWestArrowInfix = {character = "&#x2199;"; glyph = "↙"; name="south west arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let southWestArrowWithHookInfix = {character = "&#x2926;"; glyph = "⤦"; name="south west arrow with hook"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let southWestDoubleArrowInfix = {character = "&#x21D9;"; glyph = "⇙"; name="south west double arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let sphericalAngleOpeningLeftInfix = {character = "&#x29A0;"; glyph = "⦠"; name="spherical angle opening left"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let sphericalAngleOpeningUpInfix = {character = "&#x29A1;"; glyph = "⦡"; name="spherical angle opening up"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let sphericalAnglePrefix = {character = "&#x2222;"; glyph = "∢"; name="spherical angle"; form="prefix"; priority="670"; lspace="0"; rspace="0"; properties=[]}
let squareCapInfix = {character = "&#x2293;"; glyph = "⊓"; name="square cap"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let squareCupInfix = {character = "&#x2294;"; glyph = "⊔"; name="square cup"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let squaredAsteriskInfix = {character = "&#x29C6;"; glyph = "⧆"; name="squared asterisk"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let squaredDotOperatorInfix = {character = "&#x22A1;"; glyph = "⊡"; name="squared dot operator"; form="infix"; priority="390"; lspace="4"; rspace="4"; properties=[]}
let squaredFallingDiagonalSlashInfix = {character = "&#x29C5;"; glyph = "⧅"; name="squared falling diagonal slash"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let squaredMinusInfix = {character = "&#x229F;"; glyph = "⊟"; name="squared minus"; form="infix"; priority="275"; lspace="4"; rspace="4"; properties=[]}
let squaredPlusInfix = {character = "&#x229E;"; glyph = "⊞"; name="squared plus"; form="infix"; priority="275"; lspace="4"; rspace="4"; properties=[]}
let squaredRisingDiagonalSlashInfix = {character = "&#x29C4;"; glyph = "⧄"; name="squared rising diagonal slash"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let squaredSmallCircleInfix = {character = "&#x29C7;"; glyph = "⧇"; name="squared small circle"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let squaredSquareInfix = {character = "&#x29C8;"; glyph = "⧈"; name="squared square"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let squaredTimesInfix = {character = "&#x22A0;"; glyph = "⊠"; name="squared times"; form="infix"; priority="390"; lspace="4"; rspace="4"; properties=[]}
let squareImageOfInfix = {character = "&#x228F;"; glyph = "⊏"; name="square image of"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let squareImageOfOrEqualToInfix = {character = "&#x2291;"; glyph = "⊑"; name="square image of or equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let squareImageOfOrNotEqualToInfix = {character = "&#x22E4;"; glyph = "⋤"; name="square image of or not equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let squareImageOfWithSlashInfix = {character = "&#x228F;&#x338;"; glyph = "⊏̸"; name="square image of with slash"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let squareLeftOpenBoxOperatorInfix = {character = "&#x2ACD;"; glyph = "⫍"; name="square left open box operator"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let squareOriginalOfInfix = {character = "&#x2290;"; glyph = "⊐"; name="square original of"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let squareOriginalOfOrEqualToInfix = {character = "&#x2292;"; glyph = "⊒"; name="square original of or equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let squareOriginalOfOrNotEqualToInfix = {character = "&#x22E5;"; glyph = "⋥"; name="square original of or not equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let squareOriginalOfWithSlashInfix = {character = "&#x2290;&#x338;"; glyph = "⊐̸"; name="square original of with slash"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let squareRightOpenBoxOperatorInfix = {character = "&#x2ACE;"; glyph = "⫎"; name="square right open box operator"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let squareRootPrefix = {character = "&#x221A;"; glyph = "√"; name="square root"; form="prefix"; priority="845"; lspace="1"; rspace="1"; properties=["stretchy"]}
let squareWithContouredOutlineInfix = {character = "&#x29E0;"; glyph = "⧠"; name="square with contoured outline"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let starEqualsInfix = {character = "&#x225B;"; glyph = "≛"; name="star equals"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let starOperatorInfix = {character = "&#x22C6;"; glyph = "⋆"; name="star operator"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let strictlyEquivalentToInfix = {character = "&#x2263;"; glyph = "≣"; name="strictly equivalent to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let subsetAboveRightwardsArrowInfix = {character = "&#x2979;"; glyph = "⥹"; name="subset above rightwards arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let subsetAboveSubsetInfix = {character = "&#x2AD5;"; glyph = "⫕"; name="subset above subset"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let subsetAboveSupersetInfix = {character = "&#x2AD3;"; glyph = "⫓"; name="subset above superset"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let subsetOfAboveAlmostEqualToInfix = {character = "&#x2AC9;"; glyph = "⫉"; name="subset of above almost equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let subsetOfAboveEqualsSignInfix = {character = "&#x2AC5;"; glyph = "⫅"; name="subset of above equals sign"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let subsetOfAboveNotEqualToInfix = {character = "&#x2ACB;"; glyph = "⫋"; name="subset of above not equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let subsetOfAboveTildeOperatorInfix = {character = "&#x2AC7;"; glyph = "⫇"; name="subset of above tilde operator"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let subsetOfInfix = {character = "&#x2282;"; glyph = "⊂"; name="subset of"; form="infix"; priority="240"; lspace="5"; rspace="5"; properties=[]}
let subsetOfOrEqualToInfix = {character = "&#x2286;"; glyph = "⊆"; name="subset of or equal to"; form="infix"; priority="240"; lspace="5"; rspace="5"; properties=[]}
let subsetOfOrEqualToWithDotAboveInfix = {character = "&#x2AC3;"; glyph = "⫃"; name="subset of or equal to with dot above"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let subsetOfWithNotEqualToInfix = {character = "&#x228A;"; glyph = "⊊"; name="subset of with not equal to"; form="infix"; priority="240"; lspace="5"; rspace="5"; properties=[]}
let subsetOfWithVerticalLineInfix = {character = "&#x2282;&#x20D2;"; glyph = "⊂⃒"; name="subset of with vertical line"; form="infix"; priority="240"; lspace="5"; rspace="5"; properties=[]}
let subsetWithDotInfix = {character = "&#x2ABD;"; glyph = "⪽"; name="subset with dot"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let subsetWithMultiplicationSignBelowInfix = {character = "&#x2AC1;"; glyph = "⫁"; name="subset with multiplication sign below"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let subsetWithPlusSignBelowInfix = {character = "&#x2ABF;"; glyph = "⪿"; name="subset with plus sign below"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let succeedsAboveAlmostEqualToInfix = {character = "&#x2AB8;"; glyph = "⪸"; name="succeeds above almost equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let succeedsAboveEqualsSignInfix = {character = "&#x2AB4;"; glyph = "⪴"; name="succeeds above equals sign"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let succeedsAboveNotAlmostEqualToInfix = {character = "&#x2ABA;"; glyph = "⪺"; name="succeeds above not almost equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let succeedsAboveNotEqualToInfix = {character = "&#x2AB6;"; glyph = "⪶"; name="succeeds above not equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let succeedsAboveSingleLineEqualsSignInfix = {character = "&#x2AB0;"; glyph = "⪰"; name="succeeds above single-line equals sign"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let succeedsAboveSingleLineEqualsSignWithSlashInfix = {character = "&#x2AB0;&#x338;"; glyph = "⪰̸"; name="succeeds above single-line equals sign with slash"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let succeedsAboveSingleLineNotEqualToInfix = {character = "&#x2AB2;"; glyph = "⪲"; name="succeeds above single-line not equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let succeedsButNotEquivalentToInfix = {character = "&#x22E9;"; glyph = "⋩"; name="succeeds but not equivalent to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let succeedsInfix = {character = "&#x227B;"; glyph = "≻"; name="succeeds"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let succeedsOrEqualToInfix = {character = "&#x227D;"; glyph = "≽"; name="succeeds or equal to"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let succeedsOrEquivalentToInfix = {character = "&#x227F;"; glyph = "≿"; name="succeeds or equivalent to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let succeedsOrEquivalentToWithSlashInfix = {character = "&#x227F;&#x338;"; glyph = "≿̸"; name="succeeds or equivalent to with slash"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let succeedsUnderRelationInfix = {character = "&#x22B1;"; glyph = "⊱"; name="succeeds under relation"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let summationWithIntegralPrefix = {character = "&#x2A0B;"; glyph = "⨋"; name="summation with integral"; form="prefix"; priority="290"; lspace="1"; rspace="2"; properties=["largeop"; "symmetric"]}
let superscriptOnePostfix = {character = "&#xB9;"; glyph = "¹"; name="superscript one"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let superscriptThreePostfix = {character = "&#xB3;"; glyph = "³"; name="superscript three"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let superscriptTwoPostfix = {character = "&#xB2;"; glyph = "²"; name="superscript two"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let supersetAboveLeftwardsArrowInfix = {character = "&#x297B;"; glyph = "⥻"; name="superset above leftwards arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let supersetAboveSubsetInfix = {character = "&#x2AD4;"; glyph = "⫔"; name="superset above subset"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let supersetAboveSupersetInfix = {character = "&#x2AD6;"; glyph = "⫖"; name="superset above superset"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let supersetBesideAndJoinedByDashWithSubsetInfix = {character = "&#x2AD8;"; glyph = "⫘"; name="superset beside and joined by dash with subset"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let supersetBesideSubsetInfix = {character = "&#x2AD7;"; glyph = "⫗"; name="superset beside subset"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let supersetOfAboveAlmostEqualToInfix = {character = "&#x2ACA;"; glyph = "⫊"; name="superset of above almost equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let supersetOfAboveEqualsSignInfix = {character = "&#x2AC6;"; glyph = "⫆"; name="superset of above equals sign"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let supersetOfAboveNotEqualToInfix = {character = "&#x2ACC;"; glyph = "⫌"; name="superset of above not equal to"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let supersetOfAboveTildeOperatorInfix = {character = "&#x2AC8;"; glyph = "⫈"; name="superset of above tilde operator"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let supersetOfInfix = {character = "&#x2283;"; glyph = "⊃"; name="superset of"; form="infix"; priority="240"; lspace="5"; rspace="5"; properties=[]}
let supersetOfOrEqualToInfix = {character = "&#x2287;"; glyph = "⊇"; name="superset of or equal to"; form="infix"; priority="240"; lspace="5"; rspace="5"; properties=[]}
let supersetOfOrEqualToWithDotAboveInfix = {character = "&#x2AC4;"; glyph = "⫄"; name="superset of or equal to with dot above"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let supersetOfWithNotEqualToInfix = {character = "&#x228B;"; glyph = "⊋"; name="superset of with not equal to"; form="infix"; priority="240"; lspace="5"; rspace="5"; properties=[]}
let supersetOfWithVerticalLineInfix = {character = "&#x2283;&#x20D2;"; glyph = "⊃⃒"; name="superset of with vertical line"; form="infix"; priority="240"; lspace="5"; rspace="5"; properties=[]}
let supersetWithDotInfix = {character = "&#x2ABE;"; glyph = "⪾"; name="superset with dot"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let supersetWithMultiplicationSignBelowInfix = {character = "&#x2AC2;"; glyph = "⫂"; name="superset with multiplication sign below"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let supersetWithPlusSignBelowInfix = {character = "&#x2AC0;"; glyph = "⫀"; name="superset with plus sign below"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let surfaceIntegralPrefix = {character = "&#x222F;"; glyph = "∯"; name="surface integral"; form="prefix"; priority="310"; lspace="0"; rspace="1"; properties=["largeop"; "symmetric"]}
let thereDoesNotExistPrefix = {character = "&#x2204;"; glyph = "∄"; name="there does not exist"; form="prefix"; priority="230"; lspace="2"; rspace="1"; properties=[]}
let thereExistsPrefix = {character = "&#x2203;"; glyph = "∃"; name="there exists"; form="prefix"; priority="230"; lspace="2"; rspace="1"; properties=[]}
let thereforeInfix = {character = "&#x2234;"; glyph = "∴"; name="therefore"; form="infix"; priority="70"; lspace="5"; rspace="5"; properties=[]}
let thermodynamicInfix = {character = "&#x29E7;"; glyph = "⧧"; name="thermodynamic"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let threeConsecutiveEqualsSignsInfix = {character = "&#x2A76;"; glyph = "⩶"; name="three consecutive equals signs"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let threeRightwardsArrowsInfix = {character = "&#x21F6;"; glyph = "⇶"; name="three rightwards arrows"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let tieOverInfinityInfix = {character = "&#x29DD;"; glyph = "⧝"; name="tie over infinity"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let tildeOperatorAboveRightwardsArrowInfix = {character = "&#x2972;"; glyph = "⥲"; name="tilde operator above rightwards arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let tildeOperatorInfix = {character = "&#x223C;"; glyph = "∼"; name="tilde operator"; form="infix"; priority="250"; lspace="5"; rspace="5"; properties=[]}
let tildeOperatorWithDotAboveInfix = {character = "&#x2A6A;"; glyph = "⩪"; name="tilde operator with dot above"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let tildeOperatorWithRisingDotsInfix = {character = "&#x2A6B;"; glyph = "⩫"; name="tilde operator with rising dots"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let tildePostfix = {character = "~"; glyph = "~"; name="tilde"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["stretchy"; "accent"]}
let timesWithLeftHalfBlackInfix = {character = "&#x29D4;"; glyph = "⧔"; name="times with left half black"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let timesWithRightHalfBlackInfix = {character = "&#x29D5;"; glyph = "⧕"; name="times with right half black"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let tinyInfix = {character = "&#x29FE;"; glyph = "⧾"; name="tiny"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let topArcAnticlockwiseArrowInfix = {character = "&#x293A;"; glyph = "⤺"; name="top arc anticlockwise arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let topArcAnticlockwiseArrowWithPlusInfix = {character = "&#x293D;"; glyph = "⤽"; name="top arc anticlockwise arrow with plus"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let topArcClockwiseArrowWithMinusInfix = {character = "&#x293C;"; glyph = "⤼"; name="top arc clockwise arrow with minus"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let topCurlyBracketPostfix = {character = "&#x23DE;"; glyph = "⏞"; name="top curly bracket"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["stretchy"; "accent"]}
let topParenthesisPostfix = {character = "&#x23DC;"; glyph = "⏜"; name="top parenthesis"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["stretchy"; "accent"]}
let topSquareBracketPostfix = {character = "&#x23B4;"; glyph = "⎴"; name="top square bracket"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["stretchy"; "accent"]}
let topTortoiseShellBracketPostfix = {character = "&#x23E0;"; glyph = "⏠"; name="top tortoise shell bracket"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["stretchy"; "accent"]}
let transversalIntersectionInfix = {character = "&#x2ADB;"; glyph = "⫛"; name="transversal intersection"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let triangleWithDotAboveInfix = {character = "&#x29CA;"; glyph = "⧊"; name="triangle with dot above"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let triangleWithSerifsAtBottomInfix = {character = "&#x29CD;"; glyph = "⧍"; name="triangle with serifs at bottom"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let triangleWithUnderbarInfix = {character = "&#x29CB;"; glyph = "⧋"; name="triangle with underbar"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let tripleColonOperatorInfix = {character = "&#x2AF6;"; glyph = "⫶"; name="triple colon operator"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let tripleHorizontalBarWithDoubleVerticalStrokeInfix = {character = "&#x2A68;"; glyph = "⩨"; name="triple horizontal bar with double vertical stroke"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let tripleHorizontalBarWithTripleVerticalStrokeInfix = {character = "&#x2A69;"; glyph = "⩩"; name="triple horizontal bar with triple vertical stroke"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let tripleIntegralPrefix = {character = "&#x222D;"; glyph = "∭"; name="triple integral"; form="prefix"; priority="300"; lspace="0"; rspace="1"; properties=["largeop"; "symmetric"]}
let tripleNestedGreaterThanInfix = {character = "&#x2AF8;"; glyph = "⫸"; name="triple nested greater-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let tripleNestedLessThanInfix = {character = "&#x2AF7;"; glyph = "⫷"; name="triple nested less-than"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let triplePlusInfix = {character = "&#x29FB;"; glyph = "⧻"; name="triple plus"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let triplePrimePostfix = {character = "&#x2034;"; glyph = "‴"; name="triple prime"; form="postfix"; priority="880"; lspace="0"; rspace="0"; properties=["accent"]}
let tripleSolidusBinaryRelationInfix = {character = "&#x2AFB;"; glyph = "⫻"; name="triple solidus binary relation"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let tripleTildeInfix = {character = "&#x224B;"; glyph = "≋"; name="triple tilde"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let tripleVerticalBarBinaryRelationInfix = {character = "&#x2AF4;"; glyph = "⫴"; name="triple vertical bar binary relation"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let tripleVerticalBarDelimiterPostfix = {character = "&#x2980;"; glyph = "⦀"; name="triple vertical bar delimiter"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"]}
let tripleVerticalBarDelimiterPrefix = {character = "&#x2980;"; glyph = "⦀"; name="triple vertical bar delimiter"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"]}
let tripleVerticalBarRightTurnstileInfix = {character = "&#x22AA;"; glyph = "⊪"; name="triple vertical bar right turnstile"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let tripleVerticalBarWithHorizontalStrokeInfix = {character = "&#x2AF5;"; glyph = "⫵"; name="triple vertical bar with horizontal stroke"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let trueInfix = {character = "&#x22A8;"; glyph = "⊨"; name="TRUE"; form="infix"; priority="170"; lspace="5"; rspace="5"; properties=[]}
let turnedAngleInfix = {character = "&#x29A2;"; glyph = "⦢"; name="turned angle"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let twoConsecutiveEqualsSignsInfix = {character = "&#x2A75;"; glyph = "⩵"; name="two consecutive equals signs"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let twoIntersectingLogicalAndInfix = {character = "&#x2A55;"; glyph = "⩕"; name="two intersecting logical and"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let twoIntersectingLogicalOrInfix = {character = "&#x2A56;"; glyph = "⩖"; name="two intersecting logical or"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let twoJoinedSquaresInfix = {character = "&#x29C9;"; glyph = "⧉"; name="two joined squares"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let twoLogicalAndOperatorPrefix = {character = "&#x2A07;"; glyph = "⨇"; name="two logical and operator"; form="prefix"; priority="330"; lspace="1"; rspace="2"; properties=["largeop"; "movablelimits"; "symmetric"]}
let twoLogicalOrOperatorPrefix = {character = "&#x2A08;"; glyph = "⨈"; name="two logical or operator"; form="prefix"; priority="330"; lspace="1"; rspace="2"; properties=["largeop"; "movablelimits"; "symmetric"]}
let unionAboveBarAboveIntersectionInfix = {character = "&#x2A48;"; glyph = "⩈"; name="union above bar above intersection"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let unionAboveIntersectionInfix = {character = "&#x2A46;"; glyph = "⩆"; name="union above intersection"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let unionBesideAndJoinedWithUnionInfix = {character = "&#x2A4A;"; glyph = "⩊"; name="union beside and joined with union"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let unionInfix = {character = "&#x222A;"; glyph = "∪"; name="union"; form="infix"; priority="350"; lspace="4"; rspace="4"; properties=[]}
let unionWithLogicalOrInfix = {character = "&#x2A45;"; glyph = "⩅"; name="union with logical or"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let unionWithMinusSignInfix = {character = "&#x2A41;"; glyph = "⩁"; name="union with minus sign"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let unionWithOverbarInfix = {character = "&#x2A42;"; glyph = "⩂"; name="union with overbar"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let upArrowThroughCircleInfix = {character = "&#x29BD;"; glyph = "⦽"; name="up arrow through circle"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let upBarbLeftDownBarbLeftHarpoonInfix = {character = "&#x2951;"; glyph = "⥑"; name="up barb left down barb left harpoon"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upBarbLeftDownBarbRightHarpoonInfix = {character = "&#x294D;"; glyph = "⥍"; name="up barb left down barb right harpoon"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let upBarbRightDownBarbLeftHarpoonInfix = {character = "&#x294C;"; glyph = "⥌"; name="up barb right down barb left harpoon"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let upBarbRightDownBarbRightHarpoonInfix = {character = "&#x294F;"; glyph = "⥏"; name="up barb right down barb right harpoon"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upDownArrowInfix = {character = "&#x2195;"; glyph = "↕"; name="up down arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upDownArrowWithBaseInfix = {character = "&#x21A8;"; glyph = "↨"; name="up down arrow with base"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upDownDoubleArrowInfix = {character = "&#x21D5;"; glyph = "⇕"; name="up down double arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upDownWhiteArrowInfix = {character = "&#x21F3;"; glyph = "⇳"; name="up down white arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upFishTailInfix = {character = "&#x297E;"; glyph = "⥾"; name="up fish tail"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let upRightDiagonalEllipsisInfix = {character = "&#x22F0;"; glyph = "⋰"; name="up right diagonal ellipsis"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let upTackInfix = {character = "&#x22A5;"; glyph = "⊥"; name="up tack"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let upwardsArrowFromBarInfix = {character = "&#x21A5;"; glyph = "↥"; name="upwards arrow from bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upwardsArrowInfix = {character = "&#x2191;"; glyph = "↑"; name="upwards arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upwardsArrowLeftwardsOfDownwardsArrowInfix = {character = "&#x21C5;"; glyph = "⇅"; name="upwards arrow leftwards of downwards arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upwardsArrowToBarInfix = {character = "&#x2912;"; glyph = "⤒"; name="upwards arrow to bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upwardsArrowWithDoubleStrokeInfix = {character = "&#x21DE;"; glyph = "⇞"; name="upwards arrow with double stroke"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let upwardsArrowWithHorizontalStrokeInfix = {character = "&#x2909;"; glyph = "⤉"; name="upwards arrow with horizontal stroke"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let upwardsArrowWithTipLeftwardsInfix = {character = "&#x21B0;"; glyph = "↰"; name="upwards arrow with tip leftwards"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upwardsArrowWithTipRightwardsInfix = {character = "&#x21B1;"; glyph = "↱"; name="upwards arrow with tip rightwards"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upwardsDashedArrowInfix = {character = "&#x21E1;"; glyph = "⇡"; name="upwards dashed arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upwardsDoubleArrowInfix = {character = "&#x21D1;"; glyph = "⇑"; name="upwards double arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upwardsHarpoonWithBarbLeftBesideDownwardsHarpoonWithBarbRightInfix = {character = "&#x296E;"; glyph = "⥮"; name="upwards harpoon with barb left beside downwards harpoon with barb right"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upwardsHarpoonWithBarbLeftBesideUpwardsHarpoonWithBarbRightInfix = {character = "&#x2963;"; glyph = "⥣"; name="upwards harpoon with barb left beside upwards harpoon with barb right"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let upwardsHarpoonWithBarbLeftFromBarInfix = {character = "&#x2960;"; glyph = "⥠"; name="upwards harpoon with barb left from bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upwardsHarpoonWithBarbLeftToBarInfix = {character = "&#x2958;"; glyph = "⥘"; name="upwards harpoon with barb left to bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upwardsHarpoonWithBarbLeftwardsInfix = {character = "&#x21BF;"; glyph = "↿"; name="upwards harpoon with barb leftwards"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upwardsHarpoonWithBarbRightFromBarInfix = {character = "&#x295C;"; glyph = "⥜"; name="upwards harpoon with barb right from bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upwardsHarpoonWithBarbRightToBarInfix = {character = "&#x2954;"; glyph = "⥔"; name="upwards harpoon with barb right to bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upwardsHarpoonWithBarbRightwardsInfix = {character = "&#x21BE;"; glyph = "↾"; name="upwards harpoon with barb rightwards"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upwardsPairedArrowsInfix = {character = "&#x21C8;"; glyph = "⇈"; name="upwards paired arrows"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upwardsQuadrupleArrowInfix = {character = "&#x27F0;"; glyph = "⟰"; name="upwards quadruple arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upwardsTripleArrowInfix = {character = "&#x290A;"; glyph = "⤊"; name="upwards triple arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upwardsTwoHeadedArrowFromSmallCircleInfix = {character = "&#x2949;"; glyph = "⥉"; name="upwards two-headed arrow from small circle"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=[]}
let upwardsTwoHeadedArrowInfix = {character = "&#x219F;"; glyph = "↟"; name="upwards two headed arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"; "accent"]}
let upwardsWhiteArrowFromBarInfix = {character = "&#x21EA;"; glyph = "⇪"; name="upwards white arrow from bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upwardsWhiteArrowInfix = {character = "&#x21E7;"; glyph = "⇧"; name="upwards white arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upwardsWhiteArrowOnPedestalInfix = {character = "&#x21EB;"; glyph = "⇫"; name="upwards white arrow on pedestal"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upwardsWhiteArrowOnPedestalWithHorizontalBarInfix = {character = "&#x21EC;"; glyph = "⇬"; name="upwards white arrow on pedestal with horizontal bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upwardsWhiteArrowOnPedestalWithVerticalBarInfix = {character = "&#x21ED;"; glyph = "⇭"; name="upwards white arrow on pedestal with vertical bar"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upwardsWhiteDoubleArrowInfix = {character = "&#x21EE;"; glyph = "⇮"; name="upwards white double arrow"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let upwardsWhiteDoubleArrowOnPedestalInfix = {character = "&#x21EF;"; glyph = "⇯"; name="upwards white double arrow on pedestal"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["stretchy"]}
let vectorOrCrossProductInfix = {character = "&#x2A2F;"; glyph = "⨯"; name="vector or cross product"; form="infix"; priority="390"; lspace="4"; rspace="4"; properties=[]}
let verticalBarBesideRightTriangleInfix = {character = "&#x29D0;"; glyph = "⧐"; name="vertical bar beside right triangle"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let verticalBarBesideRightTriangleWithSlashInfix = {character = "&#x29D0;&#x338;"; glyph = "⧐̸"; name="vertical bar beside right triangle with slash"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let verticalBarDoubleLeftTurnstileInfix = {character = "&#x2AE4;"; glyph = "⫤"; name="vertical bar double left turnstile"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let verticalBarTripleRightTurnstileInfix = {character = "&#x2AE2;"; glyph = "⫢"; name="vertical bar triple right turnstile"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let verticalEllipsisInfix = {character = "&#x22EE;"; glyph = "⋮"; name="vertical ellipsis"; form="infix"; priority="150"; lspace="5"; rspace="5"; properties=[]}
let verticalLineInfix = {character = "|"; glyph = "|"; name="vertical line"; form="infix"; priority="270"; lspace="2"; rspace="2"; properties=["fence"; "stretchy"; "symmetric"]}
let verticalLinePostfix = {character = "|"; glyph = "|"; name="vertical line"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let verticalLinePrefix = {character = "|"; glyph = "|"; name="vertical line"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let verticalLineWithCircleAboveInfix = {character = "&#x2AEF;"; glyph = "⫯"; name="vertical line with circle above"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let verticalLineWithCircleBelowInfix = {character = "&#x2AF0;"; glyph = "⫰"; name="vertical line with circle below"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let verticalZigzagLineInfix = {character = "&#x299A;"; glyph = "⦚"; name="vertical zigzag line"; form="infix"; priority="270"; lspace="3"; rspace="3"; properties=[]}
let veryMuchGreaterThanInfix = {character = "&#x22D9;"; glyph = "⋙"; name="very much greater-than"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let veryMuchLessThanInfix = {character = "&#x22D8;"; glyph = "⋘"; name="very much less-than"; form="infix"; priority="260"; lspace="5"; rspace="5"; properties=[]}
let volumeIntegralPrefix = {character = "&#x2230;"; glyph = "∰"; name="volume integral"; form="prefix"; priority="310"; lspace="0"; rspace="1"; properties=["largeop"; "symmetric"]}
let waveArrowPointingDirectlyRightInfix = {character = "&#x2933;"; glyph = "⤳"; name="wave arrow pointing directly right"; form="infix"; priority="270"; lspace="5"; rspace="5"; properties=["accent"]}
let whiteBulletInfix = {character = "&#x25E6;"; glyph = "◦"; name="white bullet"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let whiteCircleWithDownArrowInfix = {character = "&#x29EC;"; glyph = "⧬"; name="white circle with down arrow"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let whiteDiamondContainingBlackSmallDiamondInfix = {character = "&#x25C8;"; glyph = "◈"; name="white diamond containing black small diamond"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let whiteDiamondInfix = {character = "&#x25C7;"; glyph = "◇"; name="white diamond"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let whiteDownPointingSmallTriangleInfix = {character = "&#x25BF;"; glyph = "▿"; name="white down-pointing small triangle"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let whiteDownPointingTriangleInfix = {character = "&#x25BD;"; glyph = "▽"; name="white down-pointing triangle"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let whiteHourglassInfix = {character = "&#x29D6;"; glyph = "⧖"; name="white hourglass"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let whiteLeftPointingPointerInfix = {character = "&#x25C5;"; glyph = "◅"; name="white left-pointing pointer"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let whiteLeftPointingSmallTriangleInfix = {character = "&#x25C3;"; glyph = "◃"; name="white left-pointing small triangle"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let whiteLeftPointingTriangleInfix = {character = "&#x25C1;"; glyph = "◁"; name="white left-pointing triangle"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let whiteParallelogramInfix = {character = "&#x25B1;"; glyph = "▱"; name="white parallelogram"; form="infix"; priority="260"; lspace="3"; rspace="3"; properties=[]}
let whiteRectangleInfix = {character = "&#x25AD;"; glyph = "▭"; name="white rectangle"; form="infix"; priority="260"; lspace="3"; rspace="3"; properties=[]}
let whiteRightPointingSmallTriangleInfix = {character = "&#x25B9;"; glyph = "▹"; name="white right-pointing small triangle"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let whiteRightPointingTriangleInfix = {character = "&#x25B7;"; glyph = "▷"; name="white right-pointing triangle"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let whiteSmallSquareInfix = {character = "&#x25AB;"; glyph = "▫"; name="white small square"; form="infix"; priority="260"; lspace="3"; rspace="3"; properties=[]}
let whiteSquareInfix = {character = "&#x25A1;"; glyph = "□"; name="white square"; form="infix"; priority="260"; lspace="3"; rspace="3"; properties=[]}
let whiteUpPointingSmallTriangleInfix = {character = "&#x25B5;"; glyph = "▵"; name="white up-pointing small triangle"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let whiteUpPointingTriangleInfix = {character = "&#x25B3;"; glyph = "△"; name="white up-pointing triangle"; form="infix"; priority="260"; lspace="4"; rspace="4"; properties=[]}
let whiteVerticalBarInfix = {character = "&#x2AFE;"; glyph = "⫾"; name="white vertical bar"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let whiteVerticalRectangleInfix = {character = "&#x25AF;"; glyph = "▯"; name="white vertical rectangle"; form="infix"; priority="260"; lspace="3"; rspace="3"; properties=[]}
let wreathProductInfix = {character = "&#x2240;"; glyph = "≀"; name="wreath product"; form="infix"; priority="340"; lspace="4"; rspace="4"; properties=[]}
let xorInfix = {character = "&#x22BB;"; glyph = "⊻"; name="xor"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let zNotationBagMembershipInfix = {character = "&#x22FF;"; glyph = "⋿"; name="z notation bag membership"; form="infix"; priority="265"; lspace="5"; rspace="5"; properties=[]}
let zNotationDomainAntirestrictionInfix = {character = "&#x2A64;"; glyph = "⩤"; name="z notation domain antirestriction"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let zNotationLeftBindingBracketPrefix = {character = "&#x2989;"; glyph = "⦉"; name="z notation left binding bracket"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let zNotationLeftImageBracketPrefix = {character = "&#x2987;"; glyph = "⦇"; name="z notation left image bracket"; form="prefix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let zNotationRangeAntirestrictionInfix = {character = "&#x2A65;"; glyph = "⩥"; name="z notation range antirestriction"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let zNotationRelationalCompositionInfix = {character = "&#x2A3E;"; glyph = "⨾"; name="z notation relational composition"; form="infix"; priority="265"; lspace="4"; rspace="4"; properties=[]}
let zNotationRightBindingBracketPostfix = {character = "&#x298A;"; glyph = "⦊"; name="z notation right binding bracket"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let zNotationRightImageBracketPostfix = {character = "&#x2988;"; glyph = "⦈"; name="z notation right image bracket"; form="postfix"; priority="20"; lspace="0"; rspace="0"; properties=["fence"; "stretchy"; "symmetric"]}
let zNotationSchemaCompositionInfix = {character = "&#x2A1F;"; glyph = "⨟"; name="z notation schema composition"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let zNotationSchemaPipingInfix = {character = "&#x2A20;"; glyph = "⨠"; name="z notation schema piping"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let zNotationSchemaProjectionInfix = {character = "&#x2A21;"; glyph = "⨡"; name="z notation schema projection"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let zNotationSpotInfix = {character = "&#x2981;"; glyph = "⦁"; name="z notation spot"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}
let zNotationTypeColonInfix = {character = "&#x2982;"; glyph = "⦂"; name="z notation type colon"; form="infix"; priority="265"; lspace="3"; rspace="3"; properties=[]}